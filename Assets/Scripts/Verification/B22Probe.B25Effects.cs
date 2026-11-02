#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 int B25Sounds(string id)=>B25EffectsHost.Played.TryGetValue(id,out int n)?n:0;
 IEnumerator B25CombatEffects()
 {
  yield return RoomAt("A02");yield return new WaitForSeconds(.2f);var host=B25EffectsHost.Active;Check(host!=null,"B25 shared audio profile active");
  int swing=B25Sounds("swing"),hit=B25Sounds("hit"),hurt=B25Sounds("hurt"),death=B25Sounds("death"),bursts=B25EffectsHost.Bursts;
  yield return Tap(Key.X);Check(B25Sounds("swing")>swing,"actual attack activation sound");
  var enemy=Room.GetComponentsInChildren<EnemyReset>().First(r=>r.Live.GetComponent<EnemyPatrol>()!=null).Live.GetComponent<EnemyPatrol>();
  Record("FIXTURE real ReceiveHit/TakeDamage APIs exercise presentation events; not a player-combat victory measurement");enemy.ReceiveHit(1,Body.position);yield return null;enemy.ReceiveHit(enemy.HP,Body.position);yield return null;
  Check(Health.TakeDamage(1,Body.position),"real hurt event fixture");yield return null;
  Check(B25Sounds("hit")>hit&&B25Sounds("death")>death&&B25Sounds("hurt")>hurt&&B25EffectsHost.Bursts>=bursts+3,"hit/death/hurt event audio and short fragments");
  Record($"MEASURE B25 combat swing={B25Sounds("swing")-swing} hit={B25Sounds("hit")-hit} death={B25Sounds("death")-death} hurt={B25Sounds("hurt")-hurt} bursts={B25EffectsHost.Bursts-bursts} sourcePlaying={host.AudioPlaying} lifetime={host.profile.fragmentSeconds:F3}");
  yield return new WaitForSeconds(host.profile.fragmentSeconds+.1f);Check(FindObjectsByType<PresentationFragment>(FindObjectsSortMode.None).Length==0,"short fragments expire without colliders");
 }
 IEnumerator B25DeviceEffects()
 {
  int fragment=B25Sounds("fragment"),lift=B25Sounds("lift"),steam=B25Sounds("steam"),locked=B25Sounds("doorLock"),opened=B25Sounds("doorOpen");
  yield return RoomAt("B01");var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).Single();Record("FIXTURE actual fragment trigger contact for audio event, not route proof");PlaceForAuthorizedSetup((Vector2)shard.transform.position+Vector2.down*.5f);yield return Until(()=>B25Sounds("fragment")>fragment,"fragment sound",2);
  yield return B25Pose("A03",new Vector2(45.85f,1.015f));yield return Tap(Key.X);yield return Until(()=>B25Sounds("lift")>lift,"actual lever audio",2);
  yield return RoomAt("B03");PlaceForAuthorizedSetup(new Vector2(13.6f,1.015f));yield return Until(()=>B25Sounds("steam")>steam,"actual steam cycle audio",5);
  yield return B25Pose("B04",new Vector2(20.5f,1.015f));yield return JumpTo(14.5f,12,2,"B04 audio acquisition");yield return WalkTo(()=>Body.position.x>=15.2f,Key.RightArrow,"B04 audio depart",3);Check(Boss.Introducing&&B25Sounds("doorLock")>locked,"actual boss lock audio");
  yield return Until(()=>!Boss.Introducing,"audio intro ends",3);Record("FIXTURE lethal ReceiveHit measures actual door-open event only, not player victory");Boss.ReceiveHit(Boss.HP,Body.position);yield return null;Check(B25Sounds("doorOpen")>opened,"actual defeated gate open audio");
  Record($"MEASURE B25 devices fragment={B25Sounds("fragment")-fragment} lever={B25Sounds("lift")-lift} steam={B25Sounds("steam")-steam} lock={B25Sounds("doorLock")-locked} open={B25Sounds("doorOpen")-opened}");
 }

}
#endif
