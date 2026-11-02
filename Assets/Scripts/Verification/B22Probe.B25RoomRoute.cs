#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B25RoomRoute()
 {
  string id=N5Args.Value("-b25-route-room","");string target="";double began=Time.timeAsDouble;
  Record("FIXTURE independent room entry and dash ownership; enemies live. No pose/HP writes after entry. A06 owns DJ; B06 optional challenge then return. Failure is recorded, never bypassed.");
  switch(id)
  {
   case "A01":
    yield return B25Pose(id,new Vector2(14,1.015f));target="A02";yield return JumpTo(36.8f,42,1,"A01 shallow hazard",true);break;
   case "A02":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));target="A03";yield return JumpTo(3.5f,8.2f,1,"A02 block");yield return JumpTo(10.3f,14.5f,1,"A02 shallow hazard");yield return JumpTo(18.5f,25.3f,1,"A02 E2",true);break;
   case "A03":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));target="A04";yield return GoX(42.3f);yield return JumpTo(42.3f,44.8f,1,"A03 lift board");yield return LiftRide(true);break;
   case "A04":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));NoAbilities();target="A05";
    yield return JumpTo(4.2f,5.8f,3,"A04 block");yield return GoX(10.8f);yield return PogoIslandRoute();yield return JumpTo(20.8f,24.7f,1,"A04 island exit");yield return GoX(27.5f);yield return Until(()=>Boss.Fighting,"A04 actual fight",4);yield return Fight();break;
   case "A05":
    yield return B25Pose(id,new Vector2(46.5f,9.315f));target="A03";yield return Until(()=>Motor.Grounded,"A05 entry landing",4);
    yield return JumpTo(30.6f,24.5f,1,"A05 sealed floor",true);yield return JumpTo(24.5f,18,1,"A05 E2",true);yield return GoX(15.5f);yield return JumpTo(12.3f,4.6f,1,"A05 trench",true);break;
   case "A06":
    yield return B25Pose(id,new Vector2(36.85f,1.015f));Motor.hasDoubleJump=true;target="A02";
    yield return JumpTo(36.85f,39,3,"A06 block");yield return JumpTo(39.1f,42.7f,5,"A06 exit platform");break;
   case "B01":
    yield return B25Pose(id,new Vector2(5,1.015f));target="B02";
    yield return JumpTo(12.5f,16.3f,3,"B01 market");yield return GoX(31.8f);yield return JumpTo(31.8f,34.5f,3,"B01 crate");yield return JumpTo(35.5f,38.5f,5,"B01 roof");break;
   case "B02":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));target="B03";
    yield return B25Jet(11.7f,8.5f,15,8,"B02 upper");yield return GoX(31);yield return JumpTo(31.5f,41.2f,1,"B02 upper departure",true);yield return JumpTo(43.5f,50.5f,1,"B02 E1",true);break;
   case "B03":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));target="B04";yield return B25Jet(13.6f,8.5f,10,8,"B03 lower");yield return B25Jet(7.7f,17.5f,4.3f,17,"B03 upper");break;
   case "B04":
    yield return B25Pose(id,new Vector2(20.5f,1.015f));target="B03";
    yield return JumpTo(14.5f,12,2,"B04 wall pickup");yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B04 departure",3);yield return WalkTo(()=>Body.position.x<=2.5f,Key.LeftArrow,"B04 wall approach",4);yield return Until(()=>!Boss.Introducing,"B04 intro",3);yield return B25FightInput();break;
   case "B05":
    yield return B25Pose(id,new Vector2(18,1.015f));Motor.hasWallJump=true;target="B01";break;
   case "B06":
    yield return B25Pose(id,new Vector2(3.5f,1.015f));target="B03";
    yield return B25Pogo(7.7f,new[]{10.5f,15f},new[]{2.8f,2.8f},19.5f,5,true,"B06 challenge");
    yield return B25Pogo(20.2f,new[]{23f},new[]{1.6f},26.8f,4,false,"B06 spike");yield return GoX(32);break;
   default:throw new ArgumentException("Unknown route room "+id);
  }
  Key direction=id=="A05"||id=="B03"||id=="B06"?Key.LeftArrow:Key.RightArrow;
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId==target,direction,id+" physical main exit to "+target,14);yield return Ready();
  Record($"MEASURE roomRoute={id} target={target} physicalExit=True seconds={Time.timeAsDouble-began:F3} HP={Health.HP}");
 }
}
#endif
