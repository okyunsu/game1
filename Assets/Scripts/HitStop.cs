using UnityEngine;

// Owns only the short hit pause. GameState remains the owner of all game modes.
[DefaultExecutionOrder(-31000)]
public sealed class HitStop : MonoBehaviour
{
    static HitStop instance;
    GameState state;
    bool active;
    double started, deadline;
    float previousScale;

    public static void Begin(float seconds)
    {
        if (seconds <= 0) return;
        if (instance == null)
        {
            instance = new GameObject("Hit Stop").AddComponent<HitStop>();
            DontDestroyOnLoad(instance.gameObject);
        }
        instance.StartStop(Mathf.Clamp(seconds, 0, .2f));
    }

    bool GameOwnsPause => state == null || state.Paused || state.Transitioning
        || state.Respawning || state.Completing;

    void Awake()
    {
        state = GameState.GetOrCreate();
        state.Changed += StateChanged;
    }

    void StartStop(float seconds)
    {
        // A second target in the same swing cannot renew or stack the deadline.
        if (active || GameOwnsPause || Time.timeScale <= 0) return;
        previousScale = Time.timeScale;
        started = Time.realtimeSinceStartupAsDouble;
        deadline = started + seconds;
        active = true;
        Time.timeScale = 0;
        Debug.Log($"HitStop BEGIN duration={seconds:F3}s fps={Application.targetFrameRate}");
    }

    void StateChanged()
    {
        if (!active) return;
        if (GameOwnsPause) EndStop("game state", false);
        else Time.timeScale = 0;
    }

    void Update()
    {
        if (!active) return;
        if (GameOwnsPause) EndStop("game state", false);
        else if (Time.realtimeSinceStartupAsDouble >= deadline) EndStop("elapsed", true);
    }

    void EndStop(string reason, bool restore)
    {
        active = false;
        if (restore && !GameOwnsPause && Time.timeScale == 0)
            Time.timeScale = previousScale;
        Debug.Log($"MEASURE HitStop elapsed={Time.realtimeSinceStartupAsDouble - started:F6}s reason={reason} scale={Time.timeScale:F1} fps={Application.targetFrameRate}");
    }

    void OnDestroy()
    {
        if (state != null) state.Changed -= StateChanged;
        if (active) EndStop("destroyed", true);
        if (instance == this) instance = null;
    }
}
