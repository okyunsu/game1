#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public sealed class B10InputMatrix:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B10InputMatrix>("-verify-b10-input-matrix");
 IEnumerator Setup(bool ground)
 {
  yield return Keys();if(RoomSession.Instance!=null)Destroy(RoomSession.Instance.gameObject);Destroy(GameState.GetOrCreate().gameObject);yield return null;
  ProgressSave.Pending=null;ProgressSave.Enabled=false;ProgressSave.ResetBrokenWalls();SceneManager.LoadScene("Assets/Scenes/Test/MechanicsTest.unity");yield return Ready();
  PlaceForAuthorizedSetup(new Vector2(7,ground?1.81f:7));Player.ClearTransientInput();yield return Keys();yield return new WaitForSeconds(.06f);
 }
 IEnumerator Frame(params Key[] keys){InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));yield return null;}
 protected override IEnumerator Tests()
 {
  int cases=0,unexpected=0;
  foreach(bool ground in new[]{true,false})foreach(string other in new[]{"none","attack","jump","dash"})foreach(float offset in new[]{-.05f,0f,.05f})
  {
   yield return Setup(ground);var slam=Player.GetComponent<PlayerSlam>();var dash=Player.GetComponent<PlayerDash>();var attack=Player.GetComponent<PlayerAttack>();int dj=Motor.DoubleJumpsPerformed,pogo=attack.PogosPerformed;
   Key key=other=="attack"?Key.X:other=="jump"?Key.Z:Key.C;
   bool beganGround=Motor.Grounded;
   if(offset<0&&other!="none"){yield return Frame(key);yield return new WaitForSecondsRealtime(-offset);yield return Frame();}
   bool wasDashing=dash.IsDashing;bool airBefore=!Motor.Grounded;
   if(offset==0&&other!="none")yield return Frame(Key.DownArrow,Key.C,key);else yield return Frame(Key.DownArrow,Key.C);
   yield return new WaitForFixedUpdate();bool started=slam.Slamming;
   if(offset>0&&other!="none"){yield return new WaitForSecondsRealtime(offset);yield return Frame(key);yield return new WaitForFixedUpdate();}
   bool expected=airBefore&&!wasDashing;bool ok=started==expected;
   lines.Add((ok?"PASS":"UNEXPECTED")+" matrix initial="+(ground?"ground":"air")+" observedGround="+beganGround+" other="+other+" offset="+offset+" preDash="+wasDashing+" expectedSlam="+expected+" started="+started+" currentSlam="+slam.Slamming+" dash="+dash.IsDashing+" attack="+attack.Attacking+" DJdelta="+(Motor.DoubleJumpsPerformed-dj)+" pogoDelta="+(attack.PogosPerformed-pogo));
   cases++;if(!ok)unexpected++;
  }
  // Down attack and its Pogo remain independent when no dash key is pressed.
  foreach(float delay in new[]{0f,.05f,.1f}){
   yield return Setup(false);var target=FindFirstObjectByType<PogoTarget>();PlaceForAuthorizedSetup(new Vector2(target.transform.position.x,target.GetComponent<Collider2D>().bounds.max.y+2.1f));int before=Player.GetComponent<PlayerAttack>().PogosPerformed;yield return new WaitForSecondsRealtime(delay);yield return Frame(Key.DownArrow,Key.X);double end=Time.realtimeSinceStartupAsDouble+1;
   while(Time.realtimeSinceStartupAsDouble<end&&Player.GetComponent<PlayerAttack>().PogosPerformed==before)yield return null;
   bool ok=!Player.GetComponent<PlayerSlam>().Slamming&&Player.GetComponent<PlayerAttack>().PogosPerformed>before;lines.Add((ok?"PASS":"UNEXPECTED")+" down attack Pogo delay="+delay+" slam="+Player.GetComponent<PlayerSlam>().Slamming+" pogoDelta="+(Player.GetComponent<PlayerAttack>().PogosPerformed-before));cases++;if(!ok)unexpected++;
  }
  lines.Add("MEASURE input matrix cases="+cases+" unexpected="+unexpected);
  // Findings are a result list even when the intended priorities do not match.
 }
}
#endif
