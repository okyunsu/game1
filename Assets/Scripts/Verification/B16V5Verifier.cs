#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public sealed class B16V5Verifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(N5Args.Has("-verify-b16-v5"))N5Args.Start<B16V5Verifier>("-verify-b16-v5");else N5Args.Start<B16V5Verifier>("-verify-b16-launch");}
 static string Arg(string prefix,string fallback){foreach(string a in Environment.GetCommandLineArgs())if(a.StartsWith(prefix,StringComparison.Ordinal))return a.Substring(prefix.Length);return fallback;}
 RoomDefinition Room=>FindFirstObjectByType<RoomDefinition>();
 RoomExit Exit(string id)=>Room.GetComponentsInChildren<RoomExit>().Single(e=>e.exitId==id);
 Vector2 ContactPoint(RoomExit exit){var bounds=exit.GetComponent<Collider2D>().bounds;return new Vector2(bounds.center.x,bounds.min.y+.05f);}
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save path");yield return Ready();
  Check(V4SceneMap.V5&&WorldMapLayout.Current.rooms.Length==6,"V5 six-room layout");
  Check(RoomSession.Instance.CurrentRoomId==Arg("-start-room=","A01"),"launcher start room");
  string give=Arg("-give=","");Check(Player.GetComponent<PlayerDash>().hasDash==give.Contains("dash"),"fixture dash");Check(Motor.hasDoubleJump==(give.Contains("dj")||give.Contains("double")),"fixture double jump");
  Check(N5Args.Has("-no-map")?FindObjectsByType<V4WorldMap>(FindObjectsSortMode.None).Length==0:V4WorldMap.Instance!=null,"map switch");
  Check(ProgressSave.TryLoad(out var initial)&&initial.checkpointId==(Arg("-start-room=","A01")=="A03"?"CP-A03":"CP-A01"),"existing schema checkpoint saved");
  if(N5Args.Has("-verify-b16-launch")){yield return new WaitForSecondsRealtime(10);yield break;}
  int patrolTotal=0,shardTotal=0;
  foreach(string id in V4SceneMap.Rooms)
  {
   yield return Setup(id);var bindings=Room.GetComponentsInChildren<V5PatrolBinding>();
   foreach(var b in bindings){var live=b.GetComponent<EnemyReset>().Live.GetComponent<EnemyPatrol>();Check(b.Connected&&live.leftPoint==b.leftPoint&&live.rightPoint==b.rightPoint,"live patrol endpoints "+id);}
   var previous=bindings.Select(b=>b.GetComponent<EnemyReset>().Live).ToArray();Health.RestoreMax();yield return null;yield return null;
   for(int i=0;i<bindings.Length;i++)Check(bindings[i].Connected&&bindings[i].GetComponent<EnemyReset>().Live!=previous[i],"restored patrol endpoints "+id);
   patrolTotal+=bindings.Length;int shards=Room.GetComponentsInChildren<DiscoveryReward>().Length;shardTotal+=shards;Check(shards==(id=="A02"?2:id=="A04"||id=="A06"?1:0),"shard count "+id);lines.Add("MEASURE scene="+id+" patrol="+bindings.Length+" shards="+shards);
  }
  Check(patrolTotal==13&&shardTotal==4,"patrol13 shards4 total");
  foreach(var item in new[]{new[]{"A01","A01-Z01"},new[]{"A02","A02-B05"},new[]{"A03","A03-B01"},new[]{"A05","A05-Z01"}})
  {
   yield return Setup(item[0]);Player.GetComponent<PlayerDash>().hasDash=true;Motor.hasDoubleJump=Motor.hasWallJump=true;Player.GetComponent<PlayerSlam>().hasSlam=true;yield return Blocked(item[1],true);
  }
  yield return Setup("A02");yield return Blocked("A02-A06",true);
  yield return Setup("A05");yield return Blocked("A04-A05",true);
  yield return Setup("A01");Motor.hasDoubleJump=false;yield return Blocked("A01-A06",false);Motor.hasDoubleJump=true;
  string[][] steps={new[]{"A01","A01-A02","A02"},new[]{"A02","A01-A02","A01"},new[]{"A02","A02-A03","A03"},new[]{"A03","A02-A03","A02"},new[]{"A02","A02-A03U","A03"},new[]{"A03","A02-A03U","A02"},new[]{"A03","A03-A04","A04"},new[]{"A04","A03-A04","A03"},new[]{"A04","A04-A05","A05"},new[]{"A05","A05-A03","A03"},new[]{"A03","A05-A03","A05"},new[]{"A06","A02-A06","A02"},new[]{"A01","A01-A06","A06"},new[]{"A06","A01-A06","A01"}};
  foreach(var step in steps){yield return Setup(step[0]);Motor.hasDoubleJump=true;yield return Door(step[1],step[2]);}
  Player.GetComponent<PlayerDash>().hasDash=false;Motor.hasDoubleJump=Motor.hasWallJump=false;ProgressSave.HasSlam=false;yield return Setup("A05");var pickup=FindFirstObjectByType<DashPickup>();Check(pickup!=null,"authored Dash shrine exists");PlaceForAuthorizedSetup(pickup.transform.position);yield return new WaitForSeconds(.2f);Check(Player.GetComponent<PlayerDash>().hasDash,"physical Dash pickup");Check(ProgressSave.TryLoad(out var dash)&&dash.dash,"Dash pickup saved");
  yield return Setup("A04");var reward=FindFirstObjectByType<DiscoveryReward>();string rewardId=reward.candidateId;PlaceForAuthorizedSetup(reward.transform.position);yield return new WaitForSeconds(.15f);Check(DiscoveryReward.Found(rewardId),"physical shard collection");
  ProgressSave.SaveCurrent(RoomSession.Instance);yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A01"&&Player.GetComponent<PlayerDash>().hasDash,"Continue restores V5 checkpoint and Dash");Check(N5Args.Has("-no-map")||FindObjectsByType<V4WorldMap>(FindObjectsSortMode.None).Length==1,"single map after Continue");
  yield return new WaitForSecondsRealtime(10);
 }
 IEnumerator Setup(string id)
 {
  if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),"Entry"),"explicit room fixture "+id);yield return Ready();}
  Health.RestoreMax();yield return new WaitForSeconds(.5f);yield return Ready();
 }
 IEnumerator Blocked(string id,bool disabled)
 {
  var exit=Exit(id);Check(exit.enabled!=disabled,"exit enabled state "+id);Check(!exit.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"rule blocks "+id);string before=RoomSession.Instance.CurrentRoomId;
  PlaceForAuthorizedSetup(ContactPoint(exit));yield return new WaitForSeconds(.15f);Check(RoomSession.Instance.CurrentRoomId==before&&!RoomSession.Instance.Transitioning,"physical blocked transition0 "+id);
  Check(Room.TryGetSpawn("Entry",out var entry),"fixture Entry");PlaceForAuthorizedSetup(entry.transform.position);yield return new WaitForSeconds(.1f);
 }
 IEnumerator Door(string id,string destination)
 {
  var exit=Exit(id);Check(exit.enabled&&exit.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"rule permits "+id);string spawnId=exit.destinationSpawnId;
  PlaceForAuthorizedSetup(ContactPoint(exit));lines.Add("MEASURE door contact="+id+" source="+RoomSession.Instance.CurrentRoomId+" player="+Body.position);yield return new WaitForFixedUpdate();yield return Until(()=>RoomSession.Instance.CurrentRoomId==destination&&!RoomSession.Instance.Transitioning,"physical door "+id);yield return Ready();
  Check(Room.TryGetSpawn(spawnId,out var spawn),"destination spawn "+id);Check(Vector2.Distance(Body.position,spawn.transform.position)<.4f,"arrival position "+id);if(destination=="A05"&&id=="A05-A03")Check(Mathf.Abs(spawn.transform.position.x-3.5f)<.01f,"A05 west door arrival x3.5");
  lines.Add("MEASURE door="+id+" destination="+destination+" spawn="+spawn.transform.position+" player="+Body.position);
 }
}
#endif
