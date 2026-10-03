using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class UICheck
{
    static UICheck(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("UI.Check",false))return;if(s==PlayModeStateChange.EnteredPlayMode){var runner=new GameObject("UI Editor verification").AddComponent<UIVerification>();runner.output=Nightly.Path("U1-ui");runner.finished=pass=>{SessionState.SetInt("UI.Exit",pass?0:1);EditorApplication.isPlaying=false;};}if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("UI.Check",false);EditorApplication.Exit(SessionState.GetInt("UI.Exit",1));}};}
    public static void Run(){if(File.Exists(Nightly.Path("U1-ui")))throw new IOException("Preserve evidence");SessionState.SetBool("UI.Check",true);EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
