using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B07GateVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07GateVerifier>("-verify-b07-gates");
 IEnumerator Fixture(string id,Vector2 at,bool dash=false,bool wall=false)
 {
  yield return Ready();
  if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V3SceneMap.Path(id),"Entry"),"gate fixture "+id);yield return Ready();}
  yield return Keys();Motor.hasDoubleJump=false;Motor.hasWallJump=wall;Player.GetComponent<PlayerDash>().hasDash=dash;PlaceForAuthorizedSetup(at);yield return new WaitForFixedUpdate();
 }
 protected override IEnumerator Tests()
 {
  yield return NewGame();ProgressSave.Enabled=false;
  for(int mode=0;mode<3;mode++)
  {
   yield return Fixture("A03",new Vector2(42,8.31f));yield return Keys(Key.RightArrow);
   for(int i=0;i<5;i++){yield return Keys(Key.RightArrow,mode==0?Key.Z:mode==1?Key.C:Key.X);yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.25f);}
   Check(RoomSession.Instance.CurrentRoomId=="A03"&&Body.position.x<44&&Health.HP>0,"G-D no Dash mode="+mode+" x="+Body.position.x);
  }
  yield return Fixture("A02",new Vector2(13.1f,13.81f));float peak=Body.position.y;
  for(int i=0;i<12;i++){yield return Keys(i%2==0?Key.LeftArrow:Key.RightArrow,Key.Z);yield return new WaitForSeconds(.25f);peak=Mathf.Max(peak,Body.position.y);yield return Keys();}
  Check(RoomSession.Instance.CurrentRoomId=="A02"&&peak<18&&Health.HP>0,"G-W no wall/Dash/DJ/Pogo peak="+peak);
  // All ownership combinations of wall jump/Dash, with both attack directions, against the high ledge.
  for(int mask=0;mask<8;mask++)
  {
   yield return Fixture("C03",new Vector2(58,1.81f),(mask&1)!=0,(mask&2)!=0);
   for(int i=0;i<7;i++){yield return Keys(Key.RightArrow,Key.Z);yield return new WaitForSeconds(.12f);yield return Keys(Key.RightArrow);yield return Keys(Key.RightArrow,Key.Z);yield return Keys(Key.RightArrow,Key.C);yield return Keys(Key.RightArrow,Key.X,(mask&4)!=0?Key.DownArrow:Key.UpArrow);yield return new WaitForSeconds(.15f);}
   Check(RoomSession.Instance.CurrentRoomId=="C03"&&Health.HP>0,"G-J no DJ ownership/attack mask="+mask+" pos="+Body.position);
  }
  foreach(var entry in new[]{new[]{"A03","V3-shortcut-A05"},new[]{"B01","V3-shortcut-B04"}})
  {
   yield return Fixture(entry[0],Vector2.zero,true,true);var target=FindObjectsByType<BreakableWall>(FindObjectsSortMode.None).Single(w=>w.wallId==entry[1]);float side=target.breakableFrom==BreakableWall.Side.Left?1:-1;
   PlaceForAuthorizedSetup((Vector2)target.transform.position+Vector2.right*side*1.1f);
   for(int i=0;i<6;i++){yield return Keys(side>0?Key.LeftArrow:Key.RightArrow,Key.X);yield return Keys(side>0?Key.LeftArrow:Key.RightArrow);yield return new WaitForSeconds(.35f);}
   Check(!target.Broken&&target.Hits==0,"shortcut blocked-side actual attacks "+entry[1]);
  }
  yield return Fixture("B01",new Vector2(36,2),true,true);var pit=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Single(e=>e.exitId=="Pit");PlaceForAuthorizedSetup(new Vector2(36,-1));Body.linearVelocity=Vector2.up*8;Check(!pit.GetComponent<V3ExitRule>().Allows(Player.GetComponent<Collider2D>()),"pit upward motion denied");yield return new WaitForFixedUpdate();yield return null;Check(RoomSession.Instance.CurrentRoomId=="B01"&&!RoomSession.Instance.Transitioning,"pit upward physical overlap rejected");
  yield return Fixture("A03",new Vector2(30,1.81f),true,true);Motor.hasDoubleJump=true;
  Check(!FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Any(e=>e.destinationRoomId=="B01"&&e.transform.position.x<35),"pit landing has no reverse B01 exit");
  for(int i=0;i<6;i++){yield return Keys(Key.Z);yield return Keys();yield return Keys(Key.Z);yield return Keys();yield return new WaitForSeconds(.3f);}
  Check(RoomSession.Instance.CurrentRoomId=="A03"&&Health.HP>0,"pit reverse jumping cannot enter B01");
 }
}
