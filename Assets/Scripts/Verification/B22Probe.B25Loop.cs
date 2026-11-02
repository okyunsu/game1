#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B25LateDashLeft(float takeoff,float destination,string label)
 {
  Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=takeoff,"late dash takeoff "+label,3);
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+5;bool rose=false,dashed=false,land=false;
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   var keys=new List<Key>{Key.LeftArrow};if(Time.timeAsDouble-began<.45)keys.Add(Key.Z);rose|=Body.linearVelocity.y>0;
   if(rose&&!dashed&&Body.linearVelocity.y<=.6f){dashed=true;keys.Add(Key.LeftShift);}Input(keys.ToArray());
   if(rose&&Motor.Grounded&&Body.position.x<=destination+.5f&&Math.Abs(Body.position.y-1)<.12f){land=true;break;}if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
  }
  yield return Keys();Record($"MEASURE late dash {label} success={land} pos={Body.position:F3} HP={Health.HP}");Check(land,"actual late dash "+label);
 }
 IEnumerator B25Loop()
 {
  double began=Time.timeAsDouble;yield return B24Loop();Record("PASS B25 stage A-loop/Continue");
  // B02 uses its approved upper branch. The bottom-route failure remains a separate T7 result.
  yield return GoX(44.8f);yield return LiftRide(true);
  yield return JumpTo(43.75f,36.7f,13,"practice1",true);yield return JumpTo(35.25f,28.1f,13,"practice2",true);yield return JumpTo(26.75f,20.8f,15,"practice3 shelf",true);
  yield return GoX(4.7f);Input(Key.LeftArrow,Key.Z);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="B01","B25 physical ceiling link",3);yield return Keys();yield return Ready();Record("PASS B25 stage A03-B01");
  yield return JumpTo(12.5f,16.3f,3,"B01 market stall");yield return GoX(31.8f);yield return JumpTo(31.8f,34.5f,3,"B01 crate");yield return JumpTo(35.5f,38.5f,5,"B01 roof");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B02",Key.RightArrow,"B25 B01-B02",5);yield return Ready();Record("PASS B25 stage B01 checkpoint/roof-B02");
  yield return B25Jet(11.7f,8.5f,15,8,"B02 loop upper");yield return GoX(31);yield return JumpTo(31.5f,41.2f,1,"B02 upper E3/acid departure",true);yield return JumpTo(43.5f,50.5f,1,"B02 E1 exit",true);yield return GoX(61);
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.RightArrow,"B25 B02-B03",3);yield return Ready();Record("PASS B25 stage B02 upper-B03");
  yield return B25Jet(13.6f,8.5f,10,8,"B03 loop lower");yield return B25Jet(7.7f,17.5f,4.3f,17,"B03 loop upper");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B04",Key.LeftArrow,"B25 B03-B04",4);yield return Ready();
  yield return JumpTo(14.5f,12,2,"B04 loop wall pickup");yield return Until(()=>Motor.hasWallJump,"B25 actual wall jump",2);yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B25 B04 departure",3);
  Check(Boss.Introducing&&Boss.encounterGate.activeSelf,"B25 B04 locks on departure");yield return WalkTo(()=>Body.position.x<=2.5f,Key.LeftArrow,"B25 B04 wall approach",4);yield return Until(()=>!Boss.Introducing,"B25 B04 intro ends",3);yield return B25FightInput();Record("PASS B25 stage B04 actual victory");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.RightArrow,"B25 opened B04 exit",6);yield return Ready();yield return GoX(15.8f);Input();yield return Until(()=>Motor.Grounded&&Body.position.y<2,"B03 lower chimney access",4);yield return GoX(20.7f);
  yield return B25WallRise(Key.RightArrow,()=>RoomSession.Instance.CurrentRoomId=="B06","B25 actual B03-B06");yield return Ready();Record("PASS B25 stage B03 chimney-B06");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.LeftArrow,"B25 B06 physical return",4);yield return Ready();yield return Keys();yield return Until(()=>Motor.Grounded&&Body.position.y<2,"B03 chimney descent",5);
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B02",Key.LeftArrow,"B25 B03-B02 return",7);yield return Ready();Record("PASS B25 stage B06-B03-B02; B06 challenge remains separate T8");
  yield return JumpTo(55,47.5f,1,"B02 return E1",true);yield return GoX(40.4f);yield return B25LateDashLeft(40.1f,33,"B02 return acid6");yield return GoX(23.8f);yield return B25LateDashLeft(21.65f,13.8f,"B02 return acid8");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B01",Key.LeftArrow,"B25 B02-B01 final",7);yield return Ready();yield return GoX(22);
  Check(ProgressSave.TryLoad(out var saved)&&saved.dash&&saved.wallJump&&saved.defeatedBossIds.Contains("B1-A04")&&saved.defeatedBossIds.Contains("B2-B04"),"B25 loop save owns both abilities and boss IDs");
  yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="B01"&&Motor.hasWallJump&&Player.GetComponent<PlayerDash>().hasDash,"B25 final Continue restores B01/dash/wall jump");Record($"MEASURE B25 expanded loop seconds={Time.timeAsDouble-began:F3} complete=True");
 }
}
#endif
