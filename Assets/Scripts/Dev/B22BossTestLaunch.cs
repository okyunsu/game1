#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
public sealed class B22BossTestLaunch:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(N5Args.Has("-boss-test-b1"))N5Args.Start<B22BossTestLaunch>("-boss-test-b1");}
 IEnumerator Start()
 {
  DontDestroyOnLoad(gameObject);
  while(RoomSession.Instance?.CurrentRoomId!="A03"||RoomSession.Instance.Transitioning)yield return null;
  yield return null;
  RoomSession.Instance.RequestTransition("A04","Assets/Scenes/Test/BossTest_B1.unity","Entry");Destroy(gameObject);
 }
}
#endif
