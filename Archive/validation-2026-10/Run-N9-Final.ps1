$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskPwsh=Join-Path $PSHOME 'pwsh.exe'
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskP=Start-Process $taskPwsh -ArgumentList @('-NoProfile','-File',('"'+(Join-Path $PSScriptRoot 'Run-Regression.ps1')+'"'),'-Tag','N9-final') -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskP.WaitForExit(2400000)){Stop-Process -Id $taskP.Id;throw 'Final regression timeout'}
Write-Output "final regression exit=$($taskP.ExitCode)"
$taskP=Start-Process $taskUnity -ArgumentList @('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N9Build.Run','-n9-build','S5-hard-ui-final','-logFile','Logs/N9-S5-hard-ui-final-build.log') -WindowStyle Hidden -PassThru
if(!$taskP.WaitForExit(300000)){Stop-Process -Id $taskP.Id;throw 'Final S5 build timeout'}
if($taskP.ExitCode -ne 0){throw 'Final S5 build failure'}
$taskSource=[IO.Path]::GetFullPath((Join-Path $taskRoot 'Builds/S5-hard-ui-final'))
$taskDelivery=[IO.Path]::GetFullPath((Join-Path $taskRoot 'Builds/S5-hard'))
$taskArchive=[IO.Path]::GetFullPath((Join-Path $taskRoot 'Builds/S5-hard-pre-ui'))
$taskBoundary=[IO.Path]::GetFullPath((Join-Path $taskRoot 'Builds'))+[IO.Path]::DirectorySeparatorChar
foreach($taskPath in @($taskSource,$taskDelivery,$taskArchive)){if(!$taskPath.StartsWith($taskBoundary,[StringComparison]::OrdinalIgnoreCase)){throw 'Unsafe build path'}}
if(Test-Path -LiteralPath $taskArchive){throw 'Preserve prior delivery archive'}
Move-Item -LiteralPath $taskDelivery -Destination $taskArchive
New-Item -ItemType Directory -Path $taskDelivery | Out-Null
foreach($taskItem in Get-ChildItem -LiteralPath $taskSource){
 $taskName=$taskItem.Name.Replace('Afterglow-S5-hard-ui-final','Afterglow-S5-hard')
 Copy-Item -LiteralPath $taskItem.FullName -Destination (Join-Path $taskDelivery $taskName) -Recurse
}
Set-Content -LiteralPath (Join-Path $taskDelivery 'PogoTest 실행.bat') -Encoding ascii -Value '"%~dp0Afterglow-S5-hard.exe" -pogotest'
foreach($taskCheck in @('attack-up','pogotest','boss-retry')){
 $taskTag='N9-delivery-'+$taskCheck
 $taskP=Start-Process $taskPwsh -ArgumentList @('-NoProfile','-File',('"'+(Join-Path $PSScriptRoot 'Run-N9-Exe.ps1')+'"'),'-Exe','Builds/S5-hard/Afterglow-S5-hard.exe','-Tag',$taskTag,'-Check',$taskCheck) -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
 if(!$taskP.WaitForExit(400000)){Stop-Process -Id $taskP.Id;throw 'Delivery checker timeout'}
 Write-Output "$taskTag exit=$($taskP.ExitCode)"
}
$taskP=Start-Process (Join-Path $taskDelivery 'Afterglow-S5-hard.exe') -ArgumentList @('-pogotest','-verify-display','start','-verify-shot','Validation/N9-delivery-course.png','-verify-output','Validation/N9-delivery-course.txt','-save-path','Logs/N9-delivery-course-save.json','-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11','-logFile','Logs/N9-delivery-course.log') -WorkingDirectory $taskRoot -WindowStyle Normal -PassThru
if(!$taskP.WaitForExit(60000)){Stop-Process -Id $taskP.Id;throw 'Delivery capture timeout'}
Write-Output "delivery capture exit=$($taskP.ExitCode)"
