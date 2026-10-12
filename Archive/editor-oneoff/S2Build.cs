using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class S2Build
{
 public static void SaveCheck()=>Build("Validation/S2-04-save-build.txt","Builds/S2-save-check/Afterglow-S2.exe");
 public static void SaveCheckRerun()=>Build("Validation/S2-04-save-build-rerun.txt","Builds/S2-save-check/Afterglow-S2.exe");
 public static void Night()=>Build("Validation/S2-night-build.txt","Builds/S2-night/Afterglow-S2.exe");
 public static void NightRerun()=>Build("Validation/S2-night-build-rerun.txt","Builds/S2-night/Afterglow-S2.exe");
 public static void Day()=>Build("Validation/S2-day-build.txt","Builds/S2-day/Afterglow-S2.exe");
 public static void DayRerun()=>Build("Validation/S2-day-build-rerun.txt","Builds/S2-day/Afterglow-S2.exe");
 static void Build(string evidence,string destination){if(File.Exists(evidence))throw new IOException("Preserve evidence");var scenes=new[]{"A01","A02","A03","A04"}.ToList();if(AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/A05.unity")!=null)scenes.Add("A05");if(AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/B01.unity")!=null)scenes.Add("B01");if(AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/SaveMenu.cs")!=null)scenes.Insert(0,"Menu");Directory.CreateDirectory(Path.GetDirectoryName(destination));var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes.Select(s=>"Assets/Scenes/"+s+".unity").ToArray(),locationPathName=destination,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});File.WriteAllText(evidence,$"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; scenes={string.Join(",",scenes)}; start={scenes[0]}; Windows x64; Unity={Application.unityVersion}\n");if(report.summary.result!=BuildResult.Succeeded||report.summary.totalErrors!=0)throw new Exception("S2 build failed");}
}
