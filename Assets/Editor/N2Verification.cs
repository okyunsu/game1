using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Unity.Cinemachine;

[InitializeOnLoad]
public static class N2Verification
{
    static N2Verification()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (SessionState.GetString("N2.Stage", "") == "") return;
            if (state == PlayModeStateChange.EnteredPlayMode)
                new GameObject("Editor-only N2 verification").AddComponent<N2VerificationRunner>();
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                SessionState.SetString("N2.Stage", "");
                EditorApplication.Exit(SessionState.GetInt("N2.Exit", 1));
            }
        };
    }
    public static void Camera() => Begin("S1-06", "Assets/Scenes/CameraTest.unity");
    static void Begin(string stage, string scene)
    {
        if (File.Exists($"Validation/{stage}-N2-A.txt")) throw new IOException("Preserve existing evidence");
        SessionState.SetString("N2.Stage", stage);
        SessionState.SetInt("N2.Exit", 1);
        EditorSceneManager.OpenScene(scene);
        double at = EditorApplication.timeSinceStartup + 3;
        EditorApplication.CallbackFunction wait = null;
        wait = () =>
        {
            if (EditorApplication.timeSinceStartup < at || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            EditorApplication.update -= wait;
            EditorApplication.isPlaying = true;
        };
        EditorApplication.update += wait;
    }
}

public sealed class N2VerificationRunner : MonoBehaviour
{
    readonly List<string> results = new();
    readonly List<string> errors = new();
    void Log(string message, string stack, LogType kind)
    {
        if (kind is LogType.Error or LogType.Exception or LogType.Assert) errors.Add(message + stack);
    }
    void Check(bool ok, string name)
    {
        if (!ok) throw new Exception(name);
        results.Add("PASS: " + name);
    }
    IEnumerator Start()
    {
        string stage = SessionState.GetString("N2.Stage", "");
        Application.logMessageReceived += Log;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        var stack = new Stack<IEnumerator>();
        stack.Push(CameraChecks());
        bool failed = false;
        while (stack.Count > 0 && !failed)
        {
            object next = null;
            try
            {
                if (!stack.Peek().MoveNext()) { stack.Pop(); continue; }
                next = stack.Peek().Current;
            }
            catch (Exception e) { results.Add("FAIL: " + e); failed = true; }
            if (next is IEnumerator child) stack.Push(child);
            else if (!failed) yield return next;
        }
        Application.logMessageReceived -= Log;
        failed |= errors.Count != 0;
        results.AddRange(errors);
        results.Insert(0, $"{(failed ? "FAIL" : "PASS")} {stage}; Unity={Application.unityVersion}; UTC={DateTime.UtcNow:O}");
        Directory.CreateDirectory("Logs");
        File.WriteAllLines($"Logs/{stage}-N2-A-{DateTime.UtcNow:yyyyMMdd-HHmmss}.txt", results);
        if (!failed)
        {
            Directory.CreateDirectory("Validation");
            File.WriteAllLines($"Validation/{stage}-N2-A.txt", results);
        }
        SessionState.SetInt("N2.Exit", failed ? 1 : 0);
        EditorApplication.isPlaying = false;
    }

    IEnumerator CameraChecks()
    {
        string renderDirectory = $"Validation/Camera/Run-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
        var rig = FindFirstObjectByType<RoomCameraRig>();
        var player = FindFirstObjectByType<PlayerInputReader>();
        player.GetComponent<PlayerMotor>().enabled = false;
        player.GetComponent<Rigidbody2D>().simulated = false;
        Check(rig.orthographicSize == 5.5f && rig.horizontalLead == 1f && rig.damping == .15f, "Inspector prototype values 5.5u / 1u / 0.15s");
        yield return null;
        var cam = rig.OutputCamera;
        foreach (var resolution in new[] { new Vector2Int(1280, 720), new Vector2Int(1920, 1080) })
        {
            var texture = new RenderTexture(resolution.x, resolution.y, 24);
            cam.targetTexture = texture;
            rig.BindAndSnap(player);
            Check(Mathf.Abs(cam.orthographicSize - 5.5f) < .001f, "orthographic lens applied");
            foreach (float x in new[] { -24f, 0f, 24f })
            {
                player.transform.position = new Vector3(x, 4, 0);
                rig.BindAndSnap(player);
                yield return null;
                var bounds = rig.boundary.bounds;
                float halfWidth = cam.orthographicSize * cam.aspect;
                var p = cam.transform.position;
                Check(p.x - halfWidth >= bounds.min.x - .03f && p.x + halfWidth <= bounds.max.x + .03f
                    && p.y - cam.orthographicSize >= bounds.min.y - .03f && p.y + cam.orthographicSize <= bounds.max.y + .03f,
                    $"{resolution.x}x{resolution.y} frame confined at x={x}");
                Check(Mathf.Abs(p.x - rig.VirtualCamera.State.GetFinalPosition().x) < .01f, "camera snap output equals new virtual state immediately");
            }
            player.transform.position = new Vector3(0, 8, 0);
            rig.BindAndSnap(player);
            for (int frame = 0; frame < 30; frame++)
            {
                player.transform.position += Vector3.down * .1f;
                yield return null;
                var landing = cam.WorldToViewportPoint(new Vector3(0, 2.5f, 0));
                Check(landing.y >= 0 && landing.y <= 1, "falling landing point remains in view");
            }
            cam.Render();
            var pixels = new Texture2D(resolution.x, resolution.y, TextureFormat.RGB24, false);
            RenderTexture.active = texture;
            pixels.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0);
            pixels.Apply();
            Directory.CreateDirectory(renderDirectory);
            string path = $"{renderDirectory}/S1-06-{resolution.x}x{resolution.y}.png";
            File.WriteAllBytes(path, pixels.EncodeToPNG());
            results.Add("RENDER: " + path);
            RenderTexture.active = null;
            cam.targetTexture = null;
            texture.Release(); Destroy(texture); Destroy(pixels);
        }
        player.transform.position = new Vector3(0, 4, 0);
        rig.BindAndSnap(player);
        var keyboard = InputSystem.AddDevice<Keyboard>();
        var oldFocus = InputSystem.settings.editorInputBehaviorInPlayMode;
        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        float previousX = cam.transform.position.x;
        float largestStep = 0;
        for (int i = 0; i < 60; i++)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(i < 30 ? Key.D : Key.A));
            player.transform.position += Vector3.right * (i < 30 ? .05f : -.05f);
            yield return null;
            largestStep = Mathf.Max(largestStep, Mathf.Abs(cam.transform.position.x - previousX));
            previousX = cam.transform.position.x;
        }
        Check(largestStep < .5f, "rapid reversal camera step stays below 0.5u; no single-frame 2u offset jump");
        results.Add($"MEASURE: largest reversal camera step={largestStep:F4}u; automated positional check, human feel pending");
        InputSystem.RemoveDevice(keyboard);
        InputSystem.settings.editorInputBehaviorInPlayMode = oldFocus;
    }
}
