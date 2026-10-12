$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q6 START"
foreach($pair in @(@('Q6NightBuild-correction-source.txt','Assets/Editor/Q6NightBuild.cs'),@('Q6CandidateLabel-correction-source.txt','Assets/Scripts/Q6CandidateLabel.cs'))){$target=Join-Path $taskRoot $pair[1];Copy-Item -LiteralPath (Join-Path $PSScriptRoot $pair[0]) -Destination $target -Force}
$lines=[System.Collections.Generic.List[string]]::new()
foreach($candidate in @('A','B','C')){
 $argsList=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','Q6NightBuild.Run','-candidate',$candidate,'-logFile',('"'+(Join-Path $taskRoot "Logs/Q6-$candidate-build-correction.log")+'"'))
 $p=Start-Process $taskUnity -ArgumentList $argsList -WindowStyle Hidden -PassThru
 if(!$p.WaitForExit(360000)){Stop-Process -Id $p.Id;throw "Q6-$candidate build timeout"};if($p.ExitCode -ne 0){throw "Q6-$candidate build failed exit=$($p.ExitCode)"}
 Write-Output "Q6-$candidate build complete"
 $exe=Join-Path $taskRoot "Builds/Q6-$candidate/Afterglow-Q6-$candidate.exe";$output=Join-Path $PSScriptRoot "Q6-$candidate-runtime-correction.txt"
 $argsList=@('-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-save-path',('"'+(Join-Path $taskRoot "Logs/Q6-$candidate-save-correction.json")+'"'),'-pre-verify',('"'+$output+'"'),'-logFile',('"'+(Join-Path $taskRoot "Logs/Q6-$candidate-runtime-correction.log")+'"'))
 $p=Start-Process $exe -ArgumentList $argsList -WindowStyle Hidden -PassThru
 if(!$p.WaitForExit(90000)){Stop-Process -Id $p.Id;throw "Q6-$candidate runtime timeout"};if($p.ExitCode -ne 0){throw "Q6-$candidate runtime failed exit=$($p.ExitCode)"}
 $lines.Add("PASS Q6-$candidate build + actual exe attack activation + enemy hit + Error0; see Q6-$candidate-build-correction.txt / Q6-$candidate-runtime-correction.txt")
 $lines.AddRange([string[]](Get-Content (Join-Path $taskRoot "Logs/Q6-$candidate-runtime-correction.log") | Where-Object {$_ -like 'Q6 candidate*'}))
  $expected=@{A='Feedback-A scene=DontDestroyOnLoad slash=0.7 flash=0.1 pip=0.18 pop=1.6';B='Feedback-B scene=DontDestroyOnLoad slash=0.9 flash=0.14 pip=0.24 pop=2';C='Feedback-C scene=DontDestroyOnLoad slash=0.5 flash=0.07 pip=0.14 pop=1.3'}
 $runtimeText=[IO.File]::ReadAllText((Join-Path $taskRoot "Logs/Q6-$candidate-runtime-correction.log"))
 if(!$runtimeText.Contains($expected[$candidate])){throw "Candidate $candidate actual tuning binding failed after single correction"}
 Write-Output "Q6-$candidate actual exe and tuning binding PASS"
}
& (Join-Path $PSScriptRoot 'Run-Regression.ps1') -Tag Q6-night4-correction
$summary=Get-Content (Join-Path $PSScriptRoot 'REG-Q6-night4-correction-summary.txt');if(@($summary | Where-Object {$_ -match ' PASS '}).Count -ne 13){throw 'Q6 regression failed; restore Q6 new files before proceeding'}
$lines.Insert(0,'PASS Q6 feedback candidates; presentation acceptance pending user.')
$lines.Add('A | slashAlpha .7 | hitFlash .10s | hpPip .18u | deathPopScale 1.6')
$lines.Add('B | slashAlpha .9 | hitFlash .14s | hpPip .24u | deathPopScale 2.0')
$lines.Add('C | slashAlpha .5 | hitFlash .07s | hpPip .14u | deathPopScale 1.3')
$lines.Add('Default FeedbackTuning untouched. Candidate copies Assets/Resources/Q6; build-scene copies remap presentation references. Q6CandidateLabel exists only in extra build defines Q6_A/B/C; displays title bottom-left and rebinds presentation tuning for enemies spawned from unchanged prefabs. No gameplay/default source-scene edits.')
$lines.Add('Compare each build for two minutes: title candidate name, A01 attack stroke, A04 enemy flash/HP/death pop. Do not change defaults until user selects a candidate. Play-mode experimentation requires re-entering/saving desired values after Stop; candidate asset changes require candidate rebuild.')
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q6-feedback-candidates-correction.txt'),$lines)
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q6 END PASS user acceptance pending"
git add -A Assets; git add Assets/Editor/Q6NightBuild.cs Assets/Editor/Q6NightBuild.cs.meta Assets/Scripts/Q6CandidateLabel.cs Assets/Scripts/Q6CandidateLabel.cs.meta Assets/Resources/Q6 Assets/Resources/Q6.meta Validation
git commit -m 'build: create isolated night4 feedback candidates and verify executables'
if($LASTEXITCODE -ne 0){throw 'Q6 commit failed'}
