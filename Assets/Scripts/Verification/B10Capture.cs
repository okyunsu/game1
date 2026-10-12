using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Actual scene renders for the B10 geometry handoff. Does not assert traversal.
public sealed class B10Capture:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10Capture>("-verify-b10-capture");
 protected override IEnumerator Tests()
 {
  yield return NewGame();ProgressSave.Enabled=false;string id=N5Args.Value("-b10-room","A01");
  if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);Destroy(GameState.GetOrCreate().gameObject);yield return null;
  SceneManager.LoadScene(V4SceneMap.Path(id));yield return Ready();yield return null;
  var room=FindFirstObjectByType<V4Room>();var camera=Camera.main;Vector3 pos=camera.transform.position;float size=camera.orthographicSize;
  Vector2 dimensions=room.dimensions;camera.transform.position=new Vector3(dimensions.x/2,dimensions.y/2,pos.z);camera.orthographicSize=Mathf.Max(dimensions.y/2+1,dimensions.x/2/((float)1920/1080)+1);
  Capture(camera,Path.ChangeExtension(Evidence,"overview.png"));
  if(N5Args.Has("-b10-x")){float x=float.Parse(N5Args.Value("-b10-x","0"),System.Globalization.CultureInfo.InvariantCulture),y=float.Parse(N5Args.Value("-b10-y","0"),System.Globalization.CultureInfo.InvariantCulture);camera.transform.position=new Vector3(x,y,pos.z);camera.orthographicSize=8;Capture(camera,Path.ChangeExtension(Evidence,"detail.png"));}
  camera.transform.position=pos;camera.orthographicSize=size;Check(true,"actual V4 room render "+id+"; camera changes only, no gameplay traversal claim");
 }
 void Capture(Camera camera,string path)
 {
  if(File.Exists(path))throw new IOException("Preserve "+path);var previous=camera.targetTexture;var active=RenderTexture.active;var target=RenderTexture.GetTemporary(1920,1080,24);camera.targetTexture=target;camera.Render();RenderTexture.active=target;
  var pixels=new Texture2D(1920,1080,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,1920,1080),0,0);pixels.Apply();var encode=System.Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});File.WriteAllBytes(path,(byte[])encode.Invoke(null,new object[]{pixels}));camera.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);Destroy(pixels);
 }
}
