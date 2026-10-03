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
        EditorSceneManager.OpenScene(Nightly.Tag.StartsWith("S3-08") ? "Assets/Scenes/C02.unity" : Nightly.Tag.StartsWith("S3-07") ? "Assets/Scenes/B05.unity" : "Assets/Scenes/B02.unity");
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
        var stack = new Stack<IEnumerator>(); stack.Push(Nightly.Tag.StartsWith("S3-08") ? C03C04() : Nightly.Tag.StartsWith("S3-07") ? C01C02() : B02()); bool failed = false;
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
    IEnumerator JumpFrom(Vector2 start, bool twice, bool dash, float seconds = 1.15f)
    {
        yield return Keys(); Place(start); yield return new WaitForSeconds(.2f);
        Check(Motor.Grounded, "jump begins on authored top at " + start);
        yield return Keys(Key.RightArrow, Key.Z); float max = Body.position.y - .8f;
        double end = Time.realtimeSinceStartupAsDouble + .28;
        while (Time.realtimeSinceStartupAsDouble < end) { max = Mathf.Max(max, Body.position.y - .8f); yield return null; }
        if (twice) { yield return Keys(Key.RightArrow); yield return Keys(Key.RightArrow, Key.Z); }
        if (dash) yield return Keys(Key.RightArrow, Key.Z, Key.C);
        end = Time.realtimeSinceStartupAsDouble + seconds - .28;
        while (Time.realtimeSinceStartupAsDouble < end) { max = Mathf.Max(max, Body.position.y - .8f); yield return null; }
        yield return Keys(); yield return new WaitForSeconds(.15f);
        lines.Add($"MEASURE start={start} double={twice} dash={dash} max feet={max:F3} final={Body.position} grounded={Motor.Grounded}");
        peak = max;
    }
    float peak;
    IEnumerator C03C04()
    {
        yield return new WaitForSeconds(.3f); ProgressSave.Enabled = true;
        Motor.hasDoubleJump = true; Player.GetComponent<PlayerDash>().hasDash = true;
        yield return ExitTo("C03"); var patrol = FindFirstObjectByType<EnemyPatrol>(); var turret = FindFirstObjectByType<EnemyTurret>();
        Check(patrol != null && turret != null && turret.direction == -1, "C03 has exactly E1 and left-facing E3");
        yield return new WaitForSeconds(3.5f);
        Check(patrol.Turns > 0 && patrol.transform.position.x >= 8.9f && patrol.transform.position.x <= 15.1f && Mathf.Abs(patrol.transform.position.y - 3.5f) < .05f, "E1 patrol stays on authored top3 between x9 and15");
        var perch = new GameObject("Verification perch"); perch.layer = 6; perch.transform.position = new Vector2(24, 2.5f); perch.AddComponent<BoxCollider2D>().size = new Vector2(2, 1);
        Place(new Vector2(24, 3.81f)); yield return Until(() => turret.Warning); yield return Until(() => turret.ShotsFired == 1);
        Place(new Vector2(32, 1.81f)); perch.SetActive(false);
        yield return Until(() => EnemyShot.LastWallHit == "C03_Cover", 4);
        Check(EnemyShot.LastWallHit == "C03_Cover", "C03 cover physically stops emitted E3 projectile");
        Place(new Vector2(19, 1.81f)); int shots = turret.ShotsFired; yield return new WaitForSeconds(2.2f);
        Check(turret.ShotsFired == shots && Player.GetComponent<PlayerHealth>().HP == 5, "player behind cover remains safe");
        yield return ExitTo("C02"); yield return ExitTo("C03"); yield return ExitTo("C04");
        var cp = FindFirstObjectByType<Checkpoint>(); Place(cp.transform.position); yield return new WaitForSeconds(.2f);
        Check(RoomSession.Instance.CheckpointId == "CP-C04", "CP-C04 activates at x26");
        Check(ProgressSave.TryLoad(out var data) && data.checkpointId == "CP-C04", "CP-C04 persisted valid");
        Check(ProgressSave.Continue() && ProgressSave.PendingScene.EndsWith("C04.unity"), "CP-C04 Continue mapping"); ProgressSave.Pending = null;
        yield return ExitTo("C03"); Check(RoomSession.Instance.Die(), "death in C03"); yield return Until(() => !RoomSession.Instance.Respawning);
        Check(RoomSession.Instance.CurrentRoomId == "C04" && Mathf.Abs(Body.position.x - 26) < .1f, "death returns to CP-C04");
        double started = Time.timeAsDouble; yield return Keys(Key.RightArrow); yield return Until(() => Body.position.x >= 33.4f);
        double elapsed = Time.timeAsDouble - started; yield return Keys(); lines.Add($"MEASURE CP-C04 x26 to C06 entry trigger x33.75: {elapsed:F3}s");
        Check(elapsed < 15, "CP-C04 to C06 entry less than15s by prescribed right input");
        yield return ExitTo("C03"); yield return ExitTo("C02"); yield return ExitTo("C03"); yield return ExitTo("C04");
    }
    IEnumerator C01C02()
    {
        yield return new WaitForSeconds(.3f); ProgressSave.Enabled = true;
        Motor.hasDoubleJump = true; Player.GetComponent<PlayerDash>().hasDash = true;
        yield return ExitTo("C01"); var cp = FindFirstObjectByType<Checkpoint>();
        Place(cp.transform.position); yield return new WaitForSeconds(.2f);
        Check(RoomSession.Instance.CheckpointId == "CP-C01", "CP-C01 activates");
        Check(ProgressSave.TryLoad(out var data) && data.checkpointId == "CP-C01", "CP-C01 valid in disk save");
        Check(ProgressSave.Continue() && ProgressSave.PendingScene.EndsWith("C01.unity"), "CP-C01 Continue scene mapping"); ProgressSave.Pending = null;
        yield return ExitTo("C02");
        Check(RoomSession.Instance.Die(), "C02 death request"); yield return Until(() => !RoomSession.Instance.Respawning);
        Check(RoomSession.Instance.CurrentRoomId == "C01" && Mathf.Abs(Body.position.x - 8) < .1f, "C02 death returns to safe CP-C01");
        yield return ExitTo("B05"); yield return ExitTo("C01"); yield return ExitTo("C02");
        yield return JumpFrom(new Vector2(18, 1.81f), true, false);
        Check(peak < 6.8f && Body.position.y - .8f < 6.8f, "floor double jump cannot reach authored high platforms top6.8");
        yield return JumpFrom(new Vector2(9.3f, 3.81f), false, false);
        Check(peak < 6.8f, "Start top3 ordinary jump cannot reach High1 top6.8");
        yield return JumpFrom(new Vector2(9.3f, 3.81f), true, false, .95f);
        Check(Body.position.x >= 12 && Body.position.x <= 16.4f && Motor.Grounded && Mathf.Abs(Body.position.y - 7.6f) < .08f, "Start double jump lands on High1");
        yield return JumpFrom(new Vector2(15.3f, 7.61f), false, false, .9f);
        Check(Body.position.y - .8f < 6.7f, "High1 to High2 ordinary jump fails at 5u gap");
        yield return JumpFrom(new Vector2(15.3f, 7.61f), false, true, .9f);
        Check(Body.position.x >= 21 && Body.position.x <= 25.4f && Motor.Grounded && Mathf.Abs(Body.position.y - 7.6f) < .08f, "High1 to High2 jump plus dash succeeds");
        var room = FindFirstObjectByType<RoomDefinition>(); room.TryGetSpawn("FromRight", out var spawn);
        var exit = Array.Find(FindObjectsByType<RoomExit>(FindObjectsSortMode.None), e => e.exitId == "Right");
        var playerBounds = Player.GetComponent<BoxCollider2D>().bounds; playerBounds.center = spawn.transform.position;
        Check(!playerBounds.Intersects(exit.GetComponent<BoxCollider2D>().bounds), "C02 FromRight player bounds do not overlap exit trigger");
        yield return ExitTo("C01"); yield return ExitTo("C02");
    }
}
