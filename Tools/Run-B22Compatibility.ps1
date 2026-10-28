param([Parameter(Mandatory=$true)][ValidateSet('Completion','B3Cycle','B3Ending','B3Phase')][string]$Mode,[Parameter(Mandatory=$true)][string]$Run,[string]$Build='Builds/B22-delivery')
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskOutput=Join-Path $taskRoot ('Validation/_raw/22/'+$Run)
if(Test-Path -LiteralPath $taskOutput){throw "Preserve $taskOutput"}
New-Item -ItemType Directory -Path $taskOutput | Out-Null
$taskSave=Join-Path $taskOutput 'save.json'
if($Mode -ne 'Completion'){Copy-Item -LiteralPath (Join-Path $taskRoot 'Launchers/current/saves/BeforeBoss.json') -Destination $taskSave}
$taskPhases=if($Mode -eq 'Completion'){@('kill-now','continue')}else{@($Mode)}
foreach($taskPhase in $taskPhases){
 $taskResult=Join-Path $taskOutput ($taskPhase+'.txt')
 $taskArgs=@('-batchmode','-save-path',('"'+$taskSave+'"'),'-logFile',('"'+$taskOutput+'/'+$taskPhase+'.log"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
 if($Mode -eq 'Completion'){$taskArgs+=@('-completion-verify',$taskPhase,('"'+$taskResult+'"'))}else{
  $taskFlag=if($Mode -eq 'B3Cycle'){'-b12-cycle'}elseif($Mode -eq 'B3Ending'){'-b14-defeat'}else{'-b12-phase'}
  $taskArgs+=@('-v4','-boss=b3','-boss-fixture','-verify-b04-boss',$taskFlag,'-verify-fps','60','-verify-output',('"'+$taskResult+'"'))
 }
 $taskProcess=Start-Process (Join-Path $taskRoot ($Build+'/Afterglow.exe')) -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
 if(!$taskProcess.WaitForExit(240000)){Stop-Process -Id $taskProcess.Id;throw 'Compatibility timeout'}
 $taskProcess.Refresh();Write-Output ($taskPhase+' exit='+$taskProcess.ExitCode)
 if(Test-Path -LiteralPath $taskResult){Get-Content -LiteralPath $taskResult}
 if($taskProcess.ExitCode -ne 0){exit $taskProcess.ExitCode}
}
