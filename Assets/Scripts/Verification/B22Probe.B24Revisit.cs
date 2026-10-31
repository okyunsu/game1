#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator LateLeftJump(float takeoff,float x,float top)
 {
  Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=takeoff,"late jump takeoff",3);
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+3;bool rose=false,land=false;
  while(Time.realtimeSinceStartupAsDouble<end){var keys=new List<Key>();if(Time.timeAsDouble-began<.43)keys.Add(Key.Z);if(Math.Abs(Body.position.x-x)>.1f)keys.Add(Body.position.x<x?Key.RightArrow:Key.LeftArrow);Input(keys.ToArray());rose|=Body.linearVelocity.y>0;if(rose&&Motor.Grounded&&Math.Abs(Body.position.y-top)<.1f&&Math.Abs(Body.position.x-x)<.5f){land=true;break;}yield return null;}
  yield return Keys();Record($"MEASURE revisit late jump landed={land} pos={Body.position:F4}");Check(land,"actual coyote jump to A02 shard");
 }
 IEnumerator E4CrossRoute()
 {
  yield return GoX(42);yield return Until(()=>Motor.Grounded,"E4 takeoff ground",2);var e4=Room.GetComponentsInChildren<E4Hover>().Single();int hp=e4.HP,pogo=Player.GetComponent<PlayerAttack>().PogosPerformed;double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+10,next=0;bool landed=false;
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   bool bounced=Player.GetComponent<PlayerAttack>().PogosPerformed>pogo;float x=bounced?36.5f:e4.transform.position.x,dx=x-Body.position.x;var keys=new List<Key>{Key.Z,Key.DownArrow};if(Math.Abs(dx)>.1f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(!bounced&&Time.timeAsDouble-began>.25&&Math.Abs(dx)<1.25f&&Time.timeAsDouble>=next){keys.Add(Key.X);next=Time.timeAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.02;}
   Input(keys.ToArray());if(bounced&&Motor.Grounded&&Body.position.x>=35.5f&&Body.position.x<=37.5f){landed=true;break;}if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
  }
  yield return Keys();Record($"MEASURE revisit E4 pogos={Player.GetComponent<PlayerAttack>().PogosPerformed-pogo} E4HP={hp}->{e4.HP} crossed={landed} playerHP={Health.HP}");Check(landed,"actual E4 pogo crosses spike pit");
 }
 IEnumerator A01Revisit()
 {
  double began=Time.timeAsDouble;Record("PASS revisit begins after actual core-loop Continue; no setup writes");
  yield return GoX(26);yield return HubBuildings();yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A02",Key.LeftArrow,"upper A03-A02 door",7);yield return Ready();
  yield return JumpTo(23.8f,19.8f,9,"A02 upper enemy",true);yield return GoX(17.9f);yield return LateLeftJump(16.55f,13.55f,10.5f);
  var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderBy(r=>r.transform.position.y).First();Check(DiscoveryReward.Found(shard.candidateId),"actual A02 shard collected on return");
  yield return GoX(8);yield return JumpTo(8,5.8f,3,"A02 exit block");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A01",Key.LeftArrow,"A02-A01 physical door",8);yield return Ready();
  yield return E4CrossRoute();yield return GoX(8);yield return JumpTo(8,5.8f,3,"A01 block");yield return JumpTo(6.65f,10.7f,4.8f,"A01 P0");yield return JumpTo(12.45f,15,6.8f,"A01 P1");yield return JumpTo(15.2f,21.7f,6.8f,"A01 upper enemy",true);
  yield return JumpTo(22.25f,25.4f,8.8f,"A01 crumble1");yield return JumpTo(26.05f,29.2f,10.5f,"A01 crumble2");yield return JumpTo(29.85f,34,11.6f,"A01 high shelf");yield return GoX(36.5f);
  yield return JumpTo(37.15f,44.5f,11.6f,"A01 dash vault revisit",true);yield return GoX(45.2f);Check(Room.GetComponentsInChildren<DiscoveryReward>(true).All(r=>DiscoveryReward.Found(r.candidateId)),"A01 high and vault fragments collected");
  yield return GoX(42);yield return Until(()=>Motor.Grounded,"drop to east floor",3);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A02",Key.RightArrow,"A01-A02 return",5);yield return Ready();
  yield return JumpTo(3.5f,5.8f,3,"A02 return block");yield return JumpTo(10.4f,15,1,"A02 spike return",true);yield return JumpTo(17,24.5f,1,"A02 E2 return",true);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A03",Key.RightArrow,"A02-A03 return",6);yield return Ready();
  Record($"MEASURE A01 revisit seconds={Time.timeAsDouble-began:F3} complete=True hp={Health.HP}");
 }
}
#endif
