#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(250)]
public sealed class B21HazardReturn:MonoBehaviour
{
 public B21HazardReturnSettings settings;
 public int Returns{get;private set;}
 public bool Returning{get;private set;}
 public bool HasSafePoint{get;private set;}
 public Vector2 SafePoint{get;private set;}
 string room;float supported,fade;
 public double LastGetUpSeconds{get;private set;}
 PlayerInputReader input;
 Rigidbody2D body;BoxCollider2D shape;PlayerMotor motor;PlayerHealth health;
 readonly ContactPoint2D[] contacts=new ContactPoint2D[16];
 void Awake(){input=GetComponent<PlayerInputReader>();body=GetComponent<Rigidbody2D>();shape=GetComponent<BoxCollider2D>();motor=GetComponent<PlayerMotor>();health=GetComponent<PlayerHealth>();}
 void OnDisable(){StopAllCoroutines();if(input!=null)input.SetHazardLock(false);fade=0;if(Returning&&body!=null&&RoomSession.Instance?.Respawning!=true&&RoomSession.Instance?.Transitioning!=true)body.simulated=true;Returning=false;}
 void FixedUpdate()
 {
  var session=RoomSession.Instance;if(session==null||settings==null)return;
  if(room!=session.CurrentRoomId){room=session.CurrentRoomId;supported=0;HasSafePoint=false;}
  if(Returning){body.linearVelocity=Vector2.zero;return;}
  if(session.Transitioning||session.Respawning||health.HP<=0||GetComponent<PlayerInputReader>().InputLocked||!motor.Grounded){supported=0;return;}
  bool stable=false;int count=shape.GetContacts(new ContactFilter2D{useLayerMask=true,layerMask=1<<6,useTriggers=false},contacts);
  for(int i=0;i<count;i++)
  {
   if(contacts[i].normal.y<.7f)continue;var floor=contacts[i].collider==shape?contacts[i].otherCollider:contacts[i].collider;
   if(floor==null||floor.GetComponentInParent<LeverLift>()!=null||floor.GetComponentInParent<CrumblePlatform>()!=null||floor.attachedRigidbody!=null&&floor.attachedRigidbody.bodyType!=RigidbodyType2D.Static)continue;
   stable=true;
  }
  if(stable)
  {
   foreach(var hazard in FindObjectsByType<DamageBlock>(FindObjectsSortMode.None))
   {
    var collider=hazard.GetComponent<Collider2D>();if(collider==null||!collider.enabled)continue;var h=collider.bounds;var b=shape.bounds;
    if(b.min.y>=h.max.y+settings.edgeMargin||h.min.y>b.max.y+settings.edgeMargin)continue;
    float gap=Mathf.Max(0,h.min.x-b.max.x,b.min.x-h.max.x);if(gap<settings.edgeMargin){stable=false;break;}
   }
  }
  if(!stable){supported=0;return;}supported+=Time.fixedDeltaTime;
  if(supported+1e-6f>=settings.safeGroundSeconds){SafePoint=body.position;HasSafePoint=true;}
 }
 public void AfterHazardDamage(){if(!Returning&&HasSafePoint&&health.HP>0&&settings!=null)StartCoroutine(Return());}
 IEnumerator Return()
 {
  Returning=true;string origin=room;Vector2 point=SafePoint;input.SetHazardLock(true);body.simulated=false;body.linearVelocity=Vector2.zero;
  float before=Mathf.Max(settings.pauseSeconds,settings.fadeSeconds);double began=Time.timeAsDouble;
  while(Time.timeAsDouble-began<before){fade=settings.fadeSeconds>0?Mathf.Clamp01((float)(Time.timeAsDouble-began)/settings.fadeSeconds):1;yield return null;}
  if(RoomSession.Instance!=null&&RoomSession.Instance.CurrentRoomId==origin&&health.HP>0&&!RoomSession.Instance.Respawning&&!RoomSession.Instance.Transitioning)
  {
   body.position=point;transform.position=point;body.linearVelocity=Vector2.zero;motor.ClearTransientState();input.ClearTransientInput();Physics2D.SyncTransforms();Returns++;
   began=Time.timeAsDouble;
   while(Time.timeAsDouble-began<settings.getUpTime){fade=1-Mathf.Clamp01((float)(Time.timeAsDouble-began)/Mathf.Max(.001f,settings.fadeSeconds));yield return null;}
   LastGetUpSeconds=Time.timeAsDouble-began;
  }
  fade=0;if(RoomSession.Instance?.Respawning!=true&&RoomSession.Instance?.Transitioning!=true)body.simulated=true;input.SetHazardLock(false);Returning=false;supported=0;
 }
 void OnGUI(){if(fade<=0)return;var previous=GUI.color;GUI.depth=-100;GUI.color=new Color(0,0,0,fade);GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);GUI.color=previous;}
}
#endif
