using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;
using J=B09Json;
public static class B09Audit
{
 public static void Run()
 {
  var rows=new List<string>();int failures=0,count=0;var data=J.D(J.Read(File.ReadAllText("Design/rooms_v2.json"))["rooms"]);
  foreach(var pair in data)
  {
   var d=J.D(pair.Value);EditorSceneManager.OpenScene(V4SceneMap.Path(pair.Key));var room=Object.FindFirstObjectByType<RoomDefinition>();
   foreach(var enemy in room.GetComponentsInChildren<EnemyReset>(true)){bool valid=enemy.prefab!=null;rows.Add((valid?"PASS":"FAIL")+" ENEMY PREFAB "+pair.Key+" "+enemy.name);if(!valid)failures++;}
   foreach(string kind in new[]{"solid","platform","climb","seal","cover","breakable","hazard","pad"}){int expected=J.A(d,kind).Count,actual=room.GetComponentsInChildren<Transform>(true).Count(t=>System.Text.RegularExpressions.Regex.IsMatch(t.name,"^"+kind+"[0-9]+$"));rows.Add((actual==expected?"PASS":"FAIL")+" COUNT "+pair.Key+" "+kind+" json="+expected+" scene="+actual);if(actual!=expected)failures++;}
   var sourceSpawnIds=J.A(d,"spawns").Select(J.D).Select(s=>J.S(s,"id")).ToArray();
   var counts=new Dictionary<string,int>{["doors"]=room.GetComponentsInChildren<RoomExit>(true).Length,["spawns"]=room.GetComponentsInChildren<RoomSpawn>(true).Count(s=>sourceSpawnIds.Contains(s.spawnId)),["pogo"]=room.GetComponentsInChildren<PogoTarget>(true).Length,["steam"]=room.GetComponentsInChildren<SteamVent>(true).Length,["lifts"]=room.GetComponentsInChildren<LeverLift>(true).Length,["crumble"]=room.GetComponentsInChildren<CrumblePlatform>(true).Length,["enemies"]=room.GetComponentsInChildren<EnemyReset>(true).Length+room.GetComponentsInChildren<E4Hover>(true).Length+room.GetComponentsInChildren<BossEncounter>(true).Length,["marks"]=room.GetComponentsInChildren<DashPickup>(true).Length+room.GetComponentsInChildren<WallJumpPickup>(true).Length+room.GetComponentsInChildren<DoubleJumpPickup>(true).Length+room.GetComponentsInChildren<Checkpoint>(true).Length+room.GetComponentsInChildren<SignPost>(true).Length+room.GetComponentsInChildren<DiscoveryReward>(true).Length+room.GetComponentsInChildren<V3SummitEnding>(true).Length};
   foreach(var item in counts){int expected=J.A(d,item.Key).Count;rows.Add((expected==item.Value?"PASS":"FAIL")+" COUNT "+pair.Key+" "+item.Key+" json="+expected+" scene="+item.Value);if(expected!=item.Value)failures++;}
   foreach(var node in J.A(d,"doors"))
   {
    var door=J.D(node);string target=J.S(door,"to").Split('(')[0],id=J.S(door,"id");if(pair.Key=="S02"){rows.Add("ONEWAY S02 passive arrival marker; no return trigger");continue;}
    var destination=J.D(data[target]);var matching=J.A(destination,"spawns").Select(J.D).Where(s=>J.S(s,"id")==id).ToArray();count++;
    if(matching.Length==0){rows.Add("ONEWAY "+pair.Key+" -> "+target+" "+id+" no arrival; reverse marker disabled");continue;}
    if(matching.Length!=1){rows.Add("FAIL "+pair.Key+" -> "+target+" "+id+" matching spawn="+matching.Length);failures++;continue;}
    var spawn=matching[0];var reverse=J.A(destination,"doors").Select(J.D).FirstOrDefault(p=>J.S(p,"id")==id);float distance=float.PositiveInfinity;
    if(reverse!=null){var shape=J.A(reverse,"rect");if(J.F(shape[2])==1&&J.F(shape[3])>1){float center=J.F(shape[0])+.5f,w=J.F(J.A(destination,"size")[0]),x=J.F(spawn,"x");bool wallSide=center<w/2?x>center&&x<w/2:x<center&&x>w/2;rows.Add((wallSide?"PASS":"FAIL")+" ARRIVAL WALL SIDE "+target+" "+id+" x="+x);if(!wallSide)failures++;}}
    if(reverse!=null){var r=J.A(reverse,"rect");var bounds=new Rect(J.F(r[0]),J.F(r[1]),J.F(r[2]),J.F(r[3]));var position=new Vector2(J.F(spawn,"x"),J.F(spawn,"y"));distance=Vector2.Distance(position,bounds.center);}
    bool far=distance>=2.5f;rows.Add((far?"PASS":"FAIL")+" LINK "+pair.Key+" -> "+target+" "+id+" distance="+distance);if(!far)failures++;
   }
  }
  rows.Add("MEASURE scenes="+data.Count+" directed exits="+count+" violations="+failures);File.WriteAllLines(N5Args.Value("-b09-audit-output","Validation/_raw/B09/T5-6-audit-current.txt"),rows);EditorApplication.Exit(0);
 }
}
