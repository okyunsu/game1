using UnityEngine;

// Added only to B07 build output. The previous V2 candidate remains its default mode.
public sealed class B07CandidateMapping:MonoBehaviour
{
 void Awake()
 {
  if(V3SceneMap.Enabled)return;
  var go=new GameObject("V2 mapping");go.SetActive(false);go.AddComponent<V2SceneMap>().rooms=new[]{"A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06"};go.SetActive(true);
 }
}
