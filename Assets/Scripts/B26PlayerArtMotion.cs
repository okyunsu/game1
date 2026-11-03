using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(800)]
public sealed class B26PlayerArtMotion:MonoBehaviour
{
 PlayerMotor motor;PlayerDash dash;Rigidbody2D body;SpriteRenderer image;B25ArtSettings settings;Vector3 baseline;bool grounded,wasDash;double animated=-100,dashBegan;float vertical,stretch;int inDash;
 public int JumpEvents{get;private set;}public int LandingEvents{get;private set;}public int Afterimages{get;private set;}
 public void Initialize(GameObject actor,B25ArtSettings shared){motor=actor.GetComponent<PlayerMotor>();dash=actor.GetComponent<PlayerDash>();body=actor.GetComponent<Rigidbody2D>();image=GetComponent<SpriteRenderer>();settings=shared;baseline=transform.localScale;grounded=motor.Grounded;}
 void LateUpdate()
 {
  if(motor==null||GameState.GetOrCreate().Paused)return;double now=Time.timeAsDouble;image.flipX=motor.Facing<0;
  if(!grounded&&motor.Grounded){animated=now;stretch=-.15f;LandingEvents++;Debug.Log($"MEASURE B26 player landing seconds={settings.playerMotionSeconds:F3}");}
  else if(!motor.Grounded&&body.linearVelocity.y>2&&(grounded||vertical<=0)){animated=now;stretch=.12f;JumpEvents++;Debug.Log($"MEASURE B26 player jump seconds={settings.playerMotionSeconds:F3}");}
  float t=Mathf.Clamp01(1-(float)(now-animated)/settings.playerMotionSeconds);transform.localScale=new Vector3(baseline.x*(1-stretch*t),baseline.y*(1+stretch*t),baseline.z);grounded=motor.Grounded;vertical=body.linearVelocity.y;
  if(dash.IsDashing&&!wasDash){dashBegan=now;inDash=0;}
  if(dash.IsDashing&&inDash<3&&now-dashBegan>=inDash*motor.Tuning.dashDuration/3)
  {
   var go=new GameObject("B26 dash afterimage");SceneManager.MoveGameObjectToScene(go,gameObject.scene);go.transform.SetPositionAndRotation(transform.position,transform.rotation);go.transform.localScale=transform.lossyScale;var sr=go.AddComponent<SpriteRenderer>();sr.sprite=image.sprite;sr.flipX=image.flipX;sr.color=new Color(1,1,1,.35f);sr.sortingOrder=image.sortingOrder-1;go.AddComponent<PresentationFragment>().Initialize(.12f,Vector2.zero);inDash++;Afterimages++;Debug.Log("MEASURE B26 player dash afterimage="+inDash+"/3");
  }
  wasDash=dash.IsDashing;
 }
}
