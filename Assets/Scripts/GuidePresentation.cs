using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GuidePresentation : MonoBehaviour
{
    public static GuidePresentation Instance { get; private set; }
    static readonly bool disabled=N5Args.Has("-no-guide");
    public static bool Disabled => disabled;
    public GuideObjectives objectives;
    public GuideGraph graph;
    public GuideObjectivesV3 v3Objectives;
    [Min(10),Tooltip("Goal font size in 1280x720 reference pixels.")] public int fontSize=16;
    bool debug;
    GUIStyle labelStyle;
    public bool DebugVisible => Active&&!Disabled&&debug;
    bool Active => (V2SceneMap.Enabled||V3SceneMap.Enabled)&&RoomSession.Instance?.Player!=null&&!RoomSession.Instance.Player.InputLocked&&RoomSession.Instance.CurrentRoomId!="WallJumpTest"&&RoomSession.Instance.CurrentRoomId!="PogoTest";
    public int Stage
    {
        get
        {
            var p=RoomSession.Instance?.Player;if(p==null||objectives==null)return -1;
            if(V3SceneMap.Enabled) return v3Objectives==null?-1:v3Objectives.Stage(p.GetComponent<PlayerDash>().hasDash,p.GetComponent<PlayerMotor>().hasWallJump,p.GetComponent<PlayerMotor>().hasDoubleJump,ProgressSave.BossDefeated,ProgressSave.EndingReached,ProgressSave.HasVisited("B01"),ProgressSave.HasVisited("B05"),ProgressSave.HasVisited("C01"));
            return objectives.Stage(p.GetComponent<PlayerDash>().hasDash,p.GetComponent<PlayerMotor>().hasDoubleJump,ProgressSave.BossDefeated);
        }
    }
    public GuideObjectives.Objective CurrentObjective => Stage<0?null:V3SceneMap.Enabled?v3Objectives.objectives[Stage]:objectives.objectives[Stage];
    public string GoalText => Active&&!Disabled?CurrentObjective?.text??"":"";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot(){if(Instance==null)new GameObject("Goal and development guide").AddComponent<GuidePresentation>();}
    void Awake(){Instance=this;DontDestroyOnLoad(gameObject);objectives=Resources.Load<GuideObjectives>("GuideObjectives");v3Objectives=Resources.Load<GuideObjectivesV3>("GuideObjectives_V3");graph=Resources.Load<GuideGraph>(V3SceneMap.Enabled?"GuideGraph_V3":"GuideGraph");}
    void Update()
    {
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if(!Disabled&&Active&&Keyboard.current?.f1Key.wasPressedThisFrame==true)debug=!debug;
        #endif
    }
    void OnGUI()
    {
        if(!Active||Disabled)return;
        var old=GUI.matrix;float scale=Mathf.Min(Screen.width/1280f,Screen.height/720f);float width=Screen.width/scale,height=Screen.height/scale;
        GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));
        labelStyle??=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter};labelStyle.fontSize=fontSize;var style=labelStyle;
        int stage=Stage;var objective=stage<0?null:V3SceneMap.Enabled?v3Objectives.objectives[stage]:objectives.objectives[stage];
        GUI.Label(new Rect(380,16,width-760,58),objective?.text??"",style);
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if(DebugVisible&&stage>=0)
        {
            string room=RoomSession.Instance.CurrentRoomId,target=objective.roomId;
            GUI.Label(new Rect(380,78,width-400,32),"F1 | "+room+" | 단계 "+(stage+1)+" | 목표 "+target,style);
            if(graph!=null&&graph.NextExit(room,target,out var next))
            {
                Vector2 delta=next.position-(Vector2)RoomSession.Instance.Player.transform.position;
                bool horizontal=Mathf.Abs(delta.x)>=Mathf.Abs(delta.y);
                string arrow=horizontal?(delta.x>=0?"→":"←"):(delta.y>=0?"↑":"↓");
                var at=horizontal?new Rect(delta.x>=0?width-95:15,height*.5f,80,60):new Rect(width*.5f-40,delta.y>=0?115:height-75,80,60);
                GUI.Label(at,arrow+" "+next.to,style);
            }
        }
        #endif
        GUI.matrix=old;
    }
    void OnDestroy(){if(Instance==this)Instance=null;}
}
