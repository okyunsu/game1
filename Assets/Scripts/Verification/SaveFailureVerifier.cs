using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public sealed class SaveFailureVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize()=>N5Args.Start<SaveFailureVerifier>("-verify-save-failure");
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N5-TA-save-failure.txt");
 protected override IEnumerator Tests(){string scenario=N5Args.Value("-verify-save-failure","readonly");if(scenario=="ending"){yield return EndingContinue();yield break;}yield return ContinueGame();Check(RoomSession.Instance.CurrentRoomId=="A01","write failure fixture at A01");var checkpoint=FindFirstObjectByType<Checkpoint>();var cpBounds=checkpoint.GetComponent<Collider2D>().bounds;float outside=cpBounds.max.x+.8f;float inside=cpBounds.center.x;yield return Keys();PlaceForAuthorizedSetup(new Vector2(outside,1.81f));yield return new WaitForSecondsRealtime(.2f);string slot=ProgressSave.FilePath;byte[] before=File.ReadAllBytes(slot);
 // Host owns file attributes/ACL and restores them through the READY marker handshake.
 string marker=N5Args.Value("-verify-marker","Logs/N5-TA");File.WriteAllText(marker+".ready","ready to apply file/folder protection");yield return Until(()=>File.Exists(marker+".protected"),"host protection ready",30);
 // Leave and re-enter the authored CP-A01 trigger using only movement input.
 yield return WalkTo(()=>Body.position.x>=outside,Key.RightArrow,"leave CP");yield return WalkTo(()=>Body.position.x<=inside,Key.LeftArrow,"re-enter CP");yield return new WaitForSecondsRealtime(.2f);
 Check(!string.IsNullOrEmpty(ProgressSave.Message)&&ProgressSave.Message.Contains("저장 실패"),"checkpoint write failure message visible");Check(Health.HP>0&&!Player.InputLocked,"game remains playable after write failure");Check(Convert.ToBase64String(before)==Convert.ToBase64String(File.ReadAllBytes(slot)),"normal save bytes protected");File.WriteAllText(marker+".failed","write failure observed");yield return Until(()=>File.Exists(marker+".restored"),"host restores permissions",30);
 yield return WalkTo(()=>Body.position.x>=outside,Key.RightArrow,"leave CP retry");yield return WalkTo(()=>Body.position.x<=inside,Key.LeftArrow,"CP retry");yield return new WaitForSecondsRealtime(.2f);Check(string.IsNullOrEmpty(ProgressSave.Message)&&ProgressSave.TryLoad(out _),"next checkpoint write succeeds after permission recovery");lines.Add("Scenario="+scenario+" slot="+slot);
 }
 IEnumerator EndingContinue(){yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"title");yield return new WaitForSecondsRealtime(.2f);Check(ProgressSave.TryLoad(out var data)&&data.bossDefeated&&data.endingReached,"fresh exe reads reached-ending fixture");for(int i=0;i<2;i++){Check(FindFirstObjectByType<SaveMenu>().CanContinue,"ending Continue enabled "+i);yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"Ending "+i);Check(SceneManager.GetActiveScene().name=="Ending","Continue routes Ending "+i);yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"Ending title "+i);yield return new WaitForSecondsRealtime(.2f);}Check(ProgressSave.TryLoad(out var saved)&&saved.endingReached&&saved.bossDefeated,"repeat ending preserves flags");}
}
