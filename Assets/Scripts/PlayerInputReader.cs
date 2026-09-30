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
    public Vector2 Move => Paused ? Vector2.zero : move.ReadValue<Vector2>();
    public bool JumpHeld => !Paused && !suppressJump && jump.IsPressed();
    public uint JumpSequence { get; private set; }
    public double JumpPressedAt { get; private set; } = double.NegativeInfinity;
    public double JumpReleasedAt { get; private set; } = double.NegativeInfinity;
    public bool Paused { get; private set; }
    public bool GamepadDisconnected { get; private set; }
    public bool UsingGamepad => lastDevice is Gamepad;
    public event Action Cleared;

    void Awake()
    {
        runtime = Instantiate(actions);
        move = runtime.FindAction("Gameplay/Move", true);
        jump = runtime.FindAction("Gameplay/Jump", true);
        move.performed += ctx => lastDevice = ctx.control.device;
        jump.performed += ctx =>
        {
            lastDevice = ctx.control.device;
            if (suppressJump || Paused) return;
            JumpSequence++;
            JumpPressedAt = ctx.time;
        };
        jump.canceled += ctx => JumpReleasedAt = ctx.time;
        runtime.FindAction("Gameplay/Pause", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(true); };
        runtime.FindAction("UI/Navigate", true).performed += ctx => lastDevice = ctx.control.device;
        runtime.FindAction("UI/Submit", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(false); };
        runtime.FindAction("UI/Cancel", true).performed += ctx => { lastDevice = ctx.control.device; SetPaused(false); };
    }

    void OnEnable()
    {
        Paused = false;
        GamepadDisconnected = false;
        Time.timeScale = 1;
        runtime.Disable();
        runtime.FindActionMap("Gameplay").Enable();
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
        if (Paused == paused) return;
        Paused = paused;
        runtime.Disable();
        ClearTransientInput();
        Time.timeScale = paused ? 0 : 1;
        runtime.FindActionMap(paused ? "UI" : "Gameplay").Enable();
        if (!paused) GamepadDisconnected = false;
    }

    void DeviceChanged(InputDevice device, InputDeviceChange change)
    {
        if (device != lastDevice || device is not Gamepad) return;
        if (change != InputDeviceChange.Disconnected && change != InputDeviceChange.Removed) return;
        GamepadDisconnected = true;
        SetPaused(true);
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= DeviceChanged;
        Paused = false;
        GamepadDisconnected = false;
        if (runtime != null) runtime.Disable();
        ClearTransientInput();
        Time.timeScale = 1;
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
