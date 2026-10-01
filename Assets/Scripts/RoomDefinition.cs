using UnityEngine;

public sealed class RoomDefinition : MonoBehaviour
{
    [Tooltip("Stable room ID, matching the authored world graph.")] public string roomId;
    [Tooltip("Starting point when opening this room directly in Editor.")] public string initialSpawnId = "Entry";
    [Tooltip("Reusable player prefab. Only one persistent player is instantiated.")] public GameObject playerPrefab;

    void Start()
    {
        var session = RoomSession.GetOrCreate(playerPrefab);
        if (!session.Transitioning && string.IsNullOrEmpty(session.CurrentRoomId)) session.EnterInitial(this);
    }
    public bool TryGetSpawn(string id, out RoomSpawn spawn)
    {
        spawn = null;
        foreach (var candidate in GetComponentsInChildren<RoomSpawn>())
        {
            if (candidate.spawnId != id) continue;
            if (spawn != null) { spawn = null; return false; }
            spawn = candidate;
        }
        return spawn != null;
    }
}
