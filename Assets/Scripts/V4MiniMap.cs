#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class V4MiniMap:MonoBehaviour
{
 public bool Visible{
 get{
 #if DEVELOPMENT_BUILD || UNITY_EDITOR
 return V4WorldMap.Instance!=null&&V4WorldMap.Instance.Mode!=V4WorldMap.Display.Off;
 #else
 return false;
 #endif
 }}
 public string GridText{get{string text="M | 방문한 방 / 현재 위치\n";for(int i=0;i<V4SceneMap.Rooms.Length;i++){string id=V4SceneMap.Rooms[i];text+=(id==RoomSession.Instance?.CurrentRoomId?">":ProgressSave.HasVisited(id)?"●":"·")+id+"  ";if(i%6==5)text+="\n";}return text;}}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){
 #if DEVELOPMENT_BUILD || UNITY_EDITOR
 // Compatibility type only; V4WorldMap owns M and drawing.
 #endif
 }
}

#endif
