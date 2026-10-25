param(
 [Parameter(Mandatory=$true)][ValidatePattern('^[A-Za-z0-9-]+$')][string]$Batch,
 [switch]$SkipUnity,
 [string]$UnityPath,
 [ValidateRange(1,1800)][int]$TimeoutSeconds=300
)
# This script does not grant sandbox permissions, change Git, or repair licensing.
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskRun=Join-Path $taskRoot ('Validation/_raw/'+$Batch+'/preflight-'+[DateTime]::Now.ToString('yyyyMMdd-HHmmss-fff')+'-'+[Guid]::NewGuid().ToString('N').Substring(0,8))
New-Item -ItemType Directory -Path $taskRun | Out-Null
$taskLines=New-Object 'System.Collections.Generic.List[string]'
$taskResult=[ordered]@{batch=$Batch;project=$taskRoot;skipUnity=[bool]$SkipUnity;status='ERROR';exitCode=1;unityExitCode=$null;output=$taskRun}
$taskCode=1
function Write-Check([string]$Line){$taskLines.Add($Line);Write-Output $Line}
Push-Location -LiteralPath $taskRoot
# Git prints UTF-8 (Korean commit messages). Decode native output as UTF-8 and restore afterwards.
$taskOldEncoding=$null
try {$taskOldEncoding=[Console]::OutputEncoding;[Console]::OutputEncoding=New-Object System.Text.UTF8Encoding($false)} catch {$taskOldEncoding=$null}
try {
 Write-Check ('MEASURE project='+$taskRoot)
 Write-Check ('MEASURE LOCALAPPDATA='+$env:LOCALAPPDATA)
 $taskLocalExisting=![string]::IsNullOrWhiteSpace($env:LOCALAPPDATA) -and [IO.Path]::IsPathRooted($env:LOCALAPPDATA) -and (Test-Path -LiteralPath $env:LOCALAPPDATA -PathType Container)
 $taskSandboxLocal=$env:LOCALAPPDATA -match '(?i)[\\/]Packages[\\/]sandbox\.'
 $taskLocalNormal=$taskLocalExisting -and !$taskSandboxLocal
 Write-Check ('MEASURE localAppDataExistingAbsolute='+$taskLocalExisting+' sandboxLocalAppData='+$taskSandboxLocal+' expectedUsualSuffix=AppData\Local')
 if($taskSandboxLocal){Write-Check 'WARN sandbox LOCALAPPDATA detected; Unity requires approved outside-sandbox invocation'}
 $taskHub=@(Get-Process -Name 'Unity Hub' -ErrorAction SilentlyContinue)
 Write-Check ('MEASURE unityHubProcesses='+$taskHub.Count+' informationalOnly=True')
 $taskStatus=@(& git status --short 2>&1)
 if($LASTEXITCODE -ne 0){throw ('git status failed: '+($taskStatus -join ' '))}
 Write-Check ('MEASURE workingTreeEntries='+$taskStatus.Count)
 foreach($taskLine in $taskStatus){Write-Check ('GIT '+$taskLine)}
 $taskHead=@(& git rev-parse --short HEAD 2>&1)
 if($LASTEXITCODE -ne 0){throw ('git HEAD failed: '+($taskHead -join ' '))}
 $taskPending=@(& git log --oneline origin/main..HEAD 2>&1)
 if($LASTEXITCODE -ne 0){throw ('local origin/main reference unavailable: '+($taskPending -join ' '))}
 Write-Check ('MEASURE HEAD='+($taskHead -join '')+' localOriginPendingCommits='+$taskPending.Count+' fetched=False')
 foreach($taskLine in $taskPending){Write-Check ('PENDING '+$taskLine)}
 if($SkipUnity){
  $taskResult.status='SKIP_UNITY';$taskCode=0
  Write-Check 'PASS non-Unity preflight; Unity startup NOT RUN'
 } else {
  if(!$taskLocalNormal){throw 'LOCALAPPDATA missing, relative, nonexistent, or sandbox redirected; Unity NOT RUN'}
  # A live Editor can hold this project lock. Never terminate an existing Editor.
  $taskEditors=@(Get-CimInstance Win32_Process -Filter "Name = 'Unity.exe'" -ErrorAction Stop)
  $taskSame=@($taskEditors | Where-Object {
   if(!$_.CommandLine){throw 'Cannot inspect existing Unity command line; Unity NOT RUN'}
   if($_.CommandLine -match '(?i)-projectPath\s+(?:"([^"]+)"|(\S+))'){
    $taskOpen=if($Matches[1]){$Matches[1]}else{$Matches[2]}
    [IO.Path]::GetFullPath($taskOpen).TrimEnd('\','/') -eq [IO.Path]::GetFullPath($taskRoot).TrimEnd('\','/')
   }
  })
  if($taskSame.Count){
   $taskResult.status='PROJECT_OPEN';$taskCode=3
   Write-Check 'FAIL same-project Unity already open; startup NOT RUN; non-Unity tasks may continue'
  } else {
   if(!$UnityPath){
    $taskVersionLine=Get-Content -LiteralPath (Join-Path $taskRoot 'ProjectSettings/ProjectVersion.txt') | Where-Object {$_ -match '^m_EditorVersion: ' } | Select-Object -First 1
    if(!$taskVersionLine){throw 'Project Unity version missing'}
    $taskVersion=($taskVersionLine -split ': ',2)[1].Trim()
    $UnityPath=Join-Path $env:ProgramFiles ('Unity/Hub/Editor/'+$taskVersion+'/Editor/Unity.exe')
   }
   if(!(Test-Path -LiteralPath $UnityPath -PathType Leaf)){throw ('Unity executable missing: '+$UnityPath)}
   $taskUnityLog=Join-Path $taskRun 'unity.log'
   if($taskRoot.Contains('"') -or $taskUnityLog.Contains('"')){throw 'Unsupported quote in project/log path'}
   $taskArgs=@('-batchmode','-quit','-projectPath',('"'+$taskRoot+'"'),'-logFile',('"'+$taskUnityLog+'"'))
   Write-Check ('MEASURE unityExecutable='+$UnityPath+' timeoutSeconds='+$TimeoutSeconds)
   $taskProcess=Start-Process -FilePath $UnityPath -ArgumentList $taskArgs -WorkingDirectory $taskRoot -WindowStyle Hidden -PassThru
   if(!$taskProcess.WaitForExit($TimeoutSeconds*1000)){
    # Stop only the process tree this script started (Unity may spawn worker processes).
    try {& taskkill.exe /PID $taskProcess.Id /T /F 2>&1 | Out-Null} catch {}
    try {Stop-Process -Id $taskProcess.Id -ErrorAction SilentlyContinue} catch {}
    $taskResult.status='TIMEOUT';$taskCode=124
    Write-Check 'FAIL Unity startup timeout; only the spawned preflight process tree stopped; non-Unity tasks may continue'
   } else {
    $taskProcess.WaitForExit();$taskProcess.Refresh()
    $taskUnityExit=$taskProcess.ExitCode
    $taskResult.unityExitCode=$taskUnityExit
    Write-Check ('MEASURE unityExitCode='+$taskUnityExit)
    if($taskUnityExit -eq 0){$taskResult.status='PASS';$taskCode=0;Write-Check 'PASS Unity startup exit=0'}
    elseif($taskUnityExit -eq 198){$taskResult.status='LICENSE_BLOCKED';$taskCode=198;Write-Check 'FAIL Unity startup exit=198; Unity tasks BLOCKED(external dependency); do not retry unchanged environment; non-Unity tasks may continue'}
    else{$taskResult.status='UNITY_ERROR';$taskCode=1;Write-Check ('FAIL Unity startup exit='+$taskUnityExit+'; cause not classified as licensing; non-Unity tasks may continue')}
   }
   if(!(Test-Path -LiteralPath $taskUnityLog)){Write-Check 'WARN Unity log missing; see process exit result'}
  }
 }
} catch {
 $taskResult.status='ERROR';$taskCode=1
 Write-Check ('FAIL preflight '+$_.Exception.Message+'; non-Unity tasks may continue after checking their dependencies')
} finally {
 Pop-Location
 if($taskOldEncoding){try {[Console]::OutputEncoding=$taskOldEncoding} catch {}}
 $taskResult.exitCode=$taskCode
 $taskLines | Set-Content -LiteralPath (Join-Path $taskRun 'preflight.txt') -Encoding UTF8
 $taskResult | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $taskRun 'result.json') -Encoding UTF8
 Write-Output ('MEASURE evidence='+$taskRun)
}
exit $taskCode
