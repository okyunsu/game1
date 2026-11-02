#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 bool b25Head;bool b25Watch;
 IEnumerator B25WatchHead(){var contacts=new ContactPoint2D[16];while(b25Watch){if(Player!=null){int n=Player.GetComponent<Collider2D>().GetContacts(contacts);for(int i=0;i<n;i++)if(contacts[i].normal.y<-.7f){if(!b25Head)Record("MEASURE B25 head collider="+contacts[i].collider.name+" pos="+Body.position);b25Head=true;}}yield return null;}}
 IEnumerator B25Pose(string id,Vector2 at)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId==id)yield return RoomAt(id=="A03"?"A01":"A03");yield return RoomAt(id);
  NoAbilities();Player.GetComponent<PlayerDash>().hasDash=true;Health.RestoreMax();Motor.ClearTransientState();Player.GetComponent<PlayerDash>().Cancel();PlaceForAuthorizedSetup(at);yield return new WaitForSeconds(.35f);
  b25Head=false;Record($"FIXTURE B25 segment {id} start={Body.position:F3} dash=True wall=False enemiesLive={Room.GetComponentsInChildren<EnemyReset>().Count(e=>e.Live!=null&&e.Live.activeSelf)}; no further pose/HP writes in segment");
 }
 IEnumerator B25Jet(float x,float top,float landingX,float landingY,string label)
 {
  yield return GoX(x);Input();yield return Until(()=>Body.position.y>=top-.08f,"steam top "+label,10);
  Key dir=landingX>x?Key.RightArrow:Key.LeftArrow;yield return Keys(dir,Key.LeftShift);Input(dir);
  yield return Until(()=>Motor.Grounded&&Math.Abs(Body.position.y-landingY)<.12f,"steam shelf "+label,4);yield return Keys();
  Record($"MEASURE steam {label} top={top:F2} landing={Body.position:F3} HP={Health.HP}");
 }
 IEnumerator B25Pogo(float edge,float[] xs,float[] tops,float landX,float landY,bool dash,string label)
 {
  if(xs.Length>1){yield return GoX(edge-1.2f);yield return new WaitForSeconds(Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03f);}
  Input(Key.RightArrow);yield return Until(()=>Body.position.x>=edge,"pogo running takeoff "+label,2);
  int before=Player.GetComponent<PlayerAttack>().PogosPerformed,returns=Hazard.Returns;double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+15,next=0;bool landed=false,dashed=false;Input(Key.RightArrow,Key.Z);
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   int count=Player.GetComponent<PlayerAttack>().PogosPerformed-before;float x=count<xs.Length?xs[count]:landX,dx=x-Body.position.x;var keys=new List<Key>{Key.DownArrow};
   if(Time.timeAsDouble-began<.45)keys.Add(Key.Z);if(Math.Abs(dx)>.12f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(count<xs.Length&&!Motor.Grounded&&Body.linearVelocity.y<=0&&Math.Abs(dx)<.75f&&Body.position.y<=tops[count]+(landY>=5?.7f:xs.Length==1?.8f:.4f)&&Time.timeAsDouble>=next){keys.Add(Key.X);next=Time.timeAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03;}
   if(count>=xs.Length&&dash&&!dashed&&Body.linearVelocity.y<=.6f){keys.Add(Key.LeftShift);dashed=true;}
   Input(keys.ToArray());if(count>=xs.Length&&Motor.Grounded&&Math.Abs(Body.position.y-landY)<.12f&&Math.Abs(Body.position.x-landX)<1){landed=true;break;}if(Hazard.Returns>returns||Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
  }
  yield return Keys();Record($"MEASURE B25 pogo={label} pogos={Player.GetComponent<PlayerAttack>().PogosPerformed-before} landed={landed} head={b25Head} HP={Health.HP} pos={Body.position:F3}");Check(landed,"pogo route "+label);
 }
 IEnumerator B25FirstPass()
 {
  b25Watch=true;var watcher=StartCoroutine(B25WatchHead());
  string section=N5Args.Value("-b25-section","all");
  for(int trial=1;trial<=5;trial++)
  {
   if(section=="all"||section=="A03"){
   yield return B25Pose("A03",new Vector2(7,15.015f));yield return GoX(4.7f);Input(Key.LeftArrow,Key.Z);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="B01","physical ceiling A03-B01 via ledge",3);yield return Keys();yield return Ready();
   Check(!b25Head,"A03-B01 head0 trial="+trial);Record("PASS B25 A03-B01 trial="+trial);}
   if(section=="all"||section=="B01"){
   yield return B25Pose("B01",new Vector2(5,1.015f));yield return JumpTo(12.5f,16.3f,3,"B01 market stall");yield return GoX(31.8f);yield return JumpTo(31.8f,34.5f,3,"B01 crate");yield return JumpTo(35.5f,38.5f,5,"B01 roof");
   yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B02",Key.RightArrow,"B01-B02 roof door",5);yield return Ready();Check(!b25Head,"B01 roof head0 trial="+trial);Record("PASS B25 B01 roof trial="+trial);}
   if(section=="all"||section=="B02"){
   yield return B25Pose("B02",new Vector2(3.5f,1.015f));yield return B25Pogo(13.7f,new[]{17.5f,20f},new[]{2.8f,3.1f},23.5f,1,false,"B02 two lanterns");
   yield return JumpTo(24,30.8f,1,"B02 E2",true);yield return JumpTo(33.7f,41.2f,1,"B02 acid6 dash",true);yield return GoX(61);
   yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.RightArrow,"B02-B03 physical door",3);yield return Ready();Check(!b25Head,"B02 bottom head0 trial="+trial);Record("PASS B25 B02 bottom trial="+trial);}
   if(section=="all"||section=="B03"){
   yield return B25Pose("B03",new Vector2(3.5f,1.015f));yield return B25Jet(13.6f,8.5f,10,8,"B03 lower");yield return B25Jet(7.7f,17.5f,4.3f,17,"B03 upper");
   yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B04",Key.LeftArrow,"B03-B04 physical door",4);yield return Ready();Check(!b25Head,"B03 steam head0 trial="+trial);Record("PASS B25 B03 steam to B04 trial="+trial);}
  }
  if(section=="all"||section=="B02"){
  yield return B25Pose("B02",new Vector2(33,1.015f));int hp=Health.HP,returns0=Hazard.Returns;Input(Key.RightArrow);yield return Until(()=>Hazard.Returns>returns0,"B02 acid return",3);yield return Until(()=>!Hazard.Returning,"B02 acid return settled",3);yield return Keys();
  Record($"MEASURE B02 acid returns={Hazard.Returns-returns0} HP={hp}->{Health.HP} safe={Body.position:F3}");Check(Health.HP==hp-1&&Body.position.x<34,"B02 acid returns to entry side with damage1");}
  b25Watch=false;StopCoroutine(watcher);
 }
 IEnumerator B25Optional()
 {
  b25Watch=true;var watcher=StartCoroutine(B25WatchHead());
  string section=N5Args.Value("-b25-optional-section","all");
  for(int trial=1;trial<=5;trial++)
  {
   if(section=="all"||section=="B02"){
   yield return B25Pose("B02",new Vector2(3.5f,1.015f));yield return B25Jet(11.7f,8.5f,15,8,"B02 upper");yield return GoX(31);yield return Keys();
   var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).Single();Check(DiscoveryReward.Found(shard.candidateId),"B02 upper actual shard collected");Check(!b25Head,"B02 optional head0 trial="+trial);Record("PASS B25 B02 upper trial="+trial);}
   if(section=="all"||section=="B06"){
   yield return B25Pose("B06",new Vector2(3.5f,1.015f));yield return B25Pogo(7.7f,new[]{10.5f,15f},new[]{2.8f,2.8f},19.5f,5,true,"B06 lanterns to crumble");
   yield return B25Pogo(20.2f,new[]{23f},new[]{1.6f},26.8f,4,false,"B06 spike to platform");yield return JumpTo(27.5f,31.5f,1,"B06 reward floor");yield return GoX(32);
   var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).Single();Check(DiscoveryReward.Found(shard.candidateId),"B06 actual shard collected");Check(!b25Head,"B06 optional head0 trial="+trial);Record("PASS B25 B06 trial="+trial);}
  }
  b25Watch=false;StopCoroutine(watcher);
 }
}
#endif
