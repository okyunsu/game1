#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B07GuideVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07GuideVerifier>("-verify-b07-guide");
 void Stage(int expected){var guide=GuidePresentation.Instance;Check(guide.Stage==expected,"V3 objective stage "+(expected+1));Check(guide.GoalText==(GuidePresentation.Disabled?"":guide.v3Objectives.objectives[expected].text),"V3 objective text "+(expected+1));}
 protected override IEnumerator Tests()
 {
  yield return NewGame();yield return new WaitForSeconds(.2f);var guide=GuidePresentation.Instance;Check(guide.v3Objectives!=null&&guide.graph!=null,"V3 guide resources");Stage(0);
  RoomSession.Instance.GrantDash();Stage(1);ProgressSave.VisitRoom("B01",RoomSession.Instance);Stage(2);RoomSession.Instance.GrantWallJump();Stage(3);ProgressSave.VisitRoom("B05",RoomSession.Instance);Stage(4);ProgressSave.VisitRoom("C01",RoomSession.Instance);Stage(5);RoomSession.Instance.GrantDoubleJump();Stage(6);ProgressSave.BossDefeated=true;Stage(7);ProgressSave.EndingReached=true;Stage(8);ProgressSave.BossDefeated=ProgressSave.EndingReached=false;
  yield return Tap(Key.F1);Check(guide.DebugVisible==(!GuidePresentation.Disabled&&Debug.isDebugBuild),"F1 build and no-guide visibility");int total=0;
  foreach(var id in V3SceneMap.Rooms)
  {
   if(id!="A01"){Check(RoomSession.Instance.RequestTransition(id,V3SceneMap.Path(id),id=="C06"?"FromLeft":"Entry"),"guide room fixture "+id);yield return Ready();}yield return new WaitForSeconds(.1f);
   var exits=FindObjectsByType<RoomExit>(FindObjectsInactive.Include,FindObjectsSortMode.None);int required=exits.Count(e=>e.mainPath);int visible=FindObjectsByType<MainPathLantern>(FindObjectsSortMode.None).Length;
   Check(visible==(GuidePresentation.Disabled?0:required),"lanterns "+id+" required="+required+" active="+visible);total+=required;
   Check(guide.graph.edges.Count(e=>e.from==id)==exits.Length,"graph matches Scene exits "+id);
  }
  Check(guide.graph.NextExit("B01","A02",out var edge)&&edge.to=="A03","guide leads to mandatory descent");lines.Add("MEASURE V3 guide stages=9 rooms=19 requiredLanterns="+total);
 }
}
#endif
