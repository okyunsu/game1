using UnityEngine;

// A small opt-in observer for the existing input fight runner; no combat writes.
public sealed class B02BossStats:MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()=>N5Args.Start<B02BossStats>("-b02-boss-stats");
    BossEncounter observed;PlayerHealth health;double started;int hits;bool active,reported;
    void Hit(){if(active)hits++;}
    void Update()
    {
        var player=RoomSession.Instance?.Player;
        if(player!=null&&health==null){health=player.GetComponent<PlayerHealth>();health.Hit+=Hit;}
        var boss=FindFirstObjectByType<BossEncounter>();
        if(boss==null){observed=null;active=false;return;}
        if(boss!=observed){observed=boss;active=false;reported=false;hits=0;}
        if(!active&&boss.Fighting){active=true;started=Time.realtimeSinceStartupAsDouble;}
        if(active&&!reported&&boss.State==BossEncounter.Phase.Defeated)
        {
            reported=true;active=false;
            Debug.Log($"MEASURE B02 basic boss Editor input victory realSeconds={Time.realtimeSinceStartupAsDouble-started:F3} hitsReceived={hits}");
        }
    }
    void OnDestroy(){if(health!=null)health.Hit-=Hit;}
}
