using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class BossSetup
{
 public static void Run(){
  var tuning=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/ScriptableObjects/BossTuning.asset");if(tuning==null){tuning=ScriptableObject.CreateInstance<BossTuning>();AssetDatabase.CreateAsset(tuning,"Assets/ScriptableObjects/BossTuning.asset");}
  var sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab").GetComponent<SpriteRenderer>().sprite;
  var wave=new GameObject("BossWave");wave.AddComponent<SpriteRenderer>().sprite=sprite;wave.GetComponent<SpriteRenderer>().color=new Color(1,.7f,.2f);wave.AddComponent<BoxCollider2D>().isTrigger=true;wave.AddComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Kinematic;wave.AddComponent<BossWave>();var wavePrefab=PrefabUtility.SaveAsPrefabAsset(wave,"Assets/Prefabs/BossWave.prefab");Object.DestroyImmediate(wave);
  EditorSceneManager.OpenScene("Assets/Scenes/C04.unity");var camera=Object.Instantiate(Object.FindFirstObjectByType<RoomCameraRig>().gameObject);var previous=SceneManager.GetActiveScene();
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.MoveGameObjectToScene(camera,scene);EditorSceneManager.CloseScene(previous,true);SceneManager.SetActiveScene(scene);camera.name="Room Camera";
  var root=new GameObject("C06");var room=root.AddComponent<RoomDefinition>();room.roomId="C06";room.initialSpawnId="FromLeft";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");root.AddComponent<RoomLabel>();
  S3Authoring.Block(room,"Landing","C06 Floor",new Vector2(10,.5f),new Vector2(20,1));S3Authoring.Block(room,"Wall","C06 Left Wall",new Vector2(.5f,7),new Vector2(1,14));S3Authoring.Block(room,"Wall","C06 Right Wall",new Vector2(19.5f,7),new Vector2(1,14));S3Authoring.Block(room,"Ceiling","C06 Ceiling",new Vector2(10,14.5f),new Vector2(20,1));
  var boundary=new GameObject("C06 Camera Boundary");boundary.transform.SetParent(root.transform);var polygon=boundary.AddComponent<PolygonCollider2D>();polygon.isTrigger=true;polygon.SetPath(0,new[]{new Vector2(0,0),new Vector2(20,0),new Vector2(20,14),new Vector2(0,14)});camera.GetComponent<RoomCameraRig>().boundary=polygon;
  var exit=S3Authoring.Exit(room,"Left",new Vector2(1.75f,2),"C04","FromRight");S3Authoring.Spawn(room,"FromLeft",new Vector2(3.5f,1.81f));
  var boss=new GameObject("Normal Summit Guardian");boss.layer=6;boss.transform.SetParent(root.transform);boss.transform.position=new Vector2(17.5f,2.5f);boss.transform.localScale=new Vector3(2,3,1);boss.AddComponent<SpriteRenderer>().sprite=sprite;boss.GetComponent<SpriteRenderer>().color=new Color(.55f,.5f,.65f);boss.GetComponent<SpriteRenderer>().flipX=true;boss.AddComponent<BoxCollider2D>();
  var tell=new GameObject("Melee Wave Warning");tell.transform.SetParent(root.transform);tell.transform.position=new Vector2(15,1.5f);tell.transform.localScale=new Vector3(3,1,1);var warning=tell.AddComponent<SpriteRenderer>();warning.sprite=sprite;warning.sortingOrder=5;
  var encounter=boss.AddComponent<BossEncounter>();encounter.tuning=tuning;encounter.leftExit=exit;encounter.warning=warning;encounter.wavePrefab=wavePrefab;S3Authoring.Save("Assets/Scenes/C06.unity");
  EditorSceneManager.OpenScene("Assets/Scenes/C04.unity");foreach(var e in Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))if(e.exitId=="Right")e.enabled=true;EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
 }
}

