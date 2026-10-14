#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
public sealed class B05Replay:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Replay>("-verify-b05-replay");
 RouteTickDriver driver;bool failed,done;protected override void Cleanup(){if(driver!=null)driver.active=false;}
 protected override IEnumerator Tests(){yield return NewGame();var library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(N5Args.Value("-b05-inputs","")));driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;for(int n=0;n<library.segments.Count;n++){
  var segment=library.segments[n];string id=segment.startScene.Replace("_V2","");if(n==0&&id!="A01"){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity",id=="C06"?"FromLeft":"Entry"),"independent initial Scene setup");yield return Ready();if(segment.dash)RoomSession.Instance.GrantDash();if(segment.doubleJump)RoomSession.Instance.GrantDoubleJump();}
  Check(RoomSession.Instance.CurrentRoomId==id,"continuous recorded start "+segment.id);yield return Keys();yield return Until(()=>Motor.Grounded,"replay settle",8);lines.Add("MEASURE start deviation "+segment.id+"="+Vector2.Distance(Body.position,segment.startPosition).ToString("F3")+"u; no placement");Check(Player.GetComponent<PlayerDash>().hasDash==segment.dash,"recorded ability "+segment.id);done=failed=false;driver.tick=0;
  driver.observe=()=>{if(RoomSession.Instance.Respawning){failed=true;driver.active=false;}else if(segment.goal=="Dash"?Player.GetComponent<PlayerDash>().hasDash:segment.goal.StartsWith("V2-")?DiscoveryReward.Found(segment.goal)&&Motor.Grounded:RoomSession.Instance.CurrentRoomId==segment.goal){done=true;driver.active=false;}};
  driver.produce=()=>{if(driver.tick>=segment.inputs.Count){failed=true;driver.active=false;return new N5InputTick();}return segment.inputs[driver.tick];};driver.active=true;double end=Time.realtimeSinceStartupAsDouble+50;while(driver.active&&Time.realtimeSinceStartupAsDouble<end)yield return null;driver.active=false;yield return Keys();Check(done&&!failed,segment.id+" prescribed fixed tick replay room="+RoomSession.Instance.CurrentRoomId+" pos="+Body.position);yield return Ready();
 }}
}


#endif
