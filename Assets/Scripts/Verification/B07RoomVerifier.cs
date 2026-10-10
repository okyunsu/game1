using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B07RoomVerifier:N5Runner
{
 [Serializable]public sealed class Recording{public string room,route,goal;public List<N5InputTick> inputs=new();}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07RoomVerifier>("-verify-b07-room");
 RouteTickDriver driver;PlayerTuning original,candidate;V3RoomRoutes.Route route;Recording recording;
 int stage,jumpAt=-1000,climbRelease=-1000,previousJumps,pogos,nextAttack;bool climbHeld=true,climbWaiting,dead;
 double bossPhase=-1;string roomId;
 protected override void Cleanup(){if(driver!=null)driver.active=false;if(Player!=null&&original!=null)Motor.SetWallJumpTestTuning(original);if(candidate!=null)Destroy(candidate);}
 protected override IEnumerator Tests()
 {
  Check(V3SceneMap.Enabled,"V3 explicit mode");roomId=N5Args.Value("-verify-b07-room","A01");string routeName=N5Args.Value("-b07-route","main");
  DiscoveryReward.ResetSession();yield return NewGame();ProgressSave.Enabled=false;
  if(roomId!="A01"){Check(RoomSession.Instance.RequestTransition(roomId,V3SceneMap.Path(roomId),roomId=="C06"?"FromLeft":"Entry"),"room start fixture "+roomId);yield return Ready();}
  var plan=FindFirstObjectByType<V3RoomRoutes>();Check(plan!=null,"authored room itinerary");route=Array.Find(plan.routes,r=>r.name==routeName);Check(route!=null,"route "+routeName);
  Player.GetComponent<PlayerDash>().hasDash=route.dash;Motor.hasWallJump=route.wallJump;Motor.hasDoubleJump=route.doubleJump&&!N5Args.Has("-b07-no-jump");
  if(route.startSpawn!="Entry"||roomId=="C06"){var definition=FindFirstObjectByType<RoomDefinition>();Check(definition.TryGetSpawn(roomId=="C06"?"FromLeft":route.startSpawn,out var spawn),"start spawn");PlaceForAuthorizedSetup(spawn.transform.position);}
  original=Motor.Tuning;if(N5Args.Has("-b07-short")){candidate=Instantiate(original);candidate.wallJumpShortHorizontalSpeed=3.5f;candidate.wallJumpShortInputLock=.06f;Motor.SetWallJumpTestTuning(candidate);}
  yield return Keys();yield return new WaitForSeconds(.3f);
  int enemies=FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length+FindObjectsByType<EnemyCharger>(FindObjectsSortMode.None).Length+FindObjectsByType<EnemyTurret>(FindObjectsSortMode.None).Length+FindObjectsByType<BossEncounter>(FindObjectsSortMode.None).Length;
  Check(enemies==plan.enemyCount,"authored enemy count "+enemies);lines.Add("MEASURE "+roomId+" dimensions="+plan.dimensions+" fallPits="+plan.fallPits+" enemies="+enemies+" route="+routeName+" short="+N5Args.Has("-b07-short"));
  if(N5Args.Has("-b07-shot"))Capture(plan);
  string replay=N5Args.Value("-b07-replay","");recording=replay==""?new Recording{room=roomId,route=routeName,goal=route.goal}:JsonUtility.FromJson<Recording>(File.ReadAllText(replay));
  driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;driver.produce=()=>
  {
   if(replay!=""){if(driver.tick>=recording.inputs.Count){dead=true;return new N5InputTick();}return recording.inputs[driver.tick];}
   var input=Generate();input.tick=driver.tick;recording.inputs.Add(input);return input;
  };
  driver.observe=()=>{if(Complete())driver.active=false;};driver.active=true;double began=Time.realtimeSinceStartupAsDouble,limit=began+N5Args.Int("-b07-limit",150);
  while(driver.active&&!dead&&Time.realtimeSinceStartupAsDouble<limit){if(RoomSession.Instance?.Respawning==true||!Complete()&&RoomSession.Instance!=null&&RoomSession.Instance.CurrentRoomId!=roomId&&RoomSession.Instance.CurrentRoomId!=route.goal){dead=true;}yield return null;}
  driver.active=false;yield return Keys();if(replay=="")File.WriteAllText(Path.ChangeExtension(Evidence,"inputs.json"),JsonUtility.ToJson(recording,true));
  lines.Add("MEASURE route seconds="+(Time.realtimeSinceStartupAsDouble-began)+" ticks="+driver.tick+" stage="+stage+" pos="+(Player==null?Vector2.zero:Body.position)+" pogos="+(Player==null?0:Player.GetComponent<PlayerAttack>().PogosPerformed));
  if(N5Args.Has("-b07-negative")){Check(!Complete(),"ability gate denies optional route");yield break;}
  Check(!dead&&Complete(),roomId+" "+routeName+" actual input "+(replay==""?"record":"replay"));
 }
 bool Complete()
 {
  if(route==null)return false;if(route.goal.StartsWith("Discovery:"))return DiscoveryReward.Found(route.goal.Substring(10));
  if(route.goal=="Ending")return FindFirstObjectByType<EndingMenu>()!=null;
  return RoomSession.Instance!=null&&RoomSession.Instance.CurrentRoomId==route.goal&&!RoomSession.Instance.Transitioning;
 }
 void Advance(){stage++;jumpAt=-1000;climbWaiting=false;climbHeld=true;previousJumps=Motor.WallJumpsPerformed;pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;}
 N5InputTick Generate()
 {
  var result=new N5InputTick();if(Player==null||Player.InputLocked)return result;
  if(stage>=route.points.Length)return result;var point=route.points[stage];Vector2 p=Body.position;float dx=point.position.x-p.x;result.axis=Mathf.Abs(dx)<.12f?0:Mathf.Sign(dx);
  var action=point.action;int tick=driver.tick;
  if(action==V3RoomRoutes.Action.Wall)
  {
   if(ProgressSave.IsWallBroken(point.id)){Advance();return result;}
   var wall=FindObjectsByType<BreakableWall>(FindObjectsSortMode.None).FirstOrDefault(w=>w.wallId==point.id);
   if(wall!=null){result.axis=Mathf.Sign(wall.transform.position.x-p.x);if(Mathf.Abs(dx)<.3f)result.axis=tick%22==0?result.axis:0;}
   if(tick>=nextAttack){result.held|=8;nextAttack=tick+22;}return result;
  }
  if(action==V3RoomRoutes.Action.Boss)
  {
   var boss=FindFirstObjectByType<BossEncounter>();if(boss==null||ProgressSave.BossDefeated){Advance();return result;}
   if(!boss.Fighting){result.axis=1;return result;}result.axis=p.x<15.6f?1:p.x>15.9f?-1:0;
   double phase=Time.timeAsDouble-boss.PhaseStarted;
   if(Motor.Grounded&&bossPhase!=boss.PhaseStarted&&((boss.State==BossEncounter.Phase.MeleeTell&&phase>boss.tuning.meleeTell-.38)||(boss.State==BossEncounter.Phase.WaveTell&&phase>boss.tuning.waveTell-.38))){jumpAt=tick;bossPhase=boss.PhaseStarted;}
   if(tick-jumpAt<28)result.held|=2;if(Motor.Grounded&&tick>=nextAttack){result.axis=1;result.held|=8;nextAttack=tick+20;}return result;
  }
  if(action==V3RoomRoutes.Action.Climb)
  {
   if(Mathf.Abs(dx)<.8f){var wall=FindObjectsByType<ClimbableWall>(FindObjectsSortMode.None).Where(w=>w.GetComponent<Collider2D>().bounds.min.y<p.y+1&&w.GetComponent<Collider2D>().bounds.max.y>p.y-1).OrderBy(w=>Mathf.Abs(w.transform.position.x-p.x)).FirstOrDefault();if(wall!=null)result.axis=Mathf.Sign(wall.transform.position.x-p.x);}
   if(p.y>=point.position.y&&Mathf.Abs(dx)<.8f){Advance();return result;}
   if(Motor.Grounded){climbHeld=true;climbWaiting=false;}
   if(Motor.WallJumpsPerformed>previousJumps){previousJumps=Motor.WallJumpsPerformed;climbWaiting=false;}
   if(Motor.WallSliding&&!climbWaiting){if(climbHeld){climbHeld=false;climbRelease=tick;}else if(tick-climbRelease>=2){climbHeld=true;climbWaiting=true;}}
   if(climbHeld)result.held|=2;return result;
  }
  if(action==V3RoomRoutes.Action.Pogo)
  {
   if(Player.GetComponent<PlayerAttack>().PogosPerformed>pogos){Advance();return result;}
   var targets=FindObjectsByType<Collider2D>(FindObjectsSortMode.None).Where(c=>c.GetComponent<PogoTarget>()!=null||c.GetComponent<EnemyPatrol>()!=null);
   var target=targets.OrderBy(c=>Mathf.Abs(c.bounds.center.x-point.position.x)).FirstOrDefault();
   if(target!=null){dx=target.bounds.center.x-p.x;result.axis=Mathf.Abs(dx)<.1f?0:Mathf.Sign(dx);
    if(Motor.Grounded&&tick-jumpAt>35)jumpAt=tick;if(tick-jumpAt<27)result.held|=2;
    if(!Motor.Grounded&&!Player.GetComponent<PlayerDash>().IsDashing&&Body.linearVelocity.y<=1&&p.y-target.bounds.max.y<2.6f&&tick>=nextAttack){result.held|=8|32;nextAttack=tick+20;}
   }return result;
  }
  bool at=Mathf.Abs(dx)<.55f&&p.y>=point.position.y-.3f;
  
  if(at&&(action==V3RoomRoutes.Action.Walk||Motor.Grounded||action==V3RoomRoutes.Action.DashAir&&Mathf.Abs(dx)<.55f)){Advance();return result;}
  if(action is V3RoomRoutes.Action.Jump or V3RoomRoutes.Action.DoubleJump or V3RoomRoutes.Action.Dash or V3RoomRoutes.Action.DashAir)
  {
   if(Motor.Grounded&&tick-jumpAt>38)jumpAt=tick;
   // A pogo-to-double-jump route spends its remaining airborne jump near the bounce apex.
   if(action==V3RoomRoutes.Action.DoubleJump&&jumpAt<0&&!Motor.Grounded&&Body.linearVelocity.y<=1)jumpAt=tick-16;
   int elapsed=tick-jumpAt;
   if(action==V3RoomRoutes.Action.DoubleJump){if(elapsed<14||elapsed>=16&&elapsed<42)result.held|=2;}
   else if(elapsed<28)result.held|=2;
   if((action==V3RoomRoutes.Action.Dash||action==V3RoomRoutes.Action.DashAir)&&elapsed==18)result.held|=4;
  }
  if(action==V3RoomRoutes.Action.Walk&&Motor.Grounded&&tick-jumpAt>38&&result.axis!=0)
  {
   var hit=Physics2D.BoxCast(p,new Vector2(.6f,1.4f),0,Vector2.right*result.axis,.7f,1<<6);
   if(hit.collider!=null&&Mathf.Abs(hit.normal.x)>.7f&&hit.collider.GetComponent<DashGate>()==null&&hit.collider.GetComponent<BreakableWall>()==null&&hit.collider.bounds.max.y-(p.y-.8f)<2.3f)jumpAt=tick;
  }
  if(action==V3RoomRoutes.Action.Walk&&tick-jumpAt<28)result.held|=2;
  if(action==V3RoomRoutes.Action.GroundDash)
  {
   var gate=FindObjectsByType<DashGate>(FindObjectsSortMode.None).OrderBy(g=>Vector2.Distance(g.transform.position,p)).FirstOrDefault();
   if(gate!=null&&Mathf.Abs(gate.transform.position.x-p.x)<1.4f&&tick-jumpAt>38&&!Player.GetComponent<PlayerDash>().IsDashing){result.held|=4;jumpAt=tick;}
  }
  if(action!=V3RoomRoutes.Action.Pogo&&action!=V3RoomRoutes.Action.Dash&&tick>=nextAttack)
  {
   bool threat=FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Any(e=>Mathf.Abs(e.transform.position.x-p.x)<2&&Mathf.Abs(e.transform.position.y-p.y)<1);
   if(threat){result.held|=8;nextAttack=tick+20;}
  }
  return result;
 }
 void Capture(V3RoomRoutes plan)
 {
  string path="Validation/_raw/B07/"+roomId+"-overview.png";if(File.Exists(path))return;
  var rig=FindFirstObjectByType<RoomCameraRig>();Bounds bounds=rig.boundary.bounds;float aspect=bounds.size.x/bounds.size.y;int w=aspect>=1?2048:Mathf.RoundToInt(2048*aspect),h=aspect>=1?Mathf.RoundToInt(2048/aspect):2048;
  var go=new GameObject("Temporary room overview camera");var camera=go.AddComponent<Camera>();camera.enabled=false;camera.clearFlags=CameraClearFlags.SolidColor;camera.orthographic=true;camera.orthographicSize=bounds.size.y*.5f+1;camera.backgroundColor=Camera.main.backgroundColor;go.transform.position=new Vector3(bounds.center.x,bounds.center.y,-10);
  var target=RenderTexture.GetTemporary(w,h,24);var prior=RenderTexture.active;camera.targetTexture=target;camera.Render();RenderTexture.active=target;var pixels=new Texture2D(w,h,TextureFormat.RGB24,false);pixels.ReadPixels(new Rect(0,0,w,h),0,0);pixels.Apply();var encode=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)});File.WriteAllBytes(path,(byte[])encode.Invoke(null,new object[]{pixels}));RenderTexture.active=prior;camera.targetTexture=null;RenderTexture.ReleaseTemporary(target);Destroy(pixels);Destroy(go);lines.Add("CAPTURE full scene "+path);
 }
}
