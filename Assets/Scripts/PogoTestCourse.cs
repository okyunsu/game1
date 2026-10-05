using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PogoTestCourse : MonoBehaviour
{
    [Header("Authored section starts / goals")]
    public Checkpoint[] starts;
    public BoxCollider2D[] goals;
    public int Section { get; private set; }
    public int Completed { get; private set; }
    public int SectionPogos => RoomSession.Instance.Player.GetComponent<PlayerAttack>().PogosPerformed-initialPogos;
    int initialPogos,initialJumps;bool changing,sawDash;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot(){if(!N5Args.Has("-pogotest"))return;var go=new GameObject("PogoTest launch");DontDestroyOnLoad(go);go.AddComponent<PogoTestLaunch>();}
    IEnumerator Start(){while(RoomSession.Instance?.Player==null)yield return null;ProgressSave.Enabled=false;BeginSection(0);}
    public void BeginSection(int section)
    {
        Section=section;var s=RoomSession.Instance;var p=s.Player;
        ProgressSave.Enabled=false;p.ClearTransientInput();p.GetComponent<PlayerMotor>().hasDoubleJump=true;p.GetComponent<PlayerDash>().hasDash=true;
        s.ActivateCheckpoint(starts[section]);var body=p.GetComponent<Rigidbody2D>();body.position=starts[section].spawn.transform.position;p.transform.position=body.position;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();
        initialPogos=p.GetComponent<PlayerAttack>().PogosPerformed;initialJumps=p.GetComponent<PlayerMotor>().DoubleJumpsPerformed;sawDash=false;
    }
    void Update()
    {
        var s=RoomSession.Instance;if(s?.Player==null||s.Respawning||changing)return;
        var p=s.Player;if(p.GetComponent<PlayerDash>().IsDashing)sawDash=true;
        bool needed=SectionPogos>=(Section==0?3:1)&&(Section!=2||p.GetComponent<PlayerMotor>().DoubleJumpsPerformed>initialJumps&&sawDash);
        if(needed&&p.GetComponent<PlayerMotor>().Grounded&&goals[Section].bounds.Contains(p.transform.position)){Completed=Mathf.Max(Completed,Section+1);Debug.Log("PogoTest P0"+(Section+1)+" goal");if(Section<2&&!N5Args.Has("-verify-pogotest"))StartCoroutine(Next());}
    }
    IEnumerator Next(){changing=true;yield return new WaitForSecondsRealtime(.6f);BeginSection(Section+1);changing=false;}
    void OnGUI(){GUI.Label(new Rect(260,16,1000,50),"PogoTest "+(Section+1)+"/3 — "+new[]{"가시 위 포고 3회 후 오른쪽 착지","적 위 포고로 높은 선반","포고 → 더블 점프 → 대시"}[Section]+" | 공중 ↓+X / 스틱 아래+X");if(Completed==3)GUI.Label(new Rect(260,56,800,30),"3구간 완료 — Esc로 타이틀 복귀");}
}
public sealed class PogoTestLaunch : MonoBehaviour
{
    IEnumerator Start(){yield return null;ProgressSave.Enabled=false;ProgressSave.Pending=null;SceneManager.LoadScene("Assets/Scenes/Test/PogoTest.unity");Destroy(gameObject);}
}
