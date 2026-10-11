using UnityEngine;
public sealed class CrumblePlatform:MonoBehaviour
{
 public MechanicsTuning tuning;
 public bool Falling{get;private set;}
 Vector3 origin;double stood=-1,fell;float velocity;
 void Awake(){origin=transform.position;}
 void OnCollisionStay2D(Collision2D collision){if(stood<0&&!Falling&&collision.collider.GetComponent<PlayerMotor>()!=null&&collision.collider.bounds.min.y>=GetComponent<Collider2D>().bounds.max.y-.1f)stood=Time.timeAsDouble;}
 void FixedUpdate(){if(stood>=0&&!Falling&&Time.timeAsDouble-stood>=tuning.crumbleDelay){Falling=true;fell=Time.timeAsDouble;GetComponent<Collider2D>().enabled=false;}if(!Falling)return;if(Time.timeAsDouble-fell>=tuning.crumbleRestore){transform.position=origin;velocity=0;stood=-1;Falling=false;GetComponent<Collider2D>().enabled=true;return;}velocity+=tuning.crumbleGravity*Time.fixedDeltaTime;transform.position+=Vector3.down*velocity*Time.fixedDeltaTime;}
}
