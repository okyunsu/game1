using UnityEngine;
public sealed class V4BossReward:MonoBehaviour
{
 public BossEncounter boss;public GameObject[] roof;public RoomExit upperExit;
 void OnEnable(){boss.Defeated+=Victory;}
 void OnDisable(){boss.Defeated-=Victory;}
 void Start(){if(ProgressSave.BossDefeated){Open();boss.gameObject.SetActive(false);}}
 void Victory(){ProgressSave.HasSlam=true;RoomSession.Instance.Player.GetComponent<PlayerSlam>().hasSlam=true;ProgressSave.MarkBossDefeated(RoomSession.Instance);Open();}
 void Open(){foreach(var go in roof)go.SetActive(false);upperExit.enabled=true;}
}
