#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Explicitly opted-in, isolated fixtures; measured at equal start/landing elevation.
public sealed class B06bLevelMetricsVerifier:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06bLevelMetricsVerifier>("-verify-b06b-level-metrics");
    GameObject fixture,left,landing;float distance,height,gain;bool landed;
    GameObject Block(string name,Vector2 at,Vector2 size,bool marked=false)
    {var go=new GameObject(name);go.transform.SetParent(fixture.transform);go.layer=LayerMask.NameToLayer("Ground");go.transform.position=at;go.AddComponent<BoxCollider2D>().size=size;if(marked)go.AddComponent<ClimbableWall>();return go;}
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.2f);fixture=new GameObject("Authorized B06b isolated metrics");
        Block("floor",new Vector2(180,.5f),new Vector2(60,1));Block("right 12u wall",new Vector2(180,7),new Vector2(.5f,12),true);
        Motor.hasDoubleJump=false;Motor.Tuning.wallAttachWhileRising=false;
        for(int preset=1;preset<=2;preset++)
        {
            Motor.Tuning.wallJumpShortHorizontalSpeed=preset==1?8:3.5f;Motor.Tuning.wallJumpShortInputLock=preset==1?.15f:.06f;
            yield return Kick(false,-1);lines.Add("MEASURE preset="+preset+" long horizontal="+distance+" peakHeight="+height);
            yield return Kick(true,-1);lines.Add("MEASURE preset="+preset+" short horizontal="+distance+" peakHeight="+height+" reattachGain="+gain);
            for(int width=3;width<=6;width++){yield return Chimney(preset,width,false);yield return Chimney(preset,width,true);}
            float best=0,bestDelay=0;
            for(int sample=0;sample<=16;sample++)
            {float delay=sample*.05f;yield return Kick(false,delay);lines.Add("MEASURE preset="+preset+" dashDelay="+delay+" equalHeightAirRange="+distance);if(distance>best){best=distance;bestDelay=delay;}}
            float gap=Mathf.Floor((best-.22f)*20)/20;landing=Block("same elevation landing",new Vector2(179.75f-gap-1.5f,6.95f),new Vector2(3,.5f));
            yield return Kick(false,bestDelay);Check(landed,"measured dash gap physically landed preset="+preset);
            lines.Add("MEASURE preset="+preset+" wallJumpDash validatedGap="+gap+" delay="+bestDelay+" resolution=0.05 clearance=0.22");Destroy(landing);landing=null;yield return null;
        }
        Destroy(fixture);
    }
    IEnumerator Kick(bool shortKick,float dashDelay)
    {
        yield return Keys();Player.ClearTransientInput();Motor.ClearWallState();Player.GetComponent<PlayerDash>().hasDash=true;
        PlaceForAuthorizedSetup(new Vector2(179.37f,8));Body.linearVelocity=Vector2.down;yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.12f);
        Check(Motor.WallSliding,"measurement wall attached");float startX=Body.position.x,startY=Body.position.y;int initial=Motor.WallJumpsPerformed;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(shortKick?Key.RightArrow:Key.LeftArrow,Key.Z));yield return Until(()=>Motor.WallJumpsPerformed>initial,"measurement kick");
        double began=Time.fixedUnscaledTimeAsDouble,end=Time.realtimeSinceStartupAsDouble+3;distance=height=gain=0;landed=false;bool dashed=false;
        while(Time.realtimeSinceStartupAsDouble<end)
        {
            distance=Mathf.Max(distance,startX-Body.position.x);height=Mathf.Max(height,Body.position.y-startY);gain=Body.position.y-startY;
            if(shortKick&&Motor.WallSliding)break;
            if(Time.fixedUnscaledTimeAsDouble-began>.15&&(Body.position.y<=startY||Motor.Grounded)){landed=Motor.Grounded&&Body.position.y>7;break;}
            if(!shortKick&&dashDelay>=0&&!dashed&&Time.fixedUnscaledTimeAsDouble-began>=dashDelay){InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftArrow,Key.Z,Key.C));dashed=true;}
            else InputSystem.QueueStateEvent(keyboard,new KeyboardState(shortKick?Key.RightArrow:Key.LeftArrow,Key.Z));
            yield return null;
        }
        // Ground contact is reported on the following physics tick.
        if(landing!=null){yield return new WaitForFixedUpdate();landed=Motor.Grounded&&Body.position.y>7;}
        Check(Motor.WallJumpsPerformed==initial+1,"single kick measurement no duplicate jump");yield return Keys();
    }
    IEnumerator Chimney(int preset,int width,bool sameWall)
    {
        left=Block("left chimney "+width,new Vector2(179.5f-width,8),new Vector2(.5f,10),true);
        yield return Keys();Player.ClearTransientInput();Motor.ClearWallState();PlaceForAuthorizedSetup(new Vector2(sameWall?179.37f:179.75f-width*.5f,1.81f));yield return new WaitForSeconds(.15f);
        int initial=Motor.WallJumpsPerformed,previous=initial,direction=1;bool held=true,waiting=false;double released=0,until=Time.realtimeSinceStartupAsDouble+12;float peak=Body.position.y;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z));
        while(Body.position.y<13.85f&&Time.realtimeSinceStartupAsDouble<until)
        {
            peak=Mathf.Max(peak,Body.position.y);if(Motor.WallJumpsPerformed>previous){previous=Motor.WallJumpsPerformed;waiting=false;}
            if(Motor.WallSliding&&!waiting){if(held){held=false;released=Time.fixedUnscaledTimeAsDouble;}else if(Time.fixedUnscaledTimeAsDouble-released>=.04){held=true;waiting=true;}}
            if(!sameWall&&!Motor.WallSliding&&Body.linearVelocity.y>0&&Motor.WallJumpsPerformed>initial)direction=Body.linearVelocity.x>0?1:-1;
            InputSystem.QueueStateEvent(keyboard,held?new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow,Key.Z):new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow));yield return null;
        }
        lines.Add("MEASURE preset="+preset+" chimneyWidth="+width+" strategy="+(sameWall?"sameWall":"alternating")+" climbed="+(Body.position.y>=13.85f)+" jumps="+(Motor.WallJumpsPerformed-initial)+" peak="+Mathf.Max(peak,Body.position.y)+" observationLimit=12s");
        yield return Keys();Destroy(left);left=null;yield return null;
    }
}
#endif
