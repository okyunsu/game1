using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Feedback Tuning")]
public sealed class FeedbackTuning:ScriptableObject
{
 [Header("Attack display")][Range(0,1),Tooltip("Slash opacity (0–1).")]public float slashAlpha=.7f;
 [Tooltip("Slash color.")]public Color slashColor=new Color(1,1,.55f,1);
 [Header("Hit overlay")][Min(0),Tooltip("Overlay duration (scaled seconds).")]public float hitFlashSeconds=.10f;
 [Header("Hit stop")][Range(0,.2f),Tooltip("Hit pause duration (real-time seconds). Zero disables hit stop.")]public float hitStopSeconds=.05f;
 [Header("Enemy HP pips")][Min(.01f),Tooltip("Square side (world u).")]public float hpPipSize=.18f;
 [Min(0),Tooltip("Square gap (world u).")]public float hpPipGap=.06f;
 [Tooltip("Offset above collider top (world u).")]public float hpPipOffsetY=.35f;
 [Header("Death display")][Min(.01f),Tooltip("Pop duration (scaled seconds).")]public float deathPopSeconds=.20f;
 [Min(1),Tooltip("Final size multiplier.")]public float deathPopScale=1.6f;
}
