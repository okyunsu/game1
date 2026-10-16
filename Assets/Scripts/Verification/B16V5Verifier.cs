#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B16V5Verifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){foreach(string flag in new[]{"-verify-b16-v5","-verify-b16-map","-verify-b16-route","-verify-b16-launch"})if(N5Args.Has(flag)){N5Args.Start<B16V5Verifier>(flag);break;}}
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
  if(N5Args.Has("-verify-b16-map")){yield return Map();yield break;}
  if(N5Args.Has("-verify-b16-route")){yield return Route();yield break;}
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
 // Fixed authored waypoints are measurement inputs, never tuning or level edits.
 // A segment may restart only at an existing arrival spawn after its predecessor fails.
 void RouteLog(string message){lines.Add(message);Debug.Log(message);}
 IEnumerator Route()
 {
  RouteLog("MEASURE route limit=120s/segment input=virtual-keyboard teleportWithinSegment=0 abilities=none");
  yield return Segment("A01","start","A02",new[]{new Vector2(47.8f,1)});
  yield return Segment("A02","A01-A02","A03",new[]{new Vector2(27.8f,1)});
  yield return Segment("A03","A02-A03","A04",new[]{new Vector2(14.5f,1),new Vector2(18.5f,2.9f),new Vector2(20.5f,2.9f),new Vector2(22.5f,1),new Vector2(25.5f,2.9f),new Vector2(27.4f,2.9f),new Vector2(30.5f,4.9f),new Vector2(32.4f,4.9f),new Vector2(36.5f,6.9f),new Vector2(38,6.9f),new Vector2(40.2f,9.3f),new Vector2(41.6f,11.4f),new Vector2(46,11),new Vector2(49.8f,11)});
  yield return Segment("A04","A03-A04","A05",new[]{new Vector2(6.5f,1),new Vector2(10.5f,2.9f),new Vector2(12.3f,2.9f),new Vector2(15.5f,4.9f),new Vector2(17.3f,4.9f),new Vector2(19.5f,7.3f),new Vector2(25,6.9f),new Vector2(40,6.9f),new Vector2(46,1)});
  yield return Segment("A05","A04-A05","Dash",new[]{new Vector2(40.5f,1),new Vector2(37.5f,2.6f)});
  RouteLog("MEASURE route measurement complete; BLOCKED rows are controller limits, not proof of impossible geometry");
  yield return new WaitForSecondsRealtime(10);
 }
 IEnumerator Segment(string room,string spawn,string goal,Vector2[] points)
 {
  yield return Keys();yield return Ready();
  if(RoomSession.Instance.CurrentRoomId!=room)
  {
   RouteLog("MEASURE route independent-fixture room="+room+" spawn="+spawn+" after previous segment did not arrive");
   Check(RoomSession.Instance.RequestTransition(room,V4SceneMap.Path(room),spawn),"route arrival fixture "+room);yield return Ready();
  }
  else RouteLog("MEASURE route natural-continuation room="+room);
  double began=Time.realtimeSinceStartupAsDouble,jumpUntil=0,nextJump=0,nextAttack=0,nextSample=0;
  int step=0;float maxY=Body.position.y,maxX=Body.position.x;bool passed=false;string reason="120s input budget exhausted";Vector2 last=Body.position;int hp=Health.HP,pogos=0;
  RouteLog("MEASURE route begin room="+room+" spawn="+spawn+" position="+last+" hp="+hp+" dash="+Player.GetComponent<PlayerDash>().hasDash+" dj="+Motor.hasDoubleJump);
  while(Time.realtimeSinceStartupAsDouble-began<120)
  {
   if(!RoomSession.Instance.Transitioning&&RoomSession.Instance.CurrentRoomId==goal){passed=true;break;}
   if(Player==null||RoomSession.Instance.Transitioning||RoomSession.Instance.Respawning){InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;continue;}
   if(RoomSession.Instance.CurrentRoomId!=room){reason="left source room for "+RoomSession.Instance.CurrentRoomId;break;}
   last=Body.position;hp=Health.HP;pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;maxY=Mathf.Max(maxY,last.y);maxX=Mathf.Max(maxX,last.x);
   if(goal=="Dash"&&Player.GetComponent<PlayerDash>().hasDash){passed=true;break;}
   Vector2 target=points[step];if(step<points.Length-1&&Mathf.Abs(last.x-target.x)<.25f&&last.y>=target.y-.25f){step++;target=points[step];RouteLog("MEASURE route waypoint room="+room+" step="+step+" position="+last);}
   double now=Time.realtimeSinceStartupAsDouble;var keys=new List<Key>();float dx=target.x-last.x;
   if(Mathf.Abs(dx)>.2f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   bool enemyNear=Room.GetComponentsInChildren<EnemyReset>().Any(e=>e.Live!=null&&Mathf.Abs(e.Live.transform.position.x-last.x)<2.5f&&Mathf.Abs(e.Live.transform.position.y-last.y)<2);
   if(Motor.Grounded&&now>=nextJump&&(target.y>last.y+.4f||enemyNear)){jumpUntil=now+.42;nextJump=now+.65;}
   if(now<jumpUntil)keys.Add(Key.Z);
   if(now>=nextAttack&&(!Motor.Grounded||enemyNear)){keys.Add(Key.X);if(!Motor.Grounded)keys.Add(Key.DownArrow);nextAttack=now+.34;}
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));
   if(now>=nextSample){RouteLog("MEASURE route sample room="+room+" elapsed="+(now-began).ToString("F1")+" step="+step+" position="+last+" grounded="+Motor.Grounded+" hp="+hp+" pogos="+pogos);nextSample=now+10;}
   yield return null;
  }
  yield return Keys();
  RouteLog((passed?"PASS route ":"BLOCKED route ")+room+" goal="+goal+" elapsed="+(Time.realtimeSinceStartupAsDouble-began).ToString("F2")+" position="+last+" step="+step+" maxX="+maxX.ToString("F2")+" maxY="+maxY.ToString("F2")+" hp="+hp+" pogos="+pogos+(passed?"":" reason="+reason));
  if(passed)yield return Ready();
 }
 IEnumerator Map()
 {
  var map=V4WorldMap.Instance;Check(map!=null&&map.All,"map-all enabled");
  Check(map.VisibleRooms.Length==6&&map.VisibleDoors.Length==12,"six rooms twelve doors");
  Check(map.VisibleDoors.Count(d=>d.stub)==4&&map.VisibleDoors.Count(d=>d.type=="oneway")==1,"four stub one oneway");
  yield return Tap(Key.M);Check(map.Mode==V4WorldMap.Display.Large,"actual M large map");
  RoomSession.Instance.GrantDash();RoomSession.Instance.GrantDoubleJump();map.RefreshProgress();
  foreach(var d in map.VisibleDoors)
  {
   string label=map.DoorLabel(d);Check(d.span.Length==2,"door geometry "+d.id);
   if(d.stub){Check(label.Contains("×")&&!map.ShowsUnknown(d),"closed stub no question "+d.id);Check(map.DoorDisplayColor(d)==V4WorldMap.GateColor(d.gate),"stub stays locked color "+d.id);}
   if(d.type=="oneway")Check(d.a=="A06"&&d.b=="A02"&&label.Contains("→"),"oneway A06 to A02 right arrow");
   lines.Add("MEASURE map door="+d.id+" a="+d.a+" b="+d.b+" type="+d.type+" stub="+d.stub+" label="+label+" unknown="+map.ShowsUnknown(d));
  }
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
