using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class Q6NightBuild : IProcessSceneWithReport
{
 public int callbackOrder=>1000;
 static string candidate;
 public void OnProcessScene(Scene scene,BuildReport report){
  if(report==null||candidate==null)return;
  var tuning=AssetDatabase.LoadAssetAtPath<FeedbackTuning>("Assets/Resources/Q6/Feedback-"+candidate+".asset");
  foreach(var root in scene.GetRootGameObjects()){
   foreach(var fx in root.GetComponentsInChildren<AttackFeedback>(true))fx.tuning=tuning;
   foreach(var fx in root.GetComponentsInChildren<HitFeedback>(true))fx.tuning=tuning;
  }
 }
 public static void Run(){
  var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-candidate");candidate=i>=0?args[i+1]:null;
  if(candidate is not ("A" or "B" or "C"))throw new Exception("Expected Q6 candidate A/B/C");
  string evidence="Validation/Q6-"+candidate+"-build-correction.txt";if(File.Exists(evidence))throw new IOException("Preserve evidence");
  Directory.CreateDirectory("Assets/Resources/Q6");AssetDatabase.Refresh();
  string assetPath="Assets/Resources/Q6/Feedback-"+candidate+".asset";
  if(AssetDatabase.LoadAssetAtPath<FeedbackTuning>(assetPath)!=null)throw new IOException("Preserve candidate asset");
  var original=AssetDatabase.LoadAssetAtPath<FeedbackTuning>("Assets/ScriptableObjects/FeedbackTuning.asset");
  var copy=UnityEngine.Object.Instantiate(original);copy.name="Feedback-"+candidate;
  if(candidate=="B"){copy.slashAlpha=.9f;copy.hitFlashSeconds=.14f;copy.hpPipSize=.24f;copy.deathPopScale=2;}
  if(candidate=="C"){copy.slashAlpha=.5f;copy.hitFlashSeconds=.07f;copy.hpPipSize=.14f;copy.deathPopScale=1.3f;}
  AssetDatabase.CreateAsset(copy,assetPath);AssetDatabase.SaveAssets();
  string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending"};
  Directory.CreateDirectory("Builds/Q6-"+candidate);
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName="Builds/Q6-"+candidate+"/Afterglow-Q6-"+candidate+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None,extraScriptingDefines=new[]{"Q6_"+candidate}});
  File.WriteAllText(evidence,$"{report.summary.result} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings}\nCandidate {candidate}: slashAlpha={copy.slashAlpha}; hitFlashSeconds={copy.hitFlashSeconds}; hpPipSize={copy.hpPipSize}; deathPopScale={copy.deathPopScale}\nBuild scene copies remapped by IProcessSceneWithReport; source scenes/prefabs/default asset untouched.\n");candidate=null;
  EditorApplication.Exit(report.summary.result==BuildResult.Succeeded&&report.summary.totalErrors==0?0:1);
 }
}
