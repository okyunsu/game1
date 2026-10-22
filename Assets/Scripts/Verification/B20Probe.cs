#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Explicit Batch20 measurement fixtures; no authored Scene or shared Asset writes.
public sealed class B20Probe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Init()=>N5Args.Start<B20Probe>("-verify-b20");
 RoomDefinition Room=>FindFirstObjectByType<RoomDefinition>();
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 void Record(string text){lines.Add(text);Debug.Log(text);}
 IEnumerator Setup(string id,string spawn="Entry")
 {
  yield return Keys();
  if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),spawn),"fixture arrival "+id);yield return Ready();}
  Health.RestoreMax();yield return new WaitForSeconds(.5f);
 }
 IEnumerator Door(string destination)
 {
  yield return new WaitForSeconds(.5f);yield return PhysicalDoor(destination);Record("MEASURE door arrival room="+RoomSession.Instance.CurrentRoomId+" feet="+Body.position.ToString("F4"));
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();
  string mode=N5Args.Value("-b20-mode","shortcut");
  if(mode=="shortcut")yield return Shortcut();
 }
 IEnumerator Shortcut()
 {
  yield return Setup("A03");Motor.hasDoubleJump=Motor.hasWallJump=false;Player.GetComponent<PlayerDash>().hasDash=false;
  yield return Door("A05");
  var wall=Room.GetComponentsInChildren<BreakableWall>(true).Single(w=>w.wallId=="V5-A05-breakable0");
  Record("MEASURE shortcut first-entry x="+Body.position.x.ToString("F4")+" wallMin="+wall.GetComponent<Collider2D>().bounds.min.x.ToString("F4")+" wallMax="+wall.GetComponent<Collider2D>().bounds.max.x.ToString("F4")+" wallPresent="+wall.gameObject.activeInHierarchy+" collider="+wall.GetComponent<Collider2D>().enabled+" broken="+wall.Broken+" side="+(Body.position.x>2?"inside":"outside")+" breakableFrom="+wall.breakableFrom);
  Check(Body.position.x>2&&!wall.Broken&&wall.gameObject.activeInHierarchy,"reproduced A03 to A05 inside intact wall");
  PlaceForAuthorizedSetup(new Vector2(2.8f,1.02f));yield return Keys(Key.LeftArrow);yield return Keys();
  for(int i=0;i<3&&!wall.Broken;i++){yield return Tap(Key.X);yield return new WaitForSeconds(.45f);}
  Check(wall.Broken,"actual inward-side attack broke wall");
  string wallId=wall.wallId;yield return Door("A03");yield return Door("A05");
  wall=Room.GetComponentsInChildren<BreakableWall>(true).Single(w=>w.wallId==wallId);
  Record("MEASURE shortcut reentry broken="+wall.Broken+" active="+wall.gameObject.activeInHierarchy+" saveKey="+wallId);
  Check(wall.Broken&&!wall.gameObject.activeInHierarchy,"wall state survives room roundtrip");
  ProgressSave.SaveCurrent(RoomSession.Instance);yield return Title();yield return ContinueGame();
  Check(ProgressSave.IsWallBroken(wallId),"Continue restores persistent wall key");yield return Setup("A03");yield return Door("A05");
  wall=Room.GetComponentsInChildren<BreakableWall>(true).Single(w=>w.wallId==wallId);
  Record("MEASURE shortcut after-continue broken="+wall.Broken+" active="+wall.gameObject.activeInHierarchy+" x="+Body.position.x.ToString("F4"));
  Check(wall.Broken&&!wall.gameObject.activeInHierarchy,"wall remains open after save Continue and reentry");
 }
}
#endif
