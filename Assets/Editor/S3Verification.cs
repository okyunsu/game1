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
public static class S3Verification
{
    static S3Verification(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("S3.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("S3 verification").AddComponent<S3Runner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("S3.Run",false);EditorApplication.Exit(SessionState.GetInt("S3.Exit",1));}};}
    public static void Run(){string stem=Nightly.Tag;string path="Validation/"+stem+".txt";if(File.Exists(path))throw new IOException("Preserve evidence");SessionState.SetString("S3.Path",path);SessionState.SetBool("S3.Run",true);EditorSceneManager.OpenScene("Assets/Scenes/Test/DoubleJumpTest.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class S3Runner:MonoBehaviour
{
    readonly List<string> lines=new(),errors=new();Keyboard kb;
    PlayerInputReader Player=>RoomSession.Instance.Player;
    Rigidbody2D Body=>Player.GetComponent<Rigidbody2D>();
    PlayerMotor Motor=>Player.GetComponent<PlayerMotor>();
    void Check(bool ok,string m){if(!ok)throw new Exception(m);lines.Add("PASS: "+m);}
    void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
    IEnumerator Start(){DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;kb=InputSystem.AddDevice<Keyboard>();var stack=new Stack<IEnumerator>();stack.Push(DoubleJump());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add("FAIL: "+e);}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;InputSystem.RemoveDevice(kb);fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" "+Nightly.Tag+" Unity="+Application.unityVersion);lines.Add("Error/Exception/Assert "+errors.Count);File.WriteAllLines(SessionState.GetString("S3.Path",""),lines);SessionState.SetInt("S3.Exit",fail?1:0);EditorApplication.isPlaying=false;}
    void Place(Vector2 p){Player.ClearTransientInput();Player.transform.position=p;Body.position=p;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    IEnumerator Keys(params Key[] keys){InputSystem.QueueStateEvent(kb,new KeyboardState(keys));yield return null;yield return new WaitForFixedUpdate();}
    IEnumerator Settle(){yield return Keys();Place(new Vector2(10,1.81f));yield return new WaitForSeconds(.3f);Check(Motor.Grounded,"settled on floor");}
    float normalMax,doubleMax;
    IEnumerator Measure(bool twice,int fps){yield return Settle();int count=Motor.DoubleJumpsPerformed;yield return Keys(UnityEngine.InputSystem.Key.Z);float max=Body.position.y-.8f;double end=Time.realtimeSinceStartupAsDouble+.35;while(Time.realtimeSinceStartupAsDouble<end){max=Mathf.Max(max,Body.position.y-.8f);yield return null;}if(twice){yield return Keys();yield return Keys(UnityEngine.InputSystem.Key.Z);}end=Time.realtimeSinceStartupAsDouble+.6;while(Time.realtimeSinceStartupAsDouble<end){max=Mathf.Max(max,Body.position.y-.8f);yield return null;}yield return Keys();float height=max-1;lines.Add($"MEASURE fps={fps} double={twice} max feet={max:F3} height above floor={height:F3}u");if(twice){doubleMax=height;Check(Motor.DoubleJumpsPerformed==count+1,"one additional jump");}else{normalMax=height;Check(Motor.DoubleJumpsPerformed==count,"hold never triggers double jump");}}
    IEnumerator DoubleJump(){yield return new WaitForSeconds(.3f);ProgressSave.Enabled=false;QualitySettings.vSyncCount=0;Check(Motor.hasDoubleJump,"test player owned true");Check(!AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab").GetComponent<PlayerMotor>().hasDoubleJump,"production player ownership false");foreach(int fps in new[]{30,60,120}){Application.targetFrameRate=fps;yield return Measure(false,fps);yield return Measure(true,fps);Check(3.6f>=normalMax+.8f&&3.6f<=doubleMax-.5f,"G-J 3.6 within measured margins");}
        Application.targetFrameRate=60;yield return Settle();yield return Keys(UnityEngine.InputSystem.Key.Z);yield return new WaitForSeconds(.15f);yield return Keys();yield return Keys(UnityEngine.InputSystem.Key.Z);int used=Motor.DoubleJumpsPerformed;yield return Keys();yield return Keys(UnityEngine.InputSystem.Key.Z);Check(Motor.DoubleJumpsPerformed==used&&Body.linearVelocity.y<12,"third jump impossible");
        Player.GetComponent<PlayerDash>().hasDash=true;yield return Keys(UnityEngine.InputSystem.Key.C);yield return new WaitForSeconds(.2f);yield return Keys();yield return Keys(UnityEngine.InputSystem.Key.Z);Check(Motor.DoubleJumpsPerformed==used,"dash does not restore double jump");
        Place(new Vector2(10,7));yield return new WaitForSeconds(.15f);Check(!Motor.DoubleJumpAvailable,"transient input clear/airborne placement does not restore");yield return Settle();Place(new Vector2(10,7));yield return new WaitForSeconds(.15f);yield return Keys(UnityEngine.InputSystem.Key.Z);Check(Motor.DoubleJumpsPerformed==used+1,"walk-off beyond Coyote has one additional jump");
        Motor.hasDoubleJump=false;yield return Settle();Place(new Vector2(10,7));yield return new WaitForSeconds(.15f);used=Motor.DoubleJumpsPerformed;yield return Keys(UnityEngine.InputSystem.Key.Z);Check(Motor.DoubleJumpsPerformed==used,"unowned airborne jump ignored");yield return Keys();
        Check(ProgressSave.Write(new ProgressData{dash=true,doubleJump=true,checkpointId="CP-B01"}),"isolated save write");Check(ProgressSave.Continue(),"saved ownership load requested");Check(ProgressSave.PendingScene=="Assets/Scenes/B01.unity","Continue CP-B01 scene mapping");Destroy(RoomSession.Instance.gameObject);yield return null;SceneManager.LoadScene("B01");yield return new WaitForSeconds(.4f);Check(Motor.hasDoubleJump&&RoomSession.Instance.CheckpointId=="CP-B01","saved ownership applied to production player");
    }
}
