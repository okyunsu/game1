using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerInputReader))]
public sealed class PlayerMotor : MonoBehaviour
{
    [SerializeField, Tooltip("Single source of shared movement values. Changes apply on the next physics tick.")] PlayerTuning tuning;
    [SerializeField, Tooltip("Only this layer can provide floor contact.")] LayerMask groundLayers;
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
        consumedJump = input.JumpSequence;
        lastGrounded = double.NegativeInfinity;
        groundJumpAvailable = false;
        jumpCutAvailable = false;
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
            lastGrounded = now;
            groundJumpAvailable = true;
        }

        if(Mathf.Abs(input.Move.x)>.01f) Facing=input.Move.x>0?1:-1;
        var dash=GetComponent<PlayerDash>();
        if(dash!=null && dash.Tick(Grounded,now)) { consumedJump=input.JumpSequence;jumpCutAvailable=false;groundJumpAvailable=false;lastGrounded=double.NegativeInfinity;return; }
        Vector2 velocity = body.linearVelocity;
        float target = Mathf.Clamp(input.Move.x, -1, 1) * tuning.moveSpeed;
        float rate = Mathf.Abs(target) < .001f || target * velocity.x < 0 ? tuning.deceleration : tuning.acceleration;
        velocity.x = Mathf.MoveTowards(velocity.x, target, rate * (Grounded ? 1 : tuning.airControl) * Time.fixedDeltaTime);
        float gravity = tuning.gravity * (velocity.y <= 0 ? tuning.fallGravityMultiplier : 1);
        velocity.y = Mathf.Max(velocity.y - gravity * Time.fixedDeltaTime, -tuning.maxFallSpeed);
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
