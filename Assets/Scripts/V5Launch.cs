#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Development fixtures use the existing save schema and caller-supplied slots.
public sealed class V5Launch:MonoBehaviour
{
 static string Value(string prefix,string fallback){foreach(string arg in Environment.GetCommandLineArgs())if(arg.StartsWith(prefix,StringComparison.Ordinal))return arg.Substring(prefix.Length);return fallback;}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(V4SceneMap.V5)new GameObject("V5 explicit launch").AddComponent<V5Launch>();}
 IEnumerator Start()
 {
  DontDestroyOnLoad(gameObject);yield return null;
  if(!N5Args.Has("-save-path"))throw new ArgumentException("V5 requires an explicit isolated -save-path");
  string room=Value("-start-room=","A01"),give=Value("-give=","");
  if(!Array.Exists(V4SceneMap.Rooms,id=>id==room))throw new ArgumentException("Unknown V5 room "+room);
  if(!ProgressSave.NewGame())yield break;
  var data=new ProgressData{checkpointId=room=="A03"?"CP-A03":"CP-A01",dash=give.Contains("dash"),doubleJump=give.Contains("dj")||give.Contains("double"),wallJump=give.Contains("wall"),hasSlam=give.Contains("slam")};
  if(!ProgressSave.Write(data)||!ProgressSave.Continue())yield break;
  SceneManager.LoadScene(V4SceneMap.Path(room));Debug.Log("V5 launch room="+room+" give="+give+" layout="+WorldMapLayout.Name);Destroy(gameObject);
 }
}
#endif
