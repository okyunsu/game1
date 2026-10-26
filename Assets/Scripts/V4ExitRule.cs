using UnityEngine;
public sealed class V4ExitRule:MonoBehaviour
{
 public string gate;
 public SealFloor elevatorSeal;
 [Tooltip("Persistent BreakableWall.wallId required to open this door. Empty preserves the existing gate rules.")] public string closedUntilWallBroken;
 public bool WallLocked=>!string.IsNullOrWhiteSpace(closedUntilWallBroken)&&!ProgressSave.IsWallBroken(closedUntilWallBroken);
 SpriteRenderer marker;Color openColor;BoxCollider2D doorway;int openLayer;bool openTrigger,managedWall;
 void Awake(){marker=GetComponent<SpriteRenderer>();if(marker!=null)openColor=marker.color;doorway=GetComponent<BoxCollider2D>();openLayer=gameObject.layer;openTrigger=doorway!=null&&doorway.isTrigger;ApplyWallState();}
 void LateUpdate()=>ApplyWallState();
 void ApplyWallState()
 {
  if(string.IsNullOrWhiteSpace(closedUntilWallBroken))
  {
   if(!managedWall)return;if(doorway!=null)doorway.isTrigger=openTrigger;gameObject.layer=openLayer;if(marker!=null)marker.color=openColor;managedWall=false;return;
  }
  managedWall=true;bool locked=WallLocked;
  if(doorway!=null)doorway.isTrigger=locked?false:openTrigger;
  gameObject.layer=locked?6:openLayer;if(marker!=null)marker.color=locked?Color.gray:openColor;
 }
 public bool Allows(Collider2D other){if(!GetComponent<RoomExit>().enabled)return false;var player=other.GetComponent<PlayerInputReader>();if(player==null)return false;if(GetComponentInParent<V4Room>().ArrivalSuppressed)return false;var motor=player.GetComponent<PlayerMotor>();if(gate=="Boss"&&!ProgressSave.BossDefeated||gate=="DJ"&&!motor.hasDoubleJump||gate=="WallJump"&&!motor.hasWallJump||gate=="Dash"&&!player.GetComponent<PlayerDash>().hasDash||gate=="Slam"&&player.GetComponent<PlayerSlam>()?.hasSlam!=true)return false;return !WallLocked&&(elevatorSeal==null||elevatorSeal.Broken);}
}
