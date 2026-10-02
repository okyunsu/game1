using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public sealed class EnemyPatrol : MonoBehaviour, IDamageable
{
    public EnemyTuning tuning;
    [Header("Scene patrol endpoints (world positions)")]
    public Transform leftPoint, rightPoint;
    public int HP { get; private set; }
    public int Direction { get; private set; } = 1;
    public int Turns { get; private set; }
    public Vector2 LastHitVelocity { get; private set; }
    public int LastHitHPBefore { get; private set; }
    Rigidbody2D body;
    BoxCollider2D shape;
    bool hitPending;
    readonly RaycastHit2D[] hits = new RaycastHit2D[8];
    void Awake() { body=GetComponent<Rigidbody2D>(); shape=GetComponent<BoxCollider2D>(); HP=tuning.maxHP; }
    void FixedUpdate()
    {
        if(HP<=0)return;
        // Preserve the impulse for a physics step before resuming patrol.
        if(hitPending){hitPending=false;return;}
        var bounds=shape.bounds;
        float edge=Direction>0?bounds.max.x:bounds.min.x;
        bool end=leftPoint!=null&&rightPoint!=null&&(Direction>0?body.position.x>=rightPoint.position.x:body.position.x<=leftPoint.position.x);
        var filter=new ContactFilter2D(); filter.SetLayerMask(1<<6); filter.useTriggers=false;
        bool wall=body.Cast(Vector2.right*Direction,filter,hits,tuning.speed*Time.fixedDeltaTime+.04f)>0;
        bool floor=Physics2D.Raycast(new Vector2(edge+Direction*.08f,bounds.min.y+.08f),Vector2.down,.25f,1<<6).collider!=null;
        if(end||wall||!floor){Direction=-Direction;Turns++;}
        body.linearVelocity=new Vector2(Direction*tuning.speed,body.linearVelocity.y);
    }
    public void ReceiveHit(int damage, Vector2 source)
    {
        if(HP<=0||damage<=0)return;
        LastHitHPBefore=HP;HP=Mathf.Max(0,HP-damage);
        LastHitVelocity=new Vector2((body.position.x>=source.x?1:-1)*tuning.knockback,body.linearVelocity.y);
        body.linearVelocity=LastHitVelocity;hitPending=true;
        if(HP==0)Destroy(gameObject);
    }
    void Contact(Collision2D collision){collision.collider.GetComponentInParent<PlayerHealth>()?.TakeDamage(tuning.contactDamage,body.position);}
    void OnCollisionEnter2D(Collision2D collision)=>Contact(collision);
    void OnCollisionStay2D(Collision2D collision)=>Contact(collision);
    void OnTriggerEnter2D(Collider2D other){if(other.GetComponentInParent<KillZone>()!=null)Destroy(gameObject);}
    void OnDrawGizmosSelected(){if(leftPoint==null||rightPoint==null)return;Gizmos.color=Color.red;Gizmos.DrawLine(leftPoint.position,rightPoint.position);}
}
