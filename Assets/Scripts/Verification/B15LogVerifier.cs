#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class B15LogVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B15LogVerifier>("-verify-b15-log");
 protected override IEnumerator Tests(){
  yield return NewGame();var logger=PlaytestLogger.Instance;yield return Until(()=>logger.Recording,"CSV recording");yield return null;
  foreach(string target in new[]{"A02","A03"}){
   var exit=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Single(e=>e.destinationRoomId==target);var bounds=exit.GetComponent<Collider2D>().bounds;
   PlaceForAuthorizedSetup(new Vector2(bounds.min.x-1,bounds.min.y+.01f));lines.Add("FIXTURE outside exit "+exit.exitId+"; actual RightArrow triggers transition");
   yield return WalkTo(()=>RoomSession.Instance.CurrentRoomId==target,Key.RightArrow,"physical exit "+target,5);yield return Ready();yield return null;
  }
  yield return Tap(Key.M);yield return new WaitForSecondsRealtime(.3f);yield return Tap(Key.M);yield return Tap(Key.M);
  Check(logger.MapChanges==3&&logger.MapOpens==2&&logger.LargeMapSeconds>.3,"M changes3 / opens2 / large duration");logger.Finish();
  var rows=File.ReadAllLines(logger.CsvPath);Check(rows[0]=="utc,event,room,x,y,cause,elapsed_active,pause_total,details","original nine columns unchanged");
  var enters=rows.Where(r=>r.Contains(",\"room_enter\",")).ToArray();var exits=rows.Where(r=>r.Contains(",\"room_exit\",")).ToArray();Check(enters.Length==3&&exits.Length==2,"three entries and two completed door pairs");
  Check(exits.Count(r=>r.Contains("exit=A01-A02;"))==1&&exits.Count(r=>r.Contains("exit=A02-A03;"))==1,"actual used door IDs exactly once");Check(exits.All(r=>r.Contains("room_active_seconds=")),"room dwell at exit");
  lines.Add("MEASURE CSV="+logger.CsvPath+" entries="+enters.Length+" exits="+exits.Length+" mapOpens="+logger.MapOpens+" changes="+logger.MapChanges+" largeSeconds="+logger.LargeMapSeconds);
 }
}
#endif
