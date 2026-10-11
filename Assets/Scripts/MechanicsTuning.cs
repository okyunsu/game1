using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Mechanics Tuning")]
public sealed class MechanicsTuning:ScriptableObject
{
 [Min(0),Tooltip("Slam downward speed, u/s.")] public float slamSpeed=24;
 [Min(0),Tooltip("Slam landing attack width, u.")] public float slamWidth=1.6f;
 [Min(0),Tooltip("Slam landing recovery, seconds.")] public float slamRecovery=.15f;
 [Min(1),Tooltip("Slam landing damage, HP.")] public int slamDamage=1;
 [Min(0),Tooltip("Slam pad bounce apex above contact, u.")] public float padHeight=7.5f;
 [Min(0),Tooltip("Steam rising speed, u/s.")] public float steamSpeed=8;
 [Min(0.01f),Tooltip("Steam enabled duration, seconds.")] public float steamOn=2;
 [Min(0.01f),Tooltip("Steam disabled duration, seconds.")] public float steamOff=2;
 [Min(0.01f),Tooltip("Lift movement speed, u/s.")] public float liftSpeed=3;
 [Min(0),Tooltip("Crumble delay after standing contact, seconds.")] public float crumbleDelay=.6f;
 [Min(0),Tooltip("Crumble restoration delay after falling, seconds.")] public float crumbleRestore=3;
 [Min(0),Tooltip("Crumble falling acceleration, u/s squared.")] public float crumbleGravity=30;
 [Min(1),Tooltip("E4 health, HP.")] public int e4HP=2;
 [Min(1),Tooltip("E4 contact damage, HP.")] public int e4Damage=1;
 [Min(0),Tooltip("E4 vertical hover amplitude, u.")] public float e4Amplitude=.5f;
 [Min(.01f),Tooltip("E4 hover period, seconds.")] public float e4Period=2;
}
