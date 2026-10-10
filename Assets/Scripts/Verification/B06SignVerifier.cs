using System.Collections;
using UnityEngine;

public sealed class B06SignVerifier : N5Runner
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B06SignVerifier>("-verify-b06-sign");
    protected override IEnumerator Tests()
    {
        yield return Ready();yield return new WaitForSeconds(.15f);
        var signs=FindObjectsByType<SignPost>(FindObjectsSortMode.None);Check(signs.Length==2,"two signs in "+RoomSession.Instance.CurrentRoomId);
        int hp=Health.HP;uint attacks=Player.GetComponent<PlayerAttack>().Sequence;Body.simulated=false;
        foreach(var sign in signs)
        {
            PlaceForAuthorizedSetup(sign.transform.position);yield return null;Check(sign.TextVisible&&!string.IsNullOrEmpty(sign.message),"near shows: "+sign.message);
            PlaceForAuthorizedSetup((Vector2)sign.transform.position+Vector2.right*3);yield return null;Check(!sign.TextVisible,"3u away hides");
            Check(sign.GetComponentsInChildren<Collider2D>().Length==0,"no collision or attack target");
        }
        Check(Health.HP==hp&&Player.GetComponent<PlayerAttack>().Sequence==attacks,"display has no combat/input writes");Body.simulated=true;
    }
}
