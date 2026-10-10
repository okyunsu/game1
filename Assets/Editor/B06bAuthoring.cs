using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B06bAuthoring
{
    public static void TestCourse()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Test/WallJumpTest.unity");
        if(GameObject.Find("Single wall 12u")!=null)throw new IOException("Preserve already authored course");
        var room=Object.FindFirstObjectByType<RoomDefinition>();var course=room.GetComponent<WallJumpTestCourse>();
        course.presets=new[]{new WallJumpTestCourse.Preset{label="1 기준",shortHorizontal=8,shortLock=.15f},new WallJumpTestCourse.Preset{label="2 짧은 킥",shortHorizontal=3.5f,shortLock=.06f},new WallJumpTestCourse.Preset{label="3 짧은 킥 + 상승 부착",shortHorizontal=3.5f,shortLock=.06f,rising=true}};
        var wall=S3Authoring.Block(room,"Landing","Single wall 12u",new Vector2(114,7),new Vector2(.5f,12));wall.AddComponent<ClimbableWall>();wall.GetComponent<SpriteRenderer>().color=Color.cyan;
        S3Authoring.Block(room,"Landing","Single wall summit",new Vector2(115.75f,12.75f),new Vector2(3,.5f));
        S3Authoring.Block(room,"Landing","Added safe floor",new Vector2(119,.5f),new Vector2(22,1));
        var poly=Object.FindFirstObjectByType<RoomCameraRig>().boundary;poly.SetPath(0,new[]{new Vector2(0,-2),new Vector2(130,-2),new Vector2(130,20),new Vector2(0,20)});
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorApplication.Exit(0);
    }
    public static void Metrics()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Test/WallJumpTest.unity");var room=Object.FindFirstObjectByType<RoomDefinition>();
        if(room.GetComponent<WallJumpTestMetrics>()!=null)throw new IOException("Preserve metrics");
        room.gameObject.AddComponent<WallJumpTestMetrics>().zones=new[]{Zone("3u chimney",7,14),Zone("4u chimney",21,29),Zone("5u chimney",35,44),Zone("Single 12u",108,118)};
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorApplication.Exit(0);
    }
    static WallJumpTestMetrics.Zone Zone(string label,float left,float right)=>new WallJumpTestMetrics.Zone{label=label,xRange=new Vector2(left,right)};
}

