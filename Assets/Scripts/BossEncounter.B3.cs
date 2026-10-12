using UnityEngine;
public sealed partial class BossEncounter
{
 public int ActivePattern=>pattern;
 public int PatternStarts{get;private set;}
 public bool IsNewRecovery=>State==Phase.ChaseRecovery||State==Phase.ComboLeapRecovery||State==Phase.BackshotRecovery||State==Phase.FallingRecovery;
 public bool PhaseTwo{get;private set;}
 public int PhaseTransitions{get;private set;}
 static readonly int[] B3Order={0,4,1,6,2,5,7,3};
 public static string PatternLabel(int i)=>i switch{0=>"melee",1=>"leap",2=>"wave",3=>"charge",4=>"chase",5=>"comboLeap",6=>"backshot",7=>"falling",_=>"off"};
 public string EnabledPatterns=>string.Join(",",System.Array.ConvertAll(System.Array.FindAll(B3Order,PatternEnabled),PatternLabel));
 bool PatternEnabled(int i)=>i switch{0=>tuning.meleeEnabled,1=>tuning.leapEnabled,2=>tuning.waveEnabled,3=>tuning.chargeEnabled,4=>tuning.chaseEnabled,5=>tuning.comboLeapEnabled,6=>tuning.backshotEnabled,7=>tuning.fallingEnabled,_=>false};
 int NextPattern(){if(!tuning.b3Patterns)return (pattern+1)%4;int at=System.Array.IndexOf(B3Order,pattern);for(int step=1;step<=B3Order.Length;step++){int next=B3Order[(at+step)%B3Order.Length];if(PatternEnabled(next))return next;}return -1;}
 int strikes;float chaseStart,nextStrike;
 Bounds ChaseBounds=>new Bounds(new Vector3(transform.position.x+facing*(GetComponent<BoxCollider2D>().bounds.extents.x+tuning.chaseRange*.5f),floorY+.5f,0),new Vector3(tuning.chaseRange,1,1));
 float RecoveryTime(float value)=>PhaseTwo?Mathf.Max(tuning.phaseTwoRecoveryMinimum,value*tuning.phaseTwoRecoveryScale):value;
 void BeginB3Pattern(int index){
  if(index==7){BeginFalling();return;}if(index==6){BeginBackshot();return;}if(index==5){BeginComboLeap();return;}if(index==4){strikes=0;chaseStart=transform.position.x;SetPhase(Phase.ChaseTell);Warn(ChaseBounds.center.x,tuning.chaseRange,new Color(1,.25f,.15f,.6f));}
 }
 bool UpdateB3(float elapsed){if(UpdatePhaseTwo(elapsed))return true;if(UpdateComboLeap(elapsed)||UpdateBackshot(elapsed)||UpdateFalling(elapsed))return true;
  if(State==Phase.ChaseTell){if(elapsed>=tuning.chaseTell){nextStrike=tuning.chaseStrikeTell;SetPhase(Phase.Chase);}return true;}
  if(State==Phase.Chase){
   float destination=ClampX(chaseStart+facing*tuning.chaseDistance);
   MoveBoss(new Vector3(Mathf.MoveTowards(transform.position.x,destination,tuning.chaseSpeed*Time.deltaTime),StandingY,0));
   bool tell=elapsed>=nextStrike-tuning.chaseStrikeTell;warning.enabled=tell;if(tell)Warn(ChaseBounds.center.x,tuning.chaseRange,new Color(1,.25f,.15f,.6f));
   if(elapsed>=nextStrike){attackToken++;DamageArea(ChaseBounds);strikes++;nextStrike+=tuning.chaseInterval;
    if(strikes>=(PhaseTwo?tuning.phaseTwoChaseCount:tuning.chaseCount))SetPhase(Phase.ChaseRecovery);
   }return true;
  }
  if(State==Phase.ChaseRecovery){if(elapsed>=RecoveryTime(tuning.chaseRecovery))BeginWalk();return true;}
  return false;
 }
 public int ComboLandings=>comboLandings;
 int comboLandings;bool comboWaves;
 void BeginComboLeap(){comboLandings=0;comboWaves=false;BeginComboTell();}
 void BeginComboTell(){leapTarget=ClampX(RoomSession.Instance.Player.transform.position.x);SetPhase(Phase.ComboLeapTell);Warn(leapTarget,tuning.slamWidth,new Color(1,.25f,.15f,.6f));}
 bool UpdateComboLeap(float elapsed){
  if(State==Phase.ComboLeapTell){float tell=comboLandings==0?tuning.comboFirstTell:tuning.comboNextTell;
   if(elapsed>=tell){leapStart=transform.position;SetPhase(Phase.ComboLeapFlight);Warn(leapTarget,tuning.slamWidth,new Color(1,.25f,.15f,.6f));}return true;}
  if(State==Phase.ComboLeapFlight){float t=Mathf.Clamp01(elapsed/tuning.leapFlight);MoveBoss(new Vector3(Mathf.Lerp(leapStart.x,leapTarget,t),StandingY+4*t*(1-t)*tuning.leapHeight,0));
   if(t>=1){attackToken++;DamageArea(new Bounds(new Vector3(leapTarget,floorY+.5f,0),new Vector3(tuning.slamWidth,1,1)));comboLandings++;comboWaves=comboLandings>1;
    if(comboWaves){SpawnCandidateWave(-1,leapTarget-1.3f);SpawnCandidateWave(1,leapTarget+1.3f);}
    SetPhase(comboLandings>=(PhaseTwo?tuning.phaseTwoLeapCount:tuning.comboLeapCount)?Phase.ComboLeapRecovery:Phase.ComboLeapGap);
   }return true;}
  if(State==Phase.ComboLeapGap){if(elapsed>=tuning.comboLandingGap)BeginComboTell();return true;}
  if(State==Phase.ComboLeapRecovery){if(elapsed>=RecoveryTime(tuning.comboLeapRecovery))BeginWalk();return true;}
  return false;
 }

