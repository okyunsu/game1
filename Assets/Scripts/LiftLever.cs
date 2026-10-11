using UnityEngine;
public sealed class LiftLever:MonoBehaviour,IDamageable
{
 public LeverLift lift;
 public bool upper;
 public void ReceiveHit(int damage,Vector2 source){lift.Call(upper);}
}
