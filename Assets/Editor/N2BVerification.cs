using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class N2BVerification
{
 static N2BVerification(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("N2B.Check",false))return;if(s==PlayModeStateChange.EnteredPlayMode){var runner=new GameObject("N2-B checkpoint verification").AddComponent<SliceVerification>();runner.traverse=SessionState.GetBool("N2B.Slice",false);runner.outputPath=SessionState.GetString("N2B.ResultPath", "");runner.finished=pass=>{SessionState.SetInt("N2B.Exit",pass?0:1);EditorApplication.isPlaying=false;};}if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("N2B.Check",false);EditorApplication.Exit(SessionState.GetInt("N2B.Exit",1));}};}
 public static void CheckpointsNight()=>Begin(false,Nightly.Path("S1-08"));
 public static void Checkpoints()=>Begin(false);
 public static void CheckpointIntegration()=>Begin(false,"Validation/S1-08-N2-B-integration.txt");
 public static void Slice()=>Begin(true);
 static void Begin(bool slice,string path=null){path??=slice?"Validation/S1-09-N2-B-05.txt":"Validation/S1-08-N2-B.txt";SessionState.SetString("N2B.ResultPath",path);SessionState.SetBool("N2B.Slice",slice);if(File.Exists(path))throw new IOException("Preserve evidence");SessionState.SetBool("N2B.Check",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
