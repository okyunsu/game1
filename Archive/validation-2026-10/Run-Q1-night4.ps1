$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskExe=Join-Path $taskRoot 'Builds/S4-pre2/Afterglow-S4-pre2.exe'
$taskDir=Join-Path $taskRoot 'Logs/Q1-night4'
New-Item -ItemType Directory -Path $taskDir -Force | Out-Null
$taskLines=[System.Collections.Generic.List[string]]::new()
function Run-Exe($name,$flags,$slot,$stopCondition=$null){
 $evidence=Join-Path $PSScriptRoot "Q1-$name.txt"
 if(Test-Path -LiteralPath $evidence){throw "Preserve $evidence"}
 $argsList=@('-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-save-path',('"'+$slot+'"'),'-logFile',('"'+(Join-Path $taskDir "$name.log")+'"'))+$flags+@(('"'+$evidence+'"'))
 $p=Start-Process -FilePath $taskExe -ArgumentList $argsList -WindowStyle Hidden -PassThru
 $watch=[Diagnostics.Stopwatch]::StartNew();$killed=$false
 while(!$p.HasExited -and $watch.Elapsed.TotalSeconds -lt 90){
  if($null -ne $stopCondition -and (Test-Path -LiteralPath $slot)){
   try{$data=Get-Content -LiteralPath $slot -Raw | ConvertFrom-Json;if(& $stopCondition $data){Stop-Process -Id $p.Id;$killed=$true;break}}catch{}
  }
  Start-Sleep -Milliseconds 10;$p.Refresh()
 }
 if(!$killed -and !$p.HasExited){Stop-Process -Id $p.Id;$taskLines.Add("KNOWN-LIMIT $name timeout");return}
 if($killed){$taskLines.Add("MEASURE $name forced Stop-Process PID=$($p.Id) observed=$($watch.Elapsed.TotalSeconds.ToString('F3'))s save=$((Get-Content -LiteralPath $slot -Raw).Trim())");return}
 $p.WaitForExit();$taskLines.Add("$name exit=$($p.ExitCode) evidence=Q1-$name.txt");if(Test-Path $evidence){$taskLines.Add((Get-Content $evidence -First 1))}
}
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q1 START"
$slot=Join-Path $taskDir 'dash.json'
Run-Exe 'dash-kill' @('-s2-verify','acquire') $slot {param($d) $d.dash}
Run-Exe 'dash-continue-cp-kill' @('-s2-verify','continue') $slot {param($d) $d.checkpointId -eq 'CP-A03'}
Run-Exe 'cp-continue' @('-s2-verify','continuecp') $slot
$slot=Join-Path $taskDir 'jump.json'
Run-Exe 'jump-kill' @('-s3-verify','route') $slot {param($d) $d.doubleJump}
Run-Exe 'jump-continue' @('-s3-verify','continue') $slot
$slot=Join-Path $taskDir 'boss.json'
Run-Exe 'boss-kill' @('-completion-verify','kill-now') $slot {param($d) $d.bossDefeated -and !$d.endingReached}
Run-Exe 'ending-kill' @('-completion-verify','continue') $slot {param($d) $d.endingReached}
Run-Exe 'ending-continue' @('-s2-verify','continuecp') $slot
# The existing continuecp runner requires A03, so its failure is a harness limitation
# for an ending slot; it must never be reported as a gameplay failure.
foreach($case in @(@('truncated','{"version":1,"checkpointId":'),@('empty',''),@('invalid','{not-json}'),@('version','{"version":999,"checkpointId":"CP-A01"}'))){
 $slot=Join-Path $taskDir ($case[0]+'.json');[IO.File]::WriteAllText($slot,$case[1]);Run-Exe ('corrupt-'+$case[0]) @('-s2-verify','corrupt') $slot
}
$taskLines.Add('NOT RUN read-only file / unwritable directory: existing verifier has no write-failure assertion; no game-code changes allowed in Q1.')
$taskLines.Add('NOT RUN three boss deaths/reset/exit-lock: existing executable runners do not expose this sequence; Editor regression is separate evidence.')
$taskLines.Add('Ending-continue runner expects CP-A03 and cannot certify ending branch. See ending-kill: Continue from boss save reaches and persists ending; after-ending repeat needs a suitable observer.')
$taskLines.Insert(0,'Q1 PARTIAL / item evidence required; forced termination poll timing is recorded, not exact same-frame termination.')
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'Q1-dod-robustness.txt'),$taskLines)
Add-Content (Join-Path $PSScriptRoot 'Q-night4-progress.txt') "$((Get-Date).ToUniversalTime().ToString('o')) Q1 END PARTIAL"
