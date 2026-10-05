using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth))]
[DefaultExecutionOrder(100)] // Down hits follow Motor gravity and precede the physics simulation.
public sealed class PlayerAttack:MonoBehaviour
{
    [Tooltip("Shared attack Prototype Values.")] public CombatTuning tuning;
    public bool Attacking { get; private set; }
    public uint Sequence { get; private set; }
    public int VerticalDirection { get; private set; }
    public int PogosPerformed { get; private set; }
    public double LastPogoAt { get; private set; }
    bool pogoApplied;
    PlayerInputReader input;PlayerMotor motor;PlayerHealth health;BoxCollider2D shape;
    double started=double.NegativeInfinity;int direction=1;
    readonly HashSet<IDamageable> damaged=new();
    readonly Collider2D[] contacts=new Collider2D[32];
    void Awake(){input=GetComponent<PlayerInputReader>();motor=GetComponent<PlayerMotor>();health=GetComponent<PlayerHealth>();shape=GetComponent<BoxCollider2D>();}
    void OnEnable(){input.AttackPressed+=Begin;input.DashPressed+=DashCancel;input.Cleared+=Cancel;health.Hit+=Cancel;}
    void OnDisable(){input.AttackPressed-=Begin;input.DashPressed-=DashCancel;input.Cleared-=Cancel;health.Hit-=Cancel;Cancel();}
    void Begin(){if(input.InputLocked||health.HP<=0||health.KnockbackLocked||GetComponent<PlayerDash>()?.IsDashing==true||Time.timeAsDouble-started<tuning.attackCooldown)return;started=Time.timeAsDouble;direction=Mathf.Abs(input.Move.x)>.01f?(input.Move.x>0?1:-1):motor.Facing;VerticalDirection=input.Move.y>.5f?1:input.Move.y<-.5f&&!motor.Grounded?-1:0;pogoApplied=false;damaged.Clear();Attacking=true;Sequence++;}
    void DashCancel(){if(GetComponent<PlayerDash>()?.hasDash==true)Cancel();}
    public void Cancel(){Attacking=false;damaged.Clear();GetComponent<AttackFeedback>()?.Hide();}
    public Vector2 HitboxSize=>VerticalDirection<0?new Vector2(tuning.downAttackWidth,tuning.downAttackHeight):VerticalDirection>0?new Vector2(tuning.verticalAttackWidth,tuning.verticalAttackHeight):new Vector2(tuning.attackRange,tuning.attackHeight);
    public Vector2 HitboxCenter{get{var bounds=shape.bounds;if(VerticalDirection<0)return new Vector2(bounds.center.x,bounds.min.y-tuning.downAttackHeight*.5f);if(VerticalDirection>0)return new Vector2(bounds.center.x,bounds.max.y+tuning.verticalAttackHeight*.5f);return new Vector2((direction>0?bounds.max.x:bounds.min.x)+direction*tuning.attackRange*.5f,bounds.center.y);}}
    void Update(){if(VerticalDirection>=0)TickAttack(false);}
    void FixedUpdate(){if(VerticalDirection<0)TickAttack(true);}
    void TickAttack(bool down){if(!Attacking)return;if(input.InputLocked||health.HP<=0||health.KnockbackLocked||GetComponent<PlayerDash>()?.IsDashing==true){Cancel();return;}double elapsed=Time.timeAsDouble-started;if(elapsed>=tuning.attackStartup+tuning.attackActive){Cancel();return;}if(elapsed<tuning.attackStartup)return;
        Vector2 center=HitboxCenter,size=HitboxSize;
        GetComponent<AttackFeedback>()?.Show(center,size);
        int count=Physics2D.OverlapBox(center,size,0,new ContactFilter2D{useTriggers=true},contacts);
        for(int i=0;i<count;i++){var target=contacts[i].GetComponentInParent<IDamageable>();bool hit=target!=null&&damaged.Add(target);if(hit)target.ReceiveHit(tuning.attackDamage,transform.position);if(down&&!pogoApplied&&(hit||contacts[i].GetComponentInParent<PogoTarget>()!=null)){pogoApplied=true;motor.ApplyPogo(tuning.pogoVelocity,tuning.pogoRefreshesAir);if(tuning.pogoRefreshesAir)GetComponent<PlayerDash>()?.RefreshAirFromPogo();PogosPerformed++;LastPogoAt=Time.timeAsDouble;Debug.Log("Pogo sequence="+Sequence+" vy="+tuning.pogoVelocity);}}
    }
    void OnDrawGizmosSelected(){if(tuning==null)return;shape=GetComponent<BoxCollider2D>();if(shape==null)return;Gizmos.color=Attacking?Color.yellow:Color.white;Gizmos.DrawWireCube(HitboxCenter,new Vector3(HitboxSize.x,HitboxSize.y,.1f));}
}
