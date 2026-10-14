#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public sealed class B09Measure:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09Measure>("-verify-b09-measure");
 protected override IEnumerator Tests()
 {
  yield return Ready();ProgressSave.Enabled=false;
  // Reuse the already tested four-unit chimney separately; measurements use an isolated floor.
  var floor=new GameObject("B09 measurement floor");floor.layer=6;floor.transform.position=new Vector2(-100,-.5f);floor.AddComponent<BoxCollider2D>().size=new Vector2(100,1);
  yield return Jump("standing",false,false,0);yield return Jump("running-flat",true,false,0);yield return Jump("running-rise1.8",true,false,1.8f);yield return Jump("dash",true,true,0);Motor.hasDoubleJump=true;yield return Jump("double",false,false,0,true);
  yield return Keys();PlaceForAuthorizedSetup(new Vector2(-110,.81f));yield return new WaitForSeconds(.2f);float start=Body.position.y,max=start;
  Motor.ApplyPogo(Player.GetComponent<PlayerAttack>().tuning.pogoVelocity,true);
  while(Body.linearVelocity.y>0){max=Mathf.Max(max,Body.position.y);yield return null;}
  lines.Add("MEASURE pogo-rise="+(max-start).ToString("F4")+" attack-height="+Player.GetComponent<PlayerAttack>().tuning.downAttackHeight);
  Check(max-start>=2.02f*.9f,"pogo within ten percent checker assumption");
  yield return Keys();
 }
 IEnumerator Jump(string label,bool run,bool dash,float rise,bool dj=false)
 {
  yield return Keys();Player.ClearTransientInput();PlaceForAuthorizedSetup(new Vector2(-120,.81f));yield return new WaitForSeconds(.2f);
  if(run)yield return Keys(Key.RightArrow);if(run)yield return new WaitForSeconds(.3f);
  Vector2 start=Body.position;float max=start.y,range=0;bool jumped=false,dashed=false,second=false;
  InputSystem.QueueStateEvent(keyboard,run?new KeyboardState(Key.RightArrow,Key.Z):new KeyboardState(Key.Z));
  double end=Time.timeAsDouble+3;
  while(Time.timeAsDouble<end)
  {
   float y=Body.position.y-start.y;max=Mathf.Max(max,Body.position.y);
   if(y>.1f)jumped=true;
   if(jumped&&Body.linearVelocity.y<=0&&y>=rise)range=Body.position.x-start.x;
   if(dash&&!dashed&&y>1.8f){dashed=true;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z,Key.C));}
   if(dj&&!second&&jumped&&Body.linearVelocity.y<=.7f){second=true;yield return Keys();yield return Keys(Key.Z);}
   if(jumped&&Motor.Grounded)break;
   yield return null;
  }
  lines.Add("MEASURE "+label+" peak="+(max-start.y).ToString("F4")+" range="+range.ToString("F4"));
  Check(jumped,"actual input jump "+label);
  if(label=="standing")Check(max-start.y>=2.4f*.9f,"standing within ten percent checker assumption");
  yield return Keys();
 }
}
#endif
