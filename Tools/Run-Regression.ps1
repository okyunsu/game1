param([ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag=('R'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss')),[switch]$Quiet)
# Single entry for the basic 13 regression. Wraps Validation/Run-Regression.ps1 -BasicOnly without changing its checks.
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskValidation=Join-Path $taskRoot 'Validation'
$taskNames=@('S1-02','S1-04','S1-07','S1-08','Camera','T1','T2','T3','T4','S2','F2R','LOGR','Boss')
$taskLines=@()
try{
 if(!$Quiet){$taskLines=@(& (Join-Path $taskValidation 'Run-Regression.ps1') -Tag $Tag -BasicOnly)}
 else{
  # Same 13 methods and evidence criteria. Camera writes rendered PNGs and keeps graphics.
  $taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
  $taskChecks=@(
   @('S1-02','Nightly.Input',"S2-$Tag","S1-02-night-S2-$Tag.txt"),
   @('S1-04','Nightly.Buffer',"S2-$Tag","S1-04-night-S2-$Tag.txt"),
   @('S1-07','N2Verification.RoomsNight',"S2-$Tag","S1-07-night-S2-$Tag.txt"),
   @('S1-08','N2BVerification.CheckpointsNight',"S2-$Tag","S1-08-night-S2-$Tag.txt"),
   @('Camera','N2Verification.CameraLead0',"S2-$Tag","S1-06-lead0-night-S2-$Tag.txt"),
   @('T1','NightTests.Run',"T1-$Tag","Verification-night-T1-$Tag.txt"),
   @('T2','NightTests.Run',"T2-$Tag","Verification-night-T2-$Tag.txt"),
   @('T3','NightTests.Run',"T3-$Tag","Verification-night-T3-$Tag.txt"),
   @('T4','C4Verification.Run',"S2-$Tag","Verification-T4-night-S2-$Tag.txt"),
   @('S2','S2Tests.Run',"S2-$Tag","S2-01-integration-night-S2-$Tag.txt"),
   @('F2R','F2RVerification.Run',"S2-$Tag","F2R-feedback-night-S2-$Tag.txt"),
   @('LOGR','LOGRCheck.Run',"S2-$Tag","LOGR-playtest-night-S2-$Tag.txt"),
   @('Boss','BossRecoveryCheck.Run',"S2-$Tag","boss-recovery-night-S2-$Tag.txt"))
  foreach($taskCheck in $taskChecks){
   $taskEvidence=Join-Path $taskValidation $taskCheck[3]
   if(Test-Path -LiteralPath $taskEvidence){throw "Preserve evidence $taskEvidence"}
   $taskLog=Join-Path $taskValidation "_raw/regression/$Tag/$($taskCheck[0]).log"
   New-Item -ItemType Directory -Force (Split-Path $taskLog -Parent) | Out-Null
   $taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$taskCheck[1],'-nightTask',$taskCheck[2],'-save-path',('"'+(Join-Path $taskValidation "_raw/regression/$Tag/$($taskCheck[0])-save.json")+'"'),'-logFile',('"'+$taskLog+'"'),'-screen-width','640','-screen-height','360','-screen-fullscreen','0')
   $taskHeadless=$taskCheck[0] -ne 'Camera'
   if($taskHeadless){$taskArgs+='-nographics'}
   $taskProcess=Start-Process $taskUnity -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
   if(!$taskProcess.WaitForExit(240000)){Stop-Process -Id $taskProcess.Id;$taskResult='FAIL timeout'}
   elseif($taskProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskEvidence) -and (Get-Content -LiteralPath $taskEvidence -First 1).StartsWith('PASS')){$taskResult='PASS'}
   else{$taskResult='FAIL exit='+$taskProcess.ExitCode}
   $taskLines+="$($taskCheck[0]) $taskResult evidence=$($taskCheck[3]) mode=$(if($taskHeadless){'headless'}else{'graphics-640x360'})"
  }
 }
}catch{$taskLines+="ERROR $($_.Exception.Message)"}
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
