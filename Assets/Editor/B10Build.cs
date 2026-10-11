using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
public static class B10Build
{
 public static void Run()
 {
  string tag=N5Args.Value("-b10-build","B10-release");
  if(!System.Text.RegularExpressions.Regex.IsMatch(tag,"^B10-[A-Za-z0-9-]+$"))throw new System.ArgumentException("B10 tag");
  Directory.CreateDirectory("Builds/"+tag);
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=B07Build.Scenes().Concat(V4SceneMap.Rooms.Select(V4SceneMap.Path)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"}).ToArray(),locationPathName="Builds/"+tag+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=tag=="B10-delivery"?BuildOptions.Development:BuildOptions.None});
  string path=N5Args.Value("-b10-build-output","Validation/_raw/B10/"+tag+"-"+System.DateTime.Now.ToString("yyyyMMdd-HHmmss")+".txt");
  if(File.Exists(path))throw new IOException("Preserve "+path);
  File.WriteAllText(path,report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings);
  EditorApplication.Exit(report.summary.result==BuildResult.Succeeded?0:1);
 }
}
