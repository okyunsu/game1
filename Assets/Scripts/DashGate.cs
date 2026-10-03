using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public sealed class DashGate : MonoBehaviour
{
    static DashGate active;
    Collider2D gate, player;
    void OnEnable(){gate=GetComponent<BoxCollider2D>();active=this;}
    public static void Apply(PlayerDash dash)
    {
        if(active==null)return;
        var collider=dash.GetComponent<Collider2D>();
        if(active.player!=null&&active.player!=collider)Physics2D.IgnoreCollision(active.gate,active.player,false);
        active.player=collider;
        Physics2D.IgnoreCollision(active.gate,collider,dash.hasDash&&dash.IsDashing);
    }
    void OnDisable(){if(gate!=null&&player!=null)Physics2D.IgnoreCollision(gate,player,false);if(active==this)active=null;}
}
