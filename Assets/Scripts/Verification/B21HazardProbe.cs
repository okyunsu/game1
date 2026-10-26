#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B21HazardProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21HazardProbe>("-verify-b21-hazard");
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 B21HazardReturn Candidate=>Player.GetComponent<B21HazardReturn>();
 void DisableEnemies(){foreach(var e in FindFirstObjectByType<RoomDefinition>().GetComponentsInChildren<EnemyReset>())if(e.Live!=null)e.Live.SetActive(false);}
 IEnumerator Room(string id)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),"Entry"),"fixture "+id);yield return Ready();}
  Health.RestoreMax();Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
  DisableEnemies();
  yield return new WaitForSeconds(.55f);
 }
 IEnumerator Reset(Vector2 point)
 {
  yield return Keys();if(Candidate!=null)yield return Until(()=>!Candidate.Returning,"return finished",2);Health.RestoreMax();DisableEnemies();Motor.ClearTransientState();Player.GetComponent<PlayerDash>().Cancel();PlaceForAuthorizedSetup(point);yield return new WaitForSeconds(.35f);
 }
 IEnumerator Authored(bool enabled)
 {
  yield return Room("A05");Check((Candidate!=null)==enabled,"hazard candidate opt-in "+enabled);
  for(int trial=1;trial<=(enabled?5:1);trial++)
  {
   yield return Reset(new Vector2(enabled?13.5f:5,1.015f));int before=Candidate?.Returns??0,hp=Health.HP;bool crossed=false;double began=Time.realtimeSinceStartupAsDouble,next=0,held=0;
   if(enabled)Check(Candidate.HasSafePoint&&Candidate.SafePoint.x>=12.5f,"safe eastern solid floor recorded");
   while(Time.realtimeSinceStartupAsDouble-began<6)
   {
    double now=Time.realtimeSinceStartupAsDouble;if(Motor.Grounded&&now>=next){held=now+.45;next=now+.65;}
    var direction=enabled?Key.LeftArrow:Key.RightArrow;Input(now<held?new[]{direction,Key.Z}:new[]{direction});
    crossed|=enabled?Body.position.x<5.5f:Body.position.x>13;
    if(enabled&&Candidate.Returns>before||!enabled&&crossed)break;yield return null;
   }
   yield return Keys();if(enabled)yield return Until(()=>!Candidate.Returning,"return settles",2);
   lines.Add("MEASURE A05-return enabled="+enabled+" trial="+trial+" crossed="+crossed+" hp="+hp+"->"+Health.HP+" returns="+((Candidate?.Returns??0)-before)+" end="+Body.position.ToString("F4"));
   Check(enabled?!crossed&&Candidate.Returns==before+1&&Body.position.x>=12.5f:crossed&&hp-Health.HP==1,enabled?"A05 no crossing and eastern return "+trial:"B20 ordinary-jump crossing damage1 reproduced");
  }
 }
 IEnumerator PogoAt(float x)
 {
  int before=Player.GetComponent<PlayerAttack>().PogosPerformed;double began=Time.realtimeSinceStartupAsDouble;
  while(Time.realtimeSinceStartupAsDouble-began<4)
  {
   var keys=new System.Collections.Generic.List<Key>{Key.DownArrow};float dx=x-Body.position.x;if(Mathf.Abs(dx)>.1f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if((Time.realtimeSinceStartupAsDouble-began)%.4<.2)keys.Add(Key.X);Input(keys.ToArray());
   if(Player.GetComponent<PlayerAttack>().PogosPerformed>before&&Mathf.Abs(dx)<.8f){yield return Keys();yield break;}yield return null;
  }
  Check(false,"A04 pogo at "+x+" feet="+Body.position);
 }
 IEnumerator A04()
 {
  yield return Room("A04");yield return Reset(new Vector2(20.8f,1.015f));int returns=Candidate.Returns;
  Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(24,4.1f));yield return Until(()=>!Motor.Grounded,"airborne pose observed before down attack",1);
  yield return PogoAt(24);yield return PogoAt(27.5f);
  double until=Time.realtimeSinceStartupAsDouble+4;
  while(Time.realtimeSinceStartupAsDouble<until&&!(Motor.Grounded&&Body.position.y>1.7f&&Body.position.x>=29.5f&&Body.position.x<=32.5f))
  {
   float dx=31-Body.position.x;Input(Mathf.Abs(dx)>.12f?new[]{dx>0?Key.RightArrow:Key.LeftArrow}:Array.Empty<Key>());yield return null;
  }
  yield return Keys();lines.Add("MEASURE A04-pogo end="+Body.position.ToString("F4")+" grounded="+Motor.Grounded+" pogos="+Player.GetComponent<PlayerAttack>().PogosPerformed+" returns="+(Candidate.Returns-returns)+" enemiesDisabled=True");
  Check(Motor.Grounded&&Body.position.x>=29.5f&&Body.position.x<=32.5f&&Body.position.y>1.7f,"A04 two-lantern pogo route reaches island");
 }
 GameObject Box(string name,Vector2 center,Vector2 size,bool hazard=false)
 {
  var go=new GameObject(name);go.layer=hazard?0:6;go.transform.position=center;var shape=go.AddComponent<BoxCollider2D>();shape.size=size;shape.isTrigger=hazard;if(hazard)go.AddComponent<DamageBlock>();return go;
 }
 IEnumerator Arena()
 {
  Check(Candidate!=null,"arena candidate enabled");var course=FindFirstObjectByType<B21ArenaCourse>();
  var floor=Box("B21 trench base",new Vector2(2.25f,-.8f),new Vector2(4.5f,.4f));var hazard=Box("B21 damaging trench",new Vector2(2.25f,-.45f),new Vector2(4.5f,.3f),true);
  foreach(float gap in new[]{4.5f,5,5.5f,6})foreach(bool owned in new[]{false,true})for(int trial=1;trial<=3;trial++)
  {
   course.Configure(gap);floor.transform.position=new Vector2(gap/2,-.8f);floor.GetComponent<BoxCollider2D>().size=new Vector2(gap,.4f);hazard.transform.position=new Vector2(gap/2,-.45f);hazard.GetComponent<BoxCollider2D>().size=new Vector2(gap,.3f);Physics2D.SyncTransforms();
   yield return Reset(new Vector2(gap+3.5f,.015f));var dash=Player.GetComponent<PlayerDash>();dash.hasDash=owned;int before=Candidate.Returns,hp=Health.HP;
   Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=gap+.35f,"arena runup",2);Input(Key.LeftArrow,Key.Z);bool launched=false,dashed=false,crossed=false;double until=Time.realtimeSinceStartupAsDouble+4;
   while(Time.realtimeSinceStartupAsDouble<until)
   {
    launched|=Body.linearVelocity.y>0;if(owned&&launched&&!dashed&&Body.linearVelocity.y<=.6f){Input(Key.LeftArrow,Key.Z,Key.LeftShift);dashed=true;}else if(dashed)Input(Key.LeftArrow,Key.Z);
    if(Motor.Grounded&&Body.position.x<-.2f){crossed=true;break;}if(Candidate.Returns>before)break;yield return null;
   }
   yield return Keys();yield return Until(()=>!Candidate.Returning,"arena return finished",2);
   lines.Add("MEASURE trench-width width="+gap.ToString("F1")+" dashOwned="+owned+" trial="+trial+" crossed="+crossed+" hp="+hp+"->"+Health.HP+" returns="+(Candidate.Returns-before)+" end="+Body.position.ToString("F4"));
   if(!crossed)Check(Candidate.Returns>before,"failed crossing touched damaging trench and returned");
  }
  Destroy(floor);Destroy(hazard);
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return new WaitForSeconds(.2f);
  string mode=N5Args.Value("-b21-hazard-mode","on");if(mode=="arena")yield return Arena();else if(mode=="pogo")yield return A04();else if(mode=="fatal")yield return Fatal();else if(mode=="island5")yield return IslandFive();else{yield return Authored(mode!="off");if(mode!="off")yield return A04();}
 }
 IEnumerator Fatal()
 {
  yield return Room("A05");yield return Reset(new Vector2(13.5f,1.015f));Check(Candidate.HasSafePoint,"fatal fixture safe point");int before=Candidate.Returns;
  Health.TakeDamage(Health.HP-1,Body.position);PlaceForAuthorizedSetup(new Vector2(13.5f,1.015f));yield return new WaitForSeconds(Health.tuning.invincibility+.1f);
  PlaceForAuthorizedSetup(new Vector2(9,1.2f));yield return Until(()=>Health.HP==0||RoomSession.Instance.Respawning,"fatal hazard damage",2);
  Check(Health.HP==0&&Candidate.Returns==before,"HP0 follows existing death instead of hazard return");yield return Ready();
  Check(Health.HP==Health.tuning.playerMaxHP&&Candidate.Returns==before,"existing checkpoint respawn restores HP without candidate return");
 }
 IEnumerator IslandFive()
 {
  Check(Candidate!=null,"hazard candidate enabled");int successes=0;
  for(int trial=1;trial<=5;trial++)
  {
   yield return Room("A04");yield return Reset(new Vector2(20.8f,1.015f));Player.GetComponent<PlayerSlam>().hasSlam=false;
   var room=FindFirstObjectByType<RoomDefinition>();bool enemiesOff=room.GetComponentsInChildren<EnemyReset>().All(e=>e.Live==null||!e.Live.activeInHierarchy);Check(enemiesOff,"actual enemies disabled after HP reset "+trial);
   Check(!Motor.hasDoubleJump&&!Motor.hasWallJump&&!Player.GetComponent<PlayerDash>().hasDash,"no movement abilities "+trial);
   var reward=room.GetComponentsInChildren<DiscoveryReward>(true).Single();DiscoveryReward.ResetSession();reward.gameObject.SetActive(true);Physics2D.SyncTransforms();
   var attack=Player.GetComponent<PlayerAttack>();int pogos=attack.PogosPerformed,returns=Candidate.Returns,hp=Health.HP;Vector2 failurePoint=Body.position;bool hit=false,success=false;double began=Time.realtimeSinceStartupAsDouble,nextAttack=0;
   while(Time.realtimeSinceStartupAsDouble-began<14)
   {
    double now=Time.realtimeSinceStartupAsDouble;int target=Mathf.Min(2,attack.PogosPerformed-pogos);float dx=31-Body.position.x;
    var keys=new System.Collections.Generic.List<Key>{Key.Z,Key.DownArrow};if(Mathf.Abs(dx)>.15f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
    if(target<2)
    {
     float x=target==0?24:27.5f,top=target==0?2.8f:3.4f,allowance=target==0?.4f:.7f;
     if(now>=nextAttack&&!Motor.Grounded&&Body.linearVelocity.y<0&&Mathf.Abs(Body.position.x-x)<.7f&&Player.GetComponent<Collider2D>().bounds.min.y<=top+allowance)
     {keys.Add(Key.X);nextAttack=now+attack.tuning.attackCooldown+.03;}
    }
    Input(keys.ToArray());if(!hit&&Health.HP<hp){hit=true;failurePoint=Body.position;}
    if(DiscoveryReward.Found(reward.candidateId)&&Motor.Grounded&&Body.position.x>=29.5f&&Body.position.x<=32.5f&&Body.position.y>1.7f){success=true;break;}
    if(Candidate.Returns>returns)break;yield return null;
   }
   yield return Keys();yield return Until(()=>!Candidate.Returning,"sample return settles",2);if(success)successes++;
   lines.Add("MEASURE A04-island trial="+trial+" success="+success+" reward="+DiscoveryReward.Found(reward.candidateId)+" hp="+hp+"->"+Health.HP+" returns="+(Candidate.Returns-returns)+" pogos="+(attack.PogosPerformed-pogos)+" failureAt="+(hit?failurePoint:Body.position).ToString("F4")+" end="+Body.position.ToString("F4")+" enemiesDisabled="+enemiesOff+" abilities=none");
  }
  lines.Add("MEASURE A04-island-summary successes="+successes+" attempts=5; fixed-input samples, not a general route solver");Check(true,"five A04 samples recorded");
 }
}
#endif
