#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B23Hazard()
 {
  yield return RoomAt("A05");yield return ResetAt(new Vector2(13.5f,1.015f));NoAbilities();
  Check(Hazard.HasSafePoint,"safe floor recorded");int hp=Health.HP,returns=Hazard.Returns;Vector2 safe=Hazard.SafePoint;
  PlaceForAuthorizedSetup(new Vector2(9,.5f));yield return Until(()=>Hazard.Returning,"hazard hit");double began=Time.timeAsDouble;int damageAccepted=0;
  while(Hazard.Returning){if(Health.TakeDamage(1,Body.position))damageAccepted++;Input(Key.RightArrow,Key.Z,Key.X,Key.LeftShift);yield return null;}
  yield return Keys();Record($"MEASURE return total={(Time.timeAsDouble-began):F3} getUp={Hazard.LastGetUpSeconds:F3} damageDuring={damageAccepted} hp={hp}->{Health.HP} returns={Hazard.Returns-returns}");
  Check(Health.HP==hp-1&&damageAccepted==0&&Hazard.Returns==returns+1&&Vector2.Distance(Body.position,safe)<.1f&&Math.Abs(Hazard.LastGetUpSeconds-.45)<.05,"return lock/invulnerability 0.45s and safe point");
  // Lethal damage remains the real checkpoint death path.
  yield return new WaitForSeconds(Health.tuning.invincibility+.1f);Health.TakeDamage(Health.HP-1,Body.position);yield return new WaitForSeconds(Health.tuning.invincibility+.1f);
  PlaceForAuthorizedSetup(new Vector2(9,.5f));yield return Until(()=>RoomSession.Instance.Respawning,"HP0 death");Check(!Hazard.Returning,"HP0 skips safe-point coroutine");yield return Ready();Check(Health.HP==Health.tuning.playerMaxHP,"checkpoint death restores HP");
 }
 IEnumerator LiftRide(bool up)
 {
  var lift=Room.GetComponentsInChildren<LeverLift>().Single();
  // Lever is struck with the same X attack used in the player flow.
  if(Body.position.x>45.85f)yield return WalkTo(()=>Body.position.x<=45.5f,Key.LeftArrow,"step onto lift",4);
  if(Body.position.x<45.85f)yield return WalkTo(()=>Body.position.x>=45.85f,Key.RightArrow,"walk onto lift",12);
  else yield return Keys();
  yield return new WaitForSeconds(Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.02f);
  if(up&&Body.position.y<lift.high-1&&lift.GetComponent<Collider2D>().bounds.max.y>lift.low+.1f){yield return Tap(Key.X);yield return Until(()=>Math.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.low)<.02f,"call lift down",12);yield return new WaitForSeconds(.4f);}
  yield return new WaitForSeconds(Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.02f);
  yield return Tap(Key.X);
  yield return WalkTo(()=>Body.position.x<=44.8f,Key.LeftArrow,"center rider clear of upper shelf",3);
  yield return Until(()=>Math.Abs(lift.GetComponent<Collider2D>().bounds.max.y-(up?lift.high:lift.low))<.02f,"lift endpoint",12);
  yield return Keys();Record($"MEASURE A03 lift up={up} surface={lift.GetComponent<Collider2D>().bounds.max.y:F3} feet={Body.position.y:F3} unlocked={lift.Unlocked}");
  Check(Math.Abs(Body.position.y-(up?lift.high:lift.low))<.1f,"player rides lift to endpoint");
 }
 IEnumerator B23Lift()
 {
  yield return RoomAt("A03");yield return ResetAt(new Vector2(44.8f,1.015f));var lift=Room.GetComponentsInChildren<LeverLift>().Single();
  Check(lift.startLow&&lift.Unlocked&&Math.Abs(lift.GetComponent<Collider2D>().bounds.max.y-1)<.02,"new lift starts low unlocked");
  yield return LiftRide(true);yield return LiftRide(false);yield return LiftRide(true);
  foreach(string id in new[]{"B01","C01"}){yield return RoomAt(id,"Assets/Scenes/V4/"+id+"_V4.unity");foreach(var old in Room.GetComponentsInChildren<LeverLift>()){Check(!old.startLow&&!old.Unlocked&&Math.Abs(old.GetComponent<Collider2D>().bounds.max.y-old.high)<.02,"V4 "+id+" starts high locked");old.Call(false);Check(!old.Unlocked,"V4 lower cannot unlock");old.Call(true);Check(old.Unlocked,"V4 upper unlocks");yield return new WaitForSeconds(.15f);Check(old.GetComponent<Collider2D>().bounds.max.y<old.high,"V4 descends after upper call");}}
 }
 IEnumerator B23Field()
 {
  bool allClear=true;
  foreach(string id in new[]{"A01","A02","A04"})
  {
   yield return RoomAt(id);NoAbilities();DisableEnemies();var roomData=B09Json.D(B09Json.Read(System.IO.File.ReadAllText("Design/v5/rooms_v5_A.json"))["rooms"]);var data=B09Json.D(roomData[id]);
   var solids=B09Json.A(data,"solid").Select(n=>((System.Collections.Generic.List<object>)n).Select(B09Json.F).ToArray()).ToArray();var platforms=B09Json.A(data,"platform").Select(n=>((System.Collections.Generic.List<object>)n).Select(B09Json.F).ToArray()).ToArray();
   // Each local upward approach uses the unchanged authored rectangles as fixture poses.
   foreach(var target in platforms)
   {
    var sources=solids.Concat(platforms).Where(r=>r[2]>=r[3]&&target[1]>r[1]+r[3]&&target[1]+target[3]-r[1]-r[3]>=1.5f&&target[1]+target[3]-r[1]-r[3]<=2.6f&&Mathf.Max(0,target[0]-r[0]-r[2],r[0]-target[0]-target[2])<=2.1f).ToArray();
    foreach(var source in sources)
    {
     int bumps=0,landed=0;bool right=target[0]>source[0];float edge=right?source[0]+source[2]:source[0],sx=edge+(right?-1:1)*.8f;
     for(int trial=0;trial<5;trial++)
     {
      // Give any previously touched crumble its full fall/restore cycle before a fresh trial.
      if(Room.GetComponentsInChildren<CrumblePlatform>().Any(c=>Mathf.Abs(c.transform.position.x-(target[0]+target[2]/2))<.1f||Mathf.Abs(c.transform.position.x-(source[0]+source[2]/2))<.1f))yield return new WaitForSeconds(3.8f);
      yield return ResetAt(new Vector2(sx,source[1]+source[3]+.015f));Key dir=right?Key.RightArrow:Key.LeftArrow;Input(dir);yield return Until(()=>right?Body.position.x>=edge-.32f:Body.position.x<=edge+.32f,"approach runup",2);Input(dir,Key.Z);bool rose=false,hit=false,ok=false;double end=Time.realtimeSinceStartupAsDouble+1.5;
      while(Time.realtimeSinceStartupAsDouble<end){rose|=Body.linearVelocity.y>0;var contacts=new ContactPoint2D[16];int count=Player.GetComponent<Collider2D>().GetContacts(contacts);for(int i=0;i<count;i++)if(contacts[i].normal.y<-.7f){if(!hit)Record($"MEASURE {id} trial={trial+1} headBlocker={contacts[i].collider.name} position={Body.position:F3}");hit=true;}if(rose&&Motor.Grounded&&Body.position.y>source[1]+source[3]+.1f){ok=true;break;}if(rose&&Body.position.y<source[1]+source[3]-.2f)break;yield return null;}
      yield return Keys();if(hit)bumps++;if(ok)landed++;
     }
     Record($"MEASURE {id} approach from=({source[0]},{source[1]+source[3]}) to=({target[0]},{target[1]+target[3]}) headContacts={bumps}/5 landed={landed}/5");allClear&=bumps==0;
    }
   }
   if(id=="A01")
   {
    var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderByDescending(r=>r.transform.position.y).First();DiscoveryReward.ResetSession();shard.gameObject.SetActive(true);yield return ResetAt(new Vector2(33,11.615f));yield return GoX(shard.transform.position.x);Check(DiscoveryReward.Found(shard.candidateId),"A01 high shelf shard collected by walking");
   }
   foreach(var crumble in Room.GetComponentsInChildren<CrumblePlatform>())
   {
    var bounds=crumble.GetComponent<Collider2D>().bounds;yield return ResetAt(new Vector2(bounds.center.x,bounds.max.y+.015f));yield return Until(()=>crumble.Falling,"crumble falls",3);Check(!crumble.GetComponent<Collider2D>().enabled,"falling collider disabled");yield return Until(()=>!crumble.Falling,"crumble restores",8);Check(crumble.GetComponent<Collider2D>().enabled,"crumble restored");Record("PASS "+id+" "+crumble.name+" stood/fell/restored");
   }
   if(id=="A01"||id=="A02")
   {
    var spike=Room.GetComponentsInChildren<DamageBlock>().Single(d=>d.name.StartsWith("hazard"));var b=spike.GetComponent<Collider2D>().bounds;yield return ResetAt(new Vector2(b.max.x+1,1.015f));Check(Hazard.HasSafePoint,"spike safe point");int before=Hazard.Returns;Vector2 safe=Hazard.SafePoint;PlaceForAuthorizedSetup(new Vector2(b.center.x,b.max.y+.2f));yield return Until(()=>Hazard.Returns>before,"spike return",3);yield return Until(()=>!Hazard.Returning,"spike getup",2);Check(Vector2.Distance(Body.position,safe)<.1f,"spike returns last safe floor");Record("PASS "+id+" spike damage and safe floor return");
   }
  }
  Check(allClear,"all field approach head contacts0");
 }
 IEnumerator B23Loop()
 {
  Check(RoomSession.Instance.CurrentRoomId=="A03","loop starts A03");NoAbilities();double began=Time.timeAsDouble;
  Record("FIXTURE loop initial CP-A03 only; no position/health/ownership writes after start");
  yield return LiftRide(true);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A04",Key.RightArrow,"A04 physical door",5);yield return Ready();
  yield return JumpTo(4.2f,5.8f,3,"A04 block");yield return JumpTo(7.25f,10.2f,5,"A04 platform");yield return JumpTo(11.4f,14.1f,7,"A04 crumble");yield return JumpTo(14.25f,16.5f,7,"A04 upper");yield return GoX(27.5f);yield return Until(()=>Boss!=null&&Boss.Fighting,"arena trigger",4);yield return Fight();
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A05",Key.RightArrow,"opened floor",8);yield return Ready();yield return JumpTo(40,37.5f,2.6f,"dash shrine");yield return Until(()=>Player.GetComponent<PlayerDash>().hasDash,"dash pickup",3);
  yield return GoX(15.5f);yield return JumpTo(12.3f,4.6f,1,"A05 trench",true);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A03",Key.LeftArrow,"return A03",5);yield return Ready();
  yield return LiftRide(true);yield return JumpTo(43.75f,36.7f,13,"practice1",true);yield return JumpTo(35.25f,28.1f,13,"practice2",true);yield return JumpTo(26.75f,20.8f,15,"practice3 shelf",true);
  Check(ProgressSave.TryLoad(out var save)&&save.dash&&save.defeatedBossIds.Contains("B1-A04"),"save dash/B1");yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A03"&&Player.GetComponent<PlayerDash>().hasDash&&ProgressSave.IsBossDefeated("B1-A04"),"Continue restores CP/dash/B1");Record($"MEASURE B23 loop seconds={Time.timeAsDouble-began:F3} complete=True");
 }
}
#endif

