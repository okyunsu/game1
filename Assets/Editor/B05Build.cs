using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class B05Build:IProcessSceneWithReport
{
 public int callbackOrder=>1500;
 public void OnProcessScene(Scene scene,BuildReport report){if(report==null||!N5Args.Has("-b05-v2"))return;var ids=new[]{"A01","A02","A03","A04","A05","A06"};if(scene.name=="Menu"){var go=new GameObject("V2 build mapping");SceneManager.MoveGameObjectToScene(go,scene);go.AddComponent<V2SceneMap>().rooms=ids;}
 foreach(var root in scene.GetRootGameObjects())foreach(var exit in root.GetComponentsInChildren<RoomExit>(true))if(ids.Contains(exit.destinationRoomId))exit.destinationScenePath="Assets/Scenes/V2/"+exit.destinationRoomId+"_V2.unity";
 }
 public static void Run(){string tag=N5Args.Value("-b05-build","B05-V2");string[] ids={"Menu","A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06","Ending","Test/PogoTest"};var scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToList();if(N5Args.Has("-b05-v2"))scenes.AddRange(Enumerable.Range(1,6).Select(n=>"Assets/Scenes/V2/A"+n.ToString("D2")+"_V2.unity"));if(N5Args.Has("-b05-v2"))scenes.Add("Assets/Scenes/V2/SkinTest_V2.unity");Directory.CreateDirectory("Builds/"+tag);var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes.ToArray(),locationPathName="Builds/"+tag+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});File.WriteAllText("Validation/_raw/B05/"+tag+"-build.txt",result.summary.result+" errors="+result.summary.totalErrors+"\n"+string.Join("\n",scenes));EditorApplication.Exit(result.summary.result==BuildResult.Succeeded?0:1);}
}

