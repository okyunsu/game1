#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public sealed class B07InventoryVerifier:B07RoomVerifier
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void InitInventory(){N5Args.Start<B07InventoryVerifier>("-verify-b07-inventory");N5Args.Start<B07InventoryVerifier>("-verify-b07-boss-link");}
 protected override IEnumerator Tests()
 {
  yield return NewGame();yield return new WaitForSeconds(.15f);
  bool bossLink=N5Args.Has("-verify-b07-boss-link");
  foreach(string id in bossLink?new[]{"C06"}:V3SceneMap.Rooms)
  {
   roomId=id;if(RoomSession.Instance.CurrentRoomId!=id){PlaytestLogger.Instance?.ObserveExit("QA-inventory-fixture",id);Check(RoomSession.Instance.RequestTransition(id,V3SceneMap.Path(id),id=="C06"?"FromLeft":"Entry"),"inventory fixture "+id);yield return Ready();}yield return new WaitForSeconds(.15f);
   var plan=FindFirstObjectByType<V3RoomRoutes>();Check(plan!=null&&plan.routes.Length>0,"authored plan "+id);
   var decor=plan.transform.Find("Regional decor");Check(decor==null||decor.GetComponentsInChildren<Collider2D>(true).Length==0,"decor collision0 "+id);Check(id!="S01"||decor==null,"summit stays empty");
   Check(id!="S01"||FindObjectsByType<KillZone>(FindObjectsSortMode.None).Length==0,"summit lethal hazards zero "+id);
   if(bossLink){var boss=FindFirstObjectByType<BossEncounter>();Check(boss!=null,"V3 arena actual boss");lines.Add("MEASURE requestedRuntimeBoss="+N5Args.Value("-boss","")+" extendedPatterns="+boss.tuning.extendedPatterns+" HP="+boss.tuning.maxHP+" meleeTell="+boss.tuning.meleeTell+" tuning="+boss.tuning.name);}
   else{Capture(plan);lines.Add("MEASURE inventory room="+id+" dimensions="+plan.dimensions+" pits="+plan.fallPits+" enemies="+plan.enemyCount+" optionalRoutes="+plan.routes.Count(r=>r.goal.StartsWith("Discovery:")));}
  }
  if(!bossLink){PlaytestLogger.Instance?.Finish();string csv=PlaytestLogger.Instance?.CsvPath;Check(!string.IsNullOrEmpty(csv)&&File.Exists(csv),"V3 CSV written");string[] rows=File.ReadAllLines(csv);Check(V3SceneMap.Rooms.All(id=>rows.Any(row=>row.Contains("\"room_enter\",\""+id+"\""))),"19 room enter rows in explicitly labeled QA inventory");lines.Add("MEASURE CSV="+csv+"; fixture sweep is not an automatic continuous completion");}
 }
}
#endif
