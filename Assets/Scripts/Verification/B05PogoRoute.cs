using UnityEngine;
// B06's existing lethal pogo targets require deliberate downward attacks.
public sealed class B05PogoRoute
{
 readonly PlayerInputReader player;readonly PlayerMotor motor;readonly PlayerAttack attack;readonly Rigidbody2D body;readonly int before;int stage,jumped=-100,firstHit,dashed=-100;bool usedDash;
 public bool Done=>stage==4;
 public B05PogoRoute(PlayerInputReader p){player=p;motor=p.GetComponent<PlayerMotor>();attack=p.GetComponent<PlayerAttack>();body=p.GetComponent<Rigidbody2D>();before=attack.PogosPerformed;}
 public N5InputTick Generate(int tick){float x=body.position.x;
  if(stage==0){if(Mathf.Abs(x-8.5f)<.15f&&motor.Grounded){stage=1;jumped=tick;}else return new N5InputTick{axis=Mathf.Sign(8.5f-x)};}
  if(stage==1){if(attack.PogosPerformed>before){stage=2;firstHit=tick;}else return new N5InputTick{axis=1,held=(tick-jumped<26?2:0)|32|(tick-jumped==18?8:0)};}
  if(stage==2){if(!usedDash&&x>=12.4f){usedDash=true;dashed=tick;return new N5InputTick{axis=1,held=4|32};}if(usedDash&&tick-dashed>=13){stage=3;return new N5InputTick{axis=1,held=32|8};}return new N5InputTick{axis=1,held=32};}
  if(stage==3){if(attack.PogosPerformed>=before+2&&x>20&&motor.Grounded){stage=4;return new N5InputTick();}return new N5InputTick{axis=1,held=32|(tick-firstHit>=19&&(tick-firstHit)%20==0?8:0)};}
  return new N5InputTick();
 }
}
