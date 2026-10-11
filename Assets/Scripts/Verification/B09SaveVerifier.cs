using System.Collections;
using UnityEngine;
public sealed class B09SaveVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09SaveVerifier>("-verify-b09-save");
 protected override IEnumerator Tests()
 {
  yield return NewGame();
  var fixtures=new[]{new ProgressData{dash=true,checkpointId="CP-A01"},new ProgressData{dash=true,wallJump=true,checkpointId="CP-B01"},new ProgressData{dash=true,wallJump=true,doubleJump=true,checkpointId="CP-C01"},new ProgressData{dash=true,wallJump=true,doubleJump=true,bossDefeated=true,endingReached=true,hasSlam=true,reachedS02=true,checkpointId="CP-A01"}};
  foreach(var fixture in fixtures){yield return Title();Check(ProgressSave.Write(fixture),"isolated checkpoint fixture write "+fixture.checkpointId);yield return ContinueGame();Check(Player.GetComponent<PlayerDash>().hasDash==fixture.dash&&Motor.hasWallJump==fixture.wallJump&&Motor.hasDoubleJump==fixture.doubleJump&&Player.GetComponent<PlayerSlam>().hasSlam==fixture.hasSlam,"Continue restores owned abilities "+fixture.checkpointId);Check(RoomSession.Instance.CurrentRoomId==(fixture.checkpointId=="CP-B01"?"B01":fixture.checkpointId=="CP-C01"?"C01":"A01"),"Continue restores room "+fixture.checkpointId);Check(ProgressSave.ReachedS02==fixture.reachedS02,"S02 flag restored");yield return Until(()=>Motor.Grounded,"checkpoint landing",3);Check(Motor.Grounded,"restored authored foot spawn grounded");}
 }
}

