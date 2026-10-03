param([string]$Tag,[string]$Method='S3Verification.Run',[switch]$Regress)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskEditor='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskMethods= @($Method)
if($Regress){$taskMethods+=@('Nightly.Input','Nightly.Buffer','N2Verification.RoomsNight','N2BVerification.CheckpointsNight')}
foreach($taskMethod in $taskMethods){
 $taskLabel=$taskMethod.Replace('.','-')
 $taskLog=Join-Path $taskRoot "Validation/$Tag-$taskLabel.log"
 if(Test-Path -LiteralPath $taskLog){throw 'Preserve evidence'}
 $taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$taskMethod,'-nightTask',$Tag,'-save-path',('"'+$taskRoot+'\Logs\'+$Tag+'-slot.json"'),'-logFile',('"'+$taskLog+'"'))
 if($taskMethod.StartsWith('S3Authoring') -or $taskMethod -eq 'S3Build.Run'){$taskArgs+='-quit'}
 $taskProcess=Start-Process -FilePath $taskEditor -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
 $taskProcess.WaitForExit()
 Add-Content -LiteralPath (Join-Path $taskRoot "Validation/$Tag-process.txt") -Value "$taskMethod exit=$($taskProcess.ExitCode)"
 if($taskProcess.ExitCode -ne 0){exit 1}
}