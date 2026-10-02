using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
[InitializeOnLoad]
public static class S2Tests
{
 static S2Tests(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("S2.Tests",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("S2 persistent verification").AddComponent<S2Runner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("S2.Tests",false);EditorApplication.Exit(SessionState.GetInt("S2.Exit",1));}};}
 public static void Run(){string path=Nightly.Tag.Contains("rerun")?"Validation/S2-01-integration-rerun.txt":"Validation/S2-01-integration.txt";if(File.Exists(path))throw new IOException("Preserve evidence");SessionState.SetString("S2.Path",path);SessionState.SetBool("S2.Tests",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class S2Runner:MonoBehaviour
{
 readonly List<string> lines=new(),errors=new();
 void Check(bool ok,string msg){if(!ok)throw new Exception(msg);lines.Add("PASS: "+msg);}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 IEnumerator Start(){DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;var stack=new Stack<IEnumerator>();stack.Push(Integration());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add("FAIL: "+e);}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" S2-01 integration");lines.Add("FAIL "+(fail?1:0));File.WriteAllLines(SessionState.GetString("S2.Path",""),lines);SessionState.SetInt("S2.Exit",fail?1:0);EditorApplication.isPlaying=false;}
 void Place(Vector2 p){var player=RoomSession.Instance.Player;var body=player.GetComponent<Rigidbody2D>();player.ClearTransientInput();player.transform.position=p;body.position=p;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Key(Keyboard kb,params Key[] keys){InputSystem.QueueStateEvent(kb,new KeyboardState(keys));yield return null;}
 IEnumerator Wait(){double until=Time.realtimeSinceStartupAsDouble+5;while(RoomSession.Instance.Transitioning||RoomSession.Instance.Respawning){if(Time.realtimeSinceStartupAsDouble>until)throw new Exception("Room operation timeout");yield return null;}}
 IEnumerator Enter(string id){Check(RoomSession.Instance.RequestTransition(id,$"Assets/Scenes/{id}.unity","Entry"),"transition "+id);yield return Wait();yield return new WaitForSeconds(.15f);}
 void EnemyPlace(EnemyPatrol e,Vector2 p){e.transform.position=p;e.GetComponent<Rigidbody2D>().position=p;e.GetComponent<Rigidbody2D>().linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Attack(Keyboard kb){yield return Key(kb,UnityEngine.InputSystem.Key.D);yield return new WaitForFixedUpdate();yield return Key(kb,UnityEngine.InputSystem.Key.J);yield return Key(kb);yield return new WaitForSeconds(.21f);}
 IEnumerator Integration(){
  InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
  var kb=InputSystem.AddDevice<Keyboard>();var pad=InputSystem.AddDevice<Gamepad>();yield return new WaitForSeconds(.25f);var session=RoomSession.Instance;var player=session.Player;var hp=player.GetComponent<PlayerHealth>();var dash=player.GetComponent<PlayerDash>();var attack=player.GetComponent<PlayerAttack>();
  Check(hp!=null&&dash!=null&&attack!=null&&hp.HP==5&&!dash.hasDash,"base player health/attack; Dash false");Check(hp.TakeDamage(1,new Vector2(2,1)),"HP persistence setup");
  foreach(string room in new[]{"A01","A02","A03","A04"}){
   if(session.CurrentRoomId!=room)yield return Enter(room);Check(hp.HP==4,"HP persists in "+room);Check(player.GetComponents<PlayerDash>().Length==1&&player.GetComponents<PlayerHealth>().Length==1&&player.GetComponents<PlayerAttack>().Length==1,"single integrated components in "+room);
   yield return new WaitForSeconds(1.05f);yield return Key(kb,UnityEngine.InputSystem.Key.J);Check(attack.Attacking,room+" J attack");yield return Key(kb);yield return new WaitForSeconds(.4f);InputSystem.QueueStateEvent(pad,new GamepadState().WithButton(GamepadButton.West));yield return null;Check(attack.Attacking,room+" gamepad X attack");InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;yield return Key(kb,UnityEngine.InputSystem.Key.K);yield return new WaitForFixedUpdate();Check(!dash.IsDashing,room+" unowned Dash ignored");yield return Key(kb);
   if(room=="A03"){var cp=FindFirstObjectByType<Checkpoint>();Place(cp.transform.position);yield return new WaitForFixedUpdate();yield return null;Check(session.CheckpointId=="CP-A03","CP-A03 contact");}
  }
  hp.RestoreMax();yield return null;var spawner=FindFirstObjectByType<EnemyRespawn>();var enemy=spawner.live;var body=enemy.GetComponent<Rigidbody2D>();Place(new Vector2(6,1.81f));float low=18,high=18;int turns=enemy.Turns;float end=Time.time+9;
  while(Time.time<end){low=Mathf.Min(low,body.position.x);high=Mathf.Max(high,body.position.x);yield return null;}lines.Add($"MEASURE A04 patrol x={low:F3}..{high:F3}, y={body.position.y:F3}, turns={enemy.Turns-turns}");Check(enemy.Turns-turns>=2&&body.position.y>3.4f,"A04 patrol reverses without falling from Arena");
  enemy.enabled=false;EnemyPlace(enemy,new Vector2(20,3.51f));Place(new Vector2(18.75f,3.81f));yield return Attack(kb);Check(enemy.HP==1,"A04 actual attack one HP");yield return new WaitForSeconds(.4f);Place(new Vector2(body.position.x-1.25f,3.81f));yield return Attack(kb);Check(enemy==null,"A04 second actual attack removes E1");
  hp.RestoreMax();yield return null;enemy=spawner.live;enemy.enabled=false;EnemyPlace(enemy,new Vector2(20,3.51f));Place(new Vector2(19.15f,3.81f));yield return new WaitForSeconds(.1f);Check(hp.HP==4&&hp.Invincible,"A04 physical contact damage 1/invincibility");Place(new Vector2(19.15f,3.81f));yield return new WaitForSeconds(.1f);Check(hp.HP==4,"contact duplicates blocked");
  Place(new Vector2(6,1.81f));hp.RestoreMax();for(int i=0;i<5;i++){Check(hp.TakeDamage(1,new Vector2(2,1)),"damage "+i);if(i<4)yield return new WaitForSeconds(1.05f);}Check(hp.HP==0&&session.Respawning,"HP zero triggers death");yield return Wait();Check(hp.HP==5&&session.CurrentRoomId=="A03","HP restored at last checkpoint");
  yield return Enter("A02");Check(hp.TakeDamage(1,new Vector2(2,1)),"KillZone invincibility setup");Place(FindFirstObjectByType<KillZone>().transform.position);yield return new WaitForFixedUpdate();yield return null;Check(session.Respawning&&hp.HP==0,"A02 KillZone bypasses invincibility");yield return Wait();Check(hp.HP==5,"KillZone return max HP");
  yield return Enter("A04");enemy=FindFirstObjectByType<EnemyPatrol>();Check(enemy!=null&&enemy.HP==2&&FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==1,"A04 reentry resets one E1");InputSystem.RemoveDevice(kb);InputSystem.RemoveDevice(pad);
 }
}
