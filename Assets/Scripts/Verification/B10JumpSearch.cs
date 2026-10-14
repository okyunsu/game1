#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using J=B09Json;

// Opt-in Release instrumentation. Gameplay input remains in Dynamic Update.
public sealed class B10JumpSearch:N5Runner
{
 [Serializable] public sealed class Frame {public float seconds;public int keys;}
 [Serializable] public sealed class Recording {public string room,route,source,target;public Vector2 start,end;public int abilities,fps;public List<Frame> frames=new();}
 [Serializable] public sealed class Candidate {public float launch,run,hold,dj,dash,wall,attack,slam,wait;public int air,method;}
 sealed class Node {public string token,destination;public Vector2 point;public float left,right;public bool surface;}
 Dictionary<string,object> rooms;string roomId,routeName,root;List<string> tokens;
 List<Recording> prefix=new();Recording recording;bool success,dead,prefixOK;int pass,blocked,deferred,ability=15;
 float bestDistance,bestHeight,bestTravel;Vector2 bestPosition;string contacts="";readonly ContactPoint2D[] contactBuffer=new ContactPoint2D[32];
 string live;double itemStart;int attempted;bool liftLowSeen;int verificationFps;bool fast;double emittedRealTime;long emittedFrames;
 bool worldCapture;readonly List<Frame> worldFrames=new();readonly List<string> worldTimes=new();
 int failedPrefix=-1;readonly HashSet<string> untrustedTargets=new();
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10JumpSearch>("-verify-b10-search");
 void Row(string row){lines.Add(row);File.AppendAllText(live,row+Environment.NewLine);}
 protected override IEnumerator Tests()
 {
  root=N5Args.Value("-b10-input-root","Validation/_raw/B10/inputs");Directory.CreateDirectory(root);
  live=Path.ChangeExtension(Evidence,"live.txt");if(File.Exists(live))throw new IOException("Preserve "+live);
  string design=Resources.Load<TextAsset>("V4Design").text;string expectedDesign=N5Args.Value("-b10-design-sha256","");
  using(var hash=System.Security.Cryptography.SHA256.Create()){string actualDesign=BitConverter.ToString(hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(design))).Replace("-","");Row("MEASURE loaded design SHA256="+actualDesign);if(expectedDesign!=""&&actualDesign!=expectedDesign)throw new Exception("Build design differs from approved workspace design; exclude this run");}
  rooms=J.D(J.Read(design)["rooms"]);
  string mode=N5Args.Value("-verify-b10-search","routes");
  // Opt-in capture cadence keeps render-input replay independent of OS scheduling.
  verificationFps=Application.targetFrameRate;Time.captureDeltaTime=1f/verificationFps;fast=N5Args.Has("-b10-fast")&&mode!="world"&&mode!="boss";Row("MEASURE cadence input-fps="+verificationFps+" fast-search="+fast+" physics-step="+Time.fixedDeltaTime);
  yield return NewGame();ProgressSave.Enabled=false;
  if(mode=="replay")yield return ReplayAll();
  else if(mode=="mechanics")yield return Mechanics(false);
  else if(mode=="course")yield return Mechanics(true);
  else if(mode=="world")yield return World();
  else if(mode=="world-replay")yield return WorldReplay();
  else if(mode=="boss")yield return BossTests();
  else if(mode=="clock")yield return ClockProbe();
  else if(mode=="connections"||mode=="isolation")yield return Entries(mode=="isolation");
  else foreach(var pair in rooms)
  {
   if(N5Args.Value("-b10-room",pair.Key)!=pair.Key)continue;
   roomId=pair.Key;var routes=J.D(J.D(pair.Value)["routes"]);
   foreach(var route in routes)
   {
    if(N5Args.Value("-b10-route",route.Key)!=route.Key)continue;
    routeName=route.Key;tokens=((List<object>)route.Value).Select(n=>n.ToString()).ToList();ability=15;prefix.Clear();
    if(N5Args.Has("-b10-split")&&roomId=="A02"&&routeName=="wall"){tokens.Insert(1,"platform2~16.5@wj");Row("MEASURE two-stage route A02/wall via actual alcove platform; no intermediate placement");}
    if(N5Args.Has("-b10-split")&&roomId=="C02"&&routeName=="low"){tokens.Insert(1,"solid9");Row("MEASURE two-stage route C02/low via actual crate1; no intermediate placement");}
    itemStart=Time.realtimeSinceStartupAsDouble;attempted=0;
    Row("BEGIN route "+roomId+"/"+routeName+" nodes="+tokens.Count);
    yield return SearchRoute();
   }
  }
  Row("MEASURE observed active render-fps="+(emittedFrames/Math.Max(.001,emittedRealTime))+" frames="+emittedFrames);Row("MEASURE B10 mode="+mode+" PASS="+pass+" BLOCKED-GEOMETRY="+blocked+" budget-deferred="+deferred);
  if(deferred>0)throw new Exception("Search budget reached before exhaustive classification; pending items retained");
 }
 Node Resolve(string token)
 {
  var d=J.D(rooms[roomId]);string bare=token.Split('@')[0];var split=bare.Split('~');bare=split[0];var node=new Node{token=token};
  if(bare.StartsWith("point:")){var xy=bare.Substring(6).Split(',');node.point=new Vector2(float.Parse(xy[0],System.Globalization.CultureInfo.InvariantCulture),float.Parse(xy[1],System.Globalization.CultureInfo.InvariantCulture));node.left=node.point.x;node.right=node.point.x;foreach(string kind in new[]{"solid","platform","pad"})foreach(var obj in J.A(d,kind)){var r=(List<object>)obj;float y=J.F(r[1])+J.F(r[3]);if(Mathf.Abs(y-node.point.y)<.15f&&node.point.x>=J.F(r[0])&&node.point.x<=J.F(r[0])+J.F(r[2])){node.surface=true;node.left=J.F(r[0]);node.right=node.left+J.F(r[2]);}}return node;}
  if(bare.StartsWith("floor:")||bare.StartsWith("ceil:")||bare.StartsWith("door:")){var p=J.A(d,"doors").Select(J.D).First(v=>J.S(v,"id")==bare.Substring(bare.IndexOf(':')+1));var r=J.A(p,"rect");node.point=new Vector2(J.F(r[0])+J.F(r[2])/2,J.F(r[1])+J.F(r[3])/2);node.destination=J.S(p,"to").Split('(')[0];return node;}
  if(bare.StartsWith("spawn:")){var p=J.A(d,"spawns").Select(J.D).First(v=>J.S(v,"id")==bare.Substring(6));node.point=new Vector2(J.F(p,"x"),J.F(p,"y"));node.left=node.point.x;node.right=node.point.x;return node;}
  if(bare.StartsWith("pogo:")||bare.StartsWith("enemy:")){string kind=bare.StartsWith("pogo:")?"pogo":"enemies";var p=J.A(d,kind).Select(J.D).First(v=>J.S(v,"id")==bare.Split(':')[1]);node.point=new Vector2(J.F(p,"x"),J.F(p,"y")+(kind=="pogo"?.5f:1));node.left=node.point.x-.5f;node.right=node.point.x+.5f;return node;}
  if(bare.StartsWith("lift:")){var p=J.A(d,"lifts").Select(J.D).First(v=>J.S(v,"id")==bare.Split(':')[1]);node.left=J.F(p,"x0");node.right=J.F(p,"x1");node.point=new Vector2((node.left+node.right)/2,J.F(p,bare.EndsWith(":lo")?"lo":"hi"));node.surface=true;return node;}
  if(bare.StartsWith("lever:")){var p=J.A(d,"lifts").Select(J.D).First(v=>J.S(v,"id")==bare.Split(':')[1]);var lever=J.A(p,"lever");node.point=new Vector2(J.F(lever[0])-1,J.F(p,"hi"));node.left=node.right=node.point.x;return node;}
  var m=Regex.Match(bare,@"^([a-z]+)(\d+)$");if(!m.Success)throw new Exception("Unknown node "+token);
  var rect=(List<object>)J.A(d,m.Groups[1].Value)[int.Parse(m.Groups[2].Value)];node.left=J.F(rect[0]);node.right=node.left+J.F(rect[2]);node.point=new Vector2(split.Length>1?float.Parse(split[1],System.Globalization.CultureInfo.InvariantCulture):(node.left+node.right)/2,J.F(rect[1])+J.F(rect[3]));if(split.Length>1){node.left=Mathf.Max(node.left,node.point.x-3);node.right=Mathf.Min(node.right,node.point.x+3);}node.surface=true;return node;
 }
 float Feet=>Player.GetComponent<BoxCollider2D>().bounds.min.y;
 bool Alive=>RoomSession.Instance!=null&&Player!=null&&!RoomSession.Instance.Respawning&&Health.HP>0;
 bool Goal(Node node,int initialPogos)
 {
  if(!Alive||Health.KnockbackLocked)return false;
  if(roomId=="MechanicsTest"){
   if(node.token.StartsWith("PAD:"))return FindFirstObjectByType<SlamPad>().Bounces>0&&Feet>=node.point.y;
   if(node.token.StartsWith("SEAL:"))return FindFirstObjectByType<SealFloor>().Broken;
   if(node.token.StartsWith("POGO:")||node.token.StartsWith("ENEMY:"))return Player.GetComponent<PlayerAttack>().PogosPerformed>initialPogos;
   if(node.token.StartsWith("STEAM:"))return Feet>=node.point.y;
   if(node.token.StartsWith("LIFT:"))return liftLowSeen&&Motor.Grounded&&Feet>=FindFirstObjectByType<LeverLift>().high-.1f;
   if(node.token.StartsWith("CRUMBLE:"))return FindFirstObjectByType<CrumblePlatform>().Falling;
   if(node.token.StartsWith("WALL:"))return Feet>=node.point.y&&(!node.token.Contains("@dj")||Mathf.Abs(Body.position.x-node.point.x)<.3f);
  }
  if(RoomSession.Instance.CurrentRoomId!=roomId)return node.destination==RoomSession.Instance.CurrentRoomId;
  if(node.token.StartsWith("pogo:")||node.token.StartsWith("enemy:"))return Player.GetComponent<PlayerAttack>().PogosPerformed>initialPogos;
  if(node.token.StartsWith("lever:"))return FindObjectsByType<LeverLift>(FindObjectsSortMode.None).Any(v=>v.name=="lift:"+node.token.Split(':')[1]&&v.Unlocked);
  if(node.token.StartsWith("lift:")){var lift=FindObjectsByType<LeverLift>(FindObjectsSortMode.None).First(v=>v.name=="lift:"+node.token.Split(':')[1]);if((node.token.Contains("@lift")&&!lift.Unlocked)||Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-node.point.y)>.12f)return false;}
  if(!node.surface)return Vector2.Distance(new Vector2(Body.position.x,Feet),node.point)<.55f;
  // Require actual floor contact on the named surface; proximity in the air is not success.
  return Motor.Grounded&&Body.position.x>=node.left+.08f&&Body.position.x<=node.right-.08f&&Mathf.Abs(Feet-node.point.y)<.12f&&(!node.token.StartsWith("point:")||Mathf.Abs(Body.position.x-node.point.x)<.5f)&&(!worldCapture||!node.token.Contains('~')||Mathf.Abs(Body.position.x-AimFor(node))<.5f);
 }
 IEnumerator Reset(Vector2 start,bool place=true)
 {
  Time.timeScale=1;
  if(fast)yield return Emit(0,false);else yield return Keys();if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);Destroy(GameState.GetOrCreate().gameObject);yield return null;
  ProgressSave.Enabled=false;ProgressSave.Pending=null;ProgressSave.ResetBrokenWalls();ProgressSave.ResetVisitedRooms();ProgressSave.HasSlam=(ability&8)!=0;ProgressSave.BossDefeated=(ability&8)!=0;ProgressSave.EndingReached=false;ProgressSave.ReachedS02=false;
  SceneManager.LoadScene(roomId=="MechanicsTest"?"Assets/Scenes/Test/MechanicsTest.unity":V4SceneMap.Path(roomId));yield return Ready();
  int presetIndex=N5Args.Int("-b10-preset",0);if(presetIndex>0){var vent=FindFirstObjectByType<SteamVent>();var crumble=FindFirstObjectByType<CrumblePlatform>();var lift=FindFirstObjectByType<LeverLift>();var baseline=vent!=null?vent.tuning:crumble!=null?crumble.tuning:lift!=null?lift.tuning:null;if(baseline!=null){var room=FindFirstObjectByType<V4Room>();var host=room!=null?room.gameObject:FindFirstObjectByType<MechanicsTestPresets>().gameObject;var presets=host.GetComponent<MechanicsTestPresets>()??host.AddComponent<MechanicsTestPresets>();presets.baseline=baseline;presets.Select(presetIndex);}}
  Player.GetComponent<PlayerDash>().hasDash=(ability&1)!=0;Motor.hasWallJump=(ability&2)!=0;Motor.hasDoubleJump=(ability&4)!=0;Player.GetComponent<PlayerSlam>().hasSlam=(ability&8)!=0;
  // Only the path's initial setup is placed; every later waypoint uses recorded input.
  if(roomId=="MechanicsTest"&&routeName=="steam"){var vent=FindFirstObjectByType<SteamVent>();yield return Until(()=>!vent.On,"steam preparation off",5);yield return Until(()=>vent.On,"steam preparation fresh on",5);}
  if(roomId=="MechanicsTest"&&routeName=="continuous"){var vent=FindFirstObjectByType<SteamVent>();yield return Until(()=>!vent.On,"course initial phase off",5);yield return Until(()=>vent.On,"course initial phase on",5);}
  if(roomId!="MechanicsTest"){var vents=FindObjectsByType<SteamVent>(FindObjectsSortMode.None);if(vents.Length>0){yield return Until(()=>vents[0].On,"V4 initial steam on",6);yield return Until(()=>!vents[0].On,"V4 initial fresh steam off",6);}}
  // Align setup to the same existing physics/render phase; input is still Dynamic Update.
  while(Time.timeAsDouble-Time.fixedTimeAsDouble>.004)yield return null;
  if(place)PlaceForAuthorizedSetup(start);liftLowSeen=false;Player.ClearTransientInput();if(fast)yield return Emit(0,false);else yield return Keys();yield return new WaitForSeconds(roomId=="MechanicsTest"&&routeName=="lift"?.02f:.12f);
 }
 IEnumerator Emit(int mask,bool record=true)
 {
  var keys=new List<Key>();if((mask&1)!=0)keys.Add(Key.LeftArrow);if((mask&2)!=0)keys.Add(Key.RightArrow);if((mask&4)!=0)keys.Add(Key.Z);if((mask&8)!=0)keys.Add(Key.C);if((mask&16)!=0)keys.Add(Key.X);if((mask&32)!=0)keys.Add(Key.DownArrow);if((mask&64)!=0)keys.Add(Key.UpArrow);
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));double before=Time.realtimeSinceStartupAsDouble;yield return null;
  emittedRealTime+=Time.realtimeSinceStartupAsDouble-before;emittedFrames++;
  if(roomId=="MechanicsTest"){var lift=FindFirstObjectByType<LeverLift>();if(lift!=null&&lift.Unlocked&&Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.low)<.1f)liftLowSeen=true;}
  if(record)recording.frames.Add(new Frame{seconds=Time.unscaledDeltaTime,keys=mask});
 }
 IEnumerator Play(Recording r,bool check=true)
 {
  int pogo=Alive?Player.GetComponent<PlayerAttack>().PogosPerformed:0;double elapsed=0;int index=0;
  while(index<r.frames.Count&&Alive){double begin=Time.realtimeSinceStartupAsDouble;int mask=r.frames[index].keys;yield return Emit(mask,false);if(r.fps==verificationFps){index++;continue;}elapsed+=Time.unscaledDeltaTime;while(index<r.frames.Count&&elapsed>=r.frames[index].seconds){elapsed-=r.frames[index].seconds;index++;}}
  if(check&&RoomSession.Instance?.Transitioning==true)yield return Ready();
  if(check){var node=roomId=="MechanicsTest"?MechanicsNode(r.target):Resolve(r.target);success=Goal(node,pogo)||r.target=="END"&&FindFirstObjectByType<EndingMenu>()!=null;if(!success)Row("FAIL saved goal room="+roomId+" target="+r.target+" actual-room="+RoomSession.Instance?.CurrentRoomId+" pos="+(Player==null?Vector2.zero:Body.position)+" feet="+(Alive?Feet:0)+" HP="+(Alive?Health.HP:0)+" frames="+r.frames.Count);}
 }
 IEnumerator Prepare(Node first)
 {
  yield return Reset(first.point+Vector2.up*(roomId=="MechanicsTest"?.81f:.01f));prefixOK=true;failedPrefix=-1;
  for(int i=0;i<prefix.Count;i++){yield return Play(prefix[i]);if(!success){prefixOK=false;failedPrefix=i;break;}}
 }
 IEnumerable<Candidate> Candidates(Node source,Node target)
 {
  bool downAttack=CoveredFloorDoor(target,"cover"),downSlam=CoveredFloorDoor(target,"seal");
  if(target.token.Contains("@wj")&&source.surface)foreach(float launch in new[]{target.point.x,target.point.x-1,target.point.x+1})foreach(float wall in Steps(0,.1f,.02f))yield return new Candidate{launch=Mathf.Clamp(launch,source.left+.35f,source.right-.35f),hold=.4f,dj=-1,dash=-1,attack=-1,slam=-1,wall=wall,air=1};
  var ending=FindFirstObjectByType<V3SummitEnding>();if(ending!=null&&source.surface){var b=ending.GetComponent<Collider2D>().bounds;float side=target.point.x<source.point.x?1:-1;if((side>0?source.right>b.max.x:source.left<b.min.x)&&(target.point.x-b.center.x)*side<0)foreach(float distance in Steps(1.5f,3.5f,.25f))foreach(int air in new[]{0,1})foreach(float dj in (ability&4)!=0?new[]{.35f,.45f,-1f}:new[]{-1f})yield return new Candidate{launch=b.center.x+side*(b.extents.x+distance),hold=.4f,dj=dj,dash=-1,wall=-1,attack=-1,slam=-1,air=air};}
  if(target.token.Contains("@dj")&&source.surface&&target.surface&&target.point.y>source.point.y+2)foreach(float dj in new[]{.35f,.4f,.45f,.5f,.55f})foreach(float inset in new[]{.4f,.6f,.8f})yield return new Candidate{launch=target.point.x>source.point.x?source.right-inset:source.left+inset,hold=.4f,dj=dj,dash=-1,wall=-1,attack=-1,slam=-1,air=3};
  if(UpcomingWall(source,target)!=null)foreach(float wall in Steps(0,.1f,.02f))foreach(float offset in new[]{0f,.05f,-.05f})yield return new Candidate{launch=UpcomingAnchor(source,UpcomingWall(source,target))+(UpcomingWall(source,target).GetComponent<Collider2D>().bounds.center.x<source.point.x?.07f:-.07f)+offset,hold=.4f,dj=-1,dash=-1,attack=-1,slam=-1,wall=wall,air=1,method=2};
  float direction=target.point.x>=source.point.x?1:-1,near=Mathf.Clamp(source.point.x,source.left+.35f,Mathf.Max(source.left+.35f,source.right-.35f));
  if(target.point.y<source.point.y-.15f)yield return new Candidate{launch=source.point.x,run=0,hold=0,dj=-1,dash=-1,wall=-1,attack=-1,slam=-1,air=1,method=1};
  // First cover the useful center/edge launches, then enumerate the full grid below.
  foreach(float launch in new[]{near,Mathf.Max(source.left+.35f,source.right-.35f),source.left+.35f}.Distinct())
  foreach(float dj in target.token.Contains("@dj")?new[]{.3f,.25f,.35f,.2f,.4f}:new[]{-1f})
   yield return new Candidate{launch=launch,run=0,hold=.4f,dj=dj,dash=target.token.Contains("@dash")?.1f:-1,wall=target.token.Contains("@wj")?.02f:-1,attack=downAttack||target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")||target.token.StartsWith("POGO:")||target.token.StartsWith("ENEMY:")?.1f:-1,slam=downSlam||target.token.Contains("@slam")||source.token.StartsWith("seal")?.3f:-1,air=1,method=0};
  // Try useful full holds first, then the complete documented grid. No geometry claims on truncation.
  float[] holds=N5Args.Has("-b10-expanded")?Steps(.1f,.4f,.02f).ToArray():new[]{.4f,.3f,.2f,.1f,.15f,.25f,.35f};
  IEnumerable<float> positions=Positions(source,near,N5Args.Has("-b10-expanded")?.1f:.25f);
  foreach(float launch in positions)foreach(float run in new[]{0f,1f,2f})foreach(float hold in holds)foreach(int air in new[]{1,0,2})
  foreach(float dj in target.token.Contains("@dj")?Steps(.15f,.55f,.05f):new[]{-1f})
  foreach(float dash in target.token.Contains("@dash")?Steps(0,.3f,.05f):new[]{-1f})
  foreach(float wall in target.token.Contains("@wj")?Steps(0,.1f,.02f):new[]{-1f})
  foreach(float attack in downAttack||target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")||target.token.StartsWith("POGO:")||target.token.StartsWith("ENEMY:")?Steps(0,.6f,.05f):new[]{-1f})
  foreach(float slam in downSlam||target.token.Contains("@slam")||source.token.StartsWith("seal")?new[]{.15f,.3f,.45f,.6f}:new[]{-1f})
  foreach(float wait in target.token.Contains("@steam")?new[]{0f,1f,2f,3f}:new[]{0f})
   yield return new Candidate{launch=launch,run=run,hold=hold,dj=dj,dash=dash,wall=wall,attack=attack,slam=slam,wait=wait,air=air};
 }
 IEnumerable<float> Positions(Node n,float first,float step){yield return first;if(!n.surface)yield break;float edge=n.right-n.left<.7f?first:n.right-.35f;yield return edge;yield return n.left+.35f;for(float x=n.left+.35f;x<=edge+.001f;x+=step)if(Mathf.Abs(x-first)>.001f&&Mathf.Abs(x-edge)>.001f)yield return x;}
 bool CoveredFloorDoor(Node node,string kind){if(node.destination==null||node.point.y>1)return false;return J.A(J.D(rooms[roomId]),kind).Any(obj=>{var r=(List<object>)obj;return node.point.x>=J.F(r[0])&&node.point.x<=J.F(r[0])+J.F(r[2])&&J.F(r[1])<=1;});}
 IEnumerable<float> Steps(float lo,float hi,float step){for(int i=0;i<=Mathf.RoundToInt((hi-lo)/step);i++)yield return lo+i*step;}
 void Measure(Node source,Node target)
 {
  if(!Alive||RoomSession.Instance.CurrentRoomId!=roomId)return;float dist=Vector2.Distance(new Vector2(Body.position.x,Feet),target.point);bestHeight=Mathf.Max(bestHeight,Feet);bestTravel=Mathf.Max(bestTravel,Mathf.Abs(Body.position.x-source.point.x));
  if(dist<bestDistance){bestDistance=dist;bestPosition=new Vector2(Body.position.x,Feet);int n=Player.GetComponent<Collider2D>().GetContacts(contactBuffer);contacts=string.Join(",",contactBuffer.Take(n).Select(c=>c.collider==Player.GetComponent<Collider2D>()?c.otherCollider.name:c.collider.name).Distinct());}
 }
 IEnumerator Attempt(Node source,Node target,Candidate c)
 {
  recording=new Recording{room=roomId,route=routeName,source=source.token,target=target.token,start=Body.position,abilities=ability,fps=verificationFps};success=false;dead=false;int pogo=Player.GetComponent<PlayerAttack>().PogosPerformed;
  if(RoomSession.Instance.CurrentRoomId!=roomId){success=Goal(target,pogo);recording.end=Body.position;yield break;}
  if(source.surface&&target.point.y>source.point.y+.2f)yield return ClearTurretAbove(source);
  int direction=target.point.x>=source.point.x?2:1;double started=Time.timeAsDouble,nextEnemyAttack=0,enemyLockUntil=-1;
  // Walk to run-up position using ordinary movement; do not teleport to the takeoff grid.
  float support=c.method==2?-Player.GetComponent<Collider2D>().bounds.extents.x+.05f:.35f;float runX=Mathf.Clamp(c.launch+(direction==2?-c.run:c.run),source.left+support,Mathf.Max(source.left+support,source.right-support));
  double runLimit=Mathf.Abs(Body.position.x-runX)/Motor.Tuning.moveSpeed+4,runJumpUntil=-1;
  if(source.surface)while(Alive&&RoomSession.Instance.CurrentRoomId==roomId&&Mathf.Abs(Body.position.x-runX)>.13f&&Time.timeAsDouble-started<runLimit){int runMask=Body.position.x<runX?2:1;var bounds=Player.GetComponent<Collider2D>().bounds;var obstacle=Physics2D.BoxCast(bounds.center,new Vector2(bounds.size.x*.8f,bounds.size.y*.85f),0,runMask==2?Vector2.right:Vector2.left,.65f,1<<6);if(Motor.Grounded&&obstacle.collider!=null&&obstacle.collider.bounds.max.y>Feet+.1f&&obstacle.collider.bounds.min.y<bounds.max.y-.1f&&Time.timeAsDouble>runJumpUntil+.1)runJumpUntil=Time.timeAsDouble+.4;if(Time.timeAsDouble<runJumpUntil)runMask|=4;runMask=CombatMask(runMask,target,ref nextEnemyAttack,ref enemyLockUntil);runMask=SteamCrossingMask(runMask,target);yield return Emit(runMask);Measure(source,target);}
  if(!Alive||GameState.GetOrCreate().Completing||RoomSession.Instance.CurrentRoomId!=roomId){dead=!Alive;recording.end=Player==null?Vector2.zero:Body.position;yield break;}
  yield return Emit(0);double wait=Time.timeAsDouble+c.wait;while(Alive&&!GameState.GetOrCreate().Completing&&Time.timeAsDouble<wait)yield return Emit(0);
  bool launched=false,slamSent=false,dashSent=false,airReleased=false;double reverseAt=-1,hatchJumpUntil=-1,hatchAttackAt=-1;double launchTime=0;int wallCount=Motor.WallJumpsPerformed;double wallAt=-1,wallHoldUntil=-1;int wallPulse=0,wallSide=0;int attackCount=0;int bounces=FindObjectsByType<SlamPad>(FindObjectsSortMode.None).Sum(p=>p.Bounces);double until=Time.timeAsDouble+(target.token.StartsWith("LIFT:")||target.token.StartsWith("lift:")?22:target.token.Contains("@wj")?30:Mathf.Max(7,Mathf.Abs(target.point.x-source.point.x)/Motor.Tuning.moveSpeed+6));
  while(Alive&&!GameState.GetOrCreate().Completing&&RoomSession.Instance.CurrentRoomId==roomId&&Time.timeAsDouble<until)
  {
   if(Goal(target,pogo)){success=true;break;}
   int mask=direction;float aim=AimFor(target);float dx=aim-Body.position.x;
   if(c.air==3&&launched&&Feet<target.point.y-.6f)mask&=~3;
   if(!launched&&(!source.surface||(direction==2?Body.position.x>=c.launch-.12f:Body.position.x<=c.launch+.12f))){launched=true;launchTime=Time.timeAsDouble;}
   float t=launched?(float)(Time.timeAsDouble-launchTime):-1;
   if(launched&&t>1.5f&&Motor.Grounded&&target.surface&&target.point.y>Feet+.15f&&!target.token.Contains("@wj")&&!target.token.Contains("@steam")&&!target.token.Contains("@lift")&&!target.token.StartsWith("LIFT:")&&!target.token.StartsWith("CRUMBLE:"))break;
   bool onTarget=target.surface&&Mathf.Abs(Feet-target.point.y)<.12f&&Body.position.x>=target.left-Player.GetComponent<Collider2D>().bounds.extents.x&&Body.position.x<=target.right+Player.GetComponent<Collider2D>().bounds.extents.x;
   if(launched&&Motor.Grounded&&c.hold>0&&t>c.hold+.2f&&!(airReleased&&onTarget)){var bounds=Player.GetComponent<Collider2D>().bounds;var obstacle=Physics2D.BoxCast(bounds.center,new Vector2(bounds.size.x*.8f,bounds.size.y*.85f),0,direction==2?Vector2.right:Vector2.left,.8f,1<<6);if(obstacle.collider!=null&&obstacle.collider.bounds.max.y>Feet+.1f&&obstacle.collider.bounds.min.y<bounds.max.y-.1f){launchTime=Time.timeAsDouble;t=0;dashSent=false;airReleased=false;}}
   if(launched){float firstHold=c.dj>=0?Mathf.Min(c.hold,Mathf.Max(0,c.dj-.04f)):c.hold;if(t<firstHold)mask|=4;if(c.dj>=0&&t>=c.dj&&t<c.dj+c.hold)mask|=4;if(!dashSent&&c.dash>=0&&t>=c.dash){mask|=8;dashSent=true;}
    float vx=Mathf.Abs(Body.linearVelocity.x),brake=Mathf.Max(.25f,vx*vx/(2*Motor.Tuning.deceleration*(Motor.Grounded?1:Motor.Tuning.airControl))+vx*(Time.fixedDeltaTime+1f/verificationFps));
    if(c.air==1&&(airReleased||Mathf.Abs(dx)<brake||dx*(direction==2?1:-1)<0)){mask&=~3;airReleased=true;}else if(c.air==2&&dx*(direction==2?1:-1)<.2f){if(reverseAt<0)reverseAt=Time.realtimeSinceStartupAsDouble;mask=(mask&~3)|(Time.realtimeSinceStartupAsDouble-reverseAt<.1?(direction==2?1:2):0);}
    if(c.attack>=0&&t>=c.attack&&t>=c.attack+attackCount*.32f){mask|=32|16;attackCount++;}
    bool padSetup=source.token.StartsWith("pad")&&target.token.Contains("@slam")&&FindObjectsByType<SlamPad>(FindObjectsSortMode.None).Sum(p=>p.Bounces)==bounces;
    if(padSetup)mask&=~3;
    if(!slamSent&&c.slam>=0&&t>=c.slam&&!Motor.Grounded&&(padSetup||source.token.StartsWith("seal")||Mathf.Abs(dx)<.6f)){mask=(mask&~3)|32|8;slamSent=true;}
   }
   if(c.air==1&&airReleased&&Motor.Grounded&&onTarget)mask=(mask&~3)|(Mathf.Abs(dx)<.035f?0:dx>0?2:1);
   if(target.token.Contains("@wj"))
   {
    var upcoming=c.method==2?UpcomingWall(source,target):null;if(upcoming!=null&&t>=0&&Feet<source.point.y+Motor.Tuning.jumpVelocity*Motor.Tuning.jumpVelocity/(2*Motor.Tuning.gravity)+.3f&&Player.GetComponent<Collider2D>().bounds.max.y<upcoming.GetComponent<Collider2D>().bounds.min.y+.02f){float anchor=UpcomingAnchor(source,upcoming);mask=(mask&~3)|(Mathf.Abs(Body.position.x-anchor)<.08f?0:Body.position.x<anchor?2:1);}
    var walls=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).Where(w=>roomId=="B05"||Mathf.Abs(w.transform.position.x-target.point.x)<12).ToArray();float head=Player.GetComponent<Collider2D>().bounds.max.y;var available=walls.Where(w=>w.GetComponent<Collider2D>().bounds.max.y>Feet+.1f&&w.GetComponent<Collider2D>().bounds.min.y<head+.1f).ToArray();var wall=available.OrderBy(w=>Mathf.Abs(w.transform.position.x-Body.position.x)).FirstOrDefault();
    if(Motor.WallJumpsPerformed>wallCount){wallCount=Motor.WallJumpsPerformed;wallSide=wallSide==2?1:2;wallHoldUntil=Time.timeAsDouble+c.hold;wallPulse=0;wallAt=-1;if(target.token.Contains("@dj"))launchTime=Time.timeAsDouble;}
    if(wall!=null&&!Motor.Grounded&&Feet<(target.destination==null?target.point.y+.6f:target.point.y-Player.GetComponent<Collider2D>().bounds.size.y+.1f)){bool outside=Body.position.x<walls.Min(w=>w.GetComponent<Collider2D>().bounds.min.x)||Body.position.x>walls.Max(w=>w.GetComponent<Collider2D>().bounds.max.x);if(wallSide==0||outside||available.Length==1)wallSide=wall.transform.position.x>Body.position.x?2:1;mask=(mask&~3)|wallSide;}
    if(Time.timeAsDouble<wallHoldUntil&&!target.token.Contains("@dj"))mask|=4;
    if(Motor.WallSliding&&Feet<(target.destination==null?target.point.y+.6f:target.point.y-Player.GetComponent<Collider2D>().bounds.size.y+.1f)){if(wallAt<0)wallAt=Time.timeAsDouble;if(wallPulse==2&&Time.timeAsDouble-wallAt>c.wall+.12){wallPulse=0;wallAt=Time.timeAsDouble;}if(Time.timeAsDouble-wallAt>=c.wall){if(wallPulse==0){mask&=~4;wallPulse=1;}else{mask|=4;wallPulse=2;}}}
    else{wallAt=-1;wallPulse=0;}
    if(Feet>=(target.destination==null?target.point.y+.6f:target.point.y-Player.GetComponent<Collider2D>().bounds.size.y+.1f))mask=(mask&~3)|(Mathf.Abs(dx)<.08f?0:dx>0?2:1);
   }
   if(target.token.Contains("@steam")){var vent=FindObjectsByType<SteamVent>(FindObjectsSortMode.None).OrderBy(v=>Vector2.Distance(v.transform.position,source.point)).FirstOrDefault();if(vent!=null&&Feet<target.point.y-.2f){float center=vent.GetComponent<Collider2D>().bounds.center.x;mask=(mask&~3)|(Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1);}}
   if(target.token.StartsWith("lever:")){var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>v.upper);float anchor=lever.transform.position.x-Player.GetComponent<PlayerAttack>().tuning.attackRange*.7f;mask=Mathf.Abs(Body.position.x-anchor)<.15f?0:Body.position.x<anchor?2:1;if(Mathf.Abs(Body.position.x-anchor)<.2f&&(int)(Time.time*10)%3==0)mask=2|16;}
   if(target.token.StartsWith("lift:")){var lift=FindFirstObjectByType<LeverLift>();float center=lift.GetComponent<Collider2D>().bounds.center.x,top=lift.GetComponent<Collider2D>().bounds.max.y;
    if(target.token.Contains("@lift")&&!lift.Unlocked){var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>v.upper);float anchor=lever.transform.position.x+Player.GetComponent<PlayerAttack>().tuning.attackRange*.7f;mask=Mathf.Abs(Body.position.x-anchor)<.15f?0:Body.position.x<anchor?2:1;if(Mathf.Abs(Body.position.x-anchor)<.2f&&(int)(Time.time*10)%3==0)mask=1|16;}
    else if(target.token.Contains(":hi")&&ProgressSave.IsWallBroken(lift.liftId+"-down")){var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>!v.upper);float anchor=lever.transform.position.x+Player.GetComponent<PlayerAttack>().tuning.attackRange+.3f;mask=Mathf.Abs(Body.position.x-anchor)<.1f?0:Body.position.x<anchor?2:1;if(Mathf.Abs(Body.position.x-anchor)<.3f&&(int)(Time.time*10)%3==0)mask=1|16;}
    else{mask=Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1;if(target.token.Contains(":hi")&&Feet<top-.15f)mask|=4;}
   }
   if(target.token.StartsWith("LIFT:")){
    var lift=FindFirstObjectByType<LeverLift>();float center=lift.GetComponent<Collider2D>().bounds.center.x;
    if(!lift.Unlocked){var upper=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>v.upper);mask=(upper.transform.position.x>Body.position.x?2:1)|((int)(Mathf.Max(0,t)*10)%3==0?16:0);}
    else if(!liftLowSeen){mask=Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1;}
    else if(!ProgressSave.IsWallBroken(lift.liftId+"-down")){mask=Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1;if(Mathf.Abs(Feet-lift.GetComponent<Collider2D>().bounds.max.y)>.15f)mask|=4;}
    else{var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>!v.upper);float anchor=lever.transform.position.x+Player.GetComponent<PlayerAttack>().tuning.attackRange+.3f;mask=Mathf.Abs(Body.position.x-anchor)<.1f?1:Body.position.x<anchor?2:1;if(Mathf.Abs(Body.position.x-anchor)<.3f&&(int)(Time.time*10)%3==0)mask|=16;}
   }
   if(target.token.StartsWith("CRUMBLE:")&&Motor.Grounded&&Feet>=target.point.y-.15f)mask=0;
   if(!target.token.StartsWith("cover")){var wall=FindObjectsByType<BreakableWall>(FindObjectsSortMode.None).FirstOrDefault(w=>!w.Broken&&Mathf.Abs(w.transform.position.x-Body.position.x)<w.GetComponent<Collider2D>().bounds.extents.x+Player.GetComponent<PlayerAttack>().tuning.attackRange+.4f&&w.GetComponent<Collider2D>().bounds.max.y>Feet&&w.GetComponent<Collider2D>().bounds.min.y<Feet+1.6f);if(wall!=null&&(int)(Time.time*10)%4==0)mask=(mask&~3)|(wall.transform.position.x>Body.position.x?2:1)|16;}
   if(CoveredFloorDoor(target,"cover")&&Mathf.Abs(dx)<.6f){if(Motor.Grounded&&Time.timeAsDouble>hatchJumpUntil+.1){hatchJumpUntil=Time.timeAsDouble+.4;hatchAttackAt=Time.timeAsDouble+Mathf.Max(.1f,c.attack);}if(Time.timeAsDouble<hatchJumpUntil)mask|=4;if(Time.timeAsDouble>=hatchAttackAt&&Time.timeAsDouble<hatchAttackAt+.06)mask|=32|16;}
   mask=CombatMask(mask,target,ref nextEnemyAttack,ref enemyLockUntil);mask=SteamCrossingMask(mask,target);
   Measure(source,target);yield return Emit(mask);
  }
  if(Alive&&RoomSession.Instance.CurrentRoomId!=roomId){while(RoomSession.Instance.Transitioning)yield return Emit(0);yield return Emit(0);success=target.destination==RoomSession.Instance.CurrentRoomId;}
  if(Alive&&Goal(target,pogo))success=true;
  if(success&&Alive&&RoomSession.Instance.CurrentRoomId==roomId){double settle=Time.timeAsDouble+.12;while(Alive&&Time.timeAsDouble<settle)yield return Emit(0);success=Goal(target,pogo);}
  dead=!Alive;recording.end=Player==null?Vector2.zero:Body.position;
 }
 float AimFor(Node target)
 {
  if(!target.surface)return target.point.x;float half=Player.GetComponent<Collider2D>().bounds.extents.x,aim=Mathf.Clamp(target.point.x,target.left+half+.07f,Mathf.Max(target.left+half+.07f,target.right-half-.07f));
  foreach(var obj in J.A(J.D(rooms[roomId]),"solid")){var r=(List<object>)obj;float left=J.F(r[0]),right=left+J.F(r[2]);if(J.F(r[1])+J.F(r[3])<=target.point.y+.12f||J.F(r[1])>=target.point.y+Player.GetComponent<Collider2D>().bounds.size.y||aim+half<left||aim-half>right)continue;float a=left-half-.07f,b=right+half+.07f;float free=Mathf.Abs(a-target.point.x)<Mathf.Abs(b-target.point.x)?a:b;if(free>=target.left+half&&free<=target.right-half)aim=free;}
  return aim;
 }
 ClimbableWall UpcomingWall(Node source,Node target)
 {
  if(!target.token.Contains("@wj")||target.point.y<=source.point.y+2)return null;float height=Player.GetComponent<Collider2D>().bounds.size.y,reach=Motor.Tuning.jumpVelocity*Motor.Tuning.jumpVelocity/(2*Motor.Tuning.gravity)+height+.2f;
  return FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).Where(w=>{var b=w.GetComponent<Collider2D>().bounds;return b.min.y>source.point.y+height&&b.min.y<source.point.y+reach&&(Mathf.Abs(b.min.x-source.left)<.2f||Mathf.Abs(b.max.x-source.right)<.2f);}).OrderBy(w=>w.GetComponent<Collider2D>().bounds.min.y).FirstOrDefault();
 }
 float UpcomingAnchor(Node source,ClimbableWall wall){var b=wall.GetComponent<Collider2D>().bounds;float half=Player.GetComponent<Collider2D>().bounds.extents.x;return Mathf.Abs(b.min.x-source.left)<.2f?b.min.x-half-.02f:b.max.x+half+.02f;}
 IEnumerator ClearTurretAbove(Node source)
 {
  if(FindObjectsByType<CrumblePlatform>(FindObjectsSortMode.None).Any(p=>Mathf.Abs(p.GetComponent<Collider2D>().bounds.max.y-Feet)<.15f&&p.GetComponent<Collider2D>().bounds.min.x<=Body.position.x&&p.GetComponent<Collider2D>().bounds.max.x>=Body.position.x))yield break;
  var attack=Player.GetComponent<PlayerAttack>().tuning;float half=Player.GetComponent<Collider2D>().bounds.extents.x;
  foreach(var reset in FindObjectsByType<EnemyReset>(FindObjectsSortMode.None)){if(reset.Live==null||reset.Live.GetComponent<EnemyTurret>()==null)continue;var enemy=reset.Live;var bounds=enemy.GetComponent<Collider2D>().bounds;float anchor=Mathf.Clamp(bounds.center.x,source.left+half+.08f,Mathf.Max(source.left+half+.08f,source.right-half-.08f));float reach=Motor.Tuning.jumpVelocity*Motor.Tuning.jumpVelocity/(2*Motor.Tuning.gravity)+Player.GetComponent<Collider2D>().bounds.size.y+attack.verticalAttackHeight;
   if(bounds.min.y-Feet<2.2f||bounds.min.y-Feet>reach||Mathf.Abs(anchor-bounds.center.x)>attack.verticalAttackWidth/2+bounds.extents.x-.05f)continue;
   double end=Time.timeAsDouble+10;while(Alive&&enemy!=null&&Mathf.Abs(Body.position.x-anchor)>.12f&&Time.timeAsDouble<end)yield return Emit(Body.position.x<anchor?2:1);yield return Emit(0);double cycle=Time.timeAsDouble;
   while(Alive&&enemy!=null&&Time.timeAsDouble<end){float t=(float)((Time.timeAsDouble-cycle)%1.25);int mask=64;if(t<.4f)mask|=4;if(t>=.28f&&t<.34f)mask|=16;yield return Emit(mask);}
   while(Alive&&!Motor.Grounded&&Time.timeAsDouble<end)yield return Emit(0);yield return Emit(0);Row("MEASURE actual upward attack detour turret="+reset.name+" destroyed="+(enemy==null)+" HP="+(Alive?Health.HP:0));
  }
 }
 int CombatMask(int mask,Node target,ref double next,ref double locked)
 {
  if(target.token.StartsWith("enemy:")||target.token.StartsWith("pogo:")||target.token.StartsWith("cover"))return mask;
  var enemy=FindObjectsByType<EnemyReset>(FindObjectsSortMode.None).FirstOrDefault(e=>e.Live!=null&&Mathf.Abs(e.Live.transform.position.x-Body.position.x)<1.9f&&Mathf.Abs(e.Live.transform.position.y-Body.position.y)<2);
  if(enemy==null)return mask;float dx=enemy.Live.transform.position.x-Body.position.x;mask&=~3;if(Time.timeAsDouble>=locked){if(Mathf.Abs(dx)>1.6f)mask|=dx>0?2:1;else if(Mathf.Abs(dx)<1.25f)mask|=dx>0?1:2;}
  if(Time.timeAsDouble>=next&&Mathf.Abs(dx)<=1.65f){mask=(mask&~3)|(dx>0?2:1)|16;var t=Player.GetComponent<PlayerAttack>().tuning;next=Time.timeAsDouble+t.attackCooldown+.04;locked=Time.timeAsDouble+t.attackStartup+t.attackActive;}return mask;
 }
 int SteamCrossingMask(int mask,Node target)
 {
  if(!target.surface||target.token.Contains("@steam"))return mask;float sign=(mask&2)!=0?1:(mask&1)!=0?-1:0;if(sign==0)return mask;
  foreach(var vent in FindObjectsByType<SteamVent>(FindObjectsSortMode.None)){var area=vent.GetComponent<Collider2D>().bounds;if(target.point.y>=area.max.y-.2f||Feet>=area.max.y)continue;float edge=sign>0?area.min.x:area.max.x,distance=(edge-Body.position.x)*sign;if(distance<0||distance>.6f)continue;float phase=Time.time%(vent.tuning.steamOn+vent.tuning.steamOff),remaining=vent.tuning.steamOn+vent.tuning.steamOff-phase;if(vent.On||remaining<(area.size.x+.4f)/Motor.Tuning.moveSpeed+.15f)return mask&~3;}return mask;
 }
 bool TimingValid(string file){try{var r=JsonUtility.FromJson<Recording>(File.ReadAllText(file));double ratio=r.frames.Sum(f=>(double)f.seconds)/Math.Max(1,r.frames.Count)*r.fps;return ratio>.7&&ratio<1.4;}catch{return false;}}
 string FileFor(int index){string stem=roomId+"_"+routeName+"_"+index;return Directory.GetFiles(root,stem+"-r*.json").Where(TimingValid).Where(p=>{var r=JsonUtility.FromJson<Recording>(File.ReadAllText(p));if(roomId=="B03"&&routeName=="climb"&&index==6&&!r.frames.Any(f=>(f.keys&80)==80))return false;return tokens==null||index>=tokens.Count||index==0||r.source==tokens[index-1]&&r.target==tokens[index];}).OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault()??Path.Combine(root,stem+".json");}
 void ExpandLiftApproach()
 {
  if(tokens.TakeWhile(t=>!t.Contains(":lo")).Any(t=>t.StartsWith("lift:")&&t.Contains(":hi")))return;
  int low=tokens.FindIndex(t=>t.StartsWith("lift:")&&t.Contains(":lo"));if(low<1||tokens.Any(t=>t.StartsWith("lever:")))return;string id=tokens[low].Split(':')[1];var d=J.D(rooms[roomId]);var lift=J.A(d,"lifts").Select(J.D).First(v=>J.S(v,"id")==id);float hi=J.F(lift,"hi"),x=J.F(J.A(lift,"lever")[0]);List<string> best=null;float cost=float.PositiveInfinity;
  foreach(var route in J.D(d["routes"])){var path=((List<object>)route.Value).Select(t=>t.ToString()).ToList();if(path.Any(t=>t.StartsWith("lift:")))continue;for(int i=0;i<path.Count;i++){var node=Resolve(path[i]);float distance=Mathf.Abs(node.point.x-x);if(!node.surface||Mathf.Abs(node.point.y-hi)>.15f||distance>=cost)continue;cost=distance;best=path.Take(i+1).ToList();}}
  if(best==null)return;var origin=Resolve(tokens[low-1]);var first=Resolve(best[0]);if((ability&1)!=0&&origin.surface&&first.surface&&(origin.left-first.right>3||first.left-origin.right>3))best[0]+="@dash";best.Add("lever:"+id);tokens.InsertRange(low,best);Row("MEASURE lift upper lever approach added actual-input nodes="+best.Count+"; original lift low/high goals retained, no intermediate placement");
 }
 IEnumerator SearchRoute()
 {
  ExpandLiftApproach();
  for(int i=tokens.Count-2;i>0;i--){var support=Resolve(tokens[i]);var exit=Resolve(tokens[i+1]);if((tokens[i].StartsWith("seal")||tokens[i].StartsWith("cover"))&&exit.destination!=null&&exit.point.y<1&&exit.point.x>=support.left&&exit.point.x<=support.right){Row("MEASURE covered floor approach goes directly to its exit; no intermediate destroyed-surface goal="+tokens[i]);tokens.RemoveAt(i);}}
  if(tokens.Count<2){var node=Resolve(tokens[0]);yield return Prepare(node);recording=new Recording{room=roomId,route=routeName,source=node.token,target=node.token,abilities=ability,fps=verificationFps,start=Body.position};for(int i=0;i<verificationFps/2;i++)yield return Emit(0);var saved=recording;string file=Path.Combine(root,roomId+"_"+routeName+"_0-r"+DateTime.UtcNow.ToString("HHmmssffff")+".json");File.WriteAllText(file,JsonUtility.ToJson(saved,true));saved=JsonUtility.FromJson<Recording>(File.ReadAllText(file));bool both=true;for(int rep=0;rep<2;rep++){yield return Prepare(node);yield return Play(saved);both&=success;Row((success?"PASS":"FAIL")+" stationary saved replay "+roomId+"/"+routeName+" repetition="+(rep+1));}Row((both?"PASS":"FAIL")+" route "+roomId+"/"+routeName+" replay=2 stationary input="+file);if(both)pass++;else deferred++;yield break;}
  var first=Resolve(tokens[0]);
  for(int index=prefix.Count+1;index<tokens.Count;index++)
  {
   Node source=Resolve(tokens[index-1]),target=Resolve(tokens[index]);bestDistance=float.PositiveInfinity;bestHeight=bestTravel=float.NegativeInfinity;bestPosition=Vector2.zero;contacts="";bool found=false,exhausted=true;int tries=0,gridIndex=0;int part=N5Args.Int("-b10-part",0),parts=N5Args.Int("-b10-parts",1);
   string cursor=CursorFile(index,part,parts);int completed=0;bool everReached=false;if(File.Exists(cursor)){var prior=File.ReadAllLines(cursor);string last=prior.LastOrDefault();if(last!=null)int.TryParse(last.Split('\t')[0],out completed);everReached=prior.Any(row=>row.Split('\t').Skip(1).FirstOrDefault()=="True");}Row("MEASURE candidate partition "+roomId+"/"+routeName+"/"+index+" part="+part+" parts="+parts+" completed-grid-index="+completed+" cursor="+cursor);
   string file=FileFor(index);
   bool restarting=false;
   if(!untrustedTargets.Contains(roomId+"/"+target.token)&&File.Exists(file)&&TimingValid(file)){var r=JsonUtility.FromJson<Recording>(File.ReadAllText(file));bool both=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){both=false;break;}yield return Play(r);both&=success;}if(both){prefix.Add(r);Row("PASS cached segment "+roomId+"/"+routeName+"/"+index+" replay=2");continue;}}
   if(routeName.StartsWith("isolation-")||routeName.StartsWith("return-")||routeName.StartsWith("roundtrip-return-")){foreach(string shared in Directory.GetFiles(root,roomId+"_*-r*.json").OrderByDescending(File.GetLastWriteTimeUtc).Where(TimingValid)){var r=JsonUtility.FromJson<Recording>(File.ReadAllText(shared));if(r.source!=source.token||r.target!=target.token)continue;bool both=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){both=false;break;}yield return Play(r);both&=success;if(!success)break;}if(both){prefix.Add(r);found=true;Row("PASS shared physical source-target input "+roomId+"/"+routeName+"/"+index+" abilities="+ability+" replay=2 input="+shared);break;}if(Time.realtimeSinceStartupAsDouble-itemStart>2400)break;}if(found)continue;}
   foreach(var c in Candidates(source,target))
   {
    if(gridIndex++%parts!=part)continue;
    if(gridIndex<=completed)continue;
    if(Time.realtimeSinceStartupAsDouble-itemStart>2400){exhausted=false;break;}
    yield return Prepare(first);if(!prefixOK){exhausted=false;Row("FAIL prefix replay "+roomId+"/"+routeName+"/"+index+" pos="+(Player==null?Vector2.zero:Body.position)+" feet="+(Alive?Feet:0));if(failedPrefix>=0){untrustedTargets.Add(roomId+"/"+prefix[failedPrefix].target);prefix.RemoveRange(failedPrefix,prefix.Count-failedPrefix);index=failedPrefix;restarting=true;Row("MEASURE repair unstable prefix from segment="+(failedPrefix+1)+"; initial fixture unchanged");}break;}
    yield return Attempt(source,target,c);tries++;attempted++;
    File.AppendAllText(cursor,gridIndex+"\t"+success+"\t"+recording.end+"\t"+JsonUtility.ToJson(c)+Environment.NewLine);
    if(tries<=5)Row("MEASURE attempt "+roomId+"/"+routeName+"/"+index+" number="+tries+" success="+success+" dead="+dead+" end="+recording.end+" closest="+bestDistance+" grounded="+(Alive&&Motor.Grounded)+" feet="+(Alive?Feet:0)+" candidate="+JsonUtility.ToJson(c));
    if(tries%25==0){Row("MEASURE search "+roomId+"/"+routeName+"/"+index+" attempts="+tries+" closest="+bestDistance+" peak="+bestHeight+" travel="+bestTravel+" candidate="+JsonUtility.ToJson(c));string shared=FileFor(index);if(File.Exists(shared)&&TimingValid(shared)){var qualified=JsonUtility.FromJson<Recording>(File.ReadAllText(shared));bool sharedBoth=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){sharedBoth=false;break;}yield return Play(qualified);sharedBoth&=success;}if(sharedBoth){prefix.Add(qualified);found=true;Row("PASS shared saved segment "+roomId+"/"+routeName+"/"+index+" replay=2 input="+shared);break;}}}
    if(!success)continue;everReached=true;
    var saved=recording;string candidateFile=Path.Combine(root,roomId+"_"+routeName+"_"+index+"-candidate-"+DateTime.UtcNow.ToString("HHmmssffff")+"-"+Guid.NewGuid().ToString("N").Substring(0,8)+".json");File.WriteAllText(candidateFile,JsonUtility.ToJson(saved,true));
    // Verify by reading the saved file, never by a second adaptive controller.
    saved=JsonUtility.FromJson<Recording>(File.ReadAllText(candidateFile));bool both=true;
    for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){both=false;break;}yield return Play(saved);both&=success;Row((success?"PASS":"FAIL")+" replay "+roomId+"/"+routeName+"/"+index+" repetition="+(rep+1));if(!success)break;}
    if(!both)continue;
    file=Path.Combine(root,roomId+"_"+routeName+"_"+index+"-r"+DateTime.UtcNow.ToString("HHmmssffff")+"-"+Guid.NewGuid().ToString("N").Substring(0,8)+".json");File.Copy(candidateFile,file);prefix.Add(saved);found=true;Row("PASS segment "+roomId+"/"+routeName+"/"+index+" source="+source.token+" target="+target.token+" attempts="+tries+" replay=2 input="+file);break;
   }
   if(restarting)continue;
   if(!found){string status=exhausted?(everReached?"REPLAY-UNSTABLE":parts>1?"PARTITION-EXHAUSTED":"BLOCKED-GEOMETRY"):"BUDGET-DEFERRED";Row(status+" route "+roomId+"/"+routeName+" segment="+index+" source="+source.token+" target="+target.token+" failure="+bestPosition+" closest="+bestDistance+" peak="+bestHeight+" travel="+bestTravel+" contacts="+contacts+" attempts="+tries+" grid="+gridIndex+" part="+part+" parts="+parts+" reached="+everReached+" classification=pending-physical-cause");if(exhausted&&parts==1&&!everReached)blocked++;else deferred++;yield break;}
  }
  // Entire route also needs two uninterrupted replays from its single initial fixture.
  string wholeFile=Path.Combine(root,roomId+"_"+routeName+"-whole-"+DateTime.UtcNow.ToString("HHmmssffff")+"-"+Guid.NewGuid().ToString("N").Substring(0,8)+".json");File.WriteAllText(wholeFile,JsonUtility.ToJson(new CourseRecording{segments=prefix},true));prefix=JsonUtility.FromJson<CourseRecording>(File.ReadAllText(wholeFile)).segments;
  bool routeOK=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);routeOK&=prefixOK;}
  Row((routeOK?"PASS":"FAIL")+" route "+roomId+"/"+routeName+" replay=2 segments="+prefix.Count+" whole-input="+wholeFile);if(routeOK)pass++;else deferred++;
 }
 string CursorFile(int index,int part,int parts)
 {
  string scope="B10-search-r63|"+Resources.Load<TextAsset>("V4Design").text+"|"+roomId+"|"+routeName+"|"+string.Join("/",tokens)+"|"+ability+"|"+verificationFps+"|"+N5Args.Value("-b10-preset","0");
  using(var hash=System.Security.Cryptography.SHA256.Create()){string key=System.BitConverter.ToString(hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(scope))).Replace("-","").Substring(0,24);return Path.Combine(root,"cursor-"+key+"-s"+index+"-p"+part+"of"+parts+".txt");}
 }
 IEnumerator ReplayAll()
 {
  string manifest=N5Args.Value("-b10-manifest","");
  if(manifest!=""){
   foreach(string row in File.ReadAllLines(manifest)){var fields=row.Split('\t');if(fields.Length<3)continue;roomId=fields[0];routeName=fields[1];if(N5Args.Value("-b10-room",roomId)!=roomId)continue;var bundle=JsonUtility.FromJson<CourseRecording>(File.ReadAllText(fields[2]));prefix=bundle.segments;if(prefix==null||prefix.Count==0){Row("FAIL empty registered input "+row);deferred++;continue;}tokens=new List<string>{prefix[0].source};tokens.AddRange(prefix.Select(r=>r.target));ability=prefix[0].abilities;bool both=true;for(int rep=0;rep<2;rep++){yield return Prepare(Resolve(tokens[0]));both&=prefixOK;}Row((both?"PASS":"FAIL")+" fps-replay "+roomId+"/"+routeName+" fps="+verificationFps+" replay=2 input="+fields[2]);if(both)pass++;else deferred++;
   }yield break;
  }
  foreach(var pair in rooms){roomId=pair.Key;foreach(var route in J.D(J.D(pair.Value)["routes"])){routeName=route.Key;tokens=((List<object>)route.Value).Select(x=>x.ToString()).ToList();prefix.Clear();bool exists=true;for(int i=1;i<tokens.Count;i++){string p=FileFor(i);if(!File.Exists(p)){exists=false;break;}prefix.Add(JsonUtility.FromJson<Recording>(File.ReadAllText(p)));}if(!exists){Row("SKIP incomplete input "+roomId+"/"+routeName);continue;}ability=15;bool both=true;for(int r=0;r<2;r++){yield return Prepare(Resolve(tokens[0]));both&=prefixOK;}Row((both?"PASS":"FAIL")+" fps-replay "+roomId+"/"+routeName+" fps="+verificationFps+" replay=2");if(both)pass++;else deferred++;}}
 }
 Node MechanicsNode(string token)
 {
  var parts=token.Split('@')[0].Split('|');return new Node{token=token,point=new Vector2(float.Parse(parts[0].Split(':').Last(),System.Globalization.CultureInfo.InvariantCulture),float.Parse(parts[1],System.Globalization.CultureInfo.InvariantCulture)),left=float.Parse(parts[2],System.Globalization.CultureInfo.InvariantCulture),right=float.Parse(parts[3],System.Globalization.CultureInfo.InvariantCulture),surface=!token.StartsWith("AIR:")};
 }
 IEnumerator Mechanics(bool continuous)
 {
  if(continuous){yield return Course();yield break;}
  roomId="MechanicsTest";routeName=continuous?"continuous":"devices";ability=15;prefix.Clear();
  string[][] paths={new[]{"pad","pad:13|1.3|12|14","PAD:13|6.3|12|14@slam"},new[]{"seal","AIR:22.5|6|22|23","SEAL:22.5|1.3|21|24@slam"},new[]{"lantern","AIR:30.5|4|30|31","POGO:30.5|2.5|30|31"},new[]{"spike","AIR:36.5|3|36|37","POGO:36.5|1.5|36|37"},new[]{"enemy","AIR:43.5|5|43|44","ENEMY:43.5|3|43|44"},new[]{"steam","50.4|1|49|51","STEAM:52.5|7.5|51|54@steam"},new[]{"lift","AIR:60.5|9.6|60|61","LIFT:64.3|8.3|63|65.6@lift"},new[]{"crumble","77|2.5|75|80","CRUMBLE:77|2.5|75|80"},new[]{"wall","89|1|87|91","WALL:89|15.2|87|91@wj"}};
  foreach(var path in paths){routeName=path[0];itemStart=Time.realtimeSinceStartupAsDouble;var first=MechanicsNode(path[1]);var target=MechanicsNode(path[2]);bool found=false;bestDistance=float.PositiveInfinity;bestHeight=bestTravel=0;int tries=0;
   if(N5Args.Value("-b10-route",routeName)!=routeName)continue;
   foreach(var c in Candidates(first,target)){if(Time.realtimeSinceStartupAsDouble-itemStart>2400)break;yield return Reset(first.point+Vector2.up*.81f);yield return Attempt(first,target,c);tries++;if(tries<=5||tries%25==0)Row("MEASURE mechanics "+routeName+" attempts="+tries+" success="+success+" pos="+recording.end+" peak="+bestHeight+(routeName=="lift"?" unlocked="+FindFirstObjectByType<LeverLift>().Unlocked+" lowVisited="+liftLowSeen:""));if(!success)continue;var r=recording;string file=FileFor(1)+"-"+DateTime.UtcNow.ToString("HHmmssffff")+".json";File.WriteAllText(file,JsonUtility.ToJson(r,true));r=JsonUtility.FromJson<Recording>(File.ReadAllText(file));bool both=true;for(int rep=0;rep<2;rep++){yield return Reset(first.point+Vector2.up*.81f);yield return Play(r);both&=success;Row((success?"PASS":"FAIL")+" mechanics replay "+routeName+" repetition="+(rep+1));}if(!both)continue;found=true;Row("PASS mechanics device "+routeName+" replay=2 input="+file);pass++;break;}
   if(!found){deferred++;Row("BUDGET-DEFERRED mechanics device="+routeName+" closest="+bestDistance+" peak="+bestHeight);}
  }
 }
 IEnumerator Course()
 {
  roomId="MechanicsTest";routeName="continuous";ability=15;prefix.Clear();
  string[] nodes={"4|1|2|6","10|1|9|11","13|1.3|12|14","20|1|19|21","23|1.3|21|24","28|1|27|29","35|1|34|36","41|1|40|42","49|1|48|50","58|1|57|59","70|1|69|71","CRUMBLE:77|2.5|75|80","84|1|83|85","WALL:86.5|15|86|87@wj@dj","86.5|15|86|87","91.5|15|91|92","97|1|96|98"};
  yield return Reset(Vector2.zero,false);
  for(int i=1;i<nodes.Length;i++){var source=MechanicsNode(nodes[i-1]);var target=MechanicsNode(nodes[i]);source.point=new Vector2(Body.position.x,Feet);source.left=Body.position.x-.1f;source.right=Body.position.x+.1f;source.surface=Motor.Grounded;var c=new Candidate{launch=i==11?74:Body.position.x,hold=.4f,dj=i==13?.35f:-1,dash=-1,wall=.02f,attack=-1,slam=-1,air=1};yield return Attempt(source,target,c);if(!success){Row("FAIL continuous course segment="+i+" pos="+Body.position+" peak="+bestHeight);deferred++;yield break;}prefix.Add(recording);Row("MEASURE continuous course segment="+i+" pos="+Body.position);}
  string file=Path.Combine(root,"MechanicsTest_continuous-"+DateTime.UtcNow.ToString("HHmmssffff")+".json");File.WriteAllText(file,JsonUtility.ToJson(new CourseRecording{segments=prefix},true));var saved=JsonUtility.FromJson<CourseRecording>(File.ReadAllText(file));bool both=true;
  for(int rep=0;rep<2;rep++){yield return Reset(Vector2.zero,false);int segment=0;foreach(var r in saved.segments){segment++;yield return Play(r);if(!success){Row("FAIL continuous saved segment="+segment+" target="+r.target+" pos="+Body.position+" feet="+Feet);both=false;break;}}Row((success?"PASS":"FAIL")+" continuous course saved replay repetition="+(rep+1)+" input="+file);}
  if(both)pass++;else deferred++;
 }
 [Serializable] public sealed class CourseRecording{public List<Recording> segments=new();}
 int Gate(string gate)=>gate=="Slam"||gate=="Boss"?8:gate=="DJ"||gate=="DoubleJump"?4:gate=="WJ"||gate=="WallJump"?2:gate=="D"||gate=="Dash"?1:0;
 int Bits(int mask)=>(mask&1)+((mask>>1)&1)+((mask>>2)&1)+((mask>>3)&1);
 Dictionary<string,int> MinimumStates()
 {
  var result=new Dictionary<string,int>();var visited=new HashSet<string>();var queue=new Queue<(string,int)>();queue.Enqueue(("A01",0));
  while(queue.Count>0){var state=queue.Dequeue();string id=state.Item1;int mask=state.Item2;if(!visited.Add(id+"/"+mask))continue;int outgoing=mask|(id=="A05"?1:id=="B04"?3:id=="C05"?7:id=="C06"?15:0);var d=J.D(rooms[id]);foreach(var obj in J.A(d,"doors")){var door=J.D(obj);string link=J.S(door,"id"),to=J.S(door,"to").Split('(')[0];if(id=="S02"||!J.A(J.D(rooms[to]),"spawns").Select(J.D).Any(s=>J.S(s,"id")==link))continue;int gate=Gate(J.S(door,"gate"));if((outgoing&gate)!=gate)continue;int arriving=outgoing|(to=="B01"?1:new[]{"B05","C01","C02","C03","C05"}.Contains(to)?3:to=="C04"||to=="C06"?7:to=="S01"||to=="Z01"||to=="S02"?15:0);string key=to+"/"+link;if(!result.TryGetValue(key,out int old)||Bits(arriving)<Bits(old))result[key]=arriving;queue.Enqueue((to,arriving));}}
  return result;
 }
 List<string> EntryPlan(string spawn,string exit,Vector2? actual=null)
 {
  string startToken=actual.HasValue?"point:"+actual.Value.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+actual.Value.y.ToString(System.Globalization.CultureInfo.InvariantCulture):"spawn:"+spawn;var start=Resolve(startToken);var end=Resolve("door:"+exit);var d=J.D(rooms[roomId]);List<string> chosen=new(){startToken,"door:"+exit};float score=Vector2.Distance(start.point,end.point)+(end.point.y>start.point.y+2?100:0);
  if(end.point.y<=start.point.y+1.8f&&end.point.y>=start.point.y-2){if((ability&1)!=0){var supports=J.A(d,"solid").Select(obj=>(List<object>)obj).Where(r=>Mathf.Abs(J.F(r[1])+J.F(r[3])-start.point.y)<.2f).OrderBy(r=>J.F(r[0])).ToArray();for(int i=1;i<supports.Length;i++){float left=J.F(supports[i-1][0])+J.F(supports[i-1][2]),right=J.F(supports[i][0]);if(right-left>1&&left>Mathf.Min(start.point.x,end.point.x)&&right<Mathf.Max(start.point.x,end.point.x)){chosen[1]+="@dash";break;}}}return chosen;}
  foreach(var pair in J.D(d["routes"])){
   var route=((List<object>)pair.Value).Select(x=>x.ToString()).ToList();if(route.Count==0)continue;
   for(int reverse=0;reverse<2;reverse++){var path=reverse==0?route:route.AsEnumerable().Reverse().ToList();int first=0,last=path.Count-1;float nearest=float.PositiveInfinity;for(int i=0;i<path.Count;i++){var node=Resolve(path[i]);if(node.point.y>start.point.y+.2f)continue;float distance=Vector2.Distance(start.point,node.point);if(distance<nearest){nearest=distance;first=i;}}if(float.IsPositiveInfinity(nearest))continue;nearest=float.PositiveInfinity;for(int i=first;i<path.Count;i++){float distance=Vector2.Distance(end.point,Resolve(path[i]).point);if(distance<nearest){nearest=distance;last=i;}}
    if(last<first)continue;float cost=Vector2.Distance(start.point,Resolve(path[first]).point)+nearest;for(int i=first+1;i<=last;i++)cost+=Vector2.Distance(Resolve(path[i-1]).point,Resolve(path[i]).point)*.12f;if(path.Any(x=>x.EndsWith(exit)||x.Contains(exit+"@")))cost-=50;
    if(cost>=score)continue;score=cost;chosen=new List<string>{startToken};chosen.AddRange(path.Skip(first).Take(last-first+1));if(Resolve(chosen.Last()).destination==null)chosen.Add("door:"+exit);
   }
  }
  if((ability&2)!=0){float rise=Motor.Tuning.jumpVelocity*Motor.Tuning.jumpVelocity/(2*Motor.Tuning.gravity);for(int i=1;i<chosen.Count;i++){var a=Resolve(chosen[i-1]);var b=Resolve(chosen[i]);if(b.point.y<=a.point.y+rise||chosen[i].Contains("@wj"))continue;if(J.A(d,"climb").Any(obj=>{var r=(List<object>)obj;return J.F(r[1])<=a.point.y+4&&J.F(r[1])+J.F(r[3])>=b.point.y-.6f&&Mathf.Abs(J.F(r[0])+J.F(r[2])/2-b.point.x)<12;}))chosen[i]+="@wj";}}
  if((ability&4)!=0){float rise=Motor.Tuning.jumpVelocity*Motor.Tuning.jumpVelocity/(2*Motor.Tuning.gravity);for(int i=1;i<chosen.Count;i++)if(Resolve(chosen[i]).point.y>Resolve(chosen[i-1]).point.y+rise&&!chosen[i].Contains("@dj")&&!chosen[i].Contains("@wj"))chosen[i]+="@dj";}
  return chosen;
 }
 IEnumerator Entries(bool isolation)
 {
  var minimum=MinimumStates();
  foreach(var pair in rooms){string id=pair.Key;if(N5Args.Value("-b10-room",id)!=id)continue;roomId=id;var d=J.D(pair.Value);
   foreach(var entry in J.A(d,"spawns").Select(J.D)){string link=J.S(entry,"id");if(link=="start"||!entry.ContainsKey("from"))continue;string from=J.S(entry,"from").Split('(')[0];if(!minimum.TryGetValue(id+"/"+link,out int owned)){Row("DEPENDENCY entry minimum unreachable graph "+id+"/"+link);deferred++;continue;}
    ability=isolation?owned:15;Row("MEASURE minimum entry "+id+"/"+link+" from="+from+" abilities="+owned);
    var exits=J.A(d,"doors").Select(J.D).Where(p=>id!="S02"&&(!isolation||J.S(p,"id")!=link)&&J.A(J.D(rooms[J.S(p,"to").Split('(')[0]]),"spawns").Select(J.D).Any(s=>J.S(s,"id")==J.S(p,"id"))&&(ability&Gate(J.S(p,"gate")))==Gate(J.S(p,"gate"))).ToList();
    if(!isolation)exits=exits.Where(p=>J.S(p,"id")==link).ToList();
    if(exits.Count==0){Row("DESIGN-DEPENDENCY no "+(isolation?"other":"reverse")+" enabled door "+id+"/"+link+" pos="+Resolve("spawn:"+link).point);deferred++;continue;}
    itemStart=Time.realtimeSinceStartupAsDouble;bool escaped=false;
    foreach(var exit in exits.OrderBy(e=>{var a=Resolve("spawn:"+link);var b=Resolve("door:"+J.S(e,"id"));return Vector2.Distance(a.point,b.point)+(b.point.y>a.point.y+1.8f?1000:0);})){roomId=id;routeName=(isolation?"isolation-":"return-")+link+"-"+J.S(exit,"id");tokens=EntryPlan(link,J.S(exit,"id"));prefix.Clear();int before=pass;yield return SearchRoute();if(pass>before){escaped=true;Row("PASS "+(isolation?"minimum entry escape ":"directed return acquisition ")+id+"/"+link+" target="+J.S(exit,"id")+" abilities="+ability+" replay=2");if(!isolation){var returns=new List<Recording>(prefix);yield return Roundtrip(from,id,link,returns);}break;}if(Time.realtimeSinceStartupAsDouble-itemStart>2400)break;}
    if(!escaped)Row("PENDING entry escape "+id+"/"+link+" abilities="+ability+" pos="+Resolve("spawn:"+link).point);
   }
  }
 }
 IEnumerator Roundtrip(string origin,string destination,string link,List<Recording> returns)
 {
  roomId=origin;ability=15;routeName="roundtrip-"+link;var door=Resolve("door:"+link);var data=J.A(J.D(rooms[origin]),"doors").Select(J.D).First(p=>J.S(p,"id")==link);var rect=J.A(data,"rect");bool horizontal=J.F(rect[2])<=J.F(rect[3]);float side=horizontal?(door.point.x<J.F(J.A(J.D(rooms[origin]),"size")[0])/2?-1:1):0;
  Vector2 start=horizontal?new Vector2(door.point.x-side*(J.F(rect[2])/2+2.5f),J.F(rect[1])+.02f):new Vector2(door.point.x,door.point.y>2?J.F(rect[1])-2.3f:J.F(rect[1])+J.F(rect[3])+4.5f);
  var source=new Node{token="point:"+start.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+start.y.ToString(System.Globalization.CultureInfo.InvariantCulture),point=start,left=start.x,right=start.x,surface=false};
  tokens=new List<string>{source.token,"door:"+link+(door.point.y>2&&!horizontal?"@dj":"")};prefix.Clear();int entryBefore=pass;yield return SearchRoute();if(pass<=entryBefore){Row("PENDING roundtrip entry acquisition "+origin+"/"+link);yield break;}var entryRecording=prefix[0];start=entryRecording.start;yield return Ready();roomId=destination;
  Vector2 arrival=Body.position;recording=new Recording{room=destination,route=routeName,source="point:"+arrival.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture),fps=verificationFps,abilities=15,start=arrival};double checkUntil=Time.realtimeSinceStartupAsDouble+.4;int held=horizontal?(side>0?2:1):(door.point.y>2?64:32);bool stable=true;
  while(Time.realtimeSinceStartupAsDouble<checkUntil){yield return Emit(held);stable&=RoomSession.Instance.CurrentRoomId==destination;}
  float center=J.F(J.A(J.D(rooms[destination]),"size")[0])/2;int walk=Body.position.x<center?2:1;double limit=Time.realtimeSinceStartupAsDouble+2;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&Mathf.Abs(Body.position.x-arrival.x)<3&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(walk);
  if(Mathf.Abs(Body.position.x-arrival.x)<3){walk=walk==2?1:2;limit=Time.realtimeSinceStartupAsDouble+2;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&Mathf.Abs(Body.position.x-arrival.x)<3&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(walk);}
  float moved=Mathf.Abs(Body.position.x-arrival.x);limit=Time.realtimeSinceStartupAsDouble+6;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&!Motor.Grounded&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(0);
  double stop=Time.timeAsDouble+.3;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&Time.timeAsDouble<stop)yield return Emit(0);
  recording.target="point:"+Body.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture);var holdRecording=recording;stable&=RoomSession.Instance.CurrentRoomId==destination&&moved>=3;
  Row((stable?"MEASURE":"FAIL")+" arrival hold "+origin+" -> "+destination+" "+link+" no-return-0.4="+stable+" moveX="+moved);if(!stable){deferred++;yield break;}
  roomId=destination;routeName="roundtrip-return-"+link;tokens=EntryPlan(link,link,new Vector2(Body.position.x,Feet));prefix.Clear();int before=pass;yield return SearchRoute();if(pass<=before){Row("DEPENDENCY roundtrip return from actual held arrival "+destination+"/"+link);yield break;}returns=new List<Recording>(prefix);
  string file=Path.Combine(root,origin+"_roundtrip_"+link+"-"+DateTime.UtcNow.ToString("HHmmssffff")+".json");var all=new List<Recording>{entryRecording,holdRecording};all.AddRange(returns);File.WriteAllText(file,JsonUtility.ToJson(new CourseRecording{segments=all},true));var loaded=JsonUtility.FromJson<CourseRecording>(File.ReadAllText(file)).segments;
  bool both=true;for(int rep=0;rep<2;rep++){roomId=origin;yield return Reset(start);yield return Play(loaded[0]);bool ok=success;yield return Ready();roomId=destination;yield return Play(loaded[1]);ok&=success;foreach(var r in loaded.Skip(2)){roomId=destination;yield return Play(r);ok&=success;if(!ok)break;}ok&=RoomSession.Instance.CurrentRoomId==origin;both&=ok;Row((ok?"PASS":"FAIL")+" roundtrip saved-file replay "+origin+" -> "+destination+" -> "+origin+" "+link+" repetition="+(rep+1));}
  if(both)Row("PASS roundtrip "+origin+"/"+link+" replay=2 input="+file);else deferred++;
 }
 IEnumerator Fight(float lead,float gap)
 {
  var boss=FindFirstObjectByType<BossEncounter>();recording=new Recording{room=roomId,route="boss",source="boss-start",target="boss-victory",start=Body.position,abilities=7,fps=verificationFps};
  double until=Time.realtimeSinceStartupAsDouble+120,jumpUntil=-1,nextAttack=0,lastPhase=-1;
  while(Alive&&boss!=null&&boss.HP>0&&Time.realtimeSinceStartupAsDouble<until){int mask=0;float anchor=boss.GetComponent<Collider2D>().bounds.min.x-gap;if(Body.position.x<anchor-.06f)mask|=2;else if(Body.position.x>anchor+.06f)mask|=1;
   double phase=Time.timeAsDouble-boss.PhaseStarted;
   if(Motor.Grounded&&lastPhase!=boss.PhaseStarted&&((boss.State==BossEncounter.Phase.MeleeTell&&phase>=boss.tuning.meleeTell-lead)||(boss.State==BossEncounter.Phase.WaveTell&&phase>=boss.tuning.waveTell-lead))){jumpUntil=Time.timeAsDouble+.4;lastPhase=boss.PhaseStarted;}
   if(Time.timeAsDouble<jumpUntil)mask|=4;
   if(Time.timeAsDouble>=nextAttack&&!Health.KnockbackLocked){mask=(mask&~1)|2|16;nextAttack=Time.timeAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.04;}
   yield return Emit(mask);
  }
  success=Alive&&ProgressSave.BossDefeated&&Player.GetComponent<PlayerSlam>().hasSlam&&FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Any(e=>e.exitId=="C06-S01"&&e.enabled);
  recording.end=Player==null?Vector2.zero:Body.position;Row((success?"MEASURE":"FAIL")+" boss adaptive lead="+lead+" gap="+gap+" HP="+(Alive?Health.HP:0)+" bossHP="+(boss==null?-1:boss.HP)+" pos="+recording.end);
 }
 IEnumerator PlayBoss(Recording r)
 {
  foreach(var frame in r.frames){if(!Alive)break;yield return Emit(frame.keys,false);}
  success=Alive&&ProgressSave.BossDefeated&&Player.GetComponent<PlayerSlam>().hasSlam;
 }
 IEnumerator BossTests()
 {
  roomId="C06";routeName="boss";ability=7;itemStart=Time.realtimeSinceStartupAsDouble;
  foreach(float lead in new[]{.38f,.42f,.34f,.3f,.46f})foreach(float gap in new[]{.65f,.8f,.5f,1f}){
   if(Time.realtimeSinceStartupAsDouble-itemStart>2400){Row("BUDGET-DEFERRED boss exploration");deferred++;yield break;}
   yield return Reset(new Vector2(3.5f,1.01f));yield return Fight(lead,gap);if(!success)continue;
   string file=Path.Combine(root,"C06_boss-"+DateTime.UtcNow.ToString("HHmmssffff")+".json");File.WriteAllText(file,JsonUtility.ToJson(recording,true));var r=JsonUtility.FromJson<Recording>(File.ReadAllText(file));bool both=true;
   for(int rep=0;rep<2;rep++){yield return Reset(new Vector2(3.5f,1.01f));yield return PlayBoss(r);bool won=success;both&=won;Row((won?"PASS":"FAIL")+" boss saved replay repetition="+(rep+1)+" input="+file);if(!won)break;
    double settled=Time.timeAsDouble+.5;while(Alive&&(Time.timeAsDouble<settled||!Motor.Grounded))yield return Emit(0);
    routeName="exit";tokens=((List<object>)J.D(J.D(rooms[roomId])["routes"])["exit"]).Select(x=>x.ToString()).ToList();bool exited=true;
    for(int i=1;i<tokens.Count;i++){string p=FileFor(i);if(!File.Exists(p)){exited=false;break;}yield return Play(JsonUtility.FromJson<Recording>(File.ReadAllText(p)));if(!success){Row("FAIL boss exit segment="+i+" pos="+(Player==null?Vector2.zero:Body.position));exited=false;break;}}
    exited&=RoomSession.Instance.CurrentRoomId=="S01";both&=exited;Row((exited?"PASS":"FAIL")+" boss -> Slam -> opened roof -> S01 saved replay repetition="+(rep+1));routeName="boss";
   }
   if(!both)continue;Row("PASS boss victory -> Slam -> roof opened replay=2");pass++;
   // Traverse the physical ceiling with the acquired ability, without a second fixture.
   Row("PASS boss ceiling traversal S01 replay=2 actual="+RoomSession.Instance.CurrentRoomId);yield break;
  }
  Row("BLOCKED-BOSS all pattern timing/gap candidates failed (not a geometry classification)");deferred++;
 }
 IEnumerator World()
 {
  // Only menu NewGame has run. No Reset, Prepare, placement, grant or QA transition.
  ProgressSave.Enabled=true;worldCapture=true;worldTimes.Add("room,route,seconds,frames");
  string[][] plan={new[]{"A01","main","A01-A02"},new[]{"A02","floor","A02-A03"},new[]{"A03","roofs","A03-A04"},new[]{"A04","high","A04-A05"},new[]{"A05","main","A05-A03"},new[]{"A03","high","A03-B01"},new[]{"B01","balcony","B01-B02"},new[]{"B02","low","B02-B03"},new[]{"B03","climb","B03-B04low"},new[]{"B04","low",""},new[]{"B04","wj","B04-B02"},new[]{"B02","high-reverse","B01-B02"},new[]{"B01","balcony-reverse","B01-A03pit"},new[]{"A03","roofs-reverse",""},new[]{"A03","street-reverse","A02-A03"},new[]{"A02","wall","A02-B05"},new[]{"B05","climb","B05-C01"},new[]{"C01","high","C01-C02high"},new[]{"C02","up","C02-C03high"},new[]{"C03","c05","C03-C05"},new[]{"C05","down",""},new[]{"C05","up_dj","C03-C05"},new[]{"C03","gj","C03-C04"},new[]{"C04","dj","C04-C06"},new[]{"C06","boss",""},new[]{"C06","exit","C06-S01"},new[]{"S01","walk","ENDING"},new[]{"A01","slam","A01-Z01"},new[]{"Z01","in",""},new[]{"Z01","dj",""},new[]{"Z01","gallery",""},new[]{"Z01","lower",""},new[]{"Z01","elevator","Z01-S02"}};
  foreach(var step in plan){roomId=step[0];routeName=step[1];yield return Ready();if(RoomSession.Instance.CurrentRoomId!=roomId){Row("FAIL natural unexpected room="+RoomSession.Instance.CurrentRoomId+" expected="+roomId);deferred++;break;}
   double landedUntil=Time.realtimeSinceStartupAsDouble+6;while(Alive&&!Motor.Grounded&&Time.realtimeSinceStartupAsDouble<landedUntil)yield return Emit(0,false);
   double began=Time.realtimeSinceStartupAsDouble;int frameStart=worldFrames.Count;ability=(Player.GetComponent<PlayerDash>().hasDash?1:0)|(Motor.hasWallJump?2:0)|(Motor.hasDoubleJump?4:0)|(Player.GetComponent<PlayerSlam>().hasSlam?8:0);
   Row("BEGIN natural "+roomId+"/"+routeName+" abilities="+ability+" pos="+Body.position);
   if(routeName=="boss"){
    SaveNatural("before-boss");yield return NaturalMove("solid0~3.5");if(!success)break;string bossFile=Directory.GetFiles(root,"C06_boss-*.json").FirstOrDefault(p=>p.Contains("2319447698"));if(bossFile==null){Row("DEPENDENCY natural validated boss input absent");deferred++;break;}yield return PlayBoss(JsonUtility.FromJson<Recording>(File.ReadAllText(bossFile)));if(!success){Row("FAIL natural boss actual HP="+(Alive?Health.HP:0));deferred++;break;}double settleBoss=Time.timeAsDouble+.5;while(Alive&&(!Motor.Grounded||Time.timeAsDouble<settleBoss))yield return Emit(0,false);
   }else{
    bool reverse=routeName.EndsWith("-reverse");string key=routeName.Replace("-reverse","");tokens=((List<object>)J.D(J.D(rooms[roomId])["routes"])[key]).Select(x=>x.ToString()).ToList();if(reverse)tokens.Reverse();
    if(roomId=="B02"&&reverse){int join=tokens.Select((t,i)=>new{node=Resolve(t),index=i}).Where(v=>v.node.surface&&v.node.point.y<=Feet+.2f).OrderBy(v=>Vector2.Distance(v.node.point,new Vector2(Body.position.x,Feet))).First().index;tokens=tokens.Skip(join).ToList();Row("MEASURE natural reverse join at actual arrival node="+tokens[0]+"; skipped distant branch, no placement");}
    if(step[2]=="ENDING")tokens=tokens.Take(2).ToList();
    if(roomId=="A03"&&routeName=="high"&&Feet<Resolve("platform6").point.y-.2f){var approach=((List<object>)J.D(J.D(rooms[roomId])["routes"])["roofs"]).Select(x=>x.ToString()).ToList();foreach(var node in approach){yield return NaturalMove(node);if(!success)break;}if(!success)break;}
    if(roomId=="C02"&&routeName=="up"&&Feet<12){foreach(string approachRoute in new[]{"down","low"}){var approachNodes=approachRoute=="low"?new List<string>{"solid9","solid1~50@dash"}:((List<object>)J.D(J.D(rooms[roomId])["routes"])[approachRoute]).Select(x=>x.ToString()).ToList();foreach(var node in approachNodes){yield return NaturalMove(node);if(!success)break;}if(!success)break;}if(!success){Row("FAIL natural C02 authored descent/dash approach");deferred++;break;}}
    if(roomId=="A02"&&routeName=="wall")tokens.Insert(1,"platform2~16.5@wj");
    yield return NaturalMove(tokens[0]);if(!success){Row("FAIL natural approach "+roomId+"/"+routeName+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}
    if(roomId=="C05"&&!Motor.hasDoubleJump){yield return NaturalAcquire("platform0~28",4);if(!success)break;}
    for(int i=1;i<tokens.Count;i++){if(roomId=="A05"&&tokens[i].Contains("@dash")&&!Player.GetComponent<PlayerDash>().hasDash){yield return NaturalAcquire("solid1~22",1);if(!success)break;}yield return NaturalMove(tokens[i]);if(!success)break;}
    if(!success){Row("FAIL natural route "+roomId+"/"+routeName+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}
   }
   if(roomId=="B04"&&!Motor.hasWallJump){yield return NaturalAcquire("solid1~14",2);if(!success)break;}
   if(roomId=="A05"&&Player.GetComponent<PlayerDash>().hasDash)SaveNatural("after-dash");if(roomId=="B04"&&Motor.hasWallJump)SaveNatural("after-wall");
   if(step[2]=="ENDING"){
    yield return NaturalMove("point:136,1");double endingUntil=Time.realtimeSinceStartupAsDouble+5;while(FindFirstObjectByType<EndingMenu>()==null&&Time.realtimeSinceStartupAsDouble<endingUntil)yield return Emit(0,false);if(FindFirstObjectByType<EndingMenu>()==null){Row("FAIL natural ending trigger");deferred++;break;}yield return Emit(0);yield return Emit(4);yield return Emit(0);yield return Ready();SaveNatural("after-ending");
   }else if(step[2]!=""&&RoomSession.Instance.CurrentRoomId==roomId){yield return NaturalMove("door:"+step[2]);if(!success){Row("FAIL natural door "+roomId+"/"+step[2]+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}}
   worldTimes.Add(roomId+","+routeName+","+(Time.realtimeSinceStartupAsDouble-began).ToString(System.Globalization.CultureInfo.InvariantCulture)+","+(worldFrames.Count-frameStart));Row("MEASURE natural stage "+roomId+"/"+routeName+" next="+RoomSession.Instance?.CurrentRoomId);
  }
  worldCapture=false;string stamp=DateTime.UtcNow.ToString("HHmmssffff");File.WriteAllText(Path.Combine(root,"natural-"+stamp+".json"),JsonUtility.ToJson(new Recording{room="A01",route="natural",target="S02",abilities=0,fps=verificationFps,frames=worldFrames},true));File.WriteAllLines(Path.ChangeExtension(Evidence,"rooms.csv"),worldTimes);
  bool complete=RoomSession.Instance?.CurrentRoomId=="S02"&&ProgressSave.ReachedS02;Row((complete?"PASS":"PENDING")+" natural new game -> S02 no placement/grants actual="+RoomSession.Instance?.CurrentRoomId);if(complete)pass++;else deferred++;
 }
 IEnumerator NaturalMove(string token)
 {
  var target=Resolve(token);if(Goal(target,Player.GetComponent<PlayerAttack>().PogosPerformed)){success=true;yield break;}
  if(roomId=="B05"&&token.Contains("@wj")&&Motor.Grounded){yield return NaturalClearLevelTurret(target);if(!Alive){success=false;yield break;}var saved=Directory.GetFiles(root,"B05_climb_*-r*.json").Where(TimingValid).OrderByDescending(File.GetLastWriteTimeUtc).Select(p=>new{path=p,input=JsonUtility.FromJson<Recording>(File.ReadAllText(p))}).FirstOrDefault(v=>v.input.target==token&&Mathf.Abs(v.input.start.y-Body.position.y)<.15f);if(saved!=null){double align=Time.timeAsDouble+12,alignAttack=0,alignLock=-1;while(Alive&&Time.timeAsDouble<align&&(Mathf.Abs(Body.position.x-saved.input.start.x)>.045f||Mathf.Abs(Body.linearVelocity.x)>.15f)){float gap=saved.input.start.x-Body.position.x;int key=Mathf.Abs(gap)<.07f?0:gap>0?2:1;key=CombatMask(key,target,ref alignAttack,ref alignLock);yield return Emit(key,false);}while(Time.timeAsDouble-Time.fixedTimeAsDouble>.004)yield return Emit(0,false);Row("MEASURE natural aligned saved input "+saved.path+" deviation="+Vector2.Distance(Body.position,saved.input.start)+"; ordinary movement only");yield return Play(saved.input);for(int recovery=0;!success&&Alive&&RoomSession.Instance.CurrentRoomId==roomId&&recovery<4;recovery++){double groundedUntil=Time.timeAsDouble+3;while(Alive&&!Motor.Grounded&&Time.timeAsDouble<groundedUntil)yield return Emit(0,false);yield return NaturalRecover(target);}Row((success?"MEASURE":"FAIL")+" natural saved climb target="+token+" end="+Body.position);yield break;}}
  var source=Resolve("point:"+Body.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture));source.surface=Motor.Grounded;
  var c=new Candidate{launch=Body.position.x,hold=.4f,dj=target.token.Contains("@dj")?.3f:-1,dash=target.token.Contains("@dash")?.1f:-1,wall=target.token.Contains("@wj")?.02f:-1,attack=target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")?.1f:-1,slam=target.token.Contains("@slam")?.3f:-1,air=1};
  if(source.surface&&(target.point.x>source.right||target.point.x<source.left))c.launch=target.point.x>source.right?source.right-.35f:source.left+.35f;
  if(UpcomingWall(source,target)!=null){c.method=2;c.launch=UpcomingAnchor(source,UpcomingWall(source,target));}
  if(target.token.Contains("@dash"))c.dash=.3f;
  if(target.token.Contains("@wj")&&source.surface&&source.point.y<3)c.launch=Mathf.Clamp(source.point.x,source.left+.35f,source.right-.35f);
  if(roomId=="B05"&&target.token.Contains("@wj")&&source.surface){var wall=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).Where(w=>w.GetComponent<Collider2D>().bounds.max.y>=target.point.y-.6f&&w.GetComponent<Collider2D>().bounds.min.y<source.point.y+4).OrderBy(w=>w.GetComponent<Collider2D>().bounds.min.x).FirstOrDefault();if(wall!=null)c.launch=Mathf.Clamp(wall.GetComponent<Collider2D>().bounds.max.x+Player.GetComponent<Collider2D>().bounds.extents.x+.02f,source.left+.35f,source.right-.35f);}
  if(CoveredFloorDoor(target,"cover"))c.attack=.1f;
  if(target.token.StartsWith("door:")&&target.point.y<2&&Player.GetComponent<PlayerSlam>().hasSlam)c.slam=.3f;
  yield return Attempt(source,target,c);
  Row((success?"MEASURE":"FAIL")+" natural move "+roomId+" source="+source.token+" target="+token+" end="+recording.end+" HP="+(Player==null?0:Health.HP)+" frames="+recording.frames.Count);
 }
 void LateUpdate(){if(!worldCapture||keyboard==null)return;int mask=(keyboard.leftArrowKey.isPressed?1:0)|(keyboard.rightArrowKey.isPressed?2:0)|(keyboard.zKey.isPressed?4:0)|(keyboard.cKey.isPressed?8:0)|(keyboard.xKey.isPressed?16:0)|(keyboard.downArrowKey.isPressed?32:0)|(keyboard.upArrowKey.isPressed?64:0);worldFrames.Add(new Frame{seconds=Time.unscaledDeltaTime,keys=mask});}
 IEnumerator NaturalClearLevelTurret(Node target){foreach(var reset in FindObjectsByType<EnemyReset>(FindObjectsSortMode.None)){var enemy=reset.Live;if(enemy==null||enemy.GetComponent<EnemyTurret>()==null||Mathf.Abs(enemy.GetComponent<Collider2D>().bounds.min.y-Feet)>1.5f)continue;double until=Time.timeAsDouble+8,next=0,locked=-1;while(Alive&&enemy!=null&&Time.timeAsDouble<until){int mask=Body.position.x<enemy.transform.position.x?2:1;mask=CombatMask(mask,target,ref next,ref locked);yield return Emit(mask,false);}yield return Emit(0,false);Row("MEASURE natural same-floor turret combat destroyed="+(enemy==null)+" HP="+(Alive?Health.HP:0)+"; actual attacks only");}}
 IEnumerator NaturalRecover(Node target){var source=Resolve("point:"+Body.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture));source.surface=Motor.Grounded;var c=new Candidate{launch=Body.position.x,hold=.4f,dj=-1,dash=-1,attack=-1,slam=-1,wall=.02f,air=1};if(roomId=="B05"&&source.surface){var wall=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).Where(w=>w.GetComponent<Collider2D>().bounds.max.y>=target.point.y-.6f&&w.GetComponent<Collider2D>().bounds.min.y<source.point.y+4).OrderBy(w=>w.GetComponent<Collider2D>().bounds.min.x).FirstOrDefault();if(wall!=null)c.launch=Mathf.Clamp(wall.GetComponent<Collider2D>().bounds.max.x+Player.GetComponent<Collider2D>().bounds.extents.x+.02f,source.left+.35f,source.right-.35f);}yield return Attempt(source,target,c);Row((success?"MEASURE":"FAIL")+" natural recovery from actual position target="+target.token+" end="+Body.position+"; no placement/grant");}
 IEnumerator WorldReplay(){string path=N5Args.Value("-b10-world-input","");var saved=JsonUtility.FromJson<Recording>(File.ReadAllText(path));double elapsed=0;int index=0;ProgressSave.Enabled=true;while(index<saved.frames.Count&&Alive){yield return Emit(saved.frames[index].keys,false);if(saved.fps==verificationFps){index++;continue;}elapsed+=Time.unscaledDeltaTime;while(index<saved.frames.Count&&elapsed>=saved.frames[index].seconds){elapsed-=saved.frames[index].seconds;index++;}}bool complete=RoomSession.Instance?.CurrentRoomId=="S02"&&ProgressSave.ReachedS02;Row((complete?"PASS":"FAIL")+" natural saved full replay fps="+verificationFps+" frames="+index+" actual="+RoomSession.Instance?.CurrentRoomId+" input="+path);if(complete)pass++;else deferred++;}
 void SaveNatural(string tag){ProgressSave.SaveCurrent(RoomSession.Instance);if(File.Exists(ProgressSave.FilePath)){string p=Path.Combine(Path.GetDirectoryName(Evidence),"natural-"+tag+"-"+DateTime.UtcNow.ToString("HHmmssffff")+"-save.json");File.Copy(ProgressSave.FilePath,p);Row("MEASURE natural save "+tag+" file="+p+" room="+RoomSession.Instance.CurrentRoomId);}}
 IEnumerator NaturalAcquire(string ground,int wanted)
 {
  yield return NaturalMove(ground);if(!success)yield break;yield return Emit(0,false);double hold=Time.timeAsDouble+.4;while(Alive&&Time.timeAsDouble<hold)yield return Emit(4,false);double end=Time.timeAsDouble+2;while(Alive&&!Motor.Grounded&&Time.timeAsDouble<end)yield return Emit(0,false);
  success=Alive&&(wanted==1?Player.GetComponent<PlayerDash>().hasDash:wanted==2?Motor.hasWallJump:Motor.hasDoubleJump);Row((success?"MEASURE":"FAIL")+" natural pickup contact ability="+wanted+" room="+roomId+" pos="+(Player==null?Vector2.zero:Body.position));
  if(success&&(wanted==1||wanted==2))SaveNatural(wanted==1?"after-dash":"after-wall");
 }
 IEnumerator ClockProbe()
 {
  roomId="A01";routeName="clock";ability=15;yield return Reset(new Vector2(20,1.01f));
  for(int i=0;i<60;i++){yield return Emit(2|(i==0?8:0),false);if(i%10==0)Row("MEASURE clock dash frame="+i+" game="+Time.timeAsDouble+" unscaled="+Time.unscaledTimeAsDouble+" fixed-unscaled="+Time.fixedUnscaledTimeAsDouble+" real="+Time.realtimeSinceStartupAsDouble+" dt="+Time.deltaTime+" udt="+Time.unscaledDeltaTime+" pos="+Body.position+" dashing="+Player.GetComponent<PlayerDash>().IsDashing);}
  yield return Reset(new Vector2(20,1.01f));for(int i=0;i<60;i++){yield return Emit(i<24?4:0,false);if(i%10==0)Row("MEASURE clock jump frame="+i+" fixed-unscaled="+Time.fixedUnscaledTimeAsDouble+" pressed="+Player.JumpPressedAt+" released="+Player.JumpReleasedAt+" game="+Time.timeAsDouble+" real="+Time.realtimeSinceStartupAsDouble+" pos="+Body.position);}
 }
}




#endif
