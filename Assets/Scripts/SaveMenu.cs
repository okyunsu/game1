using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public sealed class SaveMenu:MonoBehaviour
{
 [Tooltip("UI map only; Gameplay remains disabled in this menu.")] public InputActionAsset actions;
 public bool CanContinue { get; private set; }
 public bool Confirming { get; private set; }
 public int Selection { get; private set; }
 InputActionAsset runtime;bool leaving;
 void Awake(){if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);var state=FindFirstObjectByType<GameState>();if(state!=null)Destroy(state.gameObject);Time.timeScale=1;ProgressSave.Enabled=false;ProgressSave.Pending=null;}
 void Start(){CanContinue=ProgressSave.TryLoad(out _);Selection=CanContinue?0:1;runtime=Instantiate(actions);runtime.FindAction("UI/Navigate",true).performed+=ctx=>{if(!Confirming&&Mathf.Abs(ctx.ReadValue<Vector2>().y)>.5f)Selection=CanContinue?(ctx.ReadValue<Vector2>().y>0?0:1):1;};runtime.FindAction("UI/Submit",true).performed+=ctx=>Submit();runtime.FindAction("UI/Cancel",true).performed+=ctx=>Confirming=false;runtime.FindActionMap("UI",true).Enable();}
 public void Submit(){if(leaving)return;if(Selection==0){if(!ProgressSave.Continue())return;}else{if(File.Exists(ProgressSave.FilePath)&&!Confirming){Confirming=true;return;}if(!ProgressSave.NewGame())return;}leaving=true;runtime.Disable();SceneManager.LoadScene(ProgressSave.PendingScene);}
 void OnDestroy(){if(runtime!=null){runtime.Disable();Destroy(runtime);}}
 void OnGUI(){float x=Screen.width*.5f-150,y=Screen.height*.5f-80;GUI.Label(new Rect(x,y-40,400,30),"Afterglow — Sprint 2");if(Confirming){GUI.Label(new Rect(x,y,600,30),"새 게임: 기존 진행을 초기화할까요? Z / Enter / A 확인, X / Esc / B 취소");if(GUI.Button(new Rect(x,y+40,300,40),"확인"))Submit();return;}GUI.enabled=CanContinue;if(GUI.Button(new Rect(x,y,300,40),(Selection==0?"> ":"")+"이어하기")){Selection=0;Submit();}GUI.enabled=true;if(GUI.Button(new Rect(x,y+50,300,40),(Selection==1?"> ":"")+"새 게임")){Selection=1;Submit();}GUI.Label(new Rect(x,y+100,700,60),ProgressSave.Message);}
}
