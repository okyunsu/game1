using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public sealed class PlayerDash:MonoBehaviour
{
    [Tooltip("Test/ability ownership. Default false; enabled only in DashTest.")] public bool hasDash;
    public bool IsDashing { get; private set; }
    public bool AirUsed { get; private set; }
    PlayerInputReader input;PlayerMotor motor;Rigidbody2D body;
    bool pending;int direction;double started=double.NegativeInfinity;
    readonly RaycastHit2D[] hits=new RaycastHit2D[8];
    void Awake(){input=GetComponent<PlayerInputReader>();motor=GetComponent<PlayerMotor>();body=GetComponent<Rigidbody2D>();}
    void OnEnable(){input.DashPressed+=Request;input.Cleared+=Cancel;}
    void OnDisable(){input.DashPressed-=Request;input.Cleared-=Cancel;Cancel();}
    void Request(){var health=GetComponent<PlayerHealth>();if(hasDash&&!input.InputLocked&&(health==null||!health.KnockbackLocked))pending=true;}
    public void Cancel(){pending=false;IsDashing=false;DashGate.Apply(this);if(body!=null){body.gravityScale=0;body.linearVelocity=new Vector2(0,0);}}
    public bool Tick(bool grounded,double now)
    {
        if(grounded&&!IsDashing)AirUsed=false;
        if(input.InputLocked){if(IsDashing)Cancel();return false;}
        if(pending){pending=false;if(hasDash&&!IsDashing&&now-started>=motor.Tuning.dashCooldown&&(grounded||!AirUsed)){IsDashing=true;AirUsed=true;started=now;direction=Mathf.Abs(input.Move.x)>.01f?(input.Move.x>0?1:-1):motor.Facing;}}
        DashGate.Apply(this);if(!IsDashing)return false;
        if(!hasDash||now-started>=motor.Tuning.dashDuration-.000001){Cancel();return false;}
        var filter=new ContactFilter2D{useLayerMask=true,layerMask=1<<6,useTriggers=false};
        int count=body.Cast(Vector2.right*direction,filter,hits,motor.Tuning.dashSpeed*Time.fixedDeltaTime+.01f);
        for(int i=0;i<count;i++)if(hits[i].collider.GetComponent<DashGate>()==null&&Mathf.Abs(hits[i].normal.x)>.7f){Cancel();return false;}
        body.linearVelocity=new Vector2(direction*motor.Tuning.dashSpeed,0);return true;
    }
}
