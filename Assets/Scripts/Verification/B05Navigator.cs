#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
// Opt-in route producer state. It never changes gameplay values or places the player.
public sealed class B05Navigator
{
 int stage,jumpStart=-100; bool jumping;
 public N5InputTick Generate(PlayerInputReader player,string goal,int tick)
 {
  var body=player.GetComponent<Rigidbody2D>();var motor=player.GetComponent<PlayerMotor>();string id=RoomSession.Instance.CurrentRoomId;float x=body.position.x;
  if(id=="B05"&&goal=="C01"){
   Vector2[] points={new(10,1.81f),new(16,4.61f),new(22.5f,4.61f),new(28,7.31f),new(34.5f,7.31f),new(41,9.81f),new(49.5f,9.81f),new(54,10.41f),new(58.25f,10.41f)};
   var target=points[stage];if(stage==0&&Mathf.Abs(x-target.x)<.15f&&motor.Grounded){stage++;target=points[stage];}
   else if(stage>0&&stage<points.Length-1&&motor.Grounded&&Mathf.Abs(x-target.x)<.3f&&Mathf.Abs(body.position.y-target.y)<.3f){stage++;jumping=false;target=points[stage];}
   if(jumping&&motor.Grounded&&tick>jumpStart+45)jumping=false;
   if(stage>0&&!jumping&&motor.Grounded){jumping=true;jumpStart=tick;}
   int dt=tick-jumpStart;int held=stage%2==0?0:dt>=0&&(dt<13||dt>=16&&dt<38)?2:0;
   return new N5InputTick{axis=Mathf.Abs(x-target.x)<.06f?0:Mathf.Sign(target.x-x),held=held};
  }
  float destination=0;foreach(var exit in Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))if(exit.destinationRoomId==goal)destination=exit.transform.position.x;
  if(goal=="DoubleJump")destination=Object.FindFirstObjectByType<DoubleJumpPickup>().transform.position.x;
  if(id=="B01"&&goal=="B05"){
   if(stage==0){if(Mathf.Abs(x-69.5f)<.15f&&motor.Grounded)stage=1;else return new N5InputTick{axis=Mathf.Sign(69.5f-x),held=x<35&&tick%50<26?2:0};}
   if(jumping&&motor.Grounded&&tick>jumpStart+45)jumping=false;if(!jumping&&motor.Grounded){jumping=true;jumpStart=tick;}
   int dt=tick-jumpStart;return new N5InputTick{axis=1,held=dt<13||dt>=16&&dt<38?2:0};
  }
  bool jump=tick%60<14||motor.hasDoubleJump&&tick%60>=17&&tick%60<36;
  return new N5InputTick{axis=Mathf.Abs(x-destination)<.06f?0:Mathf.Sign(destination-x),held=(jump?2:0)|(tick%22==0?8:0)};
 }
}

#endif
