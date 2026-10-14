#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class B07T0Verifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07T0Verifier>("-verify-b07-t0");
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"explicit isolated save required");
  yield return Ready();yield return new WaitForSeconds(.3f);
  var course=FindFirstObjectByType<WallJumpTestCourse>();Check(course!=null,"course owns runtime tuning");
  var original=(PlayerTuning)typeof(WallJumpTestCourse).GetField("original",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(course);
  course.SelectPreset(1);Check(Motor.Tuning!=original,"test tuning is a copy");
  yield return Keys();PlaceForAuthorizedSetup(new Vector2(25.63f,5));Body.linearVelocity=Vector2.down;
  yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.12f);Check(Motor.WallSliding&&Motor.Facing==-1,"right wall faces away");
  yield return Keys(Key.RightArrow,Key.C);Check(Player.GetComponent<PlayerDash>().IsDashing&&Body.linearVelocity.x<0,"wall slide dash moves away despite wallward input");
  yield return Keys();yield return new WaitForSeconds(.6f);
  PlaceForAuthorizedSetup(new Vector2(5,1.81f));yield return new WaitForSeconds(.2f);
  typeof(PlayerMotor).GetField("wallLockUntil",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(Motor,Time.fixedUnscaledTimeAsDouble+.15);
  yield return Keys(Key.RightArrow);Check(Motor.Grounded&&Body.linearVelocity.x>0,"landing clears remaining kick lock immediately");
  Health.TakeDamage(Health.HP,Body.position+Vector2.left);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="A01"&&!RoomSession.Instance.Respawning,"death leaves test course",15);
  Check(Motor.Tuning==original,"death and different room restore original Asset");
  var map=new GameObject("Isolated V2 fixture");map.SetActive(false);map.AddComponent<V2SceneMap>().rooms=new[]{"A01","B01","C01"};map.SetActive(true);
  ProgressSave.Enabled=true;ProgressSave.EndingReached=false;ProgressSave.BossDefeated=true;
  ProgressSave.Write(new ProgressData{dash=true,bossDefeated=true,checkpointId="CP-C04"});ProgressSave.MarkEndingReached();
  Check(ProgressSave.Continue()&&ProgressSave.Pending.checkpointId=="CP-C01"&&ProgressSave.PendingScene.EndsWith("C01_V2.unity"),"first ending persists C01 once");
  ProgressSave.Write(new ProgressData{dash=true,bossDefeated=true,endingReached=true,checkpointId="CP-B01"});
  Check(ProgressSave.Continue()&&ProgressSave.PendingScene.EndsWith("B01_V2.unity"),"later B01 checkpoint respected");
  ProgressSave.MarkEndingReached();Check(ProgressSave.TryLoad(out var saved)&&saved.checkpointId=="CP-B01","repeated ending does not reset later checkpoint");
  map.SetActive(false);var replacement=new GameObject("Empty mapping");replacement.SetActive(false);replacement.AddComponent<V2SceneMap>().rooms=System.Array.Empty<string>();replacement.SetActive(true);
  Check(ProgressSave.Continue()&&ProgressSave.PendingScene=="Assets/Scenes/Ending.unity","default world retains Continue Ending");
  ProgressSave.Enabled=false;ProgressSave.Pending=null;
 }
}
#endif
