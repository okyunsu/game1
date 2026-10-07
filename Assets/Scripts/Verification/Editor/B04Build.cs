using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class B04Build:IProcessSceneWithReport
{
 public int callbackOrder=>1400;
 public void OnProcessScene(Scene scene,BuildReport report){if(report==null||!N5Args.Has("-b04-av1"))return;var go=new GameObject("AV1 candidate bootstrap");SceneManager.MoveGameObjectToScene(go,scene);go.AddComponent<PresentationBootstrap>().profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>("Assets/Candidates/AV1/PresentationProfile.asset");}
 // One-off creation for this batch. Never overwrites existing assets.
 public static void Create(){string folder="Assets/Candidates/AV1";Directory.CreateDirectory(folder);Directory.CreateDirectory("Assets/Audio/AV1");if(File.Exists(folder+"/PresentationProfile.asset"))throw new IOException("Preserve profile");var p=ScriptableObject.CreateInstance<PresentationProfile>();string[] names={"jump","dash","swing","hit","hurt","death","checkpoint","ability","bossTell","victory"};p.sounds=new PresentationProfile.Sound[names.Length];
  for(int i=0;i<names.Length;i++){string path="Assets/Audio/AV1/"+names[i]+".wav";if(File.Exists(path))throw new IOException(path);float duration=i==5?.32f:i>=6?.26f:.13f;int count=(int)(44100*duration);using(var w=new BinaryWriter(File.Create(path))){w.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));w.Write(36+count*2);w.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(44100);w.Write(88200);w.Write((short)2);w.Write((short)16);w.Write(System.Text.Encoding.ASCII.GetBytes("data"));w.Write(count*2);double phase=0;for(int j=0;j<count;j++){double t=(double)j/count;double hz=180+i*55+(i==5?-120:200)*t;phase+=2*Math.PI*hz/44100;double envelope=Math.Min(1,t*30)*Math.Pow(1-t,2);w.Write((short)(Math.Sin(phase)*envelope*16000));}}AssetDatabase.ImportAsset(path);p.sounds[i]=new PresentationProfile.Sound{eventName=names[i],clip=AssetDatabase.LoadAssetAtPath<AudioClip>(path),volume=i==8?.34f:.3f};}
  AssetDatabase.CreateAsset(p,folder+"/PresentationProfile.asset");AssetDatabase.SaveAssets();EditorApplication.Exit(0);}
 public static void Run(){string tag=N5Args.Value("-b04-build","B04-main");string evidence="Validation/_raw/B04/"+tag+"-build.txt";if(File.Exists(evidence))throw new IOException(evidence);string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending","Test/PogoTest","A06","B06","C05"};var scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray();Directory.CreateDirectory("Builds/"+tag);var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName="Builds/"+tag+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.CleanBuildCache});File.WriteAllText(evidence,$"{report.summary.result} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings}\nAV1={N5Args.Has("-b04-av1")}\n"+string.Join("\n",scenes));EditorApplication.Exit(report.summary.totalErrors==0&&report.summary.result==BuildResult.Succeeded?0:1);}
}
