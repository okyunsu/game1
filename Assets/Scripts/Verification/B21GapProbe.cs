#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B21GapProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21GapProbe>("-verify-b21-gaps");
 bool crossed;
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 IEnumerator Trial(string recipe,float gap,int trial)
 {
  yield return Keys();var dash=Player.GetComponent<PlayerDash>();dash.Cancel();Motor.ClearTransientState();Motor.hasDoubleJump=Motor.hasWallJump=false;dash.hasDash=recipe!="run-jump";
  FindFirstObjectByType<B21ArenaCourse>().Configure(gap);PlaceForAuthorizedSetup(new Vector2(-3.5f,.015f));yield return new WaitForSeconds(.3f);Check(Motor.Grounded,"arena supported");
  if(recipe!="ground-dash-jump")
  {
   Input(Key.RightArrow);yield return Until(()=>Body.position.x>=-.35f,"run-up",2);
   Input(recipe=="dash-only"?new[]{Key.RightArrow,Key.LeftShift}:new[]{Key.RightArrow,Key.Z});
  }
  else
  {
   Input(Key.RightArrow,Key.LeftShift);yield return Until(()=>dash.IsDashing,"ground dash began",1);yield return Until(()=>!dash.IsDashing,"ground dash ended",1);Input(Key.RightArrow,Key.Z);
  }
  Vector2 takeoff=Body.position;bool launched=false,airDash=false;crossed=false;double until=Time.realtimeSinceStartupAsDouble+2.5;
  while(Time.realtimeSinceStartupAsDouble<until)
  {
   launched|=Body.linearVelocity.y>0;
   if(recipe=="apex-dash-jump"&&launched&&!airDash&&Body.linearVelocity.y<=.6f){Input(Key.RightArrow,Key.Z,Key.LeftShift);airDash=true;}
   else if(airDash)Input(Key.RightArrow,Key.Z);
   if(Motor.Grounded&&Body.position.x>=gap+.2f&&Body.position.y>-.05f){crossed=true;break;}
   if(Body.position.y < -1)break;yield return null;
  }
  yield return Keys();lines.Add("MEASURE gap-attempt recipe="+recipe+" trial="+trial+" gap="+gap.ToString("F2")+" crossed="+crossed+" takeoff="+takeoff.ToString("F4")+" end="+Body.position.ToString("F4")+" apexDash="+airDash);
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="B21Arena","generated arena loaded");
  Check(Mathf.Abs(Player.GetComponent<Collider2D>().bounds.size.y-1.6f)<.001f,"baseline body1.6");
  foreach(string recipe in new[]{"run-jump","apex-dash-jump","ground-dash-jump","dash-only"})for(int trial=1;trial<=3;trial++)
  {
   int lo=10,hi=280;yield return Trial(recipe,lo*.05f,trial);Check(crossed,"lower gap bound "+recipe);yield return Trial(recipe,hi*.05f,trial);Check(!crossed,"upper gap bound "+recipe);
   while(hi-lo>1){int mid=(lo+hi)/2;yield return Trial(recipe,mid*.05f,trial);if(crossed)lo=mid;else hi=mid;}
   lines.Add("MEASURE gap-limit recipe="+recipe+" trial="+trial+" success="+(lo*.05f).ToString("F2")+" failure="+(hi*.05f).ToString("F2")+" resolution=0.05 body=1.6");
  }
 }
}
#endif
