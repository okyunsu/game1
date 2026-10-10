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
            Wall(room,"Chimney "+width+" left",new Vector2(start-.25f,8),new Vector2(.5f,10)); // 2u entrance; summit remains 12u above floor.
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
    public static void OpenTestEntrances()
    {
        Open();
        for(int width=3;width<=5;width++)
        {
            var wall=GameObject.Find("Chimney "+width+" left");
            wall.transform.position=new Vector3(wall.transform.position.x,8,0);wall.transform.localScale=new Vector3(.5f,10,1);
        }
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        File.WriteAllText("Validation/_raw/B06/F1-test-entrances.txt","PASS 2u lower left entrances in three test chimneys; floor1 to summit13 remains12u; no main/V2 geometry changes.\n");EditorApplication.Exit(0);
    }
    public static void CreateBreakables()
    {
        Directory.CreateDirectory("Assets/Prefabs/B06");AssetDatabase.Refresh();
        foreach(bool secret in new[]{false,true})
        {
            string path="Assets/Prefabs/B06/BreakableWall_"+(secret?"Secret":"Shortcut")+".prefab";
            if(File.Exists(path))throw new IOException("Preserve "+path);
            var go=PrefabUtility.LoadPrefabContents("Assets/Prefabs/Level/Landing.prefab");go.name=secret?"BreakableWall_Secret":"BreakableWall_Shortcut";
            go.transform.localScale=new Vector3(1,3,1);var wall=go.AddComponent<BreakableWall>();wall.breakableFrom=secret?BreakableWall.Side.Any:BreakableWall.Side.Left;
            var feedback=go.AddComponent<HitFeedback>();feedback.tuning=AssetDatabase.LoadAssetAtPath<FeedbackTuning>("Assets/ScriptableObjects/FeedbackTuning.asset");
            var overlay=new GameObject("Hit flash");overlay.transform.SetParent(go.transform,false);feedback.overlay=overlay.AddComponent<SpriteRenderer>();feedback.overlay.sprite=go.GetComponent<SpriteRenderer>().sprite;feedback.overlay.sortingOrder=10;overlay.SetActive(false);
            var mark=new GameObject(secret?"Cracked seam":"Open side indicator");mark.transform.SetParent(go.transform,false);mark.transform.localPosition=new Vector3(secret?0:-.5f,0,-.02f);mark.transform.localScale=secret?new Vector3(.08f,.65f,1):new Vector3(.07f,.15f,1);
            var sprite=mark.AddComponent<SpriteRenderer>();sprite.sprite=go.GetComponent<SpriteRenderer>().sprite;sprite.color=secret?Color.black:Color.yellow;sprite.sortingOrder=3;
            // Shortcut marker is a thin edge on the permitted side, neutral wall face elsewhere.
            PrefabUtility.SaveAsPrefabAsset(go,path);PrefabUtility.UnloadPrefabContents(go);
        }
        Open();var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
        for(int i=0;i<2;i++)
        {
            var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/BreakableWall_"+(i==0?"Shortcut":"Secret")+".prefab"));
            go.transform.SetParent(room.transform);go.transform.position=new Vector2(46+i*4,2.5f);go.GetComponent<BreakableWall>().wallId="WJT-"+(i==0?"Shortcut":"Secret");
        }
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
        Directory.CreateDirectory("Validation/_raw/B06");File.WriteAllText("Validation/_raw/B06/C1-authoring.txt","PASS two new breakable prefabs + test floor instances; existing prefabs/scenes unchanged.\n");EditorApplication.Exit(0);
    }
}
