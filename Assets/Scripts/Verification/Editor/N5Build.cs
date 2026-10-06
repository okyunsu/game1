using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
public static class N5Build
{
 public static void Run(){var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,"-n5-build");string tag=i>=0?a[i+1]:"N5-tools";string evidence="Validation/"+tag+"-build.txt";if(File.Exists(evidence))throw new IOException("Preserve evidence "+evidence);string buildRoot=N5Args.Value("-build-root","Builds");if(buildRoot!="Builds"&&buildRoot!="Builds/_tmp")throw new ArgumentException("Builds or Builds/_tmp only");string folder=buildRoot+"/"+tag;Directory.CreateDirectory(folder);
  string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending"};
  string initial=N5Args.Value("-n5-start-scene","Menu");if(!ids.Contains(initial))throw new ArgumentException("Unknown startup scene");
  var r=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.OrderBy(id=>id==initial?0:1).Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName=folder+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
  File.WriteAllText(evidence,$"{r.summary.result} errors={r.summary.totalErrors} warnings={r.summary.totalWarnings}\n17 scenes; default tuning/source scene layout preserved; opt-in tools inactive without flags.\n");EditorApplication.Exit(r.summary.result==BuildResult.Succeeded&&r.summary.totalErrors==0?0:1);
 }
}
