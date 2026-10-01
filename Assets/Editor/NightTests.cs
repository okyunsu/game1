using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class NightTests
{
    static NightTests(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("Night.Tests",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("Editor night verification").AddComponent<NightTestRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("Night.Tests",false);EditorApplication.Exit(SessionState.GetInt("Night.Exit",1));}};}
    public static void Run(){if(File.Exists(Nightly.Path("Verification")))throw new IOException("Preserve evidence");SessionState.SetString("Night.Tag",Nightly.Tag);SessionState.SetBool("Night.Tests",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class NightTestRunner:MonoBehaviour
{
    readonly List<string> results=new();readonly List<string> errors=new();
    void Log(string msg,string stack,LogType type){if(type is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(msg+stack);}
    void Check(bool ok,string msg){if(!ok)throw new Exception(msg);results.Add("PASS: "+msg);}
    IEnumerator Start(){Application.logMessageReceived+=Log;var stack=new Stack<IEnumerator>();stack.Push(T0());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;results.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;fail|=errors.Count>0;results.AddRange(errors);results.Insert(0,(fail?"FAIL ":"PASS ")+SessionState.GetString("Night.Tag","")+" Unity="+Application.unityVersion);File.WriteAllLines(Nightly.Path("Verification"),results);SessionState.SetInt("Night.Exit",fail?1:0);EditorApplication.isPlaying=false;}
    IEnumerator T0(){yield return new WaitForSeconds(.3f);var session=RoomSession.Instance;var body=session.Player.GetComponent<Rigidbody2D>();
        foreach(string name in new[]{"Landing","Wall","Ceiling","KillZone"}){var asset=AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Level/{name}.prefab");Check(asset!=null&&asset.layer==6&&asset.GetComponent<SpriteRenderer>().drawMode==SpriteDrawMode.Simple&&!asset.GetComponent<BoxCollider2D>().autoTiling&&asset.GetComponent<BoxCollider2D>().size==Vector2.one,name+" Simple/1u collider/Ground; no autoTiling");}
        var landing=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab"));landing.transform.position=new Vector3(9,5,0);landing.transform.localScale=new Vector3(6,1,1);Physics2D.SyncTransforms();
        Check(Vector2.Distance(landing.GetComponent<BoxCollider2D>().bounds.size,new Vector2(6,1))<.01f,"Landing scale 6x1 => collider world 6x1u");
        body.position=new Vector2(9,8);session.Player.transform.position=body.position;body.linearVelocity=Vector2.zero;session.Player.ClearTransientInput();Physics2D.SyncTransforms();yield return new WaitForSeconds(1);
        Check(session.Player.GetComponent<PlayerMotor>().Grounded&&Mathf.Abs(body.position.y-6.31f)<.15f,"player physically lands on scaled Landing");Destroy(landing);
        var room=FindFirstObjectByType<RoomDefinition>();Check(room.GetComponent<SliceRoute>()==null&&room.GetComponent<RoomLabel>()!=null,"SliceRoute removed; runtime room label retained");
    }
}
