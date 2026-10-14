#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class B06bEndingVerifier:N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()
    {
        if(!N5Args.Has("-verify-b06b-ending"))return;
        if(!N5Args.Has("-save-path")){N5Args.Start<B06bEndingVerifier>("-verify-b06b-ending");return;}
        ProgressSave.Write(new ProgressData{dash=true,doubleJump=true,wallJump=true,bossDefeated=true,endingReached=false,checkpointId="CP-C04",brokenWalls=new[]{"QA-B06b-wall"}});
        N5Args.Start<B06bEndingVerifier>("-verify-b06b-ending");
    }
    protected override IEnumerator Tests()
    {
        Check(N5Args.Has("-save-path"),"isolated explicit save path");yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"title");yield return new WaitForSeconds(.2f);
        if(!V2SceneMap.Enabled){var map=new GameObject("QA V2 mapping");map.SetActive(false);map.AddComponent<V2SceneMap>().rooms=new[]{"A01","C01","C04","C05","C06"};map.SetActive(true);}
        Check(ProgressSave.Continue()&&ProgressSave.PendingScene=="Assets/Scenes/Ending.unity","boss defeated before ending retains Ending");ProgressSave.Pending=null;
        yield return Tap(Key.Z);yield return Until(()=>SceneManager.GetActiveScene().name=="Ending","ending reached");yield return new WaitForSeconds(.2f);
        Check(ProgressSave.TryLoad(out var ended)&&ended.endingReached,"ending writes reached flag");
        yield return Tap(Key.Z);yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"ending to title");yield return ContinueGame();
        Check(RoomSession.Instance.CurrentRoomId=="C01"&&SceneManager.GetActiveScene().name=="C01_V2"&&RoomSession.Instance.CheckpointId=="CP-C01","ending Continue C01_V2 CP-C01");
        Check(Motor.hasDoubleJump&&Motor.hasWallJump&&Player.GetComponent<PlayerDash>().hasDash&&ProgressSave.IsWallBroken("QA-B06b-wall"),"all abilities and broken walls preserved");
        Check(RoomSession.Instance.RequestTransition("C06","Assets/Scenes/V2/C06_V2.unity","FromLeft"),"QA C06 revisit");yield return Ready();
        yield return Until(()=>RoomSession.Instance.CurrentRoomId=="C06"&&!RoomSession.Instance.Transitioning,"C06 ready");
        Check(FindFirstObjectByType<BossEncounter>()==null,"defeated boss absent");
        var bosses=Resources.FindObjectsOfTypeAll<BossEncounter>();bool found=false;foreach(var boss in bosses)if(boss.gameObject.scene==SceneManager.GetSceneByPath("Assets/Scenes/V2/C06_V2.unity")){found=true;Check(!boss.gameObject.activeSelf&&boss.leftExit.enabled,"boss inactive / exit unlocked");}
        Check(found,"suppression preserved authored boss object");yield return new WaitForSeconds(.5f);Check(SceneManager.GetActiveScene().name!="Ending"&&!GameState.GetOrCreate().Completing,"no repeated ending");
        yield return PhysicalDoor("C04");Check(RoomSession.Instance.CurrentRoomId=="C04","physical exit usable");
        yield return Title();yield return NewGame();Check(RoomSession.Instance.CurrentRoomId=="A01"&&!Motor.hasDoubleJump&&!Motor.hasWallJump&&!Player.GetComponent<PlayerDash>().hasDash,"NewGame A01 abilities reset");
        File.WriteAllText(ProgressSave.FilePath,"{\"version\":1,\"dash\":true,\"bossDefeated\":true,\"checkpointId\":\"CP-C04\"}");
        Check(ProgressSave.Continue()&&!ProgressSave.Pending.endingReached&&ProgressSave.PendingScene=="Assets/Scenes/Ending.unity","legacy missing endingReached retains Ending");
        lines.Add("MEASURE post-ending Continue C01_V2/CP-C01 abilities3/3 brokenWalls1/1 boss0 exits usable ending repeats0 legacy PASS");
    }
}
#endif
