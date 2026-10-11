using UnityEngine;
public sealed class SlamPad:MonoBehaviour
{
 public MechanicsTuning tuning;
 public int Bounces{get;private set;}
 public void Bounce(PlayerMotor motor){Bounces++;motor.ApplyPogo(Mathf.Sqrt(2*motor.Tuning.gravity*tuning.padHeight)+motor.Tuning.gravity*Time.fixedDeltaTime*.5f,true);motor.GetComponent<PlayerDash>().RefreshAirFromPogo();}
}
