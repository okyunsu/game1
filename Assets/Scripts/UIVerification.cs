using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

// Opt-in verification only. Inert in ordinary game sessions.
public sealed class UIVerification : MonoBehaviour
{
    public string output="Validation/U1-ui.txt";
    public Action<bool> finished;
    public bool screenshots;
    readonly List<string> lines=new(),errors=new();Keyboard kb;Gamepad pad;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if(Application.isEditor)return;
        var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"-ui-verify");if(index<0||index+1>=args.Length)return;
        var go=new GameObject("UI prescribed verification");DontDestroyOnLoad(go);var runner=go.AddComponent<UIVerification>();runner.output=Path.GetFullPath(args[index+1]);runner.screenshots=Array.IndexOf(args,"-ui-shots")>=0;
    }
    void Check(bool ok,string message){if(!ok)throw new Exception(message);lines.Add("PASS: "+message);}
    void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;QualitySettings.vSyncCount=0;Application.targetFrameRate=60;
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        #if UNITY_EDITOR
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        #endif
        kb=InputSystem.AddDevice<Keyboard>();pad=InputSystem.AddDevice<Gamepad>();
        var stack=new Stack<IEnumerator>();stack.Push(Tests());bool fail=false;
        while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}
        PlaytestLogger.Instance?.Finish();Application.logMessageReceived-=Log;InputSystem.RemoveDevice(kb);InputSystem.RemoveDevice(pad);fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" U1 UI "+(Application.isEditor?"Editor":"actual exe"));lines.Add("Error/Exception/Assert="+errors.Count);
        if(File.Exists(output))throw new IOException("Preserve evidence "+output);File.WriteAllLines(output,lines);if(finished!=null)finished(!fail);else Application.Quit(fail?1:0);
    }
    IEnumerator Until(Func<bool> condition,float seconds=8)
    {
        double deadline=Time.realtimeSinceStartupAsDouble+seconds;
        while(!condition()){if(Time.realtimeSinceStartupAsDouble>deadline)throw new Exception("UI verification timeout");yield return null;}
    }
    IEnumerator Keys(params Key[] keys){InputSystem.QueueStateEvent(kb,new KeyboardState(keys));yield return null;yield return new WaitForSecondsRealtime(.03f);}
    IEnumerator Ready(){yield return Until(()=>RoomSession.Instance?.Player!=null&&!RoomSession.Instance.Transitioning&&!RoomSession.Instance.Respawning);yield return null;yield return null;}
    void Place(Vector2 p){var input=RoomSession.Instance.Player;input.ClearTransientInput();input.transform.position=p;var body=input.GetComponent<Rigidbody2D>();body.position=p;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    IEnumerator MenuNewGame()
    {
        var menu=FindFirstObjectByType<SaveMenu>();Check(menu!=null,"title present");
        if(menu.Selection==0){yield return Keys(Key.DownArrow);yield return Keys();}
        yield return Keys(Key.Z);yield return Keys();menu=FindFirstObjectByType<SaveMenu>();
        if(menu!=null&&menu.Confirming){yield return Keys(Key.Z);yield return Keys();}
        yield return Ready();
    }
    IEnumerator Shot(string name,int width,int height){if(screenshots)lines.Add("NOT RUN screenshot "+name+": capture path stopped after tool correction failed; see U1-tool-correction.txt.");yield break;}
    IEnumerator Enter(string id,string spawn="Entry"){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/"+id+".unity",spawn),"direct verification placement "+id);yield return Ready();}
    IEnumerator Tests()
    {
        yield return new WaitForSecondsRealtime(.3f);Check(GameHUD.Instance!=null&&GameHUD.Instance.tuning!=null,"runtime HUD and UITuning asset");
        yield return MenuNewGame();var hud=GameHUD.Instance;var session=RoomSession.Instance;var hp=session.Player.GetComponent<PlayerHealth>();
        Check(session.CurrentRoomId=="A01"&&hud.FirstNoticeVisible,"new game A01 first controls visible");Check(hud.VisibleHP==5&&!hud.DashOwned&&!hud.DoubleJumpOwned,"HP5 and both gray ownership states");
        yield return Shot("A01-start",1280,720);yield return Shot("A01-start",1920,1080);
        yield return new WaitForSecondsRealtime(hud.tuning.noticeSeconds+.05f);Check(!hud.FirstNoticeVisible,"first notice expires at configured duration");
        Place(new Vector2(8,1.81f));yield return new WaitForSecondsRealtime(.1f);Check(hp.TakeDamage(1,new Vector2(5,1)),"actual hit accepted");yield return null;Check(hud.VisibleHP==4&&hud.VisibleHP==hp.HP,"HUD four white pips after hit");
        yield return Shot("after-hit",1280,720);yield return Shot("after-hit",1920,1080);
        double began=Time.realtimeSinceStartupAsDouble;Check(session.Die(),"death accepted");yield return null;Check(hud.DeathOverlayVisible&&session.Player.InputLocked,"Respawning dark overlay active, existing input lock");
        yield return Until(()=>!session.Respawning);yield return null;Check(!hud.DeathOverlayVisible&&hp.HP==5,"respawn removes overlay and restores HP");Check(Time.realtimeSinceStartupAsDouble-began<1,"existing .6s death flow preserved");
        yield return Enter("A05");var pickup=FindFirstObjectByType<DashPickup>();Check(pickup!=null,"authored Dash pickup");Place(pickup.transform.position);yield return new WaitForFixedUpdate();yield return null;yield return null;
        Check(hud.DashOwned&&hud.AcquisitionVisible&&hud.AcquisitionText.Contains("C"),"Dash white icon and keyboard C notice");yield return new WaitForSecondsRealtime(hud.tuning.noticeSeconds+.05f);Check(!hud.AcquisitionVisible,"Dash notice expires at UITuning seconds");
        yield return Enter("B04");var jump=FindFirstObjectByType<DoubleJumpPickup>();Check(jump!=null,"authored Double Jump pickup");Place(jump.transform.position);yield return new WaitForFixedUpdate();yield return null;yield return null;
        Check(hud.DoubleJumpOwned&&hud.DashOwned&&hud.AcquisitionVisible&&hud.AcquisitionText.Contains("Z"),"both white icons; Double Jump keyboard Z notice");
        yield return Shot("both-abilities",1280,720);yield return Shot("both-abilities",1920,1080);
        InputSystem.QueueStateEvent(pad,new GamepadState().WithButton(GamepadButton.South));yield return null;Check(session.Player.UsingGamepad&&hud.AcquisitionText.Contains("A"),"gamepad ownership labels and Double Jump A notice");InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;
        yield return new WaitForSecondsRealtime(hud.tuning.noticeSeconds+.05f);Check(!hud.AcquisitionVisible,"Double Jump notice expires");
        session.Player.SetPaused(true);yield return null;Check(!hud.FirstNoticeVisible&&!hud.AcquisitionVisible,"Pause panel unobstructed by notices");session.Player.ReturnTitle();yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null);yield return new WaitForSecondsRealtime(.2f);
        var menu=FindFirstObjectByType<SaveMenu>();Check(menu.CanContinue&&menu.Selection==0,"Continue valid after abilities saved");yield return Keys(Key.Z);yield return Keys();yield return Ready();yield return null;
        Check(!hud.FirstNoticeVisible&&hud.DashOwned&&hud.DoubleJumpOwned&&!hud.AcquisitionVisible,"Continue has no first/acquisition notice; saved ownership read only");
        RoomSession.Instance.Player.SetPaused(true);RoomSession.Instance.Player.ReturnTitle();yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null);yield return new WaitForSecondsRealtime(.2f);yield return MenuNewGame();Check(hud.FirstNoticeVisible,"second new game gets its own first notice");yield return Keys(Key.RightArrow);yield return Keys();Check(!hud.FirstNoticeVisible,"first movement dismisses notice");
        RoomSession.Instance.Player.SetPaused(true);RoomSession.Instance.Player.ReturnTitle();yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null);yield return new WaitForSecondsRealtime(.2f);yield return Keys(Key.Z);yield return Keys();yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="A01"&&!hud.FirstNoticeVisible,"Continue at CP-A01 without abilities still suppresses first notice");
        // Actual executable integration: direct placement uses real ability pickups and boss defeat flow.
        yield return Enter("A05");Place(FindFirstObjectByType<DashPickup>().transform.position);yield return new WaitForFixedUpdate();yield return null;
        yield return Enter("B04");Place(FindFirstObjectByType<DoubleJumpPickup>().transform.position);yield return new WaitForFixedUpdate();yield return null;
        Check(RoomSession.Instance.Player.GetComponent<PlayerDash>().hasDash&&RoomSession.Instance.Player.GetComponent<PlayerMotor>().hasDoubleJump,"both abilities acquired before final boss flow");
        yield return Enter("C04","FromLeft");yield return Enter("C06","FromLeft");Place(new Vector2(8,1.81f));var boss=FindFirstObjectByType<BossEncounter>();yield return Until(()=>boss.Fighting);yield return null;boss.ReceiveHit(20,RoomSession.Instance.Player.transform.position);
        yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null);Check(true,"direct boss defeat reaches Ending");yield return Keys(Key.Z);yield return Keys();yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null);Check(true,"Ending returns title");
        Check(PlaytestLogger.Instance!=null,"existing CSV logger retained");lines.Add("Presentation acceptance pending user; synthetic input and direct placements, not natural completion.");
    }
}
