using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class V5Batch20
{
 public static void Compile(){EditorApplication.Exit(0);}
 public static void Build()
 {
  bool release=N5Args.Has("-b20-release");
  string folder=N5Args.Value("-b20-folder","Builds/_tmp/B20-measure");
  if(!folder.StartsWith("Builds/",StringComparison.Ordinal))throw new ArgumentException("Builds folder required");
  string output=N5Args.Value("-verify-output","Validation/_raw/B20/build.txt");
  if(File.Exists(output))throw new IOException("Preserve "+output);
  var scenes=B07Build.Scenes().Concat(Directory.GetFiles("Assets/Scenes/V4","*.unity").OrderBy(p=>p)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"});
  if(!release)scenes=scenes.Concat(Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p));
  if(!release&&File.Exists("Assets/Scenes/Test/BodySizeTest.unity"))scenes=scenes.Concat(new[]{"Assets/Scenes/Test/BodySizeTest.unity"});
  string[] paths=scenes.Select(p=>p.Replace(Path.DirectorySeparatorChar,'/')).Distinct().ToArray();
  Directory.CreateDirectory(folder);
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=paths,locationPathName=folder+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=release?BuildOptions.None:BuildOptions.Development});
  var rows=new System.Collections.Generic.List<string>{report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings,"Development="+!release};
  bool pass=report.summary.result==BuildResult.Succeeded&&report.summary.totalErrors==0;
  if(pass&&release)
  {
   string binary=System.Text.Encoding.ASCII.GetString(File.ReadAllBytes(folder+"/Afterglow_Data/Managed/Assembly-CSharp.dll"));
   foreach(string type in new[]{"BodySizeOverride","BodySizeBootstrap","BodySizeCourse","B20Probe","B20BodyProbe","B20LaunchProbe","B16V5Verifier"}){bool found=binary.Contains(type);rows.Add("RELEASE type="+type+" present="+found);pass&=!found;}
  }
  rows.AddRange(paths);File.WriteAllLines(output,rows);EditorApplication.Exit(pass?0:1);
 }
}
