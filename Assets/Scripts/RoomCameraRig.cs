using UnityEngine;
using Unity.Cinemachine;

[DefaultExecutionOrder(-100)]
public sealed class RoomCameraRig : MonoBehaviour
{
    [Header("Camera (Prototype values)")]
    [Min(.1f), Tooltip("Vertical half-height in world units.")] public float orthographicSize = 5.5f;
    [Min(0), Tooltip("Horizontal offset towards movement, in world units.")] public float horizontalLead = 0f;
    [Min(0), Tooltip("Cinemachine position damping, seconds.")] public float damping = .15f;
    [Header("Room boundary")]
    [Tooltip("Polygon covering the visible room, separate from ground collision.")] public PolygonCollider2D boundary;
    public CinemachineCamera VirtualCamera { get; private set; }
    public Camera OutputCamera { get; private set; }
    CinemachinePositionComposer composer;
    CinemachineConfiner2D confiner;
    CinemachineBrain brain;
    PlayerInputReader input;
    float facing = 1;

    void Awake()
    {
        OutputCamera = GetComponent<Camera>();
        brain = GetComponent<CinemachineBrain>();
        VirtualCamera = GetComponentInChildren<CinemachineCamera>();
        composer = VirtualCamera.GetComponent<CinemachinePositionComposer>();
        confiner = VirtualCamera.GetComponent<CinemachineConfiner2D>();
        ApplySettings();
    }

    void Start()
    {
        var player = FindFirstObjectByType<PlayerInputReader>();
        if (player != null) BindAndSnap(player);
    }

    void Update()
    {
        if (input != null && Mathf.Abs(input.Move.x) > .01f) facing = Mathf.Sign(input.Move.x);
        ApplySettings();
    }

    void ApplySettings()
    {
        if (VirtualCamera == null) return;
        OutputCamera.orthographic = true;
        VirtualCamera.Lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
        VirtualCamera.Lens.OrthographicSize = orthographicSize;
        composer.TargetOffset = new Vector3(facing * horizontalLead, 0, 0);
        composer.Damping = new Vector3(damping, damping, 0);
        if (confiner.BoundingShape2D != boundary)
        {
            confiner.BoundingShape2D = boundary;
            confiner.InvalidateBoundingShapeCache();
        }
    }

    public void BindAndSnap(PlayerInputReader player)
    {
        input = player;
        ApplySettings();
        VirtualCamera.Follow = player.transform;
        VirtualCamera.PreviousStateIsValid = false;
        confiner.InvalidateLensCache();
        var updateMode = brain.UpdateMethod;
        brain.UpdateMethod = CinemachineBrain.UpdateMethods.ManualUpdate;
        brain.ManualUpdate();
        brain.UpdateMethod = updateMode;
    }
}
