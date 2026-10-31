#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B24BossSmoke(){yield return new WaitForSeconds(2);Check(RoomSession.Instance.CurrentRoomId=="B04"&&Boss!=null&&Boss.HP==26&&!Boss.Fighting,"B2 launcher scene HP26 idle");Record("MEASURE B2 launcher wall="+Motor.hasWallJump);}
 IEnumerator B2Intro()
 {
  Check(RoomSession.Instance.CurrentRoomId=="B04"&&Boss.bossId=="B2-B04","B2 authored trial");Check(!Motor.hasWallJump&&!Boss.Fighting,"pickup required before combat");
  yield return JumpTo(9.5f,12,2,"B2 acquisition pedestal");yield return Until(()=>Motor.hasWallJump,"actual wall jump pickup",2);
  Check(!Boss.Fighting&&ProgressSave.TryLoad(out var save)&&save.wallJump,"acquisition saves immediately, no combat at pedestal");
  yield return WalkTo(()=>Body.position.x<=8.8f,Key.LeftArrow,"depart pedestal",3);Check(Boss.Introducing&&Boss.encounterGate.activeSelf&&!Boss.leftExit.enabled,"departure3 closes entrance and starts arrival");double began=Boss.PhaseStarted;
  yield return WalkTo(()=>Body.position.x<=1.5f,Key.LeftArrow,"move to wall during introduction",3);yield return Until(()=>Boss.PatternStarts>0,"B2 first pattern",3);
  Record($"MEASURE B2 introduction seconds={Time.timeAsDouble-began:F3}");Check(Math.Abs(Time.timeAsDouble-began-1.8)<.05,"B2 introduction 1.8s");
  double tell=Boss.PhaseStarted;yield return Until(()=>Boss.WavesSpawned>=2,"first bidirectional wave",2);double firstTell=Time.timeAsDouble-tell;yield return new WaitForFixedUpdate();var waves=FindObjectsByType<BossWave>(FindObjectsSortMode.None).Where(w=>w.candidateOwner==Boss).ToArray();
  Record($"MEASURE B2 first tell={firstTell:F3} height={waves.First().GetComponent<Collider2D>().bounds.size.y:F3} directions={string.Join(",",waves.Select(w=>w.direction).OrderBy(v=>v))}");
  Check(Math.Abs(firstTell-1)<.05&&waves.Length==2&&waves.Any(w=>w.direction<0)&&waves.Any(w=>w.direction>0)&&waves.All(w=>Math.Abs(w.GetComponent<Collider2D>().bounds.size.y-2.8)<.001),"first tell1.0 and two waves height2.8");
 }
 void ForceB2Wave(){foreach(var wave in FindObjectsByType<BossWave>(FindObjectsSortMode.None))Destroy(wave.gameObject);typeof(BossEncounter).GetMethod("BeginPattern",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(Boss,new object[]{2});}
 bool waveHit;
 IEnumerator B2JumpWave(float x,float feet,string label)
 {
  yield return Keys();Health.RestoreMax();Player.ClearTransientInput();PlaceForAuthorizedSetup(new Vector2(x,feet+.015f));yield return new WaitForSeconds(.2f);ForceB2Wave();yield return null;
  yield return Until(()=>FindObjectsByType<BossWave>(FindObjectsSortMode.None).Any(w=>w.direction<0),"wave fixture spawned",2);int hp=Health.HP;bool jumped=false;double start=0,end=Time.realtimeSinceStartupAsDouble+4;float peak=Body.position.y;
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   var wave=FindObjectsByType<BossWave>(FindObjectsSortMode.None).FirstOrDefault(w=>w.direction<0);if(wave==null)break;
   float eta=(wave.GetComponent<Collider2D>().bounds.min.x-Player.GetComponent<Collider2D>().bounds.max.x)/wave.speed;
   if(!jumped&&eta<=.4f){jumped=true;start=Time.timeAsDouble;}Input(jumped&&Time.timeAsDouble-start<.43?new[]{Key.Z}:Array.Empty<Key>());peak=Math.Max(peak,Body.position.y);
   if(Health.HP<hp)break;if(jumped&&eta<-.2)break;yield return null;
  }
  yield return Keys();waveHit=Health.HP<hp;Record($"MEASURE B2 {label} noWallJump={!Motor.hasWallJump} jumped={jumped} hit={waveHit} damage={hp-Health.HP} peakFeet={peak:F3}");
 }
 IEnumerator B24BossPatterns()
 {
  yield return B2Intro();var original=Health.tuning;Health.tuning=Instantiate(original);Health.tuning.playerMaxHP=100;Health.RestoreMax();Record("FIXTURE durability100 for pattern observation only, not a victory");
  var order=new List<int>{Boss.ActivePattern};int count=Boss.PatternStarts;double end=Time.realtimeSinceStartupAsDouble+45;
  while(order.Distinct().Count()<3&&Time.realtimeSinceStartupAsDouble<end){if(Boss.PatternStarts!=count){count=Boss.PatternStarts;order.Add(Boss.ActivePattern);}yield return null;}
  Record("MEASURE B2 observed="+string.Join(",",order)+" phaseTransitions="+Boss.PhaseTransitions);Check(order.Take(3).SequenceEqual(new[]{2,3,7})&&Boss.PhaseTransitions==0,"only wave charge falling, ordered, one phase");
  Motor.hasWallJump=Motor.hasDoubleJump=false;Player.GetComponent<PlayerDash>().hasDash=false;int hits=0;for(int i=1;i<=10;i++){yield return B2JumpWave(5,1,"floor trial="+i);if(waveHit)hits++;}Check(hits==10,"no wall jump floor test hit10/10");
  yield return B2JumpWave(12,2,"pedestal counterexample");Record("MEASURE B2 pedestal ordinary jump bypass="+!waveHit+"; floor ten-hit evidence is not universal ability-gate proof");
  Health.tuning=original;Health.RestoreMax();
 }
 IEnumerator B24BossFight()
 {
  yield return B2Intro();Check(Health.TakeDamage(Health.HP,Body.position),"lethal checkpoint fixture after real pickup");yield return Until(()=>RoomSession.Instance.Respawning,"B2 death",3);yield return Ready();
  Check(RoomSession.Instance.CheckpointId=="CP-B01"&&Motor.hasWallJump&&Boss.HP==26&&Boss.PatternStarts==0&&!Boss.Fighting,"checkpoint retry keeps wall jump and resets boss");Check(FindFirstObjectByType<WallJumpPickup>()==null,"retry pedestal is empty");
  yield return JumpTo(9.5f,12,2,"B2 retry centre");yield return WalkTo(()=>Boss.Fighting,Key.RightArrow,"owned reentry centre trigger",2);yield return WalkTo(()=>Body.position.x<=1.5f,Key.LeftArrow,"retry wall approach",4);yield return Until(()=>!Boss.Introducing,"retry intro finishes",3);
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+150,nextAttack=0,nextDash=0,holdJump=0,nextJump=0;int walls=Motor.WallJumpsPerformed,pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;bool chosen=false;float safe=0;int lastHP=Health.HP;
  Record("FIXTURE fight starts with actual checkpoint HP and acquired wall jump; no HP/pose/boss/ownership writes during fight");
  while(Boss!=null&&Boss.HP>0&&Health.HP>0&&!RoomSession.Instance.Respawning&&Time.realtimeSinceStartupAsDouble<end)
  {
   double now=Time.timeAsDouble;var boss=Boss;var keys=new List<Key>();float px=Body.position.x,bx=boss.transform.position.x,feet=Player.GetComponent<Collider2D>().bounds.min.y,dx=bx-px;
   bool waves=boss.State==BossEncounter.Phase.WaveTell||boss.State==BossEncounter.Phase.WaveRecovery||FindObjectsByType<BossWave>(FindObjectsSortMode.None).Length>0;
   if(Health.HP!=lastHP){Record($"MEASURE B2 damage hp={Health.HP} bossHP={boss.HP} state={boss.State} pos={Body.position:F2}");lastHP=Health.HP;}
   float destination=waves?(px<12?1.4f:22.6f):bx;
   bool falling=boss.State==BossEncounter.Phase.FallingZones||boss.State==BossEncounter.Phase.FallingDrop;
   if(falling&&!chosen){chosen=true;safe=Mathf.Clamp(px+(px<12?1.5f:-1.5f),1.4f,22.6f);}if(!falling)chosen=false;if(falling)destination=safe;
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
  Check(!Boss.encounterGate.activeSelf&&Boss.leftExit.enabled,"victory opens entrance");yield return RoomAt("B01","Assets/Scenes/V4/B01_V4.unity");yield return RoomAt("B04","Assets/Scenes/Test/BossTest_B2.unity");Check(Boss==null,"defeated B2 absent on reentry");
 }
}
#endif




