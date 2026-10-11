using UnityEngine;
using UnityEngine.InputSystem;
public sealed class MechanicsTestPresets:MonoBehaviour
{
 public MechanicsTuning baseline;
 MechanicsTuning candidate;int index;
 void Update(){if(Keyboard.current?.fKey.wasPressedThisFrame==true)Select((index+1)%3);}
 public void Select(int value){index=value;if(candidate!=null)Destroy(candidate);candidate=Instantiate(baseline);if(index>0){candidate.steamOn=candidate.steamOff=index==1?1.5f:2.5f;candidate.crumbleDelay=index==1?.4f:.8f;candidate.liftSpeed=4;}
 foreach(var device in GetComponentsInChildren<SteamVent>())device.tuning=candidate;foreach(var device in GetComponentsInChildren<CrumblePlatform>())device.tuning=candidate;foreach(var device in GetComponentsInChildren<LeverLift>())device.tuning=candidate;}
 void OnDestroy(){if(candidate!=null)Destroy(candidate);}
 void OnGUI(){GUI.Label(new Rect(16,110,1000,35),"F 장치 후보 "+index+" | "+(index==0?"기본 증기2/2s · 무너짐.6s · 승강기3u/s":index==1?"증기1.5/1.5s · 무너짐.4s · 승강기4u/s":"증기2.5/2.5s · 무너짐.8s · 승강기4u/s"));}
}
