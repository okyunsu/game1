#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class N9PogoRoute : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<N9PogoRoute>("-verify-pogotest");
    RouteTickDriver driver;PogoTestCourse course;RouteLibrary library=new();RouteSegment segment;
    int index,previous,startPogos,state,stateTick;bool failed,done,release;double nextAttack;
    protected override void Cleanup(){if(driver!=null)driver.active=false;}
    void Update(){if(driver!=null&&driver.active&&RoomSession.Instance?.Respawning==true){failed=true;driver.active=false;}}
    protected override IEnumerator Tests()
    {
        yield return Until(()=>FindFirstObjectByType<PogoTestCourse>()!=null&&Player!=null,"PogoTest boot");course=FindFirstObjectByType<PogoTestCourse>();yield return Ready();yield return new WaitForSecondsRealtime(.4f);
        Check(Player.GetComponent<PlayerDash>().hasDash&&Motor.hasDoubleJump,"course owns both abilities");
        driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;
        string file=N5Args.Value("-pogo-library","Archive/validation-2026-10/N9-pogo-library.json");bool train=N5Args.Value("-verify-pogotest","all")=="train";
        if(!train)library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(file));
        for(index=0;index<3;index++)
        {
            course.BeginSection(index);yield return Keys();yield return Until(()=>Motor.Grounded,"course section ground");yield return new WaitForSecondsRealtime(.1f);
            segment=train?new RouteSegment{id="P0"+(index+1),startScene="PogoTest",startPosition=Body.position,dash=true,doubleJump=true,startHP=Health.HP,goal="course goal "+(index+1),limitSeconds=20}:library.segments[index];
            done=failed=release=false;previous=state=stateTick=0;nextAttack=0;startPogos=Player.GetComponent<PlayerAttack>().PogosPerformed;driver.tick=0;
            driver.observe=()=>{if(RoomSession.Instance.Respawning||driver.tick>1000)failed=true;if(course.Completed>=index+1)done=true;if(done||failed)driver.active=false;};
            driver.produce=()=>{N5InputTick t;if(train){t=Generate();t.tick=driver.tick;t.pressed=t.held&~previous;t.released=previous&~t.held;previous=t.held;segment.inputs.Add(t);}else{if(driver.tick>=segment.inputs.Count){failed=true;return new N5InputTick();}t=segment.inputs[driver.tick];}return t;};
            driver.active=true;yield return Until(()=>done||failed,"course P0"+(index+1),22);driver.active=false;yield return Keys();
            segment.passed=done&&!failed;segment.result=segment.passed?"PASS":"FAIL actual death/exhaustion pos="+Body.position;segment.attempts=1;
            if(train){library.segments.Add(segment);string attempt=N5Args.Value("-pogo-prefix","Validation/N9-pogo")+"-"+segment.id+".txt";if(File.Exists(attempt))throw new IOException("Preserve "+attempt);File.WriteAllText(attempt,segment.result+"\n"+JsonUtility.ToJson(segment,true));}
            Check(segment.passed,"P0"+(index+1)+" input goal pogos="+(Player.GetComponent<PlayerAttack>().PogosPerformed-startPogos));
        }
        if(train){if(File.Exists(file))throw new IOException("Preserve library");File.WriteAllText(file,JsonUtility.ToJson(library,true));}
        // Actual failure after each independently selected section must restore its own checkpoint.
        for(int n=0;n<3;n++){course.BeginSection(n);yield return Keys();PlaceForAuthorizedSetup(new Vector2(n*26+10,-1.7f));yield return Until(()=>RoomSession.Instance.Respawning,"course actual fall death");yield return Ready();yield return new WaitForSecondsRealtime(.12f);Check(RoomSession.Instance.CheckpointId=="P0"+(n+1)&&Vector2.Distance(Body.position,course.starts[n].spawn.transform.position)<.1f,"P0"+(n+1)+" failure returns section start");}
        // Spend owned double jump and dash away from ground, then hit a target naturally.
        course.BeginSection(0);yield return Keys();yield return Until(()=>Motor.Grounded,"refresh start ground");yield return Keys(Key.Z);yield return new WaitForSecondsRealtime(.15f);yield return Keys();yield return Tap(Key.Z);yield return Tap(Key.C);
        Check(!Motor.DoubleJumpAvailable&&Player.GetComponent<PlayerDash>().AirUsed,"owned abilities spent (ground jump then air jump/dash)");
        // The following drop is an authorized check setup, not a route replay.
        yield return new WaitForSecondsRealtime(.25f);PlaceForAuthorizedSetup(new Vector2(11,6.4f));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;int pogo=Player.GetComponent<PlayerAttack>().PogosPerformed;bool fired=false;var attack=Player.GetComponent<PlayerAttack>();
        driver.tick=0;driver.observe=null;driver.produce=()=>{int held=32;float gap=Player.GetComponent<BoxCollider2D>().bounds.min.y-.25f;if(!fired&&gap<=attack.tuning.downAttackHeight+Motor.Tuning.maxFallSpeed*(attack.tuning.attackStartup+.04f)){fired=true;held|=8;}return new N5InputTick{held=held};};driver.active=true;
        yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>pogo||RoomSession.Instance.Respawning,"owned refresh on target");driver.active=false;yield return Keys();
        Check(!RoomSession.Instance.Respawning&&Motor.DoubleJumpAvailable&&!Player.GetComponent<PlayerDash>().AirUsed,"owned abilities restored by pogo");
    }
    N5InputTick Generate()
    {
        var a=Player.GetComponent<PlayerAttack>();var t=new N5InputTick{axis=1,held=32};float x=Body.position.x,y=Body.position.y;
        if(state==0){if(index>0&&x<(index==1?29.4f:55.4f)){stateTick=driver.tick;return t;}t.held|=2;if(driver.tick-stateTick>=24){state=1;stateTick=driver.tick;}return t;}
        if(index==0&&x>=16&&a.PogosPerformed-startPogos<3)t.axis=0;
        if(index==1&&x>=37)t.axis=0;
        if(index==2)
        {
            if(state==1&&a.PogosPerformed>startPogos){state=2;stateTick=driver.tick;}
            if(state==2){if(driver.tick-stateTick<12)t.axis=0;else{t.held|=2;if(driver.tick-stateTick>=27){state=3;stateTick=driver.tick;}}}
            else if(state==3){t.held|=2;if(driver.tick-stateTick==1)t.held|=4;if(x>=66)t.axis=0;}
        }
        if(state==1&&Body.linearVelocity.y<0)
        {
            float targetY=index==1?2.01f:.25f;float gap=Player.GetComponent<BoxCollider2D>().bounds.min.y-targetY;
            bool near=index==1?x>32.8f&&x<35.2f:index==0?x>3.5f&&x<18.5f:x>57.5f&&x<62.5f;
            if(release)release=false;else if(near&&Time.timeAsDouble>=nextAttack&&gap<=a.tuning.downAttackHeight+Mathf.Abs(Body.linearVelocity.y)*(a.tuning.attackStartup+.04f)&&gap>.1f){t.held|=8;release=true;nextAttack=Time.timeAsDouble+a.tuning.attackCooldown+.025;}
        }
        return t;
    }
}

#endif
