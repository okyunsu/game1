using UnityEngine;
public sealed class V4ExitRule:MonoBehaviour
{
 public string gate;
 public SealFloor elevatorSeal;
 public bool Allows(Collider2D other){if(!GetComponent<RoomExit>().enabled)return false;var player=other.GetComponent<PlayerInputReader>();if(player==null)return false;if(GetComponentInParent<V4Room>().ArrivalSuppressed)return false;var motor=player.GetComponent<PlayerMotor>();if(gate=="Boss"&&!ProgressSave.BossDefeated||gate=="DJ"&&!motor.hasDoubleJump||gate=="WallJump"&&!motor.hasWallJump||gate=="Dash"&&!player.GetComponent<PlayerDash>().hasDash||gate=="Slam"&&player.GetComponent<PlayerSlam>()?.hasSlam!=true)return false;return elevatorSeal==null||elevatorSeal.Broken;}
}
