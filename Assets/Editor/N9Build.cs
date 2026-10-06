using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class N9Build : IProcessSceneWithReport
{
    public int callbackOrder=>1100;
    static string candidate;
    static GameObject player,e1,e2,e3;static BossTuning boss;
    public void OnProcessScene(Scene scene,BuildReport report)
    {
        if(report==null)return;
        string sceneCandidate=N5Args.Value("-n9-candidate",null);
        string sceneEvidence="Validation/N9-"+N5Args.Value("-n9-build","S5-hard")+"-scene-map.txt";
        File.AppendAllText(sceneEvidence,scene.name+" candidate="+(sceneCandidate??"main")+"\n");
        if(sceneCandidate==null||sceneCandidate=="H1")return;
        string sceneFolder="Assets/N9Candidates/"+sceneCandidate;
        player=AssetDatabase.LoadAssetAtPath<GameObject>(sceneFolder+"/Player.prefab");e1=AssetDatabase.LoadAssetAtPath<GameObject>(sceneFolder+"/E1.prefab");e2=AssetDatabase.LoadAssetAtPath<GameObject>(sceneFolder+"/E2.prefab");e3=AssetDatabase.LoadAssetAtPath<GameObject>(sceneFolder+"/E3.prefab");boss=AssetDatabase.LoadAssetAtPath<BossTuning>(sceneFolder+"/BossTuning.asset");
        if(player==null||e1==null||e2==null||e3==null||boss==null)throw new BuildFailedException("Missing copied candidate references");
        foreach(var root in scene.GetRootGameObjects())
        {
            foreach(var room in root.GetComponentsInChildren<RoomDefinition>(true))room.playerPrefab=player;
            foreach(var reset in root.GetComponentsInChildren<EnemyReset>(true)){string name=reset.prefab.name;reset.prefab=name.StartsWith("E1")?e1:name.StartsWith("E2")?e2:e3;}
            foreach(var reset in root.GetComponentsInChildren<EnemyRespawn>(true)){reset.prefab=e1;if(reset.live!=null)reset.live.tuning=e1.GetComponent<EnemyPatrol>().tuning;}
            foreach(var b in root.GetComponentsInChildren<BossEncounter>(true))b.tuning=boss;
        }
    }
    static T Copy<T>(string name,string folder)where T:ScriptableObject
    {
        var asset=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<T>("Assets/ScriptableObjects/"+name+".asset"));asset.name=name;
        AssetDatabase.CreateAsset(asset,folder+"/"+name+".asset");return asset;
    }
    static GameObject Prefab(string name,string folder,Action<GameObject> edit)
    {
        var go=PrefabUtility.LoadPrefabContents("Assets/Prefabs/"+name+".prefab");edit(go);var result=PrefabUtility.SaveAsPrefabAsset(go,folder+"/"+name+".prefab");PrefabUtility.UnloadPrefabContents(go);return result;
    }
    public static void Run()
    {
        string tag=N5Args.Value("-n9-build","S5-hard");candidate=N5Args.Value("-n9-candidate",null);
        string evidence="Validation/N9-"+tag+"-build.txt";if(File.Exists(evidence))throw new IOException("Preserve "+evidence);
        if(candidate!=null&&candidate!="H1")
        {
            if(candidate is not ("H2" or "H3"))throw new ArgumentException("H1/H2/H3 only");
            string folder="Assets/N9Candidates/"+candidate;Directory.CreateDirectory(folder);AssetDatabase.Refresh();
            if(File.Exists(folder+"/CombatTuning.asset"))
            {
                player=AssetDatabase.LoadAssetAtPath<GameObject>(folder+"/Player.prefab");e1=AssetDatabase.LoadAssetAtPath<GameObject>(folder+"/E1.prefab");e2=AssetDatabase.LoadAssetAtPath<GameObject>(folder+"/E2.prefab");e3=AssetDatabase.LoadAssetAtPath<GameObject>(folder+"/E3.prefab");boss=AssetDatabase.LoadAssetAtPath<BossTuning>(folder+"/BossTuning.asset");
            }
            else
            {
                var combat=Copy<CombatTuning>("CombatTuning",folder);combat.playerMaxHP=candidate=="H2"?4:5;combat.invincibility=candidate=="H2"?.7f:1;
                var first=Copy<EnemyTuning>("E1Tuning",folder);first.speed=3;
                var second=Copy<E2Tuning>("E2Tuning",folder);second.warningSeconds*=.85f;second.chargeSpeed=8;
                var third=Copy<E3Tuning>("E3Tuning",folder);third.shotPeriod*=.85f;
                boss=Copy<BossTuning>("BossTuning",folder);boss.maxHP+=4;boss.meleeTell=.5f;boss.waveTell=.65f;boss.recovery=.7f;boss.waveSpeed=7.5f;
                player=Prefab("Player",folder,go=>{go.GetComponent<PlayerAttack>().tuning=combat;go.GetComponent<PlayerHealth>().tuning=combat;});
                e1=Prefab("E1",folder,go=>go.GetComponent<EnemyPatrol>().tuning=first);
                e2=Prefab("E2",folder,go=>go.GetComponent<EnemyCharger>().tuning=second);
                e3=Prefab("E3",folder,go=>go.GetComponent<EnemyTurret>().tuning=third);
                EditorUtility.SetDirty(combat);EditorUtility.SetDirty(first);EditorUtility.SetDirty(second);EditorUtility.SetDirty(third);EditorUtility.SetDirty(boss);AssetDatabase.SaveAssets();
            }
        }
        string[] ids={"Menu","A01","A02","A03","A04","A05","B01","B02","B03","B04","B05","C01","C02","C03","C04","C06","Ending","Test/PogoTest"};
        string buildRoot=N5Args.Value("-build-root","Builds");
        if(buildRoot!="Builds"&&buildRoot!="Builds/_tmp")throw new ArgumentException("Builds or Builds/_tmp only");
        Directory.CreateDirectory(buildRoot+"/"+tag);
        var r=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=ids.Select(id=>"Assets/Scenes/"+id+".unity").ToArray(),locationPathName=buildRoot+"/"+tag+"/Afterglow-"+tag+".exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.CleanBuildCache,extraScriptingDefines=candidate==null?Array.Empty<string>():new[]{"N9_"+candidate}});
        if(!File.Exists("Validation/N9-"+tag+"-scene-map.txt")||File.ReadAllLines("Validation/N9-"+tag+"-scene-map.txt").Length!=18)throw new BuildFailedException("Expected eighteen actually processed scenes");
        File.WriteAllText(evidence,$"{r.summary.result} errors={r.summary.totalErrors} warnings={r.summary.totalWarnings}\n18 scenes incl opt-in PogoTest; EditorBuildSettings unchanged. Candidate={candidate??"main Hard"}; source tuning unchanged by candidate build.\n");candidate=null;EditorApplication.Exit(r.summary.result==BuildResult.Succeeded&&r.summary.totalErrors==0?0:1);
    }
}
