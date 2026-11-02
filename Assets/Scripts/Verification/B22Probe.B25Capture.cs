#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public sealed partial class B22Probe
{
 void B25CameraPng(string path,Camera chosen=null,int width=1280,int height=720)
 {
  var camera=chosen!=null?chosen:Camera.main;var previous=camera.targetTexture;var active=RenderTexture.active;var target=RenderTexture.GetTemporary(width,height,24,RenderTextureFormat.ARGB32);Texture2D pixels=null;
  try{camera.targetTexture=target;camera.Render();RenderTexture.active=target;pixels=new Texture2D(width,height,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,width,height),0,0);pixels.Apply();var encode=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});File.WriteAllBytes(path,(byte[])encode.Invoke(null,new object[]{pixels}));}
  finally{camera.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);if(pixels!=null)Destroy(pixels);}
 }
 IEnumerator B25OverviewCapture()
 {
  yield return RoomAt("B03");Body.simulated=false;PlaceForAuthorizedSetup(new Vector2(4,1.015f));yield return null;yield return null;
  var go=new GameObject("Temporary B03 overview camera");var camera=go.AddComponent<Camera>();camera.enabled=false;camera.orthographic=true;camera.orthographicSize=18;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=Camera.main.backgroundColor;go.transform.position=new Vector3(12,18,-10);
  string path="Validation/_raw/25/art/B03-overview.png";if(File.Exists(path))throw new IOException("Preserve "+path);B25CameraPng(path,camera,768,1152);Destroy(go);Body.simulated=true;Check(File.Exists(path),"B03 full authored-room overview captured");Record("CAPTURE actual B03 camera 768x1152; comparison with mockup; setup pose, not route proof");
 }
 IEnumerator B25ArtCapture()
 {
  var poses=new Dictionary<string,Vector2[]>{
   {"A01",new[]{new Vector2(16,6.815f),new Vector2(44.5f,11.615f)}},{"A02",new[]{new Vector2(20.5f,9.015f),new Vector2(14,30)}},
   {"A03",new[]{new Vector2(20.5f,5.015f),new Vector2(35.5f,13.015f)}},{"A04",new[]{new Vector2(20.3f,2.015f),new Vector2(37,1.015f)}},
   {"A05",new[]{new Vector2(37,2.615f),new Vector2(9,2)}},{"A06",new[]{new Vector2(15,5),new Vector2(36,5)}},
   {"B01",new[]{new Vector2(23,1.015f),new Vector2(7,15.015f)}},{"B02",new[]{new Vector2(20,4),new Vector2(31,8.015f)}},
   {"B03",new[]{new Vector2(12,8),new Vector2(21,26)}},{"B04",new[]{new Vector2(12,2),new Vector2(12,11)}},
   {"B05",new[]{new Vector2(14,4),new Vector2(14,18)}},{"B06",new[]{new Vector2(15,4),new Vector2(27,4)}}};
  string folder=N5Args.Value("-b25-art-output","Validation/_raw/25/art");Directory.CreateDirectory(folder);int count=0;
  foreach(string id in poses.Keys)
  {
   Body.simulated=true;yield return RoomAt(id);Body.simulated=false;
   var art=Room.GetComponent<B25ArtRoom>();Check(art!=null&&Math.Abs(art.settings.parallax-.3f)<.001,"authored original art room "+id);
   foreach(var reset in Room.GetComponentsInChildren<EnemyReset>()){var live=reset.Live.GetComponent<BoxCollider2D>();var source=reset.prefab.GetComponent<BoxCollider2D>();Check(live.size==source.size&&live.offset==source.offset,"enemy collider unchanged "+id);}
   int index=0;
   foreach(var at in poses[id])
   {
    PlaceForAuthorizedSetup(at);FindFirstObjectByType<RoomCameraRig>().BindAndSnap(Player);yield return null;yield return null;
    string path=Path.Combine(folder,id+"-"+(++index)+".png");if(File.Exists(path))throw new IOException("Preserve "+path);B25CameraPng(path);count++;Record("CAPTURE actual room camera "+path+" setup pose; physics disabled; excludes OnGUI HUD");
   }
  }
  Check(count==24,"24 actual room camera screenshots");Record("MEASURE player collider size="+Player.GetComponent<BoxCollider2D>().size+" offset="+Player.GetComponent<BoxCollider2D>().offset);Body.simulated=true;
 }
}
#endif
