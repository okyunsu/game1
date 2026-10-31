using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B24Authoring
{
 public static void Counts()
 {
  var rows=new List<string>();
  foreach(string id in new[]{"A01","A02","A03","A04","A05","A06"})
  {
   EditorSceneManager.OpenScene("Assets/Scenes/V5/"+id+"_V5.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
   int e4=room.GetComponentsInChildren<E4Hover>().Length;
   int e2=room.GetComponentsInChildren<EnemyReset>().Count(e=>e.prefab.GetComponent<EnemyCharger>()!=null);
   rows.Add($"MEASURE {id} E4={e4} E2={e2} rewards={room.GetComponentsInChildren<DiscoveryReward>().Length}");
   if(id=="A01"&&(e4!=1||!room.GetComponentsInChildren<Transform>().Any(t=>t.name=="platform5")))throw new Exception("A01 draft5 elements");
   if((id=="A02"||id=="A05")&&e2!=1)throw new Exception(id+" E2 count");
  }
  File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/24/counts.txt"),rows);EditorApplication.Exit(0);
 }
}
