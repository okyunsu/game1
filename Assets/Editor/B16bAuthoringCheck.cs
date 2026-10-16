using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using J=B09Json;

public static class B16bAuthoringCheck
{
 const string Source="Design/v5/rooms_v5_A.json",World="Design/v5/world_v5_A.json",Temp="Assets/Scenes/_tmp-gen-check";
 static string Hash(byte[] value)=>BitConverter.ToString(SHA256.Create().ComputeHash(value));
 public static void GenerateV5()
 {
  var data=J.D(J.Read(File.ReadAllText(Source))["rooms"]);var world=J.Read(File.ReadAllText(World));
  int shards=J.A(J.D(data["A02"]),"marks").Select(J.D).Count(m=>J.S(m,"t").Contains("조각"));
  var arrival=J.A(J.D(data["A05"]),"spawns").Select(J.D).Single(s=>J.S(s,"id")=="A05-A03");
  var rect=J.A(J.D(J.D(world["rooms"])["A04"]),"rect");
  if(shards!=2||Mathf.Abs(J.F(arrival,"x")-3.5f)>.01f||Mathf.Abs(J.F(rect[3])-J.F(rect[1])-12)>.01f||!J.A(world,"doors").Select(J.D).Any(d=>J.S(d,"id")=="A02-A06"))throw new InvalidDataException("Await corrected design input: A02 shards2 / A05 x3.5 / A04 world height12 / A02-A06 world ID");
  B09Authoring.Generate(Source,"Assets/Scenes/V5",World);MapLayoutBuilder.Generate(World);
  string[] kinds={"solid","platform","enemies","pogo","hazard","doors","seal","breakable","pad","steam","lifts","crumble","shards","dash"};
  var rows=new List<string>{"| Room | "+string.Join(" | ",kinds)+" |","|---|"+string.Concat(kinds.Select(k=>"---:|"))};
  foreach(var pair in data)
  {
   EditorSceneManager.OpenScene(Path.Combine("Assets/Scenes/V5",pair.Key+"_V5.unity"));var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var spec=J.D(pair.Value);var counts=new List<int>();
   foreach(string kind in kinds)
   {
    int actual=kind=="enemies"?room.GetComponentsInChildren<EnemyReset>().Length+room.GetComponentsInChildren<E4Hover>().Length:kind=="pogo"?room.GetComponentsInChildren<PogoTarget>().Length:kind=="doors"?room.GetComponentsInChildren<RoomExit>().Length:kind=="steam"?room.GetComponentsInChildren<SteamVent>().Length:kind=="lifts"?room.GetComponentsInChildren<LeverLift>().Length:kind=="crumble"?room.GetComponentsInChildren<CrumblePlatform>().Length:kind=="shards"?room.GetComponentsInChildren<DiscoveryReward>().Length:kind=="dash"?room.GetComponentsInChildren<DashPickup>().Length:room.GetComponentsInChildren<Transform>().Count(t=>t.name.StartsWith(kind,StringComparison.Ordinal)&&int.TryParse(t.name.Substring(kind.Length),out _));
    int expected=kind=="shards"?J.A(spec,"marks").Select(J.D).Count(m=>J.S(m,"t").Contains("조각")):kind=="dash"?J.A(spec,"marks").Select(J.D).Count(m=>J.S(m,"t").Contains("대시 사당")):J.A(spec,kind).Count;
    if(actual!=expected)throw new Exception("Element count "+pair.Key+" "+kind+" expected="+expected+" actual="+actual);counts.Add(actual);
   }
   rows.Add("| "+pair.Key+" | "+string.Join(" | ",counts)+" |");
   foreach(var node in J.A(spec,"doors").Select(J.D).Where(d=>d.ContainsKey("stub")&&(bool)d["stub"]))if(room.GetComponentsInChildren<RoomExit>().Single(e=>e.exitId==J.S(node,"id")).enabled)throw new Exception("Stub open "+pair.Key);
  }
  File.WriteAllLines("Validation/_raw/B16b/T2-counts.md",rows);AssetDatabase.SaveAssets();Debug.Log("PASS V5 six scenes; element counts; stub4 closed");EditorApplication.Exit(0);
 }
 public static void Verify()
 {
  string stage=N5Args.Value("-b16-stage","T1b"),output=Path.Combine("Validation/_raw/B16b",stage+"-generation.txt");
  if(File.Exists(output))throw new IOException("Preserve "+output);
  if(Directory.Exists(Temp))throw new IOException("Preserve existing temporary output");
  var original=Directory.GetFiles("Assets/Scenes/V4","*",SearchOption.AllDirectories).Concat(Directory.GetFiles("Assets/Resources","*",SearchOption.AllDirectories)).ToDictionary(p=>p,File.ReadAllBytes);
  var rows=new List<string>();bool pass=false;
  try
  {
   B09Authoring.Generate("Design/rooms_v2.json","Assets/Scenes/V4");
   foreach(var pair in original){byte[] made=File.ReadAllBytes(pair.Key);bool equal=pair.Value.SequenceEqual(made);rows.Add("SHA "+pair.Key+" equal="+equal+" before="+Hash(pair.Value)+" after="+Hash(made));if(!equal){string backup=Path.Combine("Validation/_raw/B16b",stage+"-changed",pair.Key);Directory.CreateDirectory(Path.GetDirectoryName(backup));File.WriteAllBytes(backup,made);}}
   if(original.Any(p=>!p.Value.SequenceEqual(File.ReadAllBytes(p.Key))))throw new Exception("V4 default regeneration SHA differs; originals restored");
   B09Authoring.Generate(Source,Temp,World);
   var data=J.D(J.Read(File.ReadAllText(Source))["rooms"]);
   var scenes=Directory.GetFiles(Temp,"*.unity");if(scenes.Length!=data.Count)throw new Exception("Temporary room count");
   foreach(string scene in scenes)
   {
    EditorSceneManager.OpenScene(scene);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var spec=J.D(data[room.roomId]);
    if(room.GetComponentsInChildren<RoomExit>().Length!=J.A(spec,"doors").Count)throw new Exception("Door count "+room.roomId);
    foreach(var exit in room.GetComponentsInChildren<RoomExit>())rows.Add("DOOR "+room.roomId+" "+exit.exitId+" enabled="+exit.enabled+" target="+exit.destinationRoomId);
    if(stage.StartsWith("T1c",StringComparison.Ordinal))
    {
     int expected=J.A(spec,"enemies").Select(J.D).Count(e=>J.A(e,"patrol").Count==2);
     var bindings=room.GetComponentsInChildren<Component>().Where(c=>c.GetType().Name=="V5PatrolBinding").ToArray();
     if(bindings.Length!=expected)throw new Exception("Patrol binding count "+room.roomId);
     foreach(var binding in bindings){var so=new SerializedObject(binding);if(so.FindProperty("leftPoint").objectReferenceValue==null||so.FindProperty("rightPoint").objectReferenceValue==null)throw new Exception("Missing patrol endpoints");}
     rows.Add("PATROL "+room.roomId+" expected="+expected+" connected="+bindings.Length+" transforms="+bindings.Length*2);
    }
   }
   rows.Add("PASS six temporary rooms; default SHA equality");pass=true;
  }
  catch(Exception e){rows.Add("FAIL "+e);}
  finally
  {
   EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");
   foreach(var pair in original)if(!pair.Value.SequenceEqual(File.ReadAllBytes(pair.Key)))File.WriteAllBytes(pair.Key,pair.Value);
   if(Directory.Exists(Temp))AssetDatabase.DeleteAsset(Temp);AssetDatabase.Refresh();
  }
  rows.Insert(0,(pass?"PASS":"FAIL")+" "+stage+" actual=Editor");File.WriteAllLines(output,rows);EditorApplication.Exit(pass?0:1);
 }
}
