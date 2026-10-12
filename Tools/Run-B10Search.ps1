param([string]$Build='B10-harness-r2',[string]$Mode='routes',[string]$Room='',[string]$Route='',[string]$Tag='run',[int]$Fps=60,[int]$Timeout=2460,[switch]$Expanded,[switch]$Fast,[int]$Part=0,[int]$Parts=1)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
if($Tag -notmatch '^[a-zA-Z0-9-]+$'){throw 'Safe tag required'}
$taskOutput=Join-Path $taskRoot "Validation/_raw/B10/$Tag.txt"
if((Test-Path -LiteralPath $taskOutput) -or (Test-Path -LiteralPath ([IO.Path]::ChangeExtension($taskOutput,'live.txt')))){throw 'Preserve evidence: new tag required'}
$taskArgs=@('-v4','-verify-b10-search',$Mode,'-verify-fps',"$Fps",'-verify-output',('"'+$taskOutput+'"'),'-save-path',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$Tag-save.json")+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$Tag.log")+'"'))
if($Room){$taskArgs+=@('-b10-room',$Room)}
if($Route){$taskArgs+=@('-b10-route',$Route)}
if($Expanded){$taskArgs+='-b10-expanded'}
if($Fast){$taskArgs+='-b10-fast';$taskArgs[$taskArgs.IndexOf('-screen-width')+1]='320';$taskArgs[$taskArgs.IndexOf('-screen-height')+1]='180'}
if($Parts -lt 1 -or $Part -lt 0 -or $Part -ge $Parts){throw 'Invalid candidate partition'}
$taskArgs+=@('-b10-part',"$Part",'-b10-parts',"$Parts")
$taskProcess=Start-Process (Join-Path $taskRoot "Builds/$Build/Afterglow.exe") -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskProcess.WaitForExit($Timeout*1000)){Stop-Process -Id $taskProcess.Id; "BUDGET-DEFERRED process timeout $Tag" | Set-Content (Join-Path $taskRoot "Validation/_raw/B10/$Tag-timeout.txt");exit 2}
Write-Output "$Tag exit=$($taskProcess.ExitCode)"
if(Test-Path -LiteralPath $taskOutput){Get-Content -LiteralPath $taskOutput | Select-Object -Last 10}
exit $taskProcess.ExitCode
