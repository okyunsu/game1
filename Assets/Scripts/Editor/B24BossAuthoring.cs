using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B24BossAuthoring
{
 static GameObject Rect(Transform parent,string name,float x,float y,float w,float h,Color color,bool trigger=false)
 {
  var go=new GameObject(name);go.transform.SetParent(parent);go.transform.position=new Vector2(x+w/2,y+h/2);go.transform.localScale=new Vector3(w,h,1);go.layer=trigger?0:6;
  go.AddComponent<SpriteRenderer>().sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;go.GetComponent<SpriteRenderer>().color=color;go.AddComponent<BoxCollider2D>().isTrigger=trigger;return go;
 }
 public static void Create()
 {
  const string path="Assets/Scenes/Test/BossTest_B2.unity";if(File.Exists(path))throw new IOException("Preserve B2 trial scene");
  var baseTuning=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/ScriptableObjects/BossTuning.asset");var tuning=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/B2/BossTuning.asset");EditorUtility.CopySerialized(baseTuning,tuning);
  tuning.maxHP=26;tuning.damage=1;tuning.extendedPatterns=tuning.b3Patterns=true;tuning.phaseTwoEnabled=false;tuning.patternOrder=new[]{2,3,7};tuning.meleeEnabled=tuning.leapEnabled=tuning.chaseEnabled=tuning.comboLeapEnabled=tuning.backshotEnabled=false;tuning.waveEnabled=tuning.chargeEnabled=tuning.fallingEnabled=true;
  tuning.waveHeight=2.8f;tuning.waveBothDirections=true;tuning.waveTell=.7f;tuning.firstWaveTell=1;tuning.chargeTell=.5f;tuning.fallingTell=.8f;tuning.recovery=tuning.fallingRecovery=1;tuning.leftAnchor=5;tuning.rightAnchor=19;EditorUtility.SetDirty(tuning);
  EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var rig=UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=rig.boundary.transform&&!rig.transform.IsChildOf(child))UnityEngine.Object.DestroyImmediate(child.gameObject);
  foreach(var c in UnityEngine.Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).ToArray())if(c!=rig.boundary&&!c.transform.IsChildOf(room.transform))UnityEngine.Object.DestroyImmediate(c.gameObject);
  room.roomId="B04";room.initialSpawnId="Entry";room.name="BossTest_B2";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V4/Player.prefab");
  var label=room.GetComponent<RoomLabel>();if(label!=null)label.notice="중앙에서 벽 점프 획득 → 획득대에서 멀어지면 전투. 벽을 타고 높은 충격파를 피하세요.";
  rig.boundary.points=new[]{Vector2.zero,new Vector2(24,0),new Vector2(24,16),new Vector2(0,16)};
  Rect(room.transform,"floor",0,0,24,1,Color.gray);Rect(room.transform,"ceiling",0,15,24,1,Color.gray);
  Rect(room.transform,"left climb",0,4,1,11,Color.cyan).AddComponent<ClimbableWall>();Rect(room.transform,"right climb",23,1,1,14,Color.cyan).AddComponent<ClimbableWall>();
  var exit=Rect(room.transform,"trial entry",0,1,1,3,Color.cyan,true).AddComponent<RoomExit>();exit.exitId="B2-entry";exit.destinationRoomId="B01";exit.destinationScenePath="Assets/Scenes/V4/B01_V4.unity";exit.destinationSpawnId="Entry";
  var gate=Rect(room.transform,"B2 gate",0,1,1,3,Color.red);gate.AddComponent<ClimbableWall>();gate.SetActive(false);
  Rect(room.transform,"wall-jump pedestal",10.5f,1,3,1,Color.gray);
  Rect(room.transform,"wall-jump pickup",11.6f,2.3f,.8f,.8f,Color.cyan,true).AddComponent<WallJumpPickup>();
  foreach(string id in new[]{"Entry","CP-B01","CP-A01"})S3Authoring.Spawn(room,id,new Vector2(3.5f,1.015f));
  var cp=Rect(room.transform,"trial checkpoint",3,1,1,1,Color.cyan,true).AddComponent<Checkpoint>();cp.checkpointId="CP-B01";cp.spawn=room.GetComponentsInChildren<RoomSpawn>().Single(s=>s.spawnId=="CP-B01");
  var boss=Rect(room.transform,"B2-B04",17,1,2,3,new Color(.6f,.2f,.8f)).AddComponent<BossEncounter>();boss.bossId="B2-B04";boss.displayName="벽 감시자";boss.tuning=tuning;boss.leftExit=exit;boss.encounterGate=gate;boss.floorY=1;boss.leftWallX=1;boss.rightWallX=23;boss.startsAfterWallJumpPickup=true;boss.acquisitionX=12;boss.acquisitionDistance=3;boss.introDuration=1.8f;
  boss.wavePrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BossWave.prefab");boss.warning=Rect(room.transform,"B2 warning",17,1.05f,2,.3f,Color.yellow,true).GetComponent<SpriteRenderer>();UnityEngine.Object.DestroyImmediate(boss.warning.GetComponent<Collider2D>());
  var top=new GameObject("B2 top pogo");top.transform.SetParent(boss.transform,false);top.transform.localPosition=new Vector3(0,.48f,0);top.transform.localScale=new Vector3(1,.04f,1);top.AddComponent<BoxCollider2D>().isTrigger=true;top.AddComponent<PogoTarget>();
  room.gameObject.AddComponent<B24BossArena>().boss=boss;
  EditorSceneManager.SaveScene(room.gameObject.scene,path);AssetDatabase.SaveAssets();File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/24/boss2-authoring.txt"),"PASS B2 HP26 damage1 patterns=wave,charge,falling phase1 wave2.8 both sides firstTell1.0 intro1.8; trial22x14 walls/pedestal/checkpoint\n");EditorApplication.Exit(0);
 }
 public static void EntryOpening()
 {
  EditorSceneManager.OpenScene("Assets/Scenes/Test/BossTest_B2.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
  var all=room.GetComponentsInChildren<Transform>(true);var wall=all.Single(t=>t.name=="left climb");wall.position=new Vector3(.5f,9.5f,0);wall.localScale=new Vector3(1,11,1);
  var exit=all.Single(t=>t.name=="trial entry");exit.position=new Vector3(.5f,2.5f,0);exit.localScale=new Vector3(1,3,1);
  var gate=all.Single(t=>t.name=="B2 gate");if(gate.GetComponent<ClimbableWall>()==null)gate.gameObject.AddComponent<ClimbableWall>();
  EditorSceneManager.SaveScene(room.gameObject.scene);AssetDatabase.SaveAssets();File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/24/boss2-entry.txt"),"PASS entry x0..1/y1..4 open before encounter; closing gate completes full climb wall\n");EditorApplication.Exit(0);
 }
}
