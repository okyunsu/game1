param([ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag=('R'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss')),[switch]$Quiet)
function Invoke-OriginalRegression {
param([Parameter(Mandatory=$true)][ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag,[switch]$OnlyRoute,[switch]$SkipPogo,[switch]$BasicOnly)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskValidation=Join-Path $taskRoot 'Validation'
if($Tag -eq 'V4'){
 $taskStamp='V4-'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss')
 $taskBuild=Get-ChildItem (Join-Path $taskRoot 'Builds') -Directory | Where-Object {$_.Name -match '^B10-release-r[0-9]+$' -and (Test-Path (Join-Path $_.FullName 'Afterglow.exe'))} | Sort-Object {[int]($_.Name -replace '^B10-release-r','')} -Descending | Select-Object -First 1
 if(!$taskBuild){throw 'Current V4 Release build required'}
 & (Join-Path $taskRoot 'Tools/Make-B10Manifest.ps1') -Tag $taskStamp
 & (Join-Path $taskRoot 'Tools/Run-B10Search.ps1') -Build $taskBuild.Name -Mode replay -Manifest "Validation/_raw/B10/$taskStamp.tsv" -Tag $taskStamp -Fps 60 -Fast
 exit $LASTEXITCODE
}
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskSummary=Join-Path $taskValidation "REG-$Tag-summary.txt"
if(Test-Path -LiteralPath $taskSummary){throw 'Preserve existing summary: use a new tag'}
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
if($OnlyRoute){$taskChecks=@()}
foreach($taskCheck in $taskChecks){
 $taskLog=Join-Path $taskRoot "Validation/_raw/regression/$Tag/$($taskCheck[0]).log"
 New-Item -ItemType Directory -Force (Split-Path $taskLog -Parent) | Out-Null
 $taskEvidence=Join-Path $taskValidation $taskCheck[3]
 if(Test-Path -LiteralPath $taskEvidence){throw "Preserve evidence $taskEvidence"}
 $taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$taskCheck[1],'-nightTask',$taskCheck[2],'-save-path',"Validation/_raw/regression/$Tag/$($taskCheck[0])-save.json",'-logFile',('"'+$taskLog+'"'))
 $taskProcess=Start-Process $taskUnity -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
 if(!$taskProcess.WaitForExit(240000)){Stop-Process -Id $taskProcess.Id; $taskResult='FAIL timeout'}
 elseif($taskProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskEvidence) -and (Get-Content -LiteralPath $taskEvidence -First 1).StartsWith('PASS')){$taskResult='PASS'}
 else{$taskResult='FAIL exit='+$taskProcess.ExitCode}
 $taskLine="$($taskCheck[0]) $taskResult evidence=$($taskCheck[3])"
 Add-Content -LiteralPath $taskSummary -Value $taskLine
 Write-Output $taskLine
}
if($BasicOnly){if((Get-Content -LiteralPath $taskSummary) -match ' FAIL'){exit 1};return}
# Build the current source before replay; an old exe cannot validate new tuning.
$taskRouteLibrary=Join-Path $taskRoot 'Archive/validation-2026-10/N8-route-final-library.json'
$taskPogoLibrary=Join-Path $taskRoot 'Archive/validation-2026-10/N9-pogo-library.json'
$taskPogoEnabled=(!$SkipPogo -and (Test-Path -LiteralPath $taskPogoLibrary))
if(Test-Path -LiteralPath $taskRouteLibrary){
 $taskRouteEvidence=Join-Path $taskValidation "RouteAll-60fps-$Tag.txt"
 if(Test-Path -LiteralPath $taskRouteEvidence){throw 'Preserve route evidence: use a new tag'}
 $taskRouteBuildTag="REG-route-$Tag"
 $taskRouteExe=Join-Path $taskRoot "Builds/_tmp/$taskRouteBuildTag/Afterglow-$taskRouteBuildTag.exe"
 $taskRouteBuildArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N5Build.Run','-n5-build',$taskRouteBuildTag,'-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-RouteBuild.log")+'"'))
 if($taskPogoEnabled){$taskRouteBuildArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N9Build.Run','-n9-build',$taskRouteBuildTag,'-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-RouteBuild.log")+'"'))}
 $taskRouteBuildArgs+=@('-build-root','Builds/_tmp')
 $taskRouteBuildProcess=Start-Process $taskUnity -ArgumentList $taskRouteBuildArgs -WindowStyle Hidden -PassThru
 if(!$taskRouteBuildProcess.WaitForExit(300000)){Stop-Process -Id $taskRouteBuildProcess.Id;$taskRouteResult='FAIL build timeout'}
 elseif($taskRouteBuildProcess.ExitCode -ne 0 -or !(Test-Path -LiteralPath $taskRouteExe)){$taskRouteResult='FAIL build exit='+$taskRouteBuildProcess.ExitCode}
 else{
  $taskRouteArgs=@('-verify-route','all','-verify-fps','60','-route-library',('"'+$taskRouteLibrary+'"'),'-verify-output',('"'+$taskRouteEvidence+'"'),'-save-path',"Logs/REG-$Tag-RouteAll-save.json",'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-RouteAll.log")+'"'))
  $taskRouteProcess=Start-Process $taskRouteExe -ArgumentList $taskRouteArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
  if(!$taskRouteProcess.WaitForExit(300000)){Stop-Process -Id $taskRouteProcess.Id;$taskRouteResult='FAIL timeout'}
  elseif($taskRouteProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskRouteEvidence) -and (Get-Content -LiteralPath $taskRouteEvidence -First 1).StartsWith('PASS')){$taskRouteResult='PASS'}
  else{$taskRouteResult='FAIL exit='+$taskRouteProcess.ExitCode}
 }
 $taskRouteLine="RouteAll-60fps $taskRouteResult evidence=$(Split-Path $taskRouteEvidence -Leaf)"
 Add-Content -LiteralPath $taskSummary -Value $taskRouteLine
 Write-Output $taskRouteLine
}elseif($OnlyRoute){throw 'Route library is required for OnlyRoute'}

if($taskPogoEnabled){
 $taskPogoEvidence=Join-Path $taskValidation "PogoTest-60fps-$Tag.txt"
 if(Test-Path -LiteralPath $taskPogoEvidence){throw 'Preserve pogo evidence: use a new tag'}
 if(!(Test-Path -LiteralPath $taskRouteExe)){$taskPogoResult='FAIL current build unavailable'}
 else{
  $taskPogoArgs=@('-pogotest','-verify-pogotest','all','-verify-fps','60','-pogo-library',('"'+$taskPogoLibrary+'"'),'-verify-output',('"'+$taskPogoEvidence+'"'),'-save-path',"Logs/REG-$Tag-PogoTest-save.json",'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-PogoTest.log")+'"'))
  $taskPogoProcess=Start-Process $taskRouteExe -ArgumentList $taskPogoArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
  if(!$taskPogoProcess.WaitForExit(180000)){Stop-Process -Id $taskPogoProcess.Id;$taskPogoResult='FAIL timeout'}
  elseif($taskPogoProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskPogoEvidence) -and (Get-Content -LiteralPath $taskPogoEvidence -First 1).StartsWith('PASS')){$taskPogoResult='PASS'}
  else{$taskPogoResult='FAIL exit='+$taskPogoProcess.ExitCode}
 }
 $taskPogoLine="PogoTest-60fps $taskPogoResult evidence=$(Split-Path $taskPogoEvidence -Leaf)"
 Add-Content -LiteralPath $taskSummary -Value $taskPogoLine
 Write-Output $taskPogoLine
}



if((Get-Content -LiteralPath $taskSummary) -match ' FAIL'){exit 1}


}
# Single entry for the basic 13 regression; original checks and verdicts preserved.
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskValidation=Join-Path $taskRoot 'Validation'
$taskNames=@('S1-02','S1-04','S1-07','S1-08','Camera','T1','T2','T3','T4','S2','F2R','LOGR','Boss')
$taskLines=@()
try{
 if(!$Quiet){$taskLines=@(Invoke-OriginalRegression -Tag $Tag -BasicOnly)}
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



