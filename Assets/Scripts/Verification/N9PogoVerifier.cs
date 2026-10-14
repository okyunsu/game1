#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[DefaultExecutionOrder(-31000)]
public sealed class N9PogoInput : MonoBehaviour
{
    public Keyboard keyboard;
    public Func<Key[]> produce;
    void FixedUpdate(){if(produce==null)return;InputSystem.QueueStateEvent(keyboard,new KeyboardState(produce()),Time.fixedUnscaledTimeAsDouble);InputSystem.Update();}
}
[DefaultExecutionOrder(200)]
public sealed class N9PogoProbe : MonoBehaviour
{
    public PlayerAttack attack; public Rigidbody2D body;
    public int observed;public float hitVelocity,hitY,peak;
    void FixedUpdate(){if(attack.PogosPerformed!=observed){observed=attack.PogosPerformed;hitVelocity=body.linearVelocity.y;hitY=body.position.y;peak=hitY;}peak=Mathf.Max(peak,body.position.y);}
}
public sealed class N9PogoVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<N9PogoVerifier>("-verify-pogo");
    N9PogoInput driver;N9PogoProbe probe;GameObject target;
    protected override void Cleanup(){if(driver!=null)driver.produce=null;Destroy(target);}
    GameObject Box(Vector2 at,bool spike,bool dummy)
    {
        var go=new GameObject("N9 authorized pogo fixture");go.transform.position=at;
        var c=go.AddComponent<BoxCollider2D>();c.size=new Vector2(1,.2f);c.isTrigger=true;
        if(spike){go.AddComponent<KillZone>();go.AddComponent<PogoTarget>();}
        else if(dummy){go.AddComponent<DummyTarget>();}
        else go.AddComponent<PogoTarget>();
        Physics2D.SyncTransforms();return go;
    }
    IEnumerator Setup(float y)
    {
        driver.produce=null;if(target!=null)target.SetActive(false);yield return Keys();yield return new WaitForSecondsRealtime(.4f);
        Player.GetComponent<PlayerAttack>().Cancel();PlaceForAuthorizedSetup(new Vector2(3.5f,y));
        Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;
        yield return new WaitForFixedUpdate();yield return new WaitForFixedUpdate();PlaceForAuthorizedSetup(new Vector2(3.5f,y));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;if(target!=null)target.SetActive(true);Physics2D.SyncTransforms();
    }
    protected override IEnumerator Tests()
    {
        yield return NewGame();yield return Until(()=>Motor.Grounded,"grounded");
        var a=Player.GetComponent<PlayerAttack>();var d=Player.GetComponent<PlayerDash>();
        driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
        probe=gameObject.AddComponent<N9PogoProbe>();probe.attack=a;probe.body=Body;probe.observed=a.PogosPerformed;
        float min=float.PositiveInfinity,max=0;
        foreach(int fps in new[]{30,60,120})
        {
            Application.targetFrameRate=fps;yield return new WaitForSecondsRealtime(.3f);
            yield return Until(()=>Motor.Grounded,"ground");yield return Keys(Key.DownArrow,Key.X);
            Check(a.VerticalDirection==0,"ground down+X is horizontal fps="+fps);yield return Keys();
            target=Box(new Vector2(3.5f,3),false,true);yield return Setup(6.4f);
            int before=a.PogosPerformed;int fired=0;
            driver.produce=()=>{if(fired++==0)return new[]{Key.DownArrow};if(fired==2)return new[]{Key.DownArrow,Key.X};return new[]{Key.DownArrow};};
            yield return Until(()=>a.PogosPerformed>before,"enemy down hit",2);
            var shape=Player.GetComponent<BoxCollider2D>();var b=shape.bounds;
            Check(a.VerticalDirection==-1&&Vector2.Distance(a.HitboxCenter,new Vector2(b.center.x,b.min.y-a.tuning.downAttackHeight*.5f))<.01f&&Vector2.Distance(a.HitboxSize,new Vector2(a.tuning.downAttackWidth,a.tuning.downAttackHeight))<.01f,"down bounds/size .01u fps="+fps);
            Check(Mathf.Abs(probe.hitVelocity-a.tuning.pogoVelocity)<.001f,"same physics tick vy=pogoVelocity fps="+fps);
            var dummy=target.GetComponent<DummyTarget>();Check(dummy.Hits==1&&dummy.HP==dummy.maxHP-a.tuning.attackDamage,"damage one fps="+fps);
            Check(!d.hasDash&&!d.AirUsed&&!Motor.hasDoubleJump&&!Motor.DoubleJumpAvailable,"unowned abilities remain unowned fps="+fps);
            var slash=Player.GetComponent<AttackFeedback>().slash;
            Check(slash.gameObject.activeSelf&&Vector2.Distance(slash.bounds.size,a.HitboxSize)<.01f,"down slash size fps="+fps);
            yield return new WaitForSecondsRealtime(.25f);Check(a.PogosPerformed==before+1&&dummy.Hits==1,"one pogo/hit per attack fps="+fps);
            driver.produce=null;Destroy(target);target=null;yield return Keys();
            yield return new WaitForSecondsRealtime(.8f);
            yield return Setup(6.4f);before=a.PogosPerformed;fired=0;
            driver.produce=()=>{if(fired++==0)return new[]{Key.DownArrow};if(fired==2)return new[]{Key.DownArrow,Key.X};return Array.Empty<Key>();};
            yield return new WaitForSecondsRealtime(.3f);Check(a.PogosPerformed==before,"miss no pogo fps="+fps);driver.produce=null;yield return Keys();
            yield return Until(()=>Motor.Grounded,"miss lands");
            target=Box(new Vector2(3.5f,3),true,false);yield return Setup(6.4f);before=a.PogosPerformed;fired=0;
            driver.produce=()=>{if(fired++==0)return new[]{Key.DownArrow};if(fired==2)return new[]{Key.DownArrow,Key.X};return Array.Empty<Key>();};
            yield return Until(()=>a.PogosPerformed>before||RoomSession.Instance.Respawning,"spike outcome",2);
            Check(!RoomSession.Instance.Respawning&&a.PogosPerformed==before+1&&Health.HP>0,"early max-fall spike pogo/deaths0 fps="+fps);
            yield return Until(()=>Body.linearVelocity.y<=0,"pogo apex",2);
            float height=probe.peak-probe.hitY;min=Mathf.Min(min,height);max=Mathf.Max(max,height);lines.Add("MEASURE pogo apex height="+height.ToString("F4")+" fps="+fps);
            driver.produce=null;Destroy(target);target=null;yield return Keys();yield return new WaitForSecondsRealtime(.7f);
            target=Box(new Vector2(3.5f,3),true,false);PlaceForAuthorizedSetup(new Vector2(3.5f,3.65f));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;
            yield return Until(()=>RoomSession.Instance.Respawning,"late body spike death",2);before=a.PogosPerformed;
            yield return Keys(Key.DownArrow,Key.X);Check(a.PogosPerformed==before&&Health.HP==0,"body-first death cannot pogo fps="+fps);
            Destroy(target);target=null;yield return Keys();yield return Ready();yield return Until(()=>Motor.Grounded,"respawn lands");
        }
        Check(max-min<=.05f,"pogo apex FPS spread <= .05u measured="+(max-min));
        // Spend both owned abilities through actual input before refreshing on a target.
        d.hasDash=true;Motor.hasDoubleJump=true;yield return Setup(9);
        yield return Tap(Key.Z);yield return Tap(Key.C);yield return new WaitForSecondsRealtime(.25f);
        Check(d.AirUsed&&!Motor.DoubleJumpAvailable,"owned air abilities spent by input");
        target=Box(new Vector2(3.5f,3),false,false);yield return Setup(6.4f);int count=a.PogosPerformed;int press=0;
        driver.produce=()=>{if(press++==0)return new[]{Key.DownArrow};if(press==2)return new[]{Key.DownArrow,Key.X};return Array.Empty<Key>();};
        yield return Until(()=>a.PogosPerformed>count,"owned refresh");Check(!d.AirUsed&&Motor.DoubleJumpAvailable,"owned double jump/dash refreshed");
        lines.Add("Clock: Time.timeAsDouble shared startup/active/cooldown; down FixedUpdate order100 after Motor, before physics. Fixtures only at test setup.");
    }
}
#endif
