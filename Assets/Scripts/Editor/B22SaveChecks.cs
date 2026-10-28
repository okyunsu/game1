using System;
using System.IO;
using UnityEditor;
public static class B22SaveChecks
{
 public static void Run()
 {
  if(!N5Args.Has("-save-path"))throw new Exception("Isolated slot required");
  string output=N5Args.Value("-verify-output","Validation/_raw/22/save.txt");
  if(File.Exists(output)||File.Exists(ProgressSave.FilePath))throw new IOException("Preserve evidence/slot");
  Directory.CreateDirectory(Path.GetDirectoryName(ProgressSave.FilePath));
  File.WriteAllText(ProgressSave.FilePath,"{\"version\":1,\"bossDefeated\":true,\"checkpointId\":\"CP-C04\"}");
  if(!ProgressSave.Continue()||!ProgressSave.IsBossDefeated("C06")||ProgressSave.IsBossDefeated("B1-A04"))throw new Exception("Legacy final boss migration");
  ProgressSave.Write(new ProgressData{checkpointId="CP-A03",defeatedBossIds=new[]{"B1-A04","B2-B04"}});
  if(!ProgressSave.Continue()||!ProgressSave.IsBossDefeated("B1-A04")||!ProgressSave.IsBossDefeated("B2-B04")||ProgressSave.BossDefeated)throw new Exception("Independent IDs");
  ProgressSave.Write(new ProgressData{checkpointId="CP-A03"});ProgressSave.Continue();
  if(ProgressSave.IsBossDefeated("B1-A04"))throw new Exception("Continue clears prior IDs");
  ProgressSave.NewGame();if(ProgressSave.BossDefeated||ProgressSave.IsBossDefeated("B2-B04"))throw new Exception("New game reset");
  File.WriteAllLines(output,new[]{"PASS legacy version1 save without defeatedBossIds: final C06 only","PASS ID array roundtrip: B1 and B2 independent of final/ending","PASS Continue and NewGame clear prior IDs","PASS compile errors=0; CompletionBuildVerification deferred to current-source build"});
  EditorApplication.Exit(0);
 }
}
