using System;
using System.Collections;
using System.IO;
using UnityEngine;
public sealed class B05Optional:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Optional>("-verify-b05-optional");
 RouteTickDriver driver;bool failed;protected override void Cleanup(){if(driver!=null)driver.active=false;}
 protected override IEnumerator Tests(){yield return NewGame();string id=N5Args.Value("-b05-room","A01");if(id!="A01"){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity","Entry"),"independent optional fixture "+id);yield return Ready();}yield return new WaitForSeconds(.3f);
  float x=(id=="A03"||id=="A05"?72:id=="A04"?90:60)*.43f;float middle=new[]{4.5f,5.2f,4.9f,4.7f,5.1f,4.8f}[int.Parse(id.Substring(1))-1];Vector2[] points={new(x,4.0f),new(x+2.8f,middle+.8f),new(x+5.6f,7.6f)};int stage=0,jumpUntil=0;bool jumping=false;var segment=new RouteSegment{id="V2-"+id+"-optional",startScene=id+"_V2",startPosition=Body.position,goal="V2-"+id,startHP=Health.HP,limitSeconds=45};int previous=0;
  driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;driver.tick=0;
  string replay=N5Args.Value("-b05-inputs","");if(replay!="")segment=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(replay)).segments[0];
  driver.observe=()=>{if(RoomSession.Instance.Respawning){failed=true;driver.active=false;}else if(DiscoveryReward.Found("V2-"+id)&&Motor.Grounded)driver.active=false;};
  driver.produce=()=>{if(replay!=""){if(driver.tick>=segment.inputs.Count){failed=true;driver.active=false;return new N5InputTick();}return segment.inputs[driver.tick];}
   var target=points[Mathf.Min(stage,2)];if(Motor.Grounded&&Mathf.Abs(Body.position.x-target.x)<.5f&&Mathf.Abs(Body.position.y-target.y)<.3f){stage++;jumping=false;if(stage<3)target=points[stage];}
   if(jumping&&Motor.Grounded&&driver.tick>jumpUntil+12)jumping=false;
   if(stage==0&&!jumping){float launch=x-3.5f;if(Mathf.Abs(Body.position.x-launch)>.1f)return Record(new N5InputTick{tick=driver.tick,axis=Mathf.Sign(launch-Body.position.x),held=Body.position.x<x-6&&driver.tick%50<26?2:0},segment,ref previous);if(!Motor.Grounded)return Record(new N5InputTick{tick=driver.tick},segment,ref previous);}
   if(!jumping&&Motor.Grounded){jumping=true;jumpUntil=driver.tick+26;}
   var input=new N5InputTick{tick=driver.tick,axis=Mathf.Abs(target.x-Body.position.x)<.06f?0:Mathf.Sign(target.x-Body.position.x),held=driver.tick<jumpUntil?2:0};input.pressed=input.held&~previous;input.released=previous&~input.held;previous=input.held;segment.inputs.Add(input);return input;};driver.active=true;double end=Time.realtimeSinceStartupAsDouble+45;while(driver.active&&Time.realtimeSinceStartupAsDouble<end)yield return null;driver.active=false;yield return Keys();segment.passed=!failed&&DiscoveryReward.Found("V2-"+id)&&Motor.Grounded;segment.result=segment.passed?"PASS":"FAIL";var library=new RouteLibrary();library.segments.Add(segment);if(replay=="")File.WriteAllText(System.IO.Path.ChangeExtension(Evidence,"inputs.json"),JsonUtility.ToJson(library,true));lines.Add("MEASURE optional "+id+" stage="+stage+" pos="+Body.position+" ticks="+driver.tick);Check(segment.passed,id+" optional discovery contact + shelf landing "+(replay!=""?"prescribed replay":"recording"));
  }
 static N5InputTick Record(N5InputTick input,RouteSegment segment,ref int previous){input.pressed=input.held&~previous;input.released=previous&~input.held;previous=input.held;segment.inputs.Add(input);return input;}
}

