using UnityEngine;
public sealed class V4Room:MonoBehaviour
{
 public MechanicsTuning tuning;
 public bool testCourse;
 public Vector2 dimensions;
 public string designJson;
 public bool ArrivalSuppressed{get;private set;}
 Vector2 arrival;double began;
 public void Arrive(PlayerInputReader player){arrival=player.transform.position;began=Time.timeAsDouble;ArrivalSuppressed=true;var slam=player.GetComponent<PlayerSlam>();if(slam==null)slam=player.gameObject.AddComponent<PlayerSlam>();slam.tuning=tuning;slam.hasSlam=ProgressSave.HasSlam;if(testCourse){slam.hasSlam=true;player.GetComponent<PlayerDash>().hasDash=true;player.GetComponent<PlayerMotor>().hasWallJump=true;player.GetComponent<PlayerMotor>().hasDoubleJump=true;ProgressSave.Enabled=false;}if(GetComponent<RoomDefinition>().roomId=="S02"){ProgressSave.ReachedS02=true;ProgressSave.SaveCurrent(RoomSession.Instance);}}
 void Update(){var player=RoomSession.Instance?.Player;if(ArrivalSuppressed&&player!=null&&(Time.timeAsDouble-began>=.4f||Vector2.Distance(arrival,player.transform.position)>=1))ArrivalSuppressed=false;}
}
