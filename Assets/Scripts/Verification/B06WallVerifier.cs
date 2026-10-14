#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Opt-in fixture measurements; never changes authored scenes or asset tuning.
public sealed class B06WallVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()=>N5Args.Start<B06WallVerifier>("-verify-b06-wall");
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.2f);
        Check(!ProgressSave.Enabled,"test never saves");
        Check(Motor.hasWallJump&&Player.GetComponent<PlayerDash>().hasDash&&!Motor.hasDoubleJump,"test starts wall+dash, no double jump");
        if(N5Args.Has("-verify-b06-presets"))
        {
            var course=FindFirstObjectByType<WallJumpTestCourse>();
            for(int i=0;i<3;i++)
            {
                yield return Tap(i==0?Key.Digit1:i==1?Key.Digit2:Key.Digit3);
                Check(course.PresetIndex==i,"numeric key selects preset "+course.CurrentPresetName);
                yield return Climb(4);lines.Add("MEASURE preset="+course.CurrentPresetName+" values="+Motor.Tuning.wallSlideMaxFallSpeed+"/"+Motor.Tuning.wallJumpHorizontalSpeed+"/"+Motor.Tuning.wallJumpVelocity+"/"+Motor.Tuning.wallJumpInputLock);
            }
            yield break;
        }
        yield return Slide(new Vector2(25.63f,8),Key.RightArrow,true);
        yield return Slide(new Vector2(91.38f,5),Key.RightArrow,false);
        yield return Slide(new Vector2(96.14f,5),Key.RightArrow,false);
        Motor.hasWallJump=false;yield return Slide(new Vector2(25.63f,8),Key.RightArrow,false);Motor.hasWallJump=true;
        yield return Climb(4);
        yield return Slide(new Vector2(25.63f,8),Key.RightArrow,true);
        Motor.hasDoubleJump=true;int jumpBefore=Motor.WallJumpsPerformed,doubleBefore=Motor.DoubleJumpsPerformed;
        yield return Keys(Key.RightArrow,Key.Z);yield return new WaitForSeconds(.04f);
        Check(Motor.WallJumpsPerformed==jumpBefore+1&&Motor.DoubleJumpsPerformed==doubleBefore,"single press consumes wall jump only");
        double end=Time.fixedUnscaledTimeAsDouble+Motor.Tuning.wallJumpInputLock-.04f;
        while(Time.fixedUnscaledTimeAsDouble<end){Check(!Motor.WallSliding,"input lock prevents wall reattachment");yield return null;}
        yield return Keys();
        yield return Slide(new Vector2(25.63f,8),Key.RightArrow,true);
        yield return Tap(Key.Escape);Check(Player.Paused&&!Motor.WallSliding,"Pause clears wall state");yield return Tap(Key.Escape);
        Check(!Motor.WallSliding,"resume has no stale wall state");
        Motor.ClearTransientState();Check(!Motor.WallSliding,"input clear clears wall state");
        lines.Add("MEASURE fps="+Application.targetFrameRate+" wallJumps="+Motor.WallJumpsPerformed+" attachments="+Motor.WallAttachments);
    }
    IEnumerator Slide(Vector2 position,Key direction,bool expected)
    {
        yield return Keys();Player.ClearTransientInput();PlaceForAuthorizedSetup(position);Body.linearVelocity=Vector2.down;
        yield return Keys(direction);yield return new WaitForSeconds(.12f);
        Check(Motor.WallSliding==expected,"slide="+expected+" at "+position);
        if(expected){Check(Body.linearVelocity.y>=-Motor.Tuning.wallSlideMaxFallSpeed-.05f,"slide speed limit vy="+Body.linearVelocity.y);Check(!Player.GetComponent<PlayerDash>().AirUsed&&Motor.DoubleJumpAvailable==Motor.hasDoubleJump,"wall refreshes owned air");}
    }
    IEnumerator Climb(int width)
    {
        yield return Keys();Player.ClearTransientInput();Motor.hasDoubleJump=false;var dash=Player.GetComponent<PlayerDash>();dash.hasDash=false;
        PlaceForAuthorizedSetup(new Vector2(24,1.81f));yield return new WaitForSeconds(.15f);
        int initial=Motor.WallJumpsPerformed;double until=Time.realtimeSinceStartupAsDouble+30;
        int direction=1;bool held=false,released=false,waitingJump=false;int pendingCount=initial;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z));held=true;
        while(Body.position.y<13.9f)
        {
            if(Time.realtimeSinceStartupAsDouble>until)throw new System.Exception("chimney timeout position="+Body.position+" wall="+Motor.WallSliding+" jumps="+Motor.WallJumpsPerformed);
            if(waitingJump&&Motor.WallJumpsPerformed>pendingCount){waitingJump=false;lines.Add("MEASURE jump="+Motor.WallJumpsPerformed+" at="+Body.position);}
            if(Motor.WallSliding&&!waitingJump)
            {
                if(held){held=false;released=true;}
                else if(released){held=true;released=false;waitingJump=true;pendingCount=Motor.WallJumpsPerformed;}
            }
            // Keep toward the current wall until jump fires; then steer across.
            if(!Motor.WallSliding&&Body.linearVelocity.y>0&&Motor.WallJumpsPerformed>initial)direction=Body.linearVelocity.x>0?1:-1;
            InputSystem.QueueStateEvent(keyboard,held?new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow,Key.Z):new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow));
            yield return null;
        }
        Check(Motor.WallJumpsPerformed>initial&&Motor.DoubleJumpsPerformed==0,"4u chimney 12u climbed with wall jumps only y="+Body.position.y);
        lines.Add("MEASURE chimney4 jumps="+(Motor.WallJumpsPerformed-initial));dash.hasDash=true;yield return Keys();
    }
}
#endif
