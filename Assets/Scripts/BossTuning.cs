using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Boss Tuning")]
public sealed class BossTuning:ScriptableObject
{
 [Header("B2 candidate only")]
 [Tooltip("Enable the experimental four-pattern cycle. Basic asset remains false.")]public bool extendedPatterns=false;
 [Min(0),Tooltip("Walk between arena anchors (u/s).")]public float walkSpeed=2.5f;
 [Tooltip("Left walk anchor, world x (u).")]public float leftAnchor=5;
 [Tooltip("Right walk anchor, world x (u).")]public float rightAnchor=17.5f;
 [Min(0),Tooltip("Leap landing warning (s).")]public float leapTell=.7f;
 [Min(.01f),Tooltip("Leap flight duration (s).")]public float leapFlight=.6f;
 [Min(0),Tooltip("Leap arc above standing center (u).")]public float leapHeight=4;
 [Min(.1f),Tooltip("Landing damage width (u).")]public float slamWidth=3;
 [Min(0),Tooltip("Charge warning (s).")]public float chargeTell=.6f;
 [Min(0),Tooltip("Charge horizontal speed (u/s).")]public float chargeSpeed=9;
 [Min(0),Tooltip("Boss clearance before arena wall (u).")]public float wallClearance=.1f;
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
