using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class B05Special:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Special>("-verify-b05-special");
 protected override IEnumerator Tests(){yield return NewGame();Check(RoomSession.Instance.RequestTransition("B01","Assets/Scenes/V2/B01_V2.unity","Entry"),"G-J fixture");yield return Ready();RoomSession.Instance.GrantDash();Check(!Motor.hasDoubleJump,"G-J no Double Jump");PlaceForAuthorizedSetup(new Vector2(69.5f,1.81f));yield return Keys(Key.RightArrow,Key.Z);yield return new WaitForSeconds(.18f);yield return Keys(Key.RightArrow,Key.Z,Key.C);yield return new WaitForSeconds(.6f);yield return Keys();lines.Add("MEASURE G-J no DJ pos="+Body.position);Check(RoomSession.Instance.CurrentRoomId=="B01"&&Body.position.y<4.6f,"jump + Dash cannot climb G-J ledge");
  Check(RoomSession.Instance.RequestTransition("B06","Assets/Scenes/V2/B06_V2.unity","Entry"),"B06 pogo fixture");yield return Ready();Check(!Motor.hasDoubleJump,"B06 Dash only");var attack=Player.GetComponent<PlayerAttack>();int before=attack.PogosPerformed;var target=GameObject.Find("V2 pogo practice 0").GetComponent<Collider2D>();lines.Add("MEASURE pogo target "+target.bounds);Player.GetComponent<PlayerAttack>().Cancel();PlaceForAuthorizedSetup(new Vector2(target.bounds.center.x,target.bounds.max.y+3.4f));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;yield return new WaitForFixedUpdate();PlaceForAuthorizedSetup(new Vector2(target.bounds.center.x,target.bounds.max.y+3.4f));Body.linearVelocity=Vector2.down*Motor.Tuning.maxFallSpeed;var input=gameObject.AddComponent<N9PogoInput>();input.keyboard=keyboard;int fired=0;input.produce=()=>++fired==2?new[]{Key.DownArrow,Key.X}:new[]{Key.DownArrow};yield return Until(()=>attack.PogosPerformed>before||RoomSession.Instance.Respawning,"actual down attack pogo",3);input.produce=null;Destroy(input);Check(!RoomSession.Instance.Respawning,"early pogo before body contact");lines.Add("MEASURE B06 pogo vy="+Body.linearVelocity.y+" refresh Dash="+(!Player.GetComponent<PlayerDash>().AirUsed));Check(!Motor.hasDoubleJump&&attack.PogosPerformed==before+1,"V2 pogo target unchanged attack + bounce");yield return Keys();
 }
}


