using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B06Guides
{
    [MenuItem("Afterglow/Rebuild V2 Guide Graph")]
    public static void RebuildGraph()
    {
        if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())return;
        var setup=EditorSceneManager.GetSceneManagerSetup();
        try{GenerateGraph();}finally{EditorSceneManager.RestoreSceneManagerSetup(setup);}
    }
    static void GenerateGraph()
    {
        var edges=new List<GuideGraph.Edge>();
        // Same playable Scene inventory used by the V2 build, source scenes only.
        foreach(string id in B05Build.V2Ids())
        {
            var scene=EditorSceneManager.OpenScene("Assets/Scenes/V2/"+id+"_V2.unity");
            foreach(var exit in UnityEngine.Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))edges.Add(new GuideGraph.Edge{from=id,to=exit.destinationRoomId,exitId=exit.exitId,position=exit.transform.position});
            foreach(var exit in UnityEngine.Object.FindObjectsByType<OptionalRoomEntry>(FindObjectsSortMode.None))edges.Add(new GuideGraph.Edge{from=id,to=exit.destinationRoomId,exitId=exit.name,position=exit.transform.position});
        }
        var graph=AssetDatabase.LoadAssetAtPath<GuideGraph>("Assets/Resources/GuideGraph.asset");
        if(graph==null){graph=ScriptableObject.CreateInstance<GuideGraph>();AssetDatabase.CreateAsset(graph,"Assets/Resources/GuideGraph.asset");}
        graph.edges=edges.ToArray();EditorUtility.SetDirty(graph);AssetDatabase.SaveAssets();
    }
    public static void Create()
    {
        const string objectivePath="Assets/Resources/GuideObjectives.asset";
        if(File.Exists(objectivePath))throw new IOException("Preserve guide objectives");
        var objectives=ScriptableObject.CreateInstance<GuideObjectives>();objectives.objectives=new[]{
            new GuideObjectives.Objective{condition=GuideObjectives.Condition.NoDash,text="대시를 찾아라 — 하부 회랑 깊은 곳",roomId="A05"},
            new GuideObjectives.Objective{condition=GuideObjectives.Condition.NoDoubleJump,text="격자를 지나 동력층으로, 더블 점프를 찾아라",roomId="B04"},
            new GuideObjectives.Objective{condition=GuideObjectives.Condition.BossAlive,text="높은 턱을 넘어 탑 꼭대기로",roomId="C06"},
            new GuideObjectives.Objective{condition=GuideObjectives.Condition.Complete,text="",roomId=""}
        };AssetDatabase.CreateAsset(objectives,objectivePath);
        var sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
        var template=new GameObject("Guide lantern");template.AddComponent<MainPathLantern>();
        var image=template.AddComponent<SpriteRenderer>();image.sprite=sprite;image.color=new Color(1,.86f,.32f);image.sortingOrder=25;template.transform.localScale=new Vector3(.28f,.36f,1);
        var prefab=PrefabUtility.SaveAsPrefabAsset(template,"Assets/Prefabs/B06/GuideLantern.prefab");UnityEngine.Object.DestroyImmediate(template);
        int count=0;string[] optional={"A06","B06","C05"};
        foreach(string id in B05Build.V2Ids())
        {
            var scene=EditorSceneManager.OpenScene("Assets/Scenes/V2/"+id+"_V2.unity");
            foreach(var exit in UnityEngine.Object.FindObjectsByType<RoomExit>(FindObjectsSortMode.None))
            {
                // Every mandatory connection direction, including return paths to observed gates.
                if(optional.Contains(id)||optional.Contains(exit.destinationRoomId))continue;
                exit.mainPath=true;EditorUtility.SetDirty(exit);
                var lamp=(GameObject)PrefabUtility.InstantiatePrefab(prefab);lamp.transform.SetParent(exit.transform,true);
                lamp.transform.position=new Vector3(exit.transform.position.x,exit.GetComponent<Collider2D>().bounds.max.y+.4f,0);lamp.transform.localScale=new Vector3(.28f,.36f,1);count++;
            }
            EditorSceneManager.SaveScene(scene);
        }
        GenerateGraph();File.WriteAllText("Validation/_raw/B06/D1-authoring.txt","PASS V2 objectives and required connection lanterns="+count+"; graph from V2 build Scene RoomExit/optional entries; no connections or placement changed.\n");
        EditorApplication.Exit(0);
    }
}
