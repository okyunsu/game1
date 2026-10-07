using System;
using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Presentation candidate")]
public sealed class PresentationProfile:ScriptableObject
{
 [Serializable]public sealed class Sound{public string eventName;public AudioClip clip;[Range(0,1),Tooltip("Linear gain (0–1).")]public float volume=.3f;[Range(0,.3f),Tooltip("Random pitch variation around 1 (ratio).")]public float pitchVariation=.04f;}
 [Tooltip("Candidate event clips, gains and pitch variations.")]public Sound[] sounds;
 [Min(0),Tooltip("Same event minimum interval (s).")]public float soundThrottle=.05f;
 [Tooltip("Candidate body colors: player, E1, E2, E3, boss.")]public Color[] bodyColors={new Color(.9f,.98f,1),new Color(1,.72f,.24f),new Color(1,.43f,.4f),new Color(.75f,.65f,1),new Color(1,.45f,.75f)};
 [Tooltip("Eye and identity mark color.")]public Color markColor=new Color(.035f,.045f,.07f);
 [Range(0,1),Tooltip("AV1 hazard sprite opacity (0–1); collider and basic colors unchanged.")]public float hazardOpacity=1;
 [Range(0,20),Tooltip("Walk lean (degrees).")]public float leanDegrees=8;
 [Range(0,.4f),Tooltip("Jump stretch fraction.")]public float jumpStretch=.14f;
 [Range(0,.4f),Tooltip("Landing squash fraction.")]public float landingSquash=.2f;
 [Min(.01f),Tooltip("Landing animation (s).")]public float landingSeconds=.12f;
 [Range(2,3),Tooltip("Dash afterimage count.")]public int afterimages=3;
 [Min(.01f),Tooltip("Afterimage lifetime (s).")]public float trailSeconds=.12f;
 [Min(.01f),Tooltip("Death fragment lifetime (s).")]public float fragmentSeconds=.3f;
 [Min(0),Tooltip("Death fragment speed (u/s).")]public float fragmentSpeed=3;
 [Tooltip("Visual scale ratios: player, E1, E2, E3, boss; collider unchanged.")]public Vector2[] formRatios={Vector2.one,new Vector2(1.18f,.72f),new Vector2(.85f,.95f),new Vector2(.85f,.9f),Vector2.one};
}
