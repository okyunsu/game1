#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using UnityEngine;

public sealed class B21RuleProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21RuleProbe>("-verify-b21-rule");
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return new WaitForSeconds(.6f);
  var rule=FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<V4ExitRule>().First(r=>r.GetComponent<RoomExit>().enabled&&string.IsNullOrEmpty(r.gate));
  var shape=Player.GetComponent<Collider2D>();
  Check(string.IsNullOrEmpty(rule.closedUntilWallBroken)&&rule.Allows(shape),"absent wall lock preserves existing V4 exit");
  rule.closedUntilWallBroken="B21-isolated-rule-wall";
  Check(rule.WallLocked&&!rule.Allows(shape),"intact persistent wall locks exit");yield return null;
  Check(rule.GetComponent<SpriteRenderer>().color==Color.gray,"locked door reuses existing gray closed marker");
  Check(!rule.GetComponent<BoxCollider2D>().isTrigger&&rule.gameObject.layer==6,"locked doorway is a physical solid");
  ProgressSave.MarkWallBroken(rule.closedUntilWallBroken,RoomSession.Instance);
  Check(!rule.WallLocked&&rule.Allows(shape),"brokenWalls key opens exit without schema change");
  rule.gate="DJ";Motor.hasDoubleJump=false;Check(!rule.Allows(shape),"wall unlock does not bypass ability gate");
  Motor.hasDoubleJump=true;Check(rule.Allows(shape),"ability and wall conditions both required");
  var seal=new GameObject("B21 isolated seal").AddComponent<SealFloor>();rule.elevatorSeal=seal;
  Check(!rule.Allows(shape),"wall unlock does not bypass elevator seal");rule.elevatorSeal=null;Destroy(seal.gameObject);
  rule.gate="";rule.closedUntilWallBroken="";Check(rule.Allows(shape),"blank lock restores legacy condition");
  yield return null;yield return null;Check(rule.GetComponent<BoxCollider2D>().isTrigger,"blank lock restores original trigger");
  yield return new WaitForSecondsRealtime(10);
 }
}
#endif
