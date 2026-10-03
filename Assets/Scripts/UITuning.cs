using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/UI Tuning")]
public sealed class UITuning : ScriptableObject
{
    [Header("HUD — reference pixels at 1280 × 720")]
    [Tooltip("Top-left HUD offset in reference pixels.")] public Vector2 hudPosition=new(16,16);
    [Tooltip("HUD panel width and height in reference pixels.")] public Vector2 hudSize=new(350,118);
    [Min(8),Tooltip("HP square width/height in reference pixels.")] public float hpPipSize=18;
    [Min(0),Tooltip("Space between HP squares in reference pixels.")] public float hpPipGap=7;
    [Min(12),Tooltip("Ability symbol square width/height in reference pixels.")] public float abilityIconSize=24;
    [Header("Notices — seconds / reference pixels")]
    [Min(0),Tooltip("First controls and acquisition notice duration in seconds; presentation only.")] public float noticeSeconds=3;
    [Min(0),Tooltip("Death shade fade-in seconds, bounded by existing Respawning state. Never changes respawn duration.")] public float deathDisplaySeconds=.15f;
    [Range(0,1),Tooltip("Maximum death shade opacity.")] public float deathOpacity=.65f;
    [Min(20),Tooltip("Notice panel height in reference pixels.")] public float noticeHeight=44;
    [Header("Typography — reference pixels")]
    [Min(10),Tooltip("HUD and notice font size in reference pixels.")] public int fontSize=16;
}
