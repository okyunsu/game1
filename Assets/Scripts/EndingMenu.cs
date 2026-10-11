using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public sealed class EndingMenu:MonoBehaviour
{
 [Tooltip("UI actions for keyboard and gamepad submit.")]public InputActionAsset actions;
 InputActionAsset runtime;bool leaving;
 void Awake(){ProgressSave.MarkEndingReached();ProgressSave.Pending=null;if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);var state=FindFirstObjectByType<GameState>();if(state!=null)Destroy(state.gameObject);Time.timeScale=1;}
 void Start(){runtime=Instantiate(actions);runtime.FindAction("UI/Submit",true).performed+=ctx=>ReturnTitle();runtime.FindActionMap("UI",true).Enable();}
 public void ReturnTitle(){if(leaving)return;leaving=true;runtime.Disable();if(V4SceneMap.Enabled){ProgressSave.Continue();SceneManager.LoadScene(V4SceneMap.Path("A01"));}else SceneManager.LoadScene("Assets/Scenes/Menu.unity");}
 void OnDestroy(){if(runtime!=null){runtime.Disable();Destroy(runtime);}}
 void OnGUI(){float x=Screen.width*.5f-200,y=Screen.height*.4f;GUI.Label(new Rect(x,y,500,60),(V3SceneMap.Enabled||V4SceneMap.Enabled)?"정상에는 아무것도 없었다.\n도시는 여전히, 아래에 있다.":"정상의 빛이 다시 켜졌습니다.\n당신의 여정은 새로운 아침을 열었습니다.");if(GUI.Button(new Rect(x,y+80,400,45),V4SceneMap.Enabled?"A01에서 재개 (Z / Enter / A)":"타이틀로 (Z / Enter / A)"))ReturnTitle();GUI.Label(new Rect(20,Screen.height-50,700,40),ProgressSave.Message);}
}

