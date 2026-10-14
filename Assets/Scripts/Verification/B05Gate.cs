#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class B05Gate:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Gate>("-verify-b05-gate");
 protected override IEnumerator Tests(){yield return NewGame();Check(RoomSession.Instance.RequestTransition("A03","Assets/Scenes/V2/A03_V2.unity","Entry"),"gate fixture");yield return Ready();Check(!Player.GetComponent<PlayerDash>().hasDash&&!Motor.hasDoubleJump,"pre Dash ownership");var gate=FindFirstObjectByType<DashGate>().GetComponent<BoxCollider2D>();Check(gate.bounds.min.y<=5&&gate.bounds.max.y>=16,"gate seals shelf to ceiling; no upper gap");PlaceForAuthorizedSetup(new Vector2(50.6f,5.81f));yield return new WaitForSeconds(.3f);Check(Motor.Grounded,"launch on second step");yield return Keys(Key.RightArrow,Key.Z,Key.C);yield return new WaitForSeconds(.5f);yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.2f);lines.Add("MEASURE no Dash approach pos="+Body.position+" gate="+gate.bounds);Check(RoomSession.Instance.CurrentRoomId=="A03"&&!Player.GetComponent<PlayerDash>().hasDash&&Body.position.x<gate.bounds.max.x+.35f,"jump + locked Dash cannot cross gate");yield return Keys();}
}

#endif
