using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Boss Tuning")]
public sealed class BossTuning:ScriptableObject
{
 [Header("Adopted B2-fast cycle")]
 [Tooltip("Enable the adopted four-pattern cycle. B3 uses its additional switches below.")]public bool extendedPatterns=false;
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
 [Header("B3 candidate — individual pattern switches")]
 [Tooltip("Eight-pattern candidate cycle; default boss remains B2-fast.")] public bool b3Patterns;
 [Tooltip("Enable original melee.")] public bool meleeEnabled=true;
 [Tooltip("Enable original leap.")] public bool leapEnabled=true;
 [Tooltip("Enable original wave.")] public bool waveEnabled=true;
 [Tooltip("Enable original charge.")] public bool chargeEnabled=true;
 [Tooltip("Enable tracking melee combo.")] public bool chaseEnabled;
 [Tooltip("Enable consecutive leaps.")] public bool comboLeapEnabled;
 [Tooltip("Enable backstep projectiles.")] public bool backshotEnabled;
 [Tooltip("Enable falling objects.")] public bool fallingEnabled;
 [Header("Tracking combo (seconds / world units)")]
 [Min(.4f),Tooltip("Initial combo warning (s).")] public float chaseTell=.5f;
 [Min(0),Tooltip("Warning before each strike (s).")] public float chaseStrikeTell=.25f;
 [Min(.01f),Tooltip("Time between combo strikes (s).")] public float chaseInterval=.35f;
 [Min(1),Tooltip("Strikes in phase one.")] public int chaseCount=3;
 [Min(0),Tooltip("Tracking horizontal speed (u/s).")] public float chaseSpeed=3;
 [Min(0),Tooltip("Maximum distance from combo start (u).")] public float chaseDistance=4;
 [Min(.1f),Tooltip("Front strike width (u).")] public float chaseRange=2.5f;
 [Min(1),Tooltip("Recovery after combo (s).")] public float chaseRecovery=1.2f;
 [Header("Consecutive leaps")]
 [Min(.4f),Tooltip("First landing warning (s).")] public float comboFirstTell=.7f;
 [Min(.4f),Tooltip("Following landing warning (s).")] public float comboNextTell=.5f;
 [Min(0),Tooltip("Delay after landing before next warning (s).")] public float comboLandingGap=.3f;
 [Min(1),Tooltip("Leaps in phase one.")] public int comboLeapCount=2;
 [Min(1),Tooltip("Recovery after final landing (s).")] public float comboLeapRecovery=1.5f;
 [Header("Backstep projectiles")]
 [Tooltip("Reusable B3 projectile prefab.")] public GameObject projectilePrefab;
 [Min(.4f),Tooltip("Initial backstep warning (s).")] public float backshotTell=.6f;
 [Min(0),Tooltip("Backstep distance (u).")] public float backstepDistance=3;
 [Min(.01f),Tooltip("Backstep duration (s).")] public float backstepDuration=.3f;
 [Min(1),Tooltip("Projectile count (low/high alternating).")] public int backshotCount=3;
 [Min(.01f),Tooltip("Time between projectiles (s).")] public float backshotInterval=.45f;
 [Min(0),Tooltip("Projectile speed (u/s).")] public float projectileSpeed=7;
 [Min(.1f),Tooltip("Projectile width and height (u).")] public float projectileSize=.6f;
 [Min(0),Tooltip("Low projectile center above floor (u).")] public float projectileLow=.5f;
 [Min(0),Tooltip("High projectile center above floor (u).")] public float projectileHigh=2.8f;
 [Min(1),Tooltip("Recovery after last shot (s).")] public float backshotRecovery=1.2f;
 [Header("Falling objects")]
 [Tooltip("Reusable B3 falling object prefab.")] public GameObject fallingPrefab;
 [Min(.4f),Tooltip("Boss floor slam warning (s).")] public float fallingTell=.8f;
 [Min(.4f),Tooltip("Landing zone warning (s).")] public float fallingZoneTell=1;
 [Min(.1f),Tooltip("Landing zone/object width (u).")] public float fallingWidth=1.5f;
 [Min(.1f),Tooltip("Offset between zone centers (u).")] public float fallingSpacing=3;
 [Min(.1f),Tooltip("Object height (u).")] public float fallingHeight=.8f;
 [Min(0),Tooltip("Ceiling spawn center above arena floor (u).")] public float fallingCeiling=14;
 [Min(.1f),Tooltip("Falling speed (u/s).")] public float fallingSpeed=10;
 [Min(1),Tooltip("Recovery after all objects land (s).")] public float fallingRecovery=1.2f;
 [Header("B3 phase two")]
 [Tooltip("Enable the half-HP phase transition.")] public bool phaseTwoEnabled;
 [Range(0,1),Tooltip("HP threshold / maximum HP.")] public float phaseTwoThreshold=.5f;
 [Min(.4f),Tooltip("Phase transition warning / pause (s).")] public float phaseTwoTell=.6f;
 [Min(0),Tooltip("Phase two recovery multiplier.")] public float phaseTwoRecoveryScale=.8f;
 [Min(.8f),Tooltip("Minimum phase two recovery (s).")] public float phaseTwoRecoveryMinimum=.8f;
 [Min(1),Tooltip("Tracking strikes in phase two.")] public int phaseTwoChaseCount=4;
 [Min(1),Tooltip("Consecutive leaps in phase two.")] public int phaseTwoLeapCount=3;

}



