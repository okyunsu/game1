using UnityEditor;
using UnityEditor.SceneManagement;
[InitializeOnLoad]public static class N5Editor
{
 static N5Editor(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.EnteredEditMode&&SessionState.GetBool("N5.Editor",false)){SessionState.SetBool("N5.Editor",false);EditorApplication.Exit(SessionState.GetInt("N5.Exit",1));}};}
 public static void Run(){SessionState.SetBool("N5.Editor",true);EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
