using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B06bCourseVerifier:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06bCourseVerifier>("-verify-b06b-course");
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.2f);var course=FindFirstObjectByType<WallJumpTestCourse>();
        if(N5Args.Has("-verify-b06b-metrics")){yield return Climb(4,false);yield break;}
        for(int i=0;i<3;i++)
        {
            yield return Tap(i==0?Key.Digit1:i==1?Key.Digit2:Key.Digit3);Check(course.PresetIndex==i,"preset key "+i);
            yield return Climb(4,false);if(i>0)yield return Climb(0,true);
        }
        yield return Tap(Key.Digit2);
        foreach(var key in new[]{Key.Digit7,Key.Digit8,Key.Digit9,Key.Digit0,Key.Digit6})
        {float speed=Motor.Tuning.wallJumpShortHorizontalSpeed,lockTime=Motor.Tuning.wallJumpShortInputLock;bool rising=Motor.Tuning.wallAttachWhileRising;
            yield return Tap(key);Check(File.ReadAllText("Logs/walljump-tuning-last.txt").TrimEnd()==course.TuningText,"display/log agree after "+key);
            Check(key==Key.Digit7?Mathf.Approximately(Motor.Tuning.wallJumpShortHorizontalSpeed,speed-.5f):key==Key.Digit8?Mathf.Approximately(Motor.Tuning.wallJumpShortHorizontalSpeed,speed+.5f):key==Key.Digit9?Mathf.Approximately(Motor.Tuning.wallJumpShortInputLock,lockTime-.01f):key==Key.Digit0?Mathf.Approximately(Motor.Tuning.wallJumpShortInputLock,lockTime+.01f):Motor.Tuning.wallAttachWhileRising!=rising,"key changes expected value "+key);
        }
        lines.Add("MEASURE current tuning="+course.TuningText.Replace("\n"," / "));
    }
    IEnumerator Climb(int width,bool single)
    {
        yield return Keys();Player.ClearTransientInput();Motor.ClearWallState();Motor.hasDoubleJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
        PlaceForAuthorizedSetup(new Vector2(single?113.37f:24,1.81f));yield return new WaitForSeconds(.15f);
        int initial=Motor.WallJumpsPerformed,previous=initial,direction=1;bool held=true,waiting=false;double released=0,started=Time.fixedUnscaledTimeAsDouble,until=Time.realtimeSinceStartupAsDouble+30;
        InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.Z));
        while(Body.position.y<13.85f)
        {
            if(Time.realtimeSinceStartupAsDouble>until)throw new System.Exception("course timeout single="+single+" at="+Body.position+" jumps="+(Motor.WallJumpsPerformed-initial));
            if(Motor.WallJumpsPerformed>previous){previous=Motor.WallJumpsPerformed;waiting=false;}
            if(Motor.WallSliding&&!waiting){if(held){held=false;released=Time.fixedUnscaledTimeAsDouble;}else if(Time.fixedUnscaledTimeAsDouble-released>=.04){held=true;waiting=true;}}
            if(!single&&!Motor.WallSliding&&Body.linearVelocity.y>0&&Motor.WallJumpsPerformed>initial)direction=Body.linearVelocity.x>0?1:-1;
            InputSystem.QueueStateEvent(keyboard,held?new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow,Key.Z):new KeyboardState(direction>0?Key.RightArrow:Key.LeftArrow));yield return null;
        }
        double elapsed=Time.fixedUnscaledTimeAsDouble-started;lines.Add("MEASURE course="+(single?"single12":"chimney4")+" preset="+FindFirstObjectByType<WallJumpTestCourse>().PresetIndex+" seconds="+elapsed+" jumps="+(Motor.WallJumpsPerformed-initial));
        Check(Motor.WallJumpsPerformed>initial,"course summit reached");
        if(N5Args.Has("-verify-b06b-metrics"))
        {
            yield return null;var metrics=FindFirstObjectByType<WallJumpTestMetrics>();
            Check(Mathf.Abs((float)(elapsed-metrics.LastElapsed))<=.05f,"HUD/log elapsed difference="+(elapsed-metrics.LastElapsed));
            Check(metrics.LastJumps==Motor.WallJumpsPerformed-initial,"HUD wall jump count agrees");
            Check(File.ReadAllText("Logs/walljump-runs.txt").Contains(metrics.DisplayText),"HUD record logged");
            lines.Add("MEASURE HUD seconds="+metrics.LastElapsed+" display="+metrics.DisplayText);
        }
        yield return Keys();Player.GetComponent<PlayerDash>().hasDash=true;
    }
}
