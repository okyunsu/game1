param([ValidateSet('NewGame','Mechanics','AfterDash','AfterWall','BeforeBoss','AfterEnding','BossTestB2')][string]$Mode='NewGame',[ValidateSet('base','b2','b2fast','b2slow','b3')][string]$Boss='',[switch]$AV1,[switch]$NoMap,[switch]$PhaseTwo,[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B26-delivery'
$taskSlot=$Mode
if($Boss){$taskSlot+='-'+$Boss}
if($PhaseTwo){$taskSlot+='-P2'}
if($Smoke){$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots'
New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskSave=Join-Path $taskSlots ($taskSlot+'.json')
$taskArgs=@('-v4','-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
if($Boss){$taskArgs+='-boss='+$Boss}
if($PhaseTwo){if($Boss -ne 'b3'){throw 'PhaseTwo launcher requires B3'};$taskArgs+=@('-boss-start-hp','14')}
if($AV1){$taskArgs+='-av1'}
if($NoMap){$taskArgs+='-no-map';Write-Output 'MAP off (-no-map)'}
if($Mode -eq 'BeforeBoss'){$taskArgs+='-boss-fixture'}
if($Mode -eq 'NewGame'){$taskArgs+='-v4-new-game'}elseif($Mode -eq 'Mechanics'){$taskArgs+='-v4-mechanics'}elseif($Mode -eq 'BossTestB2'){$taskArgs+=@('-boss-test-b2','-no-map')}else{
 $taskSnapshot=Join-Path $PSScriptRoot ('saves/'+$Mode+'.json')
 if(!(Test-Path -LiteralPath $taskSnapshot)){throw "Save snapshot for $Mode is not available."}
 Copy-Item -LiteralPath $taskSnapshot -Destination $taskSave
 Write-Output ('Select Continue in the title menu. '+$(if($Mode -in @('BeforeBoss','AfterEnding','BossTestB2')){'Development fixture save.'}else{'Natural save.'}))
}
if($Smoke){
 if(!$Evidence){throw 'Smoke requires a new evidence path'};if(Test-Path -LiteralPath (Join-Path $taskRoot $Evidence)){throw 'Preserve evidence'}
 $taskVerifier=if($Mode -eq 'BossTestB2'){'-verify-b22'}elseif($Mode -in @('NewGame','Mechanics')){'-verify-b09-launch'}else{'-verify-b10-stage'}
 $taskArgs+=@('-batchmode',$taskVerifier,'-verify-output',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))+'"'),'-logFile',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot ([IO.Path]::ChangeExtension($Evidence,'log'))))+'"'))
 if($Mode -in @('BeforeBoss','AfterEnding')){$taskArgs+='-verify-b11-launch'};if($Mode -eq 'BossTestB2'){$taskArgs+=@('-b22-mode','b24-b2-smoke')}
}
Set-Location -LiteralPath $taskRoot
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -Wait -PassThru
Write-Output ($Mode+' exit='+$taskProcess.ExitCode)
if($Smoke){Get-Content -LiteralPath (Join-Path $taskRoot $Evidence)}
exit $taskProcess.ExitCode
