using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
public static class B09Build
{
 public static void Run()
 {
  string tag=N5Args.Value("-b09-build","B09-release");
  if(!System.Text.RegularExpressions.Regex.IsMatch(tag,"^B09-[A-Za-z0-9-]+$"))throw new System.ArgumentException("B09 tag");
  Directory.CreateDirectory("Builds/"+tag);
  var scenes=B07Build.Scenes().Concat(V4SceneMap.Rooms.Select(V4SceneMap.Path)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"}).ToArray();
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName="Builds/"+tag+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=tag=="B09-delivery"?BuildOptions.Development:BuildOptions.None});
  string evidence=N5Args.Value("-b09-build-output","Validation/_raw/B09/"+tag+"-build-"+System.DateTime.Now.ToString("yyyyMMdd-HHmmss")+".txt");
  if(File.Exists(evidence))throw new IOException("Preserve build evidence "+evidence);
  File.WriteAllText(evidence,report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
  EditorApplication.Exit(report.summary.result==BuildResult.Succeeded?0:1);
 }
}
