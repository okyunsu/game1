param([ValidateSet('NewGame','Mechanics','AfterBoss')][string]$Mode='NewGame',[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B09-delivery'
$taskSlot=$Mode
if($Smoke){$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots'
New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskFlag=if($Mode -eq 'Mechanics'){'-v4-mechanics'}elseif($Mode -eq 'AfterBoss'){'-v4-afterboss'}else{'-v4-new-game'}
$taskArgs=@('-v4',$taskFlag,'-save-path',('"'+(Join-Path $taskSlots ($taskSlot+'.json'))+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
if($Smoke){$taskArgs+=@('-batchmode','-verify-b09-launch','-verify-output',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))+'"'),'-logFile',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot ([IO.Path]::ChangeExtension($Evidence,'log'))))+'"'))}
Set-Location -LiteralPath $taskRoot
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -Wait -PassThru
Write-Output ($Mode+' exit='+$taskProcess.ExitCode)
if($Smoke){Get-Content -LiteralPath (Join-Path $taskRoot $Evidence)}
exit $taskProcess.ExitCode
