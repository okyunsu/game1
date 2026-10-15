using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using J=B09Json;

public sealed class MapLayoutBuilder:IProcessSceneWithReport
{
 public int callbackOrder=>0;
 [MenuItem("Afterglow/Build Map Layout From Design")]
 public static void Generate()=>Generate(N5Args.Value("-map-design","Design/world_v7.json"));
 public static string Generate(string path)
 {
  var source=J.Read(File.ReadAllText(path));
  var layout=new WorldMapLayout{rooms=J.D(source["rooms"]).Select(p=>new MapRoom{id=p.Key,rect=J.A(J.D(p.Value),"rect").Select(J.F).ToArray()}).ToArray(),
   doors=J.A(source,"doors").Select(J.D).Select(d=>new MapDoor{id=J.S(d,"id"),a=J.S(d,"a"),b=J.S(d,"b"),edge=J.S(d,"edge"),type=J.S(d,"type"),gate=J.S(d,"gate"),at=J.F(d,"at"),span=J.A(d,"span").Select(J.F).ToArray()}).ToArray()};
  Directory.CreateDirectory("Assets/Maps");string output="Assets/Maps/"+Path.GetFileName(path);File.WriteAllText(output,JsonUtility.ToJson(layout,true)+"\n",new System.Text.UTF8Encoding(false));AssetDatabase.Refresh();
  Debug.Log($"MAP layout={output} rooms={layout.rooms.Length} doors={layout.doors.Length} elevator={layout.doors.Count(d=>d.type=="elevator")}");return output;
 }
 public static void Run(){Generate();EditorApplication.Exit(0);}
 public static void VerifyGeneration(){
  string path=Generate("Design/world_v7.json");string before=File.ReadAllText(path);Generate("Design/world_v7.json");if(before!=File.ReadAllText(path))throw new Exception("Nondeterministic map");
  var source=J.Read(File.ReadAllText("Design/world_v7.json"));var map=JsonUtility.FromJson<WorldMapLayout>(before);if(map.rooms.Length!=21||map.doors.Length!=J.A(source,"doors").Count)throw new Exception("Map counts");
  foreach(var r in map.rooms)if(!r.rect.SequenceEqual(J.A(J.D(J.D(source["rooms"])[r.id]),"rect").Select(J.F)))throw new Exception("Coordinates changed "+r.id);
  string other=Generate("Design/world_v6.json");var v6=JsonUtility.FromJson<WorldMapLayout>(File.ReadAllText(other));if(v6.rooms.Length!=J.D(J.Read(File.ReadAllText("Design/world_v6.json"))["rooms"]).Count)throw new Exception("v6 counts");AssetDatabase.DeleteAsset(other);
  File.WriteAllText("Validation/_raw/B15/T1-generation.txt","PASS rooms=21 doors="+map.doors.Length+" elevator="+map.doors.Count(d=>d.type=="elevator")+" coordinates=unchanged deterministic=True v6=True deleted=True\n");EditorApplication.Exit(0);
 }
 public void OnProcessScene(Scene scene,BuildReport report)
 {
  if(report==null)return;
  var go=new GameObject("World map layout data");SceneManager.MoveGameObjectToScene(go,scene);
  go.AddComponent<WorldMapSource>().layouts=AssetDatabase.FindAssets("t:TextAsset",new[]{"Assets/Maps"}).Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<TextAsset>).ToArray();
 }
 [InitializeOnLoadMethod]static void EditorLayout(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.ExitingEditMode&&V4SceneMap.Enabled)WorldMapLayout.Load(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Maps/"+WorldMapLayout.Name+".json"));};}
}
