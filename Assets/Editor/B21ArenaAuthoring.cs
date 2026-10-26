using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class B21ArenaAuthoring
{
 public static void Generate()
 {
  const string path="Assets/Scenes/Test/B21Arena.unity";if(File.Exists(path))throw new IOException("Preserve "+path);
  EditorSceneManager.OpenScene("Assets/Scenes/V5/A01_V5.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var rig=UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=rig.boundary.transform&&!rig.transform.IsChildOf(child))UnityEngine.Object.DestroyImmediate(child.gameObject);
  room.name=room.roomId="B21Arena";room.initialSpawnId="Entry";room.GetComponent<RoomLabel>().notice="B21 measurement arena: adjustable floor gap (u)";
  room.GetComponent<V4Room>().designJson="";room.GetComponent<V4Room>().dimensions=new Vector2(60,20);
  rig.boundary.points=new[]{new Vector2(-22,-10),new Vector2(40,-10),new Vector2(40,15),new Vector2(-22,15)};
  var sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
  Func<string,float,BoxCollider2D> floor=(name,x)=>{var go=new GameObject(name);go.transform.SetParent(room.transform);go.layer=6;go.transform.position=new Vector2(x,-.5f);go.transform.localScale=new Vector3(20,1,1);go.AddComponent<SpriteRenderer>().sprite=sprite;return go.AddComponent<BoxCollider2D>();};
  floor("Approach floor",-10);var course=room.gameObject.AddComponent<B21ArenaCourse>();course.landing=floor("Landing floor",14.5f);course.Configure(4.5f);
  S3Authoring.Spawn(room,"Entry",new Vector2(-3.5f,.015f));EditorSceneManager.SaveScene(room.gameObject.scene,path);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
