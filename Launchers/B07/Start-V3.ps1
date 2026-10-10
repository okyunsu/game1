param([ValidateSet('Guide','NoGuide','A03','B01','C01','C04','Wall')][string]$Mode='Guide',[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B07-delivery'
$taskSlot=$Mode
if($Smoke){$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots';New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskSave=Join-Path $taskSlots ($taskSlot+'.json')
$taskArgs=@('-v3','-save-path',('"'+$taskSave+'"'))
$taskCheckpoint='CP-A01';$taskDash=$false;$taskWall=$false;$taskJump=$false;$taskQa=$Mode -in @('A03','B01','C01','C04')
if($taskQa){$taskCheckpoint='CP-'+$Mode;$taskDash=$true;$taskWall=$Mode -ne 'A03';$taskJump=$Mode -in @('C01','C04');if(!(Test-Path -LiteralPath $taskSave) -or $Smoke){[IO.File]::WriteAllText($taskSave,(@{version=1;dash=$taskDash;wallJump=$taskWall;doubleJump=$taskJump;brokenWalls=@();visitedRooms=@('A01','A03');bossDefeated=$false;endingReached=$false;checkpointId=$taskCheckpoint}|ConvertTo-Json),[Text.UTF8Encoding]::new($false))}}
if($Mode -eq 'NoGuide'){$taskArgs+='-no-guide'}
if($Mode -eq 'Wall'){$taskArgs=@('-walljumptest','-save-path',('"'+$taskSave+'"'))}
if($Smoke){$taskExpected=if($Mode -eq 'Wall'){'WallJumpTest'}elseif($taskQa){$Mode}else{'A01'};$taskArgs+=@('-verify-b07-launch','-b07-expected-room',$taskExpected,'-verify-output',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))+'"'),'-logFile',('"'+[IO.Path]::GetFullPath((Join-Path $taskRoot ([IO.Path]::ChangeExtension($Evidence,'log'))))+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11');if($taskQa){$taskArgs+='-b07-qa'};if($taskDash){$taskArgs+='-b07-expected-dash'};if($taskWall){$taskArgs+='-b07-expected-wall'};if($taskJump){$taskArgs+='-b07-expected-jump'}}
Set-Location -LiteralPath $taskRoot
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -Wait -PassThru
Write-Output ($Mode+' exit='+$taskProcess.ExitCode)
if($Smoke){Get-Content -LiteralPath (Join-Path $taskRoot $Evidence)}

