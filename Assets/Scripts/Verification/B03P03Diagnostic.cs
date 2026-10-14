#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Observe the original P03 recording; no writes to game movement/combat/state.
public sealed class B03P03Diagnostic:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize()=>N5Args.Start<B03P03Diagnostic>("-verify-b03-p03");
 RouteTickDriver driver;PogoTestCourse course;bool failed,done;string reason="";N5InputTick last;
 protected override void Cleanup(){if(driver!=null)driver.active=false;}
 void Snapshot(string kind){if(Player==null)return;lines.Add($"{kind} tick={driver?.tick} held={last?.held} axis={last?.axis} x={Body.position.x:F5} y={Body.position.y:F5} vx={Body.linearVelocity.x:F5} vy={Body.linearVelocity.y:F5} ground={Motor.Grounded} DJ={Motor.DoubleJumpAvailable} dash={Player.GetComponent<PlayerDash>().IsDashing} pogos={course?.SectionPogos}");}
 void Update(){if(driver!=null&&driver.active&&RoomSession.Instance?.Respawning==true){Snapshot("FAIL-FRAME");reason="actual death";failed=true;driver.active=false;}}
 protected override IEnumerator Tests(){yield return Until(()=>FindFirstObjectByType<PogoTestCourse>()!=null&&Player!=null,"PogoTest boot");course=FindFirstObjectByType<PogoTestCourse>();yield return Ready();yield return new WaitForSecondsRealtime(.4f);course.BeginSection(2);yield return Keys();yield return Until(()=>Motor.Grounded,"P03 initial ground");yield return new WaitForSecondsRealtime(.1f);
  var library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(N5Args.Value("-pogo-library","Archive/validation-2026-10/N9-pogo-library.json")));var segment=library.segments[2];driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;driver.tick=0;
  driver.observe=()=>{Snapshot("TRACE");if(RoomSession.Instance.Respawning){failed=true;reason="actual death";}if(course.Completed>=3)done=true;if(done||failed)driver.active=false;};
  driver.produce=()=>{if(driver.tick>=segment.inputs.Count){Snapshot("FAIL-FRAME");failed=true;reason="recording exhausted";driver.active=false;return new N5InputTick();}last=segment.inputs[driver.tick];return last;};driver.active=true;double started=Time.realtimeSinceStartupAsDouble;yield return Until(()=>done||failed,"P03 replay",22);driver.active=false;Snapshot(done?"GOAL-FRAME":"FAIL-FRAME");yield return Keys();lines.Add($"MEASURE P03 outcome={(done&&!failed?"PASS":"FAIL")} seconds={Time.realtimeSinceStartupAsDouble-started:F3} reason={reason}");Check(done&&!failed,"P03 original recording goal");
 }
}

#endif
