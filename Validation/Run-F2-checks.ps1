$ErrorActionPreference = 'Stop'
$taskUnity = 'C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskChecks = @(
 @('NightTests.Run','T2-F2-rerun'),
 @('Nightly.Input','F2'),
 @('Nightly.Buffer','F2'),
 @('N2Verification.RoomsNight','F2'),
 @('N2BVerification.CheckpointsNight','F2'),
 @('NightTests.Run','T1-F2'),
 @('NightTests.Run','T3-F2'),
 @('F2Verification.E1','F2'),
 @('F2Verification.Integration','F2'),
 @('F2Verification.Boss','F2'),
 @('N2Verification.CameraLead0','S2-F2')
)
foreach ($taskCheck in $taskChecks) {
 $taskMethod=$taskCheck[0]; $taskTag=$taskCheck[1]
 $taskArguments = @('-batchmode','-projectPath','.', '-executeMethod',$taskMethod,'-nightTask',$taskTag,'-save-path','Logs/F2-isolated-progress.json','-logFile',('Logs/F2-'+$taskMethod+'-'+$taskTag+'.log'))
 $taskProcess=Start-Process -FilePath $taskUnity -ArgumentList $taskArguments -WindowStyle Hidden -PassThru
 $taskProcess.WaitForExit()
 Write-Output ($taskMethod+' '+$taskTag+' exit='+$taskProcess.ExitCode)
 if($taskProcess.ExitCode -ne 0){throw ('Check failed: '+$taskMethod+' '+$taskTag)}
}
