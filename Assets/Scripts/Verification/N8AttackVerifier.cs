using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public sealed class N8AttackVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<N8AttackVerifier>("-verify-attack-up");
 Gamepad pad;GameObject above,side;RigidbodyConstraints2D original;
 protected override void Cleanup(){if(pad!=null)InputSystem.RemoveDevice(pad);if(Player!=null)Body.constraints=original;Destroy(above);Destroy(side);}
 DummyTarget Target(string name,Vector2 at){var go=new GameObject(name);go.transform.position=at;go.AddComponent<BoxCollider2D>().size=new Vector2(.15f,.15f);go.GetComponent<BoxCollider2D>().isTrigger=true;return go.AddComponent<DummyTarget>();}
 protected override IEnumerator Tests(){yield return NewGame();yield return Until(()=>Motor.Grounded,"grounded");yield return Keys();original=Body.constraints;Body.constraints=RigidbodyConstraints2D.FreezeAll;var shape=Player.GetComponent<BoxCollider2D>();var attack=Player.GetComponent<PlayerAttack>();var feedback=Player.GetComponent<AttackFeedback>();
 var top=Target("N8 upper dummy",new Vector2(shape.bounds.center.x,shape.bounds.max.y+.6f));above=top.gameObject;var lateral=Target("N8 side dummy",new Vector2(shape.bounds.max.x+.5f,shape.bounds.center.y));side=lateral.gameObject;Physics2D.SyncTransforms();pad=InputSystem.AddDevice<Gamepad>();
 foreach(int fps in new[]{30,60,120}){Application.targetFrameRate=fps;yield return new WaitForSecondsRealtime(.4f);
 foreach(Key up in new[]{Key.UpArrow,Key.W}){yield return Keys(up);Check(Player.Move.y>.5f,"keyboard y "+up+" fps="+fps);int hits=top.Hits,sideHits=lateral.Hits;yield return Keys(up,Key.X);Check(attack.Attacking&&attack.VerticalDirection==1,"up+X begins "+up+" fps="+fps);yield return Until(()=>feedback.slash.gameObject.activeSelf,"slash active",1);
 var bounds=shape.bounds;Vector2 expected=new Vector2(bounds.center.x,bounds.max.y+attack.tuning.verticalAttackHeight*.5f);Check(Vector2.Distance(attack.HitboxCenter,expected)<.01f&&Vector2.Distance(attack.HitboxSize,new Vector2(attack.tuning.verticalAttackWidth,attack.tuning.verticalAttackHeight))<.01f,"actual OverlapBox geometry .01u "+fps);Check(Vector2.Distance(feedback.slash.bounds.center,expected)<.01f&&Vector2.Distance(feedback.slash.bounds.size,new Vector2(attack.tuning.verticalAttackWidth,attack.tuning.verticalAttackHeight))<.01f,"rendered slash geometry .01u "+fps);yield return Keys();yield return new WaitForSecondsRealtime(.4f);Check(top.Hits==hits+1&&lateral.Hits==sideHits,"upper one hit side zero "+fps);}
 int before=top.Hits;InputSystem.QueueStateEvent(pad,new GamepadState{leftStick=Vector2.up});yield return null;yield return null;InputSystem.QueueStateEvent(pad,new GamepadState{leftStick=Vector2.up}.WithButton(GamepadButton.West));yield return null;Check(attack.VerticalDirection==1&&attack.Attacking,"pad stick up+X "+fps);yield return new WaitForSecondsRealtime(.25f);InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;yield return new WaitForSecondsRealtime(.4f);Check(top.Hits==before+1,"pad upper damage "+fps);
 foreach(int sign in new[]{-1,1}){lateral.transform.position=new Vector2((sign>0?shape.bounds.max.x:shape.bounds.min.x)+sign*.5f,shape.bounds.center.y);Physics2D.SyncTransforms();yield return Keys(sign>0?Key.RightArrow:Key.LeftArrow);int hits=lateral.Hits;yield return Keys(sign>0?Key.RightArrow:Key.LeftArrow,Key.X);Check(attack.VerticalDirection==0,"no y horizontal "+sign+" "+fps);yield return Keys();yield return new WaitForSecondsRealtime(.4f);Check(lateral.Hits==hits+1,"original horizontal hit "+sign+" "+fps);}
 yield return Keys(Key.UpArrow,Key.RightArrow);Check(Mathf.Abs(Player.Move.x-1)<.001f,"up does not reduce horizontal input "+fps);yield return Keys();
 }
 }
}
