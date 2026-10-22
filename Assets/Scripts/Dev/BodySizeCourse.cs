#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

public sealed class BodySizeCourse:MonoBehaviour
{
 void Update()
 {
  foreach(var e in GetComponentsInChildren<EnemyReset>())if(e.Live!=null&&e.Live.TryGetComponent<EnemyPatrol>(out var patrol)){patrol.enabled=false;var body=e.Live.GetComponent<Rigidbody2D>();body.linearVelocity=new Vector2(0,body.linearVelocity.y);}
 }
}
#endif
