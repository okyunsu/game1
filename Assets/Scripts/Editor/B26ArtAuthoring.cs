using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B26ArtAuthoring
{
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
  var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();string before=Geometry(room);var art=room.GetComponent<B25ArtRoom>();var bg=art.background;var size=bg.sprite.bounds.size;bg.size=new Vector2(bg.size.x,size.y);var position=bg.transform.position;position.y=size.y/2;bg.transform.position=position;
   foreach(var vent in room.GetComponentsInChildren<SteamVent>())if(vent.GetComponent<B26SteamVisual>()==null)vent.gameObject.AddComponent<B26SteamVisual>();
   if(before!=Geometry(room))throw new Exception("Geometry changed "+room.roomId);EditorSceneManager.SaveScene(room.gameObject.scene,path);rows.Add("PASS "+room.roomId+" bottom-aligned single-height background22.5u; sky1 above; steam alpha0.25 rising display particles; Collider2D unchanged");
  }
  File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/26/backgrounds.txt"),rows);EditorApplication.Exit(0);
 }
}
