using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
// Photography tool only. Inert without -capture; no ordinary-play state changes.
public sealed class Q7ExeCapture:MonoBehaviour
{
 string requested,folder;MethodInfo capture;readonly List<string> lines=new(),errors=new();
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize(){if(Application.isEditor)return;var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,"-capture");if(i<0||i+1>=a.Length)return;int j=Array.IndexOf(a,"-capture-output");var go=new GameObject("Opt-in exe photography");DontDestroyOnLoad(go);var r=go.AddComponent<Q7ExeCapture>();r.requested=a[i+1];r.folder=j>=0&&j+1<a.Length?Path.GetFullPath(a[j+1]):Path.GetFullPath("Validation/Q7-shots");}
 void Log(string m,string s,LogType t){if(t is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(m+s);}
 IEnumerator Start(){Application.logMessageReceived+=Log;Directory.CreateDirectory(folder);var type=Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule",false);capture=type?.GetMethod("CaptureScreenshot",new[]{typeof(string),typeof(int)});
  var stack=new Stack<IEnumerator>();stack.Push(Photos());bool failed=false;
  while(stack.Count>0&&!failed){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){failed=true;lines.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!failed)yield return next;}
  Application.logMessageReceived-=Log;PlaytestLogger.Instance?.Finish();failed|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,failed?"KNOWN-LIMIT Q7 actual exe photography":"PASS Q7 actual exe photography");lines.Add("Error/Exception/Assert="+errors.Count);string evidence=Path.Combine(folder,$"capture-{Screen.width}x{Screen.height}.txt");if(!File.Exists(evidence))File.WriteAllLines(evidence,lines);Application.Quit(failed?1:0);
 }
 IEnumerator Until(Func<bool> test,float seconds=8){double end=Time.realtimeSinceStartupAsDouble+seconds;while(!test()){if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("Capture setup timeout");yield return null;}}
 IEnumerator Ready(){yield return Until(()=>RoomSession.Instance?.Player!=null&&!RoomSession.Instance.Transitioning&&!RoomSession.Instance.Respawning);yield return null;}
 void Place(Vector2 p){var input=RoomSession.Instance.Player;input.ClearTransientInput();input.transform.position=p;var body=input.GetComponent<Rigidbody2D>();body.position=p;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Enter(string id,string spawn="Entry"){if(!RoomSession.Instance.RequestTransition(id,"Assets/Scenes/"+id+".unity",spawn))throw new Exception("Capture transition rejected "+id);yield return Ready();}
 IEnumerator Shot(string name){if(requested!="all"&&requested!=name)yield break;string path=Path.Combine(folder,$"{Screen.width}x{Screen.height}-{name}.png");if(File.Exists(path))throw new IOException("Preserve shot "+path);yield return new WaitForEndOfFrame();capture.Invoke(null,new object[]{path,1});yield return Until(()=>File.Exists(path)&&new FileInfo(path).Length>0,10);lines.Add("CAPTURE "+path+" actual="+Screen.width+"x"+Screen.height+" scene="+SceneManager.GetActiveScene().name);}
 IEnumerator Photos(){if(capture==null)throw new NotSupportedException("ScreenCapture.CaptureScreenshot runtime module/method unavailable. Reflection avoids missing Editor compile references; no package modifications.");yield return new WaitForSecondsRealtime(.3f);var menu=FindFirstObjectByType<SaveMenu>();if(menu==null)throw new Exception("Capture Menu missing");if(requested=="ending"&&menu.CanContinue){menu.Submit();yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null);yield return Shot("ending");lines.Add("Observed Continue from saved ending into EndingMenu in a fresh actual exe process.");yield break;}if(menu.CanContinue)throw new IOException("Photography requires fresh isolated save slot");menu.Submit();yield return Ready();
  yield return Shot("A01-start");Place(new Vector2(8,1.81f));yield return new WaitForSecondsRealtime(.1f);if(!RoomSession.Instance.Player.GetComponent<PlayerHealth>().TakeDamage(1,new Vector2(5,1)))throw new Exception("Capture hit rejected");yield return Shot("after-hit");
  yield return Enter("A05");Place(FindFirstObjectByType<DashPickup>().transform.position);yield return new WaitForFixedUpdate();yield return null;
  yield return Enter("B04");Place(FindFirstObjectByType<DoubleJumpPickup>().transform.position);yield return new WaitForFixedUpdate();yield return null;yield return Shot("both-abilities");
  if(!RoomSession.Instance.Die())throw new Exception("Capture death rejected");yield return null;yield return Shot("death-overlay");yield return Ready();
  yield return Enter("C04","FromLeft");yield return Enter("C06","FromLeft");Place(new Vector2(8,1.81f));var boss=FindFirstObjectByType<BossEncounter>();yield return Until(()=>boss.State==BossEncounter.Phase.MeleeTell);yield return Shot("boss-tell");boss.ReceiveHit(20,RoomSession.Instance.Player.transform.position);yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null);yield return Shot("ending");
  lines.Add("Prescribed capture placements; not natural completion or readability acceptance. Screenshot API invoked inside actual exe after rendered frame.");
 }
}
