using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class V5Batch21
{
 static string Folder=>N5Args.Value("-b21-output","Validation/_raw/21");
 static IEnumerable<string> Paths=>Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p);
 static string[] Geometry()
 {
  var rows=new List<string>();
  foreach(string path in Paths)
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();Physics2D.SyncTransforms();
   foreach(var t in room.GetComponentsInChildren<Transform>(true))
   {
    string key=room.roomId+"/"+t.name;
    rows.Add(key+" pos="+t.position.ToString("F5")+" scale="+t.localScale.ToString("F5")+" components="+string.Join(",",t.GetComponents<Component>().Select(c=>c.GetType().Name)));
    if(t.TryGetComponent<BoxCollider2D>(out var box))rows.Add(key+" box="+box.size.ToString("F5")+" offset="+box.offset.ToString("F5")+" trigger="+box.isTrigger+" enabled="+box.enabled);
    if(t.TryGetComponent<RoomExit>(out var exit))rows.Add(key+" exit="+exit.destinationRoomId+"/"+exit.destinationSpawnId+" enabled="+exit.enabled+" gate="+t.GetComponent<V4ExitRule>().gate);
   }
  }
  return rows.OrderBy(s=>s,StringComparer.Ordinal).ToArray();
 }
 public static void Generate()
 {
  Directory.CreateDirectory(Folder);string output=Folder+"/T2-scene-generation.txt";if(File.Exists(output))throw new IOException("Preserve "+output);
  var before=Geometry();File.WriteAllLines(Folder+"/T2-geometry-before.txt",before);
  B09Authoring.Generate("Design/v5/rooms_v5_A.json","Assets/Scenes/V5","Design/v5/world_v5_A.json");
  var after=Geometry();File.WriteAllLines(Folder+"/T2-geometry-after.txt",after);
  var rows=new List<string>();bool pass=before.SequenceEqual(after);rows.Add((pass?"PASS":"FAIL")+" six-room geometry/components/door destinations/gates unchanged");
  foreach(string path in Paths)
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();
   foreach(var exit in room.GetComponentsInChildren<V4ExitRule>())
   {
    string expected=room.roomId=="A03"&&exit.GetComponent<RoomExit>().exitId=="A05-A03"?"V5-A05-breakable0":"";
    bool ok=exit.closedUntilWallBroken==expected;pass&=ok;rows.Add((ok?"PASS":"FAIL")+" lock "+room.roomId+" "+exit.name+" key="+exit.closedUntilWallBroken);
   }
   foreach(var wall in room.GetComponentsInChildren<BreakableWall>())
   {
    var expected=room.roomId=="A05"?BreakableWall.Side.Right:BreakableWall.Side.Any;bool ok=wall.breakableFrom==expected;pass&=ok;rows.Add((ok?"PASS":"FAIL")+" wall "+wall.wallId+" side="+wall.breakableFrom);
   }
   rows.Add("COUNT "+room.roomId+" boxes="+room.GetComponentsInChildren<BoxCollider2D>().Length+" doors="+room.GetComponentsInChildren<RoomExit>().Length+" enemies="+room.GetComponentsInChildren<EnemyReset>().Length+" pogo="+room.GetComponentsInChildren<PogoTarget>().Length+" shards="+room.GetComponentsInChildren<DiscoveryReward>().Length);
  }
  File.WriteAllLines(output,rows);EditorApplication.Exit(pass?0:1);
 }
}
