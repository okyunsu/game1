using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class B05World:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05World>("-verify-b05-world");
 RouteTickDriver driver;RouteLibrary library=new();bool failed,reached;double bossJumped=-1;int jumpUntil,nextAttack;
 protected override void Cleanup(){if(driver!=null)driver.active=false;}
 bool Reached(string goal)=>goal=="Menu"?FindFirstObjectByType<SaveMenu>()!=null:goal=="Ending"?FindFirstObjectByType<EndingMenu>()!=null:goal=="Dash"?Player.GetComponent<PlayerDash>().hasDash:goal=="DoubleJump"?Motor.hasDoubleJump:RoomSession.Instance?.CurrentRoomId==goal;
 protected override IEnumerator Tests(){yield return NewGame();driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;string replay=N5Args.Value("-b05-inputs","");var source=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(replay!=""?replay:N5Args.Value("-b05-world-prefix","Archive/validation-2026-10/B13-Routes/V2/V2-A-continuous-margin.json")));
  foreach(var recorded in source.segments)yield return Run(recorded,true);
  if(replay==""){var goals=N5Args.Has("-b05-world-boss-tail")?new[]{"Ending","Menu"}:new[]{"B02","B03","B04","DoubleJump","B03","B02","B01","B05","C01","C02","C03","C04","C06","Ending","Menu"};foreach(string goal in goals){if(goal=="Menu")yield return new WaitForSecondsRealtime(.3f);else {yield return Ready();yield return Keys();yield return Until(()=>Motor.Grounded,"world settle",8);}bool menu=goal=="Menu";var segment=new RouteSegment{id="V2-W"+(library.segments.Count+1).ToString("D2"),startScene=menu?"Ending":RoomSession.Instance.CurrentRoomId+"_V2",goal=goal,startPosition=menu?Vector2.zero:Body.position,dash=menu||Player.GetComponent<PlayerDash>().hasDash,doubleJump=menu||Motor.hasDoubleJump,startHP=menu?0:Health.HP,limitSeconds=60};yield return Run(segment,false);if(goal=="C01"&&N5Args.Value("-b05-world-stage","")=="B")break;}}
  Check(Reached(N5Args.Value("-b05-world-stage","")=="B"?"C01":"Menu"),"new game continuous world complete; no placement or ability grants");
 }
 IEnumerator Run(RouteSegment segment,bool replay){bool ending=segment.startScene=="Ending";if(ending)yield return new WaitForSecondsRealtime(.3f);else{yield return Ready();yield return Keys();yield return Until(()=>Motor.Grounded,"replay world settle",8);Check(RoomSession.Instance.CurrentRoomId==segment.startScene.Replace("_V2",""),"recorded world start "+segment.id);Check(Player.GetComponent<PlayerDash>().hasDash==segment.dash&&Motor.hasDoubleJump==segment.doubleJump,"world ability ownership "+segment.id);lines.Add("MEASURE "+segment.id+" start deviation="+Vector2.Distance(Body.position,segment.startPosition).ToString("F3"));}
  int prior=0;var navigator=new B05Navigator();failed=reached=false;driver.tick=0;driver.observe=()=>{if(Reached(segment.goal)){reached=true;driver.active=false;}else if(RoomSession.Instance?.Respawning==true){failed=true;driver.active=false;}};
  driver.produce=()=>{if(replay){if(driver.tick>=segment.inputs.Count){failed=true;driver.active=false;return new N5InputTick();}return segment.inputs[driver.tick];}var v=segment.goal=="Menu"?new N5InputTick{held=driver.tick%20==0?16:0}:segment.goal=="Ending"?BossInput():navigator.Generate(Player,segment.goal,driver.tick);v.tick=driver.tick;v.pressed=v.held&~prior;v.released=prior&~v.held;prior=v.held;segment.inputs.Add(v);return v;};driver.active=true;double end=Time.realtimeSinceStartupAsDouble+segment.limitSeconds+5;while(driver.active&&Time.realtimeSinceStartupAsDouble<end){if(RoomSession.Instance?.Respawning==true){failed=true;driver.active=false;lines.Add("FAIL frame-observed actual death "+segment.id);}yield return null;}driver.active=false;yield return Keys();segment.passed=reached&&!failed;segment.result=segment.passed?"PASS":"FAIL";if(!replay)for(int k=0;k<5;k++)segment.inputs.Add(new N5InputTick{tick=segment.inputs.Count});library.segments.Add(segment);if(N5Args.Value("-b05-inputs","")=="")File.WriteAllText(Path.ChangeExtension(Evidence,"inputs.json"),JsonUtility.ToJson(library,true));lines.Add("MEASURE "+segment.id+" goal="+segment.goal+" ticks="+driver.tick+" scene="+SceneManager.GetActiveScene().name+" pos="+(Player==null?Vector2.zero:Body.position));Check(segment.passed,segment.id+" "+(replay?"fixed replay":"recorded")+" continuous "+segment.goal);
 }
 N5InputTick BossInput(){var boss=FindFirstObjectByType<BossEncounter>();var v=new N5InputTick();if(!boss.Fighting){v.axis=1;return v;}double phase=Time.timeAsDouble-boss.PhaseStarted;if(bossJumped!=boss.PhaseStarted&&Motor.Grounded&&((boss.State==BossEncounter.Phase.MeleeTell&&phase>boss.tuning.meleeTell-.38)||(boss.State==BossEncounter.Phase.WaveTell&&phase>boss.tuning.waveTell-.38))){jumpUntil=driver.tick+28;bossJumped=boss.PhaseStarted;}if(Body.position.x<13.95f)v.axis=1;else if(Body.position.x>14.10f)v.axis=-1;if(driver.tick<jumpUntil)v.held|=2;if(driver.tick>=nextAttack&&Motor.Grounded){v.axis=1;v.held|=8;nextAttack=driver.tick+20;}return v;}
}





