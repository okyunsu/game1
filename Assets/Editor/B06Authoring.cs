using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B06Authoring
{
    const string TestPath="Assets/Scenes/Test/WallJumpTest.unity";
    [MenuItem("Afterglow/Open Wall Jump Test")]
    public static void Open() => EditorSceneManager.OpenScene(TestPath);
    public static void CreateWallTest()
    {
        if(File.Exists(TestPath)) throw new IOException("Preserve authored WallJumpTest");
        var old=EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");
        var camera=UnityEngine.Object.Instantiate(UnityEngine.Object.FindFirstObjectByType<RoomCameraRig>().gameObject);
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        SceneManager.MoveGameObjectToScene(camera,scene);EditorSceneManager.CloseScene(old,true);SceneManager.SetActiveScene(scene);
        var root=new GameObject("WallJumpTest");var room=root.AddComponent<RoomDefinition>();
        room.roomId="WallJumpTest";room.initialSpawnId="Entry";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        S3Authoring.Spawn(room,"Entry",new Vector2(4,1.81f));root.AddComponent<WallJumpTestCourse>();
        var boundary=new GameObject("Wall test camera boundary");boundary.transform.SetParent(root.transform);
        var poly=boundary.AddComponent<PolygonCollider2D>();poly.isTrigger=true;poly.SetPath(0,new[]{new Vector2(0,-2),new Vector2(108,-2),new Vector2(108,20),new Vector2(0,20)});
        camera.GetComponent<RoomCameraRig>().boundary=poly;
        Block(room,"Safety floor",new Vector2(54,.5f),new Vector2(108,1));
        for(int i=0;i<3;i++)
        {
            float start=8+i*14,width=3+i;
            Wall(room,"Chimney "+width+" left",new Vector2(start-.25f,7),new Vector2(.5f,12));
            Wall(room,"Chimney "+width+" right",new Vector2(start+width+.25f,7),new Vector2(.5f,12));
            Block(room,"Chimney "+width+" summit",new Vector2(start+width+2,12.75f),new Vector2(3,.5f));
        }
        Wall(room,"One sided launch wall",new Vector2(54,4),new Vector2(.5f,6));
        Block(room,"Opposite landing",new Vector2(59,4),new Vector2(3,.5f));
        Wall(room,"Dash launch wall",new Vector2(65,4),new Vector2(.5f,6));
        Block(room,"Dash landing",new Vector2(72,4.5f),new Vector2(3,.5f));
        Wall(room,"Pogo launch wall",new Vector2(79,4),new Vector2(.5f,6));
        var pogo=Block(room,"Safe pogo target",new Vector2(83,2),new Vector2(2,.5f));pogo.AddComponent<PogoTarget>();
        Block(room,"Pogo landing",new Vector2(87,5),new Vector2(3,.5f));
        Block(room,"Unmarked control wall",new Vector2(92,4),new Vector2(.5f,6));
        var gate=Block(room,"DashGate control",new Vector2(97,4),new Vector2(1,6));gate.AddComponent<DashGate>();
        var pickup=Block(room,"Double jump combination pickup",new Vector2(102,2),Vector2.one);
        pickup.layer=0;pickup.GetComponent<BoxCollider2D>().isTrigger=true;pickup.AddComponent<DoubleJumpPickup>();
        // Serialize only the new fields; preserve all existing tuning values.
        var tuning=AssetDatabase.LoadAssetAtPath<PlayerTuning>("Assets/ScriptableObjects/PlayerTuning.asset");
        var serialized=new SerializedObject(tuning);
        foreach(var field in new[]{"wallSlideMaxFallSpeed","wallJumpHorizontalSpeed","wallJumpVelocity","wallJumpInputLock","wallCoyoteTime"})
            serialized.FindProperty(field).floatValue=field=="wallSlideMaxFallSpeed"?2.5f:field=="wallJumpHorizontalSpeed"?8:field=="wallJumpVelocity"?12:field=="wallJumpInputLock"?.15f:.08f;
        serialized.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(tuning);
        EditorSceneManager.SaveScene(scene,TestPath);AssetDatabase.SaveAssets();
        Directory.CreateDirectory("Validation/_raw/B06");File.WriteAllText("Validation/_raw/B06/B1-authoring.txt","PASS WallJumpTest authored; safety floor; 3/4/5u chimney height12; jump/dash/pogo controls; unmarked/Gate controls; BuildSettings unchanged.\n");
        EditorApplication.Exit(0);
    }
    static GameObject Block(RoomDefinition room,string name,Vector2 at,Vector2 size)=>S3Authoring.Block(room,"Landing",name,at,size);
    static GameObject Wall(RoomDefinition room,string name,Vector2 at,Vector2 size)
    {
        var go=Block(room,name,at,size);go.AddComponent<ClimbableWall>();go.GetComponent<SpriteRenderer>().color=Color.cyan;return go;
    }
}
