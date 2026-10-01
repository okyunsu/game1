using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class RoomSession : MonoBehaviour
{
    public static RoomSession Instance { get; private set; }
    public PlayerInputReader Player { get; private set; }
    public string CurrentRoomId { get; private set; }
    public string LastError { get; private set; }
    public bool Transitioning => state.Transitioning;
    GameState state;
    Rigidbody2D body;
    Scene currentScene;

    public static RoomSession GetOrCreate(GameObject playerPrefab)
    {
        if (Instance != null) return Instance;
        var session = new GameObject("Room Session").AddComponent<RoomSession>();
        var player = Instantiate(playerPrefab, session.transform);
        session.Player = player.GetComponent<PlayerInputReader>();
        session.body = player.GetComponent<Rigidbody2D>();
        return session;
    }
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        state = GameState.GetOrCreate();
        DontDestroyOnLoad(gameObject);
    }
    public void EnterInitial(RoomDefinition room)
    {
        if (!room.TryGetSpawn(room.initialSpawnId, out var spawn))
        {
            LastError = "Missing or duplicate initial Spawn ID";
            Debug.LogWarning(LastError);
            return;
        }
        currentScene = room.gameObject.scene;
        CurrentRoomId = room.roomId;
        Place(spawn, room);
    }
    void Place(RoomSpawn spawn, RoomDefinition room)
    {
        Player.ClearTransientInput();
        // A non-simulated body can retain its old Transform until physics resumes.
        // Move the persistent object as well so the destination trigger cannot
        // observe the previous room's exit position when simulation is enabled.
        Player.transform.position = spawn.transform.position;
        body.position = spawn.transform.position;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0;
        Physics2D.SyncTransforms();
        foreach (var root in room.gameObject.scene.GetRootGameObjects())
        {
            var rig = root.GetComponentInChildren<RoomCameraRig>();
            if (rig != null) rig.BindAndSnap(Player);
        }
    }
    public bool RequestTransition(string roomId, string scenePath, string spawnId)
    {
        if (Transitioning) return false;
        if (state.Paused) return Reject("Cannot enter another room while paused");
        if (string.IsNullOrWhiteSpace(roomId) || string.IsNullOrWhiteSpace(spawnId)
            || !Application.CanStreamedLevelBeLoaded(scenePath))
            return Reject("Invalid destination Scene or ID");
        if (SceneManager.GetSceneByPath(scenePath).isLoaded)
            return Reject("Destination Scene already loaded");
        LastError = null;
        StartCoroutine(Transition(roomId, scenePath, spawnId));
        return true;
    }
    bool Reject(string reason)
    {
        LastError = reason;
        Debug.LogWarning(reason);
        return false;
    }
    IEnumerator Transition(string roomId, string path, string spawnId)
    {
        state.SetTransitioning(true);
        body.simulated = false;
        yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
        var next = SceneManager.GetSceneByPath(path);
        RoomDefinition room = null;
        foreach (var root in next.GetRootGameObjects())
        {
            var candidate = root.GetComponent<RoomDefinition>();
            if (candidate == null) continue;
            if (room != null) { room = null; break; }
            room = candidate;
        }
        if (room == null || room.roomId != roomId || !room.TryGetSpawn(spawnId, out var spawn))
        {
            Reject("Missing, mismatched or duplicate Room/Spawn ID; previous room retained");
            yield return SceneManager.UnloadSceneAsync(next);
            body.simulated = true;
            state.SetTransitioning(false);
            yield break;
        }
        var previous = currentScene;
        currentScene = next;
        CurrentRoomId = room.roomId;
        SceneManager.SetActiveScene(next);
        // Disable old output before the snap; never leave two rendering cameras active.
        foreach (var root in previous.GetRootGameObjects())
            foreach (var cam in root.GetComponentsInChildren<Camera>()) cam.enabled = false;
        Place(spawn, room);
        yield return SceneManager.UnloadSceneAsync(previous);
        body.simulated = true;
        state.SetTransitioning(false);
        Player.ClearTransientInput();
    }
    void OnDestroy() { if (Instance == this) Instance = null; }
}
