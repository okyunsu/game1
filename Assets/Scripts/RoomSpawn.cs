using UnityEngine;

public sealed class RoomSpawn : MonoBehaviour
{
    [Tooltip("Unique within this room. Arrival point is the player's center, in world units.")] public string spawnId;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(.7f, 1.6f, 0));
    }
}
