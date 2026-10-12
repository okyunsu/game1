using System.Collections;
using UnityEngine;
public sealed class B10StageVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10StageVerifier>("-verify-b10-stage");
 protected override IEnumerator Tests(){
  Check(ProgressSave.TryLoad(out var saved),"actual natural snapshot readable; no fixture write");
  yield return ContinueGame();
  Check(Player.GetComponent<PlayerDash>().hasDash==saved.dash&&Motor.hasWallJump==saved.wallJump&&Motor.hasDoubleJump==saved.doubleJump&&Player.GetComponent<PlayerSlam>().hasSlam==saved.hasSlam,"actual Continue restores natural abilities");
  yield return Until(()=>Motor.Grounded,"checkpoint landing",3);
  Check(Motor.Grounded,"actual natural checkpoint landing grounded");
  Row("MEASURE natural Continue checkpoint="+saved.checkpointId+" room="+RoomSession.Instance.CurrentRoomId);
 }
 void Row(string value)=>lines.Add(value);
}
