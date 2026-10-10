using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerInputReader))]
public sealed class PlayerMotor : MonoBehaviour
{
    [SerializeField, Tooltip("Single source of shared movement values. Changes apply on the next physics tick.")] PlayerTuning tuning;
    [SerializeField, Tooltip("Only this layer can provide floor contact.")] LayerMask groundLayers;
    [Header("Double jump ownership")]
    [Tooltip("Permanent ability ownership. Default false in production rooms.")] public bool hasDoubleJump;
    bool doubleJumpUsed;
    [Tooltip("Permanent wall jump ownership. Production default false.")] public bool hasWallJump;
    public bool WallSliding { get; private set; }
    public int WallJumpsPerformed { get; private set; }
    public int WallAttachments { get; private set; }
    int wallDirection;
    double lastWall = double.NegativeInfinity, wallLockUntil = double.NegativeInfinity;
    public bool DoubleJumpAvailable => hasDoubleJump && !doubleJumpUsed;
    public int DoubleJumpsPerformed { get; private set; }
    readonly ContactPoint2D[] contacts = new ContactPoint2D[16];
    Rigidbody2D body;
    BoxCollider2D shape;
    PlayerInputReader input;
    uint consumedJump;
    double lastGrounded = double.NegativeInfinity;
    bool groundJumpAvailable;
    bool jumpCutAvailable;
    double jumpPressTime;
    double jumpStartedAt;
    public bool Grounded { get; private set; }
    public PlayerTuning Tuning => tuning;
    public int Facing { get; private set; } = 1;
    public void SetWallJumpTestTuning(PlayerTuning candidate)
    {
        if (RoomSession.Instance?.CurrentRoomId == "WallJumpTest" && candidate != null) tuning = candidate;
    }

