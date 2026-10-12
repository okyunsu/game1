using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class AuthorizedBuild
{
 public static void Run(){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-stageBuild");string stage=i>=0?args[i+1]:"S3-complete";string evidence="Validation/"+(stage=="S3-complete"?"S3-complete-v2-build":stage+"-v2-build")+".txt";if(File.Exists(evidence))throw new IOException("Preserve evidence");var ids=new List<string>{"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04"};foreach(var id in new[]{"C06","Ending"})if(File.Exists("Assets/Scenes/"+id+".unity"))ids.Add(id);string path="Builds/"+stage+"/Afterglow.exe";Directory.CreateDirectory(Path.GetDirectoryName(path));var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName=path,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});File.WriteAllText(evidence,$"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Windows x64; Unity={Application.unityVersion}\nScenes={string.Join(",",ids)}\nTests excluded. Build folder ignored by Git.\n");if(report.summary.result!=BuildResult.Succeeded||report.summary.totalErrors!=0)throw new Exception("Build failed");}
}
