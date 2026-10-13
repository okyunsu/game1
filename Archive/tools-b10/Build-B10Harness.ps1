param([string]$Tag='B10-harness-r1')
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskSource=Join-Path $taskRoot 'Builds/B09-release-gates'
$taskTarget=Join-Path $taskRoot "Builds/$Tag"
if($Tag -notmatch '^B10-harness-[a-zA-Z0-9-]+$'){throw 'Harness tag required'}
if(Test-Path -LiteralPath $taskTarget){throw 'Preserve harness: use a new tag'}
New-Item -ItemType Directory -Path $taskTarget | Out-Null
Copy-Item -LiteralPath (Join-Path $taskSource 'Afterglow.exe'),(Join-Path $taskSource 'UnityPlayer.dll'),(Join-Path $taskSource 'UnityCrashHandler64.exe') -Destination $taskTarget
Copy-Item -LiteralPath (Join-Path $taskSource 'Afterglow_Data'),(Join-Path $taskSource 'MonoBleedingEdge') -Destination $taskTarget -Recurse
$taskManaged=Join-Path $taskTarget 'Afterglow_Data/Managed'
$taskRsp=Join-Path $taskRoot "Validation/_raw/B10/$Tag.rsp"
$taskLines=@('-noconfig','-nostdlib+','-target:library','-langversion:latest',('-out:"'+(Join-Path $taskManaged 'B10Tools.dll')+'"'))
$taskLines+=Get-ChildItem -LiteralPath $taskManaged -Filter '*.dll' | ForEach-Object {'-r:"'+$_.FullName+'"'}
$taskLines+=('"'+(Join-Path $taskRoot 'Assets/Scripts/Verification/B10JumpSearch.cs')+'"')
$taskLines | Set-Content -LiteralPath $taskRsp -Encoding utf8
$taskUnityData='C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data'
& "$taskUnityData/NetCoreRuntime/dotnet.exe" "$taskUnityData/DotNetSdkRoslyn/csc.dll" "@$taskRsp" 2>&1 | Tee-Object -FilePath (Join-Path $taskRoot "Validation/_raw/B10/$Tag-compile.txt")
if($LASTEXITCODE -ne 0){throw 'Harness compiler failed'}
$taskAssembliesPath=Join-Path $taskTarget 'Afterglow_Data/ScriptingAssemblies.json'
$taskAssemblies=Get-Content -LiteralPath $taskAssembliesPath -Raw | ConvertFrom-Json
$taskAssemblies.names+= 'B10Tools.dll';$taskAssemblies.types+=16
$taskAssemblies | ConvertTo-Json -Depth 5 -Compress | Set-Content -LiteralPath $taskAssembliesPath -Encoding utf8
$taskInitPath=Join-Path $taskTarget 'Afterglow_Data/RuntimeInitializeOnLoads.json'
$taskInit=Get-Content -LiteralPath $taskInitPath -Raw | ConvertFrom-Json
$taskInit.root+= [pscustomobject]@{assemblyName='B10Tools';nameSpace='';className='B10JumpSearch';methodName='Init';loadTypes=1;isUnityClass=$false}
$taskInit | ConvertTo-Json -Depth 5 -Compress | Set-Content -LiteralPath $taskInitPath -Encoding utf8
$taskHashes=Get-FileHash -LiteralPath (Join-Path $taskSource 'Afterglow_Data/Managed/Assembly-CSharp.dll'),(Join-Path $taskManaged 'Assembly-CSharp.dll')
$taskHashes | Format-List | Out-String | Set-Content (Join-Path $taskRoot "Validation/_raw/B10/$Tag-product-hash.txt")
if($taskHashes[0].Hash -ne $taskHashes[1].Hash){throw 'Product assembly must remain identical'}
Write-Output "PASS standalone verification assembly compiled; product assembly unchanged; harness=$taskTarget"
