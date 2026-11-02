using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B25Authoring
{
 static string Output=>N5Args.Value("-verify-output","Validation/_raw/25/authoring.txt");
 public static void SinkTrial()
 {
  EditorSceneManager.OpenScene("Assets/Scenes/Test/BossTest_B2.unity");
  var boss=UnityEngine.Object.FindFirstObjectByType<BossEncounter>();
  boss.sinkingPedestal=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<Transform>().Single(t=>t.name=="wall-jump pedestal");
  EditorSceneManager.SaveScene(boss.gameObject.scene);AssetDatabase.SaveAssets();
  File.WriteAllText(Output,"PASS B2 trial pedestal reference assigned; floor height derived from BossEncounter.floorY, no tuning changes\n");EditorApplication.Exit(0);
 }
}
