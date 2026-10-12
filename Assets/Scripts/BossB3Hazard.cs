using UnityEngine;
// Runtime values are supplied exclusively by the owner's BossTuning asset.
public sealed class BossB3Hazard:MonoBehaviour
{
 public BossEncounter Owner{get;private set;}
 public Vector2 Velocity{get;private set;}
 public int Token{get;private set;}
 public bool Falling{get;private set;}
 Vector2 size;readonly Collider2D[] contacts=new Collider2D[16];
 public void Setup(BossEncounter owner,Vector2 velocity,Vector2 dimensions,int token,bool falling){Owner=owner;Velocity=velocity;size=dimensions;Token=token;Falling=falling;transform.localScale=new Vector3(size.x,size.y,1);}
 void FixedUpdate(){
  if(Owner==null||!Owner.Fighting){Destroy(gameObject);return;}
  var session=RoomSession.Instance;if(GameState.GetOrCreate().Paused||session==null||session.Transitioning||session.Respawning)return;
  Vector2 previous=transform.position,next=previous+Velocity*Time.fixedDeltaTime;
  // Sweep the whole physical tick so a projectile cannot tunnel between frames.
  Vector2 swept=size+new Vector2(Mathf.Abs(next.x-previous.x),Mathf.Abs(next.y-previous.y));
  int count=Physics2D.OverlapBox((previous+next)*.5f,swept,0,new ContactFilter2D{useTriggers=true},contacts);
  for(int i=0;i<count;i++){var health=contacts[i].GetComponent<PlayerHealth>();if(health!=null)Owner.TryPatternDamage(health,Token,next);}
  transform.position=next;
  if(Falling?next.y-size.y*.5f<=Owner.floorY:Velocity.x<0?next.x-size.x*.5f<=Owner.leftWallX:next.x+size.x*.5f>=Owner.rightWallX)Destroy(gameObject);
 }
}
