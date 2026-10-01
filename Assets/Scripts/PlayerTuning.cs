using UnityEngine;

[CreateAssetMenu(menuName = "Afterglow/Player Tuning")]
public sealed class PlayerTuning : ScriptableObject
{
    [Header("Horizontal movement (Prototype Values)")]
    [Range(5, 7), Tooltip("Maximum horizontal speed, units/second.")] public float moveSpeed = 6;
    [Range(60, 100), Tooltip("Horizontal acceleration, units/second².")] public float acceleration = 80;
    [Range(80, 120), Tooltip("Braking and direction reversal, units/second².")] public float deceleration = 100;
    [Range(.8f, 1), Tooltip("Air acceleration/braking multiplier; does not reduce maximum speed.")] public float airControl = .9f;
    [Header("Vertical movement (Prototype Values)")]
    [Range(11, 13), Tooltip("Initial upward velocity, units/second. Independent of mass.")] public float jumpVelocity = 12;
    [Range(26, 34), Tooltip("Downward acceleration, units/second²; Rigidbody gravity scale stays zero.")] public float gravity = 30;
    [Range(1.2f, 1.8f), Tooltip("Gravity multiplier while descending.")] public float fallGravityMultiplier = 1.5f;
    [Range(16, 22), Tooltip("Maximum downward speed, units/second.")] public float maxFallSpeed = 18;
    [Range(.4f, .7f), Tooltip("Multiply upward velocity once when releasing the jump button.")] public float jumpCutMultiplier = .5f;
    [Header("Input forgiveness (seconds)")]
    [Range(.06f, .14f), Tooltip("A fresh jump press within this time after last floor contact may jump once.")] public float coyoteTime = .10f;
    [Range(.08f, .16f), Tooltip("An unconsumed jump press remains available until landing within this time.")] public float jumpBuffer = .12f;
    [Header("Death / respawn (seconds)")]
    [Min(0), Tooltip("Real-time death delay before checkpoint return. Prototype value: 0.6 seconds.")] public float respawnDelay = .6f;

    void OnValidate()
    {
        moveSpeed = Mathf.Clamp(moveSpeed, 5, 7);
        acceleration = Mathf.Clamp(acceleration, 60, 100);
        deceleration = Mathf.Clamp(deceleration, 80, 120);
        airControl = Mathf.Clamp(airControl, .8f, 1);
        jumpVelocity = Mathf.Clamp(jumpVelocity, 11, 13);
        gravity = Mathf.Clamp(gravity, 26, 34);
        fallGravityMultiplier = Mathf.Clamp(fallGravityMultiplier, 1.2f, 1.8f);
        maxFallSpeed = Mathf.Clamp(maxFallSpeed, 16, 22);
        jumpCutMultiplier = Mathf.Clamp(jumpCutMultiplier, .4f, .7f);
        coyoteTime = Mathf.Clamp(coyoteTime, .06f, .14f);
        jumpBuffer = Mathf.Clamp(jumpBuffer, .08f, .16f);
    }
}
