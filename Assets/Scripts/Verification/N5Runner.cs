using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
// Shared mechanics for explicitly opted-in verification tools; never bootstraps itself.
public abstract class N5Runner:MonoBehaviour
{
 protected readonly List<string> lines=new(),errors=new();protected Keyboard keyboard;
 protected virtual string Evidence=>N5Args.Value("-verify-output","Validation/N5-tool.txt");
 protected virtual bool PreserveFocusBehavior=>false;
 protected PlayerInputReader Player=>RoomSession.Instance?.Player;
 protected Rigidbody2D Body=>Player.GetComponent<Rigidbody2D>();
 protected PlayerMotor Motor=>Player.GetComponent<PlayerMotor>();
 protected PlayerHealth Health=>Player.GetComponent<PlayerHealth>();
 protected abstract IEnumerator Tests();
 protected virtual void Cleanup(){}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 protected IEnumerator Start(){DontDestroyOnLoad(gameObject);Application.runInBackground=true;Application.logMessageReceived+=Log;QualitySettings.vSyncCount=0;Application.targetFrameRate=N5Args.Int("-verify-fps",60);if(!PreserveFocusBehavior)InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
 #if UNITY_EDITOR
 if(!PreserveFocusBehavior)InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
 #endif
 keyboard=InputSystem.AddDevice<Keyboard>();var stack=new Stack<IEnumerator>();stack.Push(Tests());bool failed=false;
 while(stack.Count>0&&!failed){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){failed=true;lines.Add("FAIL "+e);}if(next is IEnumerator child)stack.Push(child);else if(!failed)yield return next;}
 Cleanup();PlaytestLogger.Instance?.Finish();InputSystem.RemoveDevice(keyboard);Application.logMessageReceived-=Log;failed|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(failed?"FAIL ":"PASS ")+GetType().Name+" actual="+(Application.isEditor?"Editor":"exe"));lines.Add("Error/Exception/Assert="+errors.Count);string output=Path.GetFullPath(Evidence);Directory.CreateDirectory(Path.GetDirectoryName(output));if(File.Exists(output))throw new IOException("Preserve evidence "+output);File.WriteAllLines(output,lines);
 #if UNITY_EDITOR
 if(Application.isEditor){UnityEditor.SessionState.SetInt("N5.Exit",failed?1:0);UnityEditor.EditorApplication.isPlaying=false;yield break;}
 #endif
 Application.Quit(failed?1:0);
 }
 protected void Check(bool ok,string message){if(!ok)throw new Exception(message);lines.Add("PASS "+message);}
 protected IEnumerator Until(Func<bool> test,string reason,float seconds=12){double end=Time.realtimeSinceStartupAsDouble+seconds;while(!test()){if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("Timeout "+reason);yield return null;}}
 protected IEnumerator Keys(params Key[] keys){InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));yield return null;yield return new WaitForSecondsRealtime(.03f);}
 protected IEnumerator Tap(Key key){yield return Keys(key);yield return Keys();}
 protected IEnumerator Ready(){yield return Until(()=>Player!=null&&!RoomSession.Instance.Transitioning&&!RoomSession.Instance.Respawning,"room ready");yield return null;yield return null;}
 protected IEnumerator ContinueGame(){Check(N5Args.Has("-save-path"),"isolated explicit save path required before writing");yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"title");yield return new WaitForSecondsRealtime(.2f);Check(FindFirstObjectByType<SaveMenu>().CanContinue,"fixture enables Continue");yield return Tap(Key.Z);yield return Ready();}
 protected IEnumerator NewGame(){Check(N5Args.Has("-save-path"),"isolated explicit save path required before writing");yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"title");yield return new WaitForSecondsRealtime(.2f);if(FindFirstObjectByType<SaveMenu>().CanContinue)yield return Tap(Key.DownArrow);yield return Tap(Key.Z);if(FindFirstObjectByType<SaveMenu>()?.Confirming==true)yield return Tap(Key.Z);yield return Ready();}
 protected IEnumerator Title(){yield return Tap(Key.Escape);Check(Player.Paused,"Pause by virtual keyboard");yield return Tap(Key.DownArrow);yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"return title");yield return new WaitForSecondsRealtime(.2f);}
 protected IEnumerator WalkTo(Func<bool> test,Key direction,string reason,float seconds=12){double end=Time.realtimeSinceStartupAsDouble+seconds;while(!test()){InputSystem.QueueStateEvent(keyboard,new KeyboardState(direction));if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("Timeout "+reason+" room="+RoomSession.Instance?.CurrentRoomId+" pos="+Body.position+" move="+Player.Move+" locked="+Player.InputLocked+" hp="+Health.HP);yield return null;}yield return Keys();}
 // Only T-D section setup and T-E exit/KillZone setup may call this explicit placement helper.
 protected void PlaceForAuthorizedSetup(Vector2 p){Body.position=p;Player.transform.position=p;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 protected IEnumerator PhysicalDoor(string destination){var exit=Array.Find(FindObjectsByType<RoomExit>(FindObjectsSortMode.None),e=>e.destinationRoomId==destination);Check(exit!=null&&exit.enabled,"enabled physical exit "+destination);yield return Keys();PlaceForAuthorizedSetup(exit.GetComponent<BoxCollider2D>().bounds.center);yield return new WaitForFixedUpdate();yield return Until(()=>RoomSession.Instance.CurrentRoomId==destination&&!RoomSession.Instance.Transitioning,"door "+destination);yield return Ready();}
}
public static class N5Args
{
 public static bool Has(string flag)=>Array.IndexOf(Environment.GetCommandLineArgs(),flag)>=0;
 public static string Value(string flag,string fallback){var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,flag);return i>=0&&i+1<a.Length?a[i+1]:fallback;}
 public static int Int(string flag,int fallback)=>int.TryParse(Value(flag,""),out int n)?n:fallback;
 public static void Start<T>(string flag)where T:MonoBehaviour{if(!Has(flag))return;var go=new GameObject(typeof(T).Name+" opt-in");UnityEngine.Object.DontDestroyOnLoad(go);go.AddComponent<T>();}
}
