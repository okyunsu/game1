#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Unity.Cinemachine;

// Opt-in measurement only. Placement is limited to authored arrival / pit test fixtures.
public sealed class B17V5Probe : N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Init()=>N5Args.Start<B17V5Probe>("-verify-b17-probe");
 readonly List<string> frames=new(); string phase="setup"; bool sampling;
 string Folder=>Path.GetDirectoryName(Path.GetFullPath(Evidence));
 RoomDefinition Room=>FindFirstObjectByType<RoomDefinition>();
 void Record(string text){lines.Add(text);Debug.Log(text);}
 void LateUpdate()
 {
  if(!sampling||Player==null)return;
  var shape=Player.GetComponent<Collider2D>();var contacts=new ContactPoint2D[32];int count=shape.GetContacts(contacts);
  string colliders=string.Join("|",contacts.Take(count).Select(c=>c.collider==shape?c.otherCollider.name:c.collider.name).Distinct());
  var triggers=Physics2D.OverlapBoxAll(shape.bounds.center,shape.bounds.size,0).Where(c=>c.isTrigger&&c.GetComponent<RoomExit>()!=null).Select(c=>c.name+":"+c.GetComponent<RoomExit>().enabled);
  var rig=FindFirstObjectByType<RoomCameraRig>();var conf=rig?.GetComponentInChildren<CinemachineConfiner2D>();var cam=rig?.OutputCamera;
  frames.Add(string.Join(",",Time.frameCount,Time.realtimeSinceStartup.ToString("F4"),phase,RoomSession.Instance.CurrentRoomId,Body.position.x.ToString("F4"),Body.position.y.ToString("F4"),Body.linearVelocity.x.ToString("F4"),Body.linearVelocity.y.ToString("F4"),Motor.Grounded,Player.Move.x,Player.InputLocked,Health.KnockbackLocked,colliders,string.Join("|",triggers),cam==null?"":cam.transform.position.x.ToString("F4"),cam==null?"":cam.transform.position.y.ToString("F4"),rig?.VirtualCamera.Follow?.name,conf!=null&&conf.enabled,conf?.BoundingShape2D?.name,Room?.GetComponent<V4Room>()?.ArrivalSuppressed));
 }
 protected override void Cleanup(){File.WriteAllLines(Path.ChangeExtension(Evidence,"csv"),new[]{"frame,time,phase,room,x,y,vx,vy,grounded,inputX,inputLocked,knockback,contacts,doorOverlaps,camX,camY,follow,confinerEnabled,boundary,arrivalSuppressed"}.Concat(frames));}
 IEnumerator Arrival(string room,string spawn)
 {
  yield return Keys();if(RoomSession.Instance.CurrentRoomId==room){Check(Room.TryGetSpawn(spawn,out var s),"authored fixture "+spawn);PlaceForAuthorizedSetup(s.transform.position);}
  else {Check(RoomSession.Instance.RequestTransition(room,V4SceneMap.Path(room),spawn),"arrival "+room+" "+spawn);yield return Ready();}
  Health.RestoreMax();
 }
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 IEnumerator Move(string name,float target,bool jump,float limit=8)
 {
  phase=name;double start=Time.realtimeSinceStartupAsDouble,nextJump=0,held=0;float startX=Body.position.x;string room=RoomSession.Instance.CurrentRoomId;
  while(Time.realtimeSinceStartupAsDouble-start<limit&&RoomSession.Instance.CurrentRoomId==room&&Mathf.Abs(Body.position.x-target)>.22f)
  {
   double now=Time.realtimeSinceStartupAsDouble;var keys=new List<Key>{target>Body.position.x?Key.RightArrow:Key.LeftArrow};
   if(jump&&Motor.Grounded&&now>=nextJump){held=now+.42;nextJump=now+.7;}
   if(now<held)keys.Add(Key.Z);Input(keys.ToArray());yield return null;
  }
  yield return Keys();Record("MEASURE "+name+" fromX="+startX.ToString("F3")+" target="+target+" position="+Body.position+" grounded="+Motor.Grounded+" locked="+Player.InputLocked+" reached="+(Mathf.Abs(Body.position.x-target)<.5f)+" elapsed="+(Time.realtimeSinceStartupAsDouble-start).ToString("F3"));
 }
 IEnumerator CameraWindow(string name,bool shrine)
 {
  phase=name;double start=Time.realtimeSinceStartupAsDouble,nextJump=0,held=0;var positions=new List<Vector3>();var playerPositions=new List<Vector2>();
  while(Time.realtimeSinceStartupAsDouble-start<5)
  {
   double now=Time.realtimeSinceStartupAsDouble;var keys=new List<Key>();
   if(shrine){if(Body.position.x>37.5f)keys.Add(Key.LeftArrow);if(Motor.Grounded&&now>=nextJump){held=now+.42;nextJump=now+.75;}if(now<held)keys.Add(Key.Z);}
   else if(name.StartsWith("A04",StringComparison.Ordinal)){if(Body.position.x<20)keys.Add(Key.RightArrow);}
   else if(Body.position.x>25)keys.Add(Key.LeftArrow);
   Input(keys.ToArray());var rig=FindFirstObjectByType<RoomCameraRig>();positions.Add(rig.OutputCamera.transform.position);playerPositions.Add(Body.position);yield return null;
  }
  yield return Keys();var liveRig=FindFirstObjectByType<RoomCameraRig>();var confiner=liveRig.GetComponentInChildren<CinemachineConfiner2D>();Record("MEASURE camera "+name+" roomHeight="+Room.GetComponent<V4Room>().dimensions.y+" cameraXRange="+(positions.Max(p=>p.x)-positions.Min(p=>p.x)).ToString("F4")+" cameraYRange="+(positions.Max(p=>p.y)-positions.Min(p=>p.y)).ToString("F4")+" playerXRange="+(playerPositions.Max(p=>p.x)-playerPositions.Min(p=>p.x)).ToString("F4")+" playerYRange="+(playerPositions.Max(p=>p.y)-playerPositions.Min(p=>p.y)).ToString("F4")+" dash="+Player.GetComponent<PlayerDash>().hasDash+" follow="+liveRig.VirtualCamera.Follow.name+" oversize="+confiner.OversizeWindow.Enabled+" cameraFirst="+positions[0]+" cameraLast="+positions.Last()+" boundary="+confiner.BoundingShape2D.bounds.size+" locked="+Player.InputLocked);
  var camera=FindFirstObjectByType<RoomCameraRig>().OutputCamera;var previous=camera.targetTexture;var active=RenderTexture.active;var rt=new RenderTexture(1280,720,24);var texture=new Texture2D(1280,720,TextureFormat.RGB24,false);
  camera.targetTexture=rt;camera.Render();RenderTexture.active=rt;texture.ReadPixels(new Rect(0,0,1280,720),0,0);texture.Apply();File.WriteAllBytes(Path.Combine(Folder,N5Args.Value("-b17-stage","probe")+"-"+name+".png"),(byte[])Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true).GetMethod("EncodeToPNG",new[]{typeof(Texture2D)}).Invoke(null,new object[]{texture}));camera.targetTexture=previous;RenderTexture.active=active;Destroy(texture);rt.Release();Destroy(rt);
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();sampling=true;
  yield return Arrival("A02","A01-A02");yield return Move("A02-right-first-platform",10,true);yield return Move("A02-left-walk",1.5f,false);yield return Move("A02-left-jump",1.5f,true);
  yield return Arrival("A05","A04-A05");yield return CameraWindow("A05-arrival",false);
  phase="A05-shrine-approach";double until=Time.realtimeSinceStartupAsDouble+12,hold=0,next=0;
  while(!Player.GetComponent<PlayerDash>().hasDash&&Time.realtimeSinceStartupAsDouble<until){double now=Time.realtimeSinceStartupAsDouble;if(Motor.Grounded&&now>=next){hold=now+.42;next=now+.7;}var k=new List<Key>();if(Mathf.Abs(Body.position.x-37.5f)>.2f)k.Add(Body.position.x<37.5f?Key.RightArrow:Key.LeftArrow);if(now<hold)k.Add(Key.Z);Input(k.ToArray());yield return null;}
  yield return Keys();Record("MEASURE shrine naturalPickup="+Player.GetComponent<PlayerDash>().hasDash+" position="+Body.position);yield return CameraWindow("A05-after-shrine",true);
  yield return Arrival("A04","A03-A04");yield return Move("A04-comparison-position",16,true);Health.RestoreMax();yield return CameraWindow("A04-height-comparison",false);Check(RoomSession.Instance.CurrentRoomId=="A04","height comparison remained in A04");
  if(N5Args.Has("-b17-pits"))yield return Pits();sampling=false;
 }
 IEnumerator Pits()
 {
  foreach(var p in new[]{new Vector3(1,38,40.5f),new Vector3(2,11,13.5f),new Vector3(3,20.5f,23),new Vector3(3,36,38.5f),new Vector3(4,17.5f,20),new Vector3(5,32.5f,35)})
  {
   string id="A0"+(int)p.x;yield return Arrival(id,"Entry");float left=p.y,right=p.z;PlaceForAuthorizedSetup(new Vector2((left+right)/2,1.1f));phase=id+"-pit-drop";yield return new WaitForSeconds(.7f);float floor=Body.position.y;double start=Time.realtimeSinceStartupAsDouble;phase=id+"-pit-climb";Input(Key.Z,Key.RightArrow);
   while(Time.realtimeSinceStartupAsDouble-start<3&&Body.position.x<right+.5f)yield return null;
   yield return Keys();Record("MEASURE pit "+id+" interval="+left+".."+right+" bottomFeet="+floor.ToString("F3")+" escape="+(Body.position.x>right&&Body.position.y>.9f)+" elapsed="+(Time.realtimeSinceStartupAsDouble-start).ToString("F3")+" position="+Body.position);
   PlaceForAuthorizedSetup(new Vector2(left-1.2f,1.02f));yield return new WaitForSeconds(.2f);phase=id+"-pit-running-jump";Input(Key.RightArrow);yield return new WaitForSeconds(.1f);Input(Key.RightArrow,Key.Z);start=Time.realtimeSinceStartupAsDouble;float minFeet=10;
   while(Time.realtimeSinceStartupAsDouble-start<2&&Body.position.x<right+.4f){if(Body.position.x>left&&Body.position.x<right)minFeet=Mathf.Min(minFeet,Body.position.y);yield return null;}
   yield return Keys();Record("MEASURE pit-jump "+id+" interval="+left+".."+right+" crossed="+(Body.position.x>right)+" minFeetOverPit="+minFeet.ToString("F3")+" position="+Body.position);
  }
 }
}
#endif
