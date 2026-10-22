#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BodySizeBootstrap:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){var go=new GameObject("Development body comparison");DontDestroyOnLoad(go);go.AddComponent<BodySizeBootstrap>();}
 IEnumerator Start()
 {
  yield return null;
  if(N5Args.Has("-body-size-test")){ProgressSave.Pending=null;ProgressSave.Enabled=false;SceneManager.LoadScene("Assets/Scenes/Test/BodySizeTest.unity");}
 }
 void Update()
 {
  bool course=SceneManager.GetActiveScene().name=="BodySizeTest";
  if(!course&&!N5Args.Has("-body-height"))return;
  var player=RoomSession.Instance?.Player;if(player==null||player.GetComponent<BodySizeOverride>()!=null)return;
  var controller=player.gameObject.AddComponent<BodySizeOverride>();
  if(N5Args.Has("-body-height")){if(!float.TryParse(N5Args.Value("-body-height","1.6"),System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out float height))throw new ArgumentException("Invalid body height");controller.SelectHeight(height);}
 }
}
#endif
