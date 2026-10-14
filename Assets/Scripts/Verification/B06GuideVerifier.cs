#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B06GuideVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06GuideVerifier>("-verify-b06-guide");
    protected override IEnumerator Tests()
    {
        yield return NewGame();yield return new WaitForSeconds(.15f);
        var guide=GuidePresentation.Instance;Check(guide!=null&&guide.objectives!=null&&guide.graph!=null,"objective/graph resources in actual build");
        Check(guide.Stage==0&&(GuidePresentation.Disabled?guide.GoalText=="":guide.GoalText==guide.objectives.objectives[0].text),"new game goal");
        RoomSession.Instance.GrantDash();yield return null;Check(guide.Stage==1,"Dash changes objective");
        RoomSession.Instance.GrantDoubleJump();yield return null;Check(guide.Stage==2,"double jump changes objective");
        ProgressSave.BossDefeated=true;yield return null;Check(guide.Stage==3&&guide.GoalText=="","boss complete hides objective");ProgressSave.BossDefeated=false;
        yield return Tap(Key.F1);
        Check(guide.DebugVisible==(!GuidePresentation.Disabled&&Debug.isDebugBuild),"F1 only Development and guide enabled; development="+Debug.isDebugBuild);
        int total=0;
        foreach(string id in guide.graph.edges.Select(e=>e.from).Distinct().OrderBy(id=>id))
        {
            if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity",id=="C06"?"FromLeft":"Entry"),"room fixture "+id);yield return Ready();Check(RoomSession.Instance.CurrentRoomId==id,"fixture arrived "+id);}
            yield return new WaitForSeconds(.08f);
            int required=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Count(e=>e.mainPath);
            int active=FindObjectsByType<MainPathLantern>(FindObjectsSortMode.None).Length;
            Check(active==(GuidePresentation.Disabled?0:required),"lantern count "+id+" required="+required+" shown="+active);total+=required;
        }
        Check(total==28,"required route directions=28, optional exits unmarked");
        Check(guide.graph.NextExit("A01","C06",out var edge)&&edge.to=="A02","BFS first exit from generated graph");
        if(GuidePresentation.Disabled)Check(guide.GoalText==""&&!guide.DebugVisible,"no-guide goal/debug0 across rooms");
    }
}
#endif
