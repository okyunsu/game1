using System;
using UnityEngine;

public sealed class GameState : MonoBehaviour
{
    static GameState instance;
    public static GameState GetOrCreate()
    {
        if (instance == null)
            instance = new GameObject("Game State").AddComponent<GameState>();
        return instance;
    }
    public bool Paused { get; private set; }
    public bool GamepadDisconnected { get; private set; }
    public bool Transitioning { get; private set; }
    public event Action Changed;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Notify()
    {
        Time.timeScale = Paused || Transitioning ? 0 : 1;
        Changed?.Invoke();
    }
    public void SetPaused(bool paused)
    {
        if (Transitioning || Paused == paused) return;
        Paused = paused;
        if (!paused) GamepadDisconnected = false;
        Notify();
    }
    public void PauseForDisconnect()
    {
        if (Transitioning) return;
        GamepadDisconnected = true;
        Paused = true;
        Notify();
    }
    public void ResetPause()
    {
        Paused = false;
        GamepadDisconnected = false;
        Notify();
    }
    public void SetTransitioning(bool value)
    {
        Transitioning = value;
        Paused = false;
        GamepadDisconnected = false;
        Notify();
    }
    void OnDestroy()
    {
        if (instance == this) { instance = null; Time.timeScale = 1; }
    }
}
