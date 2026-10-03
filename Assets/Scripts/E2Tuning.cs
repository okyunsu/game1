using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/E2 Tuning")]
public sealed class E2Tuning:ScriptableObject
{
    [Header("E2 combat")]
    [Min(1),Tooltip("Maximum hit points.")]public int maxHP=3;
    [Min(0),Tooltip("Contact damage per accepted hit.")]public int contactDamage=1;
    [Min(0),Tooltip("Horizontal hit knockback velocity (u/s).")]public float knockback=3;
    [Header("Detection and charge")]
    [Min(0),Tooltip("Player detection distance (u).")]public float detectionRange=5;
    [Min(0),Tooltip("Visible warning duration (seconds).")]public float warningSeconds=.6f;
    [Min(0),Tooltip("Fixed direction charge duration (seconds).")]public float chargeSeconds=.4f;
    [Min(0),Tooltip("Charge speed (u/s).")]public float chargeSpeed=6;
    [Min(0),Tooltip("Recovery duration (seconds).")]public float recoverySeconds=1;
}
