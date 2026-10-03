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
public static class T2LockCheck
{
 public static string Output=>"Validation/"+Nightly.Tag+"-T2v2.txt";
 static T2LockCheck(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("T2v2.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("T2 direct clock verification").AddComponent<T2LockRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("T2v2.Run",false);EditorApplication.Exit(SessionState.GetInt("T2v2.Exit",1));}};}
 public static void Run(){if(File.Exists(Output))throw new IOException("Preserve evidence");SessionState.SetBool("T2v2.Run",true);EditorSceneManager.OpenScene("Assets/Scenes/Test/CombatTest.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class T2LockRunner:MonoBehaviour
{
 readonly List<string> lines=new(),errors=new();Keyboard keyboard;
 void Check(bool ok,string text){if(!ok)throw new Exception(text);lines.Add("PASS: "+text);}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 IEnumerator Start(){Application.logMessageReceived+=Log;QualitySettings.vSyncCount=0;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;keyboard=InputSystem.AddDevice<Keyboard>();var stack=new Stack<IEnumerator>();stack.Push(Tests());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;InputSystem.RemoveDevice(keyboard);fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" T2v2 "+Nightly.Tag);lines.Add("Error/Exception/Assert="+errors.Count);File.WriteAllLines(T2LockCheck.Output,lines);SessionState.SetInt("T2v2.Exit",fail?1:0);EditorApplication.isPlaying=false;}
 IEnumerator Tests(){yield return new WaitForSeconds(.3f);var player=RoomSession.Instance.Player;var health=player.GetComponent<PlayerHealth>();var body=player.GetComponent<Rigidbody2D>();foreach(var enemy in FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None))enemy.enabled=false;
 foreach(var checkpoint in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))checkpoint.enabled=false; health.Restored+=()=>lines.Add("MEASURE health RestoreMax callback time="+Time.timeAsDouble); foreach(int fps in new[]{30,60,120}){Application.targetFrameRate=fps;yield return new WaitForSeconds(.3f);health.RestoreMax();player.ClearTransientInput();player.transform.position=new Vector3(8,1.81f,0);body.position=player.transform.position;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();Check(!health.Invincible,"no invincibility before direct hit "+fps);double started=Time.timeAsDouble;Check(health.TakeDamage(1,new Vector2(5,1))&&health.KnockbackLocked&&body.linearVelocity.x>0,"direct hit immediately locks/right knockback "+fps);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftArrow));double previous=started,maxFrame=0;int opposingTicks=0;
 while(health.KnockbackLocked){yield return null;double now=Time.timeAsDouble;maxFrame=Math.Max(maxFrame,now-previous);previous=now;if(health.KnockbackLocked){Check(body.linearVelocity.x>0,"opposite input preserves x sign while locked "+fps);opposingTicks++;}}
 double elapsed=Time.timeAsDouble-started;lines.Add($"MEASURE fps={fps} unlock={elapsed:F6}s maxObservedFrame={maxFrame:F6}s configured={health.tuning.knockbackLock:F6}s opposingFrames={opposingTicks}");Check(opposingTicks>0&&elapsed>=health.tuning.knockbackLock-0.000001&&elapsed<=health.tuning.knockbackLock+maxFrame+0.000001,"unlock within configured lock plus one observed frame "+fps);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return new WaitForSeconds(1.1f);}
 }
}
