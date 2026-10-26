using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B21CandidateBuildHook:IProcessSceneWithReport
{
 public int callbackOrder=>100;
 public void OnProcessScene(Scene scene,BuildReport report)
 {
  if(!N5Args.Has("-b21-candidates")||N5Args.Has("-b20-release")||scene.path!="Assets/Scenes/Menu.unity")return;
  var host=new GameObject("B21 candidate configuration (development)").AddComponent<B21CandidateHost>();
  SceneManager.MoveGameObjectToScene(host.gameObject,scene);host.hazardSettings=AssetDatabase.LoadAssetAtPath<B21HazardReturnSettings>("Assets/Candidates/B21HazardReturn.asset");
  host.cornerSettings=AssetDatabase.LoadAssetAtPath<B21CornerFixSettings>("Assets/Candidates/B21CornerFix.asset");
  Debug.Log("B21 development configuration injected scene="+scene.path+" hazardAsset="+(host.hazardSettings!=null));
 }
}
public static class B21CandidateAuthoring
{
 public static void RefreshCorner()
 {
  var settings=AssetDatabase.LoadAssetAtPath<B21CornerFixSettings>("Assets/Candidates/B21CornerFix.asset");settings.dashCornerEnabled=false;EditorUtility.SetDirty(settings);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void Corner()
 {
  const string path="Assets/Candidates/B21CornerFix.asset";if(File.Exists(path))throw new IOException("Preserve "+path);
  var settings=ScriptableObject.CreateInstance<B21CornerFixSettings>();AssetDatabase.CreateAsset(settings,path);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void Hazard()
 {
  const string path="Assets/Candidates/B21HazardReturn.asset";if(File.Exists(path))throw new IOException("Preserve "+path);
  var settings=ScriptableObject.CreateInstance<B21HazardReturnSettings>();AssetDatabase.CreateAsset(settings,path);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
