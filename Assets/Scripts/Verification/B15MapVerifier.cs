#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B15MapVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B15MapVerifier>("-verify-b15-map");
 protected override IEnumerator Tests()
 {
  yield return NewGame();
  Check(FindObjectsByType<WorldMapSource>(FindObjectsSortMode.None).Length==0,"layout bootstrap retired after loading");
  if(!V4SceneMap.Enabled||N5Args.Has("-no-map")){Check(FindObjectsByType<V4WorldMap>(FindObjectsSortMode.None).Length==0,"no map object outside v4 / no-map");yield break;}
  var map=V4WorldMap.Instance;Check(map!=null&&WorldMapLayout.Current!=null,"map and TextAsset loaded");Check(WorldMapLayout.Current.rooms.Length==21,"21 rooms");Check(map.Mode==V4WorldMap.Display.Small,"default small");Check(map.VisibleRooms.Length==(map.All?21:1),"initial visited visibility");
  Check(map.VisibleDoors.All(d=>d.type!="elevator"),"elevator not drawn");Check(!map.All||map.VisibleDoors.Length==31,"map-all 31 non-elevator doors");
  foreach(var d in map.VisibleDoors)lines.Add("GATE "+d.id+" "+d.gate+"="+V4WorldMap.GateLabel(d.gate));
  float scale=Time.timeScale;var asset=(InputActionAsset)typeof(PlayerInputReader).GetField("runtime",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(Player);string maps=string.Join(",",asset.actionMaps.Select(m=>m.name+"="+m.enabled));double began=Time.timeAsDouble;
  foreach(var expected in new[]{V4WorldMap.Display.Large,V4WorldMap.Display.Off,V4WorldMap.Display.Small}){yield return Tap(Key.M);Check(map.Mode==expected,"M="+expected);Check(Time.timeScale==scale&&!Player.InputLocked,"clock scale / input unchanged");Check(maps==string.Join(",",asset.actionMaps.Select(m=>m.name+"="+m.enabled)),"action maps unchanged");}
  Check(Time.timeAsDouble>began,"game time advances during map");Vector2 before=map.PlayerWorld;yield return Keys(Key.RightArrow);yield return new WaitForSecondsRealtime(.2f);yield return Keys();Check(map.PlayerWorld.x!=before.x,"player point moves");
  if(N5Args.Has("-b15-progress")){map.RefreshProgress();foreach(string gate in new[]{"Dash","WallJump","DJ","Slam","Boss"})Check(!map.GateUnlocked(gate)&&map.GateDisplayColor(gate)==V4WorldMap.GateColor(gate),"zero ability preserves "+gate);RoomSession.Instance.GrantDash();map.RefreshProgress();foreach(string gate in new[]{"Dash","WallJump","DJ","Slam","Boss"})Check(map.GateUnlocked(gate)==(gate=="Dash"),"saved Dash unlocks only D: "+gate);Check(map.GateDisplayColor("Dash")!=V4WorldMap.GateColor("Dash"),"D changes to open color");Check(map.CheckpointRoom=="A01","saved checkpoint room A01");yield return Title();yield return ContinueGame();map.RefreshProgress();Check(map.CheckpointRoom=="A01"&&map.GateUnlocked("Dash"),"CP and ability map persist after Continue");}
  foreach(var step in new[]{new[]{"A02","A01-A02"},new[]{"A03","A02-A03"}}){Check(RoomSession.Instance.RequestTransition(step[0],V4SceneMap.Path(step[0]),step[1]),"transition fixture "+step[0]);yield return Ready();Check(map.VisibleRooms.Length==(map.All?21:step[0]=="A02"?2:3),"visited count "+step[0]);var r=WorldMapLayout.Current.Room(step[0]);Check(Vector2.Distance(map.PlayerWorld,(Vector2)Player.transform.position+new Vector2(r.rect[0],r.rect[1]))<.001f,"world player offset "+step[0]);}
  if(N5Args.Has("-b15-resume")){Check(ProgressSave.TryLoad(out var save)&&save.visitedRooms.Length==3,"three visited rooms saved");yield return Title();yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A01"&&ProgressSave.HasVisited("A02")&&ProgressSave.HasVisited("A03"),"Continue restores checkpoint and visits");Check(FindObjectsByType<V4WorldMap>(FindObjectsSortMode.None).Length==1,"single map across Continue");}
  if(N5Args.Has("-b15-capture"))lines.Add("KNOWN-LIMIT screenshot attempt: ScreenCapture module unavailable (CS0103); no retry");
  yield return new WaitForSecondsRealtime(10);
 }
}
#endif
