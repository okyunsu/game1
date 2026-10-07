using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Explicit pattern fixtures are separate from the continuous input-only fight.
public sealed class B03BossVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize()=>N5Args.Start<B03BossVerifier>("-verify-b03-boss");
 BossEncounter boss;int received;void Hit(){received++;lines.Add($"HIT phase={boss?.State} x={Body.position.x:F3} y={Body.position.y:F3} boss={boss?.transform.position}");}
 void Place(Vector2 at){Player.ClearTransientInput();Body.position=at;Player.transform.position=at;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
 IEnumerator Arena(){yield return Keys();if(RoomSession.Instance.CurrentRoomId!="C04"){Check(RoomSession.Instance.RequestTransition("C04","Assets/Scenes/C04.unity","CP-C04"),"arena fixture return");yield return Ready();}Check(RoomSession.Instance.RequestTransition("C06","Assets/Scenes/C06.unity","FromLeft"),"fresh arena fixture");yield return Ready();boss=FindFirstObjectByType<BossEncounter>();
 #if UNITY_EDITOR
 string id=N5Args.Value("-b03-candidate","B2");boss.tuning=UnityEditor.AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/"+id+"/BossTuning.asset");
 #endif
 Check(boss.tuning.extendedPatterns,"runtime extended candidate");Health.RestoreMax();received=0;Place(new Vector2(11,1.81f));yield return Until(()=>boss.Fighting,"start combat");}
 void Pattern(int index){typeof(BossEncounter).GetMethod("BeginPattern",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(boss,new object[]{index});lines.Add("FIXTURE isolated pattern="+index+" player=(11,1.81), fresh arena, no health writes during outcome");}
 protected override void Cleanup(){if(Health!=null)Health.Hit-=Hit;}
 protected override IEnumerator Tests(){yield return ContinueGame();Health.Hit+=Hit;
  foreach(int index in new[]{1,3})foreach(bool dodge in new[]{true,false}){
   yield return Arena();Pattern(index);double end=Time.realtimeSinceStartupAsDouble+10;bool dashed=false,jumped=false,second=false;double jumpAt=0;
   while(boss.State!=(index==1?BossEncounter.Phase.LeapRecovery:BossEncounter.Phase.ChargeRecovery)){
    if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("pattern deadline");float elapsed=(float)(Time.timeAsDouble-boss.PhaseStarted);var keys=new List<Key>();
    if(dodge&&index==1){if(boss.State==BossEncounter.Phase.LeapTell&&elapsed>=boss.tuning.leapTell-.3f&&!dashed)keys.Add(Key.LeftArrow);if(boss.State==BossEncounter.Phase.LeapTell&&elapsed>=boss.tuning.leapTell-.2f&&!dashed){keys.Add(Key.LeftArrow);keys.Add(Key.C);dashed=true;}if(boss.State==BossEncounter.Phase.LeapFlight&&elapsed>=boss.tuning.leapFlight*.85f&&!jumped){jumped=true;jumpAt=Time.timeAsDouble;}if(jumped&&Time.timeAsDouble-jumpAt<.55)keys.Add(Key.Z);}
    if(dodge&&index==3){if(boss.State==BossEncounter.Phase.Charge&&elapsed>=.15f&&!jumped){jumped=true;jumpAt=Time.timeAsDouble;}if(jumped&&Time.timeAsDouble-jumpAt<.16)keys.Add(Key.Z);if(jumped&&Time.timeAsDouble-jumpAt>.23&&!second){keys.Add(Key.Z);second=true;}}
    InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));yield return null;
   }
   if(index==1&&dodge){yield return Keys(Key.Z);yield return new WaitForSeconds(.45f);}else yield return Keys();
   lines.Add($"MEASURE fps={Application.targetFrameRate} pattern={index} dodge={dodge} hitEvents={received} HP={Health.HP} pos={Body.position}");Check(received==(dodge?0:1),"designated dodge zero hits / failed dodge one event");
  }
  foreach(int index in new[]{1,3}){
   yield return Arena();Place(new Vector2(6,1.81f));Pattern(index);yield return Until(()=>boss.State==(index==1?BossEncounter.Phase.LeapFlight:BossEncounter.Phase.Charge),"active new pattern");yield return Tap(Key.Escape);Check(GameState.GetOrCreate().Paused,"Pause input");Vector3 position=boss.transform.position;double phaseTime=Time.timeAsDouble;yield return new WaitForSecondsRealtime(.3f);Check(boss.transform.position==position&&Time.timeAsDouble==phaseTime,"active pattern frozen");yield return Tap(Key.Escape);Check(!GameState.GetOrCreate().Paused,"resumed");yield return null;Check(Time.timeAsDouble>phaseTime,"scaled clock resumes");
   Check(Health.TakeDamage(Health.HP,Player.transform.position),"lethal fixture during new pattern");yield return Until(()=>RoomSession.Instance.Respawning,"death queued");yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="C04"&&RoomSession.Instance.CheckpointId=="CP-C04"&&Health.HP==Health.tuning.playerMaxHP,"CP-C04 reset");yield return Arena();Check(boss.HP==boss.tuning.maxHP&&boss.State==BossEncounter.Phase.MeleeTell,"fresh HP / first pattern");
  }
  yield return Arena();Check(Health.TakeDamage(Health.HP,Player.transform.position),"same-frame lethal player fixture");boss.ReceiveHit(boss.HP,Player.transform.position);yield return null;Check(boss.HP==0&&!RoomSession.Instance.Respawning&&boss.leftExit.enabled,"boss HP0 wins same-frame death / exit unlock");
 }
}

