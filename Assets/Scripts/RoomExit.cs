using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class RoomExit : MonoBehaviour
{
    [Tooltip("Unique exit ID in this room.")] public string exitId;
    [Tooltip("Expected room ID in the destination Scene.")] public string destinationRoomId;
    [Tooltip("Scene asset path included in Build Settings.")] public string destinationScenePath;
    [Tooltip("Unique Spawn ID within the destination room.")] public string destinationSpawnId;

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerInputReader>();
        var session = RoomSession.Instance;
        if (player != null && session != null && session.Player == player)
            session.RequestTransition(destinationRoomId, destinationScenePath, destinationSpawnId);
    }
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
}
