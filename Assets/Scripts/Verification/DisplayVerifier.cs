#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public sealed class DisplayVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<DisplayVerifier>("-verify-display");
 protected override bool PreserveFocusBehavior=>N5Args.Value("-verify-display","")!="death";
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N5-TF-display.txt");
 int focusChanges;Rect hudTop,hudAbilities,deathBox;bool guiObserved;
 void Focus(bool focused){focusChanges++;lines.Add("focusChanged="+focused+" time="+Time.realtimeSinceStartupAsDouble);}
 protected override void Cleanup(){Application.focusChanged-=Focus;}
 void OnGUI(){if(Event.current.type!=EventType.Repaint||Player==null||GameHUD.Instance==null)return;var t=GameHUD.Instance.tuning;float s=Mathf.Min(Screen.width/1280f,Screen.height/720f);hudTop=new Rect(t.hudPosition.x*s,t.hudPosition.y*s,t.hudSize.x*s,t.hudSize.y*s);hudAbilities=new Rect((t.hudPosition.x+12)*s,(t.hudPosition.y+65)*s,324*s,t.abilityIconSize*s);deathBox=new Rect(0,Screen.height*.5f-24*s,Screen.width,48*s);guiObserved=true;}
 protected override IEnumerator Tests(){Application.focusChanged+=Focus;string scenario=N5Args.Value("-verify-display","start");if(N5Args.Has("-pogotest")){yield return Until(()=>FindFirstObjectByType<PogoTestCourse>()!=null&&Player!=null,"opt-in course launch");yield return Ready();}else if(FindFirstObjectByType<SaveMenu>()!=null||SceneManager.GetActiveScene().name=="Menu"){if(scenario=="abilities")yield return ContinueGame();else yield return NewGame();}else if(scenario!="ending")yield return Ready();yield return new WaitForSecondsRealtime(2);
  if(scenario=="hit"){yield return Traverse(()=>Health.HP<Health.tuning.playerMaxHP,true,35);Check(Health.HP>0&&!RoomSession.Instance.Respawning,"actual contact hit survives");}
  if(scenario=="death"){yield return Traverse(()=>RoomSession.Instance.Respawning,true,35);double deathObserved=Time.realtimeSinceStartupAsDouble;yield return new WaitForSecondsRealtime(.1f);Check(GameHUD.Instance.DeathOverlayVisible,"actual KillZone death overlay");lines.Add("MEASURE death capture delay="+(Time.realtimeSinceStartupAsDouble-deathObserved).ToString("F4"));}
  if(scenario=="boss"){yield return WalkTo(()=>FindFirstObjectByType<BossEncounter>().State==BossEncounter.Phase.MeleeTell,Key.RightArrow,"natural boss tell");}
  if(scenario=="abilities")Check(GameHUD.Instance.DashOwned&&GameHUD.Instance.DoubleJumpOwned,"actual both-ability HUD");
  if(scenario=="B04")Check(SceneManager.GetActiveScene().name=="B04","B04 startup scene unmodified");
  if(scenario=="ending")Check(FindFirstObjectByType<EndingMenu>()!=null,"Ending UI");
  yield return new WaitForEndOfFrame();Type capture=Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule",true);var texture=(Texture2D)capture.GetMethod("CaptureScreenshotAsTexture",new[]{typeof(int)}).Invoke(null,new object[]{1});Check(texture!=null,"EOF screenshot texture");var pixels=texture.GetPixels32();double sum=0,square=0;foreach(var p in pixels){double value=p.r+p.g+p.b;sum+=value;square+=value*value;}double variance=square/pixels.Length-Math.Pow(sum/pixels.Length,2);Check(variance>0,"nonconstant screenshot variance="+variance.ToString("F3"));Type converter=Type.GetType("UnityEngine.ImageConversion, UnityEngine.ImageConversionModule",true);byte[] png=(byte[])converter.GetMethod("EncodeToPNG",new[]{typeof(Texture2D)}).Invoke(null,new object[]{texture});string path=N5Args.Value("-verify-shot","Archive/validation-2026-10/B13-N5-TF-shots/capture.png");Directory.CreateDirectory(Path.GetDirectoryName(path));if(File.Exists(path))throw new IOException("Preserve shot");File.WriteAllBytes(path,png);Destroy(texture);
  lines.Add($"scene={SceneManager.GetActiveScene().name} scenario={scenario} actual={Screen.width}x{Screen.height} fullscreen={Screen.fullScreenMode} graphics={SystemInfo.graphicsDeviceType} focused={Application.isFocused} GUI-observed={guiObserved}");lines.Add($"HUD rectangles observed in Repaint top={hudTop} abilities={hudAbilities} death={deathBox}; screenshot={path}");var boss=FindFirstObjectByType<BossEncounter>();if(boss!=null){foreach(var pair in new[]{boss.transform,boss.warning.transform}){Vector3 v=Camera.main.WorldToViewportPoint(pair.position);lines.Add("boss/tell viewport="+v);Check(v.x>=0&&v.x<=1&&v.y>=0&&v.y<=1,"boss/tell viewport inside");}}lines.Add(focusChanges>0?"Focus event observed count="+focusChanges:"KNOWN-LIMIT focus loss/regain not forced; default focus settings preserved, no IgnoreFocus substitution");
 }
 IEnumerator Traverse(Func<bool> condition,bool jump,float seconds){double started=Time.realtimeSinceStartupAsDouble;while(!condition()){if(Time.realtimeSinceStartupAsDouble-started>seconds)throw new Exception("capture traversal timeout room="+RoomSession.Instance.CurrentRoomId+" pos="+Body.position);bool death=N5Args.Value("-verify-display","")=="death";bool hold=jump&&(!death||Body.position.x<14)&&(Time.realtimeSinceStartupAsDouble-started)%.9<.6;UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,hold?new UnityEngine.InputSystem.LowLevel.KeyboardState(Key.RightArrow,Key.Z):new UnityEngine.InputSystem.LowLevel.KeyboardState(Key.RightArrow));yield return null;}yield return Keys();}
}

#endif
