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
$taskDecisions=@(Get-Section '## 2. 결정 대기' 2 | Where-Object {$_ -match '^\| D-'})
"결정 대기(## 2. 결정 대기 표의 D- 행): $($taskDecisions.Count)"
$taskBugs=@(Get-Section '## 4. 열린 버그' 2 | Where-Object {$_ -match '^\| ' -and $_ -notmatch '^\| (ID|---)'})
"열린 버그(## 4. 열린 버그 표 행): $($taskBugs.Count)"
$taskPlay=@(Get-Section '## 3. 직접 플레이 확인' 2 | Where-Object {$_ -match '^- '})
"직접 플레이 확인 대기(## 3. 목록 항목): $($taskPlay.Count)"
$taskText=$taskTasks -join "`n"
$taskBroken=@([regex]::Matches($taskText,'\]\(([^)]+)\)') | ForEach-Object {$_.Groups[1].Value} | Where-Object {$_ -notmatch '^(https?:|#)'} | ForEach-Object {[uri]::UnescapeDataString(($_ -split '#')[0])} | Where-Object {$_ -and !(Test-Path -LiteralPath (Join-Path $taskRoot $_))})
"TASKS 깨진 링크 수: $($taskBroken.Count)"
$taskBroken | ForEach-Object {"  $_"}
$taskStale=@([regex]::Matches($taskText,'(?:Launchers|Builds)/[A-Za-z0-9_./-]+') | ForEach-Object {$_.Value.TrimEnd('.')} | Sort-Object -Unique | Where-Object {!(Test-Path -LiteralPath (Join-Path $taskRoot $_))})
"TASKS가 가리키지만 없는 빌드·런처 수: $($taskStale.Count)"
$taskStale | ForEach-Object {"  $_"}
$taskReg=Get-ChildItem (Join-Path $taskRoot 'Validation') -Recurse -File -Include *.txt | Sort-Object LastWriteTime -Descending | ForEach-Object {
 $m=Select-String -LiteralPath $_.FullName -Pattern '^REGRESSION \d+/\d+' | Select-Object -Last 1
 if($m){[pscustomobject]@{Line=$m.Line;File=$_.FullName.Substring($taskRoot.Length+1) -replace '\\','/'}}
} | Select-Object -First 1
"마지막 REGRESSION: $(if($taskReg){"$($taskReg.Line) ($($taskReg.File))"}else{'(없음)'})"
}finally{Pop-Location}
