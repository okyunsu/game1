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
}
