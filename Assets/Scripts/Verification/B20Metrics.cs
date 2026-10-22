#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[DefaultExecutionOrder(300)]
public sealed class B20Metrics:N5Runner
{
 [Serializable] public sealed class JumpData{public float full,shortJump,calculatedFull,calculatedShort,maxStep;}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Init()=>N5Args.Start<B20Metrics>("-verify-b20-metrics");
 const float X=2000,Y=1000;
 float jumpRise;bool landed;float bestStep;
 bool watchDash,dashEnded;int dashTicks;Vector2 dashStart,dashEnd;float dashMinY,dashMaxY;
 void FixedUpdate()
 {
  if(!watchDash||Player==null)return;
  if(Player.GetComponent<PlayerDash>().IsDashing)
  {
   if(dashTicks==0){dashStart=Body.position;dashMinY=dashMaxY=Body.position.y;}
   dashTicks++;dashMinY=Mathf.Min(dashMinY,Body.position.y);dashMaxY=Mathf.Max(dashMaxY,Body.position.y);
  }
  else if(dashTicks>0&&!dashEnded){dashEnd=Body.position;dashEnded=true;}
 }
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 void Record(string text){lines.Add(text);Debug.Log(text);}
 GameObject Box(string name,Vector2 at,Vector2 size){var go=new GameObject(name);go.layer=6;go.transform.position=at;go.AddComponent<BoxCollider2D>().size=size;return go;}
 IEnumerator Reset(Vector2 at){yield return Keys();Motor.ClearTransientState();Player.GetComponent<PlayerDash>().Cancel();PlaceForAuthorizedSetup(at);yield return new WaitForSeconds(.22f);}
 IEnumerator Jump(bool shortJump,int trial)
 {
  yield return Reset(new Vector2(X,Y+.015f));Check(Motor.Grounded,"jump supported "+trial);
  float start=Player.GetComponent<Collider2D>().bounds.min.y,max=start;double began=Time.realtimeSinceStartupAsDouble;bool launched=false,released=false;float held=0;
  Input(Key.Z);
  while(Time.realtimeSinceStartupAsDouble-began<1.5)
  {
   max=Mathf.Max(max,Player.GetComponent<Collider2D>().bounds.min.y);
   if(Body.linearVelocity.y>0)launched=true;
   if(shortJump&&launched&&!released){held=(float)(Time.realtimeSinceStartupAsDouble-began);Input();released=true;}
   if(launched&&Motor.Grounded)break;yield return null;
  }
  yield return Keys();Check(launched&&Motor.Grounded,"jump landed "+trial);jumpRise=max-start;
  Record("MEASURE jump kind="+(shortJump?"short":"full")+" trial="+trial+" rise="+jumpRise.ToString("F5")+" releaseAfter="+held.ToString("F5"));
 }
 IEnumerator StepTrial(GameObject ledge,float rise,int trial)
 {
  ledge.transform.position=new Vector2(X+9,Y+rise/2);ledge.GetComponent<BoxCollider2D>().size=new Vector2(8,rise);Physics2D.SyncTransforms();
  yield return Reset(new Vector2(X+3,Y+.015f));Input(Key.RightArrow,Key.Z);double until=Time.realtimeSinceStartupAsDouble+1.5;landed=false;
  while(Time.realtimeSinceStartupAsDouble<until){if(Motor.Grounded&&Body.position.x>X+5.4f&&Body.position.y>Y+rise-.05f){landed=true;break;}yield return null;}
  yield return Keys();Record("MEASURE ledge trial="+trial+" rise="+rise.ToString("F5")+" landed="+landed+" feet="+Body.position.ToString("F4"));
 }
 IEnumerator Dash(bool air,int trial)
 {
  yield return Reset(new Vector2(X,Y+.015f));var dash=Player.GetComponent<PlayerDash>();dash.hasDash=true;
  if(air){Input(Key.Z);yield return new WaitForSeconds(.14f);yield return Keys();}
  Vector2 start=Body.position;float min=start.y,max=start.y;dashTicks=0;dashEnded=false;watchDash=true;Input(Key.RightArrow,Key.LeftShift);bool began=false;double until=Time.realtimeSinceStartupAsDouble+1;
  while(Time.realtimeSinceStartupAsDouble<until)
  {
   yield return new WaitForFixedUpdate();if(dash.IsDashing)began=true;
   if(began){min=Mathf.Min(min,Body.position.y);max=Mathf.Max(max,Body.position.y);}
   if(began&&!dash.IsDashing)break;
  }
  Check(began&&dashEnded,"dash actually began and ended");Vector2 end=Body.position;watchDash=false;yield return Keys();
  Record("MEASURE dash mode="+(air?"air":"ground")+" trial="+trial+" activeDx="+(dashEnd.x-dashStart.x).ToString("F5")+" activeDy="+(dashEnd.y-dashStart.y).ToString("F5")+" activeHeightRange="+(dashMaxY-dashMinY).ToString("F5")+" physicsTicks="+dashTicks+" requestWindowDx="+(end.x-start.x).ToString("F5")+" requestWindowDy="+(end.y-start.y).ToString("F5"));
  dash.hasDash=false;
 }
 IEnumerator Room(string id)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),"Entry"),"room fixture "+id);yield return Ready();}
  Health.RestoreMax();Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;yield return new WaitForSeconds(.5f);
 }
 IEnumerator Trench(bool jumping)
 {
  yield return Room("A05");yield return Reset(new Vector2(5,1.015f));int hp=Health.HP;double began=Time.realtimeSinceStartupAsDouble,next=0,held=0;float min=Body.position.y;bool died=false;
  while(Time.realtimeSinceStartupAsDouble-began<5&&Body.position.x<13)
  {
   if(RoomSession.Instance.Respawning||RoomSession.Instance.CurrentRoomId!="A05"){died=true;break;}
   double now=Time.realtimeSinceStartupAsDouble;if(jumping&&Motor.Grounded&&now>=next){held=now+.45;next=now+.65;}
   Input(jumping&&now<held?new[]{Key.RightArrow,Key.Z}:new[]{Key.RightArrow});min=Mathf.Min(min,Body.position.y);yield return null;
  }
  Record("MEASURE A05-trench mode="+(jumping?"ordinary-jumps":"walk-only")+" crossed="+(!died&&Body.position.x>=13)+" hp="+hp+"->"+Health.HP+" minFeet="+min.ToString("F4")+" position="+Body.position.ToString("F4")+" died="+died+" seconds="+(Time.realtimeSinceStartupAsDouble-began).ToString("F3"));
  yield return Keys();yield return Ready();
 }
 IEnumerator Hazard(string room,float x)
 {
  yield return Room(room);yield return Reset(new Vector2(x,1.2f));Health.RestoreMax();int hp=Health.HP;Vector2 start=Body.position;float minX=start.x,maxX=start.x;double until=Time.realtimeSinceStartupAsDouble+.8;bool respawn=false;
  while(Time.realtimeSinceStartupAsDouble<until){minX=Mathf.Min(minX,Body.position.x);maxX=Mathf.Max(maxX,Body.position.x);respawn|=RoomSession.Instance.Respawning;yield return null;}
  Record("MEASURE hazard room="+room+" hp="+hp+"->"+Health.HP+" start="+start.ToString("F4")+" end="+Body.position.ToString("F4")+" xRange="+minX.ToString("F4")+".."+maxX.ToString("F4")+" respawn="+respawn+" input=none");
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();
  yield return Trench(false);yield return Trench(true);yield return Hazard("A05",9);yield return Hazard("A04",24);
  Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
  var floor=Box("B20 flat metrics",new Vector2(X,Y-.5f),new Vector2(100,1));var data=new JumpData();
  var t=Motor.Tuning;data.calculatedFull=t.jumpVelocity*t.jumpVelocity/(2*t.gravity);data.calculatedShort=data.calculatedFull*t.jumpCutMultiplier*t.jumpCutMultiplier;
  Record("MEASURE jump-analytic full="+data.calculatedFull.ToString("F5")+" immediateCut="+data.calculatedShort.ToString("F5")+" jumpVelocity="+t.jumpVelocity+" gravity="+t.gravity+" cutMultiplier="+t.jumpCutMultiplier+" fixedDt="+Time.fixedDeltaTime);
  for(int i=1;i<=3;i++){yield return Jump(false,i);data.full=Mathf.Max(data.full,jumpRise);yield return Jump(true,i);data.shortJump=Mathf.Max(data.shortJump,jumpRise);}
  for(int i=1;i<=3;i++){yield return Dash(false,i);yield return Dash(true,i);}
  var ledge=Box("B20 ledge metrics",Vector2.zero,Vector2.one);
  for(int i=1;i<=3;i++)
  {
   float lo=2,hi=2.8f;yield return StepTrial(ledge,lo,i);Check(landed,"lower bound ledge");
   while(hi-lo>.013f){float mid=(lo+hi)/2;yield return StepTrial(ledge,mid,i);if(landed)lo=mid;else hi=mid;}
   bestStep=Mathf.Max(bestStep,lo);Record("MEASURE ledge-limit trial="+i+" success="+lo.ToString("F5")+" failure="+hi.ToString("F5")+" resolution="+(hi-lo).ToString("F5")+" gap=0 takeoffDistance=2 input=held-jump-right");
  }
  data.maxStep=bestStep;File.WriteAllText(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Evidence)),"T2-metrics.json"),JsonUtility.ToJson(data,true));
  Destroy(ledge);Destroy(floor);yield return Keys();
 }
}
#endif
