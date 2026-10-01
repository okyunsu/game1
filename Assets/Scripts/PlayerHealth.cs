using System;
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public sealed class PlayerHealth:MonoBehaviour
{
    [Tooltip("Shared HP, invincibility and knockback Prototype Values.")] public CombatTuning tuning;
    public int HP { get; private set; }
    public bool KnockbackLocked=>Time.timeAsDouble<lockUntil;
    public bool Invincible=>Time.timeAsDouble<invincibleUntil;
    public event Action Hit;
    public event Action Restored;
    double lockUntil,invincibleUntil;
    Rigidbody2D body;SpriteRenderer sprite;Color original;
    void Awake(){body=GetComponent<Rigidbody2D>();sprite=GetComponent<SpriteRenderer>();original=sprite.color;HP=tuning.playerMaxHP;}
    public bool TakeDamage(int damage,Vector2 source)
    {
        if(damage<=0||HP<=0||Invincible||GetComponent<PlayerInputReader>().InputLocked)return false;
        HP=Mathf.Max(0,HP-damage);invincibleUntil=Time.timeAsDouble+tuning.invincibility;lockUntil=Time.timeAsDouble+tuning.knockbackLock;
        GetComponent<PlayerDash>()?.Cancel();GetComponent<PlayerInputReader>().ClearTransientInput();Hit?.Invoke();
        int direction=transform.position.x>=source.x?1:-1;
        body.linearVelocity=new Vector2(direction*tuning.knockbackX,tuning.knockbackY);
        if(HP==0)RoomSession.Instance.Die();return true;
    }
    public void MarkDead(){HP=0;lockUntil=invincibleUntil=0;Hit?.Invoke();}
    public void RestoreMax(){HP=tuning.playerMaxHP;lockUntil=invincibleUntil=0;sprite.color=original;Restored?.Invoke();}
    void Update(){var color=original;if(Invincible&&((int)(Time.time*10)%2==0))color.a=.25f;sprite.color=color;}
    void OnGUI(){GUI.Label(new Rect(16,76,200,24),$"HP {HP}/{tuning.playerMaxHP}");}
}
