$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskPwsh=Join-Path $PSHOME 'pwsh.exe'
$taskSummary=Join-Path $PSScriptRoot 'N9-difficulty-candidates-v2.txt'
if(Test-Path -LiteralPath $taskSummary){throw 'Preserve existing candidate summary'}
$taskOriginalNames=@('CombatTuning','E1Tuning','E2Tuning','E3Tuning','BossTuning','PlayerTuning')
$taskHashes=@{}
foreach($taskName in $taskOriginalNames){$taskHashes[$taskName]=(Get-FileHash -LiteralPath (Join-Path $taskRoot "Assets/ScriptableObjects/$taskName.asset")).Hash}
$taskResults=@()
function Read-Field([string]$Path,[string]$Name){$taskMatch=Select-String -LiteralPath $Path -Pattern ('^\s+'+[regex]::Escape($Name)+':\s*(\S+)') | Select-Object -First 1;if(!$taskMatch){throw "Missing field $Name in $Path"};return $taskMatch.Matches[0].Groups[1].Value}
foreach($taskCandidate in @('H1','H2','H3')){
 $taskBuildArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N9Build.Run','-n9-build',($taskCandidate+'-v2'),'-n9-candidate',$taskCandidate,'-logFile',('"'+(Join-Path $taskRoot "Logs/N9-$taskCandidate-v2-build.log")+'"'))
 $taskBuild=Start-Process $taskUnity -ArgumentList $taskBuildArgs -WindowStyle Hidden -PassThru
 if(!$taskBuild.WaitForExit(300000)){Stop-Process -Id $taskBuild.Id;throw 'Candidate build timeout'}
 if($taskBuild.ExitCode -ne 0){throw "Candidate build failed $taskCandidate"}
 $taskCsvRoot=Join-Path $env:USERPROFILE 'AppData/LocalLow/Afterglow/Afterglow/Playtest'
 $taskRouteStart=Get-Date
 foreach($taskCheck in @('route','boss-retry')){
  $taskTag="N9-$taskCandidate-v2-$taskCheck"
  $taskCheckArgs=@('-NoProfile','-File',('"'+(Join-Path $PSScriptRoot 'Run-N9-Exe.ps1')+'"'),'-Exe',"Builds/$taskCandidate-v2/Afterglow-$taskCandidate-v2.exe",'-Tag',$taskTag,'-Check',$taskCheck,'-Candidate',$taskCandidate)
  $taskCheckProcess=Start-Process $taskPwsh -ArgumentList $taskCheckArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
  if(!$taskCheckProcess.WaitForExit(400000)){Stop-Process -Id $taskCheckProcess.Id;throw 'Candidate checker timeout'}
  Write-Output "$taskTag exit=$($taskCheckProcess.ExitCode)"
  # Expected fixed-library gameplay failure is comparison data; continue other candidates.
 }
 $taskCsv=Get-ChildItem -LiteralPath $taskCsvRoot -Filter '*.csv' | Where-Object LastWriteTime -GE $taskRouteStart | Sort-Object LastWriteTime -Descending | Select-Object -First 1
 $taskDeath='none';$taskCsvName='none'
 if($taskCsv){$taskCsvName=$taskCsv.Name;$taskDest=Join-Path $taskRoot "Playtest/$taskCsvName";if(!(Test-Path -LiteralPath $taskDest)){Copy-Item -LiteralPath $taskCsv.FullName -Destination $taskDest};$taskRows=Import-Csv -LiteralPath $taskDest;$taskLastDeath=$taskRows | Where-Object event -EQ 'death' | Select-Object -Last 1;if($taskLastDeath){$taskDeath="$($taskLastDeath.room) ($($taskLastDeath.x),$($taskLastDeath.y)); cause=$($taskLastDeath.cause)"}}
 $taskRoutePath=Join-Path $PSScriptRoot "N9-$taskCandidate-v2-route.txt"
 $taskBossPath=Join-Path $PSScriptRoot "N9-$taskCandidate-v2-boss-retry.txt"
 $taskRouteLines=Get-Content -LiteralPath $taskRoutePath
 $taskBossLines=Get-Content -LiteralPath $taskBossPath
 $taskRouteOK=$taskRouteLines[0].StartsWith('PASS')
 $taskBossOK=$taskBossLines[0].StartsWith('PASS')
 $taskBlocked=if($taskRouteOK){'-'}else{($taskRouteLines | Where-Object {$_ -like 'FAIL *'} | Select-Object -Last 1)}
 $taskFight=$taskBossLines | Select-String 'MEASURE final boss fight seconds=(\S+)' | Select-Object -First 1
 $taskFightTime=if($taskFight){$taskFight.Matches[0].Groups[1].Value}else{'not reached'}
 $taskAssetFolder=if($taskCandidate -eq 'H1'){Join-Path $taskRoot 'Assets/ScriptableObjects'}else{Join-Path $taskRoot "Assets/N9Candidates/$taskCandidate"}
 $taskValues=@{}
 foreach($taskPair in @(@('playerHP','CombatTuning','playerMaxHP'),@('invincible','CombatTuning','invincibility'),@('E1speed','E1Tuning','speed'),@('E2tell','E2Tuning','warningSeconds'),@('E2speed','E2Tuning','chargeSpeed'),@('E3period','E3Tuning','shotPeriod'),@('bossHP','BossTuning','maxHP'),@('meleeTell','BossTuning','meleeTell'),@('waveTell','BossTuning','waveTell'),@('recovery','BossTuning','recovery'),@('waveSpeed','BossTuning','waveSpeed'))){$taskValues[$taskPair[0]]=Read-Field (Join-Path $taskAssetFolder ($taskPair[1]+'.asset')) $taskPair[2]}
 $taskResults+=[pscustomobject]@{candidate=$taskCandidate;values=$taskValues;route=if($taskRouteOK){'PASS'}else{'FAIL'};blocked=$taskBlocked;death=$taskDeath;bossRetry=if($taskBossOK){'PASS'}else{'FAIL'};fightSeconds=$taskFightTime;csv=$taskCsvName}
}
foreach($taskName in $taskOriginalNames){if((Get-FileHash -LiteralPath (Join-Path $taskRoot "Assets/ScriptableObjects/$taskName.asset")).Hash -ne $taskHashes[$taskName]){throw "Original tuning changed during candidates: $taskName"}}
$taskLines=@('N9 H1/H2/H3 actual sequential candidate comparison','Original tuning hashes unchanged during all candidate builds. New copied assets/prefabs only; build scene remapping; title candidate label compiled only for candidate.','Fixed N8-route-final-library.json unchanged; candidate failures are comparison data, not claims of impossible human completion. BossRetry uses adaptive virtual inputs and three natural deaths before final victory.','', '| Candidate | Player HP / invincible(s) | E1 speed(u/s) | E2 tell(s) / speed(u/s) | E3 period(s) | Boss HP / tell melee-wave(s) / recovery(s) / wave speed(u/s) |','| --- | --- | --- | --- | --- | --- |')
foreach($taskRow in $taskResults){$v=$taskRow.values;$taskLines+="| $($taskRow.candidate) | $($v.playerHP) / $($v.invincible) | $($v.E1speed) | $($v.E2tell) / $($v.E2speed) | $($v.E3period) | $($v.bossHP) / $($v.meleeTell)-$($v.waveTell) / $($v.recovery) / $($v.waveSpeed) |"}
$taskLines+=@('','| Candidate | Fixed RouteAll60 | blocked | actual death position | BossRetry | final fight(s) | CSV |','| --- | --- | --- | --- | --- | --- | --- |')
foreach($taskRow in $taskResults){$taskLines+="| $($taskRow.candidate) | $($taskRow.route) | $($taskRow.blocked) | $($taskRow.death) | $($taskRow.bossRetry) | $($taskRow.fightSeconds) | $($taskRow.csv) |"}
$taskLines+=@('','Human review: play each candidate10min, choose one. No automatic acceptance/default tuning switch. Final main S5-hard stays H1 values (boss28), candidatesH2/H3 boss32.')
$taskLines | Set-Content -LiteralPath $taskSummary
$taskResults | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $PSScriptRoot 'N9-candidate-results-v2.json')
Write-Output 'Candidate comparison complete; original tuning unchanged.'
