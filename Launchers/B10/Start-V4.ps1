param([ValidateSet('NewGame','Mechanics','AfterDash','AfterWall','BeforeBoss','AfterEnding')][string]$Mode='NewGame',[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B10-delivery'
$taskSlot=$Mode
if($Smoke){$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots'
New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskSave=Join-Path $taskSlots ($taskSlot+'.json')
$taskArgs=@('-v4','-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
if($Mode -eq 'NewGame'){$taskArgs+='-v4-new-game'}elseif($Mode -eq 'Mechanics'){$taskArgs+='-v4-mechanics'}else{
 $taskSnapshot=Join-Path $PSScriptRoot ('saves/'+$Mode+'.json')
 if(!(Test-Path -LiteralPath $taskSnapshot)){throw "Natural save for $Mode is not available. See Validation/B10-T6-summary.md; no replacement save is fabricated."}
 Copy-Item -LiteralPath $taskSnapshot -Destination $taskSave
 Write-Output 'Select Continue in the title menu. This starts at the checkpoint in the actual natural save.'
}
if($Smoke){
 $taskVerifier=if($Mode -in @('NewGame','Mechanics')){'-verify-b09-launch'}else{'-verify-b10-stage'}
 $taskArgs+=@('-batchmode',$taskVerifier,'-verify-output',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))+'"'),'-logFile',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot ([IO.Path]::ChangeExtension($Evidence,'log'))))+'"'))
}
Set-Location -LiteralPath $taskRoot
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -Wait -PassThru
Write-Output ($Mode+' exit='+$taskProcess.ExitCode)
if($Smoke){Get-Content -LiteralPath (Join-Path $taskRoot $Evidence)}
exit $taskProcess.ExitCode