public sealed class B03BossFight:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize()=>N5Args.Start<B03BossFight>("-verify-b03-fight");
 int hits;void Hit()=>hits++;
 string activePattern;int patternHits;bool patternResolved;readonly Dictionary<string,int> attempts=new(),dodges=new();
 void FinishPattern(){if(activePattern==null)return;if(!patternResolved){lines.Add("NOT MEASURED attack resolution absent "+activePattern);activePattern=null;return;}attempts.TryGetValue(activePattern,out int n);attempts[activePattern]=n+1;dodges.TryGetValue(activePattern,out int safe);dodges[activePattern]=safe+(hits==patternHits?1:0);activePattern=null;}
 protected override IEnumerator Tests(){yield return ContinueGame();yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId=="C06",Key.RightArrow,"actual C04-C06 entry");yield return Ready();var boss=FindFirstObjectByType<BossEncounter>();
 #if UNITY_EDITOR
 string id=N5Args.Value("-b03-candidate","basic");if(id!="basic")boss.tuning=UnityEditor.AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/BossCandidates/"+id+"/BossTuning.asset");
 #endif
 Check(boss.tuning.extendedPatterns==(N5Args.Value("-b03-candidate","basic")!="basic"),"runtime candidate flag matches requested build");
 Health.Hit+=Hit;yield return WalkTo(()=>boss.Fighting,Key.RightArrow,"combat input start");double start=Time.timeAsDouble,realStart=Time.realtimeSinceStartupAsDouble,attackAt=0,jumpAt=-100;int tick=0;var counts=new Dictionary<string,int>();string previous="";
 while(boss!=null&&boss.HP>0){if(Time.realtimeSinceStartupAsDouble-realStart>240)throw new Exception("input fight deadline HP="+boss.HP);if(RoomSession.Instance.Respawning)throw new Exception("input fight death HP="+boss.HP+" hits="+hits);tick++;var keys=new List<Key>();float dx=boss.transform.position.x-Body.position.x;string state=boss.State.ToString();if(previous!=state){previous=state;if(state.EndsWith("Tell")){FinishPattern();activePattern=state.Replace("Tell","");patternHits=hits;patternResolved=false;}else if(state=="Walk")FinishPattern();else if(state.EndsWith("Recovery"))patternResolved=true;counts.TryGetValue(state,out int count);counts[state]=count+1;lines.Add($"PHASE {state} x={Body.position.x:F2} y={Body.position.y:F2} bossX={boss.transform.position.x:F2} hp={Health.HP} bossHP={boss.HP}");}
 if(!boss.tuning.extendedPatterns){
  if(Body.position.x<15.65f)keys.Add(Key.RightArrow);else if(Body.position.x>15.85f)keys.Add(Key.LeftArrow);
  float elapsed=(float)(Time.timeAsDouble-boss.PhaseStarted);if(Motor.Grounded&&((boss.State==BossEncounter.Phase.MeleeTell&&elapsed>boss.tuning.meleeTell-.38)||(boss.State==BossEncounter.Phase.WaveTell&&elapsed>boss.tuning.waveTell-.38)))jumpAt=Time.timeAsDouble;
  if(Time.timeAsDouble-jumpAt<.55)keys.Add(Key.Z);if(Motor.Grounded&&Time.timeAsDouble>=attackAt){keys.Remove(Key.LeftArrow);if(!keys.Contains(Key.RightArrow))keys.Add(Key.RightArrow);keys.Add(Key.X);attackAt=Time.timeAsDouble+.4;}
 }else{
  // Stay above the boss, track horizontally, and pogo with actual Down+X.
  if(Mathf.Abs(dx)>.25f)keys.Add(dx>0?Key.RightArrow:Key.LeftArrow);
  if(Motor.Grounded){keys.Add(Key.Z);jumpAt=Time.timeAsDouble;}
  else if(Time.timeAsDouble-jumpAt<.25)keys.Add(Key.Z);
  else if(Body.linearVelocity.y<0&&Motor.DoubleJumpAvailable&&Body.position.y<5.3f){keys.Add(Key.Z);jumpAt=Time.timeAsDouble;}
  var pb=Player.GetComponent<BoxCollider2D>().bounds;var bb=boss.GetComponent<BoxCollider2D>().bounds;
  if(Time.timeAsDouble>=attackAt&&Mathf.Abs(dx)<1.2f&&pb.min.y-bb.max.y<1.7f&&pb.min.y>bb.center.y){keys.Remove(Key.Z);keys.Add(Key.DownArrow);keys.Add(Key.X);attackAt=Time.timeAsDouble+.37;}
 }
 InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys.ToArray()));yield return null;
 }
 FinishPattern();lines.Add($"MEASURE candidate={N5Args.Value("-b03-candidate","build")} killGameSeconds={Time.timeAsDouble-start:F3} killRealSeconds={Time.realtimeSinceStartupAsDouble-realStart:F3} hits={hits}");foreach(var pair in counts)lines.Add($"MEASURE observed {pair.Key}={pair.Value}");foreach(var pair in attempts)lines.Add($"MEASURE dodge {pair.Key} safe={dodges[pair.Key]} attempts={pair.Value}");yield return Keys();yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"actual input victory Ending");Check(ProgressSave.TryLoad(out var saved)&&saved.bossDefeated&&saved.endingReached,"saved boss and ending");yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"Ending to title");Check(true,"actual input defeat-Ending-title");
 }
}
