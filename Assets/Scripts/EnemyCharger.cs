using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D),typeof(SpriteRenderer))]
public sealed class EnemyCharger:MonoBehaviour,IDamageable
{
    public enum Phase{Idle,Warning,Charge,Recovery}
    [Tooltip("Shared E2 values.")]public E2Tuning tuning;
    [Range(-1,1),Tooltip("Initial facing; -1 left, +1 right.")]public int initialDirection=-1;
    public Phase State{get;private set;}
    public int Direction{get;private set;}
    public int HP{get;private set;}
    public float PhaseElapsed{get;private set;}
    public Vector2 LastHitVelocity{get;private set;}
    Rigidbody2D body;SpriteRenderer sprite;Color original;bool hitPending;
    readonly RaycastHit2D[] hits=new RaycastHit2D[8];
    void Awake(){body=GetComponent<Rigidbody2D>();sprite=GetComponent<SpriteRenderer>();original=sprite.color;HP=tuning.maxHP;Direction=initialDirection<0?-1:1;}
    void SetPhase(Phase next){State=next;PhaseElapsed=0;}
    void FixedUpdate(){if(HP<=0)return;if(hitPending){hitPending=false;return;}PhaseElapsed+=Time.fixedDeltaTime;
        var player=RoomSession.Instance?.Player;
        if(State==Phase.Idle&&player!=null&&!player.InputLocked&&Vector2.Distance(body.position,player.transform.position)<=tuning.detectionRange){Direction=player.transform.position.x<body.position.x?-1:1;SetPhase(Phase.Warning);}
        else if(State==Phase.Warning&&PhaseElapsed>=tuning.warningSeconds)SetPhase(Phase.Charge);
        else if(State==Phase.Charge&&PhaseElapsed>=tuning.chargeSeconds)SetPhase(Phase.Recovery);
        else if(State==Phase.Recovery&&PhaseElapsed>=tuning.recoverySeconds)SetPhase(Phase.Idle);
        float speed=State==Phase.Charge?Direction*tuning.chargeSpeed:0;
        var filter=new ContactFilter2D();filter.SetLayerMask(1<<6);filter.useTriggers=false;
        if(speed!=0&&body.Cast(Vector2.right*Direction,filter,hits,Mathf.Abs(speed)*Time.fixedDeltaTime+.02f)>0){speed=0;SetPhase(Phase.Recovery);}
        body.linearVelocity=new Vector2(speed,body.linearVelocity.y);
        sprite.color=State==Phase.Warning?Color.Lerp(original,Color.yellow,Mathf.PingPong(PhaseElapsed*12,1)):original;
    }
    public void ReceiveHit(int damage,Vector2 source){if(HP<=0||damage<=0)return;HP=Mathf.Max(0,HP-damage);LastHitVelocity=new Vector2((body.position.x>=source.x?1:-1)*tuning.knockback,body.linearVelocity.y);body.linearVelocity=LastHitVelocity;hitPending=true;if(HP==0)Destroy(gameObject);}
    void Contact(Collision2D c){c.collider.GetComponentInParent<PlayerHealth>()?.TakeDamage(tuning.contactDamage,body.position);}
    void OnCollisionEnter2D(Collision2D c)=>Contact(c);
    void OnCollisionStay2D(Collision2D c)=>Contact(c);
    void OnTriggerEnter2D(Collider2D c){if(c.GetComponent<KillZone>()!=null)Destroy(gameObject);}
}
