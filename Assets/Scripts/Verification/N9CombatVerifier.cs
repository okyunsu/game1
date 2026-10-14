#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(200)]
public sealed class N9BossPogoProbe : MonoBehaviour
{
    public BossEncounter boss;public int maximum,windows,current;
    int hits;double phase=double.NaN;bool recovery;
    void FixedUpdate(){if(boss==null)return;if(phase!=boss.PhaseStarted){if(recovery){maximum=Mathf.Max(maximum,current);windows++;}current=0;phase=boss.PhaseStarted;}recovery=boss.State is BossEncounter.Phase.MeleeRecovery or BossEncounter.Phase.WaveRecovery;if(recovery)current+=boss.Hits-hits;hits=boss.Hits;maximum=Mathf.Max(maximum,current);}
}
public sealed class N9CombatVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<N9CombatVerifier>("-verify-directional-enemies");
    N9PogoInput driver;
    protected override void Cleanup(){if(driver!=null)driver.produce=null;if(Body!=null)Body.constraints=RigidbodyConstraints2D.FreezeRotation;}
    IEnumerator Room(string id,string spawn="Entry"){yield return Keys();Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/"+id+".unity",spawn),"fixture room "+id);yield return Ready();yield return new WaitForSecondsRealtime(.2f);}
    int HP(Component c)=>c is EnemyPatrol a?a.HP:c is EnemyCharger b?b.HP:c is EnemyTurret d?d.HP:((BossEncounter)c).HP;
    IEnumerator Strike(Component target,int vertical)
    {
        yield return Keys();yield return new WaitForSecondsRealtime(.4f);var b=target.GetComponent<BoxCollider2D>().bounds;
        PlaceForAuthorizedSetup(new Vector2(b.center.x,vertical>0?b.min.y-1.1f:b.max.y+1.1f));
        yield return new WaitForFixedUpdate();yield return new WaitForFixedUpdate();
        yield return Keys(vertical>0?Key.UpArrow:Key.DownArrow);int before=HP(target),p=Player.GetComponent<PlayerAttack>().PogosPerformed,calls=target.GetComponent<HitFeedback>().Calls;
        yield return Keys(vertical>0?Key.UpArrow:Key.DownArrow,Key.X);yield return new WaitForSecondsRealtime(.22f);
        Check(target==null||HP(target)==Mathf.Max(0,before-Player.GetComponent<PlayerAttack>().tuning.attackDamage),"actual "+target.GetType().Name+" "+(vertical>0?"up":"down")+" damage");
        if(target!=null)Check(target.GetComponent<HitFeedback>().Calls==calls+1,"HitFeedback direction="+vertical);
        if(vertical<0)Check(Player.GetComponent<PlayerAttack>().PogosPerformed==p+1,"enemy down pogo");yield return Keys();
    }
    protected override IEnumerator Tests()
    {
        yield return NewGame();
        foreach(string id in new[]{"A04","B02","B03"})
        {
            yield return Room(id);var enemy=(Component)(id=="A04"?FindFirstObjectByType<EnemyPatrol>():id=="B02"?(Component)FindFirstObjectByType<EnemyCharger>():FindFirstObjectByType<EnemyTurret>());
            Check(enemy!=null,"actual enemy "+id);enemy.GetComponent<BoxCollider2D>().isTrigger=true;if(enemy.GetComponent<Rigidbody2D>()!=null)enemy.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
            ((Behaviour)enemy).enabled=false;Body.constraints=RigidbodyConstraints2D.FreezeAll;int pops=HitFeedback.PopsCreated;
            yield return Strike(enemy,1);while(enemy!=null&&HP(enemy)>0)yield return Strike(enemy,-1);
            Check(HitFeedback.PopsCreated==pops+1,"actual enemy death effect "+id);Body.constraints=RigidbodyConstraints2D.FreezeRotation;
        }
        yield return Room("C06","FromLeft");var boss=FindFirstObjectByType<BossEncounter>();
        bool chain=N5Args.Has("-pogo-dash-chain");if(chain){Motor.hasDoubleJump=true;Player.GetComponent<PlayerDash>().hasDash=true;}
        Health.RestoreMax();PlaceForAuthorizedSetup(new Vector2(17.5f,10));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;yield return Keys(Key.DownArrow);
        driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
        var probe=gameObject.AddComponent<N9BossPogoProbe>();probe.boss=boss;
        double next=0,dashReady=0;int seenPogos=Player.GetComponent<PlayerAttack>().PogosPerformed;bool release=false;
        driver.produce=()=>{var a=Player.GetComponent<PlayerAttack>();if(chain&&a.PogosPerformed!=seenPogos&&Time.timeAsDouble>=dashReady&&Time.timeAsDouble-a.LastPogoAt>=.12){seenPogos=a.PogosPerformed;dashReady=Time.timeAsDouble+Motor.Tuning.dashCooldown;return new[]{Key.DownArrow,Key.C};}float gap=Player.GetComponent<BoxCollider2D>().bounds.min.y-boss.GetComponent<BoxCollider2D>().bounds.max.y;if(release){release=false;return new[]{Key.DownArrow};}if(Body.linearVelocity.y<0&&Time.timeAsDouble>=next&&gap<=a.tuning.downAttackHeight+Mathf.Abs(Body.linearVelocity.y)*(a.tuning.attackStartup+.04f)&&gap>.15f){release=true;next=Time.timeAsDouble+a.tuning.attackCooldown+(chain?0:.02);return new[]{Key.DownArrow,Key.X};}return new[]{Key.DownArrow};};
        int maximum=0,current=0,windows=0,lastHits=boss.Hits;double phase=boss.PhaseStarted;bool wasRecovery=false;double deadline=Time.realtimeSinceStartupAsDouble+25;
        while(windows<6&&boss.HP>0&&!RoomSession.Instance.Respawning&&Time.realtimeSinceStartupAsDouble<deadline)
        {
            bool recovery=boss.State is BossEncounter.Phase.MeleeRecovery or BossEncounter.Phase.WaveRecovery;
            if(phase!=boss.PhaseStarted){if(wasRecovery){maximum=Mathf.Max(maximum,current);windows++;lines.Add("MEASURE recovery window hits="+current);}current=0;phase=boss.PhaseStarted;}
            if(recovery)current+=boss.Hits-lastHits;lastHits=boss.Hits;wasRecovery=recovery;yield return null;
        }
        driver.produce=null;yield return Keys();if(!chain)Check(windows==6&&!RoomSession.Instance.Respawning,"six live boss recovery windows without death");
        maximum=Mathf.Max(probe.maximum,Mathf.Max(maximum,current));int selected=maximum>=4?32:maximum<=1?24:28;
        lines.Add("MEASURE recovery="+boss.tuning.recovery+" maxPogoHits="+maximum+" approvedBossHP="+selected);
        string measure=N5Args.Value("-pogo-measure","Validation/N9-boss-pogo-measure.txt");if(File.Exists(measure))throw new IOException("Preserve "+measure);File.WriteAllText(measure,"maxPogoHits="+maximum+"\napprovedBossHP="+selected+"\nrecovery="+boss.tuning.recovery+"\nwindows="+probe.windows+" dashChain="+chain+" actualDeath="+RoomSession.Instance.Respawning+"\nActual live recovery windows; FixedUpdate probe after damage. Virtual down attack"+(chain?" + owned dash":"")+"; no damage/HP writes or repositioning after setup.\n");
        if(chain){Check(probe.windows>0&&maximum>0,"dash-chain live maximum coverage");yield break;}
        // Isolate geometry for both boss directional damage checks, then use actual attacks to defeat.
        Body.constraints=RigidbodyConstraints2D.FreezeAll;boss.GetComponent<BoxCollider2D>().isTrigger=true;
        int popsBefore=HitFeedback.PopsCreated;yield return Strike(boss,1);yield return Strike(boss,-1);
        while(boss.HP>0)yield return Strike(boss,1);
        Check(boss.State==BossEncounter.Phase.Defeated&&boss.GetComponent<HitFeedback>().Calls>0,"boss actual directional defeat/feedback");
        Check(HitFeedback.PopsCreated==popsBefore,"boss uses existing defeat/ending effect, no generic enemy DeathPop");
    }
}
#endif
