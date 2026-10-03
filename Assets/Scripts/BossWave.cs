using UnityEngine;
public sealed class BossWave:MonoBehaviour
{
 public float speed;public int damage;
 [Tooltip("Scene left wall inner face (world x, u).")]public float leftWallX=1;
 void FixedUpdate(){transform.position+=Vector3.left*speed*Time.fixedDeltaTime;if(transform.position.x+GetComponent<BoxCollider2D>().bounds.extents.x<=leftWallX)Destroy(gameObject);}
 void OnTriggerEnter2D(Collider2D c)=>Touch(c);
 void OnTriggerStay2D(Collider2D c)=>Touch(c);
 void Touch(Collider2D c){var boss=FindFirstObjectByType<BossEncounter>();if(boss==null||!boss.Fighting)return;c.GetComponent<PlayerHealth>()?.TakeDamage(damage,transform.position);}
}
