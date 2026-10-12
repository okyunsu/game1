$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskExe=Join-Path $taskRoot 'Builds/N5-Part1/Afterglow-N5-Part1.exe'
$taskDir=Join-Path $taskRoot 'Logs/N5-Part1'
New-Item -ItemType Directory -Path $taskDir -Force | Out-Null
$taskLines=[System.Collections.Generic.List[string]]::new()
function Run-Case($name,$phase,$slot){
 $output=Join-Path $PSScriptRoot "N5-Part1-$name.txt";if(Test-Path $output){throw 'Preserve evidence'}
 $argsList=@('-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-save-path',('"'+$slot+'"'),'-s2-verify',$phase,('"'+$output+'"'),'-logFile',('"'+(Join-Path $taskDir "$name.log")+'"'))
 $p=Start-Process $taskExe -ArgumentList $argsList -WindowStyle Normal -PassThru
 if(!$p.WaitForExit(90000)){Stop-Process -Id $p.Id;throw "$name timeout"}
 $first=Get-Content $output -First 1;$taskLines.Add("$name exit=$($p.ExitCode) $first")
 if($p.ExitCode -ne 0 -or !$first.StartsWith('PASS')){throw "$name FAIL"}
}
foreach($case in @(@('truncated','{"version":1,"checkpointId":'),@('empty',''),@('invalid','{not-json}'),@('version','{"version":999,"checkpointId":"CP-A01"}'))){$slot=Join-Path $taskDir ($case[0]+'.json');[IO.File]::WriteAllText($slot,$case[1]);Run-Case $case[0] corrupt $slot}
$slot=Join-Path $taskDir 'existing-suite.json'
foreach($phase in @('acquire','continue','continuecp','new')){Run-Case $phase $phase $slot}
& (Join-Path $PSScriptRoot 'Run-Regression.ps1') -Tag N5-Part1
$summary=Get-Content (Join-Path $PSScriptRoot 'REG-N5-Part1-summary.txt');if(@($summary | Where-Object {$_ -match ' PASS '}).Count -ne 13){throw 'Regression failed'}
$taskLines.Insert(0,'PASS N5 Part1: four corruption cases + existing acquire/Continue/CP/repeated death/new-game suite; actual exe Error0; regression13/13.')
$taskLines.Add('Only ProgressSave.TryLoad catch filter changed: InvalidDataException added; existing JsonUtility ArgumentException handling retained. Save fields/version and other lines unchanged.')
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'N5-Part1-save-fix.txt'),$taskLines)
Add-Content (Join-Path $PSScriptRoot 'N5-progress.txt') "$([datetimeoffset]::UtcNow.ToString('o')) PART1 END PASS"
