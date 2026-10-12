param(
 [Parameter(Mandatory=$true)][string]$Exe,
 [Parameter(Mandatory=$true)][ValidatePattern('^[a-zA-Z0-9-]+$')][string]$Tag,
 [Parameter(Mandatory=$true)][ValidateSet('route','boss-retry','attack-up','pogotest','pogo','directional-enemies')][string]$Check,
 [int]$Fps=60,[switch]$Gamepad,[switch]$Train,[string]$Candidate='',
 [string]$Library='Validation/N8-route-final-library.json',
 [string]$PogoLibrary='Validation/N9-pogo-library.json'
)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskExe=Join-Path $taskRoot $Exe
$taskEvidence=Join-Path $PSScriptRoot "$Tag.txt"
if(Test-Path -LiteralPath $taskEvidence){throw 'Preserve evidence: unique tag required'}
$taskSave=Join-Path $taskRoot "Logs/$Tag-save.json"
if(Test-Path -LiteralPath $taskSave){throw 'Preserve fixture: unique tag required'}
if($Check -eq 'boss-retry'){Set-Content -LiteralPath $taskSave -Encoding utf8NoBOM -Value '{"version":1,"dash":true,"doubleJump":true,"bossDefeated":false,"endingReached":false,"checkpointId":"CP-C04"}'}
$taskArgs=@('-verify-'+$Check)
if($Check -eq 'route'){$taskArgs+=@('all','-route-library',('"'+(Join-Path $taskRoot $Library)+'"'));if($Gamepad){$taskArgs+='-route-gamepad'}}
if($Check -eq 'pogotest'){$taskArgs+=@($(if($Train){'train'}else{'all'}),'-pogotest','-pogo-library',('"'+(Join-Path $taskRoot $PogoLibrary)+'"'),'-pogo-prefix',('Validation/'+$Tag))}
if($Candidate){$taskArgs+=@('-verify-candidate',$Candidate)}
$taskArgs+=@('-verify-fps',"$Fps",'-verify-output',('"'+$taskEvidence+'"'),'-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Logs/$Tag.log")+'"'))
$taskWatch=[Diagnostics.Stopwatch]::StartNew()
$taskProcess=Start-Process $taskExe -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Normal -PassThru
if(!$taskProcess.WaitForExit(360000)){Stop-Process -Id $taskProcess.Id;throw 'Executable test timeout'}
$taskWatch.Stop()
Write-Output "$Tag exit=$($taskProcess.ExitCode) wallSeconds=$($taskWatch.Elapsed.TotalSeconds)"
if(!(Test-Path -LiteralPath $taskEvidence)){throw 'Missing actual exe evidence'}
Get-Content -LiteralPath $taskEvidence -First 1
if($taskProcess.ExitCode -ne 0){exit 1}
