#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B26Tests(string mode)
 {
  if(mode=="b26-hud"){var art=Room.GetComponent<B25ArtRoom>();Check(art.settings.fragment!=null,"HUD fragment sprite assigned");B26HudCapture.Save(art.settings.fragment,ProgressSave.FragmentCount%4,System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Evidence),"hud-fragment.png"));Record("CAPTURE live fragment/count in isolated RenderTexture; main HUD also uses same sprite/count; not full desktop screenshot");}
  else if(mode=="b26-room-names")yield return B26RoomNamesMeasure();
  else if(mode=="b26-white-flash")yield return B26FlashMeasure();
  else if(mode=="b26-player-art")yield return B26PlayerArtMeasure();
  else if(mode=="b26-sparkle")yield return B26SparkleMeasure();
  else if(mode=="b26-audio")yield return B26AudioMeasure();
  else if(mode=="b26-death")yield return B26DeathMeasure();
  else if(mode=="b26-measure")yield return B26Measure();
  else if(mode=="b26-route")yield return B26Route();
  else if(mode=="b26-loop")yield return B26Loop();
  else throw new ArgumentException(mode);
 }
 IEnumerator B26RoomNamesMeasure()
 {
  var data=B09Json.D(B09Json.Read(System.IO.File.ReadAllText("Design/v5/rooms_v5_AB.json"))["rooms"]);int count=0;
  foreach(var pair in data){yield return RoomAt(pair.Key);var notice=Room.GetComponent<B26RoomNotice>();string title=B09Json.S(B09Json.D(pair.Value),"title","").Split('—')[0].Trim(),expected=pair.Key+(title==""?"":" "+title);Check(notice!=null&&notice.roomName==expected&&Math.Abs(notice.seconds-1.2f)<.001,"room name matches source "+pair.Key);yield return Until(()=>!notice.Visible,"room name fades "+pair.Key,2);Record($"MEASURE B26 roomName={notice.roomName} visibleFor={Time.timeAsDouble-notice.Began:F3}");count++;}Check(count==12,"all twelve room names verified");
 }
 IEnumerator B26FlashOne(GameObject actor,string label)
 {
  var collider=actor.GetComponent<BoxCollider2D>();var size=collider.size;var offset=collider.offset;bool trigger=collider.isTrigger;var rootScale=actor.transform.localScale;int before=B26HitFlash.Emitted;double began=Time.timeAsDouble;actor.GetComponent<IDamageable>().ReceiveHit(1,Body.position);yield return Until(()=>B26HitFlash.LastEnded>began,"white flash ends "+label,2);Check(B26HitFlash.Emitted==before+1&&B26HitFlash.LastDuration>=.059&&B26HitFlash.LastDuration<.09,label+" white flash0.06s");if(actor!=null)Check(collider.size==size&&collider.offset==offset&&collider.isTrigger==trigger&&actor.transform.localScale==rootScale,label+" collider unchanged");Record($"MEASURE B26 whiteFlash actor={label} duration={B26HitFlash.LastDuration:F4} colliderUnchanged=True");
 }
 IEnumerator B26FlashMeasure()
 {
  foreach(string id in new[]{"A01","A02","B02","B03"}){yield return RoomAt(id);yield return new WaitForSeconds(.2f);var actor=id=="A01"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyPatrol>()!=null).Live:id=="A02"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyCharger>()!=null).Live:id=="B02"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyTurret>()!=null).Live:Room.GetComponentInChildren<E4Hover>().gameObject;yield return B26FlashOne(actor,id);}
  yield return B25Pose("A04",new Vector2(25.5f,1.015f));yield return GoX(27.5f);yield return Until(()=>Boss.Fighting&&!Boss.Introducing,"B1 live flash",4);yield return B26FlashOne(Boss.gameObject,"B1");
  yield return B25Pose("B04",new Vector2(20.5f,1.015f));yield return JumpTo(14.5f,12,2,"B2 flash pickup");yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B2 flash departure",3);yield return Until(()=>!Boss.Introducing,"B2 flash intro",3);yield return B26FlashOne(Boss.gameObject,"B2");
 }
 IEnumerator B26PlayerArtMeasure()
 {
  yield return B25Pose("A03",new Vector2(39,1.015f));yield return new WaitForSeconds(.2f);var fx=Player.GetComponentInChildren<B26PlayerArtMotion>();Check(fx!=null,"player sprite motion bound");var shape=Player.GetComponent<BoxCollider2D>();var size=shape.size;var offset=shape.offset;var scale=Player.transform.localScale;int jumps=fx.JumpEvents,lands=fx.LandingEvents,ghosts=fx.Afterimages;
  yield return Keys(Key.LeftArrow);yield return new WaitForSeconds(.2f);yield return Keys();Check(fx.GetComponent<SpriteRenderer>().flipX,"left input flips only sprite");yield return Keys(Key.RightArrow,Key.Z);yield return new WaitForSeconds(.25f);yield return Keys();yield return Until(()=>Motor.Grounded,"player art lands",2);yield return new WaitForSeconds(.15f);yield return Tap(Key.LeftShift);yield return new WaitForSeconds(.3f);Check(fx.JumpEvents>jumps&&fx.LandingEvents>lands&&fx.Afterimages-ghosts==3,"jump landing events and three dash afterimages");Check(shape.size==size&&shape.offset==offset&&Player.transform.localScale==scale&&Player.GetComponentsInChildren<Collider2D>().Length==Player.GetComponents<Collider2D>().Length,"player collider root unchanged and display children collider0");Record($"MEASURE B26 player jump={fx.JumpEvents-jumps} landing={fx.LandingEvents-lands} afterimages={fx.Afterimages-ghosts} eventSeconds={Room.GetComponent<B25ArtRoom>().settings.playerMotionSeconds:F3} colliderUnchanged=True");
 }
 IEnumerator B26SparkleMeasure()
 {
  yield return RoomAt("A05");Body.simulated=false;PlaceForAuthorizedSetup(new Vector2(40,2.615f));FindFirstObjectByType<RoomCameraRig>().BindAndSnap(Player);var sparkle=Room.GetComponentInChildren<B26IdleSparkle>();Check(Math.Abs(sparkle.settings.sparkleSeconds-1.5f)<.001,"sparkle cycle1.5");string folder=System.IO.Path.GetDirectoryName(Evidence);
  yield return Until(()=>sparkle.Brightness<.67,"sparkle low",2);sparkle.Refresh();B25CameraPng(System.IO.Path.Combine(folder,"sparkle-low.png"));Record($"MEASURE B26 sparkle low={sparkle.Brightness:F3}");yield return Until(()=>sparkle.Brightness>1.48,"sparkle high",2);sparkle.Refresh();B25CameraPng(System.IO.Path.Combine(folder,"sparkle-high.png"));Record($"MEASURE B26 sparkle high={sparkle.Brightness:F3} period={sparkle.settings.sparkleSeconds:F3}");Body.simulated=true;
 }
 IEnumerator B26AudioMeasure()
 {
  yield return new WaitForSeconds(.2f);var profile=B25EffectsHost.Active.profile;double bound=0;
  foreach(var sound in profile.sounds){var data=new float[sound.clip.samples*sound.clip.channels];Check(sound.clip.GetData(data,0),"PCM readable "+sound.eventName);float peak=data.Max(x=>Math.Abs(x));int voices=(int)Math.Ceiling(sound.clip.length/(Math.Max(.01,1-sound.pitchVariation)*profile.soundThrottle))+1;bound+=peak*sound.volume*voices;B25EffectsHost.Emit(sound.eventName,Body.position);Record($"MEASURE B26 audio event={sound.eventName} gain={sound.volume:F3} samplePeak={peak:F6} voices={voices}");}
  float actual=0;int clipped=0;var buffer=new float[1024];double end=Time.realtimeSinceStartupAsDouble+1;
  while(Time.realtimeSinceStartupAsDouble<end){AudioListener.GetOutputData(buffer,0);foreach(float x in buffer){actual=Mathf.Max(actual,Math.Abs(x));if(Math.Abs(x)>=1)clipped++;}yield return null;}
  var stress=new float[48000];foreach(var sound in profile.sounds){var pcm=new float[sound.clip.samples*sound.clip.channels];sound.clip.GetData(pcm,0);for(int i=0;i<stress.Length&&i<pcm.Length;i++)stress[i]+=pcm[i]*sound.volume;}for(int i=0;i<stress.Length;i++)stress[i]*=8;float post=B26AudioHeadroom.Limit(stress,out float pre,out int limitedClips);var meter=B25EffectsHost.Active.GetComponent<B26AudioHeadroom>();Record($"MEASURE B26 audio master={profile.masterVolume:F6} listenerPeak={actual:F6} clippedSamples={clipped} filterBlocks={meter.Blocks} filterOutputPeak={meter.OutputPeak:F6} PCMstressPre={pre:F6} PCMstressPost={post:F6} stressClips={limitedClips}; PCM stress is 8 simultaneous copies of all event samples, device listener separate");Check(post<=.851&&limitedClips==0&&clipped==0&&meter.ClippedSamples==0,"simultaneous voice clipping0 with normal-gain headroom filter");
 }
 IEnumerator B26DeathMeasure()
 {
  foreach(string id in new[]{"A01","A02","B02","B03"})
  {
   yield return RoomAt(id);yield return new WaitForSeconds(.2f);GameObject actor=id=="A01"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyPatrol>()!=null).Live:id=="A02"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyCharger>()!=null).Live:id=="B02"?Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyTurret>()!=null).Live:Room.GetComponentInChildren<E4Hover>().gameObject;
   int before=B26DeathDisplay.Spawned;double began=Time.timeAsDouble;actor.GetComponent<IDamageable>().ReceiveHit(999,Body.position);yield return null;Check(actor==null,"actor dies immediately "+id);Check(B26DeathDisplay.Spawned==before+1,"death display emitted "+id);yield return Until(()=>B26DeathDisplay.LastEnded>=began,"death fade complete "+id,2);double elapsed=B26DeathDisplay.LastEnded-began;yield return null;yield return Until(()=>FindObjectsByType<B26FadingSprite>(FindObjectsSortMode.None).Length==0,"death displays cleaned "+id,1);Record($"MEASURE B26 death room={id} elapsed={elapsed:F3} actorImmediate=True remainingDisplays={FindObjectsByType<B26FadingSprite>(FindObjectsSortMode.None).Length}");Check(elapsed>=.299&&elapsed<.36,"0.3s death fade "+id);
  }
 }
 // Same input recipe as the verified A04 branch; coordinates come from approved draft6.1.
 IEnumerator B26Island(float edge,float lamp,float top,float island,float height,string label)
 {
  int before=Player.GetComponent<PlayerAttack>().PogosPerformed,returns=Hazard.Returns;double next=0,end=Time.realtimeSinceStartupAsDouble+12;bool success=false;
  if(Body.position.x<edge-1.2f)yield return GoX(edge-1.2f);yield return new WaitForSeconds(Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03f);
  Input(Key.RightArrow);yield return Until(()=>Body.position.x>=edge-.1f,label+" runup",3);Input(Key.RightArrow,Key.Z);
  while(Time.realtimeSinceStartupAsDouble<end)
  {
   int count=Player.GetComponent<PlayerAttack>().PogosPerformed-before;float dx=(count==0?lamp+.3f:island)-Body.position.x;var keys=new List<Key>{Key.Z,Key.DownArrow};if(Math.Abs(dx)>.15f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(count==0&&!Motor.Grounded&&Body.linearVelocity.y<0&&Math.Abs(dx)<.7f&&Player.GetComponent<Collider2D>().bounds.min.y<=top+.4f&&Time.timeAsDouble>=next){keys.Add(Key.X);next=Time.timeAsDouble+Player.GetComponent<PlayerAttack>().tuning.attackCooldown+.03;}
   Input(keys.ToArray());if(count>=1&&Motor.Grounded&&Math.Abs(Body.position.y-height)<.12f&&Math.Abs(Body.position.x-island)<1){success=true;break;}if(Hazard.Returns>returns||Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
  }
  yield return Keys();Record($"MEASURE B26 island={label} pogos={Player.GetComponent<PlayerAttack>().PogosPerformed-before} landed={success} head={b25Head} returns={Hazard.Returns-returns} HP={Health.HP} pos={Body.position:F3}");Check(success,label+" island landing");
 }
 IEnumerator B26B02Bottom(bool whole)
 {
  if(whole){yield return GoX(9.5f);Input();var vent=Room.GetComponentsInChildren<SteamVent>().Single();yield return Until(()=>!vent.On&&Time.time%(vent.tuning.steamOn+vent.tuning.steamOff)<vent.tuning.steamOn+.2f,"B02 steam off window",6);}
  yield return B26Island(14,15.5f,2.8f,20,1.8f,"B02 floor lamps");yield return JumpTo(20.3f,23,1,"B02 island-floor");
 }
 IEnumerator B26B02E1()
 {
  Check(RoomSession.Instance.CurrentRoomId=="B02","B02 route remains in room before E1");
  var reset=Room.GetComponentsInChildren<EnemyReset>().FirstOrDefault(r=>r.Live!=null&&r.Live.GetComponent<EnemyPatrol>()!=null);var enemy=reset!=null?reset.Live:null;if(enemy==null){Record("MEASURE B26 B02 E1 absent at combat segment entry; no target fixture, proceed with actual input");yield break;}var attack=Player.GetComponent<PlayerAttack>();double end=Time.realtimeSinceStartupAsDouble+15,next=0;
  while(enemy!=null&&Health.HP>0&&!RoomSession.Instance.Respawning&&Time.realtimeSinceStartupAsDouble<end)
  {
   var keys=new List<Key>();float dx=enemy.transform.position.x-Body.position.x,reach=attack.tuning.attackRange+enemy.GetComponent<Collider2D>().bounds.extents.x+Player.GetComponent<Collider2D>().bounds.extents.x;
   if(Math.Abs(dx)>reach*.75f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
   if(Math.Abs(dx)<=reach&&Time.timeAsDouble>=next){keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);keys.Add(Key.X);next=Time.timeAsDouble+attack.tuning.attackCooldown+.04;}
   Input(keys.ToArray());yield return null;
  }
  yield return Keys();Record($"MEASURE B26 B02 E1 actualAttack defeated={enemy==null} HP={Health.HP} pos={Body.position:F3}");Check(enemy==null&&Health.HP>0,"B02 E1 actual input clear");
 }
 IEnumerator B26B02Upper()
 {
  yield return B25Jet(11.7f,8.5f,15,8,"B26 upper");yield return JumpTo(23.1f,27,8,"B02 upper gap");yield return GoX(31);yield return Keys();
  Check(ProgressSave.IsFragmentCollected("V5-B02-0"),"B02 actual upper fragment");yield return GoX(25.9f);Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=24.4f,"B02 broken upper descent x",2);Input();yield return Until(()=>Motor.Grounded&&Body.position.y<2,"B02 upper gap to safe floor",5);yield return Keys();
 }
 IEnumerator B26B06()
 {
  yield return B26Island(8,9.5f,2.8f,14,1.8f,"B06 first pair");yield return B26Island(15,16.5f,3.6f,21,1.8f,"B06 second pair");yield return B26Island(22,23.5f,3.6f,28,1,"B06 third pair");yield return GoX(31);yield return Keys();Check(ProgressSave.IsFragmentCollected("V5-B06-0"),"B06 actual fragment");
 }
 IEnumerator B26B05()
 {
  Motor.hasWallJump=true;yield return B25WallRise(Key.LeftArrow,()=>RoomSession.Instance.CurrentRoomId=="B05"&&Body.position.y>=20.2f,"B05 left pillar",24);Input(Key.LeftArrow);yield return Until(()=>Motor.Grounded&&Body.position.y>=18.9f&&Body.position.x<11,"B05 pillar top",4);yield return GoX(10);yield return Keys();Check(ProgressSave.IsFragmentCollected("V5-B05-0"),"B05 actual pillar fragment");
 }
 IEnumerator B26Measure()
 {
  string section=N5Args.Value("-b26-section","B02-floor");int trials=section=="B05"?3:5;
  for(int trial=1;trial<=trials;trial++)
  {
   string id=section.StartsWith("B02")?"B02":section;yield return B25Pose(id,section=="B02-floor"?new Vector2(13.5f,1.015f):section=="B05"?new Vector2(12.36f,10):new Vector2(3.5f,1.015f));
   b25Watch=true;b25Head=false;var watcher=StartCoroutine(B25WatchHead());int hp=Health.HP;double began=Time.timeAsDouble;
   if(section=="B02-floor")yield return B26B02Bottom(false);else if(section=="B02-upper")yield return B26B02Upper();else if(section=="B06")yield return B26B06();else if(section=="B05")yield return B26B05();else throw new ArgumentException(section);
   b25Watch=false;StopCoroutine(watcher);Record($"MEASURE B26 section={section} trial={trial}/{trials} seconds={Time.timeAsDouble-began:F3} hits={hp-Health.HP} head={b25Head} pos={Body.position:F3}");Check(!b25Head,section+" head0 trial="+trial);
  }
 }
 IEnumerator B26Route()
 {
  string id=N5Args.Value("-b25-route-room","");double began=Time.timeAsDouble;string target="";
  Record("FIXTURE independent room entry; live enemies; input-only after entry. Classification before retry in B26 route ledger.");
  if(id=="A03")
  {yield return B25Pose(id,new Vector2(3.5f,1.015f));target="A04";yield return JumpTo(14.8f,17,5,"A03 M roof from street",true);yield return JumpTo(21.7f,24,3,"A03 east box");yield return GoX(42.3f);yield return JumpTo(42.3f,44.8f,1,"A03 lift board");yield return LiftRide(true);}
  else if(id=="A05")
  {yield return B25Pose(id,new Vector2(46.5f,9.315f));target="A03";yield return GoX(40);yield return Until(()=>Motor.Grounded&&Body.position.y<2,"A05 clear shrine side landing",4);yield return JumpTo(40,37.5f,2.6f,"A05 cross shrine block");yield return JumpTo(30.6f,24.5f,1,"A05 sealed floor",true);yield return JumpTo(24.5f,18,1,"A05 E2",true);yield return GoX(15.5f);yield return JumpTo(12.3f,4.6f,1,"A05 trench",true);}
  else if(id=="A06")
  {yield return B25Pose(id,new Vector2(36.85f,1.015f));Motor.hasDoubleJump=true;target="A02";yield return JumpTo(36.85f,39,3,"A06 block");yield return JumpTo(39.1f,42.7f,5,"A06 exit platform");yield return JumpTo(43,46,5,"A06 maintain exit height",false);}
  else if(id=="B02")
  {yield return B25Pose(id,new Vector2(3.5f,1.015f));target="B03";yield return B26B02Upper();yield return JumpTo(24,30.8f,1,"B02 E2 dodge",true);yield return JumpTo(33.7f,41.2f,1,"B02 acid6",true);yield return B26B02E1();}
  else if(id=="B04")
  {yield return B25Pose(id,new Vector2(20.5f,1.015f));target="B03";yield return JumpTo(14.5f,12,2,"B04 pickup");yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B04 leave pickup",3);yield return Until(()=>!Boss.Introducing,"B04 intro finishes before wall approach",3);yield return B25FightInput();}
  else if(id=="B06")
  {yield return B25Pose(id,new Vector2(3.5f,1.015f));yield return B26B06();Record($"MEASURE B26 roomRoute=B06 challenge=True seconds={Time.timeAsDouble-began:F3}");yield break;}
  else throw new ArgumentException(id);
  Key direction=id=="A05"?Key.LeftArrow:Key.RightArrow;yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId==target,direction,id+" physical exit "+target,14);yield return Ready();Record($"MEASURE B26 roomRoute={id} target={target} seconds={Time.timeAsDouble-began:F3} HP={Health.HP}");
 }
 double b26StageAt;
 void B26Stage(string label){Record($"MEASURE B26 loop stage={label} seconds={Time.timeAsDouble-b26StageAt:F3} room={RoomSession.Instance.CurrentRoomId} pos={Body.position:F3} HP={Health.HP}");b26StageAt=Time.timeAsDouble;}
 IEnumerator B26LeftSpan(float edge,float target,float height,string label)
 {
  yield return GoX(edge+.4f);Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=edge,label+" takeoff",3);double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+5;bool rose=false,dashed=false,land=false;
  while(Time.realtimeSinceStartupAsDouble<end){var keys=new List<Key>{Key.LeftArrow};if(Time.timeAsDouble-began<.45)keys.Add(Key.Z);rose|=Body.linearVelocity.y>0;if(rose&&!dashed&&Body.linearVelocity.y<=.6f){dashed=true;keys.Add(Key.LeftShift);}Input(keys.ToArray());if(rose&&Motor.Grounded&&Math.Abs(Body.position.x-target)<1&&Math.Abs(Body.position.y-height)<.12f){land=true;break;}if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;}
  yield return Keys();Record($"MEASURE B26 leftSpan={label} landed={land} pos={Body.position:F3} HP={Health.HP}");Check(land,label+" lands");
 }
 IEnumerator B26Loop()
 {
  double began=Time.timeAsDouble;b26StageAt=began;Record("FIXTURE initial CP-A03 only. Every stage below uses actual inputs; no HP/pose/enemy/boss/ability writes. First blocked stage ends the run.");
  B26Stage("begin CP-A03 to A-loop");yield return B24Loop();B26Stage("A04 B1 A05 Dash A03 practice ledge Continue");
  yield return GoX(44.8f);yield return LiftRide(true);yield return JumpTo(43.75f,36.7f,13,"practice1",true);yield return JumpTo(35.25f,28.1f,13,"practice2",true);yield return JumpTo(26.75f,20.8f,15,"practice3 shelf",true);yield return GoX(4.7f);Input(Key.LeftArrow,Key.Z);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="B01","B26 physical ceiling link",3);yield return Keys();yield return Ready();B26Stage("A03 ledge B01");
  yield return JumpTo(12.5f,16.3f,3,"B01 market");yield return GoX(31.8f);yield return JumpTo(31.8f,34.5f,3,"B01 crate");yield return JumpTo(35.5f,38.5f,5,"B01 roof");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B02",Key.RightArrow,"B01-B02",5);yield return Ready();B26Stage("B01 roof B02");
  yield return B26B02Bottom(true);yield return JumpTo(24,30.8f,1,"B02 E2",true);yield return JumpTo(33.7f,41.2f,1,"B02 acid6",true);yield return B26B02E1();yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.RightArrow,"B02-B03",5);yield return Ready();B26Stage("B02 bottom B03");
  yield return B25Jet(13.6f,8.5f,10,8,"B03 lower");yield return B25Jet(7.7f,17.5f,4.3f,17,"B03 upper");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B04",Key.LeftArrow,"B03-B04",4);yield return Ready();B26Stage("B03 steam B04");
  yield return JumpTo(14.5f,12,2,"B04 wall pickup");yield return Until(()=>Motor.hasWallJump,"actual wall jump acquired",2);yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B04 leave pickup",3);yield return Until(()=>!Boss.Introducing,"B04 intro",3);yield return B25FightInput();B26Stage("B04 actual B2 victory");
  yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.RightArrow,"B04-B03",6);yield return Ready();yield return GoX(15.8f);Input();yield return Until(()=>Motor.Grounded&&Body.position.y<2,"B03 lower access",4);yield return GoX(20.7f);yield return B25WallRise(Key.RightArrow,()=>RoomSession.Instance.CurrentRoomId=="B06","B03-B06");yield return Ready();yield return B26B06();B26Stage("B03 chimney B06 fragment");
  yield return B26LeftSpan(27.15f,21,1.8f,"B06 return third pair");yield return B26LeftSpan(20.2f,14,1.8f,"B06 return second pair");yield return B26LeftSpan(13.2f,7,1,"B06 return first pair");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B03",Key.LeftArrow,"B06-B03",5);yield return Ready();Input();yield return Until(()=>Motor.Grounded&&Body.position.y<2,"B03 chimney descent",5);yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B02",Key.LeftArrow,"B03-B02",7);yield return Ready();B26Stage("B06 return B03 B02");
  yield return JumpTo(55,47.5f,1,"B02 return E1",true);yield return GoX(40.4f);yield return B25LateDashLeft(40.1f,33,"B02 return acid6");yield return GoX(24.5f);yield return B26LeftSpan(22.15f,20,1.8f,"B02 return island");yield return B26LeftSpan(19.15f,13.5f,1,"B02 return lamps");yield return B26B02Upper();B26Stage("B02 upper fragment descent");
  yield return B26LeftSpan(22.15f,20,1.8f,"B02 final island");yield return B26LeftSpan(19.15f,13.5f,1,"B02 final lamps");yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B01",Key.LeftArrow,"B02-B01",5);yield return Ready();B26Stage("B02 B01");
  yield return GoX(5);yield return B25WallRise(Key.RightArrow,()=>Body.position.y>=15.2f,"B01 tower");Input(Key.RightArrow);yield return Until(()=>Motor.Grounded&&Body.position.y>=14.9f&&Body.position.x>=7.3f,"B01 roof",3);yield return Keys();Input(Key.LeftArrow);yield return Until(()=>Body.position.x<=7.05f,"B01 ledge takeoff",3);Input(Key.LeftArrow,Key.Z);yield return Until(()=>Motor.Grounded&&Body.position.y>=16.9f,"B01 ledge",3);yield return Keys();yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="B05",Key.LeftArrow,"B01-B05",4);yield return Ready();B26Stage("B01 tower B05");
  yield return GoX(14);yield return Until(()=>RoomSession.Instance.CurrentRoomId=="A02","B05-A02 hole",3);yield return Ready();B26Stage("B05 A02 chimney");Check(ProgressSave.TryLoad(out var saved)&&saved.dash&&saved.wallJump&&saved.defeatedBossIds.Contains("B1-A04")&&saved.defeatedBossIds.Contains("B2-B04"),"full AB save both abilities and bosses");yield return Title();yield return ContinueGame();Check(Motor.hasWallJump&&Player.GetComponent<PlayerDash>().hasDash,"final Continue abilities");Record($"MEASURE B26 full AB loop seconds={Time.timeAsDouble-began:F3} complete=True");
 }
}
#endif
