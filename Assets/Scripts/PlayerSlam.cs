using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMotor))]
public sealed class PlayerSlam:MonoBehaviour
{
 public MechanicsTuning tuning;
 public bool hasSlam;
 public bool Slamming{get;private set;}
 public int Landings{get;private set;}
 PlayerInputReader input;PlayerMotor motor;Rigidbody2D body;PlayerDash dash;
 bool pending;double recoverUntil;readonly Collider2D[] hits=new Collider2D[32];
 void Awake(){input=GetComponent<PlayerInputReader>();motor=GetComponent<PlayerMotor>();body=GetComponent<Rigidbody2D>();dash=GetComponent<PlayerDash>();}
 void OnEnable(){input.DashPressed+=Request;input.Cleared+=Cancel;GetComponent<PlayerHealth>().Hit+=Cancel;}
 void OnDisable(){input.DashPressed-=Request;input.Cleared-=Cancel;GetComponent<PlayerHealth>().Hit-=Cancel;Cancel();}
 public bool WantsInput=>hasSlam&&!motor.Grounded&&!dash.IsDashing&&input.Move.y<-.5f&&!input.InputLocked&&!GetComponent<PlayerHealth>().KnockbackLocked;
 void Request(){if(WantsInput)pending=true;}
 public void Cancel(){pending=false;Slamming=false;recoverUntil=0;}
 void Update(){if(Debug.isDebugBuild&&Keyboard.current?.f8Key.wasPressedThisFrame==true)hasSlam=true;}
 public bool Tick(bool grounded,double now)
 {
  if(input.InputLocked||GetComponent<PlayerHealth>().HP<=0){Cancel();return false;}
  if(pending){pending=false;if(hasSlam&&!grounded&&!dash.IsDashing){Slamming=true;GetComponent<PlayerAttack>().Cancel();motor.ClearWallState();}}
  if(Slamming)
  {
   if(grounded){Slamming=false;Landings++;recoverUntil=now+tuning.slamRecovery;Landing();}
   else{body.linearVelocity=new Vector2(0,-tuning.slamSpeed);return true;}
  }
  if(now<recoverUntil){body.linearVelocity=Vector2.zero;return true;}
  return false;
 }
 void Landing()
 {
  var bounds=GetComponent<BoxCollider2D>().bounds;
  int n=Physics2D.OverlapBox(new Vector2(bounds.center.x,bounds.min.y-.08f),new Vector2(tuning.slamWidth,.3f),0,new ContactFilter2D{useTriggers=true},hits);
  SlamPad pad=null;
  var damaged=new System.Collections.Generic.HashSet<IDamageable>();
  for(int i=0;i<n;i++){hits[i].GetComponent<SealFloor>()?.Break();pad??=hits[i].GetComponent<SlamPad>();var target=hits[i].GetComponentInParent<IDamageable>();if(target!=null&&damaged.Add(target))target.ReceiveHit(tuning.slamDamage,transform.position);}
  if(pad!=null){recoverUntil=0;pad.Bounce(motor);}
 }
}
