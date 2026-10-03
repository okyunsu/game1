using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class S3Build
{
    public static void Run(){string evidence="Validation/S3-B-build.txt";if(File.Exists(evidence))throw new IOException("Preserve evidence");string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05"};var scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray();foreach(var s in scenes)if(AssetDatabase.LoadAssetAtPath<SceneAsset>(s)==null)throw new Exception("Missing build scene "+s);Directory.CreateDirectory("Builds/S3-B");var r=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName="Builds/S3-B/Afterglow-S3.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});File.WriteAllText(evidence,$"{r.summary.result}; errors={r.summary.totalErrors}; warnings={r.summary.totalWarnings}; Windows x64; Unity={Application.unityVersion}\nscenes={string.Join(",",ids)}; count={ids.Length}; test scenes excluded\n");if(r.summary.result!=BuildResult.Succeeded||r.summary.totalErrors!=0)throw new Exception("S3-B build failed");}
}
