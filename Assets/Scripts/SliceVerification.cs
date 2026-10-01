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
 public Action<bool> finished;
 readonly List<string> results=new(); readonly List<string> errors=new();
 void Log(string msg,string stack,LogType kind){if(kind is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(msg+stack);}
 void Check(bool ok,string label){if(!ok)throw new Exception(label);results.Add("PASS: "+label);}
 public IEnumerator Start(){
  DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;
  QualitySettings.vSyncCount=0;Application.targetFrameRate=60;
  var stack=new Stack<IEnumerator>();stack.Push(CheckpointChecks());bool fail=false;
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
}
