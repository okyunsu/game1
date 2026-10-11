using UnityEngine;
public sealed class E4Hover:MonoBehaviour,IDamageable
{
 public MechanicsTuning tuning;
 public int HP{get;private set;}
 Vector3 origin;
 void Awake(){origin=transform.position;HP=tuning.e4HP;}
 void FixedUpdate(){transform.position=origin+Vector3.up*(tuning.e4Amplitude*Mathf.Sin(Time.time*2*Mathf.PI/tuning.e4Period));}
 public void ReceiveHit(int damage,Vector2 source){HP=Mathf.Max(0,HP-damage);if(HP==0)Destroy(gameObject);}
 void OnTriggerStay2D(Collider2D other){other.GetComponent<PlayerHealth>()?.TakeDamage(tuning.e4Damage,transform.position);}
}
