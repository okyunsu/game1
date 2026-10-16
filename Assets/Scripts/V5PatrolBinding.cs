#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

// Endpoints belong to the authored Scene; each EnemyReset clone reuses them.
[RequireComponent(typeof(EnemyReset))]
public sealed class V5PatrolBinding:MonoBehaviour
{
 [Tooltip("Left patrol endpoint in Scene world units; adjust this Transform after Play.")] public Transform leftPoint;
 [Tooltip("Right patrol endpoint in Scene world units; adjust this Transform after Play.")] public Transform rightPoint;
 EnemyReset reset;
 GameObject bound;
 public bool Connected=>bound!=null&&bound==reset.Live;
 void Awake(){reset=GetComponent<EnemyReset>();}
 void Start()=>Bind();
 void LateUpdate()=>Bind();
 void Bind()
 {
  if(reset.Live==null||reset.Live==bound)return;
  if(reset.Live.TryGetComponent<EnemyPatrol>(out var patrol)){patrol.leftPoint=leftPoint;patrol.rightPoint=rightPoint;bound=reset.Live;}
 }
}
#endif
