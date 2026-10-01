using UnityEngine;
[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public sealed class Checkpoint : MonoBehaviour
{
 [Tooltip("Unique fixed checkpoint ID, e.g. CP-A01.")] public string checkpointId;
 [Tooltip("Safe player center on respawn.")] public RoomSpawn spawn;
 [Tooltip("Inactive checkpoint color.")] public Color inactiveColor = new Color(.25f,.55f,.65f);
 [Tooltip("Active checkpoint color.")] public Color activeColor = new Color(.4f,1,.6f);
 void OnTriggerEnter2D(Collider2D other) {
  var session=RoomSession.Instance;
  if(session!=null && other.GetComponent<PlayerInputReader>()==session.Player) session.ActivateCheckpoint(this);
 }
 void Update() { GetComponent<SpriteRenderer>().color=RoomSession.Instance!=null && RoomSession.Instance.CheckpointId==checkpointId?activeColor:inactiveColor; }
 void OnValidate(){GetComponent<BoxCollider2D>().isTrigger=true;}
}
