using System.Linq;
using UnityEngine;
public static class B09LegacyMode
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(!N5Args.Has("-v2")||V4SceneMap.Enabled||V3SceneMap.Enabled)return;var go=new GameObject("B09 explicit V2 comparison");go.SetActive(false);go.AddComponent<V2SceneMap>().rooms=V4SceneMap.Rooms.Take(18).ToArray();go.SetActive(true);}
}
