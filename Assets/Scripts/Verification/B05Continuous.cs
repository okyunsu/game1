using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
public sealed class B05Continuous:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Continuous>("-verify-b05-continuous");
 RouteTickDriver driver;RouteLibrary library=new();RouteSegment segment;int index,gateStage,jumpUntil,previous;bool jumping,failed;
 readonly string[] goals={"A02","A03","A04","A05","Dash","A04","A03","B01"};
 protected override void Cleanup(){if(driver!=null)driver.active=false;}
 protected override IEnumerator Tests(){yield return NewGame();driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;for(index=0;index<goals.Length;index++){
  string goal=goals[index];yield return Keys();yield return Until(()=>Motor.Grounded,"segment grounded",8);segment=new RouteSegment{id="V2-R"+(index+1).ToString("D2"),startScene=RoomSession.Instance.CurrentRoomId+"_V2",goal=goal,startPosition=Body.position,dash=Player.GetComponent<PlayerDash>().hasDash,startHP=Health.HP,limitSeconds=45};driver.tick=0;previous=0;
  driver.observe=()=>{if(RoomSession.Instance.Respawning){failed=true;driver.active=false;}else if(goal=="Dash"?Player.GetComponent<PlayerDash>().hasDash:RoomSession.Instance.CurrentRoomId==goal)driver.active=false;};
  driver.produce=()=>{var v=Generate(goal);v.tick=driver.tick;v.pressed=v.held&~previous;v.released=previous&~v.held;previous=v.held;segment.inputs.Add(v);return v;};driver.active=true;double end=Time.realtimeSinceStartupAsDouble+45;while(driver.active&&Time.realtimeSinceStartupAsDouble<end)yield return null;driver.active=false;yield return Keys();bool reached=goal=="Dash"?Player.GetComponent<PlayerDash>().hasDash:RoomSession.Instance.CurrentRoomId==goal;segment.passed=reached&&!failed;segment.result=segment.passed?"PASS":"FAIL";library.segments.Add(segment);File.WriteAllText(Path.ChangeExtension(Evidence,"inputs.json"),JsonUtility.ToJson(library,true));lines.Add("MEASURE "+segment.id+" ticks="+driver.tick+" room="+RoomSession.Instance.CurrentRoomId+" pos="+Body.position+" gateStage="+gateStage);Check(segment.passed,segment.id+" continuous "+goal);yield return Ready();
 }Check(RoomSession.Instance.CurrentRoomId=="B01","A01 V2 to A05 Dash to A03 V2 to original B01 without placement");}
 N5InputTick Generate(string goal){float x=Body.position.x;string id=RoomSession.Instance.CurrentRoomId;int t=driver.tick;
 if(goal=="B01"){
  Vector2[] targets={new(40,1.81f),new(45.4f,4.01f),new(50.6f,5.81f),new(60,5.81f)};var p=targets[gateStage];
  if(gateStage==0&&Mathf.Abs(x-p.x)<.2f&&Motor.Grounded){gateStage++;p=targets[gateStage];}
  else if(gateStage>0&&gateStage<3&&Mathf.Abs(x-p.x)<.5f&&Mathf.Abs(Body.position.y-p.y)<.3f&&Motor.Grounded){gateStage++;jumping=false;p=targets[gateStage];}
  if(gateStage>0&&!jumping&&Motor.Grounded){jumping=true;jumpUntil=t+28;}int held=gateStage==3?0:t<jumpUntil?2:0;if(gateStage==3&&x>54.35f&&x<58)held=4;return new N5InputTick{axis=Mathf.Abs(x-p.x)<.05f?0:Mathf.Sign(p.x-x),held=held};
 }
 float target=goal=="Dash"?FindFirstObjectByType<DashPickup>().transform.position.x:FindObjectsByType<RoomExit>(FindObjectsSortMode.None).First(e=>e.destinationRoomId==goal).transform.position.x;
 bool jump=t%50<26;if(id=="A03"&&x>27||id=="A04"&&x<10)jump=false;return new N5InputTick{axis=Mathf.Abs(x-target)<.05f?0:Mathf.Sign(target-x),held=(jump?2:0)|(id=="A04"&&t%22==0?8:0)};
 }
}


