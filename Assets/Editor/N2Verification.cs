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
    public static void Rooms() => Begin("S1-07", "Assets/Scenes/A01.unity");
    public static void RoomsN2B08() => Begin("S1-07", "Assets/Scenes/A01.unity", "N2-B-S1-08");
    static void Begin(string stage, string scene, string suffix = null)
    {
        suffix ??= stage == "S1-07" ? "N2-A-blocks-final" : "N2-A";
        if (File.Exists($"Validation/{stage}-{suffix}.txt")) throw new IOException("Preserve existing evidence");
        SessionState.SetString("N2.Suffix", suffix);
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
        if (stage == "S1-07") DontDestroyOnLoad(gameObject);
        Application.logMessageReceived += Log;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        var stack = new Stack<IEnumerator>();
        stack.Push(stage == "S1-07" ? RoomChecks() : CameraChecks());
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
            File.WriteAllLines($"Validation/{stage}-" + SessionState.GetString("N2.Suffix", "N2-A") + ".txt", results);
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

    IEnumerator RoomChecks()
    {
        yield return new WaitForSeconds(.2f);
        var session = RoomSession.Instance;
        Check(session != null && session.CurrentRoomId == "A01", "direct room start bootstraps one persistent session");
        int playerId = session.Player.GetInstanceID();
        var keyboard = InputSystem.AddDevice<Keyboard>();
        var oldFocus = InputSystem.settings.editorInputBehaviorInPlayMode;
        var oldBackground = InputSystem.settings.backgroundBehavior;
        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
        foreach (string expected in new[] { "A02", "A03", "A04", "A03", "A02", "A01" })
        {
            var definition = FindFirstObjectByType<RoomDefinition>();
            string sourceId = definition.roomId;
            var exits = definition.GetComponentsInChildren<RoomExit>();
            var exit = Array.Find(exits, x => x.destinationRoomId == expected);
            Check(exit != null, "authored graph exit to " + expected);
            bool right = exit.exitId == "Right";
            string targetSpawnId = exit.destinationSpawnId;
            var body = session.Player.GetComponent<Rigidbody2D>();
            session.Player.ClearTransientInput();
            body.position = new Vector2(exit.transform.position.x + (right ? -1.2f : 1.2f), 1.85f);
            body.linearVelocity = Vector2.zero;
            Physics2D.SyncTransforms();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(right ? Key.D : Key.A));
            double deadline = Time.realtimeSinceStartupAsDouble + 10;
            while ((session.CurrentRoomId != expected || session.Transitioning) && Time.realtimeSinceStartupAsDouble < deadline)
                yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            results.Add($"MEASURE: route {sourceId}->{expected}: room={session.CurrentRoomId}, body={body.position}, move={session.Player.Move}, paused={session.Player.Paused}, transition={session.Transitioning}, lastError={session.LastError}");
            Check(session.CurrentRoomId == expected && !session.Transitioning, "physical exit trigger completed " + sourceId + " -> " + expected);
            Check(FindObjectsByType<PlayerInputReader>(FindObjectsSortMode.None).Length == 1 && session.Player.GetInstanceID() == playerId, "same player instance survives transition");
            Check(FindObjectsByType<RoomDefinition>(FindObjectsSortMode.None).Length == 1, "previous room unloaded");
            Check(FindObjectsByType<GameState>(FindObjectsSortMode.None).Length == 1 && Time.timeScale == 1, "one GameState and playable timeScale");
            Check(double.IsNegativeInfinity(session.Player.JumpPressedAt), "transition clears pending jump timestamp");
            yield return new WaitForSeconds(.15f);
            results.Add($"MEASURE: arrival {expected} body={body.position} velocity={body.linearVelocity} grounded={session.Player.GetComponent<PlayerMotor>().Grounded}");
            Check(session.Player.GetComponent<PlayerMotor>().Grounded, "arrival settles on safe ground without wall sticking");
            var arrived = FindFirstObjectByType<RoomDefinition>();
            Check(arrived.TryGetSpawn(targetSpawnId, out var targetSpawn)
                && Mathf.Abs(body.position.x - targetSpawn.transform.position.x) < .3f,
                "arrival remains at the specified destination Spawn ID " + targetSpawnId);
            var rig = FindFirstObjectByType<RoomCameraRig>();
            var camera = rig.OutputCamera;
            float halfY = camera.orthographicSize, halfX = halfY * camera.aspect;
            Vector3 cameraPos = camera.transform.position;
            Check(rig.boundary != null && FindObjectsByType<Camera>(FindObjectsSortMode.None).Length == 1
                && cameraPos.x - halfX >= -.02f && cameraPos.x + halfX <= 32.02f
                && cameraPos.y - halfY >= -1.02f && cameraPos.y + halfY <= 13.02f,
                "one active room camera stays within PolygonCollider2D boundary");
        }
        Check(!session.RequestTransition("Invalid", "Assets/Scenes/NoSuchRoom.unity", "Entry"), "invalid scene rejected without loading");
        Check(session.CurrentRoomId == "A01" && !session.Transitioning, "invalid scene retains playable source");
        Check(session.RequestTransition("A02", "Assets/Scenes/A02.unity", "MissingSpawn"), "valid scene with bad spawn tested transactionally");
        yield return WaitForTransition(session);
        Check(session.CurrentRoomId == "A01" && session.LastError != null, "invalid spawn retains old room and records reason");
        Check(session.RequestTransition("WrongRoomId", "Assets/Scenes/A02.unity", "FromLeft"), "mismatched Room ID request starts validation");
        yield return WaitForTransition(session);
        Check(session.CurrentRoomId == "A01" && !session.Transitioning, "wrong room ID rollback restores input/time");
        session.Player.SetPaused(true);
        Check(!session.RequestTransition("A02", "Assets/Scenes/A02.unity", "FromLeft") && session.Player.Paused, "pause cannot be overwritten by room request");
        session.Player.SetPaused(false);
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
        yield return null;
        Check(session.RequestTransition("A02", "Assets/Scenes/A02.unity", "FromLeft"), "transition with held jump starts");
        session.Player.SetPaused(true);
        Check(!session.Player.Paused && session.Transitioning && session.Player.Move == Vector2.zero, "transition lock ignores Pause and blocks Gameplay");
        yield return WaitForTransition(session);
        yield return new WaitForSeconds(.1f);
        Check(session.CurrentRoomId == "A02" && session.Player.GetComponent<PlayerMotor>().Grounded
            && session.Player.GetComponent<Rigidbody2D>().linearVelocity.y <= .01f, "held pre-transition jump cannot leak at arrival");
        InputSystem.QueueStateEvent(keyboard, new KeyboardState());
        yield return null; yield return null;
        uint sequence = session.Player.JumpSequence;
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
        yield return null; yield return new WaitForFixedUpdate();
        Check(session.Player.JumpSequence == sequence + 1 && session.Player.GetComponent<Rigidbody2D>().linearVelocity.y > 0, "fresh post-transition jump works once");
        var current = FindFirstObjectByType<RoomDefinition>();
        var ground = Array.Find(current.GetComponentsInChildren<BoxCollider2D>(), x => x.name == "Ground");
        var hazard = Array.Find(current.GetComponentsInChildren<BoxCollider2D>(), x => x.name == "Hazard");
        Check(ground.gameObject.layer == 6 && !ground.isTrigger && ground.GetComponent<SpriteRenderer>() != null, "Ground block uses solid Ground layer and SpriteRenderer");
        Check(hazard.gameObject.layer == 6 && hazard.isTrigger && hazard.GetComponent<SpriteRenderer>() != null, "Hazard block uses Ground layer trigger; death behavior deferred to S1-08");
        InputSystem.RemoveDevice(keyboard);
        InputSystem.settings.editorInputBehaviorInPlayMode = oldFocus;
        InputSystem.settings.backgroundBehavior = oldBackground;
    }
    IEnumerator WaitForTransition(RoomSession session)
    {
        double deadline = Time.realtimeSinceStartupAsDouble + 10;
        while (session.Transitioning && Time.realtimeSinceStartupAsDouble < deadline) yield return null;
        Check(!session.Transitioning, "transition or rollback finishes within 10s");
    }
}
