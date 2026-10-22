#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B20BodyProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B20BodyProbe>("-verify-b20-body");
 bool head,launched,landed;
 void Input(params Key[] keys)=>InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
 void Capture(string path)
 {
  var camera=FindFirstObjectByType<RoomCameraRig>().OutputCamera;var previous=camera.targetTexture;var active=RenderTexture.active;
  var rt=new RenderTexture(1280,720,24);var texture=new Texture2D(1280,720,TextureFormat.RGB24,false);
  camera.targetTexture=rt;camera.Render();RenderTexture.active=rt;texture.ReadPixels(new Rect(0,0,1280,720),0,0);texture.Apply();
  var conversion=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true);File.WriteAllBytes(path,(byte[])conversion.GetMethod("EncodeToPNG",new[]{typeof(Texture2D)}).Invoke(null,new object[]{texture}));
  camera.targetTexture=previous;RenderTexture.active=active;Destroy(texture);rt.Release();Destroy(rt);
 }
 IEnumerator Reset(float x,float y=1.015f){yield return Keys();Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(x,y));yield return new WaitForSeconds(.15f);}
 IEnumerator Jump()
 {
  Input(Key.Z);head=launched=landed=false;double until=Time.realtimeSinceStartupAsDouble+1.5;
  while(Time.realtimeSinceStartupAsDouble<until)
  {
   launched|=Body.linearVelocity.y>0;var points=new ContactPoint2D[32];int count=Player.GetComponent<Collider2D>().GetContacts(points);head|=points.Take(count).Any(p=>p.normal.y<-.7f);
   if(launched&&Motor.Grounded){landed=true;break;}yield return null;
  }
  yield return Keys();
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return Until(()=>Player.GetComponent<BodySizeOverride>()!=null,"body component");
  var size=Player.GetComponent<BodySizeOverride>();var jumps=JsonUtility.FromJson<B20Metrics.JumpData>(File.ReadAllText("Validation/_raw/B20/T2-metrics.json"));
  Check(RoomSession.Instance.CurrentRoomId=="BodySizeTest","body test Scene loaded");Check(Mathf.Abs(size.CurrentHeight-1.6f)<.001f,"default body height 1.6");
  foreach(float height in BodySizeOverride.Heights)
  {
   yield return Reset(3);float feet=Player.GetComponent<Collider2D>().bounds.min.y;
   if(Mathf.Abs(size.CurrentHeight-height)>.001f)yield return Tap(Key.F6);
   Check(Mathf.Abs(size.CurrentHeight-height)<.001f,"actual F6 height "+height);Check(Mathf.Abs(Player.GetComponent<Collider2D>().bounds.min.y-feet)<.003f,"feet preserved on switch "+height);
   int stands=0,walks=0,landings=0;
   for(int i=0;i<10;i++)
   {
    yield return Reset(3);if(Motor.Grounded)stands++;yield return Keys(Key.RightArrow);yield return new WaitForSeconds(.1f);if(Motor.Grounded&&Body.position.x>3.1f)walks++;yield return Keys();yield return Jump();if(launched&&landed)landings++;
   }
   lines.Add("MEASURE body height="+height+" standing="+stands+"/10 walking="+walks+"/10 jumpLanding="+landings+"/10 width="+Player.GetComponent<Collider2D>().bounds.size.x.ToString("F5"));Check(stands==10&&walks==10&&landings==10,"grounding "+height);
   float[] clearance={2.2f,2.6f,3,3.5f,4,4.5f};
   for(int i=0;i<6;i++)
   {
    yield return Reset(8.5f+8*i);yield return Jump();bool expected=clearance[i]<height+jumps.full-.003f;
    lines.Add("MEASURE bar height="+height+" clearance="+clearance[i]+" expectedHead="+expected+" actualHead="+head+" launched="+launched+" landed="+landed);Check(head==expected&&launched&&landed,"bar prediction "+height+" "+clearance[i]);
   }
   yield return Reset(86);var attack=Player.GetComponent<PlayerAttack>();
   PlaceForAuthorizedSetup(new Vector2(86,5.5f));Input(Key.DownArrow);
   int before=attack.PogosPerformed;double until=Time.realtimeSinceStartupAsDouble+2;bool pressed=false;float pressFeet=0;int direction=0;
   while(attack.PogosPerformed==before&&Time.realtimeSinceStartupAsDouble<until)
   {
    if(!pressed&&!Motor.Grounded&&Body.linearVelocity.y<0&&Body.position.y<=3.5f+attack.tuning.downAttackHeight*.75f+.3f){pressFeet=Body.position.y;Input(Key.DownArrow,Key.X);pressed=true;}
    else {direction=attack.VerticalDirection;Input(Key.DownArrow);}yield return null;
   }
   yield return Keys();lines.Add("MEASURE pogo height="+height+" pressed="+pressed+" pressFeet="+pressFeet.ToString("F4")+" attackDirection="+direction+" count="+(attack.PogosPerformed-before));Check(attack.PogosPerformed==before+1,"one pogo "+height);
   Health.RestoreMax();yield return new WaitForSeconds(.1f);var enemy=FindFirstObjectByType<EnemyPatrol>();var enemyShape=enemy.GetComponent<Collider2D>();float gap=attack.tuning.attackRange*.75f;
   yield return Reset(enemyShape.bounds.min.x-Player.GetComponent<Collider2D>().bounds.extents.x-gap);yield return Keys(Key.RightArrow);yield return Keys();int hp=enemy.HP;yield return Tap(Key.X);yield return new WaitForSeconds(.25f);bool hit=enemy==null||enemy.HP<hp;
   lines.Add("MEASURE combat height="+height+" attackReach="+attack.HitboxSize.x.ToString("F5")+" downReach="+attack.tuning.downAttackHeight.ToString("F5")+" enemyGapFixture="+gap.ToString("F5")+" E1Hit="+hit+" pogoCount=1");Check(hit,"E1 hit "+height);
   yield return Reset(8.5f);yield return new WaitForSeconds(.2f);Capture(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Evidence)),"body-"+height.ToString("F1")+".png"));yield return new WaitForSeconds(.1f);
  }
 }
}
#endif
