using UnityEngine;
public sealed class B26RoomNotice:MonoBehaviour
{
 public string roomName;
 [Min(0),Tooltip("Room entry name display duration, game seconds.")]public float seconds=1.2f;
 public double Began{get;private set;}public bool Visible=>Time.timeAsDouble-Began<seconds;
 GUIStyle style;
 void Start(){Began=Time.timeAsDouble;Debug.Log($"MEASURE B26 roomName={roomName} seconds={seconds:F3}");}
 void OnGUI(){if(!Visible||RoomSession.Instance?.Player==null||RoomSession.Instance.Player.Paused)return;style??=new GUIStyle(GUI.skin.label){fontSize=28,alignment=TextAnchor.UpperCenter};GUI.Label(new Rect(Screen.width*.2f,24,Screen.width*.6f,42),roomName,style);}
}
