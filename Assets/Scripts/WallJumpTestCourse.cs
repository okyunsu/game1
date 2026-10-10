using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class WallJumpTestCourse : MonoBehaviour
{
    public Vector2 guidePosition = new Vector2(390,16);
    [Min(10), Tooltip("Test guide font size, pixels at 1280x720.")] public int guideFontSize = 16;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot()
    {
        if (!N5Args.Has("-walljumptest")) return;
        var go = new GameObject("WallJumpTest launch"); DontDestroyOnLoad(go);
        go.AddComponent<WallJumpTestLaunch>();
    }
    IEnumerator Start()
    {
        while (RoomSession.Instance?.Player == null) yield return null;
        ProgressSave.Enabled = false;
        var player = RoomSession.Instance.Player;
        player.GetComponent<PlayerDash>().hasDash = true;
        player.GetComponent<PlayerMotor>().hasWallJump = true;
        player.GetComponent<PlayerMotor>().hasDoubleJump = false;
    }
    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { fontSize = guideFontSize, wordWrap = true };
        var old = GUI.matrix; float scale = Mathf.Min(Screen.width/1280f,Screen.height/720f);
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale,scale,1));
        GUI.Label(new Rect(guidePosition.x, guidePosition.y, Screen.width/scale-guidePosition.x-12,70),
            "WallJumpTest — 벽 쪽 입력으로 붙기 / Z·A 벽 점프 / C·B 대시 / ↓+X 포고\n3·4·5u 굴뚝 → 반대 발판 → 대시 → 포고 → 일반 벽 대조",style);
        GUI.matrix = old;
    }
}
public sealed class WallJumpTestLaunch : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; ProgressSave.Enabled=false; ProgressSave.Pending=null;
        SceneManager.LoadScene("Assets/Scenes/Test/WallJumpTest.unity"); Destroy(gameObject);
    }
}
