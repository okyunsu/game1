#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
public sealed class B10StageVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10StageVerifier>("-verify-b10-stage");
 protected override IEnumerator Tests(){
  Check(ProgressSave.TryLoad(out var saved),"snapshot readable; verifier does not write fixture");
  yield return ContinueGame();
  Check(Player.GetComponent<PlayerDash>().hasDash==saved.dash&&Motor.hasWallJump==saved.wallJump&&Motor.hasDoubleJump==saved.doubleJump&&Player.GetComponent<PlayerSlam>().hasSlam==saved.hasSlam,"actual Continue restores natural abilities");
  yield return Until(()=>Motor.Grounded,"checkpoint landing",3);
  Check(Motor.Grounded,"actual natural checkpoint landing grounded");
  Row("MEASURE natural Continue checkpoint="+saved.checkpointId+" room="+RoomSession.Instance.CurrentRoomId);
  if(N5Args.Has("-verify-b11-launch")){
   Check(RoomSession.Instance.CheckpointId==saved.checkpointId,"fixture checkpoint restored");
   if(!saved.endingReached){Check(RoomSession.Instance.RequestTransition("C06",V4SceneMap.Path("C06"),"C04-C06"),"fixture arena transition");yield return Ready();var boss=FindFirstObjectByType<BossEncounter>();Check(boss!=null,"boss arena loaded");Row("MEASURE BossTuning="+boss.tuning.name+" extendedPatterns="+boss.tuning.extendedPatterns);yield return new WaitForSeconds(10);}
   else Check(RoomSession.Instance.CurrentRoomId=="A01"&&ProgressSave.EndingReached&&ProgressSave.BossDefeated,"post-ending A01 flags restored");
  }
  if(!N5Args.Has("-verify-b11-launch"))yield return new WaitForSecondsRealtime(10);
 }
 void Row(string value)=>lines.Add(value);
}
#endif
