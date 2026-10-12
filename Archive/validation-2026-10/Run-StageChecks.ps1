param([string]$TaskStage)
$ErrorActionPreference='Stop'
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskMethods=@('Nightly.Input','Nightly.Buffer','N2Verification.RoomsNight','N2BVerification.CheckpointsNight')
if($TaskStage.StartsWith('S2-')){$taskMethods+=@('N2Verification.CameraLead0')}
foreach($taskMethod in $taskMethods){$taskProcess=Start-Process $taskUnity -ArgumentList @('-batchmode','-projectPath','.', '-executeMethod',$taskMethod,'-nightTask',$TaskStage,'-save-path',('Logs/'+$TaskStage+'-save.json'),'-logFile',('Logs/'+$TaskStage+'-'+$taskMethod+'.log')) -WindowStyle Hidden -PassThru;$taskProcess.WaitForExit();Write-Output ($taskMethod+' exit='+$taskProcess.ExitCode);if($taskProcess.ExitCode -ne 0){throw ('Stage regression failure '+$taskMethod)}}
