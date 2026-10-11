using UnityEngine;
public sealed class LeverLift:MonoBehaviour
{
 public MechanicsTuning tuning;
 public string liftId;
 [Tooltip("Bottom/top platform surface, local scene u.")] public float low,high;
 public bool Unlocked{get;private set;}
 bool goingUp;float target;
 void Start(){Unlocked=ProgressSave.IsWallBroken(liftId+"-unlocked");goingUp=!ProgressSave.IsWallBroken(liftId+"-down");target=goingUp?high:low;SetY(target);}
 void SetY(float surface){var p=transform.position;p.y=surface-GetComponent<Collider2D>().bounds.extents.y;transform.position=p;}
 public void Call(bool upper){if(!Unlocked&&!upper)return;Unlocked=true;goingUp=upper?false:true;target=goingUp?high:low;ProgressSave.MarkWallBroken(liftId+"-unlocked",RoomSession.Instance);ProgressSave.SetWallFlag(liftId+"-down",!goingUp,RoomSession.Instance);}
 void FixedUpdate(){float current=GetComponent<Collider2D>().bounds.max.y,next=Mathf.MoveTowards(current,target,tuning.liftSpeed*Time.fixedDeltaTime);float delta=next-current;SetY(next);var player=RoomSession.Instance?.Player;if(player!=null){var shape=player.GetComponent<Collider2D>().bounds;var bounds=GetComponent<Collider2D>().bounds;if(Mathf.Abs(shape.min.y-current)<.12f&&shape.max.x>bounds.min.x&&shape.min.x<bounds.max.x){var body=player.GetComponent<Rigidbody2D>();body.position+=Vector2.up*delta;player.transform.position=body.position;}}}
}