    public void ApplyPogo(float upwardVelocity, bool refreshOwnedAir)
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, upwardVelocity);
        Grounded = false;
        groundJumpAvailable = false;
        lastGrounded = double.NegativeInfinity;
        jumpCutAvailable = false;
        consumedJump = input.JumpSequence;
        if (refreshOwnedAir && hasDoubleJump) doubleJumpUsed = false;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        shape = GetComponent<BoxCollider2D>();
        input = GetComponent<PlayerInputReader>();
        body.gravityScale = 0;
    }
    void OnEnable() => input.Cleared += ClearTransientState;
    void OnDisable() { input.Cleared -= ClearTransientState; }
    public void ClearTransientState()
    {
        ClearWallState();
        consumedJump = input.JumpSequence;
        lastGrounded = double.NegativeInfinity;
        groundJumpAvailable = false;
        jumpCutAvailable = false;
    }

    public void ClearWallState()
    {
        WallSliding = false;
        wallDirection = 0;
        lastWall = wallLockUntil = double.NegativeInfinity;
    }

    void FixedUpdate()
    {
        if (input.Paused) return;
        // InputAction event time and fixedUnscaledTime share the Input System's
        // realtime timeline. Do not compare event time to scaled frame time.
        double now = Time.fixedUnscaledTimeAsDouble;
        Grounded = false;
        var filter = new ContactFilter2D { useLayerMask = true, layerMask = groundLayers, useTriggers = false };
        int count = shape.GetContacts(filter, contacts);
        if (body.linearVelocity.y <= .01f)
            for (int i = 0; i < count; i++)
                if (contacts[i].normal.y >= .7f && contacts[i].point.y <= shape.bounds.min.y + .08f)
                    Grounded = true;

        if (Grounded)
        {
            doubleJumpUsed = false;
            lastGrounded = now;
            groundJumpAvailable = true;
        }

        var health=GetComponent<PlayerHealth>();
        if(health!=null&&health.KnockbackLocked){ClearWallState();var knocked=body.linearVelocity;knocked.y=Mathf.Max(knocked.y-tuning.gravity*(knocked.y<=0?tuning.fallGravityMultiplier:1)*Time.fixedDeltaTime,-tuning.maxFallSpeed);body.linearVelocity=knocked;return;}
        if(Mathf.Abs(input.Move.x)>.01f) Facing=input.Move.x>0?1:-1;
        var dash=GetComponent<PlayerDash>();
        if(dash!=null && dash.Tick(Grounded,now)) { ClearWallState();consumedJump=input.JumpSequence;jumpCutAvailable=false;groundJumpAvailable=false;lastGrounded=double.NegativeInfinity;return; }
        bool wasSliding = WallSliding;
        WallSliding = false;
        if (hasWallJump && !Grounded && !input.InputLocked && now >= wallLockUntil && body.linearVelocity.y <= 0)
            for (int i = 0; i < count; i++)
            {
                var contact = contacts[i];
                var other = contact.collider == shape ? contact.otherCollider : contact.collider;
                if (other == null || other.GetComponent<ClimbableWall>() == null || other.GetComponent<DashGate>() != null || Mathf.Abs(contact.normal.x) <= .7f) continue;
                int side = contact.normal.x < 0 ? 1 : -1;
                if (input.Move.x * side <= .01f) continue;
                wallDirection = side; lastWall = now; WallSliding = true; Facing = -side;
                if (!wasSliding) { doubleJumpUsed = false; dash?.RefreshAirFromPogo(); WallAttachments++; }
                break;
            }
        if (Grounded || !hasWallJump) { WallSliding = false; lastWall = double.NegativeInfinity; }
        Vector2 velocity = body.linearVelocity;
        float target = Mathf.Clamp(input.Move.x, -1, 1) * tuning.moveSpeed;
        float rate = Mathf.Abs(target) < .001f || target * velocity.x < 0 ? tuning.deceleration : tuning.acceleration;
        if (now >= wallLockUntil) velocity.x = Mathf.MoveTowards(velocity.x, target, rate * (Grounded ? 1 : tuning.airControl) * Time.fixedDeltaTime);
        float gravity = tuning.gravity * (velocity.y <= 0 ? tuning.fallGravityMultiplier : 1);
        velocity.y = Mathf.Max(velocity.y - gravity * Time.fixedDeltaTime, -tuning.maxFallSpeed);
        if (WallSliding) velocity.y = Mathf.Max(velocity.y, -tuning.wallSlideMaxFallSpeed);
        if (consumedJump != input.JumpSequence && input.JumpPressedAt <= now)
        {
            bool expired = now - input.JumpPressedAt > tuning.jumpBuffer;
            bool coyote = groundJumpAvailable && input.JumpPressedAt - lastGrounded <= tuning.coyoteTime;
            if (expired) consumedJump = input.JumpSequence;
            else if (Grounded || coyote)
            {
                consumedJump = input.JumpSequence;
                velocity.y = tuning.jumpVelocity;
                Grounded = false;
                groundJumpAvailable = false;
                lastGrounded = double.NegativeInfinity;
                jumpCutAvailable = true;
                jumpPressTime = input.JumpPressedAt;
                jumpStartedAt = now;
            }
            else if (hasWallJump && wallDirection != 0 && now >= wallLockUntil && now - lastWall <= tuning.wallCoyoteTime)
            {
                consumedJump = input.JumpSequence;
                velocity = new Vector2(-wallDirection * tuning.wallJumpHorizontalSpeed, tuning.wallJumpVelocity);
                Facing = -wallDirection;
                wallLockUntil = now + tuning.wallJumpInputLock;
                lastWall = double.NegativeInfinity; WallSliding = false;
                groundJumpAvailable = false; lastGrounded = double.NegativeInfinity;
                jumpCutAvailable = true; jumpPressTime = input.JumpPressedAt; jumpStartedAt = now;
                WallJumpsPerformed++;
            }
            else if (DoubleJumpAvailable)
            {
                consumedJump = input.JumpSequence;
                velocity.y = tuning.doubleJumpVelocity;
                doubleJumpUsed = true;
                DoubleJumpsPerformed++;
                groundJumpAvailable = false;
                lastGrounded = double.NegativeInfinity;
                jumpCutAvailable = true;
                jumpPressTime = input.JumpPressedAt;
                jumpStartedAt = now;
            }
        }
        if (jumpCutAvailable && velocity.y > 0 && input.JumpReleasedAt >= jumpPressTime && input.JumpReleasedAt <= now)
        {
            // Dynamic input can arrive after several physics ticks at low FPS.
            // Reconstruct the cut tick from the event's actual hold duration,
            // then correct only the ascent already simulated past that tick.
            double held = input.JumpReleasedAt - jumpPressTime;
            double cutAt = jumpStartedAt + System.Math.Ceiling(held / Time.fixedDeltaTime - 0.000001) * Time.fixedDeltaTime;
            float late = (float)System.Math.Max(0, now - cutAt);
            float velocityAtCut = velocity.y + tuning.gravity * late;
            float removed = velocityAtCut * (1 - tuning.jumpCutMultiplier);
            velocity.y -= removed;
            if (late > 0)
                body.position -= Vector2.up * (removed * late);
            jumpCutAvailable = false;
        }
        if (velocity.y <= 0) jumpCutAvailable = false;
        body.linearVelocity = velocity;
    }
}
