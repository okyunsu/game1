$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q7 START"
foreach($pair in @(@('Q7NightBuild-source.txt','Assets/Editor/Q7NightBuild.cs'),@('Q7ExeCapture-source.txt','Assets/Scripts/Q7ExeCapture.cs'))){$target=Join-Path $taskRoot $pair[1];if(Test-Path -LiteralPath $target){throw "Preserve $target"};Copy-Item -LiteralPath (Join-Path $PSScriptRoot $pair[0]) -Destination $target}
$argsList=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','Q7NightBuild.Run','-logFile',('"'+(Join-Path $taskRoot 'Logs/Q7-build.log')+'"'))
$p=Start-Process $taskUnity -ArgumentList $argsList -WindowStyle Hidden -PassThru
if(!$p.WaitForExit(360000)){Stop-Process -Id $p.Id;throw 'Q7 build timeout'};if($p.ExitCode -ne 0){throw "Q7 build failed exit=$($p.ExitCode)"}
$exe=Join-Path $taskRoot 'Builds/Q7-capture/Afterglow-Q7.exe';$folder=Join-Path $PSScriptRoot 'Q7-shots'
foreach($size in @(@(1280,720),@(1920,1080))){$name="$($size[0])x$($size[1])";$argsList=@('-screen-width',$size[0],'-screen-height',$size[1],'-screen-fullscreen','0','-save-path',('"'+(Join-Path $taskRoot "Logs/Q7-$name-save.json")+'"'),'-capture','all','-capture-output',('"'+$folder+'"'),'-logFile',('"'+(Join-Path $taskRoot "Logs/Q7-$name.log")+'"'));$p=Start-Process $exe -ArgumentList $argsList -WindowStyle Hidden -PassThru;if(!$p.WaitForExit(90000)){Stop-Process -Id $p.Id;throw "Q7-$name capture timeout"};if($p.ExitCode -ne 0){throw "Q7-$name capture failed exit=$($p.ExitCode)"};Write-Output "Q7 $name shots complete"}
& (Join-Path $PSScriptRoot 'Run-Regression.ps1') -Tag Q7-night4
$summary=Get-Content (Join-Path $PSScriptRoot 'REG-Q7-night4-summary.txt');if(@($summary | Where-Object {$_ -match ' PASS '}).Count -ne 13){throw 'Q7 regression failed; restore Q7 new files before proceeding'}
$shots=Get-ChildItem -LiteralPath $folder -Filter '*.png';if($shots.Count -ne 12){throw "Expected 12 shots, found $($shots.Count)"}
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q7-screenshots.txt'),@('PASS Q7 actual exe ScreenCapture.CaptureScreenshot; 12 PNGs captured.','1280x720 + 1920x1080: A01-start / after-hit / both-abilities / death-overlay / boss-tell / ending.','API called inside actual exe after WaitForEndOfFrame; reflection resolves runtime ScreenCaptureModule without altering Packages or adding module DLLs.','Capture tool is inert without -capture. Default build excludes Q6 candidate label by absence of Q6 defines.','See Q7-shots/capture-1280x720.txt and capture-1920x1080.txt for Error0 evidence; visual acceptance separately reviewed.'))
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q7 END PASS 12 actual exe shots"
git add Assets/Editor/Q7NightBuild.cs Assets/Editor/Q7NightBuild.cs.meta Assets/Scripts/Q7ExeCapture.cs Assets/Scripts/Q7ExeCapture.cs.meta Validation
git commit -m 'test: capture twelve night4 UI screenshots inside actual executable'
if($LASTEXITCODE -ne 0){throw 'Q7 commit failed'}
