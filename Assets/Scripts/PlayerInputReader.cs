using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerInputReader : MonoBehaviour
{
    [SerializeField, Tooltip("Gameplay and UI action maps. Runtime uses a private copy.")] InputActionAsset actions;
    InputActionAsset runtime;
    InputAction move, jump;
    InputDevice lastDevice;
    bool suppressJump;
    GameState state;
    public bool InputLocked => state != null && (state.Paused || state.Transitioning || state.Respawning);
    public Vector2 Move => InputLocked ? Vector2.zero : move.ReadValue<Vector2>();
    public bool JumpHeld => !InputLocked && !suppressJump && jump.IsPressed();
    public uint JumpSequence { get; private set; }
    public double JumpPressedAt { get; private set; } = double.NegativeInfinity;
    public double JumpReleasedAt { get; private set; } = double.NegativeInfinity;
    public bool Paused => state != null && state.Paused;
    public bool GamepadDisconnected => state != null && state.GamepadDisconnected;
    public bool UsingGamepad => lastDevice is Gamepad;
    public event Action Cleared;
    public event Action DashPressed;
    public event Action AttackPressed;

    void Awake()
    {
        state = GameState.GetOrCreate();
        runtime = Instantiate(actions);
        move = runtime.FindAction("Gameplay/Move", true);
        jump = runtime.FindAction("Gameplay/Jump", true);
        move.performed += ctx => lastDevice = ctx.control.device;
        jump.performed += ctx =>
        {
            lastDevice = ctx.control.device;
            if (suppressJump || InputLocked || (GetComponent<PlayerDash>() != null && GetComponent<PlayerDash>().IsDashing)) return;
            JumpSequence++;
            JumpPressedAt = ctx.time;
        };
        jump.canceled += ctx => JumpReleasedAt = ctx.time;
        runtime.FindAction("Gameplay/Dash",true).performed += ctx => { lastDevice=ctx.control.device;if(!InputLocked)DashPressed?.Invoke(); };
        runtime.FindAction("Gameplay/Attack",true).performed += ctx => { lastDevice=ctx.control.device;if(!InputLocked)AttackPressed?.Invoke(); };
        runtime.FindAction("Gameplay/Pause", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(true); };
        runtime.FindAction("UI/Navigate", true).performed += ctx => lastDevice = ctx.control.device;
        runtime.FindAction("UI/Submit", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(false); };
        runtime.FindAction("UI/Cancel", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(false); };
    }

    void OnEnable()
    {
        state.ResetPause();
        state.Changed += ApplyState;
        ApplyState();
        InputSystem.onDeviceChange += DeviceChanged;
    }

    void Update()
    {
        if (!suppressJump) return;
        foreach (var control in jump.controls)
            if (control.IsPressed()) return;
        suppressJump = false;
    }

    public void ClearTransientInput()
    {
        JumpPressedAt = JumpReleasedAt = double.NegativeInfinity;
        suppressJump = true;
        Cleared?.Invoke();
    }

    public void SetPaused(bool paused)
    {
        state.SetPaused(paused);
    }

    void ApplyState()
    {
        runtime.Disable();
        ClearTransientInput();
        if (!state.Transitioning && !state.Respawning)
            runtime.FindActionMap(Paused ? "UI" : "Gameplay").Enable();
    }

    void DeviceChanged(InputDevice device, InputDeviceChange change)
    {
        if (device != lastDevice || device is not Gamepad) return;
        if (change != InputDeviceChange.Disconnected && change != InputDeviceChange.Removed) return;
        state.PauseForDisconnect();
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= DeviceChanged;
        state.Changed -= ApplyState;
        state.ResetPause();
        if (runtime != null) runtime.Disable();
        ClearTransientInput();
    }

    void OnDestroy() { if (runtime != null) Destroy(runtime); }

    void OnGUI()
    {
        GUI.Label(new Rect(16, 12, 700, 30), UsingGamepad ? "Move: Left Stick / D-pad   Jump: A   Pause: Menu" : "Move: A/D or Arrows   Jump: Space   Pause: Esc");
        if (!Paused) return;
        GUI.Box(new Rect(20, 55, 420, 110), GamepadDisconnected ? "Gamepad disconnected - Paused" : "Paused");
        if (GUI.Button(new Rect(40, 95, 380, 45), "Resume (Enter / A / Esc / B)")) SetPaused(false);
    }
}
