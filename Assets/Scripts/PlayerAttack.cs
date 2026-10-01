using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth))]
public sealed class PlayerAttack:MonoBehaviour
{
    [Tooltip("Shared attack Prototype Values.")] public CombatTuning tuning;
    public bool Attacking { get; private set; }
    public uint Sequence { get; private set; }
    PlayerInputReader input;PlayerMotor motor;PlayerHealth health;BoxCollider2D shape;
    double started=double.NegativeInfinity;int direction=1;
    readonly HashSet<IDamageable> damaged=new();
    readonly Collider2D[] contacts=new Collider2D[32];
    void Awake(){input=GetComponent<PlayerInputReader>();motor=GetComponent<PlayerMotor>();health=GetComponent<PlayerHealth>();shape=GetComponent<BoxCollider2D>();}
    void OnEnable(){input.AttackPressed+=Begin;input.DashPressed+=DashCancel;input.Cleared+=Cancel;health.Hit+=Cancel;}
    void OnDisable(){input.AttackPressed-=Begin;input.DashPressed-=DashCancel;input.Cleared-=Cancel;health.Hit-=Cancel;Cancel();}
    void Begin(){if(input.InputLocked||health.HP<=0||health.KnockbackLocked||GetComponent<PlayerDash>()?.IsDashing==true||Time.timeAsDouble-started<tuning.attackCooldown)return;started=Time.timeAsDouble;direction=Mathf.Abs(input.Move.x)>.01f?(input.Move.x>0?1:-1):motor.Facing;damaged.Clear();Attacking=true;Sequence++;}
    void DashCancel(){if(GetComponent<PlayerDash>()?.hasDash==true)Cancel();}
    public void Cancel(){Attacking=false;damaged.Clear();}
    Vector2 Center(){var bounds=shape.bounds;return new Vector2((direction>0?bounds.max.x:bounds.min.x)+direction*tuning.attackRange*.5f,bounds.center.y);}
    void Update(){if(!Attacking)return;if(input.InputLocked||health.KnockbackLocked||GetComponent<PlayerDash>()?.IsDashing==true){Cancel();return;}double elapsed=Time.timeAsDouble-started;if(elapsed>=tuning.attackStartup+tuning.attackActive){Cancel();return;}if(elapsed<tuning.attackStartup)return;
        int count=Physics2D.OverlapBox(Center(),new Vector2(tuning.attackRange,tuning.attackHeight),0,new ContactFilter2D{useTriggers=true},contacts);
        for(int i=0;i<count;i++){var target=contacts[i].GetComponentInParent<IDamageable>();if(target!=null&&damaged.Add(target))target.ReceiveHit(tuning.attackDamage,transform.position);}
    }
    void OnDrawGizmosSelected(){if(tuning==null)return;shape=GetComponent<BoxCollider2D>();if(shape==null)return;Gizmos.color=Attacking?Color.yellow:Color.white;Gizmos.DrawWireCube(Center(),new Vector3(tuning.attackRange,tuning.attackHeight,.1f));}
}
