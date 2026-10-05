param([Parameter(Mandatory=$true)][ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag,[switch]$OnlyRoute)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskSummary=Join-Path $PSScriptRoot "REG-$Tag-summary.txt"
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
 $taskLog=Join-Path $taskRoot "Logs/REG-$Tag-$($taskCheck[0]).log"
 $taskEvidence=Join-Path $PSScriptRoot $taskCheck[3]
 if(Test-Path -LiteralPath $taskEvidence){throw "Preserve evidence $taskEvidence"}
 $taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$taskCheck[1],'-nightTask',$taskCheck[2],'-save-path',"Logs/REG-$Tag-$($taskCheck[0])-save.json",'-logFile',('"'+$taskLog+'"'))
 $taskProcess=Start-Process $taskUnity -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
 if(!$taskProcess.WaitForExit(240000)){Stop-Process -Id $taskProcess.Id; $taskResult='FAIL timeout'}
 elseif($taskProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskEvidence) -and (Get-Content -LiteralPath $taskEvidence -First 1).StartsWith('PASS')){$taskResult='PASS'}
 else{$taskResult='FAIL exit='+$taskProcess.ExitCode}
 $taskLine="$($taskCheck[0]) $taskResult evidence=$($taskCheck[3])"
 Add-Content -LiteralPath $taskSummary -Value $taskLine
 Write-Output $taskLine
}
# Build the current source before replay; an old exe cannot validate new tuning.
$taskRouteLibrary=Join-Path $PSScriptRoot 'N8-robust-library.json'
if(Test-Path -LiteralPath $taskRouteLibrary){
 $taskRouteEvidence=Join-Path $PSScriptRoot "RouteAll-60fps-$Tag.txt"
 if(Test-Path -LiteralPath $taskRouteEvidence){throw 'Preserve route evidence: use a new tag'}
 $taskRouteBuildTag="REG-route-$Tag"
 $taskRouteExe=Join-Path $taskRoot "Builds/$taskRouteBuildTag/Afterglow-$taskRouteBuildTag.exe"
 $taskRouteBuildArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N5Build.Run','-n5-build',$taskRouteBuildTag,'-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-RouteBuild.log")+'"'))
 $taskRouteBuildProcess=Start-Process $taskUnity -ArgumentList $taskRouteBuildArgs -WindowStyle Hidden -PassThru
 if(!$taskRouteBuildProcess.WaitForExit(300000)){Stop-Process -Id $taskRouteBuildProcess.Id;$taskRouteResult='FAIL build timeout'}
 elseif($taskRouteBuildProcess.ExitCode -ne 0 -or !(Test-Path -LiteralPath $taskRouteExe)){$taskRouteResult='FAIL build exit='+$taskRouteBuildProcess.ExitCode}
 else{
  $taskRouteArgs=@('-verify-route','all','-verify-fps','60','-route-library',('"'+$taskRouteLibrary+'"'),'-verify-output',('"'+$taskRouteEvidence+'"'),'-save-path',"Logs/REG-$Tag-RouteAll-save.json",'-logFile',('"'+(Join-Path $taskRoot "Logs/REG-$Tag-RouteAll.log")+'"'))
  $taskRouteProcess=Start-Process $taskRouteExe -ArgumentList $taskRouteArgs -WorkingDirectory $taskRoot -WindowStyle Normal -PassThru
  if(!$taskRouteProcess.WaitForExit(300000)){Stop-Process -Id $taskRouteProcess.Id;$taskRouteResult='FAIL timeout'}
  elseif($taskRouteProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskRouteEvidence) -and (Get-Content -LiteralPath $taskRouteEvidence -First 1).StartsWith('PASS')){$taskRouteResult='PASS'}
  else{$taskRouteResult='FAIL exit='+$taskRouteProcess.ExitCode}
 }
 $taskRouteLine="RouteAll-60fps $taskRouteResult evidence=$(Split-Path $taskRouteEvidence -Leaf)"
 Add-Content -LiteralPath $taskSummary -Value $taskRouteLine
 Write-Output $taskRouteLine
}elseif($OnlyRoute){throw 'Route library is required for OnlyRoute'}

