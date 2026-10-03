using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class S3FinalAuthoring
{
    public static void B02()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/B02.unity");
        var reset = UnityEngine.Object.FindFirstObjectByType<EnemyReset>();
        if (Mathf.Abs(reset.transform.position.x - 24) > .001f) throw new Exception("Unexpected E2 placement");
        var p = reset.transform.position; p.x = 17; reset.transform.position = p;
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
}
