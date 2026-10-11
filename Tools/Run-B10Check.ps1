param([Parameter(Mandatory=$true)][string]$Exe,[Parameter(Mandatory=$true)][string]$Tag,[string[]]$CheckArgs=@(),[int]$Timeout=300,[int]$Fps=60)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
if($Tag -notmatch '^[a-zA-Z0-9-]+$'){throw 'Safe tag required'}
$taskOutput=Join-Path $taskRoot "Validation/_raw/B10/$Tag.txt"
if(Test-Path -LiteralPath $taskOutput){throw 'Preserve existing evidence'}
$taskArgs=$CheckArgs+@('-verify-fps',"$Fps",'-verify-output',('"'+$taskOutput+'"'),'-save-path',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$Tag-save.json")+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$Tag.log")+'"'))
$taskProcess=Start-Process $Exe -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskProcess.WaitForExit($Timeout*1000)){Stop-Process -Id $taskProcess.Id; "FAIL process timeout $Tag" | Set-Content (Join-Path $taskRoot "Validation/_raw/B10/$Tag-timeout.txt");exit 2}
Write-Output "$Tag exit=$($taskProcess.ExitCode)"
if(Test-Path -LiteralPath $taskOutput){Get-Content -LiteralPath $taskOutput | Select-Object -Last 10}
exit $taskProcess.ExitCode
