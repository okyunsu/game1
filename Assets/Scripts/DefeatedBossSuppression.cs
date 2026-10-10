using UnityEngine;

// Scene opt-in only: absent from production/default rooms.
[DefaultExecutionOrder(-1000)]
public sealed class DefeatedBossSuppression:MonoBehaviour
{
    void Awake()
    {
        if(!ProgressSave.BossDefeated)return;
        foreach(var root in gameObject.scene.GetRootGameObjects())
            foreach(var boss in root.GetComponentsInChildren<BossEncounter>(true))
            {
                if(boss.leftExit!=null)boss.leftExit.enabled=true;
                if(boss.warning!=null)boss.warning.enabled=false;
                boss.gameObject.SetActive(false);
            }
    }
}
