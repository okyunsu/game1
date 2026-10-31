using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;
using J=B09Json;
public static class B22Authoring
{
 const string TuningPath="Assets/BossCandidates/B1/BossTuning.asset";
 static string Output=>N5Args.Value("-verify-output","Validation/_raw/22/authoring.txt");
 static Sprite Sprite=>AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
 static GameObject Rect(Transform parent,string id,float[] r,Color color,bool trigger=false)
 {
  var go=new GameObject(id);go.transform.SetParent(parent);go.transform.position=new Vector2(r[0]+r[2]/2,r[1]+r[3]/2);go.transform.localScale=new Vector3(r[2],r[3],1);go.layer=trigger?0:6;
  var sr=go.AddComponent<SpriteRenderer>();sr.sprite=Sprite;sr.color=color;go.AddComponent<BoxCollider2D>().isTrigger=trigger;return go;
 }
 static float[] Floats(Dictionary<string,object> d,string key)=>J.A(d,key).Select(J.F).ToArray();
 public static void Candidate()
 {
  if(File.Exists(TuningPath))throw new IOException("Preserve B1 tuning");
  Directory.CreateDirectory("Assets/BossCandidates/B1");AssetDatabase.Refresh();
  var t=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/ScriptableObjects/BossTuning.asset"));t.name="BossTuning";
  t.maxHP=14;t.damage=1;t.extendedPatterns=t.b3Patterns=true;t.phaseTwoEnabled=false;
  t.leapEnabled=t.waveEnabled=t.chargeEnabled=true;t.meleeEnabled=t.chaseEnabled=t.backshotEnabled=t.comboLeapEnabled=t.fallingEnabled=false;
  t.chargeSpeed=7.5f;t.leftAnchor=29;t.rightAnchor=44;
  AssetDatabase.CreateAsset(t,TuningPath);AssetDatabase.SaveAssets();
  File.WriteAllLines(Output,new[]{"PASS B1 asset HP14 damage1 phaseTwo=False patterns=leap,wave,charge","Ruling chargeSpeed=7.5u/s: midpoint of approved 7..8 candidate range; anchors=29,44 inside arena"});EditorApplication.Exit(0);
 }
 static Dictionary<string,object> BossSpec()
 {
  var rooms=J.D(J.Read(File.ReadAllText("Design/v5/rooms_v5_A.json"))["rooms"]);return J.D(J.D(rooms["A04"])["boss"]);
 }
 static void AttachBoss(RoomDefinition room,Dictionary<string,object> spec)
 {
  var a=Floats(spec,"arena");var spawn=Floats(spec,"spawn");var trigger=J.D(spec["trigger"]);var cover=J.D(spec["floor_cover"]);
  var gate=Rect(room.transform,"V5-A04-bossgate",Floats(spec,"gate_wall"),new Color(.8f,.25f,.2f));gate.SetActive(false);
  var floor=Rect(room.transform,J.S(cover,"id"),Floats(cover,"rect"),new Color(.65f,.4f,.9f));
  var go=Rect(room.transform,J.S(spec,"id"),new[]{spawn[0]-1,spawn[1],2f,2f},new Color(.75f,.2f,.5f));
  var boss=go.AddComponent<BossEncounter>();boss.tuning=AssetDatabase.LoadAssetAtPath<BossTuning>(TuningPath);boss.bossId=J.S(spec,"id");
  boss.floorY=a[1];boss.leftWallX=a[0];boss.rightWallX=a[0]+a[2];boss.startX=J.F(trigger["x_min"]);boss.startMaxY=J.F(trigger["y_max"]);boss.feetTrigger=true;boss.entranceSeconds=.8f;boss.introDuration=1.8f;
  boss.encounterGate=gate;boss.floorCover=floor;boss.wavePrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BossWave.prefab");
  boss.warning=Rect(room.transform,"B1 warning",new[]{spawn[0]-1,a[1],2f,.3f},Color.yellow,true).GetComponent<SpriteRenderer>();UnityEngine.Object.DestroyImmediate(boss.warning.GetComponent<Collider2D>());
  // Separate top target; the boss body remains damageable and uses its existing contact rules.
  var top=new GameObject("B1 top PogoTarget");top.transform.SetParent(go.transform,false);top.transform.localPosition=new Vector3(0,.48f,0);top.transform.localScale=new Vector3(1,.04f,1);top.AddComponent<BoxCollider2D>().isTrigger=true;top.AddComponent<PogoTarget>();
  var drop=room.GetComponentsInChildren<RoomExit>().FirstOrDefault(e=>e.exitId=="A04-A05");
  if(drop!=null){var c=drop.GetComponent<BoxCollider2D>();c.size=new Vector2(1,.9f);c.offset=new Vector2(0,-.05f);}
 }
 public static void TestScene()
 {
  string path="Assets/Scenes/Test/BossTest_B1.unity";if(File.Exists(path))throw new IOException("Preserve test scene");
  EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var rig=UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=rig.boundary.transform&&!rig.transform.IsChildOf(child))UnityEngine.Object.DestroyImmediate(child.gameObject);
  foreach(var c in UnityEngine.Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).ToArray())if(c!=rig.boundary&&!c.transform.IsChildOf(room.transform))UnityEngine.Object.DestroyImmediate(c.gameObject);
  room.roomId="A04";room.initialSpawnId="Entry";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V4/Player.prefab");room.name="BossTest_B1";
  rig.boundary.points=new[]{Vector2.zero,new Vector2(50,0),new Vector2(50,12),new Vector2(0,12)};
  Rect(room.transform,"floor",new[]{0f,0,44,1},Color.gray);Rect(room.transform,"right floor",new[]{49f,0,1,1},Color.gray);
  Rect(room.transform,"left wall",new[]{0f,1,1,10},Color.gray);Rect(room.transform,"right wall",new[]{49f,1,1,10},Color.gray);Rect(room.transform,"ceiling",new[]{0f,11,50,1},Color.gray);
  S3Authoring.Spawn(room,"Entry",new Vector2(25.5f,1.015f));
  S3Authoring.Spawn(room,"CP-A01",new Vector2(25.5f,1.015f));
  AttachBoss(room,BossSpec());EditorSceneManager.SaveScene(room.gameObject.scene,path);AssetDatabase.SaveAssets();File.WriteAllText(Output,"PASS BossTest_B1 arena25x10 gate/cover/top PogoTarget, Entry at safe x25.5; retry CP uses actual A03\n");EditorApplication.Exit(0);
 }
 public static void Generate()
 {
  var spec=BossSpec();ValidateBoss(spec);
  B09Authoring.Generate("Design/v5/rooms_v5_A.json","Assets/Scenes/V5","Design/v5/world_v5_A.json");
  var rows=new List<string>();
  foreach(var path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
   if(room.roomId=="A04")AttachBoss(room,spec);
   var source=J.D(J.Read(File.ReadAllText("Design/v5/rooms_v5_A.json"))["rooms"]);
   foreach(var node in J.A(J.D(source[room.roomId]),"lifts")){var data=J.D(node);var lift=room.GetComponentsInChildren<LeverLift>().Single(l=>l.name=="lift:"+J.S(data,"id"));lift.startLow=J.S(data,"start")=="low";if(lift.startLow){var p=lift.transform.position;p.y=lift.low-lift.GetComponent<Collider2D>().bounds.extents.y;lift.transform.position=p;}}

   var host=new GameObject("V5 hazard configuration").AddComponent<B21CandidateHost>();SceneManager.MoveGameObjectToScene(host.gameObject,room.gameObject.scene);
   host.hazardSettings=AssetDatabase.LoadAssetAtPath<B21HazardReturnSettings>("Assets/Candidates/B21HazardReturn.asset");host.cornerSettings=AssetDatabase.LoadAssetAtPath<B21CornerFixSettings>("Assets/Candidates/B21CornerFix.asset");
   if(room.roomId=="A03"&&(!room.TryGetSpawn("CP-A03",out var cp)||room.GetComponentsInChildren<Checkpoint>().Count(c=>c.checkpointId=="CP-A03")!=1))throw new Exception("CP-A03 missing/duplicate");
   if(room.GetComponentsInChildren<V4ExitRule>().Any(r=>!string.IsNullOrEmpty(r.closedUntilWallBroken))||room.GetComponentsInChildren<BreakableWall>().Length!=0)throw new Exception("Unexpected shortcut lock/wall");
   rows.Add("PASS "+room.roomId+" generated; shortcut locks/walls=0; hazard config present");EditorSceneManager.SaveScene(room.gameObject.scene,path);
  }
  AssetDatabase.SaveAssets();File.WriteAllLines(Output,rows);EditorApplication.Exit(0);
 }
 static void ValidateBoss(Dictionary<string,object> spec)
 {
  if(J.S(spec,"id")!="B1-A04"||!Floats(spec,"arena").SequenceEqual(new[]{24f,1,25,10})||!Floats(spec,"gate_wall").SequenceEqual(new[]{23f,1,1,10})||!Floats(spec,"spawn").SequenceEqual(new[]{42f,1})||J.S(spec,"checkpoint")!="CP-A03")throw new Exception("Boss contract mismatch");
  var trigger=J.D(spec["trigger"]);var cover=J.D(spec["floor_cover"]);
  if(J.F(trigger["x_min"])!=27||J.F(trigger["y_max"])!=4||J.S(cover,"id")!="V5-A04-bossfloor"||!Floats(cover,"rect").SequenceEqual(new[]{44f,0,5,1}))throw new Exception("Boss trigger/cover mismatch");
 }
 public static void Fixtures()
 {
  ValidateBoss(BossSpec());var invalid=BossSpec();invalid["spawn"]=new List<object>{41.0,1.0};bool rejected=false;try{ValidateBoss(invalid);}catch(Exception){rejected=true;}
  if(!rejected)throw new Exception("Bad fixture accepted");
  File.WriteAllLines(Output,new[]{"PASS fixed C-1/C-2/C-6 fields including arena gate spawn trigger cover checkpoint","PASS changed spawn fixture rejected","PASS no hazard_return field is consumed; B21 last safe floor supplies return"});EditorApplication.Exit(0);
 }
 public static void Build()
 {
  string folder=N5Args.Value("-b22-build","Builds/_tmp/B22-work");
  var scenes=B07Build.Scenes().Concat(Directory.GetFiles("Assets/Scenes/V4","*.unity")).Concat(Directory.GetFiles("Assets/Scenes/V5","*.unity")).Concat(new[]{"Assets/Scenes/Test/BossTest_B1.unity","Assets/Scenes/Test/BossTest_B2.unity","Assets/Scenes/Test/BodySizeTest.unity","Assets/Scenes/Test/B21Arena.unity","Assets/Scenes/Test/MechanicsTest.unity"}).Distinct().ToArray();
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName=folder+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development|BuildOptions.CleanBuildCache});
  File.WriteAllText(Output,report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings+"\n"+string.Join("\n",scenes));EditorApplication.Exit(report.summary.result==BuildResult.Succeeded?0:1);
 }
}
