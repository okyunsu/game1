using UnityEngine;
// Predicts E1 contact and reads existing E2/E3 tells. Never changes AI, damage or collision.
[DefaultExecutionOrder(700)]public sealed class B25EnemyTell:MonoBehaviour
{
 public PresentationProfile profile;
 public bool Bright{get;private set;}
 public float LeadSeconds{get;private set;}
 public int Activations{get;private set;}
 SpriteRenderer image;Color ordinary;EnemyPatrol patrol;EnemyCharger charger;EnemyTurret turret;Collider2D shape;
 void Awake(){patrol=GetComponent<EnemyPatrol>();charger=GetComponent<EnemyCharger>();turret=GetComponent<EnemyTurret>();shape=GetComponent<Collider2D>();}
 void LateUpdate()
 {
  if(profile==null)return;var art=GetComponent<B25ArtVisual>();var target=art!=null&&art.images.Length>0?art.images[0]:GetComponent<SpriteRenderer>();if(target!=image){image=target;ordinary=image.color;}
  float lead=float.PositiveInfinity;
  if(charger!=null&&charger.State==EnemyCharger.Phase.Warning)lead=Mathf.Max(0,charger.tuning.warningSeconds-charger.PhaseElapsed);
  else if(turret!=null&&turret.Warning)lead=turret.WarningTimeRemaining;
  else if(patrol!=null&&RoomSession.Instance?.Player!=null)
  {
   var player=RoomSession.Instance.Player;var b=player.GetComponent<Collider2D>().bounds;var a=shape.bounds;
   if(b.min.y<a.max.y&&a.min.y<b.max.y){float delta=b.center.x-a.center.x;float closing=(GetComponent<Rigidbody2D>().linearVelocity.x-player.GetComponent<Rigidbody2D>().linearVelocity.x)*Mathf.Sign(delta);if(closing>0)lead=Mathf.Max(0,(Mathf.Abs(delta)-a.extents.x-b.extents.x)/closing);}
  }
  LeadSeconds=lead;bool bright=lead<=profile.enemyTellLead;if(bright&&!Bright)Activations++;Bright=bright;float gain=bright?profile.enemyTellBrightness:1;image.color=new Color(ordinary.r*gain,ordinary.g*gain,ordinary.b*gain,image.color.a);
 }
 void OnDisable(){if(image!=null)image.color=new Color(ordinary.r,ordinary.g,ordinary.b,image.color.a);}
}
