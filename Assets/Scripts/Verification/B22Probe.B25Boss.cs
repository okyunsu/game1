#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B25BossField()
 {
  yield return B25Pose("B01",new Vector2(22,1.015f));Check(RoomSession.Instance.CheckpointId=="CP-B01","actual B01 checkpoint active");
  yield return RoomAt("B04");Check(!Motor.hasWallJump&&!Boss.Fighting&&Boss.transform.position.y>12,"B04 right entrance; boss waits at ceiling; pickup required");
  yield return JumpTo(14.5f,12,2,"B04 actual wall pickup");yield return Until(()=>Motor.hasWallJump,"B04 wall pickup",2);
  Check(!Boss.Fighting&&ProgressSave.TryLoad(out var pickup)&&pickup.wallJump,"B04 wall ownership saved before combat");
  yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B04 depart by3",3);
  Check(Boss.Introducing&&Boss.encounterGate.activeSelf&&!Boss.leftExit.enabled,"B04 departure locks right entry");double intro=Boss.PhaseStarted;
  yield return Until(()=>Boss.PatternStarts>0,"B04 first pattern",3);Record($"MEASURE B04 intro={Time.timeAsDouble-intro:F3} pedestalTop={Boss.sinkingPedestal.GetComponent<Collider2D>().bounds.max.y:F3} bossFeet={Boss.GetComponent<Collider2D>().bounds.min.y:F3}");
  Check(Math.Abs(Time.timeAsDouble-intro-1.8)<.05&&Math.Abs(Boss.sinkingPedestal.GetComponent<Collider2D>().bounds.max.y-1)<.001&&Math.Abs(Boss.GetComponent<Collider2D>().bounds.min.y-1)<.001,"B04 intro1.8 and floor landing/sink");
  double tell=Boss.PhaseStarted;yield return Until(()=>Boss.WavesSpawned>=2,"B04 wave",2);yield return new WaitForFixedUpdate();
  var waves=FindObjectsByType<BossWave>(FindObjectsSortMode.None).Where(w=>w.candidateOwner==Boss).ToArray();Record($"MEASURE B04 firstTell={Time.timeAsDouble-tell:F3} heights={string.Join(",",waves.Select(w=>w.GetComponent<Collider2D>().bounds.size.y.ToString("F3")))}");Check(waves.Length==2&&waves.All(w=>Math.Abs(w.GetComponent<Collider2D>().bounds.size.y-2.8)<.001),"B04 bidirectional height2.8");
  Check(Health.TakeDamage(Health.HP,Body.position),"B04 lethal checkpoint fixture after pickup");yield return Until(()=>RoomSession.Instance.Respawning,"B04 death queued",3);yield return Ready();
  Check(RoomSession.Instance.CurrentRoomId=="B01"&&RoomSession.Instance.CheckpointId=="CP-B01"&&Motor.hasWallJump,"B04 death returns actual B01 and keeps wall jump");
  yield return RoomAt("B04");Check(Boss.HP==26&&!Boss.Fighting&&FindFirstObjectByType<WallJumpPickup>()==null,"B04 retry fresh boss/empty pickup/idle right entry");
  yield return GoX(14.6f);Input(Key.LeftArrow,Key.Z);yield return Until(()=>Boss.Fighting,"B04 owned centre crossing",3);yield return Keys();
  yield return WalkTo(()=>Body.position.x<=2.5f,Key.LeftArrow,"B04 retry wall approach",4);yield return Until(()=>!Boss.Introducing,"B04 retry intro",3);
  yield return B25FightInput();
  Check(ProgressSave.TryLoad(out var saved)&&saved.wallJump&&saved.defeatedBossIds.Contains("B2-B04"),"B04 actual defeat and wall jump saved");Check(!Boss.encounterGate.activeSelf&&Boss.leftExit.enabled,"B04 victory opens right entrance");
  yield return RoomAt("B01");yield return RoomAt("B04");Check(Boss==null,"B04 defeated boss absent on reentry");
 }
 IEnumerator B25FightInput()
 {
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+150,nextAttack=0,nextDash=0,holdJump=0,nextJump=0;int walls=Motor.WallJumpsPerformed,pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;bool chosen=false;float safe=0;int lastHP=Health.HP;
  Record("FIXTURE fight starts with actual checkpoint HP and acquired wall jump; no HP/pose/boss/ownership writes during fight");
  while(Boss!=null&&Boss.HP>0&&Health.HP>0&&!RoomSession.Instance.Respawning&&Time.realtimeSinceStartupAsDouble<end)
  {
   double now=Time.timeAsDouble;var boss=Boss;var keys=new List<Key>();float px=Body.position.x,bx=boss.transform.position.x,feet=Player.GetComponent<Collider2D>().bounds.min.y,dx=bx-px;
   bool waves=boss.State==BossEncounter.Phase.WaveTell||boss.State==BossEncounter.Phase.WaveRecovery||FindObjectsByType<BossWave>(FindObjectsSortMode.None).Length>0;
   if(Health.HP!=lastHP){Record($"MEASURE B2 damage hp={Health.HP} bossHP={boss.HP} state={boss.State} pos={Body.position:F2}");lastHP=Health.HP;}
   float destination=waves?(px<12?2.4f:21.6f):bx;
   bool falling=boss.State==BossEncounter.Phase.FallingZones||boss.State==BossEncounter.Phase.FallingDrop;
   if(falling&&!chosen){chosen=true;safe=Mathf.Clamp(px+(px<12?1.5f:-1.5f),2.4f,21.6f);}if(!falling)chosen=false;if(falling)destination=safe;
   bool charge=boss.State==BossEncounter.Phase.ChargeTell||boss.State==BossEncounter.Phase.Charge;bool dodgeCharge=charge&&(boss.State==BossEncounter.Phase.Charge?Math.Abs(dx)/boss.tuning.chargeSpeed<.5:now-boss.PhaseStarted>=boss.tuning.chargeTell+Mathf.Max(0,Math.Abs(dx)-1.35f)/boss.tuning.chargeSpeed-.36);if(charge)destination=px;
   if(Math.Abs(destination-px)>.12f)keys.Add(destination>px?Key.RightArrow:Key.LeftArrow);
   if(waves){keys.Add(px<12?Key.LeftArrow:Key.RightArrow);if(now>=nextJump){holdJump=now+.3;nextJump=now+.5;}}
   else if(Motor.Grounded&&(!charge&&Math.Abs(dx)<4||dodgeCharge)&&now>=nextJump){holdJump=now+.43;nextJump=now+.65;}
   if(now<holdJump)keys.Add(Key.Z);
   bool down=!Motor.Grounded&&Body.linearVelocity.y<0;if(down)keys.Add(Key.DownArrow);
   bool attack=now>=nextAttack&&Math.Abs(dx)<2.6f&&(Motor.Grounded||down&&feet<=boss.GetComponent<Collider2D>().bounds.max.y+2.5f);
   if(attack){keys.Add(Key.X);nextAttack=now+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03;}
   if(!waves&&!falling&&!Motor.Grounded&&feet>5&&Math.Abs(dx)>5&&now>=nextDash){keys.Add(Key.LeftShift);nextDash=now+.55;}
   Input(keys.ToArray());yield return null;
  }
  yield return Keys();Record($"MEASURE B2 fight seconds={Time.timeAsDouble-began:F3} defeated={Boss!=null&&Boss.HP==0} bossHP={Boss?.HP} HP={Health.HP} wallJumps={Motor.WallJumpsPerformed-walls} pogos={Player.GetComponent<PlayerAttack>().PogosPerformed-pogos}");
  Check(Boss!=null&&Boss.HP==0&&Motor.WallJumpsPerformed>walls,"actual wall-jump input defeats B2");Check(ProgressSave.TryLoad(out var saved)&&saved.wallJump&&saved.defeatedBossIds.Contains("B2-B04"),"B2 ID and acquired wall jump saved");
 }
}
#endif
