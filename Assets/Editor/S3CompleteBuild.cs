using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class S3CompleteBuild
{
    public static void Run()
    {
        string evidence = "Validation/S3-complete-build.txt";
        if (File.Exists(evidence)) throw new IOException("Preserve evidence");
        string[] ids = { "Menu", "A01", "A02", "A03", "A04", "A05", "B01", "B02", "B03", "B04", "B05", "C01", "C02", "C03", "C04" };
        var scenes = ids.Select(id => "Assets/Scenes/" + id + ".unity").ToArray();
        foreach (string path in scenes) if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null) throw new Exception("Missing " + path);
        foreach (string id in new[] { "C01", "C02", "C03", "C04" })
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + id + ".unity");
            var rig = UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>();
            if (rig.orthographicSize != 5.5f || rig.horizontalLead != 0 || rig.damping != .15f) throw new Exception("Camera contract " + id);
            foreach (var exit in UnityEngine.Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))
            {
                var shape = exit.GetComponent<BoxCollider2D>();
                if (shape.size != new Vector2(1, 2) || !shape.isTrigger) throw new Exception("Exit dimensions " + id);
                if (exit.enabled && !scenes.Contains(exit.destinationScenePath)) throw new Exception("Enabled dangling exit " + id);
            }
        }
        Directory.CreateDirectory("Builds/S3-complete");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = scenes, locationPathName = "Builds/S3-complete/Afterglow-S3-complete.exe", target = BuildTarget.StandaloneWindows64, options = BuildOptions.None });
        File.WriteAllText(evidence, $"{report.summary.result}; errors={report.summary.totalErrors}; warnings={report.summary.totalWarnings}; Windows x64; Unity={Application.unityVersion}\nscenes={string.Join(",", ids)}; count={ids.Length}; test scenes excluded\nIncluded successful S3-06/07/08. C06 and Ending excluded: F1 and S3-09 twice-failed/full restoration / S3-10 dependency skip. C04 Right remains disabled. Camera and exit editor contracts checked before build.\n");
        if (report.summary.result != BuildResult.Succeeded || report.summary.totalErrors != 0) throw new Exception("S3-complete build failed");
    }
}
