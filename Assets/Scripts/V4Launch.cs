using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class V4Launch:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(V4SceneMap.Enabled&&(N5Args.Has("-v4-mechanics")||N5Args.Has("-v4-new-game")||N5Args.Has("-v4-afterboss")))new GameObject("V4 explicit launch").AddComponent<V4Launch>();}
 IEnumerator Start(){yield return null;if(N5Args.Has("-v4-mechanics")){SceneManager.LoadScene("Assets/Scenes/Test/MechanicsTest.unity");yield break;}if(N5Args.Has("-v4-afterboss")){ProgressSave.Write(new ProgressData{dash=true,doubleJump=true,wallJump=true,hasSlam=true,bossDefeated=true,endingReached=true,checkpointId="CP-A01"});if(!ProgressSave.Continue())yield break;}else if(!ProgressSave.NewGame())yield break;SceneManager.LoadScene(ProgressSave.PendingScene);}
}

