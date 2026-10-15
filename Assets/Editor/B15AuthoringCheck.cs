using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class B15AuthoringCheck
{
 const string Temp="Assets/Scenes/_tmp-gen-check";
 static string Hash(byte[] b)=>BitConverter.ToString(SHA256.Create().ComputeHash(b)).Replace("-","");
 static Dictionary<string,int> Counts(string path){var scene=EditorSceneManager.OpenScene(path);var components=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Component>(true)).ToArray();return components.GroupBy(c=>c==null?"missing":c.GetType().Name).ToDictionary(g=>g.Key,g=>g.Count());}
 public static void Run()
 {
  if(Directory.Exists(Temp))throw new Exception("Preserve existing temporary output");
  Directory.CreateDirectory("Validation/_raw/B15/BL1-backup");
  var paths=Directory.GetFiles("Assets/Scenes/V4","*.unity").Concat(new[]{"Assets/Resources/V4Design.json","Assets/Resources/GuideGraph_V4.asset"}).ToArray();
  var original=paths.ToDictionary(p=>p,File.ReadAllBytes);foreach(var pair in original)File.WriteAllBytes("Validation/_raw/B15/BL1-backup/"+Path.GetFileName(pair.Key),pair.Value);
  var lines=new List<string>();bool pass=false;
  try{
   string[] ids=V4SceneMap.Rooms;var source=B09Json.D(B09Json.Read(File.ReadAllText("Design/rooms_v2.json"))["rooms"]);
   if(ids.Length!=21||!ids.OrderBy(x=>x).SequenceEqual(source.Keys.OrderBy(x=>x)))throw new Exception("Layout / room list mismatch");
   B09Authoring.Generate("Design/rooms_v2.json","Assets/Scenes/V4");
   foreach(var pair in original){var made=File.ReadAllBytes(pair.Key);bool equal=pair.Value.SequenceEqual(made);lines.Add("SHA "+pair.Key+" before="+Hash(pair.Value)+" after="+Hash(made)+" equal="+equal);if(!equal)File.WriteAllBytes("Validation/_raw/B15/BL1-backup/generated-"+Path.GetFileName(pair.Key),made);}
   if(original.Any(p=>!p.Value.SequenceEqual(File.ReadAllBytes(p.Key))))throw new Exception("Default regeneration SHA differs; originals restored");
   B09Authoring.Generate("Design/rooms_v2.json",Temp);
   lines.Add("| Room | V4 objects | Temp objects | V4 components | Temp components | all type counts equal |");lines.Add("|---|---:|---:|---:|---:|---|");
   foreach(string id in ids){var a=Counts(V4SceneMap.Path(id));var b=Counts(Temp+"/"+id+"_V4.unity");bool same=a.Count==b.Count&&a.All(p=>b.TryGetValue(p.Key,out int value)&&value==p.Value);lines.Add($"| {id} | {a["Transform"]} | {b["Transform"]} | {a.Values.Sum()} | {b.Values.Sum()} | {same} |");if(!same)throw new Exception("Element counts "+id);}
   pass=true;
  }catch(Exception e){lines.Add("FAIL "+e.Message);}
  finally{EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");foreach(var pair in original)File.WriteAllBytes(pair.Key,pair.Value);if(Directory.Exists(Temp))AssetDatabase.DeleteAsset(Temp);AssetDatabase.Refresh();}
  lines.Insert(0,(pass?"PASS":"FAIL")+" BL1 generation; default SHA equality; temp output removed");File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/B15/BL1-generation.txt"),lines);EditorApplication.Exit(pass?0:1);
 }
}
