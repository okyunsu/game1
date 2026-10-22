using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BodySizeAuthoring
{
 static RoomDefinition room;static Sprite sprite;
 static GameObject Box(string name,float x,float y,float w,float h,Color color,bool trigger=false)
 {
  var go=new GameObject(name);go.transform.SetParent(room.transform);go.transform.position=new Vector2(x+w/2,y+h/2);go.layer=trigger?0:6;go.AddComponent<SpriteRenderer>().sprite=sprite;go.GetComponent<SpriteRenderer>().color=color;go.transform.localScale=new Vector3(w,h,1);go.AddComponent<BoxCollider2D>().isTrigger=trigger;return go;
 }
 static void Label(string text,float x,float y){var go=new GameObject("Label "+text);go.transform.SetParent(room.transform);go.transform.position=new Vector3(x,y,-1);var mesh=go.AddComponent<TextMesh>();mesh.text=text;mesh.fontSize=64;mesh.characterSize=.14f;mesh.anchor=TextAnchor.MiddleCenter;mesh.color=Color.white;}
 [MenuItem("Afterglow/Generate Body Size Test")]
 public static void Generate()
 {
  const string output="Assets/Scenes/Test/BodySizeTest.unity";if(System.IO.File.Exists(output)&&!N5Args.Has("-b20-regenerate-test"))throw new InvalidOperationException("Preserve existing BodySizeTest; edit its Scene directly");
  EditorSceneManager.OpenScene("Assets/Scenes/V5/A01_V5.unity");room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var rig=UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
  sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=rig.boundary.transform&&!rig.transform.IsChildOf(child))UnityEngine.Object.DestroyImmediate(child.gameObject);
  room.name="BodySizeTest";room.roomId="BodySizeTest";room.initialSpawnId="Entry";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V4/Player.prefab");
  room.GetComponent<RoomLabel>().notice="F6: 1.6 / 1.3 / 1.1   Z: jump   X: attack   Down+X: pogo";
  var v4=room.GetComponent<V4Room>();v4.testCourse=false;v4.designJson="";v4.dimensions=new Vector2(116,18);
  rig.boundary.points=new[]{Vector2.zero,new Vector2(116,0),new Vector2(116,18),new Vector2(0,18)};
  S3Authoring.Spawn(room,"Entry",new Vector2(3,1.015f));
  Box("flat",0,0,116,1,new Color(.25f,.32f,.4f));Box("left wall",-1,0,1,18,Color.gray);Box("right wall",116,0,1,18,Color.gray);
  float[] clearances={2.2f,2.6f,3,3.5f,4,4.5f};
  for(int i=0;i<clearances.Length;i++){float x=6+i*8;Box("bar"+i,x,1+clearances[i],5,.25f,new Color(.7f,.5f,.25f));Label(clearances[i].ToString("F1")+"u",x+2.5f,1+clearances[i]+.75f);}
  float left=61,top=1;
  for(int i=0;i<5;i++){float rise=1.5f+i*.125f;top+=rise;Box("step"+i,left,top-.5f,1.8f,.5f,new Color(.4f,.6f,.75f));Label("+"+rise.ToString("F2"),left+.9f,top+.55f);left+=1.8f+1+i*.125f;}
  Box("lantern",85.5f,3,1,.5f,Color.yellow,true).AddComponent<PogoTarget>();Label("Down + X",86,4.4f);
  var enemy=S3Authoring.Enemy("E1",new Vector2(100,1.51f));enemy.transform.SetParent(room.transform);Label("E1",100,3);
  room.gameObject.AddComponent<BodySizeCourse>();
  EditorSceneManager.SaveScene(room.gameObject.scene,output);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
