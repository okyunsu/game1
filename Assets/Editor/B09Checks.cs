using UnityEditor;
using UnityEditor.SceneManagement;
public static class B09Checks
{
 [InitializeOnLoadMethod] static void Init(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.EnteredEditMode&&SessionState.GetBool("B09.Check",false)){SessionState.SetBool("B09.Check",false);EditorApplication.Exit(SessionState.GetInt("N5.Exit",1));}};}
 public static void Measure(){SessionState.SetBool("B09.Check",true);SessionState.SetInt("N5.Exit",1);EditorSceneManager.OpenScene("Assets/Scenes/Test/WallJumpTest.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
