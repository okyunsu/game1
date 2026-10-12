using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class B07Parts
{
 public static void Validate()
 {
  var gate=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V3/DecorativeGate.prefab");
  bool valid=gate.GetComponent<V3DecorativeGate>()!=null&&!gate.GetComponent<BoxCollider2D>().isTrigger&&gate.GetComponent<RoomExit>()==null&&gate.GetComponent<BreakableWall>()==null;
  File.WriteAllText("Validation/_raw/B07/T2-decoration.txt",(valid?"PASS":"FAIL")+" DecorativeGate solid collider, no exit or breakable/interaction component\n");EditorApplication.Exit(valid?0:1);
 }
 public static void Create()
 {
  Directory.CreateDirectory("Assets/Prefabs/V3");AssetDatabase.Refresh();
  var gate=PrefabUtility.LoadPrefabContents("Assets/Prefabs/Level/Landing.prefab");gate.AddComponent<V3DecorativeGate>().meaning="열리지 않는 관문";gate.name="DecorativeGate";
  PrefabUtility.SaveAsPrefabAsset(gate,"Assets/Prefabs/V3/DecorativeGate.prefab");PrefabUtility.UnloadPrefabContents(gate);
  var drop=new GameObject("OneWayDrop",typeof(BoxCollider2D),typeof(RoomExit),typeof(V3ExitRule));drop.GetComponent<BoxCollider2D>().isTrigger=true;drop.GetComponent<V3ExitRule>().downwardOnly=true;
  PrefabUtility.SaveAsPrefabAsset(drop,"Assets/Prefabs/V3/OneWayDrop.prefab");Object.DestroyImmediate(drop);
  var summit=new GameObject("SummitEnding",typeof(BoxCollider2D),typeof(V3SummitEnding));summit.GetComponent<BoxCollider2D>().isTrigger=true;
  PrefabUtility.SaveAsPrefabAsset(summit,"Assets/Prefabs/V3/SummitEnding.prefab");Object.DestroyImmediate(summit);
  EditorSceneManager.OpenScene(V3SceneMap.Path("C06"));AttachBoss(Object.FindFirstObjectByType<RoomDefinition>());EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
  EditorSceneManager.OpenScene(V3SceneMap.Path("S01"));var room=Object.FindFirstObjectByType<RoomDefinition>();var ending=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V3/SummitEnding.prefab"));ending.transform.SetParent(room.transform);ending.transform.position=new Vector2(18,2);EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
  AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static V3BossDoor AttachBoss(RoomDefinition room)
 {
  var boss=Object.FindFirstObjectByType<BossEncounter>();var door=room.gameObject.AddComponent<V3BossDoor>();door.boss=boss;
  var exit=S3Authoring.Exit(room,"Summit",new Vector2(10,9.5f),"S01","Entry");exit.destinationScenePath=V3SceneMap.Path("S01");exit.gameObject.AddComponent<V3ExitRule>().bossOnly=true;door.upperExit=exit;
  door.blocker=S3Authoring.Block(room,"Wall","V3 summit sealed panel",new Vector2(10,9.5f),new Vector2(1,2));
  var stairs=new System.Collections.Generic.List<GameObject>();
  for(int i=0;i<4;i++)stairs.Add(S3Authoring.Block(room,"Landing","V3 post-victory step "+i,new Vector2(i%2==0?5:11,2.75f+i*2),new Vector2(3,.5f)));
  door.stairs=stairs.ToArray();foreach(var step in door.stairs)step.SetActive(false);exit.enabled=false;return door;
 }
}
