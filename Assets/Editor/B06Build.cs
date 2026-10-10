using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class B06Build
{
    public static void Run()
    {
        string tag=N5Args.Value("-b06-build","B06-delivery");
        if(!System.Text.RegularExpressions.Regex.IsMatch(tag,"^B06-[a-zA-Z0-9-]+$"))throw new System.ArgumentException("B06 tag only");
        var scenes=new[]{"Menu","A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06","Ending","Test/PogoTest","Test/WallJumpTest"}.Select(id=>"Assets/Scenes/"+id+".unity").Concat(B05Build.V2Ids().Select(id=>"Assets/Scenes/V2/"+id+"_V2.unity")).ToArray();
        Directory.CreateDirectory("Builds/"+tag);Directory.CreateDirectory("Validation/_raw/B06");
        var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName="Builds/"+tag+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=N5Args.Has("-b06-release")?BuildOptions.None:BuildOptions.Development});
        File.WriteAllText("Validation/_raw/B06/"+tag+"-build.txt",result.summary.result+" errors="+result.summary.totalErrors+" warnings="+result.summary.totalWarnings+" development="+!N5Args.Has("-b06-release")+"\n"+string.Join("\n",scenes));
        EditorApplication.Exit(result.summary.result==BuildResult.Succeeded?0:1);
    }
}
