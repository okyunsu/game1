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
public static class S3FinalVerification
{
    static S3FinalVerification()
    {
        EditorApplication.playModeStateChanged += s => {
            if (!SessionState.GetBool("S3Final.Run", false)) return;
            if (s == PlayModeStateChange.EnteredPlayMode) new GameObject("S3 final verification").AddComponent<S3FinalRunner>();
            if (s == PlayModeStateChange.EnteredEditMode) { SessionState.SetBool("S3Final.Run", false); EditorApplication.Exit(SessionState.GetInt("S3Final.Exit", 1)); }
        };
    }
    public static void Run()
    {
        string path = "Validation/" + Nightly.Tag + ".txt";
        if (File.Exists(path)) throw new IOException("Preserve evidence");
        SessionState.SetString("S3Final.Path", path); SessionState.SetBool("S3Final.Run", true);
        EditorSceneManager.OpenScene("Assets/Scenes/B02.unity");
        EditorApplication.delayCall += () => EditorApplication.isPlaying = true;
    }
}
public sealed class S3FinalRunner : MonoBehaviour
{
    readonly List<string> lines = new(), errors = new(); Keyboard kb;
    PlayerInputReader Player => RoomSession.Instance.Player;
    Rigidbody2D Body => Player.GetComponent<Rigidbody2D>();
    PlayerMotor Motor => Player.GetComponent<PlayerMotor>();
    void Check(bool ok, string message) { if (!ok) throw new Exception(message); lines.Add("PASS: " + message); }
    void Log(string m, string s, LogType t) { if (t is LogType.Error or LogType.Exception or LogType.Assert) errors.Add(m + s); }
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject); Application.logMessageReceived += Log;
        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        kb = InputSystem.AddDevice<Keyboard>(); ProgressSave.Enabled = false;
        var stack = new Stack<IEnumerator>(); stack.Push(B02()); bool failed = false;
        while (stack.Count > 0 && !failed) { object next = null; try { if (!stack.Peek().MoveNext()) { stack.Pop(); continue; } next = stack.Peek().Current; } catch (Exception e) { failed = true; lines.Add("FAIL: " + e); } if (next is IEnumerator child) stack.Push(child); else if (!failed) yield return next; }
        Application.logMessageReceived -= Log; InputSystem.RemoveDevice(kb); failed |= errors.Count > 0;
        lines.Insert(0, (failed ? "FAIL " : "PASS ") + Nightly.Tag + " Unity=" + Application.unityVersion);
        lines.AddRange(errors); lines.Add("Error/Exception/Assert " + errors.Count);
        File.WriteAllLines(SessionState.GetString("S3Final.Path", ""), lines);
        SessionState.SetInt("S3Final.Exit", failed ? 1 : 0); EditorApplication.isPlaying = false;
    }
    void Place(Vector2 p) { Player.ClearTransientInput(); Player.transform.position = p; Body.position = p; Body.linearVelocity = Vector2.zero; Physics2D.SyncTransforms(); }
    IEnumerator Keys(params Key[] keys) { InputSystem.QueueStateEvent(kb, new KeyboardState(keys)); yield return null; yield return new WaitForFixedUpdate(); }
    IEnumerator Until(Func<bool> condition, float seconds = 4) { double end = Time.realtimeSinceStartupAsDouble + seconds; while (!condition()) { if (Time.realtimeSinceStartupAsDouble > end) throw new Exception("Timeout"); yield return null; } }
    IEnumerator ExitTo(string target)
    {
        RoomExit exit = Array.Find(FindObjectsByType<RoomExit>(FindObjectsSortMode.None), e => e.destinationRoomId == target);
        Check(exit != null && exit.enabled, "enabled door to " + target); string spawnId = exit.destinationSpawnId;
        Place(exit.GetComponent<BoxCollider2D>().bounds.center); yield return new WaitForFixedUpdate(); yield return null;
        yield return Until(() => !RoomSession.Instance.Transitioning);
        Check(RoomSession.Instance.CurrentRoomId == target, "physical door to " + target);
        var room = FindFirstObjectByType<RoomDefinition>(); Check(room.TryGetSpawn(spawnId, out var spawn) && Vector2.Distance(Body.position, spawn.transform.position) < .2f, "safe arrival " + target + "/" + spawnId);
    }
    IEnumerator B02()
    {
        yield return new WaitForSeconds(.3f); var e = FindFirstObjectByType<EnemyCharger>();
        Check(Mathf.Abs(e.transform.position.x - 17) < .01f && e.Direction == -1, "E2 x17 on floor facing left");
        Place(new Vector2(12.8f, 3.81f)); yield return Until(() => e.State == EnemyCharger.Phase.Warning);
        float distance = Vector2.Distance(e.transform.position, Body.position); lines.Add($"MEASURE SafeStep top=3 right=13 player={Body.position} E2={e.transform.position} detection distance={distance:F3}u");
        Check(Motor.Grounded && distance <= 5, "warning starts while standing on actual SafeStep");
        yield return Until(() => e.State == EnemyCharger.Phase.Recovery);
        lines.Add($"MEASURE first charge ends x={e.transform.position.x:F3}; not yet at step wall x13");
        yield return Until(() => e.State == EnemyCharger.Phase.Charge);
        yield return Until(() => e.State == EnemyCharger.Phase.Recovery);
        float wallRight = GameObject.Find("B02_SafeStep").GetComponent<BoxCollider2D>().bounds.max.x;
        float enemyLeft = e.GetComponent<BoxCollider2D>().bounds.min.x;
        lines.Add($"MEASURE repeated charge stops x={e.transform.position.x:F3}; wall gap={enemyLeft-wallRight:F3}u");
        Check(enemyLeft >= wallRight - .02f && enemyLeft - wallRight < .15f && Player.GetComponent<PlayerHealth>().HP == 5, "repeated charge physically stops at step wall; player on top unharmed");
        Place(new Vector2(14.3f, 1.81f)); yield return Until(() => Player.GetComponent<PlayerHealth>().HP < 5, 4);
        Check(Player.GetComponent<PlayerHealth>().HP == 4, "floor contact still damages player");
        yield return ExitTo("B01"); yield return ExitTo("B02"); yield return ExitTo("B03"); yield return ExitTo("B02"); yield return ExitTo("B01"); yield return ExitTo("B02");
    }
}
