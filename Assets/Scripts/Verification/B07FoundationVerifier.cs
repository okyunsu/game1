#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class B07FoundationVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B07FoundationVerifier>("-verify-b07-foundation");
 protected override IEnumerator Tests()
 {
  yield return NewGame();
  Check(SceneManager.GetActiveScene().name==(V3SceneMap.Enabled?"A01_V3":V2SceneMap.Enabled?"A01_V2":"A01"),"new game maps only opted-in candidate");
  if(!V3SceneMap.Enabled)yield break;
  yield return new WaitForSeconds(.3f);
  Check(ProgressSave.HasVisited("A01")&&ProgressSave.TryLoad(out var data)&&System.Array.IndexOf(data.visitedRooms,"A01")>=0,"A01 visit persisted version1");
  foreach(var pair in new[]{new[]{"A03","CP-A03"},new[]{"B01","CP-B01"},new[]{"C01","CP-C01"},new[]{"C04","CP-C04"}})
  {
   Check(RoomSession.Instance.RequestTransition(pair[0],V3SceneMap.Path(pair[0]),pair[1]),"foundation checkpoint transition "+pair[0]);yield return Ready();
   var cp=FindFirstObjectByType<Checkpoint>();RoomSession.Instance.ActivateCheckpoint(cp);yield return Title();yield return ContinueGame();
   Check(SceneManager.GetActiveScene().name==pair[0]+"_V3"&&RoomSession.Instance.CheckpointId==pair[1],"V3 checkpoint Continue "+pair[1]);
   Check(ProgressSave.HasVisited(pair[0]),"visit restored "+pair[0]);
  }
  yield return Title();
  File.WriteAllText(ProgressSave.FilePath,"{\"version\":1,\"dash\":true,\"checkpointId\":\"CP-A01\"}");
  Check(ProgressSave.TryLoad(out var legacy)&&legacy.visitedRooms.Length==0&&legacy.version==1,"legacy absent visits empty and version1");
  yield return ContinueGame();Check(SceneManager.GetActiveScene().name=="A01_V3","legacy V3 Continue");
 }
}
#endif
