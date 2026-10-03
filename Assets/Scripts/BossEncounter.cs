using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider2D),typeof(SpriteRenderer))]
public sealed class BossEncounter:MonoBehaviour,IDamageable
{
 public enum Phase{Idle,MeleeTell,MeleeRecovery,WaveTell,WaveRecovery,Defeated}
 [Tooltip("Shared boss prototype values.")]public BossTuning tuning;
 [Header("Scene references")]
 public RoomExit leftExit;
 public SpriteRenderer warning;
 public GameObject wavePrefab;
 [Tooltip("Player center x must exceed this scene start line (u).")]public float startX=5;
 [Tooltip("Arena floor top (world y, u).")]public float floorY=1;
 [Tooltip("Left wall inner edge (world x, u).")]public float leftWallX=1;
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
 public Bounds MeleeBounds=>new Bounds(new Vector3(GetComponent<BoxCollider2D>().bounds.min.x-tuning.meleeRange*.5f,floorY+.5f,0),new Vector3(tuning.meleeRange,1,1));
 void Awake(){HP=tuning.maxHP;doorSprites=leftExit.GetComponentsInChildren<SpriteRenderer>();doorColors=new Color[doorSprites.Length];for(int i=0;i<doorSprites.Length;i++)doorColors[i]=doorSprites[i].color;warning.enabled=false;}
 void SetLock(bool locked){leftExit.enabled=!locked;for(int i=0;i<doorSprites.Length;i++)doorSprites[i].color=locked?new Color(.9f,.2f,.2f):doorColors[i];}
 void SetPhase(Phase phase){LastPhaseDuration=Time.timeAsDouble-PhaseStarted;State=phase;PhaseStarted=Time.timeAsDouble;warning.enabled=phase==Phase.MeleeTell||phase==Phase.WaveTell;warning.color=phase==Phase.MeleeTell?new Color(1,.25f,.15f,.6f):new Color(1,.8f,.1f,.6f);}
 void Update(){
  var session=RoomSession.Instance;if(session?.Player==null)return;
  if(wave==null)wave=null;
  if(State==Phase.Idle){if(!session.Player.InputLocked&&session.Player.transform.position.x>startX){SetLock(true);SetPhase(Phase.MeleeTell);}return;}
  if(!Fighting||GameState.GetOrCreate().Paused||session.Transitioning||session.Respawning||deathQueued)return;
  double elapsed=Time.timeAsDouble-PhaseStarted;
  if(State==Phase.MeleeTell&&elapsed>=tuning.meleeTell){var bounds=MeleeBounds;int n=Physics2D.OverlapBox(bounds.center,bounds.size,0,new ContactFilter2D{useTriggers=true},contacts);for(int i=0;i<n;i++)contacts[i].GetComponent<PlayerHealth>()?.TakeDamage(tuning.damage,transform.position);SetPhase(Phase.MeleeRecovery);}
  else if(State==Phase.MeleeRecovery&&elapsed>=tuning.recovery)SetPhase(Phase.WaveTell);
  else if(State==Phase.WaveTell&&elapsed>=tuning.waveTell){var go=Instantiate(wavePrefab,new Vector3(GetComponent<BoxCollider2D>().bounds.min.x-.3f,floorY+tuning.waveHeight*.5f,0),Quaternion.identity);SceneManager.MoveGameObjectToScene(go,gameObject.scene);go.transform.localScale=new Vector3(.6f,tuning.waveHeight,1);wave=go.GetComponent<BossWave>();wave.speed=tuning.waveSpeed;wave.damage=tuning.damage;wave.leftWallX=leftWallX;WavesSpawned++;SetPhase(Phase.WaveRecovery);}
  else if(State==Phase.WaveRecovery&&elapsed>=tuning.recovery)SetPhase(Phase.MeleeTell);
 }
 public void ReceiveHit(int damage,Vector2 source){if(!Fighting||HP<=0||damage<=0)return;HP=Mathf.Max(0,HP-damage);Hits++;if(HP==0){SetPhase(Phase.Defeated);SetLock(false);if(wave!=null)Destroy(wave.gameObject);wave=null;warning.enabled=false;Defeated?.Invoke();}}
 public bool DeferDeath(){if(resolvingDeath)return false;if(State==Phase.Defeated)return true;if(!Fighting)return false;if(!deathQueued){deathQueued=true;}return true;}
 void LateUpdate(){if(!deathQueued)return;if(State==Phase.Defeated){deathQueued=false;return;}resolvingDeath=true;deathQueued=false;RoomSession.Instance.Die();}
 void Contact(Collision2D c){if(Fighting&&!deathQueued)c.collider.GetComponent<PlayerHealth>()?.TakeDamage(tuning.damage,transform.position);}
 void OnCollisionEnter2D(Collision2D c)=>Contact(c);
 void OnCollisionStay2D(Collision2D c)=>Contact(c);
 void OnGUI(){if(State==Phase.Idle)return;float x=Screen.width*.5f-200;GUI.Box(new Rect(x,30,400,28),$"정상 수문장  {HP}/{tuning.maxHP}");GUI.Box(new Rect(x,60,400f*HP/tuning.maxHP,8),"");}
}

