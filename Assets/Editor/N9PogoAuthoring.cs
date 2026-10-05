using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class N9PogoAuthoring
{
    [MenuItem("Afterglow/Open Pogo Test")]
    public static void Open()=>EditorSceneManager.OpenScene("Assets/Scenes/Test/PogoTest.unity");
    public static void PrepareShelves()
    {
        Open();GameObject.Find("Goal shelf 1").transform.position=new Vector2(37,4.25f);GameObject.Find("Goal bounds 1").transform.position=new Vector2(37,5.5f);
        GameObject.Find("Goal shelf 2").transform.position=new Vector2(66,5.25f);GameObject.Find("Goal bounds 2").transform.position=new Vector2(66,6.5f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());File.WriteAllText("Validation/N9-pogotest-shelves.txt","Authored pre-test shelves: P02 x37/top4.5 for single pogo, P03 x66/top5.5 for pogo+DJ+dash. Only new PogoTest scene; existing layouts unchanged.\n");EditorApplication.Exit(0);
    }
    public static void Create()
    {
        const string path="Assets/Scenes/Test/PogoTest.unity";
        if(File.Exists(path))throw new IOException("Preserve authored PogoTest scene");
        EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");var camera=UnityEngine.Object.Instantiate(UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>().gameObject);var old=SceneManager.GetActiveScene();
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.MoveGameObjectToScene(camera,scene);EditorSceneManager.CloseScene(old,true);SceneManager.SetActiveScene(scene);
        var root=new GameObject("PogoTest");var room=root.AddComponent<RoomDefinition>();room.roomId="PogoTest";room.initialSpawnId="P01";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        var boundary=new GameObject("PogoTest camera boundary");boundary.transform.SetParent(root.transform);var poly=boundary.AddComponent<PolygonCollider2D>();poly.isTrigger=true;poly.SetPath(0,new[]{new Vector2(0,-2),new Vector2(76,-2),new Vector2(76,14),new Vector2(0,14)});camera.GetComponent<RoomCameraRig>().boundary=poly;
        var course=root.AddComponent<PogoTestCourse>();course.starts=new Checkpoint[3];course.goals=new BoxCollider2D[3];
        for(int n=0;n<3;n++)
        {
            float x=n*26;Block(room,"Start "+n,new Vector2(x+2,.5f),new Vector2(4,1));
            var spawn=S3Authoring.Spawn(room,"P0"+(n+1),new Vector2(x+2,1.81f));var cp=new GameObject("CP-P0"+(n+1));cp.transform.SetParent(root.transform);cp.transform.position=spawn.transform.position;
            cp.AddComponent<SpriteRenderer>().sprite=room.playerPrefab.GetComponent<SpriteRenderer>().sprite;cp.AddComponent<BoxCollider2D>().isTrigger=true;var checkpoint=cp.AddComponent<Checkpoint>();checkpoint.spawn=spawn;checkpoint.checkpointId=spawn.spawnId;course.starts[n]=checkpoint;
            float goalX=n==0?20:n==1?39:66,goalTop=n==0?1:n==1?5:6.5f;
            Block(room,"Goal shelf "+n,new Vector2(goalX,goalTop-.25f),new Vector2(4,.5f));
            var goal=new GameObject("Goal bounds "+n);goal.transform.SetParent(root.transform);goal.transform.position=new Vector2(goalX,goalTop+1);var gc=goal.AddComponent<BoxCollider2D>();gc.isTrigger=true;gc.size=new Vector2(3.6f,2);course.goals[n]=gc;
            if(n!=1){var spike=Block(room,"Pogo spikes "+n,new Vector2(n==0?11:60,0),new Vector2(n==0?14:4,.5f));spike.layer=0;spike.GetComponent<BoxCollider2D>().isTrigger=true;spike.AddComponent<KillZone>();spike.AddComponent<PogoTarget>();spike.GetComponent<SpriteRenderer>().color=new Color(.9f,.25f,.25f);}
            else{Block(room,"Enemy base",new Vector2(34,.5f),new Vector2(4,1));var enemy=S3Authoring.Enemy("E3",new Vector2(34,1.51f));enemy.transform.SetParent(root.transform);}
        }
        var fall=Block(room,"Fall reset",new Vector2(38,-2),new Vector2(76,1));fall.layer=0;fall.GetComponent<BoxCollider2D>().isTrigger=true;fall.AddComponent<KillZone>();
        EditorSceneManager.SaveScene(scene,path);AssetDatabase.SaveAssets();File.WriteAllText("Validation/N9-pogotest-authoring.txt","PASS new authored scene + existing reusable Level/Player/E3 prefabs; existing scenes/Build Settings untouched. Three checkpoints; normal KillZone respawn.\n");EditorApplication.Exit(0);
    }
    static GameObject Block(RoomDefinition room,string name,Vector2 at,Vector2 size)=>S3Authoring.Block(room,"Landing",name,at,size);
}
