using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public sealed class EnemyShot:MonoBehaviour
{
    public int damage;public float speed;public int direction;
    public static string LastWallHit{get;private set;}
    readonly HashSet<Collider2D> launchSurface=new();
    void Start(){var bounds=GetComponent<BoxCollider2D>().bounds;foreach(var c in Physics2D.OverlapBoxAll(bounds.center,bounds.size,0,1<<6))launchSurface.Add(c);GetComponent<Rigidbody2D>().linearVelocity=Vector2.right*direction*speed;}
    void OnTriggerExit2D(Collider2D c)=>launchSurface.Remove(c);
    void OnTriggerEnter2D(Collider2D c){if(launchSurface.Contains(c))return;if(c.gameObject.layer==6){LastWallHit=c.name;Destroy(gameObject);return;}var hp=c.GetComponentInParent<PlayerHealth>();if(hp!=null){hp.TakeDamage(damage,transform.position);Destroy(gameObject);}}
}
