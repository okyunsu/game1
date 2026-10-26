#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

public sealed class B21CornerFix:MonoBehaviour
{
 public B21CornerFixSettings settings;
 public int Corrections{get;private set;}
 public float LastShift{get;private set;}
 public int DashCorrections{get;private set;}
 public float LastDashLift{get;private set;}
 Rigidbody2D body;BoxCollider2D shape;
 readonly RaycastHit2D[] hits=new RaycastHit2D[16];
 readonly Collider2D[] overlaps=new Collider2D[16];
 void Awake(){body=GetComponent<Rigidbody2D>();shape=GetComponent<BoxCollider2D>();}
 bool Clear(Vector2 center,Vector2 size,ContactFilter2D filter)
 {
  int count=Physics2D.OverlapBox(center,size,0,filter,overlaps);for(int i=0;i<count;i++)if(overlaps[i]!=shape){if(N5Args.Has("-b21-corner-trace"))Debug.Log("B21 corner blocked by="+overlaps[i].name+" center="+center+" size="+size+" obstacle="+overlaps[i].bounds);return false;}return true;
 }
 public bool TryAdjust(Vector2 velocity)
 {
  if(settings==null||velocity.y<=0||!body.simulated)return false;
  var filter=new ContactFilter2D{useLayerMask=true,layerMask=1<<6,useTriggers=false};var current=shape.bounds;
  float margin=settings.skin+2*Physics2D.defaultContactOffset;
  int count=shape.Cast(velocity.normalized,filter,hits,velocity.magnitude*Time.fixedDeltaTime+margin);int selected=-1;float nearest=float.PositiveInfinity;
  for(int i=0;i<count;i++)if(hits[i].normal.y<-.7f&&hits[i].collider.bounds.min.y>=current.max.y-settings.skin&&hits[i].distance<nearest){selected=i;nearest=hits[i].distance;}
  if(N5Args.Has("-b21-corner-trace")&&body.position.x>2030&&body.position.x<2050)Debug.Log("B21 corner trace feet="+body.position+" vy="+velocity.y+" count="+count+" selected="+selected+(count>0?" first="+hits[0].collider.name+" normal="+hits[0].normal:""));
  if(selected<0)return false;var ceiling=hits[selected].collider.bounds;var next=current;next.center+=(Vector3)(velocity*Time.fixedDeltaTime);
  float overlap=Mathf.Min(next.max.x,ceiling.max.x)-Mathf.Max(next.min.x,ceiling.min.x);
  if(overlap<=0||overlap>settings.maxOverlap)return false;
  float left=ceiling.min.x-next.max.x-margin,right=ceiling.max.x-next.min.x+margin;
  float shift=Mathf.Abs(left)<=Mathf.Abs(right)?left:right;
  var size=(Vector2)current.size-Vector2.one*(settings.skin*.5f);
  bool clearNow=Clear((Vector2)current.center+Vector2.right*shift,size,filter),clearNext=Clear((Vector2)next.center+Vector2.right*shift,size,filter);
  if(N5Args.Has("-b21-corner-trace"))Debug.Log("B21 corner candidate overlap="+overlap+" shift="+shift+" clearNow="+clearNow+" clearNext="+clearNext);
  if(!clearNow||!clearNext)return false;
  body.position+=Vector2.right*shift;transform.position=body.position;Physics2D.SyncTransforms();LastShift=shift;Corrections++;return true;
 }
 public bool TryDashAdjust(Collider2D obstacle,int direction)
 {
  if(settings==null||!body.simulated)return false;var current=shape.bounds;
  float overlap=obstacle.bounds.max.y-current.min.y;if(overlap<=0||overlap>settings.maxDashStep)return false;
  float lift=overlap+settings.skin+2*Physics2D.defaultContactOffset;
  var filter=new ContactFilter2D{useLayerMask=true,layerMask=1<<6,useTriggers=false};var size=(Vector2)current.size-Vector2.one*(settings.skin*.5f);
  var raised=(Vector2)current.center+Vector2.up*lift;var next=raised+Vector2.right*(direction*GetComponent<PlayerMotor>().Tuning.dashSpeed*Time.fixedDeltaTime);
  if(!Clear(raised,size,filter)||!Clear(next,size,filter))return false;
  body.position+=Vector2.up*lift;transform.position=body.position;Physics2D.SyncTransforms();LastDashLift=lift;DashCorrections++;return true;
 }
}
#endif
