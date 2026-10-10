#if UNITY_EDITOR
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class B06BreakVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06BreakVerifier>("-verify-b06-break");
    BreakableWall Wall(bool secret,string id)
    {
        var asset=UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/BreakableWall_"+(secret?"Secret":"Shortcut")+".prefab");
        var go=Instantiate(asset);go.transform.position=new Vector2(8,2.5f);var wall=go.GetComponent<BreakableWall>();wall.wallId=id;return wall;
    }
    IEnumerator Swing(BreakableWall wall,bool left)
    {
        yield return Keys();PlaceForAuthorizedSetup(new Vector2(wall.transform.position.x+(left?-1.3f:1.3f),2));
        yield return Keys(left?Key.RightArrow:Key.LeftArrow);yield return Keys(left?Key.RightArrow:Key.LeftArrow,Key.X);yield return Keys();yield return new WaitForSeconds(.45f);
    }
    protected override IEnumerator Tests()
    {
        Check(N5Args.Has("-save-path"),"isolated explicit save path");
        yield return NewGame();
        var shortcut=Wall(false,"B06-Fixture-Shortcut");yield return null;
        yield return Swing(shortcut,false);Check(shortcut.Hits==0,"forbidden side real attack damage0");
        for(int i=1;i<=3;i++){yield return Swing(shortcut,true);Check(shortcut.Hits==i,"one hit per real attack "+i);if(i<3)Check(!shortcut.Broken,"not broken before third hit");}
        Check(shortcut.Broken&&!shortcut.gameObject.activeSelf,"third hit opens shortcut");
        var secret=Wall(true,"B06-Fixture-Secret");yield return null;yield return Swing(secret,false);Check(secret.Hits==1,"secret allows opposite side");
        int pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;yield return Keys();PlaceForAuthorizedSetup(new Vector2(8,5));yield return Keys(Key.DownArrow,Key.X);yield return Keys();yield return new WaitForSeconds(.45f);
        Check(Player.GetComponent<PlayerAttack>().PogosPerformed==pogos,"wall is never pogo target");
        RoomSession.Instance.GrantWallJump();Check(ProgressSave.TryLoad(out var saved)&&saved.version==1&&saved.wallJump&&System.Array.IndexOf(saved.brokenWalls,shortcut.wallId)>=0,"disk stores wall ownership and broken ID, version1");
        Health.TakeDamage(Health.HP,Body.position+Vector2.left);yield return Ready();Check(ProgressSave.IsWallBroken(shortcut.wallId),"death preserves open wall");
        Check(RoomSession.Instance.RequestTransition("A02","Assets/Scenes/A02.unity","FromLeft"),"leave room fixture");yield return Ready();
        Check(RoomSession.Instance.RequestTransition("A01","Assets/Scenes/A01.unity","FromRight"),"reenter room fixture");yield return Ready();
        shortcut=Wall(false,"B06-Fixture-Shortcut");yield return null;Check(!shortcut.gameObject.activeSelf,"reentry reconstructs open wall");
        yield return Title();yield return ContinueGame();Check(Motor.hasWallJump,"Continue restores new wall ability");
        shortcut=Wall(false,"B06-Fixture-Shortcut");yield return null;Check(!shortcut.gameObject.activeSelf,"Continue reconstructs open wall");
        yield return Title();yield return NewGame();Check(!ProgressSave.IsWallBroken("B06-Fixture-Shortcut")&&!Motor.hasWallJump,"new game clears new persistent fields");
        // Explicit QA file only; never writes the user's default slot.
        File.WriteAllText(ProgressSave.FilePath,"{\"version\":1,\"dash\":true,\"doubleJump\":true,\"checkpointId\":\"CP-A01\"}");
        Check(ProgressSave.TryLoad(out var old)&&!old.wallJump&&old.brokenWalls.Length==0&&old.dash&&old.doubleJump,"legacy missing fields default false/empty");
        Check(ProgressSave.Continue()&&!ProgressSave.IsWallBroken("B06-Fixture-Shortcut"),"legacy Continue restores empty wall set");
    }
}
#endif
