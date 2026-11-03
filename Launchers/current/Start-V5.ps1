param([ValidateSet('NewGame','AfterDash','DJ','BodySizeTest','Loop','BossTestB1')][string]$Mode='NewGame',[ValidateSet('','1.6','1.3','1.1')][string]$BodyHeight='',[switch]$HazardReturn,[switch]$CornerFix,[switch]$DashCornerFix,[switch]$NoMap,[switch]$Graybox,[switch]$Smoke,[string]$Evidence='')
$ErrorActionPreference='Stop'
$taskRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$taskBuild=Join-Path $taskRoot 'Builds/B26-delivery'
if($DashCornerFix){$CornerFix=$true}
$taskSlot='V5-'+$Mode
if($BodyHeight){$taskSlot+='-Body'+$BodyHeight.Replace('.','')}
if($HazardReturn){$taskSlot+='-HazardReturn'}
if($CornerFix){$taskSlot+='-CornerFix'}
if($DashCornerFix){$taskSlot+='-DashCornerFix'}
if($Graybox){$taskSlot+='-Graybox'}
if($Smoke){if(!$Evidence){throw 'Smoke requires a new evidence path'};$taskSlot+='-smoke-'+[IO.Path]::GetFileNameWithoutExtension($Evidence)}
$taskSlots=Join-Path $taskBuild 'slots'
New-Item -ItemType Directory -Force $taskSlots | Out-Null
$taskSave=Join-Path $taskSlots ($taskSlot+'.json')
$taskRoom=if($Mode -eq 'BodySizeTest'){'BodySizeTest'}elseif($Mode -in @('AfterDash','Loop','BossTestB1')){'A03'}else{'A01'}
$taskGive=if($Mode -eq 'AfterDash'){'dash'}elseif($Mode -eq 'DJ'){'dj'}else{''}
$taskArgs=@('-v5','-map-layout=world_v5_AB',('-start-room='+$taskRoom),('-give='+$taskGive),'-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')
if($Mode -eq 'BodySizeTest'){$taskArgs=@('-body-size-test','-save-path',('"'+$taskSave+'"'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0')}
if($Mode -eq 'BossTestB1'){$taskArgs+='-boss-test-b1'}
$taskRoomCsv=''
if($Mode -eq 'Loop'){
 $taskRoomCsv=Join-Path $taskRoot ('Playtest/'+$(if($Smoke){'smoke-'}else{''})+[DateTime]::Now.ToString('yyyyMMdd-HHmmss-fff')+'-Loop-'+[Guid]::NewGuid().ToString('N').Substring(0,8)+'.csv')
 $taskArgs+=@('-log-room-times','-playtest-csv',('"'+$taskRoomCsv+'"'))
 if($Smoke){$taskArgs+=@('-launch-smoke-seconds','30')}
}
if($BodyHeight){$taskArgs+=@('-body-height',$BodyHeight)}
if($HazardReturn){$taskArgs+='-hazard-return'}
if($CornerFix){$taskArgs+='-corner-fix'}
if($DashCornerFix){$taskArgs+='-dash-corner-fix'}
if($NoMap){$taskArgs+='-no-map'}
if($Graybox){$taskArgs+='-v5-graybox'}
if($Smoke){
 $taskOutput=[IO.Path]::GetFullPath((Join-Path $taskRoot $Evidence))
 if(Test-Path -LiteralPath $taskOutput){throw ('Preserve '+$taskOutput)}
 $taskArgs+=@('-batchmode','-verify-b20-launch','-verify-output',('"'+$taskOutput+'"'),'-logFile',('"'+[IO.Path]::ChangeExtension($taskOutput,'log')+'"'))
}
$taskProcess=Start-Process (Join-Path $taskBuild 'Afterglow.exe') -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle $(if($Smoke){'Hidden'}else{'Normal'}) -PassThru
$taskProcess.WaitForExit()
if($taskRoomCsv -and (Test-Path -LiteralPath $taskRoomCsv)){
 $taskPython=Join-Path $env:USERPROFILE '.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe'
 if(Test-Path -LiteralPath $taskPython){foreach($taskLogCsv in Get-ChildItem -LiteralPath (Split-Path $taskRoomCsv -Parent) -Filter ([IO.Path]::GetFileNameWithoutExtension($taskRoomCsv)+'*.csv') | Where-Object {$_.Name -notlike '*-room-times.csv'}){& $taskPython -B (Join-Path $taskRoot 'Tools/Export-RoomTimes.py') $taskLogCsv.FullName}} else{Write-Warning ('Room CSV saved; Python exporter unavailable: '+$taskRoomCsv)}
}
Write-Output ('V5 '+$Mode+' room='+$taskRoom+' give='+$taskGive+' exit='+$taskProcess.ExitCode)
if($Smoke -and (Test-Path -LiteralPath $taskOutput)){Get-Content -LiteralPath $taskOutput}
exit $taskProcess.ExitCode
