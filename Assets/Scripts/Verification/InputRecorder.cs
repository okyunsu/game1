#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
[Serializable]public sealed class N5Recording{public string scene,checkpoint;public bool dash,doubleJump;public Vector2 spawn,finalPosition;public List<N5InputTick> ticks=new();public List<N5RoomTick> rooms=new();}
[Serializable]public sealed class N5InputTick{public int tick;public float axis;public int held,pressed,released;}
[Serializable]public sealed class N5RoomTick{public int tick;public string room;}
// Fixed-tick virtual input producer runs before the gameplay physics consumers.
[DefaultExecutionOrder(-32000)]public sealed class N5TickInput:MonoBehaviour
{
 public Keyboard device;public N5Recording data;public bool replay,active,done;public Vector2 finalSnapshot;public int tick;int previous;bool finishNext;string room;public readonly List<N5RoomTick> observed=new();
 void FixedUpdate(){if(!active||done)return;var s=RoomSession.Instance;if(s?.Player==null)return;
  if(room!=s.CurrentRoomId){room=s.CurrentRoomId;observed.Add(new N5RoomTick{tick=tick,room=room});}
  if(finishNext||replay&&tick>=data.ticks.Count){finalSnapshot=s.Player.GetComponent<Rigidbody2D>().position;done=true;InputSystem.QueueStateEvent(device,new KeyboardState(),Time.fixedUnscaledTimeAsDouble);InputSystem.Update();return;}N5InputTick entry;
  if(replay){entry=data.ticks[tick];}
  else{int held=1; // authored A01 prescribed traversal: movement + repeated full jumps over steps
   if(tick%45<27)held|=2;entry=new N5InputTick{tick=tick,axis=1,held=held,pressed=held&~previous,released=previous&~held};data.ticks.Add(entry);previous=held;
  }
  var keys=new List<Key>();if(entry.axis>.1f)keys.Add(Key.RightArrow);if(entry.axis<-.1f)keys.Add(Key.LeftArrow);if((entry.held&2)!=0)keys.Add(Key.Z);if((entry.held&4)!=0)keys.Add(Key.C);if((entry.held&8)!=0)keys.Add(Key.X);
  InputSystem.QueueStateEvent(device,new KeyboardState(keys.ToArray()),Time.fixedUnscaledTimeAsDouble);InputSystem.Update();tick++;
  if(!replay&&room=="A02"){finishNext=true;data.rooms=new List<N5RoomTick>(observed);}
 }
}
public class InputRecorder:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(N5Args.Has("-record")&&!N5Args.Has("-verify-record-route")){N5Args.Start<N5ManualRecorder>("-record");return;}if(N5Args.Has("-replay"))N5Args.Start<InputReplayer>("-replay");else if(N5Args.Has("-record"))N5Args.Start<InputRecorder>("-record");}
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N5-TC-record-replay.txt");
 protected override IEnumerator Tests(){bool replay=N5Args.Has("-replay");string path=N5Args.Value(replay?"-replay":"-record","Logs/N5-record.json");N5Recording data=replay?JsonUtility.FromJson<N5Recording>(File.ReadAllText(path)):new N5Recording();if(replay&&string.IsNullOrEmpty(data.checkpoint))yield return NewGame();else yield return ContinueGame();yield return Until(()=>Motor.Grounded,"grounded start");yield return Keys();yield return new WaitForFixedUpdate();
  if(!replay){data.scene=RoomSession.Instance.CurrentRoomId;data.checkpoint=RoomSession.Instance.CheckpointId;data.spawn=Body.position;data.dash=Player.GetComponent<PlayerDash>().hasDash;data.doubleJump=Motor.hasDoubleJump;Check(data.scene=="A01","record A01 start");}
  else Check(data.scene==RoomSession.Instance.CurrentRoomId&&Vector2.Distance(data.spawn,Body.position)<.01f&&data.dash==Player.GetComponent<PlayerDash>().hasDash&&data.doubleJump==Motor.hasDoubleJump,"same scene/spawn/ability metadata");
  var driver=gameObject.AddComponent<N5TickInput>();driver.device=keyboard;driver.data=data;driver.replay=replay;driver.active=true;yield return Until(()=>driver.done,"FixedUpdate route replay",Mathf.Max(35,data.ticks.Count*Time.fixedDeltaTime+30));yield return new WaitForFixedUpdate();driver.active=false;
  lines.Add("ticks="+driver.tick+" fps="+Application.targetFrameRate+" final="+driver.finalSnapshot);foreach(var r in driver.observed)lines.Add("room="+r.room+" tick="+r.tick);
  if(!replay){data.finalPosition=driver.finalSnapshot;Check(data.rooms.Exists(r=>r.room=="A02"),"record reached A02 by physical traversal");if(File.Exists(path))throw new IOException("Preserve recording");File.WriteAllText(path,JsonUtility.ToJson(data,true));}
  else{Check(driver.observed.Count==data.rooms.Count,"same room entry count");for(int i=0;i<data.rooms.Count;i++)Check(data.rooms[i].room==driver.observed[i].room&&Math.Abs(data.rooms[i].tick-driver.observed[i].tick)<=1,"room tick tolerance +/-1 "+i);Check(Vector2.Distance(data.finalPosition,driver.finalSnapshot)<=.05f,"final position tolerance .05u");}
 }
}
#endif
