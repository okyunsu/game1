param([string]$Build='B10-release-r26',[string]$Room,[string]$Route,[string]$Tag,[int]$Parts=20,[int]$Timeout=2460,[switch]$Expanded,[switch]$Split)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
if($Tag -notmatch '^[a-zA-Z0-9-]+$' -or $Parts -lt 1){throw 'Safe tag and positive partition count required'}
$taskProcesses=@()
for($taskPart=0;$taskPart -lt $Parts;$taskPart++){
 $taskName="$Tag-p$taskPart"
 $taskOutput=Join-Path $taskRoot "Validation/_raw/B10/$taskName.txt"
 if((Test-Path $taskOutput) -or (Test-Path ([IO.Path]::ChangeExtension($taskOutput,'live.txt')))){throw 'Preserve evidence: new tag required'}
 $taskArgs=@('-v4','-verify-b10-search','routes','-verify-fps','60','-b10-fast','-b10-room',$Room,'-b10-route',$Route,'-b10-part',"$taskPart",'-b10-parts',"$Parts",'-verify-output',('"'+$taskOutput+'"'),'-save-path',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$taskName-save.json")+'"'),'-screen-width','320','-screen-height','180','-screen-fullscreen','0','-force-d3d11','-logFile',('"'+(Join-Path $taskRoot "Validation/_raw/B10/$taskName.log")+'"'))
 if($Expanded){$taskArgs+='-b10-expanded'}
 if($Split){$taskArgs+='-b10-split'}
 $taskProcesses+=Start-Process (Join-Path $taskRoot "Builds/$Build/Afterglow.exe") -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
}
$taskDeadline=[DateTime]::UtcNow.AddSeconds($Timeout)
while(@($taskProcesses | Where-Object {!$_.HasExited}).Count -gt 0 -and [DateTime]::UtcNow -lt $taskDeadline){Start-Sleep -Seconds 1}
foreach($taskProcess in $taskProcesses){if(!$taskProcess.HasExited){Stop-Process -Id $taskProcess.Id;Write-Output "BUDGET-DEFERRED process=$($taskProcess.Id)"}else{Write-Output "process=$($taskProcess.Id) exit=$($taskProcess.ExitCode)"}}
