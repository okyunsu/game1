using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public sealed class DamageBlock:MonoBehaviour
{
    [Min(1),Tooltip("Contact damage in HP. Prototype Value: 1.")] public int damage=1;
    void OnTriggerEnter2D(Collider2D other)=>Apply(other);
    void OnTriggerStay2D(Collider2D other)=>Apply(other);
    void Apply(Collider2D other){var health=other.GetComponent<PlayerHealth>();if(health!=null)health.TakeDamage(damage,transform.position);}
    void OnValidate(){GetComponent<BoxCollider2D>().isTrigger=true;}
}
