#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B25WallRise(Key direction,Func<bool> done,string label,float seconds=24)
 {
  int walls=Motor.WallJumpsPerformed;double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+seconds;
  while(!done()&&Time.realtimeSinceStartupAsDouble<end)
  {double phase=(Time.timeAsDouble-began)%.5;Input(phase<.43?new[]{direction,Key.Z}:new[]{direction});if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;}
  yield return Keys();Record($"MEASURE wall route={label} success={done()} jumps={Motor.WallJumpsPerformed-walls} room={RoomSession.Instance.CurrentRoomId} pos={Body.position:F3}");Check(done(),"wall route "+label);
 }
 IEnumerator B25Revisit()
 {
  string section=N5Args.Value("-b25-wall-section","all");
  for(int trial=1;trial<=3;trial++)
  {
   if(section=="all"||section=="B01")
   {
    yield return B25Pose("B01",new Vector2(5,1.015f));Motor.hasWallJump=true;Record("FIXTURE owned wall jump before B01 segment");
    yield return B25WallRise(Key.RightArrow,()=>Body.position.y>=15.2f,"B01 tower");Input(Key.RightArrow);yield return Until(()=>Motor.Grounded&&Body.position.y>=14.9f&&Body.position.x>=7.3f,"B01 tower roof",3);yield return Keys();
    Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=7.05f,"B01 ledge running takeoff",3);Input(Key.LeftArrow,Key.Z);yield return Until(()=>Motor.Grounded&&Body.position.y>=16.9f,"B01 high ledge",3);yield return Keys();
    yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B05",Key.LeftArrow,"B01-B05 high door",4);yield return Ready();Check(ProgressSave.IsFragmentCollected("V5-B01-0"),"B01 high ledge actual shard");Record("PASS B25 B01 tower/shard/B05 trial="+trial);
   }
   if(section=="all"||section=="B05")
   {
    yield return B25Pose("B05",new Vector2(20.5f,1.015f));Motor.hasWallJump=true;yield return GoX(14);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="A02","B05-A02 floor hole",3);yield return Ready();
    yield return B25WallRise(Key.RightArrow,()=>RoomSession.Instance.CurrentRoomId=="B05","A02 chimney to B05");yield return Ready();
    yield return GoX(14);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="A02","B05-A02 revisit hole",3);yield return Ready();
    double end=Time.realtimeSinceStartupAsDouble+12;
    while(!(Motor.Grounded&&Math.Abs(Body.position.y-12.4f)<.1f)&&Time.realtimeSinceStartupAsDouble<end){float dx=15.2f-Body.position.x;Input(Math.Abs(dx)<.1f?Array.Empty<Key>():new[]{dx>0?Key.RightArrow:Key.LeftArrow});yield return null;}
    yield return Keys();Check(Motor.Grounded&&Math.Abs(Body.position.y-12.4f)<.1f,"A02 shard ledge reached from owned revisit chimney");
    var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderBy(r=>Vector2.Distance(r.transform.position,new Vector2(15.2f,13.4f))).First();Check(DiscoveryReward.Found(shard.candidateId),"A02 moved shard actually collected");Record("PASS B25 B05-A02 roundtrip/shard trial="+trial);
   }
   if(section=="all"||section=="B03")
   {
    yield return B25Pose("B03",new Vector2(20.7f,1.015f));Motor.hasWallJump=true;
    yield return B25WallRise(Key.RightArrow,()=>RoomSession.Instance.CurrentRoomId=="B06","B03 east chimney");
    if(RoomSession.Instance.CurrentRoomId!="B06"){Input(Key.RightArrow);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="B06","B03-B06 upper door",3);}yield return Ready();yield return Keys();Record("PASS B25 B03 chimney/B06 trial="+trial);
   }
   if(section=="all"||section=="cover")
   {
    yield return B25Pose("B05",new Vector2(24.5f,1.015f));Motor.hasWallJump=true;
    yield return GoX(3);yield return Keys();Check(RoomSession.Instance.CurrentRoomId=="B05","B05 west crossing stays in room; floor-hole input remains unverified after retry limit");
    var cover=Room.GetComponentsInChildren<BreakableWall>(true).Single();var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).Single();
    Check(cover.Broken&&DiscoveryReward.Found(shard.candidateId),"B05 actual cover attack/shard contact, no hidden duplicate solid");Record("PASS B25 B05 cover/shard trial="+trial);
   }
  }
 }
}
#endif
