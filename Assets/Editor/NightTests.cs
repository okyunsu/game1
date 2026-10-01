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
public static class NightTests
{
    static NightTests(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("Night.Tests",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("Editor night verification").AddComponent<NightTestRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("Night.Tests",false);EditorApplication.Exit(SessionState.GetInt("Night.Exit",1));}};}
    public static void Run(){if(File.Exists(Nightly.Path("Verification")))throw new IOException("Preserve evidence");SessionState.SetString("Night.Tag",Nightly.Tag);SessionState.SetBool("Night.Tests",true);EditorSceneManager.OpenScene(Nightly.Tag.StartsWith("T0")?"Assets/Scenes/A01.unity":Nightly.Tag.StartsWith("T1")?"Assets/Scenes/Test/DashTest.unity":"Assets/Scenes/Test/CombatTest.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class NightTestRunner:MonoBehaviour
{
    readonly List<string> results=new();readonly List<string> errors=new();
    void Log(string msg,string stack,LogType type){if(type is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(msg+stack);}
    void Check(bool ok,string msg){if(!ok)throw new Exception(msg);results.Add("PASS: "+msg);}
    IEnumerator Start(){Application.logMessageReceived+=Log;var stack=new Stack<IEnumerator>();stack.Push(Nightly.Tag.StartsWith("T0")?T0():Nightly.Tag.StartsWith("T1")?T1():T2());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;results.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;fail|=errors.Count>0;results.AddRange(errors);results.Insert(0,(fail?"FAIL ":"PASS ")+SessionState.GetString("Night.Tag","")+" Unity="+Application.unityVersion);File.WriteAllLines(Nightly.Path("Verification"),results);SessionState.SetInt("Night.Exit",fail?1:0);EditorApplication.isPlaying=false;}
    IEnumerator T0(){yield return new WaitForSeconds(.3f);var session=RoomSession.Instance;var body=session.Player.GetComponent<Rigidbody2D>();
        foreach(string name in new[]{"Landing","Wall","Ceiling","KillZone"}){var asset=AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Level/{name}.prefab");Check(asset!=null&&asset.layer==6&&asset.GetComponent<SpriteRenderer>().drawMode==SpriteDrawMode.Simple&&!asset.GetComponent<BoxCollider2D>().autoTiling&&asset.GetComponent<BoxCollider2D>().size==Vector2.one,name+" Simple/1u collider/Ground; no autoTiling");}
        var landing=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab"));landing.transform.position=new Vector3(9,5,0);landing.transform.localScale=new Vector3(6,1,1);Physics2D.SyncTransforms();
        Check(Vector2.Distance(landing.GetComponent<BoxCollider2D>().bounds.size,new Vector2(6,1))<.01f,"Landing scale 6x1 => collider world 6x1u");
        body.position=new Vector2(9,8);session.Player.transform.position=body.position;body.linearVelocity=Vector2.zero;session.Player.ClearTransientInput();Physics2D.SyncTransforms();yield return new WaitForSeconds(1);
        Check(session.Player.GetComponent<PlayerMotor>().Grounded&&Mathf.Abs(body.position.y-6.31f)<.15f,"player physically lands on scaled Landing");Destroy(landing);
        var room=FindFirstObjectByType<RoomDefinition>();Check(room.GetComponent<SliceRoute>()==null&&room.GetComponent<RoomLabel>()!=null,"SliceRoute removed; runtime room label retained");
    }
    void Place(Vector2 at){var player=RoomSession.Instance.Player;var body=player.GetComponent<Rigidbody2D>();player.ClearTransientInput();player.transform.position=at;body.position=at;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    IEnumerator Key(Keyboard keyboard,params Key[] keys){InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));yield return null;}
    IEnumerator DashPress(Keyboard keyboard){yield return Key(keyboard,UnityEngine.InputSystem.Key.K);yield return new WaitForFixedUpdate();}
    IEnumerator T2(){
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        var keyboard=InputSystem.AddDevice<Keyboard>();yield return new WaitForSeconds(.25f);var session=RoomSession.Instance;var player=session.Player;var health=player.GetComponent<PlayerHealth>();var body=player.GetComponent<Rigidbody2D>();
        Check(health.HP==5&&session.CheckpointId=="CP-CombatTest","max HP 5 and test checkpoint active");
        var damage=FindFirstObjectByType<DamageBlock>();var duplicate=Instantiate(damage.gameObject);Place(damage.transform.position);yield return new WaitForFixedUpdate();yield return null;
        Check(health.HP==4&&health.Invincible,"simultaneous physical damage blocks deal only one HP");Check(!health.TakeDamage(1,Vector2.zero)&&health.HP==4,"invincibility rejects duplicate ordinary damage");Destroy(duplicate);
        Place(new Vector2(6,1.81f));yield return Key(keyboard,UnityEngine.InputSystem.Key.A);yield return new WaitForFixedUpdate();Check(health.KnockbackLocked,"knockback movement lock active for 0.12s");yield return Key(keyboard);yield return new WaitForSeconds(1.05f);
        Place(new Vector2(6,1.81f));Check(health.TakeDamage(1,new Vector2(5,1))&&body.linearVelocity.x==4&&body.linearVelocity.y==3,"left source knocks right at X4/Y3");yield return Key(keyboard,UnityEngine.InputSystem.Key.A);yield return new WaitForFixedUpdate();Check(body.linearVelocity.x>0,"opposing move does not override knockback lock");yield return Key(keyboard);yield return new WaitForSeconds(1.05f);Place(new Vector2(6,1.81f));Check(health.TakeDamage(1,new Vector2(7,1))&&body.linearVelocity.x==-4,"right source knocks left");
        health.RestoreMax();Place(new Vector2(6,1.81f));yield return new WaitForSeconds(.55f);yield return DashPress(keyboard);Check(player.GetComponent<PlayerDash>().IsDashing,"dash before damage setup");Check(health.TakeDamage(1,new Vector2(5,1))&&!player.GetComponent<PlayerDash>().IsDashing,"damage cancels dash");yield return Key(keyboard);
        health.RestoreMax();Place(new Vector2(6,1.81f));for(int i=0;i<5;i++){Check(health.TakeDamage(1,new Vector2(5,1)),"ordinary damage accepted "+i);if(i<4)yield return new WaitForSeconds(1.05f);}
        Check(health.HP==0&&session.Respawning&&player.InputLocked,"HP 0 uses S1-08 death flow");yield return new WaitForSecondsRealtime(.8f);Check(health.HP==5&&!session.Respawning&&session.CurrentRoomId=="CombatTest","respawn restores max HP in test checkpoint room");yield return new WaitForSeconds(.2f);Check(player.GetComponent<PlayerMotor>().Grounded,"respawn safe ground");
        Check(health.TakeDamage(1,new Vector2(2,1))&&health.Invincible,"KillZone invincibility setup");Place(FindFirstObjectByType<KillZone>().transform.position);yield return null;yield return new WaitForSecondsRealtime(.05f);Check(health.HP==0&&session.Respawning,"physical KillZone bypasses invincibility and sets HP 0");yield return new WaitForSecondsRealtime(.8f);Check(health.HP==5&&!session.Respawning,"KillZone return restores max HP");InputSystem.RemoveDevice(keyboard);
    }
    IEnumerator T1(){
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        var keyboard=InputSystem.AddDevice<Keyboard>();yield return new WaitForSeconds(.2f);var player=RoomSession.Instance.Player;var dash=player.GetComponent<PlayerDash>();var body=player.GetComponent<Rigidbody2D>();
        foreach(int fps in new[]{30,60,120}){QualitySettings.vSyncCount=0;Application.targetFrameRate=fps;
            yield return Key(keyboard);Place(new Vector2(5,1.81f));yield return new WaitForSeconds(.55f);float start=body.position.x;yield return DashPress(keyboard);Check(dash.IsDashing,$"{fps}fps dash starts");uint sequence=player.JumpSequence;yield return Key(keyboard,UnityEngine.InputSystem.Key.K,UnityEngine.InputSystem.Key.Space);Check(player.JumpSequence==sequence,$"{fps}fps jump ignored during dash");yield return Key(keyboard);yield return new WaitForSeconds(.18f);float distance=body.position.x-start;results.Add($"MEASURE {fps}fps dash distance={distance:F3}u");Check(distance>2.7f&&distance<3.2f,$"{fps}fps dash distance ~2.9u");yield return DashPress(keyboard);Check(!dash.IsDashing,$"{fps}fps cooldown prevents immediate reuse");yield return Key(keyboard);yield return new WaitForSeconds(.5f);
            Place(new Vector2(27,1.81f));yield return new WaitForSeconds(.1f);yield return DashPress(keyboard);yield return Key(keyboard);yield return new WaitForSeconds(.2f);Check(!dash.IsDashing&&body.position.x<28.1f,$"{fps}fps wall cancels dash without penetration");
            Place(new Vector2(5,1.81f));yield return new WaitForSeconds(.55f);Place(new Vector2(40,4));yield return new WaitForFixedUpdate();yield return DashPress(keyboard);Check(dash.IsDashing,$"{fps}fps airborne dash starts");yield return Key(keyboard);yield return new WaitForSeconds(.2f);Check(body.linearVelocity.y<0,$"{fps}fps gravity resumes after dash");yield return new WaitForSeconds(.3f);yield return DashPress(keyboard);Check(!dash.IsDashing,$"{fps}fps air reuse blocked after cooldown");yield return Key(keyboard);
        }
        Place(new Vector2(5,1.81f));yield return new WaitForSeconds(.55f);dash.hasDash=false;yield return DashPress(keyboard);Check(!dash.IsDashing,"ability false ignores Dash");yield return Key(keyboard);dash.hasDash=true;
        var state=GameState.GetOrCreate();foreach(string mode in new[]{"Pause","Transition","Death"}){Place(new Vector2(5,1.81f));yield return new WaitForSeconds(.55f);yield return DashPress(keyboard);Check(dash.IsDashing,mode+" cancellation setup");if(mode=="Pause")player.SetPaused(true);else if(mode=="Transition")state.SetTransitioning(true);else state.SetRespawning(true);Check(!dash.IsDashing&&body.linearVelocity.y==0,"dash immediately cancelled by "+mode);if(mode=="Pause")player.SetPaused(false);else if(mode=="Transition")state.SetTransitioning(false);else state.SetRespawning(false);yield return Key(keyboard);Place(new Vector2(40,4));yield return new WaitForFixedUpdate();Check(body.linearVelocity.y<0,"gravity restored after "+mode);}
        var gamepad=InputSystem.AddDevice<Gamepad>();Place(new Vector2(5,1.81f));yield return new WaitForSeconds(.55f);InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.East));yield return null;yield return new WaitForFixedUpdate();Check(dash.IsDashing,"gamepad B starts Gameplay dash");player.SetPaused(true);InputSystem.QueueStateEvent(gamepad,new GamepadState());yield return null;InputSystem.QueueStateEvent(gamepad,new GamepadState().WithButton(GamepadButton.East));yield return null;Check(!player.Paused,"UI B still cancels Pause");InputSystem.RemoveDevice(gamepad);InputSystem.RemoveDevice(keyboard);
    }
}
