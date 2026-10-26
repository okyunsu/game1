#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

public sealed class B21ArenaCourse:MonoBehaviour
{
 [Min(0),Tooltip("Horizontal opening between the two floor edges, in Unity units.")] public float gapUnits=4.5f;
 public BoxCollider2D landing;
 public void Configure(float gap){gapUnits=gap;landing.transform.position=new Vector2(gap+10,-.5f);Physics2D.SyncTransforms();}
 void OnValidate(){if(landing!=null)Configure(gapUnits);}
}
#endif
