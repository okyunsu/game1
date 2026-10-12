param([Parameter(Mandatory=$true)][string]$Base,[string[]]$Allowed=@(),[string]$Head='HEAD')
# Mechanical batch card: values come only from git and files. No judgement text.
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
Push-Location $taskRoot
try{
$Allowed=@($Allowed | ForEach-Object {$_ -split ','} | ForEach-Object {$_.Trim()} | Where-Object {$_})
$taskNum=@(git -c core.quotepath=off diff --numstat "$Base..$Head")
if($LASTEXITCODE -ne 0){throw "git diff failed for $Base..$Head"}
$taskFiles=@();$taskAdd=0;$taskDel=0;$taskBinary=0
foreach($taskRow in $taskNum){
 $taskCols=$taskRow -split "`t",3
 $taskPath=$taskCols[2]
 if($taskPath -match '^(.*)\{(.*) => (.*)\}(.*)$'){$taskPath=($Matches[1]+$Matches[3]+$Matches[4]) -replace '//','/'}
 elseif($taskPath -match ' => '){$taskPath=($taskPath -split ' => ')[1]}
 if($taskCols[0] -eq '-'){$taskBinary++}else{$taskAdd+=[int]$taskCols[0];$taskDel+=[int]$taskCols[1]}
 $taskFiles+=$taskPath
}
function Get-Folder($p){
 if($p -match '(^|/)Editor/'){return 'Editor'}
 if($p -like 'Assets/Scripts/*'){return 'Scripts'}
 if($p -like 'Assets/Scenes/*'){return 'Scenes'}
 if($p -like 'Assets/Prefabs/*'){return 'Prefabs'}
 if($p -like 'Validation/*'){return 'Validation'}
 if($p -like '*.md'){return '문서'}
 return '기타'
}
$taskOutside=@($taskFiles | Where-Object {$f=$_; -not ($Allowed | Where-Object {$f -like $_ -or $f -like ($_.TrimEnd('/')+'/*')})})
"BATCH CARD $Base..$Head ($(git rev-parse --short $Head))"
"변경 파일: $($taskFiles.Count) (+$taskAdd / -$taskDel 줄, 바이너리 $taskBinary)"
foreach($taskFolder in 'Scripts','Scenes','Prefabs','Editor','Validation','문서','기타'){"  $($taskFolder): $(@($taskFiles | Where-Object {(Get-Folder $_) -eq $taskFolder}).Count)"}
"허용 범위: $(if($Allowed.Count){$Allowed -join ', '}else{'(없음)'})"
"허용 범위 밖: $($taskOutside.Count)"
$taskOutside | ForEach-Object {"  $_"}

$taskTasks=Get-Content -Encoding utf8 (Join-Path $taskRoot 'TASKS.md')
# Status = first word of the second cell of TASKS.md table rows.
$taskStatus=@{}
foreach($taskLine in $taskTasks){
 if($taskLine -match '^\|[^|]+\|\s*(DONE|REVIEW|DOING|BLOCKED|PARTIAL)\b'){$taskStatus[$Matches[1]]=1+[int]$taskStatus[$Matches[1]]}
}
"TASKS 상태(표 둘째 칸): " + (('DONE','REVIEW','DOING','BLOCKED','PARTIAL' | ForEach-Object {"$_ $([int]$taskStatus[$_])"}) -join ' / ')
function Get-Section($start,$level){
 $on=$false;$out=@()
 foreach($l in $taskTasks){
  if($on -and $l -match "^#{1,$level} "){break}
  if($on){$out+=$l}
  if($l -eq $start){$on=$true}
 }
 return $out
}
$taskBugs=@(Get-Section '## Bugs' 2 | Where-Object {$_ -match '^### ' -and $_ -match '\bOPEN\b'})
"열린 버그(## Bugs의 ### 제목 중 OPEN): $($taskBugs.Count)"
$taskPlay=@(Get-Section '### 2. 직접 플레이 확인' 3 | Where-Object {$_ -match '^\d+\. '})
"직접 플레이 확인 대기(번호 항목): $($taskPlay.Count)"
$taskReg=Get-ChildItem (Join-Path $taskRoot 'Validation') -Recurse -File -Include *.txt | Sort-Object LastWriteTime -Descending | ForEach-Object {
 $m=Select-String -LiteralPath $_.FullName -Pattern '^REGRESSION \d+/\d+' | Select-Object -Last 1
 if($m){[pscustomobject]@{Line=$m.Line;File=$_.FullName.Substring($taskRoot.Length+1) -replace '\\','/'}}
} | Select-Object -First 1
"마지막 REGRESSION: $(if($taskReg){"$($taskReg.Line) ($($taskReg.File))"}else{'(없음)'})"
}finally{Pop-Location}
