using UnityEngine;
// Only display children; the vent's trigger and physics stay untouched.
[DefaultExecutionOrder(700)]
public sealed class B26SteamVisual:MonoBehaviour
{
 [Range(0,1),Tooltip("Steam column display opacity while on.")]public float opacity=.25f;
 [Min(.1f),Tooltip("Visual particle rise speed in world units per second.")]public float riseSpeed=1.4f;
 SpriteRenderer[] motes;SteamVent vent;Bounds area;
 void Start()
 {
  vent=GetComponent<SteamVent>();area=GetComponent<Collider2D>().bounds;motes=new SpriteRenderer[12];
  for(int i=0;i<motes.Length;i++){var go=new GameObject("Steam rising particle");go.transform.SetParent(transform);go.transform.localScale=new Vector3(.12f/transform.lossyScale.x,.12f/transform.lossyScale.y,1);var sr=go.AddComponent<SpriteRenderer>();sr.sprite=GetComponent<SpriteRenderer>().sprite;sr.sortingOrder=2;motes[i]=sr;}
 }
 void LateUpdate()
 {
  if(vent==null)return;bool gray=N5Args.Has("-v5-graybox");var sr=GetComponent<SpriteRenderer>();if(!gray){var c=sr.color;c.a=vent.On?opacity:opacity*.2f;sr.color=c;}
  for(int i=0;i<motes.Length;i++){float t=Mathf.Repeat(Time.time*riseSpeed+i*area.size.y/motes.Length,area.size.y);motes[i].transform.position=new Vector3(area.min.x+area.size.x*(.2f+.6f*Mathf.Repeat(i*.618f,1)),area.min.y+t,0);motes[i].color=new Color(.81f,.89f,.93f,opacity*(1-t/area.size.y));motes[i].enabled=!gray&&vent.On;}
 }
}
