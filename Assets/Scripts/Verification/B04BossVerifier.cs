using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
// B04 approach 1: physics-tick input, distance-based collision ETA and sustained double jump.
// Prior B03 approach used a render-frame single-tick second press and cut the second jump.
public sealed class B04BossVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B04BossVerifier>("-verify-b04-boss");
 BossEncounter boss;N9PogoInput driver;int hits;double jumped=-100;bool second;float peak;
 void Hit(){hits++;lines.Add($"HIT {boss?.State} player={Body.position} boss={boss?.transform.position}");}
 void Place(Vector2 p){Player.ClearTransientInput();Body.position=p;Player.transform.position=p;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Arena(){driver.produce=null;yield return Keys();if(RoomSession.Instance.CurrentRoomId!="C04"){Check(RoomSession.Instance.RequestTransition("C04","Assets/Scenes/C04.unity","CP-C04"),"fixture C04");yield return Ready();}Check(RoomSession.Instance.RequestTransition("C06","Assets/Scenes/C06.unity","FromLeft"),"fresh arena");yield return Ready();boss=FindFirstObjectByType<BossEncounter>();
 #if UNITY_EDITOR
 boss.tuning=UnityEditor.AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/"+N5Args.Value("-b03-candidate","B2")+"/BossTuning.asset");
 #endif
 Check(boss.tuning.extendedPatterns,"candidate active");Health.RestoreMax();Place(new Vector2(11,1.81f));yield return Until(()=>boss.Fighting,"fight");yield return new WaitForSeconds(.1f);hits=0;}
 void Pattern(int i){typeof(BossEncounter).GetMethod("BeginPattern",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(boss,new object[]{i});lines.Add("FIXTURE fresh isolated pattern="+i+"; no health/position writes during dodge outcome");}
 protected override void Cleanup(){if(driver!=null)driver.produce=null;if(Health!=null)Health.Hit-=Hit;}
 protected override IEnumerator Tests(){yield return ContinueGame();Health.Hit+=Hit;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;
  foreach(bool dodge in new[]{true,false}){yield return Arena();Pattern(3);jumped=-100;second=false;peak=Body.position.y;int dj=Motor.DoubleJumpsPerformed;
   driver.produce=()=>{peak=Mathf.Max(peak,Body.position.y);if(!dodge)return Array.Empty<Key>();float distance=Mathf.Abs(boss.transform.position.x-Body.position.x)-boss.GetComponent<BoxCollider2D>().bounds.extents.x-Player.GetComponent<BoxCollider2D>().bounds.extents.x;if(jumped<0&&boss.State==BossEncounter.Phase.Charge&&distance/boss.tuning.chargeSpeed<=.65f){jumped=Time.timeAsDouble;lines.Add($"INPUT ground jump collisionETA={distance/boss.tuning.chargeSpeed:F3}");}double t=Time.timeAsDouble-jumped;double delay=boss.tuning.chargeSpeed<8?.34:.24;if(jumped>=0&&(t<delay-.06||(t>=delay&&t<delay+.4))){if(t>=delay&&!second){second=true;lines.Add("INPUT sustained second jump .40s");}return new[]{Key.Z};}return Array.Empty<Key>();};
   yield return Until(()=>boss.State==BossEncounter.Phase.ChargeRecovery,"charge resolution",10);driver.produce=null;yield return Keys();lines.Add($"MEASURE fps={Application.targetFrameRate} candidate={N5Args.Value("-b03-candidate","B2")} charge dodge={dodge} hitEvents={hits} DJ={Motor.DoubleJumpsPerformed-dj} peak={peak:F3}");Check(hits==(dodge?0:1),"charge dodge0 / fail1");if(dodge)Check(Motor.DoubleJumpsPerformed>dj,"actual second jump consumed");}
  foreach(int i in new[]{1,3}){yield return Arena();Place(new Vector2(6,1.81f));Pattern(i);yield return Until(()=>boss.State==(i==1?BossEncounter.Phase.LeapFlight:BossEncounter.Phase.Charge),"active pattern");yield return Tap(Key.Escape);Check(GameState.GetOrCreate().Paused,"paused");Vector3 p=boss.transform.position;double time=Time.timeAsDouble;yield return new WaitForSecondsRealtime(.3f);Check(boss.transform.position==p&&Time.timeAsDouble==time,"pattern/clock frozen");yield return Tap(Key.Escape);Check(!GameState.GetOrCreate().Paused,"resumed");yield return null;Check(Time.timeAsDouble>time,"clock restarted");Check(Health.TakeDamage(Health.HP,Player.transform.position),"lethal setup");yield return Until(()=>RoomSession.Instance.Respawning,"death");yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04"&&Health.HP==Health.tuning.playerMaxHP,"CP-C04 HP restored");yield return Arena();Check(boss.HP==boss.tuning.maxHP&&boss.State==BossEncounter.Phase.MeleeTell,"fresh boss HP / pattern reset");}
 }
}
