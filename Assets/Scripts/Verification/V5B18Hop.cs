#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Measurement arena exists only in a flagged Development run. No Scene or tuning writes.
[DefaultExecutionOrder(200)]
public sealed class V5B18Hop:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Init()=>N5Args.Start<V5B18Hop>("-verify-b18-hop");
 const float X=2000,Y=1000;
 GameObject destination;bool passed;float apex;bool watchPogo;int pogoBefore;Vector2 pogoStart,pogoVelocity;
 GameObject Platform(string name,Vector2 center,Vector2 size){var go=new GameObject(name);go.layer=6;go.transform.position=center;go.AddComponent<BoxCollider2D>().size=size;return go;}
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 void FixedUpdate(){if(watchPogo&&Player!=null&&Player.GetComponent<PlayerAttack>().PogosPerformed>pogoBefore){pogoStart=Body.position;pogoVelocity=Body.linearVelocity;watchPogo=false;}}
 IEnumerator Trial(float rise,float gap,bool running)
 {
  destination.transform.position=new Vector2(X+gap+5,Y+rise-.5f);yield return Keys();Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(X-(running?2:.15f),Y+.015f));yield return new WaitForSeconds(.2f);
  if(running){Input(Key.RightArrow);double until=Time.realtimeSinceStartupAsDouble+1;while(Body.position.x<X-.15f&&Time.realtimeSinceStartupAsDouble<until)yield return null;}
  float takeoff=Body.position.x-X;float speed=Body.linearVelocity.x;Input(Key.RightArrow,Key.Z);double start=Time.realtimeSinceStartupAsDouble;passed=false;apex=Body.position.y-Y;
  while(Time.realtimeSinceStartupAsDouble-start<1.8)
  {
   apex=Mathf.Max(apex,Body.position.y-Y);
   if(Motor.Grounded&&Time.realtimeSinceStartupAsDouble-start>.12&&Body.position.x>X+gap&&Body.position.y>Y+rise-.05f){passed=true;break;}
   if(Body.position.y<Y-1)break;yield return null;
  }
  yield return Keys();lines.Add("MEASURE hop mode="+(running?"running":"standing")+" rise="+rise.ToString("F2")+" gap="+gap.ToString("F2")+" landed="+passed+" takeoffX="+takeoff.ToString("F3")+" takeoffVx="+speed.ToString("F3")+" apex="+apex.ToString("F3")+" end="+Body.position);
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
  var origin=Platform("B18 hop takeoff",new Vector2(X-5,Y-.5f),new Vector2(10,1));destination=Platform("B18 hop landing",Vector2.zero,new Vector2(10,1));
  foreach(bool running in new[]{false,true})foreach(float rise in new[]{0,1,1.5f,2,2.5f})
  {
   float best=-1,firstFailure=-1;int misses=0;
   for(float gap=0;gap<=8;gap+=.25f){yield return Trial(rise,gap,running);if(passed){best=gap;misses=0;}else{if(firstFailure<0)firstFailure=gap;misses++;if(misses>=2)break;}}
   lines.Add("MEASURE hop-limit mode="+(running?"running":"standing")+" rise="+rise.ToString("F2")+" maxGap="+(best<0?"none":best.ToString("F2"))+" firstFailure="+firstFailure.ToString("F2")+" grid=0.25 maxSearch=8 recipe=fixed-edge-held-jump");
  }
  Destroy(origin);Destroy(destination);var pogo=Platform("B18 one-pogo",new Vector2(X+3,Y+2),new Vector2(1,.5f));pogo.GetComponent<BoxCollider2D>().isTrigger=true;pogo.AddComponent<PogoTarget>();
  foreach(bool running in new[]{false,true})
  {
   yield return Keys();Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(X+3-(running?1.8f:0),Y+4));var attack=Player.GetComponent<PlayerAttack>();int before=attack.PogosPerformed;pogoBefore=before;watchPogo=true;double until=Time.realtimeSinceStartupAsDouble+3,next=0;
   while(attack.PogosPerformed==before&&Time.realtimeSinceStartupAsDouble<until){double now=Time.realtimeSinceStartupAsDouble;if(now>=next&&Body.position.y<=Y+3.5f&&Body.linearVelocity.y<0){Input(running?new[]{Key.RightArrow,Key.DownArrow,Key.X}:new[]{Key.DownArrow,Key.X});next=now+.4;}else Input(running?new[]{Key.RightArrow,Key.DownArrow}:new[]{Key.DownArrow});yield return null;}
   Check(attack.PogosPerformed==before+1&&!watchPogo,"single actual down-attack pogo "+running);Vector2 start=pogoStart;float max=Body.position.y;Input(Key.RightArrow);until=Time.realtimeSinceStartupAsDouble+2;
   while(Time.realtimeSinceStartupAsDouble<until){max=Mathf.Max(max,Body.position.y);if(Body.position.y<start.y&&Body.linearVelocity.y<0)break;yield return null;}
   lines.Add("MEASURE pogo-once mode="+(running?"running":"standing")+" count="+(attack.PogosPerformed-before)+" bodyRise="+(max-start.y).ToString("F3")+" apexAboveTarget="+(max-(Y+2.25f)).ToString("F3")+" horizontalToSameHeight="+(Body.position.x-start.x).ToString("F3")+" launchVelocity="+pogoVelocity);
  }
  Destroy(pogo);yield return Keys();
 }
}
#endif
