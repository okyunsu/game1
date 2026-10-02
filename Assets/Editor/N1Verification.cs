using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[InitializeOnLoad]
public static class N1Verification
{
    static N1Verification()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (string.IsNullOrEmpty(SessionState.GetString("N1.Stage", ""))) return;
            if (state == PlayModeStateChange.EnteredPlayMode)
                new GameObject("Editor-only verification").AddComponent<N1VerificationRunner>();
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                SessionState.SetString("N1.Stage", "");
                EditorApplication.Exit(SessionState.GetInt("N1.Exit", 1));
            }
        };
    }
    public static void Input() => Begin("S1-02", "Assets/Scenes/MovementTest.unity");
    public static void Movement() => Begin("S1-03", "Assets/Scenes/MovementTest.unity");
    public static void Forgiveness() => Begin("S1-04", "Assets/Scenes/MovementTest.unity");
    public static void VariableJump() => Begin("S1-05", "Assets/Scenes/MovementTest.unity");
    public static void InputN2A() => BeginN2A("S1-02");
    public static void ForgivenessN2A() => BeginN2A("S1-04");
    public static void InputN2B08() { SessionState.SetString("N1.Suffix", "N2-B-S1-08"); Begin("S1-02", "Assets/Scenes/MovementTest.unity"); }
    public static void ForgivenessN2B08() { SessionState.SetString("N1.Suffix", "N2-B-S1-08"); Begin("S1-04", "Assets/Scenes/MovementTest.unity"); }
    static void BeginN2A(string stage)
    {
        SessionState.SetString("N1.Suffix", "N2-A-blocks");
        Begin(stage, "Assets/Scenes/MovementTest.unity");
    }
    static void Begin(string stage, string scene)
    {
        SessionState.SetString("N1.Stage", stage);
        SessionState.SetInt("N1.Exit", 1);
        EditorSceneManager.OpenScene(scene);
        var at = EditorApplication.timeSinceStartup + 3;
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

public sealed class N1VerificationRunner : MonoBehaviour
{
    readonly List<string> results = new();
    readonly List<string> errors = new();
    Keyboard keyboard;
    Gamepad gamepad;
    PlayerInputReader input;
    string stage;
    Rigidbody2D body;
    PlayerMotor motor;
    BoxCollider2D testFloor;
    readonly List<float> shortPeaks = new();

    IEnumerator Start()
    {
        stage = SessionState.GetString("N1.Stage", "");
        Application.logMessageReceived += Log;
        var oldFocus = InputSystem.settings.editorInputBehaviorInPlayMode;
        var oldBackground = InputSystem.settings.backgroundBehavior;
        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
        keyboard = InputSystem.AddDevice<Keyboard>();
        gamepad = InputSystem.AddDevice<Gamepad>();
        input = FindFirstObjectByType<PlayerInputReader>();
        var stack = new Stack<IEnumerator>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        stack.Push(AllChecks());
        bool failed = false;
        while (stack.Count > 0 && !failed)
        {
            object next = null;
            try
            {
                if (!stack.Peek().MoveNext()) { stack.Pop(); continue; }
                next = stack.Peek().Current;
            }
            catch (Exception ex) { results.Add("FAIL: " + ex); failed = true; }
            if (next is IEnumerator nested) stack.Push(nested);
            else if (!failed) yield return next;
        }
        Application.logMessageReceived -= Log;
        foreach (var error in errors) results.Add("CONSOLE ERROR: " + error);
        failed |= errors.Count != 0;
        results.Insert(0, $"{(failed ? "FAIL" : "PASS")} {stage}; Unity={Application.unityVersion}; UTC={DateTime.UtcNow:O}; input=Unity virtual Keyboard/Gamepad (physical devices not tested)");
        Directory.CreateDirectory("Logs");
        File.WriteAllLines($"Logs/{stage}-rerun-{DateTime.UtcNow:yyyyMMdd-HHmmss}.txt", results);
        if (!failed)
        {
            Directory.CreateDirectory("Validation");
            string suffix = SessionState.GetString("N1.Suffix", "rerun");
            string path = $"Validation/{stage}-{suffix}.txt";
            if (File.Exists(path)) throw new IOException("Preserve existing result: " + path);
            File.WriteAllLines(path, results);
        }
        input.SetPaused(false);
        InputSystem.RemoveDevice(keyboard);
        if (gamepad.added) InputSystem.RemoveDevice(gamepad);
        InputSystem.settings.editorInputBehaviorInPlayMode = oldFocus;
        InputSystem.settings.backgroundBehavior = oldBackground;
        SessionState.SetInt("N1.Exit", failed ? 1 : 0);
        EditorApplication.isPlaying = false;
    }

    void Log(string message, string trace, LogType type)
    {
        if (type is LogType.Error or LogType.Exception or LogType.Assert) errors.Add(message + "\n" + trace);
    }
    void Check(bool condition, string name)
    {
        if (!condition) throw new Exception(name);
        results.Add("PASS: " + name);
    }
    IEnumerator Keys(params Key[] keys)
    {
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(keys));
        yield return null; yield return null;
    }
    IEnumerator Pad(GamepadState state)
    {
        InputSystem.QueueStateEvent(gamepad, state);
        yield return null; yield return null;
    }
    IEnumerator InputChecks()
    {
        yield return Keys(Key.D);
        Check(input.Move.x > .99f, "D moves right");
        yield return Keys(Key.LeftArrow);
        Check(input.Move.x < -.99f, "left arrow moves left");
        yield return Keys();
        yield return Pad(new GamepadState { leftStick = new Vector2(.8f, 0) });
        Check(input.Move.x > .5f && input.UsingGamepad, "gamepad stick and device switch");
        yield return Pad(new GamepadState().WithButton(GamepadButton.DpadLeft));
        Check(input.Move.x < -.99f, "gamepad D-pad");
        yield return Pad(new GamepadState());
        uint before = input.JumpSequence;
        yield return Keys(Key.Space);
        Check(input.JumpSequence == before + 1 && input.JumpHeld, "Space press once");
        yield return null; yield return null;
        Check(input.JumpSequence == before + 1, "hold does not repeat");
        yield return Keys();
        Check(!input.JumpHeld, "jump release");
        yield return Pad(new GamepadState().WithButton(GamepadButton.South));
        Check(input.JumpSequence == before + 2, "gamepad A jump");
        yield return Pad(new GamepadState());
        yield return Keys(Key.Escape);
        Check(input.Paused && Time.timeScale == 0 && input.Move == Vector2.zero, "Esc pauses physics and gameplay map");
        before = input.JumpSequence;
        yield return Keys(Key.Space, Key.D);
        Check(input.JumpSequence == before && input.Move == Vector2.zero, "UI map blocks gameplay input");
        yield return Pad(new GamepadState().WithButton(GamepadButton.South));
        Check(!input.Paused && Time.timeScale == 1 && input.JumpSequence == before, "A resumes without jump leakage");
        yield return Keys(); yield return Pad(new GamepadState());
        yield return Pad(new GamepadState().WithButton(GamepadButton.Start));
        Check(input.Paused, "Menu button pauses");
        yield return Pad(new GamepadState());
        yield return Pad(new GamepadState().WithButton(GamepadButton.East));
        Check(!input.Paused, "B cancels pause");
        yield return Pad(new GamepadState { leftStick = Vector2.right });
        InputSystem.RemoveDevice(gamepad);
        yield return null;
        Check(input.Paused && input.GamepadDisconnected, "active gamepad disconnect pauses");
        yield return Keys(Key.Enter);
        Check(!input.Paused, "keyboard resumes after disconnect");
        yield return Keys();
        Check(double.IsNegativeInfinity(input.JumpPressedAt), "pause/resume clears pending jump");
        input.SetPaused(true);
        input.gameObject.SetActive(false);
        Check(!input.Paused && !input.GamepadDisconnected && Time.timeScale == 1, "disable clears pause and disconnect flags");
        input.gameObject.SetActive(true);
        yield return Keys();
        yield return Keys(Key.D);
        Check(input.Move.x > .99f && !input.Paused, "reenabled gameplay map accepts movement");
        before = input.JumpSequence;
        yield return Keys(Key.Space);
        Check(input.JumpSequence == before + 1 && input.JumpHeld && Time.timeScale == 1, "reenabled gameplay map accepts jump");
        yield return Keys();
    }

    IEnumerator AllChecks()
    {
        yield return InputChecks();
        if (stage == "S1-02") yield break;
        motor = input.GetComponent<PlayerMotor>();
        body = input.GetComponent<Rigidbody2D>();
        var clone = Instantiate(motor.Tuning);
        var serialized = new SerializedObject(motor);
        serialized.FindProperty("tuning").objectReferenceValue = clone;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        var floor = new GameObject("Verification floor", typeof(BoxCollider2D));
        floor.layer = 6; floor.transform.position = new Vector2(40, 0);
        floor.GetComponent<BoxCollider2D>().size = new Vector2(20, 1);
        testFloor = floor.GetComponent<BoxCollider2D>();
        var wall = new GameObject("Verification wall", typeof(BoxCollider2D));
        wall.layer = 6; wall.transform.position = new Vector2(49, 5);
        wall.GetComponent<BoxCollider2D>().size = new Vector2(1, 10);
        yield return BasicMovementChecks();
        if (stage is "S1-04" or "S1-05")
            foreach (int fps in new[] { 30, 60, 120 })
            {
                Application.targetFrameRate = fps;
                yield return new WaitForSeconds(.2f);
                results.Add($"FRAME CONDITION: target={fps}, fixedDeltaTime={Time.fixedDeltaTime:F3}s");
                yield return ForgivenessChecks();
                if (stage == "S1-05")
                {
                    yield return BasicMovementChecks();
                    yield return VariableJumpChecks(fps);
                }
            }
        if (stage == "S1-05")
        {
            float spread = Mathf.Max(shortPeaks.ToArray()) - Mathf.Min(shortPeaks.ToArray());
            results.Add($"MEASURE: identical 0.100s event hold, short-height FPS spread={spread:F3}u");
            Check(spread <= .1f, "30/60/120fps short jump height spread <= 0.1u");
        }
        Destroy(clone);
    }

    IEnumerator ResetAt(Vector2 position)
    {
        yield return Keys();
        input.ClearTransientInput();
        body.position = position; body.linearVelocity = Vector2.zero;
        Physics2D.SyncTransforms();
        yield return new WaitForSeconds(.15f);
    }

    IEnumerator BasicMovementChecks()
    {
        yield return ResetAt(new Vector2(40, 1.32f));
        Check(motor.Grounded, "floor contact grounded");
        float initialX = body.position.x;
        yield return Keys(Key.D);
        yield return new WaitForSeconds(.2f);
        Check(body.position.x > initialX + .5f && Mathf.Abs(body.linearVelocity.x - 6) < .05f, "accelerate to 6 units/s");
        yield return Keys(Key.A);
        yield return new WaitForSeconds(.15f);
        Check(body.linearVelocity.x < -5.9f, "rapid reversal");
        yield return Keys();
        yield return new WaitForSeconds(.12f);
        Check(Mathf.Abs(body.linearVelocity.x) < .01f, "release brakes to rest");
        var tuningInspector = new SerializedObject(motor.Tuning);
        tuningInspector.FindProperty("moveSpeed").floatValue = 5;
        tuningInspector.ApplyModifiedPropertiesWithoutUndo();
        yield return Keys(Key.D);
        yield return new WaitForSeconds(.2f);
        Check(Mathf.Abs(body.linearVelocity.x - 5) < .05f, "Inspector tuning change applies in Play Mode");
        motor.Tuning.moveSpeed = 6;
        yield return ResetAt(new Vector2(40, 1.32f));
        float startY = body.position.y;
        yield return Keys(Key.Space, Key.D);
        yield return new WaitForSeconds(.15f);
        Check(!motor.Grounded && body.position.y > startY + .7f && body.linearVelocity.x > 5.9f, "basic jump and air control");
        yield return Keys(Key.A);
        yield return new WaitForSeconds(.15f);
        Check(body.linearVelocity.x < -5.5f, "air direction reversal");
        float before = body.linearVelocity.y;
        yield return Keys(Key.Space);
        Check(body.linearVelocity.y <= before + .1f, "no second midair jump");
        yield return Keys();
        yield return new WaitForSeconds(1);
        Check(motor.Grounded && Mathf.Abs(body.position.y - startY) < .03f, "fall and stable landing");
        yield return ResetAt(new Vector2(48.15f, 5));
        yield return Keys(Key.D);
        Check(!motor.Grounded, "wall contact is not ground");
        float wallY = body.position.y;
        yield return Keys(Key.D, Key.Space);
        Check(!motor.Grounded && body.position.y <= wallY && body.linearVelocity.y < 0, "cannot jump from wall");
        yield return Keys();
    }

    IEnumerator KeysAt(double at, params Key[] keys)
    {
        // Queue with the intended event time, even if this rendered frame is late.
        // Dynamic processing is retained; physical events also retain their times.
        while (InputState.currentTime < at) yield return null;
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(keys), at);
        yield return null;
        yield return new WaitForFixedUpdate();
    }

    IEnumerator ForgivenessChecks()
    {
        foreach (double delay in new[] { .08, .12 })
        {
            testFloor.enabled = true;
            yield return ResetAt(new Vector2(40, 1.32f));
            yield return new WaitForFixedUpdate();
            double lastContact = (double)typeof(PlayerMotor).GetField("lastGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(motor);
            testFloor.enabled = false;
            yield return KeysAt(lastContact + delay, Key.Space);
            double actual = input.JumpPressedAt - lastContact;
            results.Add($"MEASURE: {Application.targetFrameRate}fps coyote target={delay:F2}s actual={actual:F4}s velocityY={body.linearVelocity.y:F3}");
            Check(Math.Abs(actual - delay) < .001, "coyote event timestamp equals requested offset");
            Check(delay < .1 ? body.linearVelocity.y > 0 : body.linearVelocity.y < 0, $"coyote {delay:F2}s {(delay < .1 ? "allowed" : "rejected")}");
            yield return Keys();
        }
        testFloor.enabled = true;
        foreach (double lead in new[] { .10, .14 })
        {
            yield return ResetAt(new Vector2(40, 1.32f));
            body.position = new Vector2(40, 3.3f); body.linearVelocity = new Vector2(0, -4);
            input.ClearTransientInput(); Physics2D.SyncTransforms();
            double start = Time.fixedUnscaledTimeAsDouble;
            while (motor.Grounded) yield return new WaitForFixedUpdate();
            while (!motor.Grounded) yield return new WaitForFixedUpdate();
            // lastGrounded is the exact contact sample, not the coroutine frame.
            double landing = (double)typeof(PlayerMotor).GetField("lastGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(motor);
            double flight = landing - start;
            yield return ResetAt(new Vector2(40, 1.32f));
            body.position = new Vector2(40, 3.3f); body.linearVelocity = new Vector2(0, -4);
            input.ClearTransientInput(); Physics2D.SyncTransforms();
            start = Time.fixedUnscaledTimeAsDouble;
            while (motor.Grounded) yield return new WaitForFixedUpdate();
            motor.ClearTransientState();
            double scheduled = start + flight - lead;
            yield return KeysAt(scheduled, Key.Space);
            Check(Math.Abs(input.JumpPressedAt - scheduled) < .001, "buffer event timestamp equals requested offset");
            bool jumped = body.linearVelocity.y > 1;
            double deadline = start + flight + .08;
            while (!jumped && Time.fixedUnscaledTimeAsDouble < deadline)
            {
                yield return new WaitForFixedUpdate();
                jumped = body.linearVelocity.y > 1;
            }
            results.Add($"MEASURE: {Application.targetFrameRate}fps buffer lead={lead:F4}s flight={flight:F4}s jumped={jumped}");
            Check(jumped == (lead < .12), $"buffer {lead:F2}s {(lead < .12 ? "allowed" : "expired")}");
            yield return new WaitForSeconds(1.2f);
            Check(motor.Grounded && body.linearVelocity.y <= .01f, "held buffered input not reused on next landing");
            yield return Keys();
        }
        yield return ResetAt(new Vector2(40, 1.32f));
        yield return Keys(Key.Space);
        yield return new WaitForFixedUpdate();
        yield return Keys();
        yield return new WaitForFixedUpdate();
        float velocity = body.linearVelocity.y;
        yield return Keys(Key.Space);
        yield return new WaitForFixedUpdate();
        Check(body.linearVelocity.y < velocity, "coyote cannot reuse consumed ground jump");
        yield return Keys();
    }
    IEnumerator VariableJumpChecks(int fps)
    {
        double began = Time.realtimeSinceStartupAsDouble;
        int frame = Time.frameCount;
        yield return new WaitForSecondsRealtime(1);
        results.Add($"MEASURE: target={fps} average rendered fps={(Time.frameCount-frame)/(Time.realtimeSinceStartupAsDouble-began):F1}");
        float[] peaks = new float[2];
        for (int mode = 0; mode < 2; mode++)
        {
            yield return ResetAt(new Vector2(40, 1.32f));
            float baseY = body.position.y;
            double pressAt = InputState.currentTime + .04;
            yield return KeysAt(pressAt, Key.Space);
            Check(body.linearVelocity.y > 0 && !motor.Grounded, $"{fps}fps jump input reaches physics tick");
            if (mode == 0)
            {
                yield return KeysAt(pressAt + .10);
                double held = input.JumpReleasedAt - input.JumpPressedAt;
                results.Add($"MEASURE: {fps}fps actual event hold={held:F6}s");
                Check(Math.Abs(held - .10) < .000001, "short jump uses identical real-time event hold");
            }
            double deadline = Time.timeAsDouble + 1.5;
            float maxY = body.position.y;
            while (Time.timeAsDouble < deadline)
            {
                maxY = Mathf.Max(maxY, body.position.y);
                if (motor.Grounded) break;
                yield return new WaitForFixedUpdate();
            }
            peaks[mode] = maxY - baseY;
            Check(motor.Grounded, $"{fps}fps jump lands");
            yield return Keys();
        }
        results.Add($"MEASURE: {fps}fps short height={peaks[0]:F3}u long height={peaks[1]:F3}u");
        shortPeaks.Add(peaks[0]);
        Check(peaks[1] > peaks[0] + .5f, $"{fps}fps variable jump height clearly differs");
        Check(peaks[1] > 2.2f && peaks[1] < 2.7f, $"{fps}fps full jump near prototype estimate");
        yield return ResetAt(new Vector2(40, 1.32f));
        body.position = new Vector2(40, 30); body.linearVelocity = Vector2.zero;
        input.ClearTransientInput(); Physics2D.SyncTransforms();
        yield return new WaitForSeconds(.6f);
        Check(Mathf.Abs(body.linearVelocity.y + 18) < .01f, $"{fps}fps terminal fall speed 18 units/s");
        var inspector = new SerializedObject(motor.Tuning);
        inspector.FindProperty("maxFallSpeed").floatValue = 16;
        inspector.ApplyModifiedPropertiesWithoutUndo();
        yield return new WaitForFixedUpdate();
        Check(Mathf.Abs(body.linearVelocity.y + 16) < .01f, $"{fps}fps Inspector fall cap applies");
        motor.Tuning.maxFallSpeed = 18;
        yield return ResetAt(new Vector2(40, 1.32f));
        body.position = new Vector2(40, 8); body.linearVelocity = new Vector2(0, -1);
        input.ClearTransientInput();
        yield return new WaitForFixedUpdate();
        float v1 = body.linearVelocity.y;
        yield return new WaitForFixedUpdate();
        float acceleration = (v1 - body.linearVelocity.y) / Time.fixedDeltaTime;
        results.Add($"MEASURE: {fps}fps fall acceleration={acceleration:F3} units/s²");
        Check(Mathf.Abs(acceleration - 45) < .1f, $"{fps}fps fall gravity multiplier 1.5");
        yield return Keys();
    }
}
