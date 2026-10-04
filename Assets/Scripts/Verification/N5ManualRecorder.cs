using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
// User recording observer: never injects input or changes gameplay state.
[DefaultExecutionOrder(-31999)]public sealed class N5ManualRecorder:MonoBehaviour
{
 N5Recording data;InputActionAsset actions;int tick,previous;string room,path;bool saved;
 void Awake(){path=N5Args.Value("-record","Logs/player-record.json");if(File.Exists(path)){Debug.LogError("Recording exists; select a new output path");enabled=false;}DontDestroyOnLoad(gameObject);}
 void FixedUpdate(){var s=RoomSession.Instance;if(s?.Player==null||string.IsNullOrEmpty(s.CurrentRoomId))return;var p=s.Player;if(data==null){data=new N5Recording{scene=s.CurrentRoomId,checkpoint=s.CheckpointId,spawn=p.GetComponent<Rigidbody2D>().position,dash=p.GetComponent<PlayerDash>().hasDash,doubleJump=p.GetComponent<PlayerMotor>().hasDoubleJump};actions=(InputActionAsset)typeof(PlayerInputReader).GetField("runtime",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(p);}
  if(room!=s.CurrentRoomId){room=s.CurrentRoomId;data.rooms.Add(new N5RoomTick{room=room,tick=tick});}int held=0;if(p.JumpHeld)held|=2;if(actions.FindAction("Gameplay/Dash").IsPressed())held|=4;if(actions.FindAction("Gameplay/Attack").IsPressed())held|=8;data.ticks.Add(new N5InputTick{tick=tick++,axis=p.Move.x,held=held,pressed=held&~previous,released=previous&~held});previous=held;data.finalPosition=p.GetComponent<Rigidbody2D>().position;
 }
 void Save(){if(saved||data==null)return;saved=true;Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));if(!File.Exists(path))File.WriteAllText(path,JsonUtility.ToJson(data,true));}
 void Update(){if(data!=null&&FindFirstObjectByType<EndingMenu>()!=null)Save();}
 void OnApplicationQuit()=>Save();
}
