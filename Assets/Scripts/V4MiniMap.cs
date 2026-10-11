using UnityEngine;
using UnityEngine.InputSystem;
public sealed class V4MiniMap:MonoBehaviour
{
 bool visible;
 public bool Visible=>visible;
 public string GridText{get{string text="M | 방문한 방 / 현재 위치\n";for(int i=0;i<V4SceneMap.Rooms.Length;i++){string id=V4SceneMap.Rooms[i];text+=(id==RoomSession.Instance?.CurrentRoomId?">":ProgressSave.HasVisited(id)?"●":"·")+id+"  ";if(i%6==5)text+="\n";}return text;}}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){
 #if DEVELOPMENT_BUILD || UNITY_EDITOR
 if(V4SceneMap.Enabled){var go=new GameObject("V4 development map");DontDestroyOnLoad(go);go.AddComponent<V4MiniMap>();}
 #endif
 }
 void Update(){if(Keyboard.current?.mKey.wasPressedThisFrame==true)visible=!visible;}
 void OnGUI(){if(!visible||RoomSession.Instance?.Player==null)return;GUI.Box(new Rect(Screen.width-460,Screen.height-160,445,145),GridText);}
}
