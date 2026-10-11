using UnityEngine;
public sealed class LiftLever:MonoBehaviour
{
 public LeverLift lift;
 public bool upper;
 uint lastSequence; public void ReceiveAttack(uint sequence){if(lastSequence==sequence)return;lastSequence=sequence;lift.Call(upper);}
}

