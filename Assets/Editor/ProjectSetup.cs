using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectSetup
{
    static double started;
    static ProjectSetup()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (!SessionState.GetBool("SetupSmoke", false)) return;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                started = EditorApplication.timeSinceStartup;
                EditorApplication.update += Tick;
            }
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                SessionState.SetBool("SetupSmoke", false);
                File.WriteAllText("Logs/S1-01-play.txt", "PASS: Unity " + Application.unityVersion + " entered Play Mode, ran for 2 seconds, returned to Edit Mode.");
                EditorApplication.Exit(0);
            }
        };
    }

    static void Tick()
    {
        if (EditorApplication.timeSinceStartup - started < 2) return;
        EditorApplication.update -= Tick;
        EditorApplication.isPlaying = false;
    }

    public static void BuildAndPlay()
    {
        // Let Editor startup callbacks create the search index before entering Play Mode.
        var readyAt = EditorApplication.timeSinceStartup + 3;
        EditorApplication.CallbackFunction wait = null;
        wait = () =>
        {
            if (EditorApplication.timeSinceStartup < readyAt || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            EditorApplication.update -= wait;
            BuildAndPlayAfterStartup();
        };
        EditorApplication.update += wait;
    }

    static void BuildAndPlayAfterStartup()
    {
        EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode2D;
        EditorSettings.serializationMode = SerializationMode.ForceText;
        PlayerSettings.companyName = "Afterglow";
        PlayerSettings.productName = "Afterglow";
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 720;
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        camera.tag = "MainCamera";
        camera.transform.position = new Vector3(0, 0, -10);
        camera.GetComponent<Camera>().orthographic = true;
        camera.GetComponent<Camera>().orthographicSize = 5.5f;
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Boot.unity");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Menu.unity", true);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/Boot.unity", true), new EditorBuildSettingsScene("Assets/Scenes/Menu.unity", true) };
        AssetDatabase.SaveAssets();
        Directory.CreateDirectory("Builds/S1-01");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = new[] { "Assets/Scenes/Boot.unity", "Assets/Scenes/Menu.unity" }, locationPathName = "Builds/S1-01/Afterglow.exe", target = BuildTarget.StandaloneWindows64 });
        File.WriteAllText("Logs/S1-01-build.txt", $"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Unity={Application.unityVersion}");
        if (report.summary.result != BuildResult.Succeeded) throw new Exception("S1-01 Windows build failed");
        SessionState.SetBool("SetupSmoke", true);
        EditorApplication.isPlaying = true;
    }
}
