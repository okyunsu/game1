param([ValidatePattern('^[A-Za-z0-9-]+$')][string]$Batch='26',[string]$Build='Builds/B26-delivery',[ValidatePattern('^[A-Za-z0-9-]+$')][string]$Tag=('B'+$Batch+'-routes-'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss')))
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskExe=Join-Path $taskRoot ($Build+'/Afterglow.exe')
if(!(Test-Path -LiteralPath $taskExe)){throw 'Build the current Development executable first'}
$taskOutput=Join-Path $taskRoot ('Validation/_raw/'+$Batch+'/'+$Tag)
if(Test-Path -LiteralPath $taskOutput){throw ('Preserve '+$taskOutput)}
New-Item -ItemType Directory -Path $taskOutput | Out-Null
$taskRows=@()
foreach($taskRoom in @('A01','A02','A03','A04','A05','A06','B01','B02','B03','B04','B05','B06')){
 $taskDir=Join-Path $taskOutput $taskRoom
 New-Item -ItemType Directory -Path $taskDir | Out-Null
 $taskResult=Join-Path $taskDir 'result.txt'
 $taskMode=if($Batch -eq '26' -and $taskRoom -in @('A03','A05','A06','B02','B04','B06')){'b26-route'}else{'b25-room-route'}
 $taskArgs=@('-batchmode','-v5','-map-layout=world_v5_AB','-start-room=A03','-verify-b22','-b22-mode',$taskMode,'-b25-route-room',$taskRoom,'-verify-fps','60','-verify-output',('"'+$taskResult+'"'),'-save-path',('"'+$taskDir+'/save.json"'),'-logFile',('"'+$taskDir+'/game.log"'))
 $taskProcess=Start-Process $taskExe -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
 if(!$taskProcess.WaitForExit(180000)){Stop-Process -Id $taskProcess.Id;$taskVerdict='FAIL timeout'}
 elseif($taskProcess.ExitCode -eq 0 -and (Test-Path -LiteralPath $taskResult) -and (Get-Content -LiteralPath $taskResult -First 1).StartsWith('PASS')){$taskVerdict='PASS'}
 else{$taskVerdict='FAIL exit='+$taskProcess.ExitCode}
 $taskRows += [pscustomobject]@{room=$taskRoom;result=$taskVerdict;evidence=$taskResult}
 Write-Output ($taskRoom+' '+$taskVerdict)
 if(Test-Path -LiteralPath $taskResult){Get-Content -LiteralPath $taskResult | Where-Object {$_ -match '^FAIL|^MEASURE|Error/Exception/Assert='} | Write-Output}
}
$taskRows | Export-Csv -LiteralPath (Join-Path $taskOutput 'routes.csv') -NoTypeInformation -Encoding utf8
$taskPass=@($taskRows|Where-Object {$_.result -eq 'PASS'}).Count
"MEASURE attempted=12 PASS=$taskPass FAIL=$(12-$taskPass); entry fixtures, actual input exits; see each original result/game.log" | Set-Content -LiteralPath (Join-Path $taskOutput 'summary.txt') -Encoding utf8
Get-Content -LiteralPath (Join-Path $taskOutput 'summary.txt')
if($taskPass -ne 12){exit 1}
