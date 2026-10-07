using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B02Rooms
{
    public static void WidenReturnLanding()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/A06.unity");
        GameObject.Find("Raised precision").transform.localScale=new Vector3(6,.5f,1);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorApplication.Exit(0);
    }
    public static void Create()
    {
        string id=N5Args.Value("-b02-room", "A06");
        if(id is not ("A06" or "B06" or "C05"))throw new ArgumentException(id);
        string path="Assets/Scenes/"+id+".unity";
        if(File.Exists(path))throw new IOException("Preserve authored scene "+path);
        string source=id=="A06"?"A04":id=="B06"?"B03":"C03";
        // Copy only the established camera; author the new room from existing prefabs.
        EditorSceneManager.OpenScene("Assets/Scenes/"+source+".unity");
        var camera=UnityEngine.Object.Instantiate(UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>().gameObject);
        var previous=SceneManager.GetActiveScene();
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        SceneManager.MoveGameObjectToScene(camera,scene);EditorSceneManager.CloseScene(previous,true);SceneManager.SetActiveScene(scene);
        var root=new GameObject(id);var room=root.AddComponent<RoomDefinition>();room.roomId=id;
        room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        root.AddComponent<RoomLabel>().notice="선택 도전 · 발견 후 같은 입구로 돌아가기";
        Block(room,"Wall","Left Wall",new Vector2(.5f,7),new Vector2(1,14));
        Block(room,"Wall","Right Wall",new Vector2(35.5f,7),new Vector2(1,14));
        Block(room,"Ceiling","Ceiling",new Vector2(18,14.5f),new Vector2(36,1));
        Block(room,"KillZone","Fall reset",new Vector2(18,-1),new Vector2(36,1));
        Block(room,"Landing","Entry landing",new Vector2(4,.5f),new Vector2(8,1));
        S3Authoring.Spawn(room,"Entry",new Vector2(3.5f,1.81f));
        S3Authoring.Exit(room,"Return",new Vector2(1.75f,2),source,"FromLeft");
        var boundary=new GameObject("Camera Boundary");boundary.transform.SetParent(root.transform);
        var polygon=boundary.AddComponent<PolygonCollider2D>();polygon.isTrigger=true;
        polygon.SetPath(0,new[]{new Vector2(0,0),new Vector2(36,0),new Vector2(36,14),new Vector2(0,14)});
        camera.GetComponent<RoomCameraRig>().boundary=polygon;
        if(id=="A06")
        {
            // Tests downward timing, then a higher return landing, without either ability.
            Pogo(room,10.5f,0,5);Landing(room,"Pogo landing",15,1,4);
            Landing(room,"Raised precision",21,2.2f,3);
            Pogo(room,25.5f,.8f,4);Landing(room,"Outlook",31,2.4f,6);
        }
        else if(id=="B06")
        {
            // Tests dash distance and pogo restoration before a second crossing; no double jump.
            Pogo(room,11.5f,0,7);Landing(room,"Dash landing",18,1,3);
            Pogo(room,23.5f,0,6);Landing(room,"Return staging",30,1,8);
        }
        else
        {
            // Tests vertical entry, dash gap and pogo landing in one compact route.
            Landing(room,"Double jump terrace",12,4,4);
            Pogo(room,18,1.3f,6);Landing(room,"Air dash terrace",24,4,4);
            Landing(room,"Final outlook",31,6.5f,6);
        }
        float goalY=id=="A06"?3.4f:id=="B06"?2:7.5f;
        var reward=new GameObject("Discovery "+id);reward.transform.SetParent(root.transform);reward.transform.position=new Vector2(32,goalY);
        reward.transform.localScale=Vector3.one*.65f;
        var sprite=reward.AddComponent<SpriteRenderer>();sprite.sprite=room.playerPrefab.GetComponent<SpriteRenderer>().sprite;sprite.color=new Color(.5f,1,1);sprite.sortingOrder=8;
        reward.AddComponent<BoxCollider2D>().isTrigger=true;reward.AddComponent<DiscoveryReward>().candidateId=id;
        // An existing enemy type placed on the end landing, reset through the established component.
        var reset=new GameObject("E3 Reset");reset.transform.SetParent(root.transform);
        reset.transform.position=new Vector2(30,goalY-.49f);reset.AddComponent<EnemyReset>().prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/E3.prefab");
        EditorSceneManager.SaveScene(scene,path);
        EditorSceneManager.OpenScene("Assets/Scenes/"+source+".unity");
        var sourceRoom=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
        // Branch is above the main ground route. Existing objects and spawns stay in place.
        S3Authoring.Exit(sourceRoom,"Optional "+id,new Vector2(6,id=="C05"?6:4.4f),id,"Entry");
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
        File.WriteAllText("Validation/_raw/B02/"+id+"-authoring.txt","Authored "+path+" and only branch door in "+source+"; Build Settings unchanged.\n");
        EditorApplication.Exit(0);
    }
    static void Landing(RoomDefinition room,string name,float x,float top,float width)=>Block(room,"Landing",name,new Vector2(x,top-.25f),new Vector2(width,.5f));
    static void Pogo(RoomDefinition room,float x,float y,float width)
    {
        var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PogoTarget.prefab"));
        go.name="Pogo crossing";go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.transform.localScale=new Vector3(width,.5f,1);
    }
    static GameObject Block(RoomDefinition room,string prefab,string name,Vector2 at,Vector2 size)=>S3Authoring.Block(room,prefab,name,at,size);
}
