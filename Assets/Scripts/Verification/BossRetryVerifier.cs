using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class BossRetryVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize()=>N5Args.Start<BossRetryVerifier>("-verify-boss-retry");
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N5-TB-boss-retry.txt");
 protected override IEnumerator Tests(){yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04","fixture Continue CP-C04");
  for(int i=0;i<3;i++){yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="C06",Key.RightArrow,"walk to boss");yield return Ready();var boss=FindFirstObjectByType<BossEncounter>();Check(boss.HP==20&&boss.State==BossEncounter.Phase.Idle&&boss.leftExit.enabled,"precombat HP20/Idle/unlocked retry "+i);yield return WalkTo(()=>boss.Fighting,Key.RightArrow,"boss start");Check(boss.HP==20&&boss.State==BossEncounter.Phase.MeleeTell&&!boss.leftExit.enabled,"first pattern and combat exit lock retry "+i);
   yield return Keys(Key.RightArrow);yield return Until(()=>RoomSession.Instance.Respawning,"natural boss contact death "+i,35);yield return Keys();yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04"&&Health.HP==5,"death restores CP-C04/HP5 "+i);Check(System.Array.Exists(FindObjectsByType<RoomExit>(FindObjectsSortMode.None),e=>e.destinationRoomId=="C06"&&e.enabled),"C04 re-entry exit enabled after death "+i);
  }
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="C06",Key.RightArrow,"final boss entry");yield return Ready();var final=FindFirstObjectByType<BossEncounter>();yield return WalkTo(()=>final.Fighting,Key.RightArrow,"final start");
  // Fight using virtual movement/attack only; no direct damage, health or boss-state writes.
  yield return WalkTo(()=>Body.position.x>=15.7f,Key.RightArrow,"attack position");yield return Keys();
  double deadline=Time.realtimeSinceStartupAsDouble+180,nextAttack=0,jumpUntil=0;double jumpedPhase=-1;
  while(final!=null&&final.HP>0){double now=Time.timeAsDouble;if(Time.realtimeSinceStartupAsDouble>deadline)throw new System.Exception("Boss input fight timeout HP="+final.HP);if(RoomSession.Instance?.Respawning==true)throw new System.Exception("Player died during input fight bossHP="+final.HP);
   double phaseTime=now-final.PhaseStarted;if(jumpedPhase!=final.PhaseStarted&&((final.State==BossEncounter.Phase.MeleeTell&&phaseTime>.42)||(final.State==BossEncounter.Phase.WaveTell&&phaseTime>.62))&&Motor.Grounded){jumpUntil=now+.55;jumpedPhase=final.PhaseStarted;}
   var keys=new System.Collections.Generic.List<Key>();if(Body.position.x<15.65f)keys.Add(Key.RightArrow);else if(Body.position.x>15.85f)keys.Add(Key.LeftArrow);if(now<jumpUntil)keys.Add(Key.Z);if(now>=nextAttack){keys.Add(Key.X);nextAttack=now+.4;}
   UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState(keys.ToArray()));yield return null;
  }
  yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"Ending");Check(ProgressSave.TryLoad(out var saved)&&saved.bossDefeated&&saved.endingReached,"three retries then actual input victory reaches saved ending");
 }
}
