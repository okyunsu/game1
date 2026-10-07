using UnityEngine;

// B03's new optional entrance requires a deliberate upward input.
public sealed class OptionalRoomEntry:MonoBehaviour
{
 [Tooltip("Scene-authored instruction displayed above this optional entrance.")]public string instruction="↑ 선택 대시 도전";
 [Tooltip("Destination room ID.")]public string destinationRoomId="B06";
 [Tooltip("Destination scene asset path.")]public string destinationScenePath="Assets/Scenes/B06.unity";
 [Tooltip("Destination spawn ID.")]public string destinationSpawnId="Entry";
 void OnTriggerStay2D(Collider2D other){var player=other.GetComponent<PlayerInputReader>();var session=RoomSession.Instance;if(player==null||session==null||session.Player!=player||player.InputLocked||player.Move.y<.5f)return;session.RequestTransition(destinationRoomId,destinationScenePath,destinationSpawnId);}
 void OnGUI(){var camera=Camera.main;if(camera==null)return;Vector3 p=camera.WorldToScreenPoint(transform.position+Vector3.up*1.7f);if(p.z>0)GUI.Label(new Rect(p.x-80,Screen.height-p.y-12,180,30),instruction);}
}
