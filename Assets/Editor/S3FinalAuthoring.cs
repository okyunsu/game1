using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class S3FinalAuthoring
{
    public static void C01C02()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/B05.unity");
        var room = UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
        room.GetComponent<RoomLabel>().notice = "";
        S3Authoring.Block(room, "Landing", "B05_Step", new Vector2(18, 2), new Vector2(4, 2));
        S3Authoring.Exit(room, "Right", new Vector2(34.25f, 2), "C01", "FromLeft");
        S3Authoring.Spawn(room, "FromRight", new Vector2(32.5f, 1.81f));
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        room = S3Authoring.Shell("C01");
        room.GetComponent<RoomLabel>().notice = "정상 관측탑 — 대시와 더블 점프를 모두 쓴다";
        S3Authoring.Exit(room, "Left", new Vector2(1.75f, 2), "B05", "FromRight");
        S3Authoring.Exit(room, "Right", new Vector2(34.25f, 2), "C02", "FromLeft");
        S3Authoring.Spawn(room, "FromLeft", new Vector2(3.5f, 1.81f));
        S3Authoring.Spawn(room, "FromRight", new Vector2(32.5f, 1.81f));
        AddCheckpoint(room, "CP-C01", 8);
        S3Authoring.Save("Assets/Scenes/C01.unity");
        room = S3Authoring.Shell("C02");
        S3Authoring.Block(room, "Landing", "C02_Start", new Vector2(8, 2), new Vector2(4, 2));
        S3Authoring.Block(room, "Landing", "C02_High1", new Vector2(14, 6.3f), new Vector2(4, 1));
        S3Authoring.Block(room, "Landing", "C02_High2", new Vector2(23, 6.3f), new Vector2(4, 1));
        S3Authoring.Block(room, "Landing", "C02_ExitLedge", new Vector2(31, 6.3f), new Vector2(8, 1));
        S3Authoring.Exit(room, "Left", new Vector2(1.75f, 2), "C01", "FromRight");
        S3Authoring.Exit(room, "Right", new Vector2(34.25f, 7.8f), "C03", "FromLeft", false);
        S3Authoring.Spawn(room, "FromLeft", new Vector2(3.5f, 1.81f));
        S3Authoring.Spawn(room, "FromRight", new Vector2(32, 7.61f));
        S3Authoring.Save("Assets/Scenes/C02.unity");
    }
    public static void AddCheckpoint(RoomDefinition room, string id, float x)
    {
        var go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Checkpoint.prefab"));
        go.name = id; go.transform.SetParent(room.transform); go.transform.position = new Vector2(x, 1.8f);
        var cp = go.GetComponent<Checkpoint>(); cp.checkpointId = id;
        cp.spawn.spawnId = id; cp.spawn.transform.position = new Vector2(x, 1.81f);
        PrefabUtility.RecordPrefabInstancePropertyModifications(cp);
        PrefabUtility.RecordPrefabInstancePropertyModifications(cp.spawn);
        PrefabUtility.RecordPrefabInstancePropertyModifications(cp.spawn.transform);
    }
    public static void B02()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/B02.unity");
        var reset = UnityEngine.Object.FindFirstObjectByType<EnemyReset>();
        if (Mathf.Abs(reset.transform.position.x - 24) > .001f) throw new Exception("Unexpected E2 placement");
        var p = reset.transform.position; p.x = 17; reset.transform.position = p;
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
}
