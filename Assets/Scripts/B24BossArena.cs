using System.Collections;
using UnityEngine;
// The trial checkpoint is outside the trigger in the same scene, so retry resets this boss only.
public sealed class B24BossArena:MonoBehaviour
{
 public BossEncounter boss;
 Vector3 spawn;PlayerHealth health;bool sawDeath;
 IEnumerator Start(){spawn=boss.transform.position;while(RoomSession.Instance?.Player==null)yield return null;health=RoomSession.Instance.Player.GetComponent<PlayerHealth>();health.Restored+=Restored;}
 void Update(){if(health!=null&&health.HP<=0)sawDeath=true;}
 void Restored(){if(!sawDeath)return;sawDeath=false;if(boss!=null&&boss.HP>0&&!ProgressSave.IsBossDefeated(boss.bossId))boss.ResetAfterArenaDeath(spawn);}
 void OnDestroy(){if(health!=null)health.Restored-=Restored;}
}
