using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Combat Tuning")]
public sealed class CombatTuning:ScriptableObject
{
    [Header("Player health (Prototype Values)")]
    [Min(1),Tooltip("Maximum player HP.")] public int playerMaxHP=5;
    [Min(0),Tooltip("Invincibility after ordinary damage, seconds.")] public float invincibility=1;
    [Min(0),Tooltip("Player horizontal knockback, units/second.")] public float knockbackX=4;
    [Min(0),Tooltip("Player vertical knockback, units/second.")] public float knockbackY=3;
    [Min(0),Tooltip("Movement lock during knockback, seconds.")] public float knockbackLock=.12f;
}
