param([string]$Output='Validation/U1-preservation.txt')
$ErrorActionPreference='Stop'
function Remove-Display([string]$Source){
 $taskStart=$Source.IndexOf('void OnGUI()')
 if($taskStart -lt 0){return $Source.Replace("`r`n","`n").TrimEnd("`r","`n")}
 $taskBrace=$Source.IndexOf('{',$taskStart);$taskDepth=1;$taskIndex=$taskBrace+1
 while($taskDepth -gt 0 -and $taskIndex -lt $Source.Length){if($Source[$taskIndex] -eq '{'){$taskDepth++};if($Source[$taskIndex] -eq '}'){$taskDepth--};$taskIndex++}
 return ($Source.Substring(0,$taskStart)+$Source.Substring($taskIndex)).Replace("`r`n","`n").TrimEnd("`r","`n")
}
$taskLines=@('Protection comparison vs approved baseline 80c9dbf. OnGUI-only exceptions explicitly allowed.')
foreach($taskFile in @('Assets/Scripts/PlayerHealth.cs','Assets/Scripts/PlayerInputReader.cs','Assets/Scripts/RoomLabel.cs','Assets/Scripts/DashPickup.cs','Assets/Scripts/DoubleJumpPickup.cs')){
 $taskBefore=(git show "80c9dbf:$taskFile") -join "`n"
 $taskAfter=[IO.File]::ReadAllText((Join-Path (Get-Location) $taskFile)).TrimEnd("`r","`n")
 if((Remove-Display $taskBefore) -ne (Remove-Display $taskAfter)){throw "Non-display modification $taskFile"}
 $taskLines+="PASS ${taskFile}: all code outside OnGUI unchanged"
}
$taskFiles=@('Assets/Scripts/PlayerMotor.cs','Assets/Scripts/PlayerDash.cs','Assets/Scripts/GameState.cs','Assets/Scripts/RoomSession.cs','Assets/Scripts/KillZone.cs','Assets/Scripts/RoomCameraRig.cs','Assets/Scripts/ProgressSave.cs','Assets/Scripts/PlayerAttack.cs','Assets/Scripts/EnemyPatrol.cs','Assets/Scripts/EnemyCharger.cs','Assets/Scripts/EnemyTurret.cs','Assets/Scripts/BossEncounter.cs','Assets/Scenes','Assets/Prefabs','Assets/ScriptableObjects','Packages','ProjectSettings')
$taskChanged=git diff --name-only 80c9dbf -- $taskFiles
if($taskChanged){throw "Protected files changed: $taskChanged"}
$taskLines+='PASS protected gameplay/progression/save/boss/enemy/camera/Scene/Prefab/existing Tuning/Packages/ProjectSettings diff0'
$taskLines+='New UITuning/GameHUD and opt-in verification only. Existing Validation never overwritten. No push.'
if(Test-Path -LiteralPath $Output){throw 'Preserve evidence'}
$taskLines | Set-Content -LiteralPath $Output
