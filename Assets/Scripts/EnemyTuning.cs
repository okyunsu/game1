using UnityEngine;
[CreateAssetMenu(menuName="Game/Enemy Tuning")]
public sealed class EnemyTuning : ScriptableObject
{
    [Header("E1 Prototype Values")]
    [Tooltip("Maximum hit points.")] public int maxHP = 2;
    [Tooltip("Patrol speed (u/s).")] public float speed = 1.8f;
    [Tooltip("Damage per accepted contact.")] public int contactDamage = 1;
    [Tooltip("Horizontal velocity applied by a hit (u/s).")] public float knockback = 3f;
}
