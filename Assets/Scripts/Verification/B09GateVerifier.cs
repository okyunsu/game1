#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using UnityEngine;
public sealed class B09GateVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09GateVerifier>("-verify-b09-gates");
 protected override IEnumerator Tests()
 {
  yield return NewGame();ProgressSave.Enabled=false;ProgressSave.BossDefeated=false;
  Check(RoomSession.Instance.RequestTransition("C06",V4SceneMap.Path("C06"),"Entry"),"boss fixture load");yield return Ready();yield return new WaitForSeconds(.5f);
  var upper=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).First(e=>e.exitId=="C06-S01");
  Check(!upper.enabled&&!upper.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"before boss upper exit refuses disabled callback");
  // A disabled MonoBehaviour still receives Unity trigger messages: overlap it for real physics ticks.
  PlaceForAuthorizedSetup(upper.GetComponent<Collider2D>().bounds.center);Body.gravityScale=0;Body.linearVelocity=Vector2.zero;yield return new WaitForSeconds(.7f);
  Check(RoomSession.Instance.CurrentRoomId=="C06","disabled boss trigger causes no transition");
  ProgressSave.BossDefeated=true;upper.enabled=true;
  Check(upper.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"boss cleared upper exit allows");
  Check(RoomSession.Instance.RequestTransition("S02",V4SceneMap.Path("S02"),"Entry"),"oneway fixture load");yield return Ready();yield return new WaitForSeconds(.5f);
  var reverse=FindFirstObjectByType<RoomExit>();Check(!reverse.enabled&&!reverse.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"oneway reverse refuses disabled callback");
  PlaceForAuthorizedSetup(reverse.GetComponent<Collider2D>().bounds.center);Body.gravityScale=0;Body.linearVelocity=Vector2.zero;yield return new WaitForSeconds(.7f);
  Check(RoomSession.Instance.CurrentRoomId=="S02","oneway trigger causes no transition");
 }
}
#endif
