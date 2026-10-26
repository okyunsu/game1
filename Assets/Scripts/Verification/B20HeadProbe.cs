#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B20HeadProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B20HeadProbe>("-verify-b20-head");
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();
  var data=JsonUtility.FromJson<B20Headroom.Data>(File.ReadAllText(N5Args.Value("-b20-surfaces","Validation/_raw/B20/T3-surfaces.json")));
  var jumps=JsonUtility.FromJson<B20Metrics.JumpData>(File.ReadAllText("Validation/_raw/B20/T2-metrics.json"));bool all=N5Args.Has("-b20-head-all");
  var candidates=data.faces.Where(f=>Mathf.Abs(f.height-1.6f)<.001f&&f.Category(jumps.shortJump,jumps.full)!="d").GroupBy(f=>f.room).SelectMany(g=>all?g:g.Take(2)).ToArray();
  lines.Add("MEASURE head sample selection="+(all?"all-candidates-including-protocol-rechecks":"max-two-per-room")+" count="+candidates.Length+" movementTuningChanges=0 runtimeEnemyDisabled=True runtimeExitsDisabled=True runtimeDamageTriggersDisabled=True");
  foreach(var face in candidates)
  {
   yield return Keys();if(RoomSession.Instance.CurrentRoomId!=face.room){Check(RoomSession.Instance.RequestTransition(face.room,V4SceneMap.Path(face.room),"Entry"),"fixture "+face.room);yield return Ready();}
   Health.RestoreMax();foreach(var e in FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<EnemyReset>())if(e.Live!=null)e.Live.SetActive(false);
   foreach(var exit in FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<RoomExit>())
   {
    exit.enabled=false;
    if(exit.TryGetComponent<V4ExitRule>(out var rule)&&!string.IsNullOrWhiteSpace(rule.closedUntilWallBroken))exit.GetComponent<Collider2D>().enabled=false;
   }
   foreach(var hazard in FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<DamageBlock>())hazard.GetComponent<Collider2D>().enabled=false;
   Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
   Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(face.x,face.top+.015f));yield return new WaitForSeconds(.25f);
   bool supported=Motor.Grounded,contact=false,launched=false;int headFrame=-1,beforeFrame=-1;float beforeVy=0,afterVy=0,previousVy=Body.linearVelocity.y,max=Body.position.y;double until=Time.realtimeSinceStartupAsDouble+1.5;int previousFrame=Time.frameCount;
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Z));
   while(Time.realtimeSinceStartupAsDouble<until)
   {
    max=Mathf.Max(max,Player.GetComponent<Collider2D>().bounds.min.y);launched|=Body.linearVelocity.y>0;
    var points=new ContactPoint2D[32];var shape=Player.GetComponent<Collider2D>();int count=shape.GetContacts(points);var heads=points.Take(count).Where(p=>p.normal.y<-.7f).ToArray();
    string names=string.Join("|",heads.Select(p=>(p.collider==shape?p.otherCollider:p.collider).name));
    lines.Add("HEADFRAME "+face.room+" "+face.name+" frame="+Time.frameCount+" fixed="+Time.fixedTime.ToString("F4")+" feet="+shape.bounds.min.y.ToString("F4")+" vy="+Body.linearVelocity.y.ToString("F4")+" grounded="+Motor.Grounded+" head="+names);
    if(!contact&&heads.Length>0){contact=true;headFrame=Time.frameCount;beforeFrame=previousFrame;beforeVy=previousVy;afterVy=Body.linearVelocity.y;}
    if(launched&&Motor.Grounded)break;previousFrame=Time.frameCount;previousVy=Body.linearVelocity.y;yield return null;
   }
   yield return Keys();
   Check(RoomSession.Instance.CurrentRoomId==face.room,"head measurement stayed in source room");
   string row="MEASURE head-contact room="+face.room+" actualRoom="+RoomSession.Instance.CurrentRoomId+" face="+face.name+" x="+face.x.ToString("F3")+" top="+face.top.ToString("F3")+" clearance="+face.c.ToString("F3")+" category="+face.Category(jumps.shortJump,jumps.full)+" setupGrounded="+supported+" launched="+launched+" contact="+contact+" beforeFrame="+beforeFrame+" headFrame="+headFrame+" vyBefore="+beforeVy.ToString("F4")+" vyAfter="+afterVy.ToString("F4")+" maxFeet="+max.ToString("F4")+" predictionMatch="+(supported&&launched&&contact);
   lines.Add(row);Debug.Log(row);
  }
 }
}
#endif
