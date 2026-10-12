using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class S4PreBuild
{
 public static void Run(){const string evidence="Validation/S4-pre-build.txt";if(File.Exists(evidence))throw new IOException("Preserve evidence");string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending"};const string path="Builds/S4-pre/Afterglow-S4-pre.exe";Directory.CreateDirectory(Path.GetDirectoryName(path));var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName=path,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});File.WriteAllText(evidence,$"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Unity={Application.unityVersion}; Windows x64\nScenes={string.Join(",",ids)}\nF2R REVIEW + LOGR REVIEW; test scenes excluded; executable verifiers inert unless explicit CLI flags supplied.\n");if(report.summary.result!=BuildResult.Succeeded||report.summary.totalErrors!=0)throw new Exception("Build failed");}
}
