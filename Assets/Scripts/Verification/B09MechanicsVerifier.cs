using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public sealed class B09MechanicsVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09MechanicsVerifier>("-verify-b09-mechanics");
 protected override IEnumerator Tests()
 {
  SceneManager.LoadScene("Assets/Scenes/Test/MechanicsTest.unity");yield return Ready();yield return new WaitForSeconds(.2f);
  Check(!ProgressSave.Enabled,"mechanics course never writes save");
  var slam=Player.GetComponent<PlayerSlam>();Check(slam!=null&&slam.hasSlam,"course grants Slam");
  string mode=N5Args.Value("-verify-b09-mechanics","slam");if(mode=="slam")yield return Slam(slam);else if(mode=="extra")yield return Extra();else yield return Devices(slam);
 }
 IEnumerator Setup(Vector2 position){yield return Keys();Player.ClearTransientInput();PlaceForAuthorizedSetup(position);yield return new WaitForSeconds(.08f);}
 IEnumerator Slam(PlayerSlam slam)
 {
  yield return Setup(new Vector2(5,1.81f));yield return Keys(Key.DownArrow,Key.C);yield return new WaitForSeconds(.04f);Check(!slam.Slamming,"ground Slam blocked");yield return Keys();
  yield return new WaitForSeconds(.5f);yield return Setup(new Vector2(8,6));yield return Keys(Key.C);yield return new WaitForSeconds(.02f);Check(Player.GetComponent<PlayerDash>().IsDashing,"dash fixture");yield return Keys(Key.DownArrow,Key.C);Check(!slam.Slamming,"Slam while dashing blocked");
  var seal=FindFirstObjectByType<SealFloor>();yield return Setup(new Vector2(22.5f,4));yield return Keys(Key.DownArrow,Key.X);yield return new WaitForSeconds(.35f);Check(!seal.Broken,"regular down attack does not break Seal");
  yield return Setup(new Vector2(22.5f,6));int before=slam.Landings;yield return Keys(Key.DownArrow,Key.C);Check(slam.Slamming,"airborne down+dash starts Slam");Check(Body.linearVelocity.x==0&&Body.linearVelocity.y==-slam.tuning.slamSpeed,"Slam fixed velocity");yield return Keys();yield return Until(()=>slam.Landings>before,"Slam landing",2);Check(seal.Broken,"Slam breaks Seal");
  yield return Setup(new Vector2(7,1.81f));int dj=Motor.DoubleJumpsPerformed;yield return Keys(Key.Z);yield return new WaitForSeconds(.15f);yield return Keys();yield return Keys(Key.Z);yield return new WaitForSeconds(.06f);Check(Motor.DoubleJumpsPerformed==dj+1,"double jump input unchanged");
  var lantern=FindObjectsByType<PogoTarget>(FindObjectsSortMode.None)[0];yield return Setup(new Vector2(lantern.transform.position.x,lantern.GetComponent<Collider2D>().bounds.max.y+1.3f));int pogo=Player.GetComponent<PlayerAttack>().PogosPerformed;yield return Keys(Key.DownArrow,Key.X);yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>pogo,"lantern pogo",2);Check(!slam.Slamming,"down attack remains Pogo");
 }
 IEnumerator Devices(PlayerSlam slam)
 {
  var pad=FindFirstObjectByType<SlamPad>();yield return Setup(new Vector2(pad.transform.position.x,6));yield return Until(()=>Motor.Grounded,"ordinary pad landing",2);Check(pad.Bounces==0,"ordinary landing pad response zero");
  yield return Setup(new Vector2(pad.transform.position.x,6));yield return Keys(Key.DownArrow,Key.C);yield return Keys();yield return Until(()=>pad.Bounces==1,"Slam pad bounce",2);float baseY=pad.GetComponent<Collider2D>().bounds.max.y+Player.GetComponent<Collider2D>().bounds.extents.y,max=Body.position.y;while(Body.linearVelocity.y>0){max=Mathf.Max(max,Body.position.y);yield return null;}float rise=max-baseY;lines.Add("MEASURE pad-rise="+rise);Check(Mathf.Abs(rise-7.5f)<=.3f,"pad apex 7.5u +/- .3u");
  var vent=FindFirstObjectByType<SteamVent>();yield return Until(()=>!vent.On,"steam off",5);yield return Setup(new Vector2(52,1.81f));yield return new WaitForSeconds(.1f);Check(Body.linearVelocity.y<=0,"steam off upward zero");yield return Until(()=>vent.On,"steam on",5);yield return new WaitForSeconds(.2f);Check(Body.linearVelocity.y>7.9f,"steam on pushes 8u/s");
  var crumble=FindFirstObjectByType<CrumblePlatform>();yield return Setup(new Vector2(crumble.transform.position.x,crumble.GetComponent<Collider2D>().bounds.max.y+.81f));double begin=Time.timeAsDouble;yield return Until(()=>crumble.Falling,"crumble fall",2);lines.Add("MEASURE crumble delay="+(Time.timeAsDouble-begin));Check(Time.timeAsDouble-begin>=.55&&Time.timeAsDouble-begin<.8,"crumble around .6s");double fell=Time.timeAsDouble;yield return Until(()=>!crumble.Falling,"crumble restore",4);lines.Add("MEASURE crumble restore="+(Time.timeAsDouble-fell));Check(Time.timeAsDouble-fell>=2.9,"crumble restores after 3s");
  var lift=FindFirstObjectByType<LeverLift>();Check(!lift.Unlocked&&Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.high)<.05f,"lift starts fixed above");var lever=FindObjectsByType<LiftLever>(FindObjectsSortMode.None);var upper=System.Array.Find(lever,l=>l.upper);yield return Setup(new Vector2(upper.transform.position.x-.8f,upper.transform.position.y));yield return Keys(Key.RightArrow,Key.X);yield return Until(()=>lift.Unlocked,"upper lever hit",2);yield return Keys();yield return Until(()=>Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.low)<.05f,"lift down",4);Check(ProgressSave.IsWallBroken(lift.liftId+"-unlocked")&&ProgressSave.IsWallBroken(lift.liftId+"-down"),"lift state recorded");
  var e4=FindFirstObjectByType<E4Hover>();Check(e4.HP==2,"E4 health2");yield return Setup(new Vector2(e4.transform.position.x,e4.GetComponent<Collider2D>().bounds.max.y+1.3f));int before=Player.GetComponent<PlayerAttack>().PogosPerformed;yield return Keys(Key.DownArrow,Key.X);yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>before,"E4 pogo",2);Check(e4==null||e4.HP==1,"E4 down hit damage1");
 }
 IEnumerator Extra()
 {
  var spike=System.Array.Find(FindObjectsByType<PogoTarget>(FindObjectsSortMode.None),p=>p.GetComponent<DamageBlock>()!=null);Health.RestoreMax();int hp=Health.HP;yield return Setup(spike.transform.position);yield return new WaitForSeconds(.1f);Check(Health.HP==hp-1,"spike contact damage1");
  yield return Setup(new Vector2(5,1.81f));yield return new WaitForSeconds(1);Health.RestoreMax();var e4=FindFirstObjectByType<E4Hover>();float min=e4.transform.position.y,max=min;double end=Time.timeAsDouble+2.1;while(Time.timeAsDouble<end){min=Mathf.Min(min,e4.transform.position.y);max=Mathf.Max(max,e4.transform.position.y);yield return null;}lines.Add("MEASURE E4 vertical span="+(max-min));Check(Mathf.Abs(max-min-1)<.03f,"E4 hover +/- .5u");hp=Health.HP;yield return Setup(e4.transform.position);yield return new WaitForSeconds(.05f);Check(Health.HP==hp-1,"E4 contact damage1");
  var lift=FindFirstObjectByType<LeverLift>();var upper=System.Array.Find(FindObjectsByType<LiftLever>(FindObjectsSortMode.None),l=>l.upper);yield return Setup(new Vector2(upper.transform.position.x-.8f,upper.transform.position.y));yield return Keys(Key.RightArrow,Key.X);yield return Until(()=>lift.Unlocked,"upper lever",2);yield return Keys();yield return Until(()=>Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.low)<.05f,"lift bottom",4);var lower=System.Array.Find(FindObjectsByType<LiftLever>(FindObjectsSortMode.None),l=>!l.upper);yield return Setup(new Vector2(lower.transform.position.x-.8f,lower.transform.position.y));yield return Keys(Key.RightArrow,Key.X);yield return Keys();yield return Until(()=>Mathf.Abs(lift.GetComponent<Collider2D>().bounds.max.y-lift.high)<.05f,"lower lever calls up",4);
  Check(N5Args.Has("-save-path"),"explicit isolated persistence file");ProgressSave.Enabled=true;ProgressSave.SaveCurrent(RoomSession.Instance);Check(ProgressSave.TryLoad(out var saved)&&System.Array.Exists(saved.brokenWalls,id=>id==lift.liftId+"-unlocked")&&!System.Array.Exists(saved.brokenWalls,id=>id==lift.liftId+"-down"),"lift unlocked/up persisted on disk");ProgressSave.Enabled=false;
  var presets=FindFirstObjectByType<MechanicsTestPresets>();yield return Tap(Key.F);Check(lift.tuning.liftSpeed==4&&FindFirstObjectByType<CrumblePlatform>().tuning.crumbleDelay==.4f,"F candidate1");yield return Tap(Key.F);Check(FindFirstObjectByType<SteamVent>().tuning.steamOn==2.5f,"F candidate2");yield return Tap(Key.F);Check(lift.tuning.liftSpeed==3&&presets.baseline.steamOn==2&&presets.baseline.crumbleDelay==.6f,"F returns baseline without asset mutation");
 }
}
