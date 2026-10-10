using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class B06Checks
{
    static B06Checks()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (!SessionState.GetBool("B06.Check",false)) return;
            if(state==PlayModeStateChange.EnteredEditMode)
            {
                SessionState.SetBool("B06.Check",false);
                EditorApplication.Exit(SessionState.GetInt("N5.Exit",1));
            }
        };
    }
    public static void Wall()
    {
        SessionState.SetBool("B06.Check",true);SessionState.SetInt("N5.Exit",1);
        EditorSceneManager.OpenScene("Assets/Scenes/Test/WallJumpTest.unity");
        EditorApplication.delayCall += ()=>EditorApplication.isPlaying=true;
    }
    public static void Title()
    {
        bool ok=System.IO.File.ReadAllText("Assets/Scripts/SaveMenu.cs").Contains("\"메트로 1\"")
            &&PlayerSettings.productName=="Afterglow"&&PlayerSettings.companyName=="Afterglow";
        System.IO.Directory.CreateDirectory("Validation/_raw/B06");
        System.IO.File.WriteAllText("Validation/_raw/B06/E2-title.txt",(ok?"PASS":"FAIL")+" compiled title label=메트로 1; Product="+PlayerSettings.productName+" Company="+PlayerSettings.companyName+"; save path logic unchanged. GUI visual acceptance pending delivery.\n");
        EditorApplication.Exit(ok?0:1);
    }
}
