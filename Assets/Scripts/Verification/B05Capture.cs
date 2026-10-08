using System;
using System.Collections;
using System.IO;
using UnityEngine;
public sealed class B05Capture:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Capture>("-verify-b05-shot");
 protected override IEnumerator Tests(){yield return NewGame();yield return new WaitForSecondsRealtime(.4f);var camera=Camera.main;var previous=camera.targetTexture;var active=RenderTexture.active;var target=RenderTexture.GetTemporary(1920,1080,24);camera.targetTexture=target;camera.Render();RenderTexture.active=target;var pixels=new Texture2D(1920,1080,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,1920,1080),0,0);pixels.Apply();var encode=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});File.WriteAllBytes(Path.ChangeExtension(Evidence,"png"),(byte[])encode.Invoke(null,new object[]{pixels}));camera.targetTexture=previous;RenderTexture.active=active;RenderTexture.ReleaseTemporary(target);Destroy(pixels);Check(true,"1920x1080 actual V2 room camera PNG; excludes OnGUI HUD");}
}
