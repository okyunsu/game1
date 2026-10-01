using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public sealed class SliceVerification : MonoBehaviour
{
 public string outputPath;
 public bool traverse;
 public bool roundTrip;
 public Action<bool> finished;
 readonly List<string> results=new(); readonly List<string> errors=new();
 void Log(string msg,string stack,LogType kind){if(kind is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(msg+stack);}
 void Check(bool ok,string label){if(!ok)throw new Exception(label);results.Add("PASS: "+label);}
 public IEnumerator Start(){
  DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;
  QualitySettings.vSyncCount=0;Application.targetFrameRate=60;
  var stack=new Stack<IEnumerator>();if(traverse)stack.Push(TraversalChecks());stack.Push(CheckpointChecks());bool fail=false;
  while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){results.Add("FAIL: "+e);fail=true;}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}
  Application.logMessageReceived-=Log;fail|=errors.Count>0;results.AddRange(errors);results.Insert(0,$"{(fail?"FAIL":"PASS")} checkpoint/death; Unity={Application.unityVersion}; UTC={DateTime.UtcNow:O}");
  if(File.Exists(outputPath))throw new IOException("Preserve evidence: "+outputPath);
  Directory.CreateDirectory(Path.GetDirectoryName(outputPath));File.WriteAllLines(outputPath,results);
  if(finished!=null)finished(!fail);else Application.Quit(fail?1:0);
 }
 IEnumerator Wait(RoomSession session){double end=Time.realtimeSinceStartupAsDouble+3;while((session.Respawning||session.Transitioning)&&Time.realtimeSinceStartupAsDouble<end)yield return null;Check(!session.Respawning&&!session.Transitioning,"respawn/transition completes within 3s");}
 void Place(RoomSession session,Vector2 p){var body=session.Player.GetComponent<Rigidbody2D>();session.Player.transform.position=p;body.position=p;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Death(RoomSession session,string room,string spawnId){
  var keyboard=InputSystem.AddDevice<Keyboard>();
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Space));yield return null;
  double begin=Time.realtimeSinceStartupAsDouble;
  var zone=FindFirstObjectByType<KillZone>();Place(session,zone.transform.position);yield return new WaitForFixedUpdate();yield return null;
  Check(session.Respawning&&session.Player.InputLocked,"Kill Zone blocks input during death");
  Check(!session.Die()&&!session.RequestTransition("A02","Assets/Scenes/A02.unity","FromLeft"),"duplicate death and room transition rejected during death");
  yield return Wait(session);
  double elapsed=Time.realtimeSinceStartupAsDouble-begin;results.Add($"MEASURE: death->control={elapsed:F3}s; target=3s; delay={session.respawnDelay:F2}s");
  Check(elapsed>=.58&&elapsed<3&&Time.timeScale==1&&!session.Player.InputLocked,"death delay ~0.6s and playable time restored");
  Check(session.CurrentRoomId==room,"returned to expected checkpoint room "+room);
  var definition=FindFirstObjectByType<RoomDefinition>();Check(definition.TryGetSpawn(spawnId,out var spawn),"respawn Spawn ID resolves");
  Check(Vector2.Distance(session.Player.transform.position,spawn.transform.position)<.15f,"respawn at safe authored spawn");
  yield return new WaitForSeconds(.2f);
  Check(!session.Respawning&&session.Player.GetComponent<PlayerMotor>().Grounded,"no immediate redeath or sticking");
  Check(double.IsNegativeInfinity(session.Player.JumpPressedAt),"respawn clears pending jump buffer");
  InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.D));yield return null;yield return new WaitForFixedUpdate();
  Check(session.Player.Move.x>.5f,"movement available after respawn");
  InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;InputSystem.RemoveDevice(keyboard);
 }
 IEnumerator Touch(RoomSession session,string id){var cp=FindFirstObjectByType<Checkpoint>();Check(cp!=null&&cp.checkpointId==id,"checkpoint prefab/unique ID "+id);Place(session,cp.transform.position);yield return new WaitForFixedUpdate();yield return null;Check(session.CheckpointId==id,"contact activates checkpoint "+id);Check(cp.GetComponent<SpriteRenderer>().color==cp.activeColor,"active checkpoint visual state");}
 IEnumerator CheckpointChecks(){
  yield return new WaitForSeconds(.2f);var session=RoomSession.Instance;Check(session!=null&&session.CurrentRoomId=="A01","A01 startup");
#if UNITY_EDITOR
  InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
  InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
  int playerId=session.Player.GetInstanceID();Check(session.CheckpointId==null,"no checkpoint fallback case");
  yield return Death(session,"A01","Entry");yield return Touch(session,"CP-A01");yield return Death(session,"A01","CP-A01");
  Check(session.RequestTransition("A03","Assets/Scenes/A03.unity","Entry"),"load alternate checkpoint room");yield return Wait(session);
  yield return Touch(session,"CP-A03");yield return Death(session,"A03","CP-A03");
  Check(session.RequestTransition("A04","Assets/Scenes/A04.unity","FromLeft"),"leave checkpoint room");yield return Wait(session);
  Check(session.CheckpointId=="CP-A03","room entry preserves checkpoint");yield return Death(session,"A03","CP-A03");yield return Death(session,"A03","CP-A03");
  Check(session.Player.GetInstanceID()==playerId&&FindObjectsByType<PlayerInputReader>(FindObjectsSortMode.None).Length==1,"one player retained across repeated cross-room respawn");
 }
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static void StartBuildVerification(){
  var args=Environment.GetCommandLineArgs();
  if(Array.IndexOf(args,"--slice-verify")<0)return;
  int pathIndex=Array.IndexOf(args,"--verification-output");
  var runner=new GameObject("Command-line Slice verification").AddComponent<SliceVerification>();
  runner.traverse=true;runner.roundTrip=true;
  runner.outputPath=pathIndex>=0&&pathIndex+1<args.Length?args[pathIndex+1]:Path.Combine(Application.persistentDataPath,"Slice-build-verification.txt");
 }
 void Keys(Keyboard keyboard,int direction,bool jump=false){
  var keys=new List<Key>();if(direction>0)keys.Add(Key.D);if(direction<0)keys.Add(Key.A);if(jump)keys.Add(Key.Space);
  InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));
 }
 IEnumerator Walk(RoomSession session,Keyboard keyboard,float x){
  double end=Time.realtimeSinceStartupAsDouble+8;var body=session.Player.GetComponent<Rigidbody2D>();
  int direction=body.position.x<x?1:-1;
  while((x-body.position.x)*direction>.06f&&Time.realtimeSinceStartupAsDouble<end){Keys(keyboard,direction);yield return null;}
  Keys(keyboard,0);yield return new WaitForSeconds(.08f);
  Check(Mathf.Abs(body.position.x-x)<.4f,"walk reaches authored point without teleport");
 }
 IEnumerator Hop(RoomSession session,Keyboard keyboard,Transform from,Transform to){
  var source=from.GetComponent<BoxCollider2D>().bounds;var target=to.GetComponent<BoxCollider2D>().bounds;
  int direction=source.center.x<target.center.x?1:-1;
  yield return Walk(session,keyboard,direction>0?source.max.x-.55f:source.min.x+.55f);
  var body=session.Player.GetComponent<Rigidbody2D>();Check(session.Player.GetComponent<PlayerMotor>().Grounded,"jump starts from ground");
  uint initialSequence=session.Player.JumpSequence;
  double end=Time.realtimeSinceStartupAsDouble+5;bool landed=false;bool airborne=false;
  while(Time.realtimeSinceStartupAsDouble<end){
   int move=Mathf.Abs(body.position.x-target.center.x)<.2f?0:(body.position.x<target.center.x?1:-1);
   Keys(keyboard,move,true);yield return null;
   airborne |= !session.Player.GetComponent<PlayerMotor>().Grounded;
   if(session.Respawning){results.Add($"MEASURE failed hop {from.name}->{to.name}: body={body.position}, velocity={body.linearVelocity}, jumpSequence={initialSequence}->{session.Player.JumpSequence}, pressed={session.Player.JumpPressedAt}, released={session.Player.JumpReleasedAt}, airborne={airborne}, fixed={Time.fixedUnscaledTimeAsDouble}");throw new Exception("Route jump entered Kill Zone: "+to.name);}
   if(session.Player.GetComponent<PlayerMotor>().Grounded&&Mathf.Abs(body.position.y-(target.max.y+.81f))<.2f&&body.position.x>target.min.x-.2f&&body.position.x<target.max.x+.2f){landed=true;break;}
  }
  Keys(keyboard,0);yield return null;yield return null;
  results.Add($"MEASURE: hop {from.name}->{to.name}; body={body.position}; target={target}; grounded={session.Player.GetComponent<PlayerMotor>().Grounded}; landed={landed}");
  Check(landed && airborne && session.Player.JumpSequence > initialSequence,"actual input trajectory lands on "+to.name);
 }
 IEnumerator TraversalChecks(){
  var session=RoomSession.Instance;
  if(session.CurrentRoomId!="A01"){Check(session.RequestTransition("A01","Assets/Scenes/A01.unity","Entry"),"prepare route at A01");yield return Wait(session);}
  var keyboard=InputSystem.AddDevice<Keyboard>();
  yield return new WaitForSeconds(.2f);var body=session.Player.GetComponent<Rigidbody2D>();
  // Measure in A01's open floor area, outside the overhead practice platforms.
  yield return Walk(session,keyboard,1.6f);
  float startX=body.position.x;Keys(keyboard,1,true);yield return null;
  double end=Time.realtimeSinceStartupAsDouble+2;bool airborne=false;
  while(Time.realtimeSinceStartupAsDouble<end){yield return null;if(!session.Player.GetComponent<PlayerMotor>().Grounded)airborne=true;if(airborne&&session.Player.GetComponent<PlayerMotor>().Grounded)break;}
  float range=body.position.x-startX;Keys(keyboard,0);yield return null;yield return null;
  results.Add($"MEASURE: open-floor full-jump range={range:F3}u; airborne={airborne}; initial gap cap={range*.8f:F3}u");
  Check(airborne&&range>3.8f,"measured same-height full jump horizontal range");
  double begin=Time.realtimeSinceStartupAsDouble;
  var itinerary=roundTrip?new[]{"A01","A02","A03","A04","A03","A02","A01"}:new[]{"A01","A02","A03","A04"};
  for(int roomIndex=0;roomIndex<itinerary.Length;roomIndex++){
   bool backward=roomIndex>=4;string id=itinerary[roomIndex];
   Check(session.CurrentRoomId==id,"route entered "+id);
   var room=FindFirstObjectByType<RoomDefinition>();var route=room.GetComponent<SliceRoute>();
   Check(route!=null&&route.landings.Length>0,"editable scene route exists");
   if(id!="A01")Check(room.GetComponentsInChildren<KillZone>().Length>=1,"room has visible Kill Zone");
   foreach(var exit in room.GetComponentsInChildren<RoomExit>())Check(exit.GetComponentsInChildren<SpriteRenderer>().Length>=3,"door silhouette marks exit "+exit.exitId);
   foreach(var landing in route.landings)Check(landing.GetComponent<BoxCollider2D>().bounds.size.x>=2,"required landing width >=2u");
   int first=backward?route.landings.Length-1:0;int last=backward?0:route.landings.Length-1;int delta=backward?-1:1;
   yield return Walk(session,keyboard,route.landings[first].position.x);
   for(int i=first;i!=last;i+=delta){
    var a=route.landings[i].GetComponent<BoxCollider2D>().bounds;var b=route.landings[i+delta].GetComponent<BoxCollider2D>().bounds;
    float gap=Mathf.Max(0,Mathf.Abs(a.center.x-b.center.x)-(a.size.x+b.size.x)/2);
    Check(gap<=range*.8f,"gap <=80% measured jump range");
    yield return Hop(session,keyboard,route.landings[i],route.landings[i+delta]);
   }
   results.Add($"MEASURE: completed {id} {(backward?"reverse":"forward")} route; cumulative={(Time.realtimeSinceStartupAsDouble-begin):F2}s");
   Debug.Log($"SLICE PROGRESS: {id} {(backward?"reverse":"forward")} complete after {Time.realtimeSinceStartupAsDouble-begin:F2}s");
   if(roomIndex+1<itinerary.Length){
    var exit=Array.Find(room.GetComponentsInChildren<RoomExit>(),x=>x.destinationRoomId==itinerary[roomIndex+1]);
    Check(exit!=null,"authored connection to "+itinerary[roomIndex+1]);
    // A04 ends at its far platform and returns through its entry via the same landings.
    if(id=="A04")for(int i=route.landings.Length-1;i>0;i--)yield return Hop(session,keyboard,route.landings[i],route.landings[i-1]);
    int direction=exit.transform.position.x>body.position.x?1:-1;
    end=Time.realtimeSinceStartupAsDouble+10;
    while((session.CurrentRoomId!=itinerary[roomIndex+1]||session.Transitioning)&&Time.realtimeSinceStartupAsDouble<end){Keys(keyboard,direction);yield return null;}
    Keys(keyboard,0);yield return null;yield return null;
    Check(session.CurrentRoomId==itinerary[roomIndex+1]&&!session.Transitioning,"physical door transition to "+itinerary[roomIndex+1]);
    Check(FindObjectsByType<PlayerInputReader>(FindObjectsSortMode.None).Length==1&&FindObjectsByType<RoomDefinition>(FindObjectsSortMode.None).Length==1,"single player and previous room cleaned");
   }
  }
  results.Add($"MEASURE: {(roundTrip?"round-trip":"forward")} active movement time={(Time.realtimeSinceStartupAsDouble-begin):F2}s; human first-play target=300~600s, human confirmation pending");
  InputSystem.RemoveDevice(keyboard);
 }
}
