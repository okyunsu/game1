param([Parameter(Mandatory=$true)][string]$Tag)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskRaw=Join-Path $taskRoot 'Validation/_raw/B10'
$taskOutput=Join-Path $taskRaw "$Tag.tsv"
if(Test-Path -LiteralPath $taskOutput){throw 'Preserve evidence: new tag required'}
$taskRows=@{}
foreach($taskFile in Get-ChildItem $taskRaw -Filter 'T2-resume*.txt' | Where-Object {$_.Name -notlike '*.live.txt'} | Sort-Object LastWriteTimeUtc){
 foreach($taskLine in Get-Content -LiteralPath $taskFile.FullName){
  if($taskLine -match '^PASS route ([A-Z][0-9]{2})/(\S+) replay=2 .*whole-input=(.+)$'){
   $taskRows["$($Matches[1])/$($Matches[2])"]="$($Matches[1])`t$($Matches[2])`t$($Matches[3])"
  }elseif($taskLine -match '^PASS route ([A-Z][0-9]{2})/(\S+) replay=2 stationary input=(.+)$'){
   $taskRoom=$Matches[1];$taskRoute=$Matches[2];$taskInput=$Matches[3]
   $taskBundle=Join-Path $taskRaw "inputs/$Tag-$taskRoom-$taskRoute-stationary.json"
   @{segments=@((Get-Content (Join-Path $taskRoot $taskInput) -Raw | ConvertFrom-Json))} | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $taskBundle -Encoding utf8
   $taskRows["$taskRoom/$taskRoute"]="$taskRoom`t$taskRoute`t$taskBundle"
  }
 }
}
$taskRows.Keys | Sort-Object | ForEach-Object {$taskRows[$_]} | Set-Content -LiteralPath $taskOutput -Encoding utf8
Write-Output "Registered current-design saved whole-route PASS inputs=$($taskRows.Count) manifest=$taskOutput"
