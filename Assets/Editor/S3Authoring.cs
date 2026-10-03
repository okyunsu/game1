using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Explicit stage entry points; never run implicitly during import.
public static class S3Authoring
{
    public static void DoubleJump()
    {
        var root = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Player.prefab");
        root.GetComponent<PlayerMotor>().hasDoubleJump = false;
        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/Player.prefab");
        root.GetComponent<PlayerMotor>().hasDoubleJump = true;
        var testPlayer = PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/DoubleJumpTestPlayer.prefab");
        PrefabUtility.UnloadPrefabContents(root);
        var tuning = AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/ScriptableObjects/PlayerTuning.asset");
        tuning.doubleJumpVelocity = 12;
        EditorUtility.SetDirty(tuning);
        var room = Shell("DoubleJumpTest");
        room.playerPrefab = testPlayer;
        Block(room, "Landing", "Test_GJLedge", new Vector2(30.5f,4.1f), new Vector2(9,1));
        Save("Assets/Scenes/Test/DoubleJumpTest.unity");
        AssetDatabase.SaveAssets();
    }
    public static RoomDefinition Shell(string id)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/B01.unity");
        var room = UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
        foreach (var exit in room.GetComponentsInChildren<RoomExit>()) UnityEngine.Object.DestroyImmediate(exit.gameObject);
        foreach (var cp in room.GetComponentsInChildren<Checkpoint>()) UnityEngine.Object.DestroyImmediate(cp.gameObject);
        foreach (var spawn in room.GetComponentsInChildren<RoomSpawn>()) UnityEngine.Object.DestroyImmediate(spawn.gameObject);
        room.roomId = id; room.name = id; room.initialSpawnId = "Entry";
        room.GetComponent<RoomLabel>().notice = "";
        Spawn(room,"Entry",new Vector2(3.5f,1.81f));
        return room;
    }
    public static GameObject Block(RoomDefinition room,string prefab,string name,Vector2 at,Vector2 scale)
    {
        var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Level/{prefab}.prefab"));
        go.name=name;go.transform.SetParent(room.transform);go.transform.position=at;go.transform.localScale=new Vector3(scale.x,scale.y,1);return go;
    }
    public static RoomSpawn Spawn(RoomDefinition room,string id,Vector2 at)
    {
        var go=new GameObject(id);go.transform.SetParent(room.transform);go.transform.position=at;var s=go.AddComponent<RoomSpawn>();s.spawnId=id;return s;
    }
    public static void Save(string path)
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(),path);
        var scenes=EditorBuildSettings.scenes.ToList();
        if(!scenes.Any(s=>s.path==path))scenes.Add(new EditorBuildSettingsScene(path,true));
        EditorBuildSettings.scenes=scenes.ToArray();
    }
}
