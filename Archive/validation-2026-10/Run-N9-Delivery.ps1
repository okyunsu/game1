$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskPwsh=Join-Path $PSHOME 'pwsh.exe'
$taskP=Start-Process $taskPwsh -ArgumentList @('-NoProfile','-File',('"'+(Join-Path $PSScriptRoot 'Run-N9-Exe.ps1')+'"'),'-Exe','Builds/H1-v2/Afterglow-H1-v2.exe','-Tag','N9-H1-v3-route','-Check','route','-Candidate','H1') -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskP.WaitForExit(400000)){Stop-Process -Id $taskP.Id;throw 'H1 fixed replay timeout'}
$taskRows=Get-Content (Join-Path $PSScriptRoot 'N9-candidate-results-v2.json') -Raw|ConvertFrom-Json
$taskH1=$taskRows|Where-Object candidate -eq 'H1'
$taskLines=Get-Content (Join-Path $PSScriptRoot 'N9-H1-v3-route.txt')
$taskH1.route=if($taskLines[0].StartsWith('PASS')){'PASS'}else{'FAIL'}
$taskH1.blocked=if($taskH1.route -eq 'PASS'){'-'}else{($taskLines|Where-Object {$_ -like 'FAIL *'}|Select-Object -Last 1)}
$taskStart=(Get-Item (Join-Path $taskRoot 'Logs/N9-H1-v3-route.log')).CreationTime
$taskCsv=Get-ChildItem (Join-Path $env:USERPROFILE 'AppData/LocalLow/Afterglow/Afterglow/Playtest') -Filter '*.csv'|Where-Object CreationTime -ge $taskStart.AddSeconds(-2)|Sort-Object CreationTime|Select-Object -First 1
if($taskCsv){$taskDest=Join-Path $taskRoot ('Playtest/'+$taskCsv.Name);if(!(Test-Path -LiteralPath $taskDest)){Copy-Item -LiteralPath $taskCsv.FullName -Destination $taskDest};$taskH1.csv=$taskCsv.Name;$taskDeath=Import-Csv $taskDest|Where-Object event -eq 'death'|Select-Object -Last 1;$taskH1.death=if($taskDeath){"$($taskDeath.room) ($($taskDeath.x),$($taskDeath.y)); cause=$($taskDeath.cause)"}else{'none'}}
$taskOutput=@('FINAL CORRECTED N9 candidate comparison. Use only this table for current deliveries.','InitialH2/H3 ignored scene remaps and are INVALID; H1-v2-route selected legacy mode and is INVALID. Final H1 uses H1-v3-route; H2/H3 use v2 fixed RouteRunner results. All final BossRetry runtime assertions pass expected HP/inv/recovery. Eighteen clean scene conversion callbacks per build. No source tuning switch.','','| Candidate | Player HP / inv(s) | E1 speed | E2 tell / speed | E3 period | Boss HP / melee-wave tell / recovery / wave speed |','| --- | --- | --- | --- | --- | --- |')
foreach($taskRow in $taskRows){$v=$taskRow.values;$taskOutput+="| $($taskRow.candidate) | $($v.playerHP) / $($v.invincible) | $($v.E1speed) | $($v.E2tell) / $($v.E2speed) | $($v.E3period) | $($v.bossHP) / $($v.meleeTell)-$($v.waveTell) / $($v.recovery) / $($v.waveSpeed) |"}
$taskOutput+=@('','| Candidate | Fixed RouteAll60 | blocked | death | BossRetry | final fight(s) | CSV |','| --- | --- | --- | --- | --- | --- | --- |')
foreach($taskRow in $taskRows){$taskOutput+="| $($taskRow.candidate) | $($taskRow.route) | $($taskRow.blocked) | $($taskRow.death) | $($taskRow.bossRetry) | $($taskRow.fightSeconds) | $($taskRow.csv) |"}
$taskOutput+=@('','H2/H3 R06 authored-exit mismatch is an input-library limitation (old R05 recording relied on checkpoint-return goal). Fixed-input failure does not establish impossibility for a human. Compare each candidate10min and choose one; no automatic default adoption.','Source original tuning hashes unchanged across candidate runs (v2 script). Candidate geometry/source scenes unchanged.')
$taskFinal=Join-Path $PSScriptRoot 'N9-difficulty-final.txt'
if(Test-Path -LiteralPath $taskFinal){throw 'Preserve final evidence'}
$taskOutput|Set-Content $taskFinal
$taskOutput|Add-Content (Join-Path $PSScriptRoot 'N9-difficulty-candidates.txt')
$taskRows|ConvertTo-Json -Depth 5|Set-Content (Join-Path $PSScriptRoot 'N9-candidate-results-final.json')
$taskBoundary=[IO.Path]::GetFullPath((Join-Path $taskRoot 'Builds'))+[IO.Path]::DirectorySeparatorChar
$taskLabelLines=@('Compiled label check only; no claim of actual title screenshot. Tested v2 binaries copied to requested H1/H2/H3 delivery folders; exe hashes verified after rename.')
foreach($taskCandidate in @('H1','H2','H3')){
 $taskSource=[IO.Path]::GetFullPath((Join-Path $taskRoot "Builds/$taskCandidate-v2"));$taskDelivery=[IO.Path]::GetFullPath((Join-Path $taskRoot "Builds/$taskCandidate"));$taskArchive=[IO.Path]::GetFullPath((Join-Path $taskRoot "Builds/$taskCandidate-invalid-initial"))
 foreach($taskPath in @($taskSource,$taskDelivery,$taskArchive)){if(!$taskPath.StartsWith($taskBoundary,[StringComparison]::OrdinalIgnoreCase)){throw 'Unsafe candidate delivery path'}}
 if(Test-Path -LiteralPath $taskArchive){throw 'Preserve invalid initial candidate archive'}
 Move-Item -LiteralPath $taskDelivery -Destination $taskArchive
 Copy-Item -LiteralPath $taskSource -Destination $taskDelivery -Recurse
 Rename-Item -LiteralPath (Join-Path $taskDelivery "Afterglow-$taskCandidate-v2.exe") -NewName "Afterglow-$taskCandidate.exe"
 Rename-Item -LiteralPath (Join-Path $taskDelivery "Afterglow-$taskCandidate-v2_Data") -NewName "Afterglow-$taskCandidate`_Data"
 $taskHash=(Get-FileHash -LiteralPath (Join-Path $taskSource "Afterglow-$taskCandidate-v2.exe")).Hash
 if((Get-FileHash -LiteralPath (Join-Path $taskDelivery "Afterglow-$taskCandidate.exe")).Hash -ne $taskHash){throw 'Candidate binary copy mismatch'}
 $taskDll=Join-Path $taskDelivery "Afterglow-$taskCandidate`_Data/Managed/Assembly-CSharp.dll"
 $taskExpected=if($taskCandidate -eq 'H1'){'H1 — current Hard'}elseif($taskCandidate -eq 'H2'){'H2 — harder'}else{'H3 — faster combat / more forgiveness'}
 $taskHasLabel=[Convert]::ToHexString([IO.File]::ReadAllBytes($taskDll)).Contains([Convert]::ToHexString([Text.Encoding]::Unicode.GetBytes($taskExpected)))
 if(!$taskHasLabel){throw 'Missing compiled candidate title label'}
 $taskLabelLines+="$taskCandidate compiled labelPASS / copied exeSHA256=$taskHash"
}
$taskLabelLines|Set-Content (Join-Path $PSScriptRoot 'N9-candidate-delivery.txt')
Write-Output 'Final candidate comparison/delivery complete.'
& (Join-Path $PSScriptRoot 'Run-N9-Final.ps1')
