using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B23Authoring
{
 public static void Configure()
 {
  var boss=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/B1/BossTuning.asset");
  boss.maxHP=22;EditorUtility.SetDirty(boss);
  var hazard=AssetDatabase.LoadAssetAtPath<B21HazardReturnSettings>("Assets/Candidates/B21HazardReturn.asset");
  if(hazard.safeGroundSeconds!=.1f||hazard.edgeMargin!=.5f||hazard.pauseSeconds!=.12f)throw new Exception("Existing hazard values changed");
  hazard.fadeSeconds=.15f;hazard.getUpTime=.45f;EditorUtility.SetDirty(hazard);
  EditorSceneManager.OpenScene("Assets/Scenes/Test/BossTest_B1.unity");
  UnityEngine.Object.FindFirstObjectByType<BossEncounter>().introDuration=1.8f;
  EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
  AssetDatabase.SaveAssets();
  File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/23/configure.txt"),"PASS B1 HP22 intro1.8s; hazard fade0.15 getUp0.45; existing values0.1/0.5/0.12 preserved\n");
  EditorApplication.Exit(0);
 }
 public static void Counts()
 {
  var rows=new System.Collections.Generic.List<string>();
  foreach(var path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
   int crumble=room.GetComponentsInChildren<CrumblePlatform>().Length,lifts=room.GetComponentsInChildren<LeverLift>().Length,enemies=room.GetComponentsInChildren<EnemyReset>().Length;
   int spikes=room.GetComponentsInChildren<DamageBlock>().Count(d=>d.name.StartsWith("hazard"));
   rows.Add($"MEASURE {room.roomId} crumble={crumble} hazards={spikes} lifts={lifts} enemies={enemies}");
   if(room.roomId=="A01"&&(crumble!=2||spikes!=1)||room.roomId=="A02"&&spikes!=1||room.roomId=="A03"&&(lifts!=1||enemies!=0||!room.GetComponentsInChildren<LeverLift>().Single().startLow)||room.roomId=="A04"&&crumble!=1)throw new Exception("Count mismatch "+room.roomId);
  }
  File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/23/counts.txt"),rows);EditorApplication.Exit(0);
 }
}
