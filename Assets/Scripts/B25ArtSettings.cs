using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Original Art Settings")]
public sealed class B25ArtSettings:ScriptableObject
{
 public Sprite player,enemy,e1,e2,e3,e4,fragment;
 [Min(.01f),Tooltip("Enemy death display fade duration, seconds. Does not delay actor death.")]public float deathSeconds=.3f;
 [Min(.1f),Tooltip("Pickup/fragment/checkpoint idle sparkle cycle, seconds.")]public float sparkleSeconds=1.5f;
 [Range(0,1),Tooltip("Background displacement / camera displacement. 0 fixes the background; 1 follows the camera.")]public float parallax=.3f;
}
