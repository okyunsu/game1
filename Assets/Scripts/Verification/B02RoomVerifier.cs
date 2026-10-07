using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B02RoomVerifier:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()=>N5Args.Start<B02RoomVerifier>("-verify-b02-room");
    N9PogoInput driver;int tick,nextJump,nextAttack,jumpEnd;bool wasGrounded;float goal;int direction=1;
    protected override void Cleanup(){if(driver!=null)driver.produce=null;}
    protected override IEnumerator Tests()
    {
        string id=N5Args.Value("-b02-room","A06");
        yield return Ready();yield return new WaitForSeconds(.2f);
        Check(RoomSession.Instance.CurrentRoomId==id,"optional room starts at entrance "+id);
        Player.GetComponent<PlayerDash>().hasDash=id!="A06";Motor.hasDoubleJump=id=="C05";
        var reward=FindFirstObjectByType<DiscoveryReward>();goal=reward.transform.position.x;
        driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;driver.produce=Generate;
        double started=Time.realtimeSinceStartupAsDouble;
        yield return Until(()=>DiscoveryReward.Found(id)||RoomSession.Instance.Respawning,"discovery traversal",30);
        lines.Add($"MEASURE discovery stop x={Body.position.x:F3} y={Body.position.y:F3} hp={Health.HP} tick={tick}");
        Check(!RoomSession.Instance.Respawning&&DiscoveryReward.Found(id),"entrance to discovery via designated movement inputs");
        direction=-1;
        yield return Until(()=>RoomSession.Instance.CurrentRoomId!=id||RoomSession.Instance.Respawning,"return traversal",30);
        driver.produce=null;yield return Keys();
        lines.Add($"MEASURE return stop x={Body.position.x:F3} y={Body.position.y:F3} hp={Health.HP} tick={tick}");
        Check(!RoomSession.Instance.Respawning,"return traversal no death");yield return Ready();
        string parent=id=="A06"?"A04":id=="B06"?"B03":"C03";
        Check(RoomSession.Instance.CurrentRoomId==parent,"return to original room "+parent);
        Check(Player.GetComponent<PlayerDash>().hasDash==(id!="A06")&&Motor.hasDoubleJump==(id=="C05"),"only arrival-time abilities used");
        lines.Add($"MEASURE {id} entrance-discovery-return seconds={Time.realtimeSinceStartupAsDouble-started:F3}");
        lines.Add("Death checkpoint / enemy reentry / branch entry / human 1-3 minute pacing not checked by this traversal.");
    }
    Key[] Generate()
    {
        tick++;var keys=new System.Collections.Generic.List<Key>();
        keys.Add(direction>0?Key.RightArrow:Key.LeftArrow);
        // Ordinary landing jumps are pulsed; downward attacks timed by distance to authored targets.
        if(Motor.Grounded&&!wasGrounded){jumpEnd=tick;nextJump=tick+2;}
        if(Motor.hasDoubleJump&&direction>0&&Body.position.x<6)nextJump=tick+1;
        if(Motor.Grounded&&tick>=nextJump){jumpEnd=tick+20;nextJump=tick+28;}
        wasGrounded=Motor.Grounded;
        if(tick<jumpEnd)keys.Add(Key.Z);
        if(!Motor.Grounded&&Body.linearVelocity.y<0)
        {
            foreach(var target in FindObjectsByType<PogoTarget>(FindObjectsSortMode.None))
            {
                var bounds=target.GetComponent<BoxCollider2D>().bounds;
                float gap=Player.GetComponent<BoxCollider2D>().bounds.min.y-bounds.max.y;
                var attack=Player.GetComponent<PlayerAttack>();
                if(Body.position.x>=bounds.min.x-.3f&&Body.position.x<=bounds.max.x+.3f&&gap<=attack.tuning.downAttackHeight+Mathf.Abs(Body.linearVelocity.y)*(attack.tuning.attackStartup+.04f)&&gap>.1f&&tick>=nextAttack)
                {keys.Remove(Key.Z);keys.Add(Key.DownArrow);keys.Add(Key.X);nextAttack=tick+20;break;}
            }
        }
        if(Motor.hasDoubleJump&&!Motor.Grounded&&Body.linearVelocity.y<0&&tick>=jumpEnd+3&&Motor.DoubleJumpAvailable){keys.Add(Key.Z);jumpEnd=tick+4;}
        if(Player.GetComponent<PlayerDash>().hasDash&&!Motor.Grounded&&Mathf.Abs(Body.linearVelocity.y)<1&&tick%25==0)keys.Add(Key.C);
        return keys.ToArray();
    }
}
