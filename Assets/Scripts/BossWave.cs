using UnityEngine;
public sealed class BossWave:MonoBehaviour
{
 public float speed;public int damage;
 public float direction=-1;public float rightWallX=19;public BossEncounter candidateOwner;public int attackToken;
 [Tooltip("Scene left wall inner face (world x, u).")]public float leftWallX=1;
 void FixedUpdate(){transform.position+=Vector3.right*direction*speed*Time.fixedDeltaTime;if(direction<0?transform.position.x+GetComponent<BoxCollider2D>().bounds.extents.x<=leftWallX:transform.position.x-GetComponent<BoxCollider2D>().bounds.extents.x>=rightWallX)Destroy(gameObject);}
 void OnTriggerEnter2D(Collider2D c)=>Touch(c);
 void OnTriggerStay2D(Collider2D c)=>Touch(c);
 void Touch(Collider2D c){var boss=FindFirstObjectByType<BossEncounter>();if(boss==null||!boss.Fighting)return;var health=c.GetComponent<PlayerHealth>();if(health==null)return;if(candidateOwner!=null)candidateOwner.TryPatternDamage(health,attackToken,transform.position);else health.TakeDamage(damage,transform.position);}
}
