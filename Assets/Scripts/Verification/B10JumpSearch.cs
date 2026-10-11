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
 string live;double itemStart;int attempted;bool liftLowSeen;
 bool worldCapture;readonly List<Frame> worldFrames=new();readonly List<string> worldTimes=new();
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10JumpSearch>("-verify-b10-search");
 void Row(string row){lines.Add(row);File.AppendAllText(live,row+Environment.NewLine);}
 protected override IEnumerator Tests()
 {
  root=N5Args.Value("-b10-input-root","Validation/_raw/B10/inputs");Directory.CreateDirectory(root);
  live=Path.ChangeExtension(Evidence,"live.txt");if(File.Exists(live))throw new IOException("Preserve "+live);
  rooms=J.D(J.Read(Resources.Load<TextAsset>("V4Design").text)["rooms"]);
  string mode=N5Args.Value("-verify-b10-search","routes");
  // Opt-in capture cadence keeps render-input replay independent of OS scheduling.
  Time.captureDeltaTime=1f/Application.targetFrameRate;
  yield return NewGame();ProgressSave.Enabled=false;
  if(mode=="replay")yield return ReplayAll();
  else if(mode=="mechanics")yield return Mechanics(false);
  else if(mode=="course")yield return Mechanics(true);
  else if(mode=="world")yield return World();
  else if(mode=="boss")yield return BossTests();
  else if(mode=="connections"||mode=="isolation")yield return Entries(mode=="isolation");
  else foreach(var pair in rooms)
  {
   if(N5Args.Value("-b10-room",pair.Key)!=pair.Key)continue;
   roomId=pair.Key;var routes=J.D(J.D(pair.Value)["routes"]);
   foreach(var route in routes)
   {
    if(N5Args.Value("-b10-route",route.Key)!=route.Key)continue;
    routeName=route.Key;tokens=((List<object>)route.Value).Select(n=>n.ToString()).ToList();ability=15;prefix.Clear();
    itemStart=Time.realtimeSinceStartupAsDouble;attempted=0;
    Row("BEGIN route "+roomId+"/"+routeName+" nodes="+tokens.Count);
    yield return SearchRoute();
   }
  }
  Row("MEASURE B10 mode="+mode+" PASS="+pass+" BLOCKED-GEOMETRY="+blocked+" budget-deferred="+deferred);
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
  var m=Regex.Match(bare,@"^([a-z]+)(\d+)$");if(!m.Success)throw new Exception("Unknown node "+token);
  var rect=(List<object>)J.A(d,m.Groups[1].Value)[int.Parse(m.Groups[2].Value)];node.left=J.F(rect[0]);node.right=node.left+J.F(rect[2]);node.point=new Vector2(split.Length>1?float.Parse(split[1],System.Globalization.CultureInfo.InvariantCulture):(node.left+node.right)/2,J.F(rect[1])+J.F(rect[3]));node.surface=true;return node;
 }
 float Feet=>Player.GetComponent<BoxCollider2D>().bounds.min.y;
 bool Alive=>RoomSession.Instance!=null&&Player!=null&&!RoomSession.Instance.Respawning&&Health.HP>0;
 bool Goal(Node node,int initialPogos)
 {
  if(!Alive)return false;
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
  if(!node.surface)return Vector2.Distance(new Vector2(Body.position.x,Feet),node.point)<.55f;
  // Require actual floor contact on the named surface; proximity in the air is not success.
  return Motor.Grounded&&Body.position.x>=node.left+.08f&&Body.position.x<=node.right-.08f&&Mathf.Abs(Feet-node.point.y)<.12f&&(!(node.token.Contains('~')||node.token.StartsWith("point:"))||Mathf.Abs(Body.position.x-node.point.x)<.5f);
 }
 IEnumerator Reset(Vector2 start,bool place=true)
 {
  yield return Keys();if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);Destroy(GameState.GetOrCreate().gameObject);yield return null;
  ProgressSave.Enabled=false;ProgressSave.Pending=null;ProgressSave.ResetBrokenWalls();ProgressSave.ResetVisitedRooms();ProgressSave.HasSlam=(ability&8)!=0;ProgressSave.BossDefeated=(ability&8)!=0;ProgressSave.EndingReached=false;ProgressSave.ReachedS02=false;
  SceneManager.LoadScene(roomId=="MechanicsTest"?"Assets/Scenes/Test/MechanicsTest.unity":V4SceneMap.Path(roomId));yield return Ready();
  Player.GetComponent<PlayerDash>().hasDash=(ability&1)!=0;Motor.hasWallJump=(ability&2)!=0;Motor.hasDoubleJump=(ability&4)!=0;Player.GetComponent<PlayerSlam>().hasSlam=(ability&8)!=0;
  // Only the path's initial setup is placed; every later waypoint uses recorded input.
  if(roomId=="MechanicsTest"&&routeName=="steam"){var vent=FindFirstObjectByType<SteamVent>();yield return Until(()=>!vent.On,"steam preparation off",5);yield return Until(()=>vent.On,"steam preparation fresh on",5);}
  if(roomId=="MechanicsTest"&&routeName=="continuous"){var vent=FindFirstObjectByType<SteamVent>();yield return Until(()=>!vent.On,"course initial phase off",5);yield return Until(()=>vent.On,"course initial phase on",5);}
  // Align setup to the same existing physics/render phase; input is still Dynamic Update.
  while(Time.timeAsDouble-Time.fixedTimeAsDouble>.004)yield return null;
  if(place)PlaceForAuthorizedSetup(start);liftLowSeen=false;Player.ClearTransientInput();yield return Keys();yield return new WaitForSeconds(roomId=="MechanicsTest"&&routeName=="lift"?.02f:.12f);
 }
 IEnumerator Emit(int mask,bool record=true)
 {
  var keys=new List<Key>();if((mask&1)!=0)keys.Add(Key.LeftArrow);if((mask&2)!=0)keys.Add(Key.RightArrow);if((mask&4)!=0)keys.Add(Key.Z);if((mask&8)!=0)keys.Add(Key.C);if((mask&16)!=0)keys.Add(Key.X);if((mask&32)!=0)keys.Add(Key.DownArrow);if((mask&64)!=0)keys.Add(Key.UpArrow);
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));double before=Time.realtimeSinceStartupAsDouble;yield return null;
  if(worldCapture)worldFrames.Add(new Frame{seconds=Time.unscaledDeltaTime,keys=mask});
  if(roomId=="MechanicsTest"){var lift=FindFirstObjectByType<LeverLift>();if(lift!=null&&lift.Unlocked&&Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.low)<.1f)liftLowSeen=true;}
  if(record)recording.frames.Add(new Frame{seconds=Time.unscaledDeltaTime,keys=mask});
 }
 IEnumerator Play(Recording r,bool check=true)
 {
  int pogo=Alive?Player.GetComponent<PlayerAttack>().PogosPerformed:0;double elapsed=0;int index=0;
  while(index<r.frames.Count&&Alive){double begin=Time.realtimeSinceStartupAsDouble;int mask=r.frames[index].keys;yield return Emit(mask,false);if(r.fps==Application.targetFrameRate){index++;continue;}elapsed+=Time.unscaledDeltaTime;while(index<r.frames.Count&&elapsed>=r.frames[index].seconds){elapsed-=r.frames[index].seconds;index++;}}
  if(check){var node=roomId=="MechanicsTest"?MechanicsNode(r.target):Resolve(r.target);success=Goal(node,pogo)||r.target=="END"&&FindFirstObjectByType<EndingMenu>()!=null;}
 }
 IEnumerator Prepare(Node first)
 {
  yield return Reset(first.point+Vector2.up*(roomId=="MechanicsTest"?.81f:.01f));prefixOK=true;
  foreach(var r in prefix){yield return Play(r);if(!success){prefixOK=false;break;}}
 }
 IEnumerable<Candidate> Candidates(Node source,Node target)
 {
  float direction=target.point.x>=source.point.x?1:-1,near=Mathf.Clamp(source.point.x,source.left+.35f,Mathf.Max(source.left+.35f,source.right-.35f));
  if(target.point.y<source.point.y-.15f)yield return new Candidate{launch=source.point.x,run=0,hold=0,dj=-1,dash=-1,wall=-1,attack=-1,slam=-1,air=1,method=1};
  // First cover the useful center/edge launches, then enumerate the full grid below.
  foreach(float launch in new[]{near,Mathf.Max(source.left+.35f,source.right-.35f),source.left+.35f}.Distinct())
  foreach(float dj in target.token.Contains("@dj")?new[]{.3f,.25f,.35f,.2f,.4f}:new[]{-1f})
   yield return new Candidate{launch=launch,run=0,hold=.4f,dj=dj,dash=target.token.Contains("@dash")?.1f:-1,wall=target.token.Contains("@wj")?.02f:-1,attack=target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")||target.token.StartsWith("POGO:")||target.token.StartsWith("ENEMY:")?.1f:-1,slam=target.token.Contains("@slam")||source.token.StartsWith("seal")?.3f:-1,air=1,method=0};
  // Try useful full holds first, then the complete documented grid. No geometry claims on truncation.
  float[] holds=N5Args.Has("-b10-expanded")?Steps(.1f,.4f,.02f).ToArray():new[]{.4f,.3f,.2f,.1f,.15f,.25f,.35f};
  IEnumerable<float> positions=Positions(source,near,N5Args.Has("-b10-expanded")?.1f:.25f);
  foreach(float launch in positions)foreach(float run in new[]{0f,1f,2f})foreach(float hold in holds)foreach(int air in new[]{1,0,2})
  foreach(float dj in target.token.Contains("@dj")?Steps(.15f,.55f,.05f):new[]{-1f})
  foreach(float dash in target.token.Contains("@dash")?Steps(0,.3f,.05f):new[]{-1f})
  foreach(float wall in target.token.Contains("@wj")?Steps(0,.1f,.02f):new[]{-1f})
  foreach(float attack in target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")||target.token.StartsWith("POGO:")||target.token.StartsWith("ENEMY:")?Steps(0,.6f,.05f):new[]{-1f})
  foreach(float slam in target.token.Contains("@slam")||source.token.StartsWith("seal")?new[]{.15f,.3f,.45f,.6f}:new[]{-1f})
  foreach(float wait in target.token.Contains("@steam")?new[]{0f,1f,2f,3f}:new[]{0f})
   yield return new Candidate{launch=launch,run=run,hold=hold,dj=dj,dash=dash,wall=wall,attack=attack,slam=slam,wait=wait,air=air};
 }
 IEnumerable<float> Positions(Node n,float first,float step){yield return first;if(!n.surface)yield break;float edge=n.right-n.left<.7f?first:n.right-.35f;yield return edge;yield return n.left+.35f;for(float x=n.left+.35f;x<=edge+.001f;x+=step)if(Mathf.Abs(x-first)>.001f&&Mathf.Abs(x-edge)>.001f)yield return x;}
 IEnumerable<float> Steps(float lo,float hi,float step){for(int i=0;i<=Mathf.RoundToInt((hi-lo)/step);i++)yield return lo+i*step;}
 void Measure(Node source,Node target)
 {
  if(!Alive)return;float dist=Vector2.Distance(new Vector2(Body.position.x,Feet),target.point);bestHeight=Mathf.Max(bestHeight,Feet);bestTravel=Mathf.Max(bestTravel,Mathf.Abs(Body.position.x-source.point.x));
  if(dist<bestDistance){bestDistance=dist;bestPosition=new Vector2(Body.position.x,Feet);int n=Player.GetComponent<Collider2D>().GetContacts(contactBuffer);contacts=string.Join(",",contactBuffer.Take(n).Select(c=>c.collider==Player.GetComponent<Collider2D>()?c.otherCollider.name:c.collider.name).Distinct());}
 }
 IEnumerator Attempt(Node source,Node target,Candidate c)
 {
  recording=new Recording{room=roomId,route=routeName,source=source.token,target=target.token,start=Body.position,abilities=ability,fps=Application.targetFrameRate};success=false;dead=false;int pogo=Player.GetComponent<PlayerAttack>().PogosPerformed;
  int direction=target.point.x>=source.point.x?2:1;double started=Time.timeAsDouble;
  // Walk to run-up position using ordinary movement; do not teleport to the takeoff grid.
  float runX=Mathf.Clamp(c.launch+(direction==2?-c.run:c.run),source.left+.35f,Mathf.Max(source.left+.35f,source.right-.35f));
  if(source.surface)while(Alive&&RoomSession.Instance.CurrentRoomId==roomId&&Mathf.Abs(Body.position.x-runX)>.13f&&Time.timeAsDouble-started<6){yield return Emit(Body.position.x<runX?2:1);Measure(source,target);}
  yield return Emit(0);double wait=Time.timeAsDouble+c.wait;while(Alive&&Time.timeAsDouble<wait)yield return Emit(0);
  bool launched=false,slamSent=false,dashSent=false,airReleased=false;double reverseAt=-1;double launchTime=0,nextEnemyAttack=0,enemyLockUntil=-1;int wallCount=Motor.WallJumpsPerformed;double wallAt=-1,wallHoldUntil=-1;int wallPulse=0,wallSide=0;int attackCount=0;int bounces=FindObjectsByType<SlamPad>(FindObjectsSortMode.None).Sum(p=>p.Bounces);double until=Time.timeAsDouble+(target.token.StartsWith("LIFT:")?22:target.token.Contains("@wj")?30:Mathf.Max(7,Mathf.Abs(target.point.x-source.point.x)/Motor.Tuning.moveSpeed+6));
  while(Alive&&RoomSession.Instance.CurrentRoomId==roomId&&Time.timeAsDouble<until)
  {
   if(Goal(target,pogo)){success=true;break;}
   int mask=direction;float dx=target.point.x-Body.position.x;
   if(!launched&&(!source.surface||(direction==2?Body.position.x>=c.launch-.12f:Body.position.x<=c.launch+.12f))){launched=true;launchTime=Time.timeAsDouble;}
   float t=launched?(float)(Time.timeAsDouble-launchTime):-1;
   if(launched&&t>1.5f&&Motor.Grounded&&target.surface&&target.point.y>Feet+.15f&&!target.token.Contains("@wj")&&!target.token.Contains("@steam")&&!target.token.Contains("@lift")&&!target.token.StartsWith("LIFT:")&&!target.token.StartsWith("CRUMBLE:"))break;
   if(launched&&Motor.Grounded&&c.hold>0&&t>c.hold+.2f){var bounds=Player.GetComponent<Collider2D>().bounds;var obstacle=Physics2D.BoxCast(bounds.center,new Vector2(bounds.size.x*.8f,bounds.size.y*.85f),0,direction==2?Vector2.right:Vector2.left,.8f,1<<6);if(obstacle.collider!=null&&obstacle.collider.bounds.max.y>Feet+.1f&&obstacle.collider.bounds.min.y<bounds.max.y-.1f){launchTime=Time.timeAsDouble;t=0;dashSent=false;airReleased=false;}}
   if(launched){float firstHold=c.dj>=0?Mathf.Min(c.hold,Mathf.Max(0,c.dj-.04f)):c.hold;if(t<firstHold)mask|=4;if(c.dj>=0&&t>=c.dj&&t<c.dj+c.hold)mask|=4;if(!dashSent&&c.dash>=0&&t>=c.dash){mask|=8;dashSent=true;}
    if(c.air==1&&(airReleased||Mathf.Abs(dx)<.25f||dx*(direction==2?1:-1)<0)){mask&=~3;airReleased=true;}else if(c.air==2&&dx*(direction==2?1:-1)<.2f){if(reverseAt<0)reverseAt=Time.timeAsDouble;mask=(mask&~3)|(Time.timeAsDouble-reverseAt<.1?(direction==2?1:2):0);}
    if(c.attack>=0&&t>=c.attack&&t>=c.attack+attackCount*.32f){mask|=32|16;attackCount++;}
    bool padSetup=source.token.StartsWith("pad")&&target.token.Contains("@slam")&&FindObjectsByType<SlamPad>(FindObjectsSortMode.None).Sum(p=>p.Bounces)==bounces;
    if(padSetup)mask&=~3;
    if(!slamSent&&c.slam>=0&&t>=c.slam&&!Motor.Grounded&&(padSetup||source.token.StartsWith("seal")||Mathf.Abs(dx)<.6f)){mask=(mask&~3)|32|8;slamSent=true;}
   }
   if(target.token.Contains("@wj"))
   {
    var walls=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None);var wall=walls.OrderBy(w=>Mathf.Abs(w.transform.position.x-Body.position.x)).FirstOrDefault();
    if(Motor.WallJumpsPerformed>wallCount){wallCount=Motor.WallJumpsPerformed;wallSide=wallSide==2?1:2;wallHoldUntil=Time.timeAsDouble+c.hold;wallPulse=0;wallAt=-1;if(target.token.Contains("@dj"))launchTime=Time.timeAsDouble;}
    if(wall!=null&&!Motor.Grounded&&target.point.y>Feet+.2f){bool outside=Body.position.x<walls.Min(w=>w.GetComponent<Collider2D>().bounds.min.x)||Body.position.x>walls.Max(w=>w.GetComponent<Collider2D>().bounds.max.x);if(wallSide==0||outside)wallSide=wall.transform.position.x>Body.position.x?2:1;mask=(mask&~3)|wallSide;}
    if(Time.timeAsDouble<wallHoldUntil&&!target.token.Contains("@dj"))mask|=4;
    if(Motor.WallSliding){if(wallAt<0)wallAt=Time.timeAsDouble;if(Time.timeAsDouble-wallAt>=c.wall){if(wallPulse==0){mask&=~4;wallPulse=1;}else{mask|=4;wallPulse=2;}}}
    else wallAt=-1;
   }
   if(target.token.Contains("@steam")){var vent=FindObjectsByType<SteamVent>(FindObjectsSortMode.None).OrderBy(v=>Vector2.Distance(v.transform.position,source.point)).FirstOrDefault();if(vent!=null&&Feet<target.point.y-.2f){float center=vent.GetComponent<Collider2D>().bounds.center.x;mask=(mask&~3)|(Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1);}}
   if(target.token.Contains("@lift")||target.token.StartsWith("lift:")){var lift=FindFirstObjectByType<LeverLift>();if(lift!=null){var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).Where(v=>lift.Unlocked||v.upper).OrderBy(v=>Vector2.Distance(v.transform.position,Body.position)).FirstOrDefault();if(!lift.Unlocked&&lever!=null){mask=(mask&~3)|(lever.transform.position.x>Body.position.x?2:1);if((int)(Time.time*10)%3==0)mask|=16;}else if(Mathf.Abs(Feet-lift.GetComponent<Collider2D>().bounds.max.y)<.2f)mask&=~3;}}
   if(target.token.StartsWith("LIFT:")){
    var lift=FindFirstObjectByType<LeverLift>();float center=lift.GetComponent<Collider2D>().bounds.center.x;
    if(!lift.Unlocked){var upper=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>v.upper);mask=(upper.transform.position.x>Body.position.x?2:1)|((int)(Mathf.Max(0,t)*10)%3==0?16:0);}
    else if(!liftLowSeen){mask=Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1;}
    else if(!ProgressSave.IsWallBroken(lift.liftId+"-down")){mask=Mathf.Abs(Body.position.x-center)<.15f?0:Body.position.x<center?2:1;if(Mathf.Abs(Feet-lift.GetComponent<Collider2D>().bounds.max.y)>.15f)mask|=4;}
    else{var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None).First(v=>!v.upper);float anchor=lever.transform.position.x+Player.GetComponent<PlayerAttack>().tuning.attackRange+.3f;mask=Mathf.Abs(Body.position.x-anchor)<.1f?1:Body.position.x<anchor?2:1;if(Mathf.Abs(Body.position.x-anchor)<.3f&&(int)(Time.time*10)%3==0)mask|=16;}
   }
   if(target.token.StartsWith("CRUMBLE:")&&Motor.Grounded&&Feet>=target.point.y-.15f)mask=0;
   if(target.token.StartsWith("cover")||target.token.StartsWith("breakable")){if((int)(Time.time*10)%3==0)mask|=16;}
   var enemy=FindObjectsByType<EnemyReset>(FindObjectsSortMode.None).FirstOrDefault(e=>e.Live!=null&&Mathf.Abs(e.Live.transform.position.x-Body.position.x)<2&&Mathf.Abs(e.Live.transform.position.y-Body.position.y)<2);
   if(enemy!=null&&!target.token.StartsWith("enemy:")&&!target.token.StartsWith("pogo:")){
    float enemyDx=enemy.Live.transform.position.x-Body.position.x;if(Mathf.Abs(enemyDx)<1.9f){mask&=~3;if(Time.timeAsDouble>=enemyLockUntil&&Mathf.Abs(enemyDx)<1.55f)mask|=enemyDx>0?1:2;if(Time.timeAsDouble>=nextEnemyAttack){mask=(mask&~3)|(enemyDx>0?2:1)|16;var tuning=Player.GetComponent<PlayerAttack>().tuning;nextEnemyAttack=Time.timeAsDouble+tuning.attackCooldown+.04;enemyLockUntil=Time.timeAsDouble+tuning.attackStartup+tuning.attackActive;}}
   }
   Measure(source,target);yield return Emit(mask);
  }
  if(Alive&&RoomSession.Instance.CurrentRoomId!=roomId)success=target.destination==RoomSession.Instance.CurrentRoomId;
  if(Alive&&Goal(target,pogo))success=true;
  if(success&&Alive&&RoomSession.Instance.CurrentRoomId==roomId){double settle=Time.timeAsDouble+.12;while(Alive&&Time.timeAsDouble<settle)yield return Emit(0);success=Goal(target,pogo);}
  dead=!Alive;recording.end=Player==null?Vector2.zero:Body.position;
 }
 string FileFor(int index){string stem=roomId+"_"+routeName+"_"+index;return Directory.GetFiles(root,stem+"-r*.json").OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault()??Path.Combine(root,stem+".json");}
 IEnumerator SearchRoute()
 {
  if(tokens.Count<2){yield return Prepare(Resolve(tokens[0]));Row("PASS route "+roomId+"/"+routeName+" stationary single-node (no traversal claim)");pass++;yield break;}
  var first=Resolve(tokens[0]);
  for(int index=1;index<tokens.Count;index++)
  {
   Node source=Resolve(tokens[index-1]),target=Resolve(tokens[index]);bestDistance=float.PositiveInfinity;bestHeight=bestTravel=float.NegativeInfinity;bestPosition=Vector2.zero;contacts="";bool found=false,exhausted=true;int tries=0;
   string file=FileFor(index);
   if(File.Exists(file)){var r=JsonUtility.FromJson<Recording>(File.ReadAllText(file));bool both=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){both=false;break;}yield return Play(r);both&=success;}if(both){prefix.Add(r);Row("PASS cached segment "+roomId+"/"+routeName+"/"+index+" replay=2");continue;}}
   foreach(var c in Candidates(source,target))
   {
    if(Time.realtimeSinceStartupAsDouble-itemStart>2400){exhausted=false;break;}
    yield return Prepare(first);if(!prefixOK){exhausted=false;Row("FAIL prefix replay "+roomId+"/"+routeName+"/"+index+" pos="+(Player==null?Vector2.zero:Body.position)+" feet="+(Alive?Feet:0));break;}
    yield return Attempt(source,target,c);tries++;attempted++;
    if(tries<=5)Row("MEASURE attempt "+roomId+"/"+routeName+"/"+index+" number="+tries+" success="+success+" dead="+dead+" end="+recording.end+" closest="+bestDistance+" grounded="+(Alive&&Motor.Grounded)+" feet="+(Alive?Feet:0)+" candidate="+JsonUtility.ToJson(c));
    if(tries%25==0)Row("MEASURE search "+roomId+"/"+routeName+"/"+index+" attempts="+tries+" closest="+bestDistance+" peak="+bestHeight+" travel="+bestTravel+" candidate="+JsonUtility.ToJson(c));
    if(!success)continue;
    var saved=recording;string candidateFile=Path.Combine(root,roomId+"_"+routeName+"_"+index+"-candidate-"+DateTime.UtcNow.ToString("HHmmssffff")+".json");File.WriteAllText(candidateFile,JsonUtility.ToJson(saved,true));
    // Verify by reading the saved file, never by a second adaptive controller.
    saved=JsonUtility.FromJson<Recording>(File.ReadAllText(candidateFile));bool both=true;
    for(int rep=0;rep<2;rep++){yield return Prepare(first);if(!prefixOK){both=false;break;}yield return Play(saved);both&=success;Row((success?"PASS":"FAIL")+" replay "+roomId+"/"+routeName+"/"+index+" repetition="+(rep+1));if(!success)break;}
    if(!both)continue;
    if(File.Exists(file))file=Path.Combine(root,roomId+"_"+routeName+"_"+index+"-r"+DateTime.UtcNow.ToString("HHmmssffff")+".json");File.Copy(candidateFile,file);prefix.Add(saved);found=true;Row("PASS segment "+roomId+"/"+routeName+"/"+index+" source="+source.token+" target="+target.token+" attempts="+tries+" replay=2 input="+file);break;
   }
   if(!found){string status=exhausted?"BLOCKED-GEOMETRY":"BUDGET-DEFERRED";Row(status+" route "+roomId+"/"+routeName+" segment="+index+" source="+source.token+" target="+target.token+" failure="+bestPosition+" closest="+bestDistance+" peak="+bestHeight+" travel="+bestTravel+" contacts="+contacts+" attempts="+tries+" classification=pending-physical-cause");if(exhausted)blocked++;else deferred++;yield break;}
  }
  // Entire route also needs two uninterrupted replays from its single initial fixture.
  bool routeOK=true;for(int rep=0;rep<2;rep++){yield return Prepare(first);routeOK&=prefixOK;}
  Row((routeOK?"PASS":"FAIL")+" route "+roomId+"/"+routeName+" replay=2 segments="+prefix.Count);if(routeOK)pass++;else deferred++;
 }
 IEnumerator ReplayAll()
 {
  foreach(var pair in rooms){roomId=pair.Key;foreach(var route in J.D(J.D(pair.Value)["routes"])){routeName=route.Key;tokens=((List<object>)route.Value).Select(x=>x.ToString()).ToList();prefix.Clear();bool exists=true;for(int i=1;i<tokens.Count;i++){string p=FileFor(i);if(!File.Exists(p)){exists=false;break;}prefix.Add(JsonUtility.FromJson<Recording>(File.ReadAllText(p)));}if(!exists){Row("SKIP incomplete input "+roomId+"/"+routeName);continue;}ability=15;bool both=true;for(int r=0;r<2;r++){yield return Prepare(Resolve(tokens[0]));both&=prefixOK;}Row((both?"PASS":"FAIL")+" fps-replay "+roomId+"/"+routeName+" fps="+Application.targetFrameRate+" replay=2");if(both)pass++;else deferred++;}}
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
 int Gate(string gate)=>gate.Contains("Slam")||gate.Contains("Boss")?8:gate.Contains("DJ")?4:gate.Contains("WJ")?2:gate.Contains("D")?1:0;
 int Bits(int mask)=>(mask&1)+((mask>>1)&1)+((mask>>2)&1)+((mask>>3)&1);
 Dictionary<string,int> MinimumStates()
 {
  var result=new Dictionary<string,int>();var visited=new HashSet<string>();var queue=new Queue<(string,int)>();queue.Enqueue(("A01",0));
  while(queue.Count>0){var state=queue.Dequeue();string id=state.Item1;int mask=state.Item2;if(!visited.Add(id+"/"+mask))continue;int outgoing=mask|(id=="A05"?1:id=="B04"?2:id=="C05"?4:id=="C06"?8:0);var d=J.D(rooms[id]);foreach(var obj in J.A(d,"doors")){var door=J.D(obj);string link=J.S(door,"id"),to=J.S(door,"to").Split('(')[0];if(id=="S02"||!J.A(J.D(rooms[to]),"spawns").Select(J.D).Any(s=>J.S(s,"id")==link))continue;int gate=Gate(J.S(door,"gate"));if((outgoing&gate)!=gate)continue;string key=to+"/"+link;if(!result.TryGetValue(key,out int old)||Bits(outgoing)<Bits(old))result[key]=outgoing;queue.Enqueue((to,outgoing));}}
  return result;
 }
 List<string> EntryPlan(string spawn,string exit,Vector2? actual=null)
 {
  string startToken=actual.HasValue?"point:"+actual.Value.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+actual.Value.y.ToString(System.Globalization.CultureInfo.InvariantCulture):"spawn:"+spawn;var start=Resolve(startToken);var end=Resolve("door:"+exit);var d=J.D(rooms[roomId]);List<string> chosen=new(){startToken,"door:"+exit};float score=Vector2.Distance(start.point,end.point)+(end.point.y>start.point.y+2?100:0);
  foreach(var pair in J.D(d["routes"])){
   var route=((List<object>)pair.Value).Select(x=>x.ToString()).ToList();if(route.Count==0)continue;
   for(int reverse=0;reverse<2;reverse++){var path=reverse==0?route:route.AsEnumerable().Reverse().ToList();int first=0,last=path.Count-1;float nearest=float.PositiveInfinity;for(int i=0;i<path.Count;i++){var node=Resolve(path[i]);if(actual.HasValue&&node.point.y>start.point.y+.2f)continue;float distance=Vector2.Distance(start.point,node.point);if(distance<nearest){nearest=distance;first=i;}}if(float.IsPositiveInfinity(nearest))continue;nearest=float.PositiveInfinity;for(int i=first;i<path.Count;i++){float distance=Vector2.Distance(end.point,Resolve(path[i]).point);if(distance<nearest){nearest=distance;last=i;}}
    if(last<first)continue;float cost=Vector2.Distance(start.point,Resolve(path[first]).point)+nearest;for(int i=first+1;i<=last;i++)cost+=Vector2.Distance(Resolve(path[i-1]).point,Resolve(path[i]).point)*.12f;if(path.Any(x=>x.EndsWith(exit)||x.Contains(exit+"@")))cost-=50;
    if(cost>=score)continue;score=cost;chosen=new List<string>{startToken};chosen.AddRange(path.Skip(first).Take(last-first+1));if(!chosen.Last().StartsWith("floor:")&&!chosen.Last().StartsWith("ceil:"))chosen.Add("door:"+exit);
   }
  }
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
    foreach(var exit in exits.OrderBy(e=>Vector2.Distance(Resolve("spawn:"+link).point,Resolve("door:"+J.S(e,"id")).point))){roomId=id;routeName=(isolation?"isolation-":"return-")+link+"-"+J.S(exit,"id");tokens=EntryPlan(link,J.S(exit,"id"));prefix.Clear();int before=pass;yield return SearchRoute();if(pass>before){escaped=true;Row("PASS "+(isolation?"minimum entry escape ":"directed return acquisition ")+id+"/"+link+" target="+J.S(exit,"id")+" abilities="+ability+" replay=2");if(!isolation){var returns=new List<Recording>(prefix);yield return Roundtrip(from,id,link,returns);}break;}if(Time.realtimeSinceStartupAsDouble-itemStart>2400)break;}
    if(!escaped)Row("PENDING entry escape "+id+"/"+link+" abilities="+ability+" pos="+Resolve("spawn:"+link).point);
   }
  }
 }
 IEnumerator Roundtrip(string origin,string destination,string link,List<Recording> returns)
 {
  roomId=origin;ability=15;routeName="roundtrip-"+link;var door=Resolve("door:"+link);var data=J.A(J.D(rooms[origin]),"doors").Select(J.D).First(p=>J.S(p,"id")==link);var rect=J.A(data,"rect");bool horizontal=J.F(rect[2])<=J.F(rect[3]);float side=horizontal?(door.point.x<J.F(J.A(J.D(rooms[origin]),"size")[0])/2?-1:1):0;
  Vector2 start=horizontal?new Vector2(door.point.x-side*(J.F(rect[2])/2+.7f),J.F(rect[1])+.02f):new Vector2(door.point.x,door.point.y>2?J.F(rect[1])-2.3f:J.F(rect[1])+J.F(rect[3])+1.5f);
  var source=new Node{token="point:"+start.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+start.y.ToString(System.Globalization.CultureInfo.InvariantCulture),point=start,left=start.x,right=start.x,surface=false};
  yield return Reset(start);var c=new Candidate{launch=start.x,hold=.4f,dj=.35f,dash=-1,wall=.02f,attack=-1,slam=door.point.y<2?.15f:-1,air=0};yield return Attempt(source,door,c);if(!success){Row("FAIL roundtrip entry acquisition "+origin+"/"+link+" pos="+recording.end);deferred++;yield break;}var entryRecording=recording;yield return Ready();roomId=destination;
  Vector2 arrival=Body.position;recording=new Recording{room=destination,route=routeName,source="point:"+arrival.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture),fps=Application.targetFrameRate,abilities=15,start=arrival};double checkUntil=Time.realtimeSinceStartupAsDouble+.4;int held=horizontal?(side>0?2:1):(door.point.y>2?64:32);bool stable=true;
  while(Time.realtimeSinceStartupAsDouble<checkUntil){yield return Emit(held);stable&=RoomSession.Instance.CurrentRoomId==destination;}
  float center=J.F(J.A(J.D(rooms[destination]),"size")[0])/2;int walk=Body.position.x<center?2:1;double limit=Time.realtimeSinceStartupAsDouble+2;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&Mathf.Abs(Body.position.x-arrival.x)<3&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(walk);
  if(Mathf.Abs(Body.position.x-arrival.x)<3){walk=walk==2?1:2;limit=Time.realtimeSinceStartupAsDouble+2;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&Mathf.Abs(Body.position.x-arrival.x)<3&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(walk);}
  float moved=Mathf.Abs(Body.position.x-arrival.x);limit=Time.realtimeSinceStartupAsDouble+6;while(Alive&&RoomSession.Instance.CurrentRoomId==destination&&!Motor.Grounded&&Time.realtimeSinceStartupAsDouble<limit)yield return Emit(0);
  recording.target="point:"+Body.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture);var holdRecording=recording;stable&=RoomSession.Instance.CurrentRoomId==destination&&moved>=3;
  Row((stable?"MEASURE":"FAIL")+" arrival hold "+origin+" -> "+destination+" "+link+" no-return-0.4="+stable+" moveX="+moved);if(!stable){deferred++;yield break;}
  roomId=destination;routeName="roundtrip-return-"+link;tokens=EntryPlan(link,link,new Vector2(Body.position.x,Feet));prefix.Clear();itemStart=Time.realtimeSinceStartupAsDouble;int before=pass;yield return SearchRoute();if(pass<=before){Row("DEPENDENCY roundtrip return from actual held arrival "+destination+"/"+link);yield break;}returns=new List<Recording>(prefix);
  bool both=true;for(int rep=0;rep<2;rep++){roomId=origin;yield return Reset(start);yield return Play(entryRecording);bool ok=success;yield return Ready();roomId=destination;yield return Play(holdRecording);ok&=success;foreach(var r in returns){roomId=destination;yield return Play(r);ok&=success;if(!ok)break;}ok&=RoomSession.Instance.CurrentRoomId==origin;both&=ok;Row((ok?"PASS":"FAIL")+" roundtrip saved replay "+origin+" -> "+destination+" -> "+origin+" "+link+" repetition="+(rep+1));}
  string file=Path.Combine(root,origin+"_roundtrip_"+link+"-"+DateTime.UtcNow.ToString("HHmmssffff")+".json");var all=new List<Recording>{entryRecording,holdRecording};all.AddRange(returns);File.WriteAllText(file,JsonUtility.ToJson(new CourseRecording{segments=all},true));if(both)Row("PASS roundtrip "+origin+"/"+link+" replay=2 input="+file);else deferred++;
 }
 IEnumerator Fight(float lead,float gap)
 {
  var boss=FindFirstObjectByType<BossEncounter>();recording=new Recording{room=roomId,route="boss",source="boss-start",target="boss-victory",start=Body.position,abilities=7,fps=Application.targetFrameRate};
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
  string[][] plan={new[]{"A01","main","A01-A02"},new[]{"A02","floor","A02-A03"},new[]{"A03","roofs","A03-A04"},new[]{"A04","high","A04-A05"},new[]{"A05","main","A05-A03"},new[]{"A03","high","A03-B01"},new[]{"B01","balcony","B01-B02"},new[]{"B02","low","B02-B03"},new[]{"B03","climb","B03-B04low"},new[]{"B04","low",""},new[]{"B04","wj","B04-B02"},new[]{"B02","high-reverse",""},new[]{"B02","low-reverse","B01-B02"},new[]{"B01","balcony-reverse","B01-A03pit"},new[]{"A03","roofs-reverse",""},new[]{"A03","street-reverse","A02-A03"},new[]{"A02","wall","A02-B05"},new[]{"B05","climb","B05-C01"},new[]{"C01","high","C01-C02high"},new[]{"C02","up","C02-C03high"},new[]{"C03","c05","C03-C05"},new[]{"C05","down",""},new[]{"C05","up_dj","C03-C05"},new[]{"C03","gj","C03-C04"},new[]{"C04","dj","C04-C06"},new[]{"C06","boss",""},new[]{"C06","exit","C06-S01"},new[]{"S01","walk","ENDING"},new[]{"A01","slam","A01-Z01"},new[]{"Z01","in",""},new[]{"Z01","dj",""},new[]{"Z01","gallery",""},new[]{"Z01","lower",""},new[]{"Z01","elevator","Z01-S02"}};
  foreach(var step in plan){roomId=step[0];routeName=step[1];yield return Ready();if(RoomSession.Instance.CurrentRoomId!=roomId){Row("FAIL natural unexpected room="+RoomSession.Instance.CurrentRoomId+" expected="+roomId);deferred++;break;}
   double began=Time.realtimeSinceStartupAsDouble;int frameStart=worldFrames.Count;ability=(Player.GetComponent<PlayerDash>().hasDash?1:0)|(Motor.hasWallJump?2:0)|(Motor.hasDoubleJump?4:0)|(Player.GetComponent<PlayerSlam>().hasSlam?8:0);
   Row("BEGIN natural "+roomId+"/"+routeName+" abilities="+ability+" pos="+Body.position);
   if(routeName=="boss"){
    SaveNatural("before-boss");yield return NaturalMove("solid0~3.5");if(!success)break;string bossFile=Directory.GetFiles(root,"C06_boss-*.json").FirstOrDefault(p=>p.Contains("2239040430"));if(bossFile==null){Row("DEPENDENCY natural validated boss input absent");deferred++;break;}yield return PlayBoss(JsonUtility.FromJson<Recording>(File.ReadAllText(bossFile)));if(!success){Row("FAIL natural boss actual HP="+(Alive?Health.HP:0));deferred++;break;}
   }else{
    bool reverse=routeName.EndsWith("-reverse");string key=routeName.Replace("-reverse","");tokens=((List<object>)J.D(J.D(rooms[roomId])["routes"])[key]).Select(x=>x.ToString()).ToList();if(reverse)tokens.Reverse();
    yield return NaturalMove(tokens[0]);if(!success){Row("FAIL natural approach "+roomId+"/"+routeName+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}
    if(roomId=="C05"&&!Motor.hasDoubleJump){yield return NaturalAcquire("platform0~28",4);if(!success)break;}
    for(int i=1;i<tokens.Count;i++){if(roomId=="A05"&&tokens[i].Contains("@dash")&&!Player.GetComponent<PlayerDash>().hasDash){yield return NaturalAcquire("solid1~22",1);if(!success)break;}string f=FileFor(i);if(!reverse&&File.Exists(f)){yield return Play(JsonUtility.FromJson<Recording>(File.ReadAllText(f)));if(!success&&Alive&&RoomSession.Instance.CurrentRoomId==roomId){Row("MEASURE natural saved segment correction "+roomId+"/"+routeName+"/"+i+" pos="+Body.position);yield return NaturalMove(tokens[i]);}}else yield return NaturalMove(tokens[i]);if(!success)break;}
    if(!success){Row("FAIL natural route "+roomId+"/"+routeName+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}
   }
   if(roomId=="B04"&&!Motor.hasWallJump){yield return NaturalAcquire("solid1~14",2);if(!success)break;}
   if(roomId=="A05"&&Player.GetComponent<PlayerDash>().hasDash)SaveNatural("after-dash");if(roomId=="B04"&&Motor.hasWallJump)SaveNatural("after-wall");
   if(step[2]=="ENDING"){
    yield return NaturalMove("point:136,1");if(FindFirstObjectByType<EndingMenu>()==null){Row("FAIL natural ending trigger");deferred++;break;}yield return Emit(0);yield return Emit(4);yield return Emit(0);yield return Ready();SaveNatural("after-ending");
   }else if(step[2]!=""&&RoomSession.Instance.CurrentRoomId==roomId){yield return NaturalMove("door:"+step[2]);if(!success){Row("FAIL natural door "+roomId+"/"+step[2]+" pos="+(Player==null?Vector2.zero:Body.position));deferred++;break;}}
   worldTimes.Add(roomId+","+routeName+","+(Time.realtimeSinceStartupAsDouble-began).ToString(System.Globalization.CultureInfo.InvariantCulture)+","+(worldFrames.Count-frameStart));Row("MEASURE natural stage "+roomId+"/"+routeName+" next="+RoomSession.Instance?.CurrentRoomId);
  }
  worldCapture=false;string stamp=DateTime.UtcNow.ToString("HHmmssffff");File.WriteAllText(Path.Combine(root,"natural-"+stamp+".json"),JsonUtility.ToJson(new Recording{room="A01",route="natural",target="S02",abilities=0,fps=Application.targetFrameRate,frames=worldFrames},true));File.WriteAllLines(Path.ChangeExtension(Evidence,"rooms.csv"),worldTimes);
  bool complete=RoomSession.Instance?.CurrentRoomId=="S02"&&ProgressSave.ReachedS02;Row((complete?"PASS":"PENDING")+" natural new game -> S02 no placement/grants actual="+RoomSession.Instance?.CurrentRoomId);if(complete)pass++;else deferred++;
 }
 IEnumerator NaturalMove(string token)
 {
  var target=Resolve(token);if(Goal(target,Player.GetComponent<PlayerAttack>().PogosPerformed)){success=true;yield break;}
  var source=Resolve("point:"+Body.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+Feet.ToString(System.Globalization.CultureInfo.InvariantCulture));source.surface=Motor.Grounded;
  var c=new Candidate{launch=Body.position.x,hold=.4f,dj=target.token.Contains("@dj")?.3f:-1,dash=target.token.Contains("@dash")?.1f:-1,wall=target.token.Contains("@wj")?.02f:-1,attack=target.token.StartsWith("pogo:")||target.token.StartsWith("enemy:")?.1f:-1,slam=target.token.Contains("@slam")?.3f:-1,air=1};
  if(source.surface&&target.point.y>source.point.y+.15f&&(target.point.x>source.right||target.point.x<source.left))c.launch=target.point.x>source.right?source.right-.35f:source.left+.35f;
  if(target.token.StartsWith("door:")&&target.point.y<2&&Player.GetComponent<PlayerSlam>().hasSlam)c.slam=.3f;
  yield return Attempt(source,target,c);
  Row((success?"MEASURE":"FAIL")+" natural move "+roomId+" source="+source.token+" target="+token+" end="+recording.end+" HP="+(Player==null?0:Health.HP)+" frames="+recording.frames.Count);
 }
 void SaveNatural(string tag){ProgressSave.SaveCurrent(RoomSession.Instance);if(File.Exists(ProgressSave.FilePath)){string p=Path.Combine(Path.GetDirectoryName(Evidence),"natural-"+tag+"-"+DateTime.UtcNow.ToString("HHmmssffff")+"-save.json");File.Copy(ProgressSave.FilePath,p);Row("MEASURE natural save "+tag+" file="+p+" room="+RoomSession.Instance.CurrentRoomId);}}
 IEnumerator NaturalAcquire(string ground,int wanted)
 {
  yield return NaturalMove(ground);if(!success)yield break;yield return Emit(0,false);double hold=Time.timeAsDouble+.4;while(Alive&&Time.timeAsDouble<hold)yield return Emit(4,false);double end=Time.timeAsDouble+2;while(Alive&&!Motor.Grounded&&Time.timeAsDouble<end)yield return Emit(0,false);
  success=Alive&&(wanted==1?Player.GetComponent<PlayerDash>().hasDash:wanted==2?Motor.hasWallJump:Motor.hasDoubleJump);Row((success?"MEASURE":"FAIL")+" natural pickup contact ability="+wanted+" room="+roomId+" pos="+(Player==null?Vector2.zero:Body.position));
 }
}
