using UnityEngine;
using UnityEngine.SceneManagement;

// Presentation observer. Does not write gameplay state or save data.
public sealed class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }
    public UITuning tuning;
    public bool FirstNoticeVisible => input!=null&&!input.Paused&&firstNotice&&Time.unscaledTimeAsDouble<firstUntil;
    public bool AcquisitionVisible => input!=null&&!input.Paused&&Time.unscaledTimeAsDouble<acquisitionUntil;
    public bool DeathOverlayVisible => RoomSession.Instance?.Player!=null&&RoomSession.Instance.Respawning;
    public int VisibleHP => health!=null?health.HP:0;
    public bool DashOwned => dash!=null&&dash.hasDash;
    public bool DoubleJumpOwned => motor!=null&&motor.hasDoubleJump;
    public string AcquisitionText => input!=null&&input.UsingGamepad
        ? (acquiredDash?"대시 획득 — B":"더블 점프 획득 — 공중에서 A 한 번 더")
        : (acquiredDash?"대시 획득 — C":"더블 점프 획득 — 공중에서 Z 한 번 더");
    PlayerInputReader input; PlayerHealth health; PlayerDash dash; PlayerMotor motor;
    RoomSession observedSession; RoomLabel label;
    bool pendingNewGame,firstNotice,previousDash,previousJump,acquiredDash,wasDead;
    double firstUntil,acquisitionUntil,deathStarted;
    GUIStyle text,centered; int styleSize;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if(Instance!=null)return;
        new GameObject("Game HUD — presentation only").AddComponent<GameHUD>();
    }
    void Awake()
    {
        if(Instance!=null&&Instance!=this){Destroy(gameObject);return;}
        Instance=this; DontDestroyOnLoad(gameObject);
        tuning=Resources.Load<UITuning>("UITuning");
        if(tuning==null){Debug.LogError("Missing Resources/UITuning asset");enabled=false;return;}
        SceneManager.sceneLoaded+=SceneLoaded;
    }
    void SceneLoaded(Scene scene,LoadSceneMode mode)
    {
        foreach(var root in scene.GetRootGameObjects())
        {
            var menu=root.GetComponentInChildren<SaveMenu>();
            if(menu!=null)
            {
                pendingNewGame=false;firstNotice=false;acquisitionUntil=0;observedSession=null;
                if(menu.GetComponent<HUDMenuIntent>()==null)menu.gameObject.AddComponent<HUDMenuIntent>().menu=menu;
            }
        }
        label=FindFirstObjectByType<RoomLabel>();
    }
    public void ObserveMenuDeparture(bool newGame){pendingNewGame=newGame;}
    void Update()
    {
        var session=RoomSession.Instance;
        if(session?.Player==null){input=null;return;}
        if(observedSession!=session)
        {
            observedSession=session;input=session.Player;health=input.GetComponent<PlayerHealth>();
            dash=input.GetComponent<PlayerDash>();motor=input.GetComponent<PlayerMotor>();
            previousDash=DashOwned;previousJump=DoubleJumpOwned;firstNotice=false;acquisitionUntil=0;
        }
        if(pendingNewGame&&!session.Transitioning&&session.CurrentRoomId=="A01")
        {
            pendingNewGame=false;firstNotice=true;firstUntil=Time.unscaledTimeAsDouble+tuning.noticeSeconds;
        }
        if(firstNotice&&(Time.unscaledTimeAsDouble>=firstUntil||input.Move.sqrMagnitude>.001f||session.CurrentRoomId!="A01"))firstNotice=false;
        if(DashOwned&&!previousDash){acquiredDash=true;acquisitionUntil=Time.unscaledTimeAsDouble+tuning.noticeSeconds;}
        if(DoubleJumpOwned&&!previousJump){acquiredDash=false;acquisitionUntil=Time.unscaledTimeAsDouble+tuning.noticeSeconds;}
        previousDash=DashOwned;previousJump=DoubleJumpOwned;
        if(DeathOverlayVisible&&!wasDead)deathStarted=Time.unscaledTimeAsDouble;
        wasDead=DeathOverlayVisible;
        if(label==null||label.gameObject.scene.name!=session.CurrentRoomId)label=FindFirstObjectByType<RoomLabel>();
    }
    void Styles()
    {
        if(text!=null&&styleSize==tuning.fontSize)return;
        styleSize=tuning.fontSize;text=new GUIStyle(GUI.skin.label){fontSize=styleSize,alignment=TextAnchor.MiddleLeft};text.normal.textColor=Color.white;
        centered=new GUIStyle(text){alignment=TextAnchor.MiddleCenter};
    }
    static void Fill(Rect rect,Color color){var previous=GUI.color;GUI.color=color;GUI.DrawTexture(rect,Texture2D.whiteTexture);GUI.color=previous;}
    void Ability(Rect rect,bool owned,bool jump,string key)
    {
        float size=tuning.abilityIconSize;
        var icon=new Rect(rect.x,rect.y,size,size);Color color=owned?Color.white:new Color(.55f,.55f,.55f);
        if(owned)Fill(icon,color);
        else{Fill(new Rect(icon.x,icon.y,size,2),color);Fill(new Rect(icon.x,icon.yMax-2,size,2),color);Fill(new Rect(icon.x,icon.y,2,size),color);Fill(new Rect(icon.xMax-2,icon.y,2,size),color);}
        Color mark=owned?new Color(.08f,.08f,.1f):color;
        // Dash: horizontal arrow. Double Jump: two upward chevrons.
        if(!jump){Fill(new Rect(icon.x+5,icon.center.y-1,size-10,2),mark);Fill(new Rect(icon.xMax-8,icon.center.y-4,3,8),mark);}
        else{Fill(new Rect(icon.center.x-1,icon.y+5,2,size-10),mark);Fill(new Rect(icon.center.x-5,icon.y+5,10,2),mark);Fill(new Rect(icon.center.x-5,icon.y+11,10,2),mark);}
        GUI.Label(new Rect(rect.x+size+8,rect.y,130,size),(jump?"더블 점프":"대시")+(owned?"  "+key:"  —"),text);
    }
    void OnGUI()
    {
        if(tuning==null||input==null||observedSession==null||string.IsNullOrEmpty(observedSession.CurrentRoomId))return;
        var oldMatrix=GUI.matrix;var oldColor=GUI.color;var oldDepth=GUI.depth;GUI.depth=-20;
        float scale=Mathf.Min(Screen.width/1280f,Screen.height/720f);float width=Screen.width/scale,height=Screen.height/scale;
        GUI.matrix=Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(scale,scale,1));Styles();
        if(DeathOverlayVisible)
        {
            float fade=tuning.deathDisplaySeconds<=0?1:Mathf.Clamp01((float)(Time.unscaledTimeAsDouble-deathStarted)/tuning.deathDisplaySeconds);
            Fill(new Rect(0,0,width,height),new Color(0,0,0,tuning.deathOpacity*Mathf.Lerp(.5f,1,fade)));
            GUI.Label(new Rect(0,height*.5f-24,width,48),"체크포인트로 복귀 중…",centered);
        }
        if(!input.Paused)
        {
            float x=tuning.hudPosition.x,y=tuning.hudPosition.y;Fill(new Rect(x,y,tuning.hudSize.x,tuning.hudSize.y),new Color(.04f,.05f,.07f,.84f));
            GUI.Label(new Rect(x+12,y+4,tuning.hudSize.x-24,24),observedSession.CurrentRoomId,text);
            GUI.Label(new Rect(x+12,y+31,30,24),"HP",text);
            for(int i=0;i<health.tuning.playerMaxHP;i++)Fill(new Rect(x+48+i*(tuning.hpPipSize+tuning.hpPipGap),y+34,tuning.hpPipSize,tuning.hpPipSize),i<VisibleHP?Color.white:new Color(.32f,.32f,.35f));
            Ability(new Rect(x+12,y+65,160,24),DashOwned,false,input.UsingGamepad?"B":"C");
            Ability(new Rect(x+176,y+65,160,24),DoubleJumpOwned,true,input.UsingGamepad?"A":"Z");
            GUI.Label(new Rect(x+12,y+93,tuning.hudSize.x-24,24),input.UsingGamepad?"공격 X  /  일시정지 Menu":"공격 X  /  일시정지 Esc",text);
            if(label!=null&&!string.IsNullOrEmpty(label.notice))GUI.Label(new Rect(x,y+tuning.hudSize.y+4,width-x*2,28),label.notice,text);
            if(!string.IsNullOrEmpty(ProgressSave.Message))GUI.Label(new Rect(x,y+tuning.hudSize.y+34,width-x*2,36),ProgressSave.Message,text);
            if(FirstNoticeVisible)Notice(new Rect(140,height-88,width-280,tuning.noticeHeight),input.UsingGamepad?"스틱 이동 / A 점프 / X 공격 / Menu 일시정지\n스틱 위+X 위 공격 / 공중 스틱 아래+X 아래 공격(튕기기)":"←→ 이동 / Z 점프 / X 공격 / Esc 일시정지\n↑+X 위 공격 / 공중 ↓+X 아래 공격(튕기기)");
            if(AcquisitionVisible)Notice(new Rect(240,170,width-480,tuning.noticeHeight),AcquisitionText);
        }
        GUI.matrix=oldMatrix;GUI.color=oldColor;GUI.depth=oldDepth;
    }
    void Notice(Rect rect,string message){Fill(rect,new Color(.04f,.05f,.07f,.9f));GUI.Label(rect,message,centered);}
    void OnDestroy(){SceneManager.sceneLoaded-=SceneLoaded;if(Instance==this)Instance=null;}
}

// Reads the final title selection as the menu is unloaded, including mouse confirmation.
public sealed class HUDMenuIntent : MonoBehaviour
{
    public SaveMenu menu;
    void OnDestroy()
    {
        if(GameHUD.Instance!=null&&!ReferenceEquals(menu,null))GameHUD.Instance.ObserveMenuDeparture(menu.Selection==1&&ProgressSave.Enabled&&ProgressSave.Pending!=null);
    }
}
