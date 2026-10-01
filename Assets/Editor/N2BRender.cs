using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class N2BRender
{
 static N2BRender(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("N2B.Render",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("Slice static camera verification").AddComponent<N2BRenderRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("N2B.Render",false);EditorApplication.Exit(SessionState.GetInt("N2B.RenderExit",1));}};}
 public static void Run(){if(File.Exists("Validation/S1-09-camera-02.txt"))throw new IOException("Preserve evidence");SessionState.SetBool("N2B.Render",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class N2BRenderRunner:MonoBehaviour
{
 IEnumerator Start(){
  DontDestroyOnLoad(gameObject);yield return new WaitForSeconds(.2f);
  bool pass=true;var lines=new System.Collections.Generic.List<string>();
  for(int i=1;i<=4;i++){
   var session=RoomSession.Instance;
   if(i>1){session.RequestTransition($"A0{i}",$"Assets/Scenes/A0{i}.unity","Entry");while(session.Transitioning)yield return null;}
   var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var route=room.GetComponent<SliceRoute>();
   var rig=UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
   var point=i==3?new Vector3(7,1.81f,0):route.landings[^1].position+new Vector3(2,1.31f,0);
   session.Player.SetPaused(true);var body=session.Player.GetComponent<Rigidbody2D>();session.Player.transform.position=point;body.position=point;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();
   var cam=rig.OutputCamera;var rt=new RenderTexture(1280,720,24);rt.Create();cam.targetTexture=rt;cam.aspect=16f/9;rig.BindAndSnap(session.Player);
   float halfY=cam.orthographicSize,halfX=halfY*cam.aspect;var boundary=rig.boundary.bounds;var p=cam.transform.position;
   bool confined=p.x-halfX>=boundary.min.x-.02f&&p.x+halfX<=boundary.max.x+.02f&&p.y-halfY>=boundary.min.y-.02f&&p.y+halfY<=boundary.max.y+.02f;
   lines.Add($"{(confined?"PASS":"FAIL")} A0{i}: 1280x720 viewport remains in authored Polygon boundary at static review point");pass&=confined;
   cam.Render();RenderTexture.active=rt;var texture=new Texture2D(1280,720,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1280,720),0,0);texture.Apply();
   Directory.CreateDirectory("Validation/Slice");File.WriteAllBytes($"Validation/Slice/A0{i}-static-02.png",texture.EncodeToPNG());
   RenderTexture.active=null;cam.targetTexture=null;rt.Release();Destroy(rt);Destroy(texture);session.Player.SetPaused(false);
  }
  lines.Insert(0,pass?"PASS static Slice camera rendering (world sprites only; GUI/human perception not assessed)":"FAIL static Slice camera rendering");File.WriteAllLines("Validation/S1-09-camera-02.txt",lines);
  SessionState.SetInt("N2B.RenderExit",pass?0:1);EditorApplication.isPlaying=false;
 }
}
