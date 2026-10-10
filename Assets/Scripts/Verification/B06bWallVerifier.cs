using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B06bWallVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()=>N5Args.Start<B06bWallVerifier>("-verify-b06b-wall");
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.2f);
        var wall=new GameObject("Authorized isolated 12u wall fixture");wall.layer=LayerMask.NameToLayer("Ground");
        wall.transform.position=new Vector2(110,7);wall.AddComponent<BoxCollider2D>().size=new Vector2(.5f,12);wall.AddComponent<ClimbableWall>();
        var floor=new GameObject("Fixture floor");floor.layer=wall.layer;floor.transform.position=new Vector2(110,.5f);floor.AddComponent<BoxCollider2D>().size=new Vector2(20,1);
        var t=Motor.Tuning;float baseline=0;
        for(int mode=0;mode<3;mode++)
        {
            t.wallJumpShortHorizontalSpeed=mode==0?8:3.5f;t.wallJumpShortInputLock=mode==0?.15f:.06f;t.wallAttachWhileRising=mode==2;
            yield return Kick(false,value=>{if(mode==0)baseline=value;else Check(Mathf.Abs(value-baseline)<=.05f,"long distance unchanged "+value);});
            if(mode>0)yield return SingleClimb();
        }
        Destroy(wall);Destroy(floor);
    }
    IEnumerator Kick(bool shortKick,System.Action<float> measured)
    {
        yield return Keys();Player.ClearTransientInput();Motor.ClearWallState();PlaceForAuthorizedSetup(new Vector2(109.37f,8));Body.linearVelocity=Vector2.down;
        yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.12f);Check(Motor.WallSliding,"kick fixture attached");
        int count=Motor.WallJumpsPerformed;float x=Body.position.x,max=0;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(shortKick?Key.RightArrow:Key.None,Key.Z));
        yield return Until(()=>Motor.WallJumpsPerformed>count,"kick fired");
        double until=Time.realtimeSinceStartupAsDouble+.7;
        while(Time.realtimeSinceStartupAsDouble<until){max=Mathf.Max(max,x-Body.position.x);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z));yield return null;}
        Check(Motor.WallJumpsPerformed==count+1,"one held input one wall jump");lines.Add("MEASURE long kick distance="+max+" rising="+Motor.Tuning.wallAttachWhileRising);measured(max);yield return Keys();
    }
    IEnumerator SingleClimb()
    {
        yield return Keys();Player.ClearTransientInput();Motor.ClearWallState();PlaceForAuthorizedSetup(new Vector2(109.37f,1.81f));yield return new WaitForSeconds(.15f);
        Motor.hasDoubleJump=false;int initial=Motor.WallJumpsPerformed,previous=initial,rising=0;bool held=true,waiting=false;double releasedAt=0;float furthest=0;double until=Time.realtimeSinceStartupAsDouble+25;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z));
        while(Body.position.y<13.9f)
        {
            if(Time.realtimeSinceStartupAsDouble>=until)throw new System.Exception("single wall timeout position="+Body.position+" jumps="+(Motor.WallJumpsPerformed-initial));
            furthest=Mathf.Max(furthest,109.37f-Body.position.x);
            if(Motor.WallJumpsPerformed>previous){previous=Motor.WallJumpsPerformed;waiting=false;}
            if(Motor.WallSliding&&Body.linearVelocity.y>0)rising++;
            if(Motor.WallSliding&&!waiting){if(held){held=false;releasedAt=Time.fixedUnscaledTimeAsDouble;}else if(Time.fixedUnscaledTimeAsDouble-releasedAt>=.04){held=true;waiting=true;}}
            InputSystem.QueueStateEvent(keyboard,held?new KeyboardState(Key.RightArrow,Key.Z):new KeyboardState(Key.RightArrow));yield return null;
        }
        Check(furthest<=1,"short maximum wall separation <=1u actual="+furthest);
        Check(Motor.Tuning.wallAttachWhileRising?rising>0:rising==0,"rising attachment policy count="+rising);
        lines.Add("MEASURE single12 fps="+Application.targetFrameRate+" rising="+Motor.Tuning.wallAttachWhileRising+" jumps="+(Motor.WallJumpsPerformed-initial)+" distance="+furthest+" summit="+Body.position.y);yield return Keys();
    }
}

