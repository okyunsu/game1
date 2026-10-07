using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B02Build:IProcessSceneWithReport
{
    public int callbackOrder=>1200;
    public void OnProcessScene(Scene scene,BuildReport report)
    {
        if(report==null||!N5Args.Has("-b02-candidate"))return;
        var tuning=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/B2/BossTuning.asset");
        if(tuning==null)throw new BuildFailedException("B2 candidate unavailable");
        foreach(var root in scene.GetRootGameObjects())
            foreach(var boss in root.GetComponentsInChildren<BossEncounter>(true))boss.tuning=tuning;
    }
    public static void Run()
    {
        string tag=N5Args.Value("-b02-build","B02-main");
        string evidence="Validation/_raw/B02/"+tag+"-build.txt";
        if(File.Exists(evidence))throw new IOException("Preserve evidence "+evidence);
        string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending","Test/PogoTest"};
        var scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToList();
        foreach(string id in new[]{"A06","B06","C05"})
            if(File.Exists("Assets/Scenes/"+id+".unity")&&!File.Exists("Validation/_raw/B02/"+id+"-BLOCKED"))scenes.Add("Assets/Scenes/"+id+".unity");
        string folder="Builds/"+tag;Directory.CreateDirectory(folder);
        var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes.ToArray(),locationPathName=folder+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
        File.WriteAllText(evidence,$"{result.summary.result} errors={result.summary.totalErrors} warnings={result.summary.totalWarnings}\n"+string.Join("\n",scenes)+"\nCandidate="+N5Args.Has("-b02-candidate"));
        EditorApplication.Exit(result.summary.result==BuildResult.Succeeded&&result.summary.totalErrors==0?0:1);
    }
}
