using System.Collections;
using UnityEngine;

public sealed class B06LaunchVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06LaunchVerifier>("-verify-b06-launch");
    protected override IEnumerator Tests()
    {
        if(N5Args.Has("-walljumptest"))
        {
            yield return Ready();yield return new WaitForSeconds(.25f);
            Check(RoomSession.Instance.CurrentRoomId=="WallJumpTest"&&!ProgressSave.Enabled,"launcher opens non-saving wall test");
            Check(Motor.hasWallJump&&Player.GetComponent<PlayerDash>().hasDash,"wall+dash owned");
            Check(FindObjectsByType<BreakableWall>(FindObjectsSortMode.None).Length==2,"two playable breakable fixtures");
            Check(FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==0&&FindObjectsByType<KillZone>(FindObjectsSortMode.None).Length==0,"safe test, no enemy/death hazards");
        }
        else
        {
            yield return NewGame();yield return new WaitForSeconds(.25f);
            Check(RoomSession.Instance.CurrentRoomId=="A01"&&UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="A01_V2","launcher new game starts V2 A01");
            Check(GuidePresentation.Disabled?GuidePresentation.Instance.GoalText=="":GuidePresentation.Instance.GoalText.Length>0,"launcher guide mode="+!GuidePresentation.Disabled);
            Check(GuidePresentation.Disabled?FindObjectsByType<MainPathLantern>(FindObjectsSortMode.None).Length==0:FindObjectsByType<MainPathLantern>(FindObjectsSortMode.None).Length==1,"launcher lamp mode");
        }
        Check(Debug.isDebugBuild,"delivery is Development build");yield return new WaitForSeconds(.5f);
    }
}
