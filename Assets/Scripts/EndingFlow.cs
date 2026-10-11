using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BossEncounter))]
public sealed class EndingFlow:MonoBehaviour
{
 [Min(0),Tooltip("Summit light duration, real seconds.")]public float lightDuration=2;
 bool lighting;double began;
 public double LightStartedAt=>began;
 void OnEnable()=>GetComponent<BossEncounter>().Defeated+=Victory;
 void OnDisable()=>GetComponent<BossEncounter>().Defeated-=Victory;
 void Victory(){if(V3SceneMap.Enabled||V4SceneMap.Enabled||lighting)return;lighting=true;began=Time.realtimeSinceStartupAsDouble;ProgressSave.MarkBossDefeated(RoomSession.Instance);GameState.GetOrCreate().SetCompleting(true);StartCoroutine(Finish());}
 IEnumerator Finish(){yield return new WaitForSecondsRealtime(lightDuration);SceneManager.LoadScene("Assets/Scenes/Ending.unity");}
 void OnGUI(){if(!lighting)return;var prior=GUI.color;float amount=lightDuration<=0?1:Mathf.Clamp01((float)(Time.realtimeSinceStartupAsDouble-began)/lightDuration);GUI.color=new Color(1,1,1,amount*.8f);GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);GUI.color=prior;GUI.Label(new Rect(Screen.width*.5f-150,Screen.height*.4f,400,50),"정상의 빛이 켜집니다.");if(!string.IsNullOrEmpty(ProgressSave.Message))GUI.Label(new Rect(20,Screen.height-50,700,40),ProgressSave.Message);}
}

