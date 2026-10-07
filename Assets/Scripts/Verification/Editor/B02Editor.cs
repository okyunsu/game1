using UnityEditor;
using UnityEditor.SceneManagement;

public static class B02Editor
{
    public static void Run()
    {
        SessionState.SetBool("N5.Editor",true);
        EditorSceneManager.OpenScene("Assets/Scenes/"+N5Args.Value("-b02-start","Menu")+".unity");
        EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;
    }
}
