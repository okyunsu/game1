using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider2D),typeof(SpriteRenderer))]
public sealed class BossEncounter:MonoBehaviour,IDamageable
{
 public enum Phase{Idle,MeleeTell,MeleeRecovery,WaveTell,WaveRecovery,Defeated,Walk,LeapTell,LeapFlight,LeapRecovery,ChargeTell,Charge,ChargeRecovery}
 [Tooltip("Shared boss prototype values.")]public BossTuning tuning;
 [Header("Scene references")]
 public RoomExit leftExit;
 public SpriteRenderer warning;
 public GameObject wavePrefab;
 [Tooltip("Player center x must exceed this scene start line (u).")]public float startX=5;
 [Tooltip("Arena floor top (world y, u).")]public float floorY=1;
 [Tooltip("Left wall inner edge (world x, u).")]public float leftWallX=1;
 [Tooltip("Right wall inner edge (world x, u), used only by B2.")]public float rightWallX=19;
 public int HP{get;private set;}
 public int Hits{get;private set;}
 public int WavesSpawned{get;private set;}
 public Phase State{get;private set;}
 public double PhaseStarted{get;private set;}
 public double LastPhaseDuration{get;private set;}
 public bool Fighting=>State!=Phase.Idle&&State!=Phase.Defeated;
 public BossWave LiveWave=>wave;
 public event Action Defeated;
 BossWave wave;bool deathQueued,resolvingDeath;Color[] doorColors;
 SpriteRenderer[] doorSprites;readonly Collider2D[] contacts=new Collider2D[16];
 readonly System.Collections.Generic.List<BossWave> candidateWaves=new();
 readonly System.Collections.Generic.HashSet<int> damagedPatterns=new();
 int pattern,attackToken;float facing=-1,walkTarget,leapTarget;Vector3 leapStart;
 public float LeapTarget=>leapTarget;
 public int AttackToken=>attackToken;
 public float Facing=>facing;
 public Bounds MeleeBounds=>new Bounds(new Vector3(tuning.extendedPatterns?transform.position.x+facing*(GetComponent<BoxCollider2D>().bounds.extents.x+tuning.meleeRange*.5f):GetComponent<BoxCollider2D>().bounds.min.x-tuning.meleeRange*.5f,floorY+.5f,0),new Vector3(tuning.meleeRange,1,1));
 void Awake(){HP=tuning.maxHP;doorSprites=leftExit.GetComponentsInChildren<SpriteRenderer>();doorColors=new Color[doorSprites.Length];for(int i=0;i<doorSprites.Length;i++)doorColors[i]=doorSprites[i].color;warning.enabled=false;}
 void SetLock(bool locked){leftExit.enabled=!locked;for(int i=0;i<doorSprites.Length;i++)doorSprites[i].color=locked?new Color(.9f,.2f,.2f):doorColors[i];}
 void SetPhase(Phase phase){LastPhaseDuration=Time.timeAsDouble-PhaseStarted;State=phase;PhaseStarted=Time.timeAsDouble;warning.enabled=phase==Phase.MeleeTell||phase==Phase.WaveTell;warning.color=phase==Phase.MeleeTell?new Color(1,.25f,.15f,.6f):new Color(1,.8f,.1f,.6f);}
 void Update(){
  var session=RoomSession.Instance;if(session?.Player==null)return;
  if(wave==null)wave=null;
  if(State==Phase.Idle){if(!session.Player.InputLocked&&session.Player.transform.position.x>startX){SetLock(true);if(tuning.extendedPatterns){GetComponent<BoxCollider2D>().isTrigger=true;BeginPattern(0);}else SetPhase(Phase.MeleeTell);}return;}
  if(!Fighting||GameState.GetOrCreate().Paused||session.Transitioning||session.Respawning||deathQueued)return;
  double elapsed=Time.timeAsDouble-PhaseStarted;
  if(tuning.extendedPatterns){UpdateExtended((float)elapsed);return;}
  if(State==Phase.MeleeTell&&elapsed>=tuning.meleeTell){var bounds=MeleeBounds;int n=Physics2D.OverlapBox(bounds.center,bounds.size,0,new ContactFilter2D{useTriggers=true},contacts);for(int i=0;i<n;i++)contacts[i].GetComponent<PlayerHealth>()?.TakeDamage(tuning.damage,transform.position);SetPhase(Phase.MeleeRecovery);}
  else if(State==Phase.MeleeRecovery&&elapsed>=tuning.recovery)SetPhase(Phase.WaveTell);
  else if(State==Phase.WaveTell&&elapsed>=tuning.waveTell){var go=Instantiate(wavePrefab,new Vector3(GetComponent<BoxCollider2D>().bounds.min.x-.3f,floorY+tuning.waveHeight*.5f,0),Quaternion.identity);SceneManager.MoveGameObjectToScene(go,gameObject.scene);go.transform.localScale=new Vector3(.6f,tuning.waveHeight,1);wave=go.GetComponent<BossWave>();wave.speed=tuning.waveSpeed;wave.damage=tuning.damage;wave.leftWallX=leftWallX;WavesSpawned++;SetPhase(Phase.WaveRecovery);}
  else if(State==Phase.WaveRecovery&&elapsed>=tuning.recovery)SetPhase(Phase.MeleeTell);
 }
 float StandingY=>floorY+GetComponent<BoxCollider2D>().bounds.extents.y;
 float ClampX(float x)=>Mathf.Clamp(x,leftWallX+GetComponent<BoxCollider2D>().bounds.extents.x+tuning.wallClearance,rightWallX-GetComponent<BoxCollider2D>().bounds.extents.x-tuning.wallClearance);
 void Warn(float x,float width,Color color){warning.enabled=true;warning.transform.position=new Vector3(x,floorY+.15f,0);warning.transform.localScale=new Vector3(width,.3f,1);warning.color=color;}
 void BeginPattern(int index){pattern=index;attackToken++;facing=RoomSession.Instance.Player.transform.position.x<transform.position.x?-1:1;
  if(index==0){SetPhase(Phase.MeleeTell);Warn(MeleeBounds.center.x,tuning.meleeRange,new Color(1,.25f,.15f,.6f));}
  else if(index==1){leapTarget=ClampX(RoomSession.Instance.Player.transform.position.x);SetPhase(Phase.LeapTell);Warn(leapTarget,tuning.slamWidth,new Color(1,.25f,.15f,.6f));}
  else if(index==2){SetPhase(Phase.WaveTell);Warn(transform.position.x+facing*3,6,new Color(1,.8f,.1f,.6f));}
  else {SetPhase(Phase.ChargeTell);Warn((transform.position.x+(facing<0?leftWallX:rightWallX))*.5f,Mathf.Abs(transform.position.x-(facing<0?leftWallX:rightWallX)),new Color(1,.25f,.15f,.6f));}
 }
 void BeginWalk(){walkTarget=Mathf.Abs(RoomSession.Instance.Player.transform.position.x-tuning.leftAnchor)>Mathf.Abs(RoomSession.Instance.Player.transform.position.x-tuning.rightAnchor)?tuning.leftAnchor:tuning.rightAnchor;walkTarget=ClampX(walkTarget);SetPhase(Phase.Walk);}
 void MoveBoss(Vector3 p){transform.position=p;Physics2D.SyncTransforms();}
 void DamageArea(Bounds bounds){int n=Physics2D.OverlapBox(bounds.center,bounds.size,0,new ContactFilter2D{useTriggers=true},contacts);for(int i=0;i<n;i++){var health=contacts[i].GetComponent<PlayerHealth>();if(health!=null)TryPatternDamage(health,attackToken,transform.position);}}
 public bool TryPatternDamage(PlayerHealth health,int token,Vector2 source){if(!Fighting||deathQueued||damagedPatterns.Contains(token))return false;if(!health.TakeDamage(tuning.damage,source))return false;damagedPatterns.Add(token);return true;}
 void SpawnCandidateWave(float direction,float x){var go=Instantiate(wavePrefab,new Vector3(x,floorY+tuning.waveHeight*.5f,0),Quaternion.identity);SceneManager.MoveGameObjectToScene(go,gameObject.scene);go.transform.localScale=new Vector3(.6f,tuning.waveHeight,1);wave=go.GetComponent<BossWave>();wave.speed=tuning.waveSpeed;wave.damage=tuning.damage;wave.leftWallX=leftWallX;wave.rightWallX=rightWallX;wave.direction=direction;wave.candidateOwner=this;wave.attackToken=attackToken;candidateWaves.Add(wave);WavesSpawned++;}
 void UpdateExtended(float elapsed){
  if(State==Phase.Walk){MoveBoss(new Vector3(Mathf.MoveTowards(transform.position.x,walkTarget,tuning.walkSpeed*Time.deltaTime),StandingY,0));int n=Physics2D.OverlapBox(GetComponent<BoxCollider2D>().bounds.center,GetComponent<BoxCollider2D>().bounds.size,0,new ContactFilter2D{useTriggers=true},contacts);for(int i=0;i<n;i++)contacts[i].GetComponent<PlayerHealth>()?.TakeDamage(tuning.damage,transform.position);if(Mathf.Abs(transform.position.x-walkTarget)<.001f)BeginPattern((pattern+1)%4);}
  else if(State==Phase.MeleeTell&&elapsed>=tuning.meleeTell){DamageArea(MeleeBounds);SetPhase(Phase.MeleeRecovery);}
  else if(State==Phase.LeapTell&&elapsed>=tuning.leapTell){leapStart=transform.position;SetPhase(Phase.LeapFlight);Warn(leapTarget,tuning.slamWidth,new Color(1,.25f,.15f,.6f));}
  else if(State==Phase.LeapFlight){float t=Mathf.Clamp01(elapsed/tuning.leapFlight);MoveBoss(new Vector3(Mathf.Lerp(leapStart.x,leapTarget,t),StandingY+4*t*(1-t)*tuning.leapHeight,0));if(t>=1){DamageArea(new Bounds(new Vector3(leapTarget,floorY+.5f,0),new Vector3(tuning.slamWidth,1,1)));SpawnCandidateWave(-1,leapTarget-1.3f);SpawnCandidateWave(1,leapTarget+1.3f);SetPhase(Phase.LeapRecovery);}}
  else if(State==Phase.WaveTell&&elapsed>=tuning.waveTell){SpawnCandidateWave(facing,transform.position.x+facing*(GetComponent<BoxCollider2D>().bounds.extents.x+.3f));SetPhase(Phase.WaveRecovery);}
  else if(State==Phase.ChargeTell&&elapsed>=tuning.chargeTell)SetPhase(Phase.Charge);
  else if(State==Phase.Charge){float x=ClampX(transform.position.x+facing*tuning.chargeSpeed*Time.deltaTime);MoveBoss(new Vector3(x,StandingY,0));DamageArea(GetComponent<BoxCollider2D>().bounds);if(Mathf.Abs(x-ClampX(facing<0?leftWallX:rightWallX))<.001f)SetPhase(Phase.ChargeRecovery);}
  else if((State==Phase.MeleeRecovery||State==Phase.LeapRecovery||State==Phase.WaveRecovery||State==Phase.ChargeRecovery)&&elapsed>=tuning.recovery)BeginWalk();
 }
 public void ReceiveHit(int damage,Vector2 source){if(!Fighting||HP<=0||damage<=0)return;HP=Mathf.Max(0,HP-damage);Hits++;GetComponent<HitFeedback>()?.Hit(HP);if(HP==0){SetPhase(Phase.Defeated);SetLock(false);if(wave!=null)Destroy(wave.gameObject);foreach(var live in candidateWaves)if(live!=null)Destroy(live.gameObject);wave=null;warning.enabled=false;Defeated?.Invoke();}}
 public bool DeferDeath(){if(resolvingDeath)return false;if(State==Phase.Defeated)return true;if(!Fighting)return false;if(!deathQueued){deathQueued=true;}return true;}
 void LateUpdate(){if(!deathQueued)return;if(State==Phase.Defeated){deathQueued=false;return;}resolvingDeath=true;deathQueued=false;RoomSession.Instance.Die();}
 void Contact(Collision2D c){if(Fighting&&!deathQueued){var health=c.collider.GetComponent<PlayerHealth>();if(health==null)return;if(tuning.extendedPatterns&&State!=Phase.Walk)TryPatternDamage(health,attackToken,transform.position);else health.TakeDamage(tuning.damage,transform.position);}}
 void OnCollisionEnter2D(Collision2D c)=>Contact(c);
 void OnCollisionStay2D(Collision2D c)=>Contact(c);
 void OnTriggerStay2D(Collider2D c){if(!tuning.extendedPatterns||!Fighting||State==Phase.LeapFlight||State==Phase.Walk||deathQueued)return;var health=c.GetComponent<PlayerHealth>();if(health!=null)TryPatternDamage(health,attackToken,transform.position);}
 void OnGUI(){if(State==Phase.Idle)return;float x=Screen.width*.5f-200;GUI.Box(new Rect(x,30,400,28),$"정상 수문장  {HP}/{tuning.maxHP}");GUI.Box(new Rect(x,60,400f*HP/tuning.maxHP,8),"");}
}

