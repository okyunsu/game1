using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Batch12: asset authoring and builds run in separate Editor processes.
public static class B12BossAssets
{
 const string B3="Assets/BossCandidates/B3/BossTuning.asset";
 public static void Compile(){Debug.Log("MEASURE B3 compiled field="+typeof(BossTuning).GetField("b3Patterns"));EditorApplication.Exit(0);}
 public static void Create(){
  if(File.Exists(B3))throw new IOException("Preserve "+B3);
  if(!AssetDatabase.IsValidFolder("Assets/BossCandidates/B3"))AssetDatabase.CreateFolder("Assets/BossCandidates","B3");
  if(!AssetDatabase.CopyAsset("Assets/BossCandidates/B2-fast/BossTuning.asset",B3))throw new IOException("Copy B2-fast");
  var asset=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);
  Debug.Log("MEASURE reproduced missing new serialized field b3Patterns="+asset.b3Patterns);
  var so=new SerializedObject(asset);so.FindProperty("b3Patterns").boolValue=true;so.FindProperty("damage").intValue=1;
  so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(asset);AssetDatabase.SaveAssets();
  Debug.Log("MEASURE B3 saved b3Patterns="+asset.b3Patterns+" damage="+asset.damage+" HP="+asset.maxHP);EditorApplication.Exit(0);
 }
 public static void Verify(){
  var asset=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);
  if(asset==null||!asset.b3Patterns||asset.damage!=1||asset.maxHP!=28)throw new Exception("B3 import assert");
  Debug.Log("MEASURE B3 reloaded "+JsonUtility.ToJson(asset));EditorApplication.Exit(0);
 }
 public static void Enable(){
  var asset=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);var so=new SerializedObject(asset);
  string field=N5Args.Value("-b12-enable","");var property=so.FindProperty(field);if(property==null)throw new Exception(field);
  property.boolValue=true;so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(asset);AssetDatabase.SaveAssets();
  Debug.Log("MEASURE B3 enabled "+field);EditorApplication.Exit(0);
 }
 public static void Build(){
  var basic=AssetDatabase.LoadAssetAtPath<BossTuning>("Assets/ScriptableObjects/BossTuning.asset");
  // D-B3 (2026-10-14): default boss adopts B3; B2-fast stays selectable as a candidate.
  var adopted=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);
  if(JsonUtility.ToJson(basic)!=JsonUtility.ToJson(adopted))throw new Exception("D-B3 default / B3 mismatch");
  Debug.Log("MEASURE default b3Patterns="+basic.b3Patterns+" B3 fields identical");
  var b3=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);
  if(b3!=null){if(!b3.b3Patterns||b3.damage!=1||b3.maxHP!=28)throw new Exception("B3 import assert");Debug.Log("MEASURE B3 build values "+JsonUtility.ToJson(b3));}
  string folder="Builds/B12-delivery";Directory.CreateDirectory(folder);
  var scenes=B07Build.Scenes().Concat(V4SceneMap.Rooms.Select(V4SceneMap.Path)).Concat(new[]{"Assets/Scenes/Test/MechanicsTest.unity"}).ToArray();
  var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=scenes,locationPathName=folder+"/Afterglow.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.CleanBuildCache|BuildOptions.Development});
  File.WriteAllText(N5Args.Value("-b12-build-output","Validation/_raw/B12/build.txt"),result.summary.result+" errors="+result.summary.totalErrors+" warnings="+result.summary.totalWarnings);
  EditorApplication.Exit(result.summary.result==BuildResult.Succeeded?0:1);
 }
 public static void HazardPrefab(){
  string kind=N5Args.Value("-b12-prefab","Projectile");string path="Assets/Prefabs/BossB3"+kind+".prefab";
  if(File.Exists(path))throw new IOException("Preserve "+path);
  var go=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BossWave.prefab"));go.name="BossB3"+kind;
  UnityEngine.Object.DestroyImmediate(go.GetComponent<BossWave>());go.AddComponent<BossB3Hazard>();go.GetComponent<SpriteRenderer>().color=kind=="Projectile"?new Color(1,.8f,.1f):new Color(.8f,.3f,1);
  var prefab=PrefabUtility.SaveAsPrefabAsset(go,path);UnityEngine.Object.DestroyImmediate(go);
  var asset=AssetDatabase.LoadAssetAtPath<BossTuning>(B3);var so=new SerializedObject(asset);so.FindProperty(kind=="Projectile"?"projectilePrefab":"fallingPrefab").objectReferenceValue=prefab;so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(asset);AssetDatabase.SaveAssets();
  Debug.Log("MEASURE B3 prefab="+path);EditorApplication.Exit(0);
 }

}
