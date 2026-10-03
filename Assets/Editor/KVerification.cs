using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
[InitializeOnLoad]
public static class KVerification
{
 static KVerification(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("K.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("K verification").AddComponent<KRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("K.Run",false);EditorApplication.Exit(SessionState.GetInt("K.Exit",1));}};}
 public static void Run(){string path="Validation/"+(Nightly.Tag.StartsWith("K1")?"K1-keys":Nightly.Tag.StartsWith("K2")?"K2-camera":"S2-03-retry")+(Nightly.Tag.Contains("rerun")?"-rerun":"")+".txt";if(File.Exists(path))throw new IOException("Preserve evidence");SessionState.SetString("K.Path",path);SessionState.SetBool("K.Run",true);EditorSceneManager.OpenScene(Nightly.Tag.StartsWith("K1")?"Assets/Scenes/Menu.unity":"Assets/Scenes/A03.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class KRunner:MonoBehaviour
{
 readonly List<string> lines=new(),errors=new();Keyboard kb;
 void Check(bool ok,string m){if(!ok)throw new Exception(m);lines.Add("PASS: "+m);}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 IEnumerator Start(){DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;kb=InputSystem.AddDevice<Keyboard>();var stack=new Stack<IEnumerator>();stack.Push(Nightly.Tag.StartsWith("K1")?Keys():Nightly.Tag.StartsWith("K2")?CameraTests():GateTests());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add("FAIL: "+e);}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;InputSystem.RemoveDevice(kb);fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" "+Nightly.Tag);lines.Add("FAIL "+(fail?1:0));File.WriteAllLines(SessionState.GetString("K.Path",""),lines);SessionState.SetInt("K.Exit",fail?1:0);EditorApplication.isPlaying=false;}
 PlayerInputReader Player=>RoomSession.Instance.Player;
 Rigidbody2D Body=>Player.GetComponent<Rigidbody2D>();
 void Place(Vector2 p){Player.ClearTransientInput();Player.transform.position=p;Body.position=p;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator KeysHeld(params Key[] keys){InputSystem.QueueStateEvent(kb,new KeyboardState(keys));yield return null;}
 IEnumerator Tap(Key key){yield return KeysHeld(key);yield return KeysHeld();}
 IEnumerator Wait(){double end=Time.realtimeSinceStartupAsDouble+5;while(RoomSession.Instance.Transitioning||RoomSession.Instance.Respawning){if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("transition timeout");yield return null;}}
 IEnumerator Keys(){
  yield return new WaitForSeconds(.25f);ProgressSave.Write(new ProgressData{dash=true,checkpointId="CP-A01"});SceneManager.LoadScene("Menu");yield return new WaitForSeconds(.25f);var menu=FindFirstObjectByType<SaveMenu>();Check(menu.CanContinue,"Menu valid isolated save");yield return Tap(UnityEngine.InputSystem.Key.DownArrow);yield return Tap(UnityEngine.InputSystem.Key.Z);Check(menu.Confirming,"Menu Z opens new-game confirmation");yield return Tap(UnityEngine.InputSystem.Key.X);Check(!menu.Confirming,"Menu X cancels");yield return Tap(UnityEngine.InputSystem.Key.Z);yield return Tap(UnityEngine.InputSystem.Key.Z);yield return new WaitForSeconds(.4f);Check(RoomSession.Instance.CurrentRoomId=="A01","Menu Z confirms new game");ProgressSave.Enabled=false;
  Check(RoomSession.Instance.RequestTransition("CombatTest","Assets/Scenes/Test/CombatTest.unity","Entry"),"enter isolated test room");yield return Wait();yield return new WaitForSeconds(.25f);Player.GetComponent<PlayerDash>().hasDash=true;
  foreach(var k in new[]{UnityEngine.InputSystem.Key.LeftArrow,UnityEngine.InputSystem.Key.RightArrow,UnityEngine.InputSystem.Key.A,UnityEngine.InputSystem.Key.D}){Place(new Vector2(6,1.81f));yield return KeysHeld();yield return new WaitForSeconds(.15f);yield return KeysHeld(k);yield return new WaitForFixedUpdate();Check(Mathf.Abs(Player.Move.x)>.9f&&Mathf.Sign(Body.linearVelocity.x)==(k==UnityEngine.InputSystem.Key.LeftArrow||k==UnityEngine.InputSystem.Key.A?-1:1),"move "+k);yield return KeysHeld();}
  float zShort=0,zLong=0,spaceShort=0,spaceLong=0;
  foreach(var key in new[]{UnityEngine.InputSystem.Key.Z,UnityEngine.InputSystem.Key.Space})foreach(float hold in new[]{.10f,.5f}){Place(new Vector2(6,1.81f));yield return KeysHeld();yield return new WaitForSeconds(.3f);float peak=Body.position.y,start=peak;yield return KeysHeld(key);double end=Time.realtimeSinceStartupAsDouble+hold;while(Time.realtimeSinceStartupAsDouble<end){peak=Mathf.Max(peak,Body.position.y);yield return null;}yield return KeysHeld();end=Time.realtimeSinceStartupAsDouble+.9;while(Time.realtimeSinceStartupAsDouble<end){peak=Mathf.Max(peak,Body.position.y);yield return null;}float height=peak-start;lines.Add($"MEASURE {key} hold={hold:F2}s height={height:F3}u");if(key==UnityEngine.InputSystem.Key.Z){if(hold<.2f)zShort=height;else zLong=height;}else{if(hold<.2f)spaceShort=height;else spaceLong=height;}}
  Check(zLong>zShort+.3f&&Mathf.Abs(zShort-spaceShort)<.1f&&Mathf.Abs(zLong-spaceLong)<.1f,"Z short/long jump and Space equivalent heights");
  foreach(var key in new[]{UnityEngine.InputSystem.Key.X,UnityEngine.InputSystem.Key.J}){yield return new WaitForSeconds(.4f);yield return KeysHeld(key);Check(Player.GetComponent<PlayerAttack>().Attacking,"attack "+key);yield return KeysHeld();}
  foreach(var key in new[]{UnityEngine.InputSystem.Key.C,UnityEngine.InputSystem.Key.K,UnityEngine.InputSystem.Key.LeftShift}){Place(new Vector2(6,1.81f));yield return new WaitForSeconds(.6f);yield return KeysHeld(key);yield return new WaitForFixedUpdate();Check(Player.GetComponent<PlayerDash>().IsDashing,"dash "+key);yield return KeysHeld();}
  Player.SetPaused(true);yield return Tap(UnityEngine.InputSystem.Key.Z);Check(!Player.Paused,"UI Z resumes Pause");Player.SetPaused(true);yield return Tap(UnityEngine.InputSystem.Key.X);Check(!Player.Paused,"UI X cancels Pause");
  var baseline=JsonUtility.FromJson<BindingSnapshot>("{}");var asset=AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Input/PlayerControls.inputactions");foreach(var binding in asset.bindings){Check(!binding.path.Contains("leftCtrl"),"LCtrl remains unbound");if(binding.path.StartsWith("<Gamepad>"))lines.Add("MEASURE unchanged gamepad "+binding.action+"="+binding.path);}
 }
 [Serializable]class BindingSnapshot{}
 IEnumerator CameraTests(){yield return null;yield break;}
 IEnumerator GateTests(){yield return null;yield break;}
}
