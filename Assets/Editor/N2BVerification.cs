using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class N2BVerification
{
 static N2BVerification(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("N2B.Check",false))return;if(s==PlayModeStateChange.EnteredPlayMode){var runner=new GameObject("N2-B checkpoint verification").AddComponent<SliceVerification>();runner.outputPath="Validation/S1-08-N2-B.txt";runner.finished=pass=>{SessionState.SetInt("N2B.Exit",pass?0:1);EditorApplication.isPlaying=false;};}if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("N2B.Check",false);EditorApplication.Exit(SessionState.GetInt("N2B.Exit",1));}};}
 public static void Checkpoints(){if(File.Exists("Validation/S1-08-N2-B.txt"))throw new IOException("Preserve evidence");SessionState.SetBool("N2B.Check",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
