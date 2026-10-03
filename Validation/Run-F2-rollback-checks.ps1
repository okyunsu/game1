$ErrorActionPreference = 'Stop'
$taskUnity = 'C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
foreach ($taskMethod in @('Nightly.Input','Nightly.Buffer','N2Verification.RoomsNight','N2BVerification.CheckpointsNight','N2Verification.CameraLead0')) {
 $taskArguments=@('-batchmode','-projectPath','.', '-executeMethod',$taskMethod,'-nightTask','S2-F2-rollback','-save-path','Logs/F2-rollback-isolated-progress.json','-logFile',('Logs/F2-rollback-'+$taskMethod+'.log'))
 $taskProcess=Start-Process -FilePath $taskUnity -ArgumentList $taskArguments -WindowStyle Hidden -PassThru
 $taskProcess.WaitForExit()
 Write-Output ($taskMethod+' exit='+$taskProcess.ExitCode)
 if($taskProcess.ExitCode -ne 0){throw ('Rollback check failed: '+$taskMethod)}
}
