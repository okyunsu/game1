#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
public sealed class B05Optional:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Optional>("-verify-b05-optional");
 RouteTickDriver driver;bool failed;protected override void Cleanup(){if(driver!=null)driver.active=false;}
 protected override IEnumerator Tests(){yield return NewGame();string id=N5Args.Value("-b05-room","A01");if(id!="A01"){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity",id=="C06"?"FromLeft":"Entry"),"independent optional fixture "+id);yield return Ready();}yield return new WaitForSeconds(.3f);
  if(id.StartsWith("B")||id.StartsWith("C"))RoomSession.Instance.GrantDash(); if(id.StartsWith("C")||id=="B05")RoomSession.Instance.GrantDoubleJump(); float x=GameObject.Find("V2 optional precision 0").transform.position.x; var pointList=new System.Collections.Generic.List<Vector2>();for(int k=0;GameObject.Find("V2 optional precision "+k)!=null;k++){var p=GameObject.Find("V2 optional precision "+k);pointList.Add(new Vector2(p.transform.position.x,p.GetComponent<Collider2D>().bounds.max.y+.8f));}Vector2[] points=pointList.ToArray(); int stage=0,jumpUntil=0;bool jumping=false;var segment=new RouteSegment{id="V2-"+id+"-optional",startScene=id+"_V2",startPosition=Body.position,goal="V2-"+id,dash=Player.GetComponent<PlayerDash>().hasDash,doubleJump=Motor.hasDoubleJump,startHP=Health.HP,limitSeconds=45};int previous=0;
  var pogo=id=="B06"?new B05PogoRoute(Player):null;driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;driver.tick=0;
  string replay=N5Args.Value("-b05-inputs","");if(replay!="")segment=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(replay)).segments[0];
  driver.observe=()=>{if(RoomSession.Instance.Respawning){failed=true;driver.active=false;}else if(DiscoveryReward.Found("V2-"+id)&&Motor.Grounded)driver.active=false;};
  driver.produce=()=>{if(replay!=""){if(driver.tick>=segment.inputs.Count){failed=true;driver.active=false;return new N5InputTick();}return segment.inputs[driver.tick];}
   if(pogo!=null&&!pogo.Done)return Record(pogo.Generate(driver.tick),segment,ref previous);
   var target=points[Mathf.Min(stage,points.Length-1)];if(Motor.Grounded&&Mathf.Abs(Body.position.x-target.x)<.5f&&Mathf.Abs(Body.position.y-target.y)<.3f&&stage<points.Length){stage++;jumping=false;if(stage<points.Length)target=points[stage];}
   if(stage>=points.Length)return Record(new N5InputTick{tick=driver.tick},segment,ref previous);
   if(jumping&&Motor.Grounded&&driver.tick>jumpUntil+12)jumping=false;
   if(stage==0&&!jumping){float launch=x<7.5f?x+3.5f:x-3.5f;if(Mathf.Abs(Body.position.x-launch)>.1f)return Record(new N5InputTick{tick=driver.tick,axis=Mathf.Sign(launch-Body.position.x),held=Body.position.x<x-6&&driver.tick%50<26?2:0},segment,ref previous);if(!Motor.Grounded)return Record(new N5InputTick{tick=driver.tick},segment,ref previous);}
   if(!jumping&&Motor.Grounded){jumping=true;jumpUntil=driver.tick+26;}
   var input=new N5InputTick{tick=driver.tick,axis=Mathf.Abs(target.x-Body.position.x)<.06f?0:Mathf.Sign(target.x-Body.position.x),held=(stage>0&&points[stage].y-points[stage-1].y>2.4f?(driver.tick<jumpUntil-13||driver.tick>=jumpUntil-10&&driver.tick<jumpUntil+12):driver.tick<jumpUntil)?2:0};input.pressed=input.held&~previous;input.released=previous&~input.held;previous=input.held;segment.inputs.Add(input);return input;};driver.active=true;double end=Time.realtimeSinceStartupAsDouble+45;while(driver.active&&Time.realtimeSinceStartupAsDouble<end){if(RoomSession.Instance.Respawning||RoomSession.Instance.CurrentRoomId!=id){failed=true;driver.active=false;lines.Add("FAIL frame-observed death or wrong room "+id);}yield return null;}driver.active=false;yield return Keys();segment.passed=!failed&&DiscoveryReward.Found("V2-"+id)&&Motor.Grounded;segment.result=segment.passed?"PASS":"FAIL";var library=new RouteLibrary();library.segments.Add(segment);if(replay=="")File.WriteAllText(System.IO.Path.ChangeExtension(Evidence,"inputs.json"),JsonUtility.ToJson(library,true));lines.Add("MEASURE optional "+id+" stage="+stage+" pos="+Body.position+" ticks="+driver.tick+" found="+DiscoveryReward.Found("V2-"+id)+" room="+RoomSession.Instance.CurrentRoomId+" pogos="+Player.GetComponent<PlayerAttack>().PogosPerformed);if(id=="B06")Check(!Motor.hasDoubleJump&&Player.GetComponent<PlayerAttack>().PogosPerformed>=2,"B06 actual pogo chain; no Double Jump");Check(segment.passed,id+" optional discovery contact + shelf landing "+(replay!=""?"prescribed replay":"recording"));
  }
 static N5InputTick Record(N5InputTick input,RouteSegment segment,ref int previous){input.pressed=input.held&~previous;input.released=previous&~input.held;previous=input.held;segment.inputs.Add(input);return input;}
}









#endif
