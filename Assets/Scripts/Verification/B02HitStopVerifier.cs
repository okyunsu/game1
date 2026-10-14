#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;

// Opt-in state-overlap check using the existing hit-stop and damage entry points.
public sealed class B02HitStopVerifier : N5Runner
{
    int begins;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() => N5Args.Start<B02HitStopVerifier>("-verify-b02-hitstop");
    void Observe(string message, string stack, LogType type)
    {
        if (message.StartsWith("HitStop BEGIN")) begins++;
        if (message.StartsWith("MEASURE HitStop")) lines.Add(message);
    }
    protected override void Cleanup() => Application.logMessageReceived -= Observe;
    protected override IEnumerator Tests()
    {
        yield return NewGame();
        yield return new WaitForSeconds(.2f);
        Application.logMessageReceived += Observe;
        HitStop.Begin(.05f);
        Check(Time.timeScale == 0, "hit stop active before Pause");
        Player.SetPaused(true);
        yield return new WaitForSecondsRealtime(.15f);
        Check(Time.timeScale == 0 && Player.Paused, "Pause owns time after hit stop deadline");
        Player.SetPaused(false);
        yield return new WaitForSecondsRealtime(.1f);
        Check(Time.timeScale == 1 && !Player.Paused, "resume has no residual stop");

        var session = RoomSession.Instance;
        Check(session.CheckpointId == "CP-A01", "initial checkpoint active");
        HitStop.Begin(.05f);
        double died = Time.realtimeSinceStartupAsDouble;
        Check(Health.TakeDamage(Health.HP, Player.transform.position), "lethal damage accepted during hit stop");
        Check(session.Respawning && Time.timeScale == 0, "death owns time during hit stop");
        yield return Until(() => !session.Respawning, "death recovery");
        double duration = Time.realtimeSinceStartupAsDouble - died;
        lines.Add($"MEASURE hitstop death return seconds={duration:F6} expected={session.respawnDelay:F3}");
        Check(duration >= session.respawnDelay && duration < session.respawnDelay + .15,
            "real-time death return follows configured 0.6s flow");
        yield return new WaitForSecondsRealtime(.1f);
        Check(Time.timeScale == 1 && Health.HP == Health.tuning.playerMaxHP && session.CurrentRoomId == "A01",
            "death restores checkpoint / HP / time scale");
        int before = begins;
        HitStop.Begin(0);
        yield return new WaitForSecondsRealtime(.1f);
        Check(begins == before && Time.timeScale == 1, "zero seconds emits zero HitStop BEGIN and no stop");
        Check(begins == 2, "exactly two nonzero stops observed");
        lines.Add("Direct existing Begin/TakeDamage/SetPaused calls; perception and physical UI input not tested.");
    }
}
#endif
