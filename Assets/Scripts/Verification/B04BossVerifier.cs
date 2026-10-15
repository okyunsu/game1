#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
// B04 approach 1: physics-tick input, distance-based collision ETA and sustained double jump.
// Prior B03 approach used a render-frame single-tick second press and cut the second jump.
public sealed class B04BossVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B04BossVerifier>("-verify-b04-boss");
 BossTuning authoredB3;BossEncounter boss;N9PogoInput driver;int hits;double jumped=-100;bool second;float peak;
 void Hit(){hits++;lines.Add($"HIT {boss?.State} player={Body.position} boss={boss?.transform.position}");}
 void Place(Vector2 p){Player.ClearTransientInput();Body.position=p;Player.transform.position=p;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Arena(){driver.produce=null;yield return Keys();if(RoomSession.Instance.CurrentRoomId!="C04"){Check(RoomSession.Instance.RequestTransition("C04","Assets/Scenes/C04.unity","CP-C04"),"fixture C04");yield return Ready();}Check(RoomSession.Instance.RequestTransition("C06","Assets/Scenes/C06.unity","FromLeft"),"fresh arena");yield return Ready();boss=FindFirstObjectByType<BossEncounter>();
 #if UNITY_EDITOR
 boss.tuning=UnityEditor.AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/"+N5Args.Value("-b03-candidate","B2")+"/BossTuning.asset");
 #endif
 Check(boss.tuning.extendedPatterns,"candidate active");Health.RestoreMax();Place(new Vector2(11,1.81f));yield return Until(()=>boss.Fighting,"fight");yield return new WaitForSeconds(.1f);hits=0;}
 void Pattern(int i){if(N5Args.Has("-b12-pattern"))foreach(var live in FindObjectsByType<BossB3Hazard>(FindObjectsSortMode.None))Destroy(live.gameObject);if(N5Args.Has("-b12-pattern"))foreach(var live in FindObjectsByType<BossWave>(FindObjectsSortMode.None))Destroy(live.gameObject);typeof(BossEncounter).GetMethod("BeginPattern",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(boss,new object[]{i});lines.Add("FIXTURE fresh isolated pattern="+i+"; no health/position writes during dodge outcome");}
 protected override void Cleanup(){if(driver!=null)driver.produce=null;if(Player!=null&&Health!=null)Health.Hit-=Hit;}
 protected override IEnumerator Tests(){if(N5Args.Has("-b15-soak")){yield return B15SoakTests();yield break;}if(N5Args.Has("-b14-defeat")){yield return B14DefeatTests();yield break;}if(N5Args.Has("-b12-p2")){yield return B12P2Tests();yield break;}if(N5Args.Has("-b12-cycle")){yield return B12CycleTests();yield break;}if(N5Args.Has("-b12-phase")){yield return B12PhaseTests();yield break;}if(N5Args.Has("-b12-pattern")){yield return B12Tests();yield break;}yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
  foreach(bool dodge in new[]{true,false}){yield return Arena();Pattern(3);jumped=-100;second=false;peak=Body.position.y;int dj=Motor.DoubleJumpsPerformed;
   driver.produce=()=>{peak=Mathf.Max(peak,Body.position.y);if(!dodge)return Array.Empty<Key>();float distance=Mathf.Abs(boss.transform.position.x-Body.position.x)-boss.GetComponent<BoxCollider2D>().bounds.extents.x-Player.GetComponent<BoxCollider2D>().bounds.extents.x;if(jumped<0&&boss.State==BossEncounter.Phase.Charge&&distance/boss.tuning.chargeSpeed<=.65f){jumped=Time.timeAsDouble;lines.Add($"INPUT ground jump collisionETA={distance/boss.tuning.chargeSpeed:F3}");}double t=Time.timeAsDouble-jumped;double delay=boss.tuning.chargeSpeed<8?.34:.24;if(jumped>=0&&(t<delay-.06||(t>=delay&&t<delay+.4))){if(t>=delay&&!second){second=true;lines.Add("INPUT sustained second jump .40s");}return new[]{Key.Z};}return Array.Empty<Key>();};
   yield return Until(()=>boss.State==BossEncounter.Phase.ChargeRecovery,"charge resolution",10);driver.produce=null;yield return Keys();lines.Add($"MEASURE fps={Application.targetFrameRate} candidate={N5Args.Value("-b03-candidate","B2")} charge dodge={dodge} hitEvents={hits} DJ={Motor.DoubleJumpsPerformed-dj} peak={peak:F3}");Check(hits==(dodge?0:1),"charge dodge0 / fail1");if(dodge)Check(Motor.DoubleJumpsPerformed>dj,"actual second jump consumed");}
  foreach(int i in new[]{1,3}){yield return Arena();Place(new Vector2(6,1.81f));Pattern(i);yield return Until(()=>boss.State==(i==1?BossEncounter.Phase.LeapFlight:BossEncounter.Phase.Charge),"active pattern");yield return Tap(Key.Escape);Check(GameState.GetOrCreate().Paused,"paused");Vector3 p=boss.transform.position;double time=Time.timeAsDouble;yield return new WaitForSecondsRealtime(.3f);Check(boss.transform.position==p&&Time.timeAsDouble==time,"pattern/clock frozen");yield return Tap(Key.Escape);Check(!GameState.GetOrCreate().Paused,"resumed");yield return null;Check(Time.timeAsDouble>time,"clock restarted");Check(Health.TakeDamage(Health.HP,Player.transform.position),"lethal setup");yield return Until(()=>RoomSession.Instance.Respawning,"death");yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04"&&Health.HP==Health.tuning.playerMaxHP,"CP-C04 HP restored");yield return Arena();Check(boss.HP==boss.tuning.maxHP&&boss.State==BossEncounter.Phase.MeleeTell,"fresh boss HP / pattern reset");}
 }
 // B12 uses isolated setup only. All dodge outcomes use physical virtual-key input.
 IEnumerator B12Arena(int index){
  driver.produce=null;yield return Keys();
  if(RoomSession.Instance.CurrentRoomId!="C04"){Check(RoomSession.Instance.RequestTransition("C04",V4SceneMap.Enabled?V4SceneMap.Path("C04"):"Assets/Scenes/C04.unity","CP-C04"),"fixture C04");yield return Ready();}
  string arena=V4SceneMap.Enabled?V4SceneMap.Path("C06"):"Assets/Scenes/C06.unity";
  yield return Until(()=>!UnityEngine.SceneManagement.SceneManager.GetSceneByPath(arena).isLoaded,"previous arena unloaded");
  Check(RoomSession.Instance.RequestTransition("C06",arena,V4SceneMap.Enabled?"C04-C06":"FromLeft"),"fresh B3 arena");yield return Ready();boss=FindFirstObjectByType<BossEncounter>();
  Check(boss.tuning.b3Patterns,"B3 serialized candidate active");
  // Runtime copy allows isolated switches without changing authored candidate.
  authoredB3=boss.tuning;boss.tuning=Instantiate(boss.tuning);boss.tuning.meleeEnabled=boss.tuning.leapEnabled=boss.tuning.waveEnabled=boss.tuning.chargeEnabled=false;
  boss.tuning.chaseEnabled=index==4;boss.tuning.comboLeapEnabled=index==5;boss.tuning.backshotEnabled=index==6;boss.tuning.fallingEnabled=index==7;
  Place(new Vector2(13,1.81f));yield return Until(()=>Motor.Grounded,"fixture ground");yield return Until(()=>boss.Fighting,"fight");
  Health.RestoreMax();hits=0;Pattern(index);
 }
 IEnumerator B12Counter(){
  driver.produce=null;yield return Keys();int hp=boss.HP;
  Place(new Vector2(boss.transform.position.x+boss.GetComponent<BoxCollider2D>().bounds.extents.x+.7f,1.81f));
  yield return Keys(Key.LeftArrow,Key.X);yield return Until(()=>boss.HP<hp,"X counter",.4f);Check(boss.IsNewRecovery,"X damage occurred inside recovery");yield return Keys();
  Check(boss.HP<hp,"actual X counter reduces boss HP in recovery");
 }
 IEnumerator B12Tests(){
  yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
  int index=N5Args.Int("-b12-pattern",4);bool fpsOnly=N5Args.Has("-b12-fps-only");
  foreach(bool dodge in fpsOnly?new[]{true}:new[]{false,true}){
   yield return B12Arena(index);double start=Time.timeAsDouble;bool dash=false;jumped=-100;second=false;peak=Body.position.y;int dj=Motor.DoubleJumpsPerformed;float initialBossX=boss.transform.position.x;var shotHeights=new System.Collections.Generic.List<float>();var shotIds=new System.Collections.Generic.HashSet<int>();
   driver.produce=()=>{
    peak=Mathf.Max(peak,Body.position.y);if(index==6)foreach(var live in FindObjectsByType<BossB3Hazard>(FindObjectsSortMode.None))if(!live.Falling&&shotIds.Add(live.GetInstanceID()))shotHeights.Add(Mathf.Round((live.transform.position.y-boss.floorY)*10)/10);if(!dodge)return Array.Empty<Key>();double t=Time.timeAsDouble-start;
    if(index==4){if(t>=.15&&!dash){dash=true;return new[]{Key.LeftArrow,Key.C};}return t<1.5?new[]{Key.LeftArrow}:Array.Empty<Key>();}
    if(index==5){var keys=new System.Collections.Generic.List<Key>();peak=Mathf.Max(peak,Body.position.y);if(t<1.2&&Body.position.x<17)keys.Add(Key.RightArrow);if(t>=1.5&&t<2.5&&Body.position.x>12)keys.Add(Key.LeftArrow);foreach(var live in FindObjectsByType<BossWave>(FindObjectsSortMode.None)){float gap=Mathf.Abs(live.transform.position.x-Body.position.x);if(jumped<0&&live.direction*(Body.position.x-live.transform.position.x)>0&&gap/live.speed<=.65f){jumped=Time.timeAsDouble;lines.Add("INPUT combo wave jump ETA="+gap/live.speed);}}double j=Time.timeAsDouble-jumped;if(jumped>=0&&(j<.18||(j>=.24&&j<.75)))keys.Add(Key.Z);return keys.ToArray();}
    if(index==6){foreach(var live in FindObjectsByType<BossB3Hazard>(FindObjectsSortMode.None)){if(live.Falling||Mathf.Abs(live.transform.position.y-(boss.floorY+boss.tuning.projectileLow))>.1f)continue;float gap=Mathf.Abs(live.transform.position.x-Body.position.x);if((jumped<0||Time.timeAsDouble-jumped>.7)&&live.Velocity.x*(Body.position.x-live.transform.position.x)>0&&gap/boss.tuning.projectileSpeed<=.4f){jumped=Time.timeAsDouble;lines.Add("INPUT low projectile jump ETA="+gap/boss.tuning.projectileSpeed);}}return jumped>=0&&Time.timeAsDouble-jumped<.32?new[]{Key.Z}:Array.Empty<Key>();}
    if(index==7)return t>=.85&&Body.position.x<14.45f?new[]{Key.RightArrow}:Array.Empty<Key>();
    return Array.Empty<Key>();
   };
   yield return Until(()=>boss.IsNewRecovery,"B3 pattern resolution",12);if(index==5||index==6)yield return new WaitForSeconds(.8f);driver.produce=null;yield return Keys();
   lines.Add($"MEASURE B12 pattern={index} fps={Application.targetFrameRate} dodge={dodge} hitEvents={hits} hp={Health.HP} DJ={Motor.DoubleJumpsPerformed-dj} peak={peak:F3}");
   Check(dodge?hits==0:hits>0,"stand hit / intended dodge zero");if(index==5)Check(boss.ComboLandings==2&&boss.WavesSpawned==2,"two landings; only second landing waves");if(index==7){lines.Add("MEASURE falling zones="+boss.FallingZoneCount+" playerX="+Body.position.x);Check(boss.FallingZoneCount==3,"three fall zones / safe gaps");}if(index==6){lines.Add("MEASURE shot heights="+string.Join(",",shotHeights)+" backstep="+Mathf.Abs(boss.transform.position.x-initialBossX));Check(shotHeights.Count==3&&shotHeights[0]==.5f&&shotHeights[1]==2.8f&&shotHeights[2]==.5f,"actual three shots low/high/low");Check(Mathf.Abs(boss.transform.position.x-initialBossX)<=boss.tuning.backstepDistance+.01f,"backstep max3 / wall clamp");}if(dodge&&!fpsOnly)yield return B12Counter();
  }
  if(fpsOnly)yield break;
  if(index==6){yield return B12Arena(index);boss.tuning.backshotCount=1;boss.tuning.projectileLow=boss.tuning.projectileHigh;Pattern(index);hits=0;yield return Until(()=>boss.IsNewRecovery,"high shot only");yield return new WaitForSeconds(.8f);Check(hits==0,"standing high projectile damage zero");}
  yield return B12Arena(index);if(index==6||index==7)yield return Until(()=>boss.LiveHazards>0,"active projectile before pause");yield return Tap(Key.Escape);Check(GameState.GetOrCreate().Paused,"B3 paused");
  var hazards=FindObjectsByType<BossB3Hazard>(FindObjectsSortMode.None);var hazardPositions=System.Array.ConvertAll(hazards,h=>h.transform.position);Vector3 p=boss.transform.position;double clock=Time.timeAsDouble;int token=boss.AttackToken;
  yield return new WaitForSecondsRealtime(.3f);Check(boss.transform.position==p&&Time.timeAsDouble==clock&&boss.AttackToken==token,"B3 pattern frozen");
  for(int h=0;h<hazards.Length;h++)Check(hazards[h]!=null&&hazards[h].transform.position==hazardPositions[h],"hazard frozen in pause");yield return Tap(Key.Escape);Check(!GameState.GetOrCreate().Paused,"B3 resumes");Health.RestoreMax();
  Check(Health.TakeDamage(Health.HP,Player.transform.position),"lethal setup");yield return Until(()=>RoomSession.Instance.Respawning,"B3 death");yield return Ready();
  Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04","death returns CP-C04");
  yield return B12Arena(index);Check(boss.HP==28&&!boss.PhaseTwo,"fresh B3 HP / phase");
  Check(boss.LiveHazards==0,"fresh projectile cleanup");Check(FindObjectsByType<BossWave>(FindObjectsSortMode.None).Length==0,"fresh wave cleanup");
  for(int repeat=0;repeat<10;repeat++){
   driver.produce=()=>new[]{Key.Z}; // sustained jump covers melee; other runs use setup-safe far position.
   Place(new Vector2(index==4?17:3,1.81f));Pattern(index);
   yield return Until(()=>boss.IsNewRecovery,"isolated repeat "+repeat,12);driver.produce=null;yield return Keys();Health.RestoreMax();
   lines.Add("MEASURE B12 isolated pattern="+index+" repeat="+(repeat+1));
  }
  if(index==6){boss.tuning.backshotEnabled=false;yield return new WaitForSeconds(3);Check(boss.LiveHazards==0,"projectiles disappear at arena walls");Check(boss.GetComponent<BoxCollider2D>().bounds.min.x>=boss.leftWallX&&boss.GetComponent<BoxCollider2D>().bounds.max.x<=boss.rightWallX,"backstep remains inside walls");}
 }

 IEnumerator B12PhaseTests(){
  yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
  yield return B12Arena(4);boss.tuning.phaseTwoEnabled=true;boss.ReceiveHit(14,Player.transform.position);yield return Until(()=>boss.State==BossEncounter.Phase.PhaseTwoTell,"phase two transition");
  Check(boss.PhaseTwo&&boss.PhaseTransitions==1&&boss.HP==14,"half HP transition once");int hp=boss.HP;boss.ReceiveHit(1,Player.transform.position);Check(boss.HP==hp-1,"transition still accepts boss damage");
  double began=boss.PhaseStarted;yield return Until(()=>boss.State!=BossEncounter.Phase.PhaseTwoTell,"phase warning ends");Check(Time.timeAsDouble-began>=boss.tuning.phaseTwoTell,"phase warning >= .6s");
  boss.ReceiveHit(1,Player.transform.position);Health.RestoreMax();Place(new Vector2(3,1.81f));Pattern(4);
  yield return Until(()=>boss.State==BossEncounter.Phase.ChaseRecovery,"phase two chase");Check(boss.ChaseStrikes==4&&boss.PhaseTransitions==1,"phase two 4 strikes; transition not repeated");
  began=boss.PhaseStarted;yield return Until(()=>boss.State!=BossEncounter.Phase.ChaseRecovery,"phase two recovery");
  lines.Add("MEASURE phase two chase recovery="+boss.LastPhaseDuration);Check(Mathf.Abs((float)boss.LastPhaseDuration-Mathf.Max(.8f,boss.tuning.chaseRecovery*.8f))<.07f,"phase two recovery scale / minimum");
  boss.tuning.comboLeapEnabled=true;Health.RestoreMax();Place(new Vector2(13,1.81f));Pattern(5);
  yield return Until(()=>boss.State==BossEncounter.Phase.ComboLeapRecovery,"phase two three leaps",10);Check(boss.ComboLandings==3&&boss.PhaseTransitions==1,"phase two three landings / one transition");
  yield return Until(()=>boss.State!=BossEncounter.Phase.ComboLeapRecovery,"phase two combo recovery");lines.Add("MEASURE phase two combo recovery="+boss.LastPhaseDuration);Check(Mathf.Abs((float)boss.LastPhaseDuration-Mathf.Max(boss.tuning.phaseTwoRecoveryMinimum,boss.tuning.comboLeapRecovery*boss.tuning.phaseTwoRecoveryScale))<.07f,"phase two combo recovery scale / minimum");
  Health.RestoreMax();Check(Health.TakeDamage(Health.HP,Player.transform.position),"phase two lethal setup");yield return Until(()=>RoomSession.Instance.Respawning,"phase two death");yield return Ready();
  Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04","phase two death CP-C04");yield return B12Arena(4);
  Check(!boss.PhaseTwo&&boss.PhaseTransitions==0&&boss.HP==28,"death resets phase one and HP28");Check(boss.LiveHazards==0&&FindObjectsByType<BossWave>(FindObjectsSortMode.None).Length==0,"death clears all boss hazards");
 }

 IEnumerator B12CycleTests(){
  yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;yield return B12Arena(4);
  boss.tuning=Instantiate(authoredB3);Health.tuning=Instantiate(Health.tuning);Health.tuning.playerMaxHP=100;Health.RestoreMax();Place(new Vector2(13,1.81f));Pattern(0);
  lines.Add("FIXTURE one-cycle stability HP100; no health/state/position writes during cycle; not a natural victory test");
  var order=new System.Collections.Generic.List<int>{0};int starts=boss.PatternStarts;double deadline=Time.realtimeSinceStartupAsDouble+100;
  while(order.Count<9){if(Time.realtimeSinceStartupAsDouble>deadline)throw new System.Exception("B3 cycle timeout");if(boss.PatternStarts!=starts){starts=boss.PatternStarts;order.Add(boss.ActivePattern);lines.Add("MEASURE cycle pattern="+boss.ActivePattern+" state="+boss.State);}yield return null;}
  Check(string.Join(",",order)=="0,4,1,6,2,5,7,3,0","eight-pattern cycle exact order");Check(!RoomSession.Instance.Respawning,"cycle does not reset through death");
  boss.tuning.chaseEnabled=boss.tuning.comboLeapEnabled=boss.tuning.backshotEnabled=boss.tuning.fallingEnabled=boss.tuning.leapEnabled=boss.tuning.chargeEnabled=false;Pattern(0);starts=boss.PatternStarts;
  yield return Until(()=>boss.PatternStarts>starts,"disabled patterns skipped",15);Check(boss.ActivePattern==2,"disabled new/leap/charge skipped to wave");
  boss.tuning.meleeEnabled=boss.tuning.waveEnabled=false;yield return Until(()=>boss.State==BossEncounter.Phase.Dormant,"all patterns off",15);yield return new WaitForSeconds(3);
  Check(boss.LiveHazards==0&&FindObjectsByType<BossWave>(FindObjectsSortMode.None).Length==0,"all off: projectiles / falls / waves gone at walls or floor");
 }

 IEnumerator B15SoakTests(){
  yield return ContinueGame();driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;yield return B12Arena(4);
  boss.tuning=Instantiate(authoredB3);Health.RestoreMax();Body.linearVelocity=Vector2.zero;Body.constraints=RigidbodyConstraints2D.FreezeAll;
  typeof(PlayerHealth).GetField("invincibleUntil",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(Health,double.PositiveInfinity);
  Check(Health.Invincible&&Application.targetFrameRate==60,"60fps invulnerable fixed-player soak fixture");
  lines.Add("FIXTURE player invulnerable and fixed in place; all eight default B3 patterns run without hazard cleanup or boss state writes during cycles");Pattern(0);
  int[] order={0,4,1,6,2,5,7,3};int next=1,cycles=0,starts=boss.PatternStarts;double deadline=Time.realtimeSinceStartupAsDouble+360;
  while(cycles<3){
   if(Time.realtimeSinceStartupAsDouble>deadline)throw new Exception("Three-cycle soak timeout");
   if(boss.PatternStarts!=starts){starts=boss.PatternStarts;int expected=order[next%order.Length];Check(boss.ActivePattern==expected,"soak pattern "+next+"="+expected);next++;
    if(expected==0){cycles++;var hazards=FindObjectsByType<BossB3Hazard>(FindObjectsInactive.Include,FindObjectsSortMode.None);int falling=System.Array.FindAll(hazards,h=>h.Falling).Length,projectiles=hazards.Length-falling,waves=FindObjectsByType<BossWave>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length;lines.Add($"MEASURE B15 soak cycle={cycles} projectiles={projectiles} falling={falling} waves={waves}");Check(projectiles==0&&falling==0&&waves==0,"cycle "+cycles+" hazard objects zero");}
   }
   if(RoomSession.Instance.Respawning)throw new Exception("Soak unexpectedly respawned");yield return null;
  }
 }

 // B14 D-B3: default boss (no -boss argument) defeat -> Slam -> S01 summit ending. HP100 survival fixture,
 // HP setup by ReceiveHit and S01 entry by transition; only the final blow is actual X. Not a natural victory test.
 IEnumerator B14DefeatTests(){
  yield return ContinueGame();driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
  Check(RoomSession.Instance.RequestTransition("C06",V4SceneMap.Path("C06"),"C04-C06"),"default arena C06_V4");yield return Ready();boss=FindFirstObjectByType<BossEncounter>();
  Check(boss.tuning.b3Patterns&&boss.tuning.phaseTwoEnabled,"default boss is B3 without -boss");lines.Add("MEASURE default boss tuning="+boss.tuning.name+" HP="+boss.HP+" patterns="+boss.EnabledPatterns);
  Health.tuning=Instantiate(Health.tuning);Health.tuning.playerMaxHP=100;Health.RestoreMax();
  Place(new Vector2(13,1.81f));yield return Until(()=>boss.Fighting,"fight");
  boss.ReceiveHit(boss.HP-1,Player.transform.position);Check(boss.HP==1,"fixture HP1");
  yield return Until(()=>boss.IsNewRecovery,"B3 recovery for final X",30);Health.RestoreMax();driver.produce=null;yield return Keys();
  Place(new Vector2(boss.transform.position.x+boss.GetComponent<BoxCollider2D>().bounds.extents.x+.7f,1.81f));Check(boss.IsNewRecovery,"final X starts inside recovery");
  yield return Keys(Key.LeftArrow,Key.X);yield return Until(()=>boss.HP==0,"actual final X",.4f);yield return Keys();
  Check(boss.State==BossEncounter.Phase.Defeated&&ProgressSave.BossDefeated&&Player.GetComponent<PlayerSlam>().hasSlam,"actual final X defeats B3; boss flag and Slam");
  Check(RoomSession.Instance.RequestTransition("S01",V4SceneMap.Path("S01"),"C06-S01"),"S01 entry");yield return Ready();
  var summit=FindFirstObjectByType<V3SummitEnding>();Check(summit!=null,"S01 summit ending present");Place(summit.GetComponent<BoxCollider2D>().bounds.center);
  yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"summit ending",10);
  Check(ProgressSave.TryLoad(out var saved)&&saved.bossDefeated&&saved.endingReached,"boss defeat -> ending saved");
 }
 IEnumerator B12P2Tests(){
  yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;yield return B12Arena(4);
  lines.Add("MEASURE P2 launcher initial HP="+boss.HP);Check(boss.HP==14&&!boss.PhaseTwo,"HP14 begins in phase one before first hit");
  yield return Until(()=>boss.IsNewRecovery,"P2 counter opportunity");Health.RestoreMax();yield return B12Counter();
  yield return Until(()=>boss.PhaseTwo&&boss.State==BossEncounter.Phase.PhaseTwoTell,"first X activates phase two");
  Check(boss.HP==13&&boss.PhaseTransitions==1,"first actual X: HP13 / phase transition once");
 }

}
#endif
