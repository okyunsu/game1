#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public sealed partial class B22Probe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B22Probe>("-verify-b22");
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 RoomDefinition Room=>FindFirstObjectByType<RoomDefinition>();
 B21HazardReturn Hazard=>Player.GetComponent<B21HazardReturn>();
 BossEncounter Boss=>FindFirstObjectByType<BossEncounter>();
 string BossPath=>N5Args.Has("-b22-authored")?V4SceneMap.Path("A04"):"Assets/Scenes/Test/BossTest_B1.unity";
 void Record(string s){lines.Add(s);Debug.Log(s);}
 IEnumerator RoomAt(string id,string path=null)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,path??V4SceneMap.Path(id),"Entry"),"fixture transition "+id);yield return Ready();}yield return new WaitForSeconds(.5f);
 }
 void NoAbilities(){Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;Player.GetComponent<PlayerSlam>().hasSlam=false;}
 void DisableEnemies(){foreach(var e in Room.GetComponentsInChildren<EnemyReset>())if(e.Live!=null)e.Live.SetActive(false);}
 IEnumerator ResetAt(Vector2 p)
 {
  yield return Keys();if(Hazard!=null)yield return Until(()=>!Hazard.Returning,"hazard return finished",2);Health.RestoreMax();DisableEnemies();Motor.ClearTransientState();Player.GetComponent<PlayerDash>().Cancel();PlaceForAuthorizedSetup(p);yield return new WaitForSeconds(.35f);
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();
  string mode=N5Args.Value("-b22-mode","boss");
  if(mode=="b23-hazard")yield return B23Hazard();else if(mode=="b23-lift")yield return B23Lift();else if(mode=="b23-field")yield return B23Field();else if(mode=="b23-loop")yield return B23Loop();else if(mode=="boss")yield return BossTests();else if(mode=="a05")yield return Trench();else if(mode=="a04")yield return Island();else if(mode=="practice")yield return Practice();else if(mode=="hazard")yield return HazardPresence();else if(mode=="loop")yield return Loop();
 }
 IEnumerator HazardPresence()
 {
  yield return new WaitForSeconds(.3f);Check((Hazard!=null)==V4SceneMap.V5,"hazard component present only on V5 default launch");
  Record("MEASURE V5="+V4SceneMap.V5+" hazard="+(Hazard!=null)+" flag="+N5Args.Has("-hazard-return"));
  if(Hazard!=null){yield return RoomAt("A05");yield return ResetAt(new Vector2(13.5f,1.015f));Check(Hazard.HasSafePoint,"default V5 safe floor recorded");}
 }
 IEnumerator EnterBoss()
 {
  yield return RoomAt("A03");NoAbilities();var cp=Room.GetComponentsInChildren<Checkpoint>().Single(c=>c.checkpointId=="CP-A03");RoomSession.Instance.ActivateCheckpoint(cp);
  yield return RoomAt("A04",BossPath);yield return ResetAt(new Vector2(25.5f,1.015f));Check(Boss!=null&&Boss.HP==Boss.tuning.maxHP&&!Boss.Fighting,"B1 fresh configured HP idle safe landing");
 }
 IEnumerator Fight()
 {
  var boss=Boss;var attack=Player.GetComponent<PlayerAttack>();NoAbilities();int startPogos=attack.PogosPerformed;double began=Time.timeAsDouble,nextJump=0,holdJump=0,nextAttack=0;var patterns=new HashSet<int>();
  Record("FIXTURE fight uses only virtual movement/jump/X/down; no position, HP, boss state or tuning writes during combat");
  double deadline=Time.realtimeSinceStartupAsDouble+150;
  while(boss!=null&&boss.HP>0&&!RoomSession.Instance.Respawning&&Health.HP>0&&Time.realtimeSinceStartupAsDouble<deadline)
  {
   double now=Time.timeAsDouble;var keys=new List<Key>();float bx=boss.transform.position.x,px=Body.position.x,side=px<bx?-1:1;
   if(boss.PatternStarts>0)patterns.Add(boss.ActivePattern);
   float destination=bx+side*2.1f;bool dodgeJump=false;double elapsed=now-boss.PhaseStarted;
   if(boss.State==BossEncounter.Phase.LeapTell||boss.State==BossEncounter.Phase.LeapFlight){destination=boss.LeapTarget+(boss.LeapTarget<36.5f?6:-6);}
   if(boss.State==BossEncounter.Phase.ChargeTell){destination=px;float eta=Mathf.Max(0,Mathf.Abs(px-bx)-1.35f)/boss.tuning.chargeSpeed;dodgeJump=elapsed>=boss.tuning.chargeTell+eta-.36;}
   if(boss.State==BossEncounter.Phase.Charge){destination=px;dodgeJump=Mathf.Abs(px-bx)/boss.tuning.chargeSpeed<.5;}
   foreach(var wave in FindObjectsByType<BossWave>(FindObjectsSortMode.None)){float ahead=wave.direction*(px-wave.transform.position.x);if(ahead>0&&ahead/wave.speed<.4)dodgeJump=true;}
   float dx=destination-px;if(Mathf.Abs(dx)>.12f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(Motor.Grounded&&dodgeJump&&now>=nextJump){holdJump=now+.42;nextJump=now+.65;}
   if(now<holdJump)keys.Add(Key.Z);
   bool recovery=boss.State==BossEncounter.Phase.LeapRecovery||boss.State==BossEncounter.Phase.WaveRecovery||boss.State==BossEncounter.Phase.ChargeRecovery||boss.State==BossEncounter.Phase.Walk;
   if(!Motor.Grounded){keys.Add(Key.DownArrow);if(Body.linearVelocity.y<0&&Mathf.Abs(px-bx)<1.5f&&Player.GetComponent<Collider2D>().bounds.min.y<=boss.GetComponent<Collider2D>().bounds.max.y+1.1f&&now>=nextAttack){keys.Add(Key.X);nextAttack=now+attack.tuning.attackCooldown+.03;}}
   else if(recovery&&Mathf.Abs(px-bx)<2.7f&&now>=nextAttack){keys.Remove(Key.LeftArrow);keys.Remove(Key.RightArrow);keys.Add(px<bx?Key.RightArrow:Key.LeftArrow);keys.Add(Key.X);nextAttack=now+attack.tuning.attackCooldown+.03;}
   Input(keys.ToArray());yield return null;
  }
  yield return Keys();
  Record("MEASURE B1 fight seconds="+(Time.timeAsDouble-began).ToString("F3")+" defeated="+(boss!=null&&boss.HP==0)+" playerHP="+Health.HP+" pogos="+(attack.PogosPerformed-startPogos)+" patterns="+string.Join(",",patterns.OrderBy(i=>i))+" phaseTransitions="+(boss!=null?boss.PhaseTransitions:-1)+" body="+Player.GetComponent<Collider2D>().bounds.size.y.ToString("F2"));
  Check(boss!=null&&boss.HP==0,"actual inputs defeat B1 without dash/wall/double jump");Check(boss.PhaseTransitions==0,"B1 phase transitions=0");
 }
 IEnumerator BossTests()
 {
  yield return EnterBoss();var boss=Boss;
  Body.simulated=false;PlaceForAuthorizedSetup(new Vector2(28,7));yield return new WaitForSeconds(.15f);Check(!boss.Fighting&&!boss.encounterGate.activeSelf,"passing upper route does not trigger fight");Body.simulated=true;
  PlaceForAuthorizedSetup(new Vector2(27.5f,1.015f));yield return Until(()=>boss.State==BossEncounter.Phase.EntranceTell,"B1 arrival tell");double start=boss.PhaseStarted;
  Check(boss.encounterGate.activeSelf&&boss.floorCover.activeSelf,"entry closes gate, retains floor cover");
  int introHP=Health.HP,bossHP=boss.HP;uint jump=Player.JumpSequence,attacks=Player.GetComponent<PlayerAttack>().Sequence;
  if(boss.Introducing){boss.ReceiveHit(1,Body.position);Check(!Health.TakeDamage(1,boss.transform.position),"intro player damage rejected");Input(Key.RightArrow,Key.Z,Key.X,Key.LeftShift);yield return new WaitForSeconds(.3f);Check(Player.MovementOnly&&Player.JumpSequence==jump&&Player.GetComponent<PlayerAttack>().Sequence==attacks&&!Player.GetComponent<PlayerDash>().IsDashing,"intro only movement allowed");Check(Mathf.Abs(Body.linearVelocity.x)>0,"intro horizontal movement allowed");yield return Keys();}
  yield return Until(()=>boss.PatternStarts>0,"first pattern");float duration=boss.introDuration>0?boss.introDuration:boss.entranceSeconds;double actual=Time.timeAsDouble-start;Record("MEASURE B1 entrance seconds="+actual.ToString("F3")+" target="+duration+" damage="+(introHP-Health.HP)+" bossDamage="+(bossHP-boss.HP));Check(Math.Abs(actual-duration)<=.05&&Health.HP==introHP&&boss.HP==bossHP&&!Player.MovementOnly,"intro duration tolerance and no damage");
  // Observe all enabled patterns in a separate durability fixture, then reset by real death.
  var original=Health.tuning;Health.tuning=Instantiate(original);Health.tuning.playerMaxHP=100;Health.RestoreMax();var seen=new HashSet<int>();double until=Time.realtimeSinceStartupAsDouble+40;
  while(seen.Count<3&&Time.realtimeSinceStartupAsDouble<until){seen.Add(boss.ActivePattern);yield return null;}
  Record("MEASURE B1 observed="+string.Join(",",seen.OrderBy(i=>i))+" phaseTransitions="+boss.PhaseTransitions+" fixtureHP100=True");Check(seen.SetEquals(new[]{1,2,3})&&boss.PhaseTransitions==0,"only leap/wave/charge observed, one phase");
  yield return Until(()=>boss.State==BossEncounter.Phase.WaveRecovery||boss.State==BossEncounter.Phase.ChargeRecovery,"pogo recovery fixture",20);
  int pogoBefore=Player.GetComponent<PlayerAttack>().PogosPerformed;PlaceForAuthorizedSetup(new Vector2(boss.transform.position.x,boss.GetComponent<Collider2D>().bounds.max.y+.6f));Motor.ClearTransientState();yield return Until(()=>!Motor.Grounded,"airborne pogo fixture",1);yield return Keys(Key.DownArrow,Key.X);yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>pogoBefore,"top pogo bounce",1);
  Record("MEASURE B1 topPogo="+(Player.GetComponent<PlayerAttack>().PogosPerformed-pogoBefore)+" velocityY="+Body.linearVelocity.y.ToString("F3")+" fixture=airborne-recovery");yield return Keys();
  Health.tuning=original;Health.RestoreMax();Check(Health.TakeDamage(Health.HP,Body.position),"lethal retry fixture");yield return Until(()=>RoomSession.Instance.Respawning,"death queued");yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="A03"&&RoomSession.Instance.CheckpointId=="CP-A03","death returns actual CP-A03");
  yield return EnterBoss();Check(Boss.HP==Boss.tuning.maxHP&&Boss.PatternStarts==0,"retry resets B1 HP and patterns");
  yield return WalkTo(()=>Body.position.x>=27.5f,Key.RightArrow,"walk into B1");yield return Fight();
  Check(ProgressSave.TryLoad(out var save)&&save.defeatedBossIds.Contains("B1-A04")&&!save.bossDefeated&&!save.endingReached,"kill synchronously saves B1 only, no final ending");
  Check(!Boss.encounterGate.activeSelf&&!Boss.floorCover.activeSelf,"victory opens gate and removes cover");
  yield return RoomAt("A03");yield return RoomAt("A04",BossPath);Check(Boss==null,"reentry suppresses defeated B1");
  var gate=Room.GetComponentsInChildren<Transform>(true).Single(t=>t.name=="V5-A04-bossgate");var cover=Room.GetComponentsInChildren<Transform>(true).Single(t=>t.name=="V5-A04-bossfloor");Check(!gate.gameObject.activeSelf&&!cover.gameObject.activeSelf,"reentry has no gate or cover");
 }
 IEnumerator Trench()
 {
  yield return RoomAt("A05");NoAbilities();int fail=0,success=0,returned=0;
  foreach(bool dashOwned in new[]{false,true})for(int trial=1;trial<=10;trial++)
  {
   bool fromEast=trial%2==1;float direction=fromEast?-1:1,edge=fromEast?12:6;yield return ResetAt(new Vector2(fromEast?15.5f:2.5f,1.015f));Player.GetComponent<PlayerDash>().hasDash=dashOwned;
   int returns=Hazard.Returns;Key dir=fromEast?Key.LeftArrow:Key.RightArrow;Input(dir);yield return Until(()=>direction*(Body.position.x-edge)>=-.35f,"trench runup",2);Input(dir,Key.Z);
   bool launched=false,dashed=false,crossed=false;double end=Time.realtimeSinceStartupAsDouble+4;
   while(Time.realtimeSinceStartupAsDouble<end){launched|=Body.linearVelocity.y>0;if(dashOwned&&launched&&!dashed&&Body.linearVelocity.y<=.6f){Input(dir,Key.Z,Key.LeftShift);dashed=true;}else if(dashed)Input(dir,Key.Z);if(Motor.Grounded&&(fromEast?Body.position.x<5.7f:Body.position.x>12.3f)){crossed=true;break;}if(Hazard.Returns>returns)break;yield return null;}
   yield return Keys();yield return Until(()=>!Hazard.Returning,"trench return settled",2);bool correct=Hazard.Returns==returns+1&&(fromEast?Body.position.x>=12.5f:Body.position.x<=5.5f);
   if(!dashOwned&&!crossed)fail++;if(!dashOwned&&correct)returned++;if(dashOwned&&crossed)success++;
   Record("MEASURE A05 trial="+trial+" dash="+dashOwned+" side="+(fromEast?"east":"west")+" crossed="+crossed+" returnToEntry="+correct+" returns="+(Hazard.Returns-returns)+" end="+Body.position.ToString("F4"));
  }
  Record("MEASURE A05 summary noAbilityFailed="+fail+"/10 returned="+returned+"/10 dashPassed="+success+"/10");Check(fail==10&&returned==10&&success==10,"A05 no ability 0/10, entry side return, dash10/10");
 }
 IEnumerator Island()
 {
  yield return RoomAt("A04");NoAbilities();int direct=0,pogo=0,returnsOK=0;
  for(int trial=1;trial<=5;trial++)
  {
   yield return ResetAt(new Vector2(10.8f,1.015f));int returns=Hazard.Returns;Input(Key.RightArrow);yield return Until(()=>Body.position.x>=13.95f,"A04 runup",2);Input(Key.RightArrow,Key.Z);double end=Time.realtimeSinceStartupAsDouble+4;bool crossed=false;
   while(Time.realtimeSinceStartupAsDouble<end){if(Motor.Grounded&&Body.position.x>=19.3f&&Body.position.y>1.7f){crossed=true;break;}if(Hazard.Returns>returns)break;yield return null;}yield return Keys();yield return Until(()=>!Hazard.Returning,"A04 direct return",2);
   if(!crossed)direct++;if(Hazard.Returns>returns&&Body.position.x<14)returnOK();
   Record("MEASURE A04 direct trial="+trial+" island="+crossed+" returns="+(Hazard.Returns-returns)+" end="+Body.position.ToString("F4"));
  }
  for(int trial=1;trial<=5;trial++)
  {
   yield return ResetAt(new Vector2(10.8f,1.015f));var reward=Room.GetComponentsInChildren<DiscoveryReward>(true).Single();DiscoveryReward.ResetSession();reward.gameObject.SetActive(true);
   int before=Player.GetComponent<PlayerAttack>().PogosPerformed,returns=Hazard.Returns,hp=Health.HP;double began=Time.realtimeSinceStartupAsDouble,nextAttack=0;bool success=false;
   Input(Key.RightArrow);yield return Until(()=>Body.position.x>=13.9f,"A04 pogo runup",2);Input(Key.RightArrow,Key.Z);
   while(Time.realtimeSinceStartupAsDouble-began<12)
   {
    int target=Mathf.Min(2,Player.GetComponent<PlayerAttack>().PogosPerformed-before);float dx=(target==0?15.8f:target==1?17.8f:20.3f)-Body.position.x;var keys=new List<Key>{Key.Z,Key.DownArrow};if(Mathf.Abs(dx)>.15f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
    if(target<2){float x=target==0?15.8f:17.8f,top=target==0?2.8f:3.1f,allowance=target==0?.4f:.7f;if(Time.realtimeSinceStartupAsDouble>=nextAttack&&!Motor.Grounded&&Body.linearVelocity.y<0&&Mathf.Abs(Body.position.x-x)<.7f&&Player.GetComponent<Collider2D>().bounds.min.y<=top+allowance){keys.Add(Key.X);nextAttack=Time.realtimeSinceStartupAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03;}}
    Input(keys.ToArray());if(DiscoveryReward.Found(reward.candidateId)&&Motor.Grounded&&Body.position.x>=19.3f&&Body.position.x<=21.3f){success=true;break;}if(Hazard.Returns>returns)break;yield return null;
   }
   yield return Keys();yield return Until(()=>!Hazard.Returning,"pogo return settled",2);if(success)pogo++;
   Record("MEASURE A04 pogo trial="+trial+" success="+success+" pogos="+(Player.GetComponent<PlayerAttack>().PogosPerformed-before)+" hp="+hp+"->"+Health.HP+" returns="+(Hazard.Returns-returns)+" end="+Body.position.ToString("F4")+" enemiesOff="+Room.GetComponentsInChildren<EnemyReset>().All(e=>e.Live==null||!e.Live.activeInHierarchy));
  }
  Record("MEASURE A04 summary directFailed="+direct+"/5 leftReturn="+returnsOK+"/5 pogoSuccess="+pogo+"/5");Check(direct==5&&returnsOK==5&&pogo==5,"A04 direct0/5 left return5/5 pogo reward5/5");
  void returnOK(){returnsOK++;}
 }
 bool sampleSuccess;
 IEnumerator PracticeSample(int stage,bool owned,int trial)
 {
  // Authored platforms; only initial pose and ability ownership are fixture setup.
  float[] edge={17.8f,26.3f,30.8f},start={16.4f,23.9f,32.8f},y={11f,11,13},dest={23.3f,30.8f,26.3f},top={11f,13,15};int direction=stage==2?-1:1;
  if(N5Args.Has("-b23")){edge=new[]{43.5f,35f,26.5f};start=new[]{45.4f,37f,28.5f};y=new[]{13f,13,13};dest=new[]{38f,29.5f,22f};top=new[]{13f,13,15};direction=-1;}
  yield return ResetAt(new Vector2(start[stage],y[stage]+.015f));NoAbilities();Player.GetComponent<PlayerDash>().hasDash=owned;Key dir=direction>0?Key.RightArrow:Key.LeftArrow;
  Input(dir);yield return Until(()=>direction*(Body.position.x-edge[stage])>=-.25f,"practice runup",2);Input(dir,Key.Z);bool launched=false,dashed=false;sampleSuccess=false;double end=Time.realtimeSinceStartupAsDouble+3;
  while(Time.realtimeSinceStartupAsDouble<end){launched|=Body.linearVelocity.y>0;if(owned&&launched&&!dashed&&Body.linearVelocity.y<=.6f){Input(dir,Key.Z,Key.LeftShift);dashed=true;}else if(dashed)Input(dir,Key.Z);if(Motor.Grounded&&direction*(Body.position.x-dest[stage])>=.05f&&Body.position.y>=top[stage]-.1f){sampleSuccess=true;break;}if(Body.position.y<y[stage]-1)break;yield return null;}
  yield return Keys();Record("MEASURE A03 practice stage="+(stage+1)+" trial="+trial+" dash="+owned+" success="+sampleSuccess+" end="+Body.position.ToString("F4"));
 }
 IEnumerator Practice()
 {
  yield return RoomAt("A03");bool pass=true;
  if(N5Args.Has("-b23")){var lift=Room.GetComponentsInChildren<LeverLift>().Single();lift.Call(false);yield return Until(()=>Math.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.high)<.02f,"practice lift fixture",12);}
  for(int stage=0;stage<3;stage++){int no=0,yes=0;foreach(bool owned in new[]{false,true})for(int trial=1;trial<=5;trial++){yield return PracticeSample(stage,owned,trial);if(sampleSuccess){if(owned)yes++;else no++;}}Record("MEASURE A03 summary stage="+(stage+1)+" noAbility="+no+"/5 dash="+yes+"/5");pass&=no==0&&yes==5;}
  Check(pass,"A03 each noAbility0/5 dash5/5");
 }
 IEnumerator GoX(float x,float seconds=8,bool stop=true)
 {
  double end=Time.realtimeSinceStartupAsDouble+seconds,next=0;
  while(Mathf.Abs(Body.position.x-x)>.12f){if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("GoX timeout target="+x+" actual="+Body.position);var keys=new List<Key>{Body.position.x<x?Key.RightArrow:Key.LeftArrow};if(Time.timeAsDouble>=next){keys.Add(Key.X);next=Time.timeAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.04;}Input(keys.ToArray());yield return null;}
  if(stop)yield return Keys();
 }
 IEnumerator JumpTo(float takeoff,float x,float top,string label,bool dash=false)
 {
  bool running=label=="upper village";yield return GoX(takeoff,8,!running);yield return Until(()=>Motor.Grounded,"takeoff supported "+label,3);if(!running)yield return Keys();
  double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+4;bool launched=false,dashed=false,landed=false;
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   var keys=new List<Key>();float dx=x-Body.position.x;if(Mathf.Abs(dx)>.12f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(Time.timeAsDouble-began<.45)keys.Add(Key.Z);
   if((Time.timeAsDouble-began)%.4<.04)keys.Add(Key.X);
   launched|=Body.linearVelocity.y>0;
   if(dash&&launched&&!dashed&&Body.linearVelocity.y<=.6f){keys.Add(Key.LeftShift);dashed=true;}
   Input(keys.ToArray());
   if(launched&&Motor.Grounded&&Mathf.Abs(Body.position.x-x)<.35f&&Mathf.Abs(Body.position.y-top)<.08f){landed=true;break;}
   if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
  }
  yield return Keys();Record("MEASURE loop jump="+label+" landed="+landed+" end="+Body.position.ToString("F4")+" hp="+Health.HP);Check(landed,"physical jump "+label);
 }
 IEnumerator Loop()
 {
  Check(RoomSession.Instance.CurrentRoomId=="A03","loop starts A03");NoAbilities();double began=Time.timeAsDouble;Record("FIXTURE loop starts at CP-A03 via explicit isolated launch; no teleport, HP, ability or boss writes after start");
  // East stairway to the A04 door.
  yield return JumpTo(28,25.7f,3,"hub block");yield return JumpTo(26.15f,28.7f,5,"east platform5");yield return JumpTo(32.65f,35.1f,7,"east platform7");yield return JumpTo(36,38.4f,9,"east platform9");yield return JumpTo(39.3f,41.6f,11,"east platform11");yield return JumpTo(42.5f,45,13,"A04 door shelf");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A04",Key.RightArrow,"physical A03-A04 door",5);yield return Ready();Check(RoomSession.Instance.CheckpointId=="CP-A03","hub checkpoint retained");
  // Upper route uses the actual general enemy and authored platforms.
  yield return JumpTo(4.2f,5.8f,3,"A04 block");yield return JumpTo(7.15f,9.1f,5,"A04 enemy platform");yield return JumpTo(11.35f,13.5f,7,"A04 upper step");yield return JumpTo(14.1f,16.3f,7,"A04 long upper route");yield return GoX(27.5f);yield return Until(()=>Boss!=null&&Boss.Fighting,"arena landing trigger",4);
  Check(Boss.encounterGate.activeSelf,"loop arena locked");yield return Fight();Check(!Boss.floorCover.activeSelf,"loop defeat removes floor cover");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A05",Key.RightArrow,"physical opened floor fall",6);yield return Ready();Check(!Player.GetComponent<PlayerDash>().hasDash,"A05 reached before dash acquisition");
  yield return JumpTo(40,37.5f,2.6f,"A05 dash shrine");yield return Until(()=>Player.GetComponent<PlayerDash>().hasDash,"actual DashPickup",3);Record("PASS loop dash acquired in A05");
  yield return JumpTo(35.35f,31.8f,1,"A05 shallow pit");yield return GoX(15.5f);yield return Until(()=>Motor.Grounded,"trench runup ground",3);
  // The actual six-unit trench, without resetting pose or health.
  yield return JumpTo(12.3f,4.6f,1,"A05 trench dash",true);
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="A03",Key.LeftArrow,"physical A05-A03 door",5);yield return Ready();Record("PASS loop back in A03 with dash");
  yield return JumpTo(38.8f,35.5f,1,"A03 eastern shallow pit");yield return GoX(27.9f);yield return JumpTo(27.9f,25.5f,3,"return hub block");yield return JumpTo(24.3f,20,1,"return west floor");yield return JumpTo(20.1f,18.8f,3,"west block");yield return JumpTo(17.3f,14.9f,5,"west platform5");yield return JumpTo(15.7f,17.5f,7,"west platform7");yield return JumpTo(16.85f,14.4f,9,"upper village");yield return JumpTo(14.7f,16.5f,11,"S0");
  yield return JumpTo(17.55f,24,11,"practice1 gap5.5",true);yield return JumpTo(26.05f,32.5f,13,"practice2 rise2 gap4.5",true);yield return JumpTo(31.05f,24.5f,15,"practice3 dash-door shelf",true);
  Check(ProgressSave.TryLoad(out var saved)&&saved.dash&&saved.defeatedBossIds.Contains("B1-A04"),"loop save contains acquired dash and B1");yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A03"&&Player.GetComponent<PlayerDash>().hasDash&&ProgressSave.IsBossDefeated("B1-A04"),"loop Continue restores CP-A03 dash and B1");
  Record("MEASURE loop seconds="+(Time.timeAsDouble-began).ToString("F3")+" body="+Player.GetComponent<Collider2D>().bounds.size.y.ToString("F2")+" complete=True");
 }
}
#endif
