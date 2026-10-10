using UnityEngine;

[DefaultExecutionOrder(-1000)]
public sealed class V3BossDoor:MonoBehaviour
{
 public BossEncounter boss;
 public RoomExit upperExit;
 [Tooltip("Solid door panel, removed after victory.")]public GameObject blocker;
 [Tooltip("Post-victory stairs. Inactive during combat to preserve the original arena.")]public GameObject[] stairs;
 public bool Opened{get;private set;}
 void Awake()
 {
  Apply(ProgressSave.BossDefeated);
  if(ProgressSave.BossDefeated&&boss!=null){if(boss.leftExit!=null)boss.leftExit.enabled=true;if(boss.warning!=null)boss.warning.enabled=false;boss.gameObject.SetActive(false);}
 }
 void OnEnable(){if(boss!=null)boss.Defeated+=Victory;}
 void OnDisable(){if(boss!=null)boss.Defeated-=Victory;}
 void Victory(){ProgressSave.MarkBossDefeated(RoomSession.Instance);Apply(true);}
 void Apply(bool open){Opened=open;if(upperExit!=null)upperExit.enabled=open;if(blocker!=null)blocker.SetActive(!open);foreach(var step in stairs??System.Array.Empty<GameObject>())if(step!=null)step.SetActive(open);}
}
