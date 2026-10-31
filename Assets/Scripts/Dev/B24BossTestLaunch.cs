#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
public sealed class B24BossTestLaunch:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B24BossTestLaunch>("-boss-test-b2");
 IEnumerator Start()
 {
  yield return null;if(!N5Args.Has("-save-path"))throw new System.ArgumentException("B2 trial requires isolated save path");
  if(N5Args.Has("-boss-test-b2-fresh")||!ProgressSave.TryLoad(out _)){if(!ProgressSave.NewGame())yield break;if(!ProgressSave.Write(new ProgressData{checkpointId="CP-B01",dash=true}))yield break;}
  if(!ProgressSave.Continue())yield break;yield return RoomSession.LoadSceneForPlay("Assets/Scenes/Test/BossTest_B2.unity");Destroy(gameObject);
 }
}
#endif
