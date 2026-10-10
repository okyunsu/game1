using System.Collections;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public sealed class WallJumpTestCourse : MonoBehaviour
{
    [System.Serializable] public sealed class Preset
    {
        public string label;
        [Range(0,11),Tooltip("Short kick away speed, u/s.")] public float shortHorizontal;
        [Range(0,.25f),Tooltip("Short kick lock, seconds.")] public float shortLock;
        [Tooltip("Attach while rising without removing upward velocity.")] public bool rising;
    }
    public Preset[] presets={
        new Preset{label="1 기준",shortHorizontal=8,shortLock=.15f},
        new Preset{label="2 짧은 킥",shortHorizontal=3.5f,shortLock=.06f},
        new Preset{label="3 짧은 킥 + 상승 부착",shortHorizontal=3.5f,shortLock=.06f,rising=true}};
    public int PresetIndex {get;private set;}
    PlayerTuning original,runtimeTuning;PlayerMotor motor;
    public string CurrentPresetName=>presets[PresetIndex].label;
    public Vector2 guidePosition=new Vector2(390,16);
    [Min(10),Tooltip("Guide font size, pixels at 1280x720.")]public int guideFontSize=16;
    public string TuningText=>runtimeTuning==null?"준비 중":string.Format(CultureInfo.InvariantCulture,
        "{0}\nslide {1:0.00}u/s · vertical {2:0.00}u/s · coyote {3:0.00}s\nLong {4:0.00}u/s · lock {5:0.00}s\nShort {6:0.00}u/s · lock {7:0.00}s · rising {8}",
        CurrentPresetName,runtimeTuning.wallSlideMaxFallSpeed,runtimeTuning.wallJumpVelocity,runtimeTuning.wallCoyoteTime,
        runtimeTuning.wallJumpHorizontalSpeed,runtimeTuning.wallJumpInputLock,runtimeTuning.wallJumpShortHorizontalSpeed,runtimeTuning.wallJumpShortInputLock,runtimeTuning.wallAttachWhileRising);
    public string DeviceGuide=>"벽 쪽 방향키: 짧은 킥 / 중립·반대: 긴 킥 · Z 점프 · C 대시 · ↓+X 포고";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Boot()
    {if(!N5Args.Has("-walljumptest"))return;var go=new GameObject("WallJumpTest launch");DontDestroyOnLoad(go);go.AddComponent<WallJumpTestLaunch>();}
    IEnumerator Start()
    {
        while(RoomSession.Instance?.Player==null)yield return null;
        ProgressSave.Enabled=false;var player=RoomSession.Instance.Player;player.GetComponent<PlayerDash>().hasDash=true;
        motor=player.GetComponent<PlayerMotor>();motor.hasWallJump=true;motor.hasDoubleJump=false;
        original=motor.Tuning;runtimeTuning=Instantiate(original);runtimeTuning.name="WallJumpTest runtime tuning (not saved)";
        motor.SetWallJumpTestTuning(runtimeTuning);SelectPreset(0);
    }
    public void SelectPreset(int index)
    {
        if(runtimeTuning==null||index<0||index>=presets.Length)return;
        PresetIndex=index;var p=presets[index];runtimeTuning.wallJumpShortHorizontalSpeed=p.shortHorizontal;
        runtimeTuning.wallJumpShortInputLock=p.shortLock;runtimeTuning.wallAttachWhileRising=p.rising;
        motor.ClearWallState();WriteTuning();
    }
    void WriteTuning(){Directory.CreateDirectory("Logs");File.WriteAllText("Logs/walljump-tuning-last.txt",TuningText+"\n");}
    void Update()
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if(RoomSession.Instance?.CurrentRoomId!="WallJumpTest"||runtimeTuning==null||Keyboard.current==null||RoomSession.Instance.Player.InputLocked)return;
        var k=Keyboard.current;
        if(k.digit1Key.wasPressedThisFrame)SelectPreset(0);if(k.digit2Key.wasPressedThisFrame)SelectPreset(1);if(k.digit3Key.wasPressedThisFrame)SelectPreset(2);
        bool changed=false;
        if(k.digit7Key.wasPressedThisFrame){runtimeTuning.wallJumpShortHorizontalSpeed=Mathf.Clamp(runtimeTuning.wallJumpShortHorizontalSpeed-.5f,0,11);changed=true;}
        if(k.digit8Key.wasPressedThisFrame){runtimeTuning.wallJumpShortHorizontalSpeed=Mathf.Clamp(runtimeTuning.wallJumpShortHorizontalSpeed+.5f,0,11);changed=true;}
        if(k.digit9Key.wasPressedThisFrame){runtimeTuning.wallJumpShortInputLock=Mathf.Clamp(runtimeTuning.wallJumpShortInputLock-.01f,0,.25f);changed=true;}
        if(k.digit0Key.wasPressedThisFrame){runtimeTuning.wallJumpShortInputLock=Mathf.Clamp(runtimeTuning.wallJumpShortInputLock+.01f,0,.25f);changed=true;}
        if(k.digit6Key.wasPressedThisFrame){runtimeTuning.wallAttachWhileRising=!runtimeTuning.wallAttachWhileRising;changed=true;}
        if(changed){motor.ClearWallState();WriteTuning();}
        #endif
    }
    void OnDestroy(){if(motor!=null&&original!=null)motor.SetWallJumpTestTuning(original);if(runtimeTuning!=null)Destroy(runtimeTuning);}
    void OnGUI()
    {
        var style=new GUIStyle(GUI.skin.label){fontSize=guideFontSize,wordWrap=true};var old=GUI.matrix;float scale=Mathf.Min(Screen.width/1280f,Screen.height/720f);
        GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));
        GUI.Label(new Rect(guidePosition.x,guidePosition.y,1280-guidePosition.x-12,65),"WallJumpTest — "+DeviceGuide+"\n3·4·5u 굴뚝 / x114 한쪽 12u 벽 · 1/2/3 후보 · 7/8 속도 · 9/0 잠금 · 6 상승 부착",style);
        GUI.Label(new Rect(830,88,438,120),TuningText+"\nLogs/walljump-tuning-last.txt (Asset 변경 없음)",style);GUI.matrix=old;
    }
}
public sealed class WallJumpTestLaunch : MonoBehaviour
{IEnumerator Start(){yield return null;ProgressSave.Enabled=false;ProgressSave.Pending=null;SceneManager.LoadScene("Assets/Scenes/Test/WallJumpTest.unity");Destroy(gameObject);}}

