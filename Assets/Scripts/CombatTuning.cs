using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Combat Tuning")]
public sealed class CombatTuning:ScriptableObject
{
    [Header("Attack (Prototype Values)")]
    [Min(1),Tooltip("Damage per target, HP.")] public int attackDamage=1;
    [Min(0),Tooltip("Range from player collider front, units.")] public float attackRange=1.1f;
    [Min(0),Tooltip("Attack hitbox height, units.")] public float attackHeight=1.2f;
    [Min(0),Tooltip("Startup duration, seconds.")] public float attackStartup=.08f;
    [Min(0),Tooltip("Active duration, seconds.")] public float attackActive=.10f;
    [Min(0),Tooltip("Cooldown from attack start, seconds.")] public float attackCooldown=.35f;
    [Header("Player health (Prototype Values)")]
    [Min(1),Tooltip("Maximum player HP.")] public int playerMaxHP=5;
    [Min(0),Tooltip("Invincibility after ordinary damage, seconds.")] public float invincibility=1;
    [Min(0),Tooltip("Player horizontal knockback, units/second.")] public float knockbackX=4;
    [Min(0),Tooltip("Player vertical knockback, units/second.")] public float knockbackY=3;
    [Min(0),Tooltip("Movement lock during knockback, seconds.")] public float knockbackLock=.12f;
}
