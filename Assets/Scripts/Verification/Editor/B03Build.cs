using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B03Build:IProcessSceneWithReport
{
 public int callbackOrder=>1300;
 public void OnProcessScene(Scene scene,BuildReport report){if(report==null)return;string id=N5Args.Value("-b03-candidate","");if(id.Length==0)return;var tuning=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/"+id+"/BossTuning.asset");if(tuning==null)throw new BuildFailedException(id);foreach(var root in scene.GetRootGameObjects())foreach(var boss in root.GetComponentsInChildren<BossEncounter>(true))boss.tuning=tuning;}
 public static void Candidates(){
  foreach(string id in new[]{"B2","B2-fast","B2-slow"}){
   string folder="Assets/BossCandidates/"+id;Directory.CreateDirectory(folder);string path=folder+"/BossTuning.asset";if(File.Exists(path))throw new IOException("Preserve "+path);
   var tuning=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/ScriptableObjects/BossTuning.asset"));tuning.name="BossTuning";tuning.extendedPatterns=true;
   float tell=id=="B2-fast"?.8f:id=="B2-slow"?1.2f:1;float speed=id=="B2-fast"?1.2f:id=="B2-slow"?.8f:1;
   tuning.meleeTell*=tell;tuning.waveTell*=tell;tuning.leapTell*=tell;tuning.chargeTell*=tell;tuning.walkSpeed*=speed;tuning.waveSpeed*=speed;tuning.chargeSpeed*=speed;tuning.leapFlight/=speed;
   AssetDatabase.CreateAsset(tuning,path);
  }AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void Run(){string tag=N5Args.Value("-b03-build","B03-main");string evidence="Validation/_raw/B03/"+tag+"-build.txt";if(File.Exists(evidence))throw new IOException(evidence);
  string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending","Test/PogoTest","A06","B06","C05"};var scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").Where(File.Exists).ToArray();string folder="Builds/"+tag;Directory.CreateDirectory(folder);
  var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName=folder+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.CleanBuildCache});File.WriteAllText(evidence,$"{result.summary.result} errors={result.summary.totalErrors} warnings={result.summary.totalWarnings}\nCandidate={N5Args.Value("-b03-candidate","basic")}\n"+string.Join("\n",scenes));EditorApplication.Exit(result.summary.result==BuildResult.Succeeded&&result.summary.totalErrors==0?0:1);
 }
 public static void Editor(){SessionState.SetBool("N5.Editor",true);EditorSceneManager.OpenScene("Assets/Scenes/"+N5Args.Value("-b03-start","Menu")+".unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
