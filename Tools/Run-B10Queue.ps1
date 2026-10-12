param([string]$Build='B10-release-r27',[string]$Mode='routes',[string]$Rooms,[string]$Tag,[int]$Fps=60)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskDesign=Get-Content (Join-Path $taskRoot 'Design/rooms_v2.json') -Raw | ConvertFrom-Json
foreach($taskRoom in $Rooms.Split(',')){
 $taskRoutes=@('')
 if($Mode -eq 'routes'){$taskRoutes=@($taskDesign.rooms.$taskRoom.routes.PSObject.Properties.Name)}
 foreach($taskRoute in $taskRoutes){
  if($Mode -eq 'routes' -and $Build -like 'B10-harness-r4*'){
   $taskPattern='^PASS route '+[regex]::Escape("$taskRoom/$taskRoute")+' replay=2'
   $taskQualified=Get-ChildItem (Join-Path $taskRoot 'Validation/_raw/B10') -Filter 'T2-resume*.txt' | Where-Object {$_.Name -notlike '*.live.txt'} | Select-String -Pattern $taskPattern | Select-Object -First 1
   if($taskQualified){Write-Output "SKIP resume qualified $taskRoom/$taskRoute evidence=$($taskQualified.Filename)";continue}
  }elseif(($taskRoom -eq 'B02' -and $taskRoute -eq 'low') -or ($taskRoom -eq 'A01' -and $taskRoute -eq 'slam')){if($Mode -eq 'routes'){continue}}
  if($Mode -eq 'routes' -and $Build -like 'B10-release-r3*' -and $taskRoom -notin @('A01','A05','B02')){
   $taskPattern='^PASS route '+[regex]::Escape("$taskRoom/$taskRoute")+' replay=2'
   $taskQualified=Get-ChildItem (Join-Path $taskRoot 'Validation/_raw/B10') -Filter '*.txt' | Where-Object {$_.Name -notlike '*.live.txt' -and ($_.Name -like 'T1-r11-*' -or $_.Name -like 'T2-r27*' -or $_.Name -like 'T2-*-r24.txt' -or $_.Name -like 'T2-*-r29b.txt' -or $_.Name -like 'T2-r30*')} | Select-String -Pattern $taskPattern | Select-Object -First 1
   if($taskQualified){Write-Output "SKIP qualified $taskRoom/$taskRoute evidence=$($taskQualified.Filename)";continue}
  }
  $taskName="$Tag-$taskRoom";if($taskRoute){$taskName+='-'+($taskRoute -replace '[^a-zA-Z0-9-]','-')}
  & (Join-Path $PSScriptRoot 'Run-B10Search.ps1') -Build $Build -Mode $Mode -Room $taskRoom -Route $taskRoute -Tag $taskName -Fps $Fps -Fast
 }
}
