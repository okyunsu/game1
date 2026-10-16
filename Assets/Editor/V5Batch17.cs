using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using J=B09Json;

public sealed class V5Batch17 : IProcessSceneWithReport
{
 public int callbackOrder=>1;
 public static void Build()
 {
  string folder=N5Args.Value("-b17-build","Builds/_tmp/B17-before");
  var paths=B07Build.Scenes().Concat(Directory.GetFiles("Assets/Scenes/V4","*.unity").OrderBy(p=>p)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"}).Concat(Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p)).Select(p=>p.Replace(Path.DirectorySeparatorChar,'/')).Distinct().ToArray();
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=paths,locationPathName=Path.Combine(folder,"Afterglow.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development});
  string result=report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings;
  File.WriteAllText(N5Args.Value("-b17-build-output","Validation/_raw/B17/build.txt"),result+"\n"+string.Join("\n",paths));EditorApplication.Exit(report.summary.result==BuildResult.Succeeded?0:1);
 }
 public static void Generate()
 {
  B09Authoring.Generate("Design/v5/rooms_v5_A.json","Assets/Scenes/V5","Design/v5/world_v5_A.json");
  var rooms=J.D(J.Read(File.ReadAllText("Design/v5/rooms_v5_A.json"))["rooms"]);var rows=new List<string>{"|방|고체|발판|적|등불|가시 포고|hazard|문|봉인|벽|조각|바닥 구덩이|","|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|"};
  int totalPits=0;
  foreach(var pair in rooms)
  {
   var spec=J.D(pair.Value);EditorSceneManager.OpenScene("Assets/Scenes/V5/"+pair.Key+"_V5.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
   var transforms=room.GetComponentsInChildren<Transform>();var counts=new List<int>();
   foreach(string kind in new[]{"solid","platform","enemies","lantern","spike","hazard","doors","seal","breakable","shards"})
   {
    int expected=kind=="shards"?J.A(spec,"marks").Select(J.D).Count(m=>J.S(m,"t").Contains("조각")):kind=="lantern"||kind=="spike"?J.A(spec,"pogo").Select(J.D).Count(p=>J.S(p,"kind")==kind):J.A(spec,kind).Count;
    int actual=kind=="enemies"?room.GetComponentsInChildren<EnemyReset>().Length:kind=="doors"?room.GetComponentsInChildren<RoomExit>().Length:kind=="shards"?room.GetComponentsInChildren<DiscoveryReward>().Length:kind=="lantern"||kind=="spike"?room.GetComponentsInChildren<PogoTarget>().Count(p=>(p.GetComponent<DamageBlock>()!=null)==(kind=="spike")):transforms.Count(t=>t.name.StartsWith(kind,StringComparison.Ordinal)&&int.TryParse(t.name.Substring(kind.Length),out _));
    if(actual!=expected)throw new Exception(pair.Key+" "+kind+" expected="+expected+" actual="+actual);counts.Add(actual);
   }
   var hazards=J.A(spec,"hazard").Cast<List<object>>().ToArray();int pits=0;
   foreach(var node in J.A(spec,"solid").Cast<List<object>>())
   {
    float x=J.F(node[0]),y=J.F(node[1]),w=J.F(node[2]),h=J.F(node[3]);
    if(Mathf.Abs(y)<.001f&&h<=.4001f&&!hazards.Any(hz=>Mathf.Min(x+w,J.F(hz[0])+J.F(hz[2]))>Mathf.Max(x,J.F(hz[0]))+.001f))pits++;
    foreach(var hz in hazards)if(Mathf.Abs(y)<.001f&&h>.4001f&&Mathf.Min(x+w,J.F(hz[0])+J.F(hz[2]))>Mathf.Max(x,J.F(hz[0]))+.001f)throw new Exception("Covered trench "+pair.Key);
   }
   int expectedPits=pair.Key=="A06"?0:pair.Key=="A03"?2:1;if(pits!=expectedPits)throw new Exception("Pit count "+pair.Key);totalPits+=pits;
   rows.Add("|"+pair.Key+"|"+string.Join("|",counts)+"|"+pits+"|");
  }
  if(totalPits!=6)throw new Exception("pit total");rows.Add("PASS six scenes; all element counts match; six shallow pits; hazard trenches have no covering floor >0.4u");File.WriteAllLines("Validation/_raw/B17/T2-counts.md",rows);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 // Build-only map refresh: preserve the Assets/Maps source outside this batch's
 // allowed paths. Runtime gets the draft2 world coordinates in the delivered exe.
 public void OnProcessScene(Scene scene,BuildReport report)
 {
  if(report==null||!N5Args.Has("-b17-draft2-map"))return;
  var source=J.Read(File.ReadAllText("Design/v5/world_v5_A.json"));
  var layout=new WorldMapLayout{rooms=J.D(source["rooms"]).Select(p=>new MapRoom{id=p.Key,rect=J.A(J.D(p.Value),"rect").Select(J.F).ToArray()}).ToArray(),doors=J.A(source,"doors").Select(J.D).Select(d=>new MapDoor{id=J.S(d,"id"),a=J.S(d,"a"),b=J.S(d,"b"),edge=J.S(d,"edge"),type=J.S(d,"type"),gate=J.S(d,"gate"),at=J.F(d,"at"),span=J.A(d,"span").Select(J.F).ToArray(),stub=d.ContainsKey("stub")&&(bool)d["stub"]}).ToArray()};
  foreach(var root in scene.GetRootGameObjects())foreach(var map in root.GetComponentsInChildren<WorldMapSource>())map.layouts=map.layouts.Where(a=>a.name!="world_v5_A").Concat(new[]{new TextAsset(JsonUtility.ToJson(layout)){name="world_v5_A"}}).ToArray();
 }
}