 readonly System.Collections.Generic.List<BossB3Hazard> b3Hazards=new();
 public int LiveHazards=>b3Hazards.FindAll(h=>h!=null).Count;
 public int ShotsFired{get;private set;}
 int shots;float backstepStart,backstepTarget;
 void BeginBackshot(){shots=0;backstepStart=transform.position.x;backstepTarget=ClampX(backstepStart-facing*tuning.backstepDistance);SetPhase(Phase.BackshotTell);WarnShot(0);}
 void WarnShot(int number){warning.enabled=true;warning.transform.position=new Vector3(transform.position.x+facing*(GetComponent<BoxCollider2D>().bounds.extents.x+tuning.projectileSize*.5f),floorY+(number%2==0?tuning.projectileLow:tuning.projectileHigh),0);warning.transform.localScale=new Vector3(tuning.projectileSize,tuning.projectileSize,1);warning.color=new Color(1,.8f,.1f,.7f);}
 void SpawnShot(int number){attackToken++;float y=floorY+(number%2==0?tuning.projectileLow:tuning.projectileHigh);SpawnHazard(tuning.projectilePrefab,new Vector2(transform.position.x+facing*(GetComponent<BoxCollider2D>().bounds.extents.x+tuning.projectileSize*.5f),y),Vector2.right*facing*tuning.projectileSpeed,Vector2.one*tuning.projectileSize,false);ShotsFired++;}
 void SpawnHazard(GameObject prefab,Vector2 position,Vector2 velocity,Vector2 size,bool falling){
  var go=Instantiate(prefab,position,Quaternion.identity);UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go,gameObject.scene);
  var hazard=go.GetComponent<BossB3Hazard>();hazard.Setup(this,velocity,size,attackToken,falling);b3Hazards.Add(hazard);
 }
 void ClearB3Hazards(){foreach(var live in b3Hazards)if(live!=null)Destroy(live.gameObject);b3Hazards.Clear();}
 bool UpdateBackshot(float elapsed){
  if(State==Phase.BackshotTell){if(elapsed>=tuning.backshotTell)SetPhase(Phase.Backstep);return true;}
  if(State==Phase.Backstep){float t=Mathf.Clamp01(elapsed/tuning.backstepDuration);MoveBoss(new Vector3(Mathf.Lerp(backstepStart,backstepTarget,t),StandingY,0));WarnShot(0);if(t>=1){facing=RoomSession.Instance.Player.transform.position.x<transform.position.x?-1:1;SetPhase(Phase.BackshotFire);WarnShot(0);}return true;}
  if(State==Phase.BackshotFire){
   if(elapsed>=shots*tuning.backshotInterval){SpawnShot(shots++);if(shots>=tuning.backshotCount)SetPhase(Phase.BackshotRecovery);else WarnShot(shots);}return true;}
  if(State==Phase.BackshotRecovery){if(elapsed>=RecoveryTime(tuning.backshotRecovery))BeginWalk();return true;}
  return false;
 }

 readonly System.Collections.Generic.List<SpriteRenderer> fallingWarnings=new();
 readonly System.Collections.Generic.List<float> fallingTargets=new();
 public int FallingZoneCount=>fallingTargets.Count;
 void ClearFallingWarnings(){foreach(var marker in fallingWarnings)if(marker!=null)Destroy(marker.gameObject);fallingWarnings.Clear();}
 void BeginFalling(){ClearFallingWarnings();fallingTargets.Clear();SetPhase(Phase.FallingTell);Warn(transform.position.x,tuning.slamWidth,new Color(1,.25f,.15f,.6f));}
 void MarkFallingZones(){
  float playerX=RoomSession.Instance.Player.transform.position.x;ClearFallingWarnings();
  for(int i=-1;i<=1;i++){float x=playerX+i*tuning.fallingSpacing;if(x<leftWallX||x>rightWallX)continue;fallingTargets.Add(x);
   var marker=Instantiate(warning);UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(marker.gameObject,gameObject.scene);marker.transform.position=new Vector3(x,floorY+.15f,0);marker.transform.localScale=new Vector3(tuning.fallingWidth,.3f,1);marker.color=new Color(.8f,.25f,1,.7f);marker.enabled=true;fallingWarnings.Add(marker);
  }
 }
 bool UpdateFalling(float elapsed){
  if(State==Phase.FallingTell){if(elapsed>=tuning.fallingTell){SetPhase(Phase.FallingZones);MarkFallingZones();}return true;}
  if(State==Phase.FallingZones){if(elapsed>=tuning.fallingZoneTell){ClearFallingWarnings();attackToken++;foreach(float x in fallingTargets)SpawnHazard(tuning.fallingPrefab,new Vector2(x,floorY+tuning.fallingCeiling),Vector2.down*tuning.fallingSpeed,new Vector2(tuning.fallingWidth,tuning.fallingHeight),true);SetPhase(Phase.FallingDrop);}return true;}
  if(State==Phase.FallingDrop){if(b3Hazards.FindAll(h=>h!=null&&h.Falling).Count==0)SetPhase(Phase.FallingRecovery);return true;}
  if(State==Phase.FallingRecovery){if(elapsed>=RecoveryTime(tuning.fallingRecovery))BeginWalk();return true;}
  return false;
 }

 bool phaseTwoQueued;
 public int ChaseStrikes=>strikes;
 void CheckPhaseTwoHit(){if(tuning.b3Patterns&&tuning.phaseTwoEnabled&&!PhaseTwo&&HP>0&&HP<=tuning.maxHP*tuning.phaseTwoThreshold)phaseTwoQueued=true;}
 bool UpdatePhaseTwo(float elapsed){
  if(phaseTwoQueued){phaseTwoQueued=false;PhaseTwo=true;PhaseTransitions++;ClearB3Hazards();ClearFallingWarnings();foreach(var live in candidateWaves)if(live!=null)Destroy(live.gameObject);candidateWaves.Clear();wave=null;MoveBoss(new Vector3(transform.position.x,StandingY,0));SetPhase(Phase.PhaseTwoTell);Warn(transform.position.x,GetComponent<BoxCollider2D>().bounds.size.x,new Color(1,.25f,.7f,.8f));return true;}
  if(State==Phase.PhaseTwoTell){if(elapsed>=tuning.phaseTwoTell)BeginPattern(pattern);return true;}return false;
 }

}

