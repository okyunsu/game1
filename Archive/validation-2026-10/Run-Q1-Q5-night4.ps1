$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskProgress=Join-Path $PSScriptRoot 'Q-night4-progress.txt'
function Record($q,$state){Add-Content $taskProgress "$((Get-Date).ToUniversalTime().ToString('o')) $q $state"}
function Regression($q){& (Join-Path $PSScriptRoot 'Run-Regression.ps1') -Tag "$q-night4"; $summary=Get-Content (Join-Path $PSScriptRoot "REG-$q-night4-summary.txt");if(@($summary | Where-Object {$_ -match ' PASS '}).Count -ne 13){throw "$q regression failed; no gameplay changes to restore"}}
function Commit($q){git add Validation TASKS.md;git commit -m "test: record night4 $q evidence and limits";if($LASTEXITCODE -ne 0){throw 'Commit failed'}}
Regression 'Q1';Commit 'Q1'
Record Q2 START
$taskExe=Join-Path $taskRoot 'Builds/S4-pre2/Afterglow-S4-pre2.exe'
$taskLines=[System.Collections.Generic.List[string]]::new()
foreach($size in @(@(1280,720),@(1600,900),@(1920,1080),@(2560,1440))){
 $name="$($size[0])x$($size[1])";$output=Join-Path $PSScriptRoot "Q2-$name-runtime.txt"
 $argsList=@('-screen-width',$size[0],'-screen-height',$size[1],'-screen-fullscreen','0','-save-path',('"'+(Join-Path $taskRoot "Logs/Q2-$name-save.json")+'"'),'-ui-verify',('"'+$output+'"'),'-logFile',('"'+(Join-Path $taskRoot "Logs/Q2-$name.log")+'"'))
 $p=Start-Process $taskExe -ArgumentList $argsList -WindowStyle Hidden -PassThru
 if(!$p.WaitForExit(90000)){Stop-Process -Id $p.Id;$taskLines.Add("KNOWN-LIMIT $name timeout")}else{$taskLines.Add("MEASURE requested window $name exit=$($p.ExitCode); evidence=Q2-$name-runtime.txt")}
 $scale=[Math]::Min($size[0]/1280,$size[1]/720);$taskLines.Add("CALCULATED HUD bounds=($([Math]::Round(16*$scale,2)),$([Math]::Round(16*$scale,2)),$([Math]::Round(350*$scale,2)),$([Math]::Round(118*$scale,2))) reference GameHUD/UITuning; not observed rendered coordinates")
}
$taskLines.Insert(0,'Q2 BLOCKED complete display validation; partial actual-exe UI state checks only.')
$taskLines.Add('NOT RUN native focus/Alt-Tab, fullscreen and live resize: native computer automation unavailable in this session; existing verifier forces IgnoreFocus, invalidating focus testing.')
$taskLines.Add('NOT VERIFIED actual resolved dimensions, per-room HUD rendered bounds, boss/tell visibility; requested resolution and calculated bounds do not prove rendered output. Q7 is separate capture attempt.')
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q2-display.txt'),$taskLines)
Regression Q2;Record Q2 'END BLOCKED partial exe states';Commit Q2
Record Q3 START
$taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','S3Verification.Run','-nightTask','Q3-night4-jump','-save-path','Logs/Q3-isolated-save.json','-logFile','Logs/Q3-night4-jump.log')
$p=Start-Process $taskUnity -ArgumentList $taskArgs -WindowStyle Hidden -PassThru
if(!$p.WaitForExit(240000)){Stop-Process -Id $p.Id;$taskResult='KNOWN-LIMIT timeout'}else{$taskResult="exit=$($p.ExitCode)"}
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q3-fps-baseline.txt'),@('Q3 BLOCKED complete section x fps matrix; existing unchanged Editor double-jump measurement executed.',"S3Verification.Run $taskResult; evidence Q3-night4-jump.txt",'Measured ordinary/double jump at 30/60/120fps; vSync off. See actual MEASURE rows. 144fps and full A05/G-D/G-J/B04/C02/C03 matrix NOT RUN: current tools do not expose fps-parametrized full routes; no game code modification allowed.','Existing S3-05 runner includes old practice-height authoring and must not be rerun; no authoring scripts restored or executed.'))
Regression Q3;Record Q3 'END BLOCKED partial baseline';Commit Q3
Record Q4 START
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q4-route-times.txt'),@('Q4 BLOCKED route measurement validity.','Reviewed S3BuildVerification.ExitTo, S3FinalVerification.ExitTo, S2Tests.ExitTo and existing exe route evidence: they place player directly on exit triggers. These routes cannot measure CP-to-challenge or return traversal time. No successful continuous prescribed input route for these full intervals is available.','CP->challenge goal 30..90s: NOT MEASURED','CP-C04->boss goal <=15s: NOT MEASURED','A05->A03 goal <=90s: NOT MEASURED','B04->B01 goal <=120s: NOT MEASURED','PT-001 comparison unavailable: empty user memo, Q0 SKIPPED. No teleport time or theoretical speed estimate reported as observed route time.'))
Regression Q4;Record Q4 'END BLOCKED no valid continuous route';Commit Q4
Record Q5 START
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q5-soak.txt'),@('Q5 BLOCKED required 45min repeated-route instrumentation.','Existing executable CLI verifiers run finite sequences then quit. None supports 45min loop, 300 room changes, 50 deaths, 100 pauses and in-process scene GameObject/EnemyShot count export. Native application controls are unavailable; game code modifications are forbidden in Q5.','NOT RUN 45min workload. No idle memory test substituted for the prescribed workload. Working Set/GameObject/EnemyShot/Error samples and CSV were not generated.','Needed: separately approved opt-in runtime soak verification runner, or a supported native input session plus in-process observation. Gameplay/default tuning/scene assets remain unchanged.'))
Regression Q5;Record Q5 'END BLOCKED no supported workload observer';Commit Q5
