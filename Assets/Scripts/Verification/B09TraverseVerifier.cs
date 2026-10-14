#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using J=B09Json;
public sealed class B09TraverseVerifier:N5Runner
{
 Dictionary<string,object> data;int loaded,passed,failed,jumpAt=-100,nextAttack;bool jumpHeld;int priorWall;
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09TraverseVerifier>("-verify-b09-traverse");
 void Loaded(Scene s,LoadSceneMode mode){if(s.name.EndsWith("_V4"))loaded++;}
 protected override void Cleanup(){SceneManager.sceneLoaded-=Loaded;}
 protected override IEnumerator Tests()
 {
  data=J.D(J.Read(Resources.Load<TextAsset>("V4Design").text)["rooms"]);SceneManager.sceneLoaded+=Loaded;yield return NewGame();ProgressSave.Enabled=false;
  string mode=N5Args.Value("-verify-b09-traverse","connections");
  if(mode=="scenes"){foreach(string id in V4SceneMap.Rooms){yield return Load(id,true);Check(FindObjectsByType<EnemyReset>(FindObjectsSortMode.None).All(e=>e.prefab!=null&&e.Live!=null),"scene references "+id);passed++;}}else if(mode=="connections")yield return Connections();else if(mode=="world")yield return World();else if(mode=="boss"){yield return Load("C06",false);Player.GetComponent<PlayerDash>().hasDash=true;Motor.hasWallJump=Motor.hasDoubleJump=true;yield return Fight();passed++;}else yield return Routes(mode=="isolation");
  lines.Add("MEASURE mode="+mode+" PASS="+passed+" FAIL="+failed);if(failed>0)throw new Exception("Incomplete actual input checks; see per-item classifications");
 }
 IEnumerator World()
 {
  // No fixture placement or ability grants: stop at the first unresolved natural segment.
  string[][] plan={new[]{"A01","main","A01-A02"},new[]{"A02","floor","A02-A03"},new[]{"A03","roofs","A03-A04"},new[]{"A04","high","A04-A05"},new[]{"A05","main","A05-A03"},new[]{"A03","high","A03-B01"},new[]{"B01","balcony","B01-B02"},new[]{"B02","low","B02-B03"},new[]{"B03","climb","B03-B04low"},new[]{"B04","low,wj","B04-B02"},new[]{"B02","high-reverse,low-reverse","B01-B02"},new[]{"B01","balcony-reverse","B01-A03pit"},new[]{"A03","roofs-reverse,street-reverse","A02-A03"},new[]{"A02","wall","A02-B05"},new[]{"B05","climb","B05-C01"},new[]{"C01","high","C01-C02high"},new[]{"C02","up","C02-C03high"},new[]{"C03","c05","C03-C05"},new[]{"C05","down,up_dj","C03-C05"},new[]{"C03","gj","C03-C04"},new[]{"C04","dj","C04-C06"},new[]{"C06","boss,exit","C06-S01"},new[]{"S01","walk","ENDING"},new[]{"A01","slam","A01-Z01"},new[]{"Z01","in,dj,gallery,lower,elevator","Z01-S02"}};
  foreach(var segment in plan)
  {
   string id=segment[0];if(RoomSession.Instance.CurrentRoomId!=id){lines.Add("FAIL world unexpected room="+RoomSession.Instance.CurrentRoomId+" expected="+id);failed++;yield break;}
   var d=J.D(data[id]);var nodes=new List<object>();foreach(string part in segment[1].Split(',')){if(part=="boss"){yield return Fight();continue;}string name=part.Replace("-reverse","");var route=(List<object>)J.D(d["routes"])[name];nodes.AddRange(part.EndsWith("-reverse")?route.AsEnumerable().Reverse():route);}int tick=0;
   foreach(var node in nodes){Vector2 goal=Resolve(d,node.ToString());double until=Time.timeAsDouble+20;while(Vector2.Distance(Body.position,goal)>.9f&&Time.timeAsDouble<until&&RoomSession.Instance.CurrentRoomId==id&&!RoomSession.Instance.Respawning){Navigate(goal,node.ToString(),tick++);yield return null;}if(RoomSession.Instance.Respawning||Time.timeAsDouble>=until){lines.Add("FAIL world "+id+" "+node+" pos="+Body.position+" actual-input-limit; no grants/placement");failed++;yield break;}}
   if(segment[2]=="ENDING"){yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"natural summit ending",10);yield return Tap(Key.Z);yield return Ready();lines.Add("PASS ending -> A01 abilities retained Slam="+Player.GetComponent<PlayerSlam>().hasSlam);passed++;continue;}var exit=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).First(e=>e.exitId==segment[2]);double deadline=Time.timeAsDouble+15;while(RoomSession.Instance.CurrentRoomId==id&&Time.timeAsDouble<deadline){Navigate(exit.GetComponent<Collider2D>().bounds.center,id=="A01"&&segment[2]=="A01-Z01"?"@slam":"@dj",tick++);yield return null;}if(RoomSession.Instance.CurrentRoomId==id){lines.Add("FAIL world exit "+segment[2]);failed++;yield break;}yield return Ready();lines.Add("PASS world "+id+" -> "+RoomSession.Instance.CurrentRoomId);passed++;
  }
  Check(RoomSession.Instance.CurrentRoomId=="S02"&&ProgressSave.ReachedS02,"natural complete S02");
 }
 IEnumerator Fight()
 {
  var boss=FindFirstObjectByType<BossEncounter>();double end=Time.timeAsDouble+90,phase=-1;int tick=0,jump=-100,attack=0;
  while(boss!=null&&boss.HP>0&&!RoomSession.Instance.Respawning&&Time.timeAsDouble<end){var keys=new List<Key>();float x=Body.position.x,anchor=boss.GetComponent<Collider2D>().bounds.min.x-.45f;if(x<anchor-.05f)keys.Add(Key.RightArrow);else if(x>anchor+.05f)keys.Add(Key.LeftArrow);if(!Motor.Grounded&&x>anchor+2&&Body.position.y>boss.GetComponent<Collider2D>().bounds.max.y+1&&!Player.GetComponent<PlayerDash>().AirUsed)keys.Add(Key.C);double elapsed=Time.timeAsDouble-boss.PhaseStarted;if(Motor.Grounded&&phase!=boss.PhaseStarted&&((boss.State==BossEncounter.Phase.MeleeTell&&elapsed>boss.tuning.meleeTell-.38)||(boss.State==BossEncounter.Phase.WaveTell&&elapsed>boss.tuning.waveTell-.38))){jump=tick;phase=boss.PhaseStarted;}if(tick-jump<28)keys.Add(Key.Z);if(Motor.Grounded&&tick>=attack){keys.Remove(Key.LeftArrow);keys.Add(Key.RightArrow);keys.Add(Key.X);attack=tick+20;}InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));tick++;yield return null;}
  Check(ProgressSave.BossDefeated&&Player.GetComponent<PlayerSlam>().hasSlam,"actual boss victory grants Slam");yield return Keys();
 }
 IEnumerator Load(string id,bool all)
 {
  if(RoomSession.Instance==null){if(FindFirstObjectByType<EndingMenu>()!=null)yield return Tap(Key.Z);else SceneManager.LoadScene(V4SceneMap.Path("A01"));yield return Ready();}
  yield return Keys();yield return Until(()=>!RoomSession.Instance.Respawning&&!RoomSession.Instance.Transitioning,"fixture waits for prior death/transition",12);Health.RestoreMax();ProgressSave.HasSlam=all;ProgressSave.BossDefeated=all;
  if(RoomSession.Instance.CurrentRoomId!=id){if(!RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),"Entry"))throw new Exception("setup scene "+id);yield return Ready();}else{var room=FindFirstObjectByType<RoomDefinition>();room.TryGetSpawn("Entry",out var spawn);PlaceForAuthorizedSetup(spawn.transform.position);}
  Player.GetComponent<PlayerDash>().hasDash=all;Motor.hasDoubleJump=all;Motor.hasWallJump=all;Player.GetComponent<PlayerSlam>().hasSlam=all;
  foreach(var seal in FindObjectsByType<SealFloor>(FindObjectsSortMode.None))if(all)seal.Break(false);
  yield return new WaitForSeconds(.5f);Health.RestoreMax();jumpAt=-100;priorWall=Motor.WallJumpsPerformed;jumpHeld=false;
 }
 IEnumerator Connections()
 {
  foreach(string id in V4SceneMap.Rooms)
  {
   var doors=J.A(J.D(data[id]),"doors");foreach(var node in doors)
   {
    var p=J.D(node);string target=J.S(p,"to").Split('(')[0],link=J.S(p,"id");if(id=="S02"||!J.A(J.D(data[target]),"spawns").Select(J.D).Any(s=>J.S(s,"id")==link)){lines.Add("ONEWAY inactive return marker "+id+" "+link);continue;}
    yield return Load(id,true);var exit=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).First(e=>e.exitId==link);if(!exit.enabled){lines.Add("MANUAL "+id+" "+link+" boss-controlled exit");failed++;continue;}
    var b=exit.GetComponent<Collider2D>().bounds;bool horizontal=b.size.x<=b.size.y;int direction=horizontal?(b.center.x<FindFirstObjectByType<V4Room>().dimensions.x/2?-1:1):(b.center.y<FindFirstObjectByType<V4Room>().dimensions.y/2?-1:1);
    Vector2 start=horizontal?new Vector2(b.center.x-direction*(b.extents.x+.7f),b.min.y+.81f):new Vector2(b.center.x,b.center.y-direction*(b.extents.y+1));PlaceForAuthorizedSetup(start);yield return new WaitForFixedUpdate();
    Key key=horizontal?(direction>0?Key.RightArrow:Key.LeftArrow):(direction>0?Key.UpArrow:Key.DownArrow);double deadline=Time.timeAsDouble+4;int before=loaded;
    while(RoomSession.Instance.CurrentRoomId==id&&Time.timeAsDouble<deadline){InputSystem.QueueStateEvent(keyboard,horizontal||direction<0?new KeyboardState(key):new KeyboardState(key,Key.Z));yield return null;}
    if(RoomSession.Instance.CurrentRoomId!=target){lines.Add("FAIL connection "+id+" -> "+target+" "+link+" entry pos="+Body.position+" classification=automatic-input-limit/manual");failed++;continue;}
    yield return Ready();Vector2 arrived=Body.position;int once=loaded;double hold=Time.timeAsDouble+2;
    while(Time.timeAsDouble<hold){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;}
    float moved=Vector2.Distance(arrived,Body.position);bool stable=RoomSession.Instance.CurrentRoomId==target&&loaded==once&&once-before==1;
    lines.Add((stable&&moved>=3?"PASS":"FAIL")+" hold "+id+" -> "+target+" "+link+" extra="+(loaded-once)+" move="+moved);if(!stable||moved<3)failed++;else passed++;
    yield return Keys();yield return new WaitForSeconds(1);
    var reverse=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).FirstOrDefault(e=>e.exitId==link&&e.enabled);if(reverse==null){lines.Add("ONEWAY "+link+" no reverse exit");continue;}
    deadline=Time.timeAsDouble+10;int tick=0;
    while(RoomSession.Instance.CurrentRoomId==target&&Time.timeAsDouble<deadline){Navigate(reverse.GetComponent<Collider2D>().bounds.center,"@dj",tick++);yield return null;}
    bool returned=RoomSession.Instance.CurrentRoomId==id;lines.Add((returned?"PASS":"FAIL")+" return "+target+" -> "+id+" "+link+" classification="+(returned?"actual-input":"automatic-input-limit/manual"));if(returned)passed++;else failed++;
   }
  }
 }
 IEnumerator Routes(bool isolation)
 {
  foreach(string id in V4SceneMap.Rooms)
  {
   var d=J.D(data[id]);var routes=J.D(d["routes"]);foreach(var route in routes)
   {
    if(isolation&&route.Key!=routes.Keys.First())continue;
    yield return Load(id,!isolation);var nodes=(List<object>)route.Value;if(nodes.Count==0)continue;
    var first=Resolve(d,nodes[0].ToString());PlaceForAuthorizedSetup(first);yield return new WaitForSeconds(.15f);
    string tags=string.Join(" ",nodes.Select(n=>n.ToString()));Player.GetComponent<PlayerDash>().hasDash=!isolation||tags.Contains("@dash");Motor.hasWallJump=!isolation||tags.Contains("@wj");Motor.hasDoubleJump=!isolation||tags.Contains("@dj");Player.GetComponent<PlayerSlam>().hasSlam=!isolation||tags.Contains("@slam");
    bool ok=true;int index=1,tick=0;double last=Time.timeAsDouble;
    while(index<nodes.Count)
    {
     if(FindFirstObjectByType<EndingMenu>()!=null&&id=="S01"){index=nodes.Count;break;}
     if(RoomSession.Instance==null||Player==null){ok=false;break;}
     if(RoomSession.Instance.CurrentRoomId!=id){if(index==nodes.Count-1&&(nodes[index].ToString().StartsWith("ceil:")||nodes[index].ToString().StartsWith("floor:")))index=nodes.Count;else ok=false;break;}
     if(RoomSession.Instance.Respawning||Health.HP<=0){ok=false;break;}string token=nodes[index].ToString();Vector2 goal=Resolve(d,token);
     if(Vector2.Distance(Body.position,goal)<.85f||(Mathf.Abs(Body.position.x-goal.x)<.6f&&Body.position.y>=goal.y-.3f&&Motor.Grounded)){index++;last=Time.timeAsDouble;jumpAt=-100;continue;}
     if(Time.timeAsDouble-last>8){ok=false;break;}Navigate(goal,token,tick++);yield return null;
    }
    lines.Add((ok?"PASS":"MANUAL")+" route "+id+"/"+route.Key+" waypoints="+index+"/"+nodes.Count+" pos="+(Player==null?Vector2.zero:Body.position)+" classification="+(ok?"actual-input":"automatic-input-limit; product/data fault not established"));if(ok)passed++;else failed++;yield return Keys();
   }
  }
 }
 Vector2 Resolve(Dictionary<string,object>d,string token)
 {
  string bare=token.Split('@')[0];var a=bare.Split('~');bare=a[0];float x;
  if(bare.StartsWith("floor:")||bare.StartsWith("ceil:")){string link=bare.Substring(bare.IndexOf(':')+1);var door=J.A(d,"doors").Select(J.D).First(p=>J.S(p,"id")==link);var r=J.A(door,"rect");return new Vector2(J.F(r[0])+J.F(r[2])/2,J.F(r[1])+J.F(r[3])/2);}
  if(bare.StartsWith("pogo:")||bare.StartsWith("enemy:")){string kind=bare.StartsWith("pogo:")?"pogo":"enemies";var p=J.A(d,kind).Select(J.D).First(v=>J.S(v,"id")==bare.Split(':')[1]);return new Vector2(J.F(p,"x"),J.F(p,"y")+(kind=="pogo"?.5f:1)+1.3f);}
  if(bare.StartsWith("lift:")){var p=J.A(d,"lifts").Select(J.D).First(v=>J.S(v,"id")==bare.Split(':')[1]);return new Vector2((J.F(p,"x0")+J.F(p,"x1"))/2,J.F(p,bare.EndsWith(":lo")?"lo":"hi"));}
  var match=System.Text.RegularExpressions.Regex.Match(bare,@"^([a-z]+)(\d+)$");if(!match.Success)throw new Exception("unknown design route node "+token);var rect=(List<object>)J.A(d,match.Groups[1].Value)[int.Parse(match.Groups[2].Value)];x=a.Length>1?float.Parse(a[1],System.Globalization.CultureInfo.InvariantCulture):J.F(rect[0])+J.F(rect[2])/2;return new Vector2(x,J.F(rect[1])+J.F(rect[3])+.01f);
 }
 void Navigate(Vector2 goal,string token,int tick)
 {
  float dx=goal.x-Body.position.x,dy=goal.y-Body.position.y;var keys=new List<Key>();int dir=Mathf.Abs(dx)<.15f?0:dx>0?1:-1;if(dir!=0)keys.Add(dir>0?Key.RightArrow:Key.LeftArrow);
  bool under=false;var shape=Player.GetComponent<BoxCollider2D>().bounds;
  if(dy>.6f&&!token.Contains("@wj")){var ceiling=Physics2D.BoxCast(shape.center,new Vector2(.6f,1.45f),0,Vector2.up,Mathf.Max(0,dy),1<<6);if(ceiling.collider!=null&&ceiling.collider.bounds.min.y>shape.min.y+.1f){if(Motor.Grounded){under=true;var b=ceiling.collider.bounds;keys.Remove(Key.RightArrow);keys.Remove(Key.LeftArrow);keys.Add(Body.position.x-b.min.x<b.max.x-Body.position.x?Key.LeftArrow:Key.RightArrow);}else if(Body.linearVelocity.y>0){keys.Remove(Key.RightArrow);keys.Remove(Key.LeftArrow);}}}
  if(Motor.WallSliding){if(Motor.WallJumpsPerformed>priorWall){priorWall=Motor.WallJumpsPerformed;jumpHeld=false;}else jumpHeld=!jumpHeld;if(jumpHeld)keys.Add(Key.Z);}
  else{if(Motor.Grounded&&!under&&tick-jumpAt>40&&(dy>.25f||Mathf.Abs(dx)>1))jumpAt=tick;int elapsed=tick-jumpAt;if(elapsed<28&&!under)keys.Add(Key.Z);if(token.Contains("@dj")&&elapsed>=30&&elapsed<55)keys.Add(Key.Z);if(token.Contains("@dash")&&elapsed==18)keys.Add(Key.C);}
  if(token.Contains("@wj")&&!Motor.Grounded){var wall=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).OrderBy(w=>Vector2.Distance(w.transform.position,Body.position)).FirstOrDefault();if(wall!=null&&dy>1){keys.Remove(Key.RightArrow);keys.Remove(Key.LeftArrow);keys.Add(wall.transform.position.x>Body.position.x?Key.RightArrow:Key.LeftArrow);}}
  if(token.Contains("@slam")&&!Motor.Grounded&&Body.linearVelocity.y<1){keys.Add(Key.DownArrow);if(tick%20==0)keys.Add(Key.C);}
  if(token.StartsWith("pogo:")||token.StartsWith("enemy:")){if(Body.linearVelocity.y<=1&&tick>=nextAttack){keys.Add(Key.DownArrow);keys.Add(Key.X);nextAttack=tick+20;}}
  if(token.StartsWith("cover")||token.StartsWith("breakable")||token.Contains("@lift")){if(tick>=nextAttack){keys.Add(Key.X);nextAttack=tick+20;}}
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));
 }
}



#endif
