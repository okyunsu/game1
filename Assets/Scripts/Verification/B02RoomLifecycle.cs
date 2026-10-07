using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B02RoomLifecycle:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()=>N5Args.Start<B02RoomLifecycle>("-verify-b02-lifecycle");
    protected override IEnumerator Tests()
    {
        string id=N5Args.Value("-b02-room","A06");string parent=id=="A06"?"A04":id=="B06"?"B03":"C03";
        string cp=id=="A06"?"CP-A03":id=="B06"?"CP-B01":"CP-C04";
        yield return ContinueGame();Check(RoomSession.Instance.CheckpointId==cp,"arrival-time checkpoint "+cp);
        for(int death=0;death<2;death++)
        {
            // Fixture transition starts at the original room entrance. Branch itself uses input only.
            Check(RoomSession.Instance.RequestTransition(parent,"Assets/Scenes/"+parent+".unity","FromLeft"),"source room fixture");
            yield return Ready();yield return Until(()=>Motor.Grounded,"source ground");
            if(id=="B06")
            {
                yield return WalkTo(()=>Body.position.x>=7.9f,Key.RightArrow,"B06 branch approach");
                yield return Keys(Key.Z);
            }
            else
            {
                yield return Keys(Key.Z);yield return new WaitForSeconds(.24f);
                yield return Keys(Key.LeftArrow,Key.Z);
            }
            yield return Until(()=>RoomSession.Instance.CurrentRoomId==id,"physical optional branch input",3);
            yield return Keys();yield return Ready();
            var enemy=FindFirstObjectByType<EnemyTurret>();Check(enemy!=null&&enemy.HP==enemy.tuning.maxHP,"enemy reset on room entry");
            enemy.ReceiveHit(1,Player.transform.position);Check(enemy.HP==enemy.tuning.maxHP-1,"enemy damaged before death fixture");
            if(death==0)
            {
                // Fall fixture checks the authored KillZone and real checkpoint return.
                Player.transform.position=new Vector2(18,-.5f);Body.position=Player.transform.position;Body.linearVelocity=Vector2.down;
                Physics2D.SyncTransforms();yield return Until(()=>RoomSession.Instance.Respawning,"physical authored fall death");
            }
            else Check(Health.TakeDamage(Health.HP,Player.transform.position),"lethal hit accepted");
            yield return Until(()=>!RoomSession.Instance.Respawning&&RoomSession.Instance.CheckpointId==cp,"last checkpoint return");yield return Ready();
            Check(RoomSession.Instance.CurrentRoomId==(id=="A06"?"A03":id=="B06"?"B01":"C04")&&Health.HP==Health.tuning.playerMaxHP,"death restores actual last CP / HP");
        }
        Check(Time.timeScale==1,"time scale restored");
        lines.Add("Both physical branch entries and fresh E3 after previous death tested; fall and lethal hit fixtures are explicit setup.");
    }
}
