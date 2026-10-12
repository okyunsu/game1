param([ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag=('R'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss')))
# Single entry for the basic 13 regression. Wraps Validation/Run-Regression.ps1 -BasicOnly without changing its checks.
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskValidation=Join-Path $taskRoot 'Validation'
$taskNames=@('S1-02','S1-04','S1-07','S1-08','Camera','T1','T2','T3','T4','S2','F2R','LOGR','Boss')
$taskLines=@()
try{$taskLines=@(& (Join-Path $taskValidation 'Run-Regression.ps1') -Tag $Tag -BasicOnly)}catch{$taskLines+="ERROR $($_.Exception.Message)"}
$taskPass=0
$taskOut=@()
foreach($taskName in $taskNames){
 $taskLine=$taskLines | Where-Object {$_ -like "$taskName *"} | Select-Object -First 1
 $taskResult=if($taskLine -and $taskLine -match "^$([regex]::Escape($taskName)) PASS"){'PASS'}else{'FAIL'}
 if($taskResult -eq 'PASS'){$taskPass++}
 $taskOut+="$taskName $taskResult"+$(if($taskLine){" | $taskLine"}else{' | no result line'})
}
$taskOut+=$taskLines | Where-Object {$_ -like 'ERROR *'}
$taskOut+="REGRESSION $taskPass/$($taskNames.Count)"
# Raw evidence for this tag goes to Validation/_raw (not committed).
$taskRaw=Join-Path $taskValidation "_raw/regression/$Tag"
New-Item -ItemType Directory -Force $taskRaw | Out-Null
Get-ChildItem $taskValidation -File | Where-Object {$_.Name -like "*-$Tag.txt" -or $_.Name -eq "REG-$Tag-summary.txt"} | Move-Item -Destination $taskRaw
$taskOut | Set-Content -Encoding utf8 (Join-Path $taskRaw 'result.txt')
$taskOut
if($taskPass -ne $taskNames.Count){exit 1}
