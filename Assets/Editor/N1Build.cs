using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class N1Build
{
    [MenuItem("Afterglow/Open Movement Test")]
    public static void OpenMovementTest()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        EditorSceneManager.OpenScene("Assets/Scenes/MovementTest.unity");
    }

    public static void Build()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MovementTest.unity");
        AssetDatabase.ForceReserializeAssets(new[] { "Assets/ScriptableObjects/PlayerTuning.asset", "Assets/Prefabs/Player.prefab" });
        AssetDatabase.SaveAssets();
        Directory.CreateDirectory("Builds/N1");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MovementTest.unity" },
            locationPathName = "Builds/N1/Afterglow.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        });
        Directory.CreateDirectory("Validation");
        File.WriteAllText("Validation/N1-build.txt", $"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Unity={Application.unityVersion}; UTC={DateTime.UtcNow:O}; scene=Assets/Scenes/MovementTest.unity; Windows x64 Mono; Development Build=false");
        if (report.summary.result != BuildResult.Succeeded) throw new Exception("N1 build failed");
    }
}
