#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Candidate route controller reused without placement, ability grants or room fixture transitions.
public sealed class B07WorldVerifier:B07RoomVerifier
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void InitWorld()=>N5Args.Start<B07WorldVerifier>("-verify-b07-world");
 protected override IEnumerator Tests()
 {
  Check(V3SceneMap.Enabled,"explicit V3 world");yield return NewGame();yield return new WaitForSeconds(.3f);
  string[] rooms={"A01","A02","A03","A04","A05","A03","B01","B02","B03","B04","B01","A03","A02","B05","C01","C02","C03","C05","C03","C04","C06","S01"};
  string[] names={"main","main","main","main","main","dash","main","main","main","main","return","return","revisit","main","main","main","main","main","revisit","main","main","main"};
  driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;var rhythm=new List<string>{"room,route,seconds,ticks"};
  for(int index=0;index<rooms.Length;index++)
  {
   roomId=rooms[index];Check(RoomSession.Instance.CurrentRoomId==roomId,"continuous room "+index+" "+roomId);yield return Ready();
   var plan=FindFirstObjectByType<V3RoomRoutes>();route=plan.routes.Single(r=>r.name==names[index]);
   if(roomId=="A02"&&N5Args.Has("-b07-skip-a02"))
   {
    lines.Add("PARTIAL authorized QA skip A02 "+names[index]+" at "+index+"; known failed route is not retried");
    PlaytestLogger.Instance?.ObserveExit("QA-A02-skip",route.goal);
    Check(RoomSession.Instance.RequestTransition(route.goal,V3SceneMap.Path(route.goal),"Entry"),"authorized QA skip A02");yield return Ready();continue;
   }
   stage=0;jumpAt=climbRelease=-1000;previousJumps=Motor.WallJumpsPerformed;pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;nextAttack=0;climbHeld=true;climbWaiting=dead=false;bossPhase=-1;driver.tick=0;
   var recording=new Recording{room=roomId,route=names[index],goal=route.goal};
   driver.produce=()=>{var input=Generate();input.tick=driver.tick;recording.inputs.Add(input);return input;};driver.observe=()=>{if(Complete())driver.active=false;};
   driver.active=true;double began=Time.realtimeSinceStartupAsDouble,limit=began+(roomId=="C06"?180:90);
   while(driver.active&&!dead&&Time.realtimeSinceStartupAsDouble<limit){if(Complete()){driver.active=false;break;}if(RoomSession.Instance!=null&&RoomSession.Instance.Respawning||Player!=null&&Health!=null&&Health.HP<=0)dead=true;yield return null;}
   driver.active=false;yield return Keys();double elapsed=Time.realtimeSinceStartupAsDouble-began;
   bool ended=route.goal=="Ending";lines.Add("MEASURE continuous "+index+" "+roomId+" "+names[index]+" seconds="+elapsed+" ticks="+driver.tick+" stage="+stage+" pos="+(ended||Player==null?Vector2.zero:Body.position)+" HP="+(ended||Player==null?0:Health.HP)+" dash="+(!ended&&Player!=null&&Player.GetComponent<PlayerDash>().hasDash)+" wall="+(!ended&&Player!=null&&Motor.hasWallJump)+" DJ="+(!ended&&Player!=null&&Motor.hasDoubleJump));
   rhythm.Add(roomId+","+names[index]+","+elapsed.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+driver.tick);
   File.WriteAllText(Path.ChangeExtension(Evidence,"segment-"+index+".json"),JsonUtility.ToJson(recording,true));File.WriteAllLines(Path.ChangeExtension(Evidence,"rhythm.csv"),rhythm);
   if(roomId=="A02"&&!Complete()&&N5Args.Has("-b07-skip-a02"))
   {
    lines.Add("PARTIAL authorized A02 skip at "+index+"; remainder is not uninterrupted completion");
    Check(RoomSession.Instance.RequestTransition(route.goal,V3SceneMap.Path(route.goal),"Entry"),"authorized QA skip A02");yield return Ready();continue;
   }
   Check(!dead&&Complete(),"continuous traversal "+index+" "+roomId+" -> "+route.goal);
  }
  Check(FindFirstObjectByType<EndingMenu>()!=null,"world reaches summit ending");yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"ending to title");Check(ProgressSave.TryLoad(out var saved)&&saved.endingReached&&saved.checkpointId=="CP-C01","full world ending save");
 }
}

#endif
