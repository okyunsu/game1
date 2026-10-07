using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B03Room
{
 public static void RepairEntry(){EditorSceneManager.OpenScene("Assets/Scenes/B03.unity");var go=GameObject.Find("Exit Optional B06");if(go.GetComponent<OptionalRoomEntry>()==null)throw new Exception("Wrong object");UnityEngine.Object.DestroyImmediate(go.GetComponent<RoomExit>());EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorApplication.Exit(0);}
 public static void Create(){string path="Assets/Scenes/B06.unity";if(File.Exists(path))throw new IOException("Preserve B06");File.Copy("Validation/_raw/B02/blocked-B06.unity",path);File.Copy("Validation/_raw/B02/blocked-B06.unity.meta",path+".meta");AssetDatabase.Refresh();
  EditorSceneManager.OpenScene("Assets/Scenes/B03.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();if(GameObject.Find("Exit Optional B06")!=null)throw new Exception("Existing entrance");
  var entry=S3Authoring.Exit(room,"Optional B06",new Vector2(7,2),"B06","Entry");var go=entry.gameObject;go.AddComponent<OptionalRoomEntry>();UnityEngine.Object.DestroyImmediate(entry);foreach(var sprite in go.GetComponentsInChildren<SpriteRenderer>())sprite.color=new Color(.25f,.9f,1);
  EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();File.WriteAllText("Validation/_raw/B03/B06-authoring.txt","Restored B06 internal scene unchanged. B03 adds cyan optional door at (7,2), requires Up while overlapping. Existing objects unchanged; no platform needed; ordinary Left/Right/Jump/Dash replay cannot activate.\n");EditorApplication.Exit(0);
 }
}
