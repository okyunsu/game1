using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
public static class Q7NightBuild
{
 public static void Run(){string evidence="Validation/Q7-capture-build.txt";if(File.Exists(evidence))throw new IOException("Preserve evidence");
  string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending"};Directory.CreateDirectory("Builds/Q7-capture");
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName="Builds/Q7-capture/Afterglow-Q7.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
  File.WriteAllText(evidence,$"{report.summary.result} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings}\nDefault tuning and source scenes unchanged; capture script inert without -capture.\n");EditorApplication.Exit(report.summary.result==BuildResult.Succeeded&&report.summary.totalErrors==0?0:1);
 }
}
