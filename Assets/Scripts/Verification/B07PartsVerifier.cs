#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class B07PartsVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07PartsVerifier>("-verify-b07-parts");
 protected override IEnumerator Tests()
 {
  Check(V3SceneMap.Enabled,"V3 parts opted in");yield return NewGame();
  var drop=new GameObject("Isolated downward exit fixture");drop.transform.position=new Vector2(12,6);drop.AddComponent<BoxCollider2D>().isTrigger=true;
  var exit=drop.AddComponent<RoomExit>();exit.exitId="QA-drop";exit.destinationRoomId="A02";exit.destinationScenePath=V3SceneMap.Path("A02");exit.destinationSpawnId="Entry";var rule=drop.AddComponent<V3ExitRule>();rule.downwardOnly=true;
  yield return Keys();PlaceForAuthorizedSetup(new Vector2(12,6));Body.linearVelocity=Vector2.up*5;yield return new WaitForFixedUpdate();yield return null;
  Check(RoomSession.Instance.CurrentRoomId=="A01"&&!RoomSession.Instance.Transitioning,"pit rejects rising overlap");
  PlaceForAuthorizedSetup(new Vector2(12,8));yield return new WaitForSeconds(.1f);PlaceForAuthorizedSetup(new Vector2(12,6));Body.linearVelocity=Vector2.down*5;
  yield return Until(()=>RoomSession.Instance.CurrentRoomId=="A02"&&!RoomSession.Instance.Transitioning,"downward fixture exit");yield return Ready();Destroy(drop);
  Check(RoomSession.Instance.RequestTransition("C06",V3SceneMap.Path("C06"),"FromLeft"),"isolated arena setup");yield return Ready();
  var door=FindFirstObjectByType<V3BossDoor>();Check(!door.Opened&&!door.upperExit.enabled&&door.blocker.activeSelf,"summit door closed before boss");foreach(var step in door.stairs)Check(!step.activeSelf,"arena stairs absent during fight");
  PlaceForAuthorizedSetup(new Vector2(6,1.81f));yield return Until(()=>door.boss.Fighting,"boss fight starts");door.boss.ReceiveHit(door.boss.HP,Body.position);
  Check(door.Opened&&door.upperExit.enabled&&!door.blocker.activeSelf,"defeated event opens upper door");
  yield return new WaitForSecondsRealtime(2.3f);Check(SceneManager.GetActiveScene().name=="C06_V3"&&!GameState.GetOrCreate().Completing,"C06 victory never enters Ending directly");
  yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04","boss-before-ending Continue CP-C04");
  Check(RoomSession.Instance.RequestTransition("C06",V3SceneMap.Path("C06"),"FromLeft"),"boss revisit");yield return Ready();door=FindFirstObjectByType<V3BossDoor>();Check(FindFirstObjectByType<BossEncounter>()==null&&door.Opened,"saved victory suppresses boss and opens summit door");
  yield return PhysicalDoor("S01");var ending=FindFirstObjectByType<V3SummitEnding>();Check(ending!=null,"summit trigger authored");PlaceForAuthorizedSetup(ending.transform.position);
  yield return Until(()=>ending.Lighting,"central trigger starts fade");double began=ending.Began;
  yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"2s ending fade");double elapsed=Time.realtimeSinceStartupAsDouble-began;Check(elapsed>=1.95&&elapsed<3,"real fade duration "+elapsed);
  Check(ProgressSave.TryLoad(out var saved)&&saved.endingReached&&saved.checkpointId=="CP-C01","ending writes one-time C01");
  yield return Tap(Key.Z);yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="C01","ending Continue C01_V3");
  lines.Add("MEASURE parts: downward only, boss door+suppression, CP-C04 before ending, summit fade2s, CP-C01 after ending");
 }
}
#endif
