#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B21ArenaLaunch:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21ArenaLaunch>("-b21-arena");
 IEnumerator Start(){yield return null;ProgressSave.Pending=null;ProgressSave.Enabled=false;SceneManager.LoadScene("Assets/Scenes/Test/B21Arena.unity");Destroy(gameObject);}
}
#endif
