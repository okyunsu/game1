#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;

public sealed class B07LaunchVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07LaunchVerifier>("-verify-b07-launch");
 protected override IEnumerator Tests()
 {
  string expected=N5Args.Value("-b07-expected-room","A01");
  if(expected=="WallJumpTest")yield return Ready();else if(N5Args.Has("-b07-qa"))yield return ContinueGame();else yield return NewGame();
  yield return new WaitForSeconds(.2f);Check(RoomSession.Instance.CurrentRoomId==expected,"launcher room "+expected);Check(Debug.isDebugBuild,"delivery is Development");
  if(expected!="WallJumpTest")
  {
   Check(V3SceneMap.Enabled,"V3 launcher mapping");
   Check(Player.GetComponent<PlayerDash>().hasDash==N5Args.Has("-b07-expected-dash")&&Motor.hasWallJump==N5Args.Has("-b07-expected-wall")&&Motor.hasDoubleJump==N5Args.Has("-b07-expected-jump"),"launcher ability ownership");
   Check(GuidePresentation.Disabled?GuidePresentation.Instance.GoalText=="":!string.IsNullOrEmpty(GuidePresentation.Instance.GoalText),"launcher guide visibility");
  }
  Check(Health.HP>0,"launcher alive");
 }
}
#endif
