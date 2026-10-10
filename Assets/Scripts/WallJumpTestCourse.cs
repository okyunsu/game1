using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public sealed class WallJumpTestCourse : MonoBehaviour
{
    [System.Serializable]
    public sealed class Preset
    {
        public string label;
        [Tooltip("Wall slide maximum downward speed, u/s.")] public float slide;
        [Tooltip("Wall jump horizontal speed, u/s.")] public float horizontal;
        [Tooltip("Wall jump upward speed, u/s.")] public float velocity;
        [Tooltip("Wall jump input lock, seconds.")] public float inputLock;
    }
    public Preset[] presets = {
        new Preset{label="1 기본",slide=2.5f,horizontal=8,velocity=12,inputLock=.15f},
        new Preset{label="2 끈적",slide=1.5f,horizontal=7,velocity=12,inputLock=.20f},
        new Preset{label="3 빠름",slide=3.5f,horizontal=9.5f,velocity=12.5f,inputLock=.12f}
    };
    public int PresetIndex { get; private set; }
    PlayerTuning original,runtimeTuning; PlayerMotor motor;
    public string CurrentPresetName => presets[PresetIndex].label;
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
        motor=player.GetComponent<PlayerMotor>();original=motor.Tuning;runtimeTuning=Instantiate(original);
        runtimeTuning.name="WallJumpTest runtime tuning (not saved)";motor.SetWallJumpTestTuning(runtimeTuning);SelectPreset(0);
    }
    public void SelectPreset(int index)
    {
        if(runtimeTuning==null||index<0||index>=presets.Length)return;
        PresetIndex=index;var p=presets[index];runtimeTuning.wallSlideMaxFallSpeed=p.slide;
        runtimeTuning.wallJumpHorizontalSpeed=p.horizontal;runtimeTuning.wallJumpVelocity=p.velocity;runtimeTuning.wallJumpInputLock=p.inputLock;
        motor.ClearWallState();
    }
    void Update()
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if(Keyboard.current==null||RoomSession.Instance?.Player?.InputLocked!=false)return;
        if(Keyboard.current.digit1Key.wasPressedThisFrame)SelectPreset(0);
        if(Keyboard.current.digit2Key.wasPressedThisFrame)SelectPreset(1);
        if(Keyboard.current.digit3Key.wasPressedThisFrame)SelectPreset(2);
        #endif
    }
    void OnDestroy(){if(motor!=null&&original!=null)motor.SetWallJumpTestTuning(original);if(runtimeTuning!=null)Destroy(runtimeTuning);}
    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { fontSize = guideFontSize, wordWrap = true };
        var old = GUI.matrix; float scale = Mathf.Min(Screen.width/1280f,Screen.height/720f);
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale,scale,1));
        GUI.Label(new Rect(guidePosition.x, guidePosition.y, Screen.width/scale-guidePosition.x-12,70),
            "WallJumpTest — 벽 쪽 입력으로 붙기 / Z·A 벽 점프 / C·B 대시 / ↓+X 포고\n3·4·5u 굴뚝 → 반대 발판 → 대시 → 포고 → 일반 벽 대조\n프리셋 "+CurrentPresetName+" — 숫자 1/2/3 (Asset 변경 없음)",style);
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
