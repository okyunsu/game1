using UnityEngine;

public sealed class B21CornerFixSettings:ScriptableObject
{
 [Range(.15f,.35f),Tooltip("Largest horizontal overlap with a head corner eligible for correction, Unity units.")] public float maxOverlap=.25f;
 [Min(.001f),Tooltip("Extra clearance beyond the two physics contact margins, Unity units.")] public float skin=.01f;
 [Tooltip("Experimental foot-corner dash correction. Default off; Development argument can opt in for comparison.")] public bool dashCornerEnabled;
 [Range(.05f,.35f),Tooltip("Largest ledge overlap at the feet eligible for dash correction, Unity units.")] public float maxDashStep=.25f;
}
