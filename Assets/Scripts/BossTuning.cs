using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Boss Tuning")]
public sealed class BossTuning:ScriptableObject
{
 [Header("Health / damage")]
 [Min(1),Tooltip("Boss starting HP.")]public int maxHP=20;
 [Min(1),Tooltip("Melee, wave and contact damage (HP).")]public int damage=1;
 [Header("Pattern time")]
 [Min(0),Tooltip("Melee warning (s).")]public float meleeTell=.8f;
 [Min(0),Tooltip("Wave warning (s).")]public float waveTell=1;
 [Min(0),Tooltip("Recovery after each strike (s).")]public float recovery=1.2f;
 [Header("Attack geometry")]
 [Min(0),Tooltip("Melee reach from boss front (u).")]public float meleeRange=3;
 [Min(0),Tooltip("Wave horizontal speed (u/s).")]public float waveSpeed=5;
 [Min(.1f),Tooltip("Wave height above floor (u).")]public float waveHeight=1;
}
