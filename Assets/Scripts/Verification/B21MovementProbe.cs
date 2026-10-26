#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B21MovementProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21MovementProbe>("-verify-b21-movement");
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 GameObject Box(string name,Vector2 center,Vector2 size,bool trigger=false){var go=new GameObject(name);go.layer=6;go.transform.position=center;var box=go.AddComponent<BoxCollider2D>();box.size=size;box.isTrigger=trigger;return go;}
 IEnumerator Reset(Vector2 at)
 {
  yield return Keys();Player.GetComponent<PlayerDash>().Cancel();Player.GetComponent<PlayerAttack>().Cancel();Health.RestoreMax();yield return new WaitForSeconds(.45f);Motor.ClearTransientState();PlaceForAuthorizedSetup(at);yield return null;
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return new WaitForSeconds(.2f);
  Check(Player.GetComponent<B21CornerFix>()!=null,"corner candidate enabled");
  Check(Player.GetComponent<B21CornerFix>().settings!=null,"corner candidate settings present");
  var floor=Box("B21 movement floor",new Vector2(2050,999.5f),new Vector2(120,1));var wall=Box("B21 movement wall",new Vector2(2005.5f,1005),new Vector2(1,10));wall.AddComponent<ClimbableWall>();
  var lantern=Box("B21 movement lantern",new Vector2(2020,1002.25f),new Vector2(1,.5f),true);lantern.AddComponent<PogoTarget>();
  var corner=Box("B21 small head corner",new Vector2(2042,1003.25f),new Vector2(4,.5f));
  yield return Reset(new Vector2(2040-Player.GetComponent<Collider2D>().bounds.extents.x+.05f,1000.015f));yield return new WaitForSeconds(.25f);int corrections=Player.GetComponent<B21CornerFix>().Corrections;Input(Key.Z);
  yield return Until(()=>Player.GetComponent<B21CornerFix>().Corrections>corrections,"actual small corner correction",2);Check(Body.linearVelocity.y>0,"corner correction preserves rising velocity");yield return Keys();
  if(N5Args.Has("-b21-corner-only")){lines.Add("MEASURE corner compatibility height="+Player.GetComponent<Collider2D>().bounds.size.y+" corrections="+(Player.GetComponent<B21CornerFix>().Corrections-corrections));Destroy(floor);Destroy(wall);Destroy(lantern);Destroy(corner);yield break;}
  Motor.hasDoubleJump=false;var dash=Player.GetComponent<PlayerDash>();dash.hasDash=false;Motor.hasWallJump=true;
  for(int i=1;i<=10;i++)
  {
   yield return Reset(new Vector2(2006.36f,1003));Input(Key.LeftArrow);yield return Until(()=>Motor.WallSliding,"wall contact "+i,2);yield return new WaitForSeconds(.2f);int before=Motor.WallJumpsPerformed;
   yield return Keys(Key.LeftArrow,Key.Z);yield return Keys();Check(Motor.WallJumpsPerformed==before+1,"wall jump actual "+i);
  }
  Motor.hasWallJump=false;
  for(int i=1;i<=10;i++)
  {
   yield return Reset(new Vector2(2020,1003.6f));yield return Until(()=>!Motor.Grounded,"pogo airborne",1);int before=Player.GetComponent<PlayerAttack>().PogosPerformed;
   Input(Key.DownArrow,Key.X);yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>before,"actual pogo "+i,2);yield return Keys();Check(Body.linearVelocity.y>0,"pogo upward "+i);
  }
  dash.hasDash=true;
  for(int i=1;i<=10;i++)
  {
   yield return Reset(new Vector2(2000,1000.015f));yield return new WaitForSeconds(.25f);Input(Key.RightArrow,Key.LeftShift);yield return Until(()=>dash.IsDashing,"dash start "+i,1);yield return Until(()=>!dash.IsDashing,"dash end "+i,1);yield return Keys();Check(true,"dash actual "+i);
  }
  lines.Add("MEASURE corner movement wallJump=10/10 pogo=10/10 dash=10/10");Destroy(floor);Destroy(wall);Destroy(lantern);Destroy(corner);
 }
}
#endif
