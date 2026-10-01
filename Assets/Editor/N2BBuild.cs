using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class N2BBuild
{
 public static void SetScenes(){EditorBuildSettings.scenes=new[]{
  new EditorBuildSettingsScene("Assets/Scenes/A01.unity",true),new EditorBuildSettingsScene("Assets/Scenes/A02.unity",true),
  new EditorBuildSettingsScene("Assets/Scenes/A03.unity",true),new EditorBuildSettingsScene("Assets/Scenes/A04.unity",true),
  new EditorBuildSettingsScene("Assets/Scenes/MovementTest.unity",false)};}
 [MenuItem("Afterglow/Open Slice")]
 public static void OpenSlice(){if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");}
 public static void Build()=>BuildCore("Validation/S1-10-build.txt");
 public static void BuildRerun()=>BuildCore("Validation/S1-10-build-02.txt");
 static void BuildCore(string evidence){
  if(File.Exists(evidence))throw new IOException("Preserve evidence");
  SetScenes();
  EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");
  Directory.CreateDirectory("Builds/Slice");
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{
   scenes=new[]{"Assets/Scenes/A01.unity","Assets/Scenes/A02.unity","Assets/Scenes/A03.unity","Assets/Scenes/A04.unity"},
   locationPathName="Builds/Slice/Afterglow-Slice.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None
  });
  File.WriteAllText(evidence,$"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Unity={Application.unityVersion}; UTC={DateTime.UtcNow:O}; startScene=A01; scenes=A01,A02,A03,A04; Windows x64 Mono; Development Build=false\n");
  if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Slice build failed");
 }
}
