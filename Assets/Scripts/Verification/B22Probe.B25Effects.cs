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
}
#endif
