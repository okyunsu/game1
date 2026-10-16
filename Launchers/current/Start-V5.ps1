param([ValidateSet('NewGame','AfterDash','DJ')][string]$Mode='NewGame',[switch]$NoMap,[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B12-delivery'
$taskSlot='V5-'+$Mode
if($Smoke){if(!$Evidence){throw 'Smoke requires a new evidence path'};$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots'
New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskSave=Join-Path $taskSlots ($taskSlot+'.json')
$taskRoom=if($Mode -eq 'AfterDash'){'A03'}else{'A01'}
$taskGive=if($Mode -eq 'AfterDash'){'dash'}elseif($Mode -eq 'DJ'){'dj'}else{''}
$taskArgs=@('-v5','-map-layout=world_v5_A',('-start-room='+$taskRoom),('-give='+$taskGive),'-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
if($NoMap){$taskArgs+='-no-map'}
if($Smoke){
 $taskOutput=[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))
 if(Test-Path -LiteralPath $taskOutput){throw ('Preserve '+$taskOutput)}
 $taskArgs+=@('-batchmode','-verify-b16-launch','-verify-output',('"'+$taskOutput+'"'),'-logFile',('"'+[IO.Path]::ChangeExtension($taskOutput,'log')+'"'))
}
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -PassThru
$taskProcess.WaitForExit()
Write-Output ('V5 '+$Mode+' room='+$taskRoom+' give='+$taskGive+' exit='+$taskProcess.ExitCode)
if($Smoke -and (Test-Path -LiteralPath $taskOutput)){Get-Content -LiteralPath $taskOutput}
exit $taskProcess.ExitCode
