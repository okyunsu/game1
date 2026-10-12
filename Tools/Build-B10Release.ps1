param([Parameter(Mandatory=$true)][ValidatePattern('^B10-release-[a-zA-Z0-9-]+$')][string]$Tag)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
if(Test-Path (Join-Path $taskRoot "Builds/$Tag")){throw 'Preserve build: new tag required'}
$taskLog=Join-Path $taskRoot "Validation/_raw/B10/$Tag-editor.log"
if(Test-Path $taskLog){throw 'Preserve build log: new tag required'}
$taskProcess=Start-Process -FilePath 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe' -ArgumentList @('-batchmode','-nographics','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','B10Build.Run','-b10-build',$Tag,'-logFile',('"'+$taskLog+'"')) -WindowStyle Hidden -PassThru
$taskProcess.WaitForExit()
Write-Output "$Tag Unity exit=$($taskProcess.ExitCode)"
exit $taskProcess.ExitCode
