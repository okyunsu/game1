using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class B07Guides
{
 public static void Run()
 {
  const string path="Assets/Resources/GuideObjectives_V3.asset";
  var goals=AssetDatabase.LoadAssetAtPath<GuideObjectivesV3>(path);
  if(goals==null){goals=ScriptableObject.CreateInstance<GuideObjectivesV3>();AssetDatabase.CreateAsset(goals,path);}
  string[] texts={"대시를 찾아라 — 바닥촌 동쪽 깊은 곳","바닥촌의 격자를 대시로 통과하라","중층 시장 너머, 보일러 굴뚝으로","위로 가는 길은 아래에 있다","숨은 승강로를 끝까지 올라라","높은 망루에서 힘을 얻어라","수문장이 기다린다","정상으로",""};
  string[] targets={"A05","B01","B04","A02","C01","C05","C06","S01",""};
  goals.objectives=texts.Select((text,i)=>new GuideObjectives.Objective{text=text,roomId=targets[i]}).ToArray();EditorUtility.SetDirty(goals);
  var edges=new List<GuideGraph.Edge>();var rows=new List<string>();int count=0;
  var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/GuideLantern.prefab");
  foreach(string id in V3SceneMap.Rooms)
  {
   var scene=EditorSceneManager.OpenScene(V3SceneMap.Path(id));
   foreach(var exit in Object.FindObjectsByType<RoomExit>(FindObjectsInactive.Include,FindObjectsSortMode.None))
   {
    edges.Add(new GuideGraph.Edge{from=id,to=exit.destinationRoomId,exitId=exit.exitId,position=exit.transform.position});
    // Optional room doors and shortcut doors have no lantern; required return doors do.
    bool shortcut=id=="A05"&&exit.destinationRoomId=="A03"||id=="B04"&&exit.destinationRoomId=="B01"||id=="A03"&&exit.destinationRoomId=="A05"||id=="B01"&&exit.destinationRoomId=="B04";
    exit.mainPath=!shortcut&&id!="A06"&&id!="B06"&&exit.destinationRoomId!="A06"&&exit.destinationRoomId!="B06"&&!(id=="S01"&&exit.destinationRoomId=="C06");
    foreach(var lamp in exit.GetComponentsInChildren<MainPathLantern>(true))Object.DestroyImmediate(lamp.gameObject);
    if(exit.mainPath){var lamp=(GameObject)PrefabUtility.InstantiatePrefab(prefab);lamp.transform.SetParent(exit.transform,true);lamp.transform.position=new Vector3(exit.transform.position.x,exit.GetComponent<Collider2D>().bounds.max.y+.4f,0);lamp.transform.localScale=new Vector3(.28f,.36f,1);count++;}
    rows.Add(id+" -> "+exit.destinationRoomId+" / "+exit.destinationSpawnId+" / "+(exit.mainPath?"required":"unmarked"));
   }
   EditorSceneManager.SaveScene(scene);
  }
  const string graphPath="Assets/Resources/GuideGraph_V3.asset";var graph=AssetDatabase.LoadAssetAtPath<GuideGraph>(graphPath);if(graph==null){graph=ScriptableObject.CreateInstance<GuideGraph>();AssetDatabase.CreateAsset(graph,graphPath);}graph.edges=edges.ToArray();EditorUtility.SetDirty(graph);AssetDatabase.SaveAssets();
  File.WriteAllLines("Validation/_raw/B07/T23-graph.txt",rows);File.WriteAllText("Validation/_raw/B07/T23-author.txt","MEASURE objectives=9 scenes=19 lanterns="+count+" edges="+edges.Count+"\n");EditorApplication.Exit(0);
 }
}
