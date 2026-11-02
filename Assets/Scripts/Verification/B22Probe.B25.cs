#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B25Tests(string mode)
 {
  if(mode=="b25-lift")yield return B25Lift();
  else if(mode=="b25-sink")yield return B25Sink();
  else if(mode=="b25-map")yield return B25Map();
  else if(mode=="b25-first")yield return B25FirstPass();
  else if(mode=="b25-optional")yield return B25Optional();
  else if(mode=="b25-boss")yield return B25BossField();
  else if(mode=="b25-wall")yield return B25Revisit();
  else if(mode=="b25-first-four")yield return B25FirstFour();
  else throw new ArgumentException("Unknown B25 mode "+mode);
 }
 void CaptureScreen(string path)
 {
  var type=Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule",true);
  type.GetMethod("CaptureScreenshot",new[]{typeof(string),typeof(int)}).Invoke(null,new object[]{path,1});
 }
 IEnumerator B25Map()
 {
  Check(WorldMapLayout.Current.rooms.Length==12&&WorldMapLayout.Current.rooms.Count(r=>r.id.StartsWith("B"))==6,"AB12 map loaded");
  var map=V4WorldMap.Instance;Check(map!=null&&!map.All,"normal visited-only map fixture");
  Check(map.VisibleRooms.All(r=>!r.id.StartsWith("B")),"B rooms initially unvisited and hidden");
  map.Cycle();yield return new WaitForSeconds(.2f);
  string folder=System.IO.Path.GetDirectoryName(Evidence);
  CaptureScreen(System.IO.Path.Combine(folder,"map-unvisited.png"));yield return new WaitForSeconds(.5f);
  foreach(var room in WorldMapLayout.Current.rooms)ProgressSave.VisitRoom(room.id,RoomSession.Instance);
  map.RefreshProgress();Check(map.VisibleRooms.Count(r=>r.id.StartsWith("B"))==6,"six visited B rooms visible");
  Check(map.VisibleDoors.Any(d=>d.id=="A03-B01")&&map.VisibleDoors.Any(d=>d.id=="B05-C01"&&d.stub),"new floor links and stub displayed");
  CaptureScreen(System.IO.Path.Combine(folder,"map-visited.png"));yield return new WaitForSeconds(.5f);
  Check(System.IO.File.Exists(System.IO.Path.Combine(folder,"map-visited.png")),"map screenshots captured");
  Record($"MEASURE map rooms={map.VisibleRooms.Length} doors={map.VisibleDoors.Length} B=6");
 }
 IEnumerator B25Sink()
 {
  yield return B2Intro();
  Check(Boss.sinkingPedestal!=null&&Math.Abs(Boss.sinkingPedestal.GetComponent<Collider2D>().bounds.max.y-Boss.floorY)<.001,"combat pedestal flush with floor");
  var original=Health.tuning;Health.tuning=Instantiate(original);Health.tuning.playerMaxHP=100;Health.RestoreMax();
  Motor.hasWallJump=Motor.hasDoubleJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
  int floor=0,pedestal=0;
  for(int i=1;i<=10;i++){yield return B2JumpWave(5,1,"B25 floor trial="+i);if(waveHit)floor++;}
  for(int i=1;i<=10;i++){yield return B2JumpWave(12,1,"B25 sunk pedestal trial="+i);if(waveHit)pedestal++;}
  Record($"MEASURE B25 noWallJump floorHits={floor}/10 pedestalAvoidances={10-pedestal}/10 pedestalTop={Boss.sinkingPedestal.GetComponent<Collider2D>().bounds.max.y:F3}");
  Check(floor==10&&pedestal==10,"floor hit10/10 and pedestal bypass0/10");Health.tuning=original;Health.RestoreMax();
 }
 IEnumerator B25Lift()
 {
  yield return RoomAt("A03");yield return ResetAt(new Vector2(44.8f,1.015f));
  var lift=Room.GetComponentsInChildren<LeverLift>().Single();
  for(int trip=1;trip<=3;trip++)
  {
   yield return LiftRide(true);yield return LiftRide(false);
   Record("PASS A03 round trip "+trip);
  }
  yield return LiftRide(true);
  yield return ResetAt(new Vector2(41,1.015f));
  yield return GoX(44.8f);yield return LiftRide(true);
  Record("PASS lower-floor call after disembarking and reboarding");
  foreach(string id in new[]{"B01","C01"})
  {
   yield return RoomAt(id,"Assets/Scenes/V4/"+id+"_V4.unity");
   foreach(var old in Room.GetComponentsInChildren<LeverLift>())
   {
    Check(!old.startLow&&!old.Unlocked&&Math.Abs(old.GetComponent<Collider2D>().bounds.max.y-old.high)<.02,"V4 "+id+" starts high locked");
    old.Call(false);Check(!old.Unlocked,"V4 lower cannot unlock");old.Call(true);Check(old.Unlocked,"V4 upper unlocks");
    yield return new WaitForSeconds(.15f);Check(old.GetComponent<Collider2D>().bounds.max.y<old.high,"V4 descends after upper call");
   }
  }
 }
}
#endif
