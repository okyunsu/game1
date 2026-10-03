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
    public static void E3B03()
    {
        var tuning=ScriptableObject.CreateInstance<E3Tuning>();AssetDatabase.CreateAsset(tuning,"Assets/ScriptableObjects/E3Tuning.asset");
        var shot=new GameObject("E3 shot");var sr=shot.AddComponent<SpriteRenderer>();sr.sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/E1.prefab").GetComponent<SpriteRenderer>().sprite;sr.color=Color.yellow;
        var shape=shot.AddComponent<BoxCollider2D>();shape.size=Vector2.one*1.2f;shape.isTrigger=true;var body=shot.AddComponent<Rigidbody2D>();body.bodyType=RigidbodyType2D.Kinematic;body.gravityScale=0;body.collisionDetectionMode=CollisionDetectionMode2D.Continuous;shot.AddComponent<EnemyShot>();var bullet=PrefabUtility.SaveAsPrefabAsset(shot,"Assets/Prefabs/E3Shot.prefab");UnityEngine.Object.DestroyImmediate(shot);
        var enemy=PrefabUtility.LoadPrefabContents("Assets/Prefabs/E1.prefab");UnityEngine.Object.DestroyImmediate(enemy.GetComponent<EnemyPatrol>());enemy.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Kinematic;enemy.GetComponent<Rigidbody2D>().gravityScale=0;enemy.GetComponent<SpriteRenderer>().color=new Color(.6f,.2f,.85f);var turret=enemy.AddComponent<EnemyTurret>();turret.tuning=tuning;turret.projectilePrefab=bullet;PrefabUtility.SaveAsPrefabAsset(enemy,"Assets/Prefabs/E3.prefab");PrefabUtility.UnloadPrefabContents(enemy);
        EditorSceneManager.OpenScene("Assets/Scenes/Test/CombatTest.unity");Enemy("E3",new Vector2(31,1.51f));EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/Scenes/B02.unity");foreach(var e in UnityEngine.Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))if(e.exitId=="Right")e.enabled=true;EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        var room=Shell("B03");Block(room,"Wall","B03_Cover",new Vector2(14,2),new Vector2(1,2));Block(room,"Landing","B03_TurretBase",new Vector2(26,2),new Vector2(4,2));Enemy("E3",new Vector2(26,3.5f));
        Exit(room,"Left",new Vector2(1.75f,2),"B02","FromRight");Exit(room,"Right",new Vector2(34.25f,2),"B04","FromLeft",false);Spawn(room,"FromLeft",new Vector2(3.5f,1.81f));Spawn(room,"FromRight",new Vector2(32.5f,1.81f));Save("Assets/Scenes/B03.unity");AssetDatabase.SaveAssets();
    }
    public static void B01B02()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/B01.unity");var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
        room.GetComponent<RoomLabel>().notice="B02 →";
        Block(room,"Landing","B01_GJLedge",new Vector2(30.5f,4.1f),new Vector2(9,1));
        Exit(room,"Upper",new Vector2(34.25f,5.6f),"B05","FromLeft",false);
        Exit(room,"Right",new Vector2(34.25f,2),"B02","FromLeft");
        Spawn(room,"FromUpper",new Vector2(28,5.41f));Spawn(room,"FromRight",new Vector2(32.5f,1.81f));
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        room=Shell("B02");Block(room,"Landing","B02_SafeStep",new Vector2(11,2),new Vector2(4,2));
        Exit(room,"Left",new Vector2(1.75f,2),"B01","FromRight");Exit(room,"Right",new Vector2(34.25f,2),"B03","FromLeft",false);
        Spawn(room,"FromLeft",new Vector2(3.5f,1.81f));Spawn(room,"FromRight",new Vector2(32.5f,1.81f));Enemy("E2",new Vector2(24,1.51f));Save("Assets/Scenes/B02.unity");
    }
    public static RoomExit Exit(RoomDefinition room,string id,Vector2 at,string target,string spawn,bool active=true)
    {
        var go=new GameObject("Exit "+id);go.transform.SetParent(room.transform);go.transform.position=at;
        var collider=go.AddComponent<BoxCollider2D>();collider.size=new Vector2(1,2);collider.isTrigger=true;
        var e=go.AddComponent<RoomExit>();e.exitId=id;e.destinationRoomId=target;e.destinationSpawnId=spawn;e.destinationScenePath="Assets/Scenes/"+target+".unity";
        var door=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Door.prefab"));door.transform.SetParent(go.transform,false);door.transform.localScale=new Vector3(1,2,1);
        foreach(var c in door.GetComponentsInChildren<Collider2D>())UnityEngine.Object.DestroyImmediate(c);
        e.enabled=active;return e;
    }
    public static void E2()
    {
        var tuning=ScriptableObject.CreateInstance<E2Tuning>();AssetDatabase.CreateAsset(tuning,"Assets/ScriptableObjects/E2Tuning.asset");
        var go=PrefabUtility.LoadPrefabContents("Assets/Prefabs/E1.prefab");UnityEngine.Object.DestroyImmediate(go.GetComponent<EnemyPatrol>());
        go.name="E2 Charger";go.GetComponent<SpriteRenderer>().color=new Color(.9f,.4f,.15f);go.AddComponent<EnemyCharger>().tuning=tuning;
        PrefabUtility.SaveAsPrefabAsset(go,"Assets/Prefabs/E2.prefab");PrefabUtility.UnloadPrefabContents(go);
        EditorSceneManager.OpenScene("Assets/Scenes/Test/CombatTest.unity");Enemy("E2",new Vector2(25,1.81f));EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
    }
    public static GameObject Enemy(string id,Vector2 at)
    {
        var root=new GameObject(id+" Reset");root.transform.position=at;var reset=root.AddComponent<EnemyReset>();reset.prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/"+id+".prefab");return root;
    }
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
        var inheritedLedge=GameObject.Find("B01_GJLedge");if(inheritedLedge!=null)UnityEngine.Object.DestroyImmediate(inheritedLedge);
        foreach (var exit in room.GetComponentsInChildren<RoomExit>()) UnityEngine.Object.DestroyImmediate(exit.gameObject);
        foreach (var cp in room.GetComponentsInChildren<Checkpoint>()) UnityEngine.Object.DestroyImmediate(cp.gameObject);
        foreach (var spawn in room.GetComponentsInChildren<RoomSpawn>()) UnityEngine.Object.DestroyImmediate(spawn.gameObject);
        room.roomId = id; room.name = id; room.initialSpawnId = "Entry";
        room.GetComponent<RoomLabel>().notice = "";
        Spawn(room,"Entry",new Vector2(3.5f,1.81f));
        return room;
    }
    public static void CleanB02Shell(){EditorSceneManager.OpenScene("Assets/Scenes/B02.unity");var ledge=GameObject.Find("B01_GJLedge");if(ledge!=null)UnityEngine.Object.DestroyImmediate(ledge);EditorSceneManager.SaveScene(SceneManager.GetActiveScene());}
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
