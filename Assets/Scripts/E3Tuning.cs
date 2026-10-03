using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/E3 Tuning")]
public sealed class E3Tuning:ScriptableObject
{
    [Header("E3 combat")]
    [Min(1),Tooltip("Maximum hit points.")]public int maxHP=2;
    [Min(0),Tooltip("Damage per accepted projectile hit.")]public int projectileDamage=1;
    [Min(0),Tooltip("Received horizontal knockback (u/s).")]public float knockback=3;
    [Header("Fixed direction single shot")]
    [Min(0),Tooltip("Detection distance (u).")]public float detectionRange=7;
    [Min(0),Tooltip("Visible warning duration (seconds).")]public float warningSeconds=.7f;
    [Min(0),Tooltip("Time between warning starts (seconds).")]public float shotPeriod=2;
    [Min(0),Tooltip("Projectile velocity (u/s).")]public float projectileSpeed=5;
}
