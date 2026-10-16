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
