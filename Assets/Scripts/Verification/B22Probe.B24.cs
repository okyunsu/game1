#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator HeadTrial(string label,Vector2 start,float edge,Rect target,bool right,bool inPlace=false)
 {
  yield return ResetAt(start);Key dir=right?Key.RightArrow:Key.LeftArrow;
  if(!inPlace){Input(dir);yield return Until(()=>right?Body.position.x>=edge-.4f:Body.position.x<=edge+.4f,"head runup "+label,2);}
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+2;bool airborne=false,land=false,head=false;
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   var keys=new List<Key>();if(Time.timeAsDouble-began<.43f)keys.Add(Key.Z);
   if(!inPlace&&Mathf.Abs(Body.position.x-target.center.x)>.1f)keys.Add(Body.position.x<target.center.x?Key.RightArrow:Key.LeftArrow);
   Input(keys.ToArray());airborne|=Body.linearVelocity.y>0;
   var contacts=new ContactPoint2D[16];int n=Player.GetComponent<Collider2D>().GetContacts(contacts);
   for(int i=0;i<n;i++)if(contacts[i].normal.y<-.7f){if(!head)Record("MEASURE head blocker="+contacts[i].collider.name+" pos="+Body.position);head=true;}
   if(airborne&&Motor.Grounded&&Math.Abs(Body.position.y-target.y)<.08f&&Body.position.x>=target.xMin-.1f&&Body.position.x<=target.xMax+.1f){land=true;break;}
   if(airborne&&Body.position.y<start.y-1)break;yield return null;
  }
  yield return Keys();headOK=!head;landOK=land;Record($"MEASURE head trial={label} contact={head} landed={land} end={Body.position:F4}");
 }
 bool headOK,landOK;
 IEnumerator HeadFive(string label,Vector2 start,float edge,Rect target,bool right,bool inPlace=false)
 {
  int heads=0,lands=0;
  for(int i=1;i<=5;i++){if(Room.GetComponentsInChildren<CrumblePlatform>().Length>0)yield return new WaitForSeconds(3.8f);yield return HeadTrial(label+"#"+i,start,edge,target,right,inPlace);if(!headOK)heads++;if(landOK)lands++;}
  Record($"MEASURE head summary={label} contacts={heads}/5 landings={lands}/5");headAll&=heads==0&&lands==5;
 }
 bool headAll=true;
 IEnumerator B24Head()
 {
  NoAbilities();yield return RoomAt("A03");
  yield return HeadFive("A03-box-M",new Vector2(23.9f,3.015f),23,new Rect(18.7f,5,3,0),false);
  yield return HeadFive("A03-M-chimney",new Vector2(19.4f,5.015f),18.5f,new Rect(16,7,2.5f,0),false);
  yield return HeadFive("A03-chimney-W",new Vector2(16.9f,7.015f),16,new Rect(11,9,4,0),false);
  yield return HeadFive("A03-under-eave",new Vector2(9,1.015f),9,new Rect(8.8f,1,.4f,0),true,true);
  yield return RoomAt("A02");
  yield return HeadFive("A02-block-P0",new Vector2(6.1f,3.015f),7,new Rect(9.8f,5,1.8f,0),true);
  yield return HeadFive("A02-P0-P1",new Vector2(10.7f,5.015f),11.6f,new Rect(13.4f,7,1.8f,0),true);
  yield return HeadFive("A02-P1-P2",new Vector2(14.3f,7.015f),15.2f,new Rect(17,9,4,0),true);
  yield return HeadFive("A02-shard-P3",new Vector2(17.9f,9.015f),16.15f,new Rect(12.9f,10.5f,1.3f,0),false);
  yield return HeadFive("A02-shard-P4",new Vector2(13.3f,10.515f),14.2f,new Rect(16.2f,12.4f,3,0),true);
  yield return RoomAt("A04");
  yield return HeadFive("A04-block-P0",new Vector2(6.6f,3.015f),7.5f,new Rect(9.3f,5,2.4f,0),true);
  yield return HeadFive("A04-P0-crumble",new Vector2(10.8f,5.015f),11.7f,new Rect(13.5f,7,2,0),true);
  yield return RoomAt("A06");
  yield return HeadFive("A06-block-P1",new Vector2(4.6f,3.015f),5.5f,new Rect(7.3f,5,3.5f,0),true);
  yield return HeadFive("A06-P1-P0",new Vector2(8.2f,5.015f),7.3f,new Rect(1,7,4.5f,0),false);
  yield return HeadFive("A06-east-P3",new Vector2(38.6f,3.015f),39.5f,new Rect(41.3f,5,3.1f,0),true);
  Check(headAll,"all selected head approaches contact0 landing5/5");
 }
 IEnumerator B24HeadShard(){NoAbilities();yield return RoomAt("A02");yield return HeadFive("A02-shard-P3-coyote",new Vector2(17.9f,9.015f),16.15f,new Rect(12.9f,10.5f,1.3f,0),false);Check(headAll,"shard approach contact0 landing5/5");}
 IEnumerator PogoIslandRoute()
 {
  int before=Player.GetComponent<PlayerAttack>().PogosPerformed,returns=Hazard.Returns;double nextAttack=0,end=Time.realtimeSinceStartupAsDouble+12;bool success=false;
  Input(Key.RightArrow);yield return Until(()=>Body.position.x>=13.9f,"pogo runup",3);Input(Key.RightArrow,Key.Z);
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   int target=Mathf.Min(2,Player.GetComponent<PlayerAttack>().PogosPerformed-before);float x=target==0?15.8f:20.3f,dx=x-Body.position.x;
   var keys=new List<Key>{Key.Z,Key.DownArrow};if(Mathf.Abs(dx)>.15f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   float top=target==0?2.8f:3.1f,allowance=target==0?.4f:.7f;
   if(target==0&&Time.realtimeSinceStartupAsDouble>=nextAttack&&!Motor.Grounded&&Body.linearVelocity.y<0&&Math.Abs(dx)<.7f&&Player.GetComponent<Collider2D>().bounds.min.y<=top+allowance){keys.Add(Key.X);nextAttack=Time.realtimeSinceStartupAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03;}
   Input(keys.ToArray());if(Motor.Grounded&&Body.position.x>=19.3f&&Body.position.x<=21.3f&&target>=1){success=true;break;}if(Hazard.Returns>returns)break;yield return null;
  }
  yield return Keys();Record($"MEASURE loop A04 lower route pogos={Player.GetComponent<PlayerAttack>().PogosPerformed-before} landed={success} hp={Health.HP}");Check(success,"actual lower pogo route reaches island");
 }
 IEnumerator HubBuildings()
 {
  yield return JumpTo(26,24.4f,3,"A03 box");yield return JumpTo(23.3f,20.5f,5,"A03 M building");yield return JumpTo(18.9f,17.3f,7,"A03 chimney");yield return JumpTo(16.3f,13.4f,9,"A03 W roof");
 }
 IEnumerator B24Loop()
 {
  Check(RoomSession.Instance.CurrentRoomId=="A03","loop starts CP-A03");NoAbilities();double began=Time.timeAsDouble;
  Record("FIXTURE initial CP only; subsequent route uses actual inputs, no HP/pose/enemy/boss/ability writes. A04 uses its lower pogo branch, upper E1 stays live.");
  yield return JumpTo(42.3f,44.8f,1,"board low lift");yield return LiftRide(true);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A04",Key.RightArrow,"A04 physical door",5);yield return Ready();
  yield return JumpTo(4.2f,5.8f,3,"A04 block");yield return GoX(10.8f);yield return PogoIslandRoute();yield return JumpTo(20.8f,24.7f,1,"A04 island exit");yield return GoX(27.5f);yield return Until(()=>Boss!=null&&Boss.Fighting,"arena trigger",4);yield return Fight();
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A05",Key.RightArrow,"opened boss floor",8);yield return Ready();yield return JumpTo(40,37.5f,2.6f,"dash shrine");yield return Until(()=>Player.GetComponent<PlayerDash>().hasDash,"dash pickup",3);
  yield return JumpTo(30.6f,24.5f,1,"A05 sealed floor crossing",true);yield return JumpTo(24.5f,18.0f,1,"A05 E2 dash avoidance",true);yield return GoX(15.5f);yield return JumpTo(12.3f,4.6f,1,"A05 trench",true);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A03",Key.LeftArrow,"A03 physical return",5);yield return Ready();
  yield return GoX(26);yield return HubBuildings();yield return GoX(44.8f);yield return LiftRide(true);
  yield return JumpTo(43.75f,36.7f,13,"practice1",true);yield return JumpTo(35.25f,28.1f,13,"practice2",true);yield return JumpTo(26.75f,20.8f,15,"practice3 shelf",true);
  Check(ProgressSave.TryLoad(out var saved)&&saved.dash&&saved.defeatedBossIds.Contains("B1-A04"),"loop save has dash and B1");yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A03"&&Player.GetComponent<PlayerDash>().hasDash&&ProgressSave.IsBossDefeated("B1-A04"),"Continue restores CP-A03 dash and B1");Record($"MEASURE B24 loop seconds={Time.timeAsDouble-began:F3} complete=True");if(N5Args.Has("-b24-revisit"))yield return A01Revisit();
 }
}
#endif
