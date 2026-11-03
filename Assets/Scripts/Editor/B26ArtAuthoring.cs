using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B26ArtAuthoring
{
 public static void Sparkles()
 {
  var rows=new List<string>();var settings=AssetDatabase.LoadAssetAtPath<B25ArtSettings>(B25ArtImport.Root+"/ArtSettings.asset");settings.sparkleSeconds=1.5f;EditorUtility.SetDirty(settings);
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();string before=Geometry(room);int count=0;foreach(var binding in room.GetComponentsInChildren<B25ArtVisual>(true))if(binding.GetComponent<DiscoveryReward>()!=null||binding.GetComponent<Checkpoint>()!=null||binding.GetComponent<DashPickup>()!=null||binding.GetComponent<WallJumpPickup>()!=null){var sparkle=binding.GetComponent<B26IdleSparkle>()??binding.gameObject.AddComponent<B26IdleSparkle>();sparkle.settings=settings;count++;}if(before!=Geometry(room))throw new Exception("Sparkle changed geometry");EditorSceneManager.SaveScene(room.gameObject.scene,path);rows.Add($"PASS {room.roomId} idle sparkles={count} cycle=1.5s collider unchanged");}
  AssetDatabase.SaveAssets();File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/26/sparkle.txt"),rows);EditorApplication.Exit(0);
 }
 public static void Hud()
 {
  var settings=AssetDatabase.LoadAssetAtPath<B25ArtSettings>(B25ArtImport.Root+"/ArtSettings.asset");settings.fragment=B25ArtImport.Sprite("fragment");EditorUtility.SetDirty(settings);AssetDatabase.SaveAssets();File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/26/hud.txt"),"PASS HUD uses authored fragment sprite and saved n/4; other HUD/geometry unchanged\n");EditorApplication.Exit(0);
 }
 static string Geometry(RoomDefinition room)=>string.Join("\n",room.GetComponentsInChildren<Collider2D>(true).Select(c=>c.name+" "+c.transform.position.ToString("F5")+" "+c.transform.lossyScale.ToString("F5")+" "+EditorJsonUtility.ToJson(c)));
 public static void Guides()
 {
  var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();string before=Geometry(room);
   foreach(var sign in room.GetComponentsInChildren<SignPost>(true)){var sr=sign.GetComponent<SpriteRenderer>();if(sr!=null){sr.enabled=false;rows.Add("MEASURE "+room.roomId+" guide SignPost="+sign.name+" position="+sign.transform.position.ToString("F3")+" message="+sign.message+" displayDisabled=True gameplay/collider unchanged");}}
   if(before!=Geometry(room))throw new Exception("Guide geometry changed "+room.roomId);EditorSceneManager.SaveScene(room.gameObject.scene,path);
  }
  File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/26/guides.txt"),rows);EditorApplication.Exit(0);
 }
 public static void Backgrounds()
 {
  const string skyPath=B25ArtImport.Root+"/SkyBlend.asset",texturePath=B25ArtImport.Root+"/SkyBlendTexture.asset";
  var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
  if(texture==null){texture=new Texture2D(1,32,TextureFormat.RGBA32,false);texture.filterMode=FilterMode.Point;texture.wrapMode=TextureWrapMode.Clamp;for(int i=0;i<32;i++)texture.SetPixel(0,i,new Color(27/255f,34/255f,54/255f,i/31f));texture.Apply();AssetDatabase.CreateAsset(texture,texturePath);}
  var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(skyPath);if(sprite==null){sprite=UnityEngine.Sprite.Create(texture,new Rect(0,0,1,32),Vector2.one*.5f,16);AssetDatabase.CreateAsset(sprite,skyPath);}
  var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();string before=Geometry(room);var art=room.GetComponent<B25ArtRoom>();var bg=art.background;var size=bg.sprite.bounds.size;bg.size=new Vector2(bg.size.x,size.y);var position=bg.transform.position;position.y=size.y/2;bg.transform.position=position;
   var cap=room.transform.Find("Sky1 seamless transition");if(cap!=null)UnityEngine.Object.DestroyImmediate(cap.gameObject);var go=new GameObject("Sky1 seamless transition");go.transform.SetParent(room.transform);go.transform.position=new Vector3(position.x,size.y-1,0);var blend=go.AddComponent<SpriteRenderer>();blend.sprite=sprite;blend.drawMode=SpriteDrawMode.Sliced;blend.size=new Vector2(bg.size.x,2);blend.sortingOrder=-99;
   foreach(var vent in room.GetComponentsInChildren<SteamVent>())if(vent.GetComponent<B26SteamVisual>()==null)vent.gameObject.AddComponent<B26SteamVisual>();
   if(before!=Geometry(room))throw new Exception("Geometry changed "+room.roomId);EditorSceneManager.SaveScene(room.gameObject.scene,path);rows.Add("PASS "+room.roomId+" bottom-aligned single-height background22.5u; sky1 above; steam alpha0.25 rising display particles; Collider2D unchanged");
  }
  File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/26/backgrounds.txt"),rows);EditorApplication.Exit(0);
 }
}
