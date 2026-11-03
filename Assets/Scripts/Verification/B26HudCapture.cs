#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.IO;
using UnityEngine;
// Isolated RenderTexture of the live HUD icon/count, not a desktop screenshot.
public static class B26HudCapture
{
 public static void Save(Sprite fragment,int count,string path)
 {
  if(File.Exists(path))throw new IOException("Preserve "+path);var root=new GameObject("HUD fragment capture");root.transform.position=new Vector3(10000,10000,0);var target=RenderTexture.GetTemporary(384,128,24);var old=RenderTexture.active;Texture2D pixels=null;
  try
  {
   var icon=new GameObject("fragment icon");icon.layer=31;icon.transform.SetParent(root.transform,false);icon.transform.localPosition=new Vector3(-.7f,0,0);icon.transform.localScale=Vector3.one*(.8f/fragment.bounds.size.y);icon.AddComponent<SpriteRenderer>().sprite=fragment;
   var label=new GameObject("live n/4");label.layer=31;label.transform.SetParent(root.transform,false);label.transform.localPosition=new Vector3(.1f,0,0);var text=label.AddComponent<TextMesh>();text.text=count+"/4";text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=48;text.characterSize=.25f;text.anchor=TextAnchor.MiddleLeft;label.GetComponent<MeshRenderer>().material=text.font.material;
   var cameraGo=new GameObject("HUD capture camera");cameraGo.transform.SetParent(root.transform,false);cameraGo.transform.localPosition=new Vector3(0,0,-10);var camera=cameraGo.AddComponent<Camera>();camera.orthographic=true;camera.orthographicSize=.7f;camera.cullingMask=1<<31;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.04f,.05f,.07f);camera.targetTexture=target;camera.Render();RenderTexture.active=target;pixels=new Texture2D(384,128,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,384,128),0,0);pixels.Apply();var encode=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});File.WriteAllBytes(path,(byte[])encode.Invoke(null,new object[]{pixels}));
  }
  finally{RenderTexture.active=old;RenderTexture.ReleaseTemporary(target);if(pixels!=null)UnityEngine.Object.Destroy(pixels);UnityEngine.Object.Destroy(root);}
 }
}
#endif
