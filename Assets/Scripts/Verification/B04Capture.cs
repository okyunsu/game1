using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
public sealed class B04Capture:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B04Capture>("-verify-b04-shots");
 // Prior hidden swapchain EOF captures were black. Render the actual scene camera offscreen instead.
 // This records room geometry/actors, and deliberately does not claim OnGUI HUD/overlay photography.
 void Shot(string path,MethodInfo encode){var camera=Camera.main;if(camera==null)throw new Exception("Room camera absent");var previous=camera.targetTexture;var active=RenderTexture.active;var target=RenderTexture.GetTemporary(1920,1080,24,RenderTextureFormat.ARGB32);Texture2D pixels=null;try{camera.targetTexture=target;camera.Render();RenderTexture.active=target;pixels=new Texture2D(1920,1080,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,1920,1080),0,0);pixels.Apply();File.WriteAllBytes(path,(byte[])encode.Invoke(null,new object[]{pixels}));}finally{camera.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);if(pixels!=null)Destroy(pixels);}}
 protected override IEnumerator Tests(){var encode=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",false)?.GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});Check(encode!=null,"runtime PNG encoder available without package changes");yield return NewGame();string folder=N5Args.Value("-b04-shots-folder","Validation/_raw/B04/shots");Directory.CreateDirectory(folder);foreach(string id in new[]{"A01","A04","A05","B02","B03","B04","C02","C03","C06"}){if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/"+id+".unity","FromLeft"),"photography fixture "+id);yield return Ready();}yield return new WaitForSecondsRealtime(.3f);string path=Path.GetFullPath(Path.Combine(folder,id+".png"));if(File.Exists(path))throw new IOException(path);yield return new WaitForEndOfFrame();Shot(path,encode);Check(File.Exists(path)&&new FileInfo(path).Length>0,"shot saved "+id);lines.Add($"CAPTURE {id} 1920x1080 offscreen actual exe scene camera {path}");}lines.Add("Explicit scene setup; not natural traversal. Offscreen room camera excludes OnGUI HUD/overlays.");}
}
