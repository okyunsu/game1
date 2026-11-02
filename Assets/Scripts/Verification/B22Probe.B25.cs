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
  else throw new ArgumentException("Unknown B25 mode "+mode);
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
