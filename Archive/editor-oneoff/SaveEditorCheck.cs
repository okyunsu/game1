using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public static class SaveEditorCheck
{
 static SaveEditorCheck(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("Save.Direct",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("Editor direct save check").AddComponent<SaveEditorRunner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("Save.Direct",false);EditorApplication.Exit(SessionState.GetInt("Save.DirectExit",1));}};}
 public static void Run(){if(File.Exists("Validation/S2-04-editor-direct.txt"))throw new IOException("Preserve evidence");SessionState.SetBool("Save.Direct",true);EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class SaveEditorRunner:MonoBehaviour
{
 IEnumerator Start(){yield return new WaitForSeconds(.2f);var lines=new List<string>();bool fail=false;try{
  var session=RoomSession.Instance;var original=File.ReadAllText(ProgressSave.FilePath);
  if(!ProgressSave.TryLoad(out var stored)||!stored.dash||stored.checkpointId!="CP-A03")throw new Exception("pre-existing isolated save required");
  if(session.CurrentRoomId!="A01"||session.Player.GetComponent<PlayerDash>().hasDash||ProgressSave.Enabled)throw new Exception("Editor direct did not start fresh/no-save");lines.Add("PASS: A01 direct Play ignores existing Dash/CP-A03 save and disables disk saves");
  var cp=FindFirstObjectByType<Checkpoint>();session.ActivateCheckpoint(cp);if(File.ReadAllText(ProgressSave.FilePath)!=original)throw new Exception("Editor direct overwrote save");lines.Add("PASS: checkpoint activation in direct Play preserves existing disk save");
  var temporary=ProgressSave.FilePath+".tmp";Directory.CreateDirectory(temporary);bool written=ProgressSave.Write(stored);if(written||string.IsNullOrEmpty(ProgressSave.Message)||File.ReadAllText(ProgressSave.FilePath)!=original)throw new Exception("write failure not safely handled");Directory.Delete(temporary);if(!ProgressSave.Write(stored)||!string.IsNullOrEmpty(ProgressSave.Message))throw new Exception("retry failed");lines.Add("PASS: injected write failure shows message, preserves original, next save retries successfully");
 }catch(Exception e){fail=true;lines.Add(e.ToString());}lines.Insert(0,(fail?"FAIL":"PASS")+" S2-04 Editor direct/save failure");File.WriteAllLines("Validation/S2-04-editor-direct.txt",lines);SessionState.SetInt("Save.DirectExit",fail?1:0);EditorApplication.isPlaying=false;}
}
