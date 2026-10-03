using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
[InitializeOnLoad]public static class BossRecoveryCheck
{
 static BossRecoveryCheck(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("Recovery.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("Boss recovery verification").AddComponent<BossRecoveryRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("Recovery.Run",false);EditorApplication.Exit(SessionState.GetInt("Recovery.Exit",1));}};}
 public static void Run(){if(File.Exists("Validation/F2R-boss-recovery.txt"))throw new IOException("Preserve evidence");SessionState.SetBool("Recovery.Run",true);EditorSceneManager.OpenScene("Assets/Scenes/C06.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class BossRecoveryRunner:MonoBehaviour
{
 readonly List<string> lines=new(),errors=new();Keyboard kb;
 void Check(bool ok,string s){if(!ok)throw new Exception(s);lines.Add("PASS: "+s);}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 IEnumerator Start(){Application.logMessageReceived+=Log;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;QualitySettings.vSyncCount=0;Application.targetFrameRate=60;kb=InputSystem.AddDevice<Keyboard>();var stack=new Stack<IEnumerator>();stack.Push(Tests());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;InputSystem.RemoveDevice(kb);fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,fail?"FAIL boss recovery":"PASS boss recovery");lines.Add("Error/Exception/Assert="+errors.Count);File.WriteAllLines("Validation/F2R-boss-recovery.txt",lines);SessionState.SetInt("Recovery.Exit",fail?1:0);EditorApplication.isPlaying=false;}
 void Place(Vector2 p){var player=RoomSession.Instance.Player;player.ClearTransientInput();player.transform.position=p;var body=player.GetComponent<Rigidbody2D>();body.position=p;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Tests(){yield return new WaitForSeconds(.3f);Place(new Vector2(8,1.81f));var boss=FindFirstObjectByType<BossEncounter>();double end=Time.timeAsDouble+3;while(boss.State!=BossEncounter.Phase.MeleeRecovery&&Time.timeAsDouble<end)yield return null;Check(boss.State==BossEncounter.Phase.MeleeRecovery,"recovery begins");InputSystem.QueueStateEvent(kb,new KeyboardState(Key.RightArrow));yield return null;yield return new WaitForFixedUpdate();InputSystem.QueueStateEvent(kb,new KeyboardState());yield return null;Place(new Vector2(15.75f,1.81f));int hits=boss.Hits,calls=boss.GetComponent<HitFeedback>().Calls;InputSystem.QueueStateEvent(kb,new KeyboardState(Key.X));yield return null;InputSystem.QueueStateEvent(kb,new KeyboardState());yield return new WaitForSeconds(.2f);Check(boss.Hits==hits+1&&boss.GetComponent<HitFeedback>().Calls==calls+1,"actual X hits in recovery; display called once");Check(boss.HP==19&&boss.tuning.recovery==1.2f,"HP19 and unchanged recovery1.2s");}
}
