using UnityEngine;

public sealed class B21HazardReturnSettings:ScriptableObject
{
 [Min(.1f),Tooltip("Continuous stable floor contact before recording a safe point, seconds.")] public float safeGroundSeconds=.1f;
 [Min(.5f),Tooltip("Minimum horizontal body clearance from a nearby hazard edge, Unity units.")] public float edgeMargin=.5f;
 [Min(0),Tooltip("Pause before returning to the latest safe point, seconds.")] public float pauseSeconds=.12f;
 [Min(0),Tooltip("Fade to black before safe-point return, seconds.")] public float fadeSeconds=.15f;
 [Min(0),Tooltip("Input lock and invulnerability after safe-point return, seconds.")] public float getUpTime=.45f;
}
