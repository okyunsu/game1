$ErrorActionPreference='Stop'
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
foreach($taskNumber in 1..4){
 $taskTag=if($taskNumber -eq 4){'S2-D0'}else{'T'+$taskNumber+'-D0'}
 $taskMethod=if($taskNumber -eq 4){'C4Verification.Run'}else{'NightTests.Run'}
 $taskProcess=Start-Process $taskUnity -ArgumentList @('-batchmode','-projectPath','.', '-executeMethod',$taskMethod,'-nightTask',$taskTag,'-save-path','Logs/D0-save.json','-logFile',('Logs/D0-T'+$taskNumber+'.log')) -WindowStyle Hidden -PassThru
 $taskProcess.WaitForExit()
 $taskSource=if($taskNumber -eq 4){'Validation/Verification-T4-night-S2-D0.txt'}else{'Validation/Verification-night-'+$taskTag+'.txt'}
 Copy-Item -LiteralPath $taskSource -Destination ('Validation/D0-baseline-T'+$taskNumber+'.txt')
 Write-Output ('D0 T'+$taskNumber+' exit='+$taskProcess.ExitCode)
}
