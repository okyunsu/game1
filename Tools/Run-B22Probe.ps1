param([Parameter(Mandatory=$true)][string]$Run,[string]$Mode='boss',[string[]]$Extra=@(),[string]$Build='Builds/_tmp/B22-work',[switch]$V4,[int]$TimeoutSeconds=240)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskOutput=Join-Path $taskRoot ('Validation/_raw/22/'+$Run)
if(Test-Path -LiteralPath $taskOutput){throw "Preserve $taskOutput"}
New-Item -ItemType Directory -Path $taskOutput | Out-Null
$taskArgs=@('-batchmode','-verify-b22','-b22-mode',$Mode,'-save-path',('"'+$taskOutput+'/save.json"'),'-verify-output',('"'+$taskOutput+'/result.txt"'),'-logFile',('"'+$taskOutput+'/player.log"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-verify-fps','60')
if($V4){$taskArgs+=@('-v4','-v4-new-game')}else{$taskArgs+=@('-v5','-map-layout=world_v5_A','-start-room=A03')}
$taskArgs+=$Extra
$taskProcess=Start-Process (Join-Path $taskRoot ($Build+'/Afterglow.exe')) -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskProcess.WaitForExit($TimeoutSeconds*1000)){Stop-Process -Id $taskProcess.Id;throw 'Probe timeout'}
$taskProcess.Refresh();Write-Output ('Probe exit='+$taskProcess.ExitCode+' evidence='+$taskOutput)
if(Test-Path -LiteralPath "$taskOutput/result.txt"){Get-Content -LiteralPath "$taskOutput/result.txt"}else{Get-Content "$taskOutput/player.log" -Tail 18}
exit $taskProcess.ExitCode
