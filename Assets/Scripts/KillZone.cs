using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public sealed class KillZone : MonoBehaviour
{
 void OnTriggerEnter2D(Collider2D other) {
  var session=RoomSession.Instance;
  if(session!=null && other.GetComponent<PlayerInputReader>()==session.Player) session.Die();
 }
 void OnValidate(){GetComponent<BoxCollider2D>().isTrigger=true;}
}
