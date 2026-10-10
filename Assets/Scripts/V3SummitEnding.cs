using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class V3SummitEnding:MonoBehaviour
{
 [Min(0),Tooltip("Fade to white duration, real seconds.")]public float lightDuration=2;
 public bool Lighting{get;private set;}
 public double Began{get;private set;}
 void OnTriggerEnter2D(Collider2D other)
 {
  var player=other.GetComponent<PlayerInputReader>();if(Lighting||player==null||player!=RoomSession.Instance?.Player)return;
  Lighting=true;Began=Time.realtimeSinceStartupAsDouble;GameState.GetOrCreate().SetCompleting(true);StartCoroutine(Finish());
 }
 IEnumerator Finish(){yield return new WaitForSecondsRealtime(lightDuration);SceneManager.LoadScene("Assets/Scenes/Ending.unity");}
 void OnGUI()
 {
  if(!Lighting)return;var color=GUI.color;
  GUI.color=new Color(1,1,1,lightDuration<=0?1:Mathf.Clamp01((float)(Time.realtimeSinceStartupAsDouble-Began)/lightDuration));
  GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);GUI.color=color;
  GUI.Label(new Rect(Screen.width*.5f-100,Screen.height*.4f,300,50),"아무것도 없었다.");
 }
 void OnValidate()=>GetComponent<BoxCollider2D>().isTrigger=true;
}
