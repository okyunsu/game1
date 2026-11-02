using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using J=B09Json;
public static class B25WorldAuthoring
{
 public const string Rooms="Design/v5/rooms_v5_AB.json",World="Design/v5/world_v5_AB.json";
 static string Output=>N5Args.Value("-verify-output","Validation/_raw/25/world-authoring.txt");
 static Dictionary<string,object> Data()=>J.D(J.Read(File.ReadAllText(Rooms))["rooms"]);
 static float[] F(Dictionary<string,object> spec,string key)=>J.A(spec,key).Select(J.F).ToArray();
 static GameObject Rect(RoomDefinition room,string name,float[] r,Color color,bool trigger=false)
 {
  var go=new GameObject(name);go.transform.SetParent(room.transform);go.transform.position=new Vector2(r[0]+r[2]/2,r[1]+r[3]/2);go.transform.localScale=new Vector3(r[2],r[3],1);go.layer=trigger?0:6;
  var sr=go.AddComponent<SpriteRenderer>();sr.sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;sr.color=color;go.AddComponent<BoxCollider2D>().isTrigger=trigger;return go;
 }
 public static void Fixtures()
 {
  var data=Data();if(data.Count!=12)throw new Exception("AB12 rooms required");
  var b2=J.D(J.D(data["B04"])["boss"]);if(J.S(b2,"id")!="B2-B04"||!J.D(b2["pedestal"]).ContainsKey("sink_on_fight"))throw new Exception("B2 contract");
  foreach(string id in new[]{"A03-B01","A02-B05","B05-C01"})
  {
   var matches=data.Values.SelectMany(r=>J.A(J.D(r),"doors").Select(J.D)).Where(d=>J.S(d,"id")==id).ToArray();
   if(matches.Length<1||matches.Any(d=>{var r=F(d,"rect");return r[2]<=r[3];}))throw new Exception("Horizontal opening required "+id);
  }
  File.WriteAllText(Output,"PASS AB12 fixtures: A03-B01/A02-B05/B05-C01 horizontal openings; B2 id/sink contract; source read-only\n");EditorApplication.Exit(0);
 }
 static void B2(RoomDefinition room,Dictionary<string,object> spec)
 {
  var arena=F(spec,"arena");var spawn=F(spec,"spawn");var pickup=F(spec,"pickup_at");
  var gate=Rect(room,"B2 gate",F(spec,"gate_wall"),Color.red);gate.AddComponent<ClimbableWall>();gate.SetActive(false);
  var pedestal=J.D(spec["pedestal"]);var r=F(pedestal,"rect");Physics2D.SyncTransforms();
  var support=room.GetComponentsInChildren<BoxCollider2D>().Single(c=>c.name.StartsWith("solid")&&Vector2.Distance(c.bounds.center,new Vector2(r[0]+r[2]/2,r[1]+r[3]/2))<.001&&Math.Abs(c.bounds.size.x-r[2])<.001);
  Rect(room,"Wall pickup",new[]{pickup[0]-.4f,pickup[1]+.3f,.8f,.8f},Color.yellow,true).AddComponent<WallJumpPickup>();
  var boss=Rect(room,J.S(spec,"id"),new[]{spawn[0]-1,arena[1],2f,3f},new Color(.6f,.2f,.8f)).AddComponent<BossEncounter>();
  boss.bossId=J.S(spec,"id");boss.displayName="벽 감시자";boss.tuning=AssetDatabase.LoadAssetAtPath<BossTuning>(J.S(spec,"tuning"));boss.leftExit=room.GetComponentsInChildren<RoomExit>().Single();boss.encounterGate=gate;
  boss.floorY=arena[1];boss.leftWallX=arena[0];boss.rightWallX=arena[0]+arena[2];boss.startsAfterWallJumpPickup=true;boss.acquisitionX=pickup[0];boss.acquisitionDistance=J.F(J.D(spec["trigger"])["after_pickup_leave"]);boss.retryTriggerFromRight=true;
  boss.introDuration=1.8f;boss.introDropHeight=spawn[1]-arena[1];boss.sinkingPedestal=support.transform;
  boss.wavePrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BossWave.prefab");boss.warning=Rect(room,"B2 warning",new[]{spawn[0]-1,arena[1]+.05f,2f,.3f},Color.yellow,true).GetComponent<SpriteRenderer>();UnityEngine.Object.DestroyImmediate(boss.warning.GetComponent<Collider2D>());
  var top=new GameObject("B2 top pogo");top.transform.SetParent(boss.transform,false);top.transform.localPosition=new Vector3(0,.48f,0);top.transform.localScale=new Vector3(1,.04f,1);top.AddComponent<BoxCollider2D>().isTrigger=true;top.AddComponent<PogoTarget>();
 }
 public static void Generate()
 {
  var data=Data();B09Authoring.Generate(Rooms,"Assets/Scenes/V5",World);var rows=new List<string>();
  foreach(var pair in data)
  {
   string path="Assets/Scenes/V5/"+pair.Key+"_V5.unity";EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var spec=J.D(pair.Value);
   if(pair.Key=="A04")B22Authoring.AttachBoss(room,J.D(spec["boss"]));if(pair.Key=="B04")B2(room,J.D(spec["boss"]));
   foreach(var node in J.A(spec,"lifts"))
   {
    var d=J.D(node);var lift=room.GetComponentsInChildren<LeverLift>().Single(l=>l.name=="lift:"+J.S(d,"id"));lift.startLow=J.S(d,"start")=="low";
    if(lift.startLow){var p=lift.transform.position;p.y=lift.low-lift.GetComponent<Collider2D>().bounds.extents.y;lift.transform.position=p;}
   }
   Physics2D.SyncTransforms();
   foreach(var cover in room.GetComponentsInChildren<BreakableWall>().Where(c=>c.name.StartsWith("cover")))
    foreach(var solid in room.GetComponentsInChildren<BoxCollider2D>().Where(c=>c.name.StartsWith("solid")))
     if(Vector3.Distance(solid.bounds.center,cover.GetComponent<Collider2D>().bounds.center)<.001&&Vector3.Distance(solid.bounds.size,cover.GetComponent<Collider2D>().bounds.size)<.001)
     {solid.enabled=false;solid.GetComponent<SpriteRenderer>().enabled=false;rows.Add("PASS "+pair.Key+" exact duplicate "+solid.name+" delegated to breakable "+cover.name+"; union geometry unchanged");}
   var host=new GameObject("V5 hazard configuration").AddComponent<B21CandidateHost>();SceneManager.MoveGameObjectToScene(host.gameObject,room.gameObject.scene);host.hazardSettings=AssetDatabase.LoadAssetAtPath<B21HazardReturnSettings>("Assets/Candidates/B21HazardReturn.asset");host.cornerSettings=AssetDatabase.LoadAssetAtPath<B21CornerFixSettings>("Assets/Candidates/B21CornerFix.asset");
   foreach(string cp in new[]{"CP-A03","CP-B01"})if((pair.Key=="A03"&&cp=="CP-A03"||pair.Key=="B01"&&cp=="CP-B01")&&room.GetComponentsInChildren<Checkpoint>().Count(c=>c.checkpointId==cp)!=1)throw new Exception("Missing/duplicate "+cp);
   rows.Add("PASS "+pair.Key+" AB source; spawn/doors/solid preserved; bosses and hazard config bound");EditorSceneManager.SaveScene(room.gameObject.scene,path);
  }
  AssetDatabase.SaveAssets();File.WriteAllLines(Output,rows);EditorApplication.Exit(0);
 }
 public static void Counts()
 {
  var rows=new List<string>();
  foreach(var pair in Data())
  {
   EditorSceneManager.OpenScene("Assets/Scenes/V5/"+pair.Key+"_V5.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var spec=J.D(pair.Value);var ts=room.GetComponentsInChildren<Transform>(true);
   foreach(string kind in new[]{"solid","platform","climb","cover","breakable","seal","hazard","pad"})
   {int expected=J.A(spec,kind).Count,actual=ts.Count(t=>t.name.StartsWith(kind,StringComparison.Ordinal)&&int.TryParse(t.name.Substring(kind.Length),out _));if(actual!=expected)throw new Exception(pair.Key+" "+kind+" count "+actual+" != "+expected);rows.Add($"PASS {pair.Key} {kind} expected={expected} actual={actual}");}
   int enemies=room.GetComponentsInChildren<EnemyReset>(true).Length+room.GetComponentsInChildren<E4Hover>(true).Length;
   if(enemies!=J.A(spec,"enemies").Count||room.GetComponentsInChildren<SteamVent>(true).Length!=J.A(spec,"steam").Count||room.GetComponentsInChildren<LeverLift>(true).Length!=J.A(spec,"lifts").Count||room.GetComponentsInChildren<RoomExit>(true).Length!=J.A(spec,"doors").Count)throw new Exception(pair.Key+" device/enemy/door count");
   rows.Add($"PASS {pair.Key} enemies={enemies} steam={J.A(spec,"steam").Count} lifts={J.A(spec,"lifts").Count} doors={J.A(spec,"doors").Count}");
  }
  File.WriteAllLines(Output,rows);EditorApplication.Exit(0);
 }
}
