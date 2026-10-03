$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
foreach($taskPhase in @('route','continue')){
 $taskOutput=Join-Path $taskRoot "Validation/S3-B-runtime-$taskPhase.txt"
 $taskLog=Join-Path $taskRoot "Validation/S3-B-Player-$taskPhase.log"
 if((Test-Path $taskOutput) -or (Test-Path $taskLog)){throw 'Preserve evidence'}
 $taskArgs=@('-screen-fullscreen','0','-screen-width','1280','-screen-height','720','-s3-verify',$taskPhase,('"'+$taskOutput+'"'),'-save-path',('"'+$taskRoot+'\Logs\S3-B-runtime-slot.json"'),'-logFile',('"'+$taskLog+'"'))
 $taskProcess=Start-Process -FilePath (Join-Path $taskRoot 'Builds/S3-B/Afterglow-S3.exe') -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
 $taskProcess.WaitForExit()
 Add-Content -LiteralPath (Join-Path $taskRoot 'Validation/S3-B-runtime-process.txt') -Value "$taskPhase process=$($taskProcess.Id) exit=$($taskProcess.ExitCode)"
 if($taskProcess.ExitCode -ne 0){exit 1}
}
[IO.File]::WriteAllText((Join-Path $taskRoot 'Validation/S3-B-runtime.txt'),"PASS Windows x64 exe: route and Continue in two separate processes`nNew game; all doors both ways; CP-A01/A03/B01 death return; Dash and Double Jump acquisition; corrected B04 High landing; pickup reentry suppression; disk save and process restart/Continue`nError/Exception/Assert 0 in each phase. Evidence S3-B-runtime-route.txt, S3-B-runtime-continue.txt, S3-B-Player-route.log, S3-B-Player-continue.log`nDirect placement and prescribed input; no navigation robot. Isolated Logs/S3-B-runtime-slot.json; user save untouched.`n")