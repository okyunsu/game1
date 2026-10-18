using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using J=B09Json;
public static class V5Batch18
{
 public static void Clearance()
 {
  if(!N5Args.Has("-verify-b18-clearance"))throw new Exception("Explicit opt-in required");
  string output=N5Args.Value("-verify-output","Validation/_raw/B18/clearance.txt");if(File.Exists(output))throw new IOException("Preserve "+output);
  var shape=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab").GetComponent<BoxCollider2D>();var rows=new List<string>{"MEASURE player prefab size="+shape.size+" step=0.25 extraHeight=0.6 edgeExtension="+shape.size.x/2};
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p)){EditorSceneManager.OpenScene(path);rows.AddRange(V5B18Clearance.Scan(UnityEngine.Object.FindFirstObjectByType<RoomDefinition>(),shape.size));}
  File.WriteAllLines(output,rows);Debug.Log("MEASURE clearance candidates="+rows.Count(l=>l.StartsWith("FAIL",StringComparison.Ordinal)));EditorApplication.Exit(rows.Any(l=>l.StartsWith("FAIL",StringComparison.Ordinal))?1:0);
 }
 public static void Generate()
 {
  B09Authoring.Generate("Design/v5/rooms_v5_A.json","Assets/Scenes/V5","Design/v5/world_v5_A.json");
  var rooms=J.D(J.Read(File.ReadAllText("Design/v5/rooms_v5_A.json"))["rooms"]);var rows=new List<string>{"|방|고체|발판|작은 발판|적|등불|가시 포고|hazard|문|봉인|벽|조각|구덩이|","|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|"};
  foreach(var pair in rooms)
  {
   var spec=J.D(pair.Value);EditorSceneManager.OpenScene("Assets/Scenes/V5/"+pair.Key+"_V5.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var ts=room.GetComponentsInChildren<Transform>();var counts=new List<int>();
   foreach(string kind in new[]{"solid","platform","small","enemies","lantern","spike","hazard","doors","seal","breakable","shards","pits"})
   {
    int expected=kind=="small"?J.A(spec,"platform").Cast<List<object>>().Count(a=>J.F(a[2])<=2.5f):kind=="shards"?J.A(spec,"marks").Select(J.D).Count(m=>J.S(m,"t").Contains("조각")):kind=="lantern"||kind=="spike"?J.A(spec,"pogo").Select(J.D).Count(p=>J.S(p,"kind")==kind):kind=="pits"?(pair.Key=="A06"?0:pair.Key=="A03"?2:1):J.A(spec,kind).Count;
    int actual=kind=="small"?room.GetComponentsInChildren<BoxCollider2D>().Count(c=>c.name.StartsWith("platform")&&c.bounds.size.x<=2.5001f):kind=="enemies"?room.GetComponentsInChildren<EnemyReset>().Length:kind=="doors"?room.GetComponentsInChildren<RoomExit>().Length:kind=="shards"?room.GetComponentsInChildren<DiscoveryReward>().Length:kind=="lantern"||kind=="spike"?room.GetComponentsInChildren<PogoTarget>().Count(p=>(p.GetComponent<DamageBlock>()!=null)==(kind=="spike")):kind=="pits"?room.GetComponentsInChildren<BoxCollider2D>().Count(c=>c.name.StartsWith("solid")&&Mathf.Abs(c.bounds.min.y)<.001f&&c.bounds.size.y<=.4001f&&!room.GetComponentsInChildren<DamageBlock>().Any(h=>V5B18Clearance.Positive(new Bounds(new Vector3(c.bounds.center.x,h.transform.position.y),new Vector3(c.bounds.size.x,1)),h.GetComponent<Collider2D>().bounds))):ts.Count(t=>t.name.StartsWith(kind,StringComparison.Ordinal)&&int.TryParse(t.name.Substring(kind.Length),out _));
    if(actual!=expected)throw new Exception(pair.Key+" "+kind+" expected="+expected+" actual="+actual);counts.Add(actual);
   }
   rows.Add("|"+pair.Key+"|"+string.Join("|",counts)+"|");
  }
  File.WriteAllLines("Validation/_raw/B18/T3-counts.md",rows);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
