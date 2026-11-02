using UnityEngine;
[CreateAssetMenu(menuName="Afterglow/Original Art Settings")]
public sealed class B25ArtSettings:ScriptableObject
{
 public Sprite player,enemy;
 [Range(0,1),Tooltip("Background displacement / camera displacement. 0 fixes the background; 1 follows the camera.")]public float parallax=.3f;
}
