using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Batch19 fixtures are explicitly opt-in and never save an authored Scene.
public static class V5Batch19
{
 public static void ClearanceFixtures()
 {
  if(!N5Args.Has("-verify-b18-clearance"))throw new Exception("Explicit opt-in required");
  const string folder="Assets/Editor/B19TempScenes";
  if(Directory.Exists(folder))throw new IOException("Preserve existing temporary directory");
  var rows=new List<string>();var shape=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab").GetComponent<BoxCollider2D>();
  string output=N5Args.Value("-verify-output","Validation/_raw/B19/T2-fixtures.txt");if(File.Exists(output))throw new IOException("Preserve evidence");
  bool pass=false;
  try
  {
   B09Authoring.Generate(N5Args.Value("-b19-draft2","Validation/_raw/B19/rooms_v5_A.json"),folder,"Design/v5/world_v5_A.json");
   foreach(string path in Directory.GetFiles(folder,"*.unity").OrderBy(p=>p))
   {EditorSceneManager.OpenScene(path);rows.AddRange(V5B18Clearance.Scan(UnityEngine.Object.FindFirstObjectByType<RoomDefinition>(),shape.size));}
   var heads=rows.Where(l=>l.StartsWith("FAIL ")&&l.Contains("category=overhead")).ToArray();
   string[] expected={"A01 clearance surface=solid7", "A02 clearance surface=solid13", "A06 clearance surface=solid6", "A06 clearance surface=solid7"};
   if(heads.Length!=4||expected.Any(e=>!heads.Any(l=>l.Contains(e))))throw new Exception("draft2 four observed overhead failures lost");
   if(rows.Any(l=>l.StartsWith("WARN ")))throw new Exception("unexpected draft2 WARN");
   if(!rows.Any(l=>l.StartsWith("FAIL A02 enemy-spawn")&&l.Contains("solid14")))throw new Exception("draft2 enemy overlap lost");
   rows.Add("PASS draft2 overhead=4 expectedPairs=4 sideGap=0 enemyInsideSolid=1");
   EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   var room=new GameObject("B19 gap fixture").AddComponent<RoomDefinition>();room.roomId="fixture";room.gameObject.AddComponent<V4Room>().dimensions=new Vector2(20,12);
   Action<string,Vector2,Vector2> box=(name,center,size)=>{var go=new GameObject(name);go.transform.SetParent(room.transform);go.layer=6;go.transform.position=center;go.AddComponent<BoxCollider2D>().size=size;};
   box("solid0",new Vector2(10,.5f),new Vector2(20,1));box("solid1",new Vector2(5,2.5f),new Vector2(1,3));box("solid2",new Vector2(6.5f,2.5f),new Vector2(1,3));
   var fixture=V5B18Clearance.Scan(room,shape.size);rows.AddRange(fixture);
   if(fixture.Count(l=>l.StartsWith("WARN ")&&l.Contains("width=0.500"))!=1||fixture.Any(l=>l.StartsWith("FAIL ")))throw new Exception("0.5 gap must WARN only");
   rows.Add("PASS horizontal-gap fixture width=0.500 WARN=1 FAIL=0");pass=true;
  }
  catch(Exception e){rows.Add("FAIL fixture "+e);}
  finally
  {
   EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   if(Directory.Exists(folder))AssetDatabase.DeleteAsset(folder);
   rows.Add("MEASURE temporaryScenesDeleted="+!Directory.Exists(folder));
   File.WriteAllLines(output,rows);
  }
  EditorApplication.Exit(pass?0:1);
 }
}
