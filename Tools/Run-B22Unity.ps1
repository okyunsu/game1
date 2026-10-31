param([ValidatePattern('^[A-Za-z0-9-]+$')][string]$Batch='22',[Parameter(Mandatory=$true)][string]$Method,[Parameter(Mandatory=$true)][string]$Run,[string[]]$Extra=@(),[int]$TimeoutSeconds=600)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskOutput=Join-Path $taskRoot ('Validation/_raw/'+$Batch+'/'+$Run)
if(Test-Path -LiteralPath $taskOutput){throw "Preserve $taskOutput"}
New-Item -ItemType Directory -Path $taskOutput | Out-Null
$taskArgs=@('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$Method,'-verify-output',('"'+$taskOutput+'/result.txt"'),'-save-path',('"'+$taskOutput+'/save.json"'),'-logFile',('"'+$taskOutput+'/unity.log"'))+$Extra
$taskProcess=Start-Process 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe' -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
if(!$taskProcess.WaitForExit($TimeoutSeconds*1000)){Stop-Process -Id $taskProcess.Id;throw 'Unity timeout'}
$taskProcess.Refresh();Write-Output ('Unity exit='+$taskProcess.ExitCode+' evidence='+$taskOutput)
if(Test-Path -LiteralPath "$taskOutput/result.txt"){Get-Content -LiteralPath "$taskOutput/result.txt"}
Select-String -LiteralPath "$taskOutput/unity.log" -Pattern 'error CS|FAIL|MEASURE' | Select-Object -Last 20 | ForEach-Object {$_.Line}
exit $taskProcess.ExitCode
