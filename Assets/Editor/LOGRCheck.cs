using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]public static class LOGRCheck
{
 static LOGRCheck(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("LOGR.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode){var runner=new GameObject("LOGR prescribed verification").AddComponent<PreBuildVerification>();runner.output=Nightly.Path("LOGR-playtest");runner.finished=pass=>{SessionState.SetInt("LOGR.Exit",pass?0:1);EditorApplication.isPlaying=false;};}if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("LOGR.Run",false);EditorApplication.Exit(SessionState.GetInt("LOGR.Exit",1));}};}
 public static void Run(){if(File.Exists(Nightly.Path("LOGR-playtest")))throw new IOException("Preserve evidence");SessionState.SetBool("LOGR.Run",true);EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
