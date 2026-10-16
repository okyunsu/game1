using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class B16bBuild
{
 public static void Run()
 {
  bool release=N5Args.Has("-b16-release");string folder=release?"Builds/_tmp/B16b-release":"Builds/B12-delivery";
  string evidence=N5Args.Value("-b16-build-output","Validation/_raw/B16b/build.txt");if(File.Exists(evidence))throw new IOException("Preserve "+evidence);
  var scenes=B07Build.Scenes().Concat(Directory.GetFiles("Assets/Scenes/V4","*.unity").OrderBy(p=>p)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"});
  if(!release)scenes=scenes.Concat(Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p));
  string[] paths=scenes.Select(p=>p.Replace(Path.DirectorySeparatorChar,'/')).Distinct().ToArray();
  if(!release&&paths.Count(p=>p.StartsWith("Assets/Scenes/V5/",StringComparison.Ordinal))!=6)throw new Exception("V5 build requires six scenes");
  Directory.CreateDirectory(folder);var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=paths,locationPathName=Path.Combine(folder,"Afterglow.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.CleanBuildCache|(release?BuildOptions.None:BuildOptions.Development)});
  var rows=new System.Collections.Generic.List<string>{result.summary.result+" errors="+result.summary.totalErrors+" warnings="+result.summary.totalWarnings,"Development="+!release};rows.AddRange(paths);
  bool pass=result.summary.result==BuildResult.Succeeded&&result.summary.totalErrors==0;
  if(pass&&release)
  {
   string binary=Encoding.ASCII.GetString(File.ReadAllBytes(Path.Combine(folder,"Afterglow_Data/Managed/Assembly-CSharp.dll")));
   foreach(string type in new[]{"V4WorldMap","V4MiniMap","V5Launch","V5PatrolBinding","B16V5Verifier","N5Runner"}){bool present=binary.Contains(type);rows.Add("RELEASE type="+type+" present="+present);pass&=!present;}
  }
  File.WriteAllLines(evidence,rows);EditorApplication.Exit(pass?0:1);
 }
}
