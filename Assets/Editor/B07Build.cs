using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B07Build:IProcessSceneWithReport
{
 public int callbackOrder=>1600;
 public static string[] Scenes()=>new[]{"Menu","A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06","Ending","Test/PogoTest","Test/WallJumpTest"}.Select(id=>"Assets/Scenes/"+id+".unity").Concat(B05Build.V2Ids().Select(id=>"Assets/Scenes/V2/"+id+"_V2.unity")).Concat(V3SceneMap.Rooms.Select(V3SceneMap.Path).Where(File.Exists)).ToArray();
 public void OnProcessScene(Scene scene,BuildReport report)
 {
  if(report==null||!N5Args.Has("-b07-build")||scene.name!="Menu")return;
  var go=new GameObject("B07 previous candidate mapping");SceneManager.MoveGameObjectToScene(go,scene);go.AddComponent<B07CandidateMapping>();
 }
 public static void Run()
 {
  string tag=N5Args.Value("-b07-build","B07-delivery");if(!System.Text.RegularExpressions.Regex.IsMatch(tag,"^B07-[a-zA-Z0-9-]+$"))throw new System.ArgumentException("B07 tag required");
  Directory.CreateDirectory("Builds/"+tag);Directory.CreateDirectory("Validation/_raw/B07");
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=Scenes(),locationPathName="Builds/"+tag+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development});
  File.WriteAllText("Validation/_raw/B07/"+tag+"-build.txt",report.summary.result+" errors="+report.summary.totalErrors+" warnings="+report.summary.totalWarnings+"\n"+string.Join("\n",Scenes()));
  EditorApplication.Exit(report.summary.result==BuildResult.Succeeded?0:1);
 }
}
