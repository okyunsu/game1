#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B21ShortcutProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21ShortcutProbe>("-verify-b21-shortcut");
 RoomDefinition Room=>FindFirstObjectByType<RoomDefinition>();
 RoomExit Exit(string id)=>Room.GetComponentsInChildren<RoomExit>().Single(e=>e.exitId==id);
 IEnumerator Setup(string room)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId!=room){Check(RoomSession.Instance.RequestTransition(room,V4SceneMap.Path(room),"Entry"),"fixture "+room);yield return Ready();}
  Health.RestoreMax();yield return new WaitForSeconds(.55f);
 }
 IEnumerator Door(string id,string destination)
 {
  yield return new WaitForSeconds(.5f);var exit=Exit(id);Check(exit.enabled&&exit.GetComponent<V4ExitRule>().Allows(Player.GetComponent<Collider2D>()),"permitted "+Room.roomId+" "+id);
  PlaceForAuthorizedSetup(new Vector2(exit.transform.position.x,exit.GetComponent<Collider2D>().bounds.min.y+.05f));
  yield return new WaitForFixedUpdate();yield return Until(()=>RoomSession.Instance.CurrentRoomId==destination&&!RoomSession.Instance.Transitioning,"physical door "+id);yield return Ready();
  lines.Add("MEASURE door "+id+" to="+destination+" feet="+Body.position.ToString("F4"));
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();Check(!ProgressSave.IsWallBroken("V5-A05-breakable0"),"new game wall intact");
  yield return Setup("A03");var locked=Exit("A05-A03");Check(locked.GetComponent<V4ExitRule>().WallLocked,"A03 shortcut key bound");
  PlaceForAuthorizedSetup(new Vector2(48.5f,1.015f));yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.4f);yield return Keys();
  lines.Add("MEASURE locked A03 approach feet="+Body.position.ToString("F4")+" right="+Player.GetComponent<Collider2D>().bounds.max.x.ToString("F4"));
  Check(Player.GetComponent<Collider2D>().bounds.max.x<=49.02f,"locked doorway physically stops player before room edge");
  Check(RoomSession.Instance.CurrentRoomId=="A03"&&!RoomSession.Instance.Transitioning,"1 new-game A03 approach cannot enter A05; user route bypass0");
  yield return Setup("A05");var wall=Room.GetComponentsInChildren<BreakableWall>(true).Single(w=>w.wallId=="V5-A05-breakable0");
  Check(wall.breakableFrom==BreakableWall.Side.Right,"A05 wall inward-only side");
  var exits=Room.GetComponentsInChildren<RoomExit>();foreach(var exit in exits)exit.enabled=false;
  PlaceForAuthorizedSetup(new Vector2(.3f,1.02f));yield return Keys(Key.RightArrow);yield return Keys();uint seq=Player.GetComponent<PlayerAttack>().Sequence;yield return Tap(Key.X);yield return new WaitForSeconds(.45f);
  var attack=Player.GetComponent<PlayerAttack>();Check(attack.Sequence>seq&&new Bounds(attack.HitboxCenter,attack.HitboxSize).Intersects(wall.GetComponent<Collider2D>().bounds),"left fixture actual attack overlaps wall");
  Check(!wall.Broken&&wall.Hits==0,"5 left-side attack cannot break wall");foreach(var exit in exits)exit.enabled=true;
  PlaceForAuthorizedSetup(new Vector2(2.8f,1.02f));yield return Keys(Key.LeftArrow);yield return Keys();yield return Tap(Key.X);yield return new WaitForSeconds(.45f);
  Check(wall.Broken,"2 inward-side actual left attack breaks wall");
  yield return Door("A05-A03","A03");yield return Door("A05-A03","A05");Check(ProgressSave.IsWallBroken("V5-A05-breakable0"),"3 shortcut traverses both directions");
  ProgressSave.SaveCurrent(RoomSession.Instance);yield return Title();yield return ContinueGame();yield return Setup("A03");yield return Door("A05-A03","A05");
  Check(ProgressSave.IsWallBroken("V5-A05-breakable0")&&!Room.GetComponentsInChildren<BreakableWall>(true).Single().gameObject.activeInHierarchy,"4 Continue preserves open shortcut");
  string[][] steps={new[]{"A01","A01-A02","A02"},new[]{"A02","A01-A02","A01"},new[]{"A02","A02-A03","A03"},new[]{"A03","A02-A03","A02"},new[]{"A02","A02-A03U","A03"},new[]{"A03","A02-A03U","A02"},new[]{"A03","A03-A04","A04"},new[]{"A04","A03-A04","A03"},new[]{"A04","A04-A05","A05"},new[]{"A05","A05-A03","A03"},new[]{"A03","A05-A03","A05"},new[]{"A06","A02-A06","A02"},new[]{"A01","A01-A06","A06"},new[]{"A06","A01-A06","A01"}};
  foreach(var step in steps){yield return Setup(step[0]);Motor.hasDoubleJump=Motor.hasWallJump=true;Player.GetComponent<PlayerDash>().hasDash=true;yield return Door(step[1],step[2]);}
  Check(steps.Length==14,"B18 fourteen directed doors retained after shortcut unlocked");
  foreach(var pair in new[]{new[]{"A01","A01-Z01"},new[]{"A02","A02-B05"},new[]{"A03","A03-B01"},new[]{"A05","A05-Z01"},new[]{"A02","A02-A06"},new[]{"A05","A04-A05"}}){yield return Setup(pair[0]);Check(!Exit(pair[1]).enabled,"stub/reverse remains disabled "+pair[0]+" "+pair[1]);}
 }
}
#endif
