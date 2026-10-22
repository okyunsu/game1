using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class B20HeadroomScan
{
 public static void Run()
 {
  string output=N5Args.Value("-verify-output","Validation/_raw/B20/T3-surfaces.json");if(File.Exists(output))throw new IOException("Preserve evidence");
  var original=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab").GetComponent<BoxCollider2D>().size;
  var faces=new List<B20Headroom.Face>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p)){EditorSceneManager.OpenScene(path);faces.AddRange(B20Headroom.Scan(UnityEngine.Object.FindFirstObjectByType<RoomDefinition>(),original));}
  File.WriteAllText(output,JsonUtility.ToJson(new B20Headroom.Data{faces=faces.ToArray()},true));
  Debug.Log("MEASURE headroom faces="+faces.Count+" prefab="+original+" step=0.25 extension=0.35");EditorApplication.Exit(0);
 }
}
