using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

[Serializable]public sealed class RouteSegment
{
 public string id,startScene,goal;public Vector2 startPosition;public bool dash,doubleJump;public int startHP;public float limitSeconds;
 public List<N5InputTick> inputs=new();public bool passed;public int attempts;public string result;
}
[Serializable]public sealed class RouteLibrary{public List<RouteSegment> segments=new();}
[DefaultExecutionOrder(-32000)]public sealed class RouteTickDriver:MonoBehaviour
{
 public Keyboard device;public Gamepad pad;public Func<N5InputTick> produce;public Action observe;public bool active;public int tick;
 void FixedUpdate(){if(!active)return;observe?.Invoke();if(!active)return;if(RoomSession.Instance?.Transitioning==true){if(pad!=null)InputSystem.QueueStateEvent(pad,new GamepadState());else InputSystem.QueueStateEvent(device,new KeyboardState());InputSystem.Update();return;}var entry=produce();var keys=new List<Key>();if(entry.axis>.1f)keys.Add(Key.RightArrow);else if(entry.axis<-.1f)keys.Add(Key.LeftArrow);if((entry.held&2)!=0)keys.Add(Key.Z);if((entry.held&4)!=0)keys.Add(Key.C);if((entry.held&8)!=0)keys.Add(Key.X);if((entry.held&16)!=0)keys.Add(Key.Enter);
 if((entry.held&32)!=0)keys.Add(Key.DownArrow);if((entry.held&64)!=0)keys.Add(Key.UpArrow);
 if(pad!=null){var state=new GamepadState{leftStick=new Vector2(entry.axis,(entry.held&32)!=0?-1:(entry.held&64)!=0?1:0)};if((entry.held&18)!=0)state=state.WithButton(GamepadButton.South);if((entry.held&4)!=0)state=state.WithButton(GamepadButton.East);if((entry.held&8)!=0)state=state.WithButton(GamepadButton.West);InputSystem.QueueStateEvent(pad,state,Time.fixedUnscaledTimeAsDouble);}else InputSystem.QueueStateEvent(device,new KeyboardState(keys.ToArray()),Time.fixedUnscaledTimeAsDouble);InputSystem.Update();tick++;}
}
public sealed class RouteRunner:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){string mode=N5Args.Value("-verify-route","");if(mode!=""&&!mode.StartsWith("-"))N5Args.Start<RouteRunner>("-verify-route");}
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N8-route-run.txt");
 readonly string[][] paths={new[]{"A01","A02"},new[]{"A02","A03"},new[]{"A03","A04"},new[]{"A04","A05"},new[]{"A05","A04","A03"},new[]{"A03","B01"},new[]{"B01","B02"},new[]{"B02","B03"},new[]{"B03","B04"},new[]{"B04","B03","B02","B01"},new[]{"B01","B05"},new[]{"B05","C01"},new[]{"C01","C02"},new[]{"C02","C03"},new[]{"C03","C04"},new[]{"C04","C06","Ending"},new[]{"Ending","Menu"}};
 RouteTickDriver driver;RouteLibrary library=new();RouteSegment current;int pathIndex,stage,stageTick,previous,attempt;bool complete,failed;string failure;double bossJumped=-1;int jumpUntil,nextAttack;bool teleported;
 protected override void Cleanup(){if(driver!=null){driver.active=false;if(driver.pad!=null)InputSystem.RemoveDevice(driver.pad);}}
 void Update(){if(driver!=null&&driver.active&&RoomSession.Instance?.Respawning==true)Fail("actual death room="+RoomSession.Instance.CurrentRoomId+" pos="+Body.position);}
 protected override IEnumerator Tests(){string mode=N5Args.Value("-verify-route","all");string file=N5Args.Value("-route-library","Validation/N8-route-final-library.json");if(mode=="levels"){yield return LevelSegments(file);yield break;}bool train=mode=="train"||mode.StartsWith("R",StringComparison.OrdinalIgnoreCase);int selected=mode.StartsWith("R",StringComparison.OrdinalIgnoreCase)?int.Parse(mode.Substring(1))-1:-1;
 driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;if(N5Args.Has("-route-gamepad")){driver.pad=InputSystem.AddDevice<Gamepad>();yield return PadNewGame();}else yield return NewGame();
 if(train){int begin=N5Args.Int("-route-start",1)-1;if(File.Exists(file)){library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(file));if(library.segments.Count>begin)library.segments.RemoveRange(begin,library.segments.Count-begin);}bool connected=begin==0&&selected<0;if(N5Args.Has("-route-prefix-replay")){for(int p=0;p<begin;p++){current=library.segments[p];yield return ReadyFor(p);Begin(p,true);yield return Until(()=>complete||failed,"recording prefix "+current.id,current.limitSeconds+10);driver.active=false;yield return Keys();Check(complete&&!failed,"recording prefix "+current.id+" "+failure);}connected=true;}for(int n=begin;n<paths.Length;n++){if(selected>=0&&n!=selected)continue;current=new RouteSegment{id=$"R{n+1:00}",startScene=paths[n][0],goal=paths[n][paths[n].Length-1],limitSeconds=n==15?180:n is 4 or 9?45:25};bool success=false;
 for(attempt=n==begin?N5Args.Int("-route-attempt-offset",0):0;attempt<Mathf.Min(20,(n==begin?N5Args.Int("-route-attempt-offset",0):0)+N5Args.Int("-route-attempt-limit",20))&&!success;attempt++){teleported=!connected||attempt>0;if(teleported)yield return Setup(n);else yield return ReadyFor(n);current.startPosition=n==16?Vector2.zero:Body.position;current.dash=n==16||Player.GetComponent<PlayerDash>().hasDash;current.doubleJump=n==16||Motor.hasDoubleJump;current.startHP=n==16?0:Health.HP;current.inputs.Clear();Begin(n,false);yield return Until(()=>complete||failed,"route segment "+current.id,current.limitSeconds+10);driver.active=false;if(driver.pad!=null)InputSystem.QueueStateEvent(driver.pad,new GamepadState());yield return Keys();success=complete&&!failed;current.attempts=attempt+1;current.passed=success;current.result=success?"PASS":"KNOWN-LIMIT "+failure;lines.Add(current.id+" attempt="+(attempt+1)+" ticks="+driver.tick+" startTeleport="+teleported+" "+current.result);SaveAttempt(n);if(!success&&RoomSession.Instance?.Respawning==true)yield return Ready();}
 library.segments.Add(current);connected=success;SaveLibrary(file);if(N5Args.Has("-route-continuous"))Check(success,"continuous recording "+current.id+" "+failure);if(selected>=0)break;}
 lines.Add("Training complete; per-segment outcomes are in library. Training success does not mean all-route acceptance.");}
 else{library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(file));Check(library.segments.Count==17&&library.segments.TrueForAll(s=>s.passed),"17 successful segments required for all; no teleport allowed");for(int n=0;n<17;n++){current=library.segments[n];yield return ReadyFor(n);Check(n==16||RoomSession.Instance.CurrentRoomId==current.startScene,"continuous start "+current.id);if(n!=16){Check(Player.GetComponent<PlayerDash>().hasDash==current.dash&&Motor.hasDoubleJump==current.doubleJump,"recorded ability ownership "+current.id);lines.Add("MEASURE continuous start deviation "+current.id+"="+Vector2.Distance(current.startPosition,Body.position).ToString("F3")+"u; no placement performed");}Begin(n,true);yield return Until(()=>complete||failed,"replay "+current.id,current.limitSeconds+10);driver.active=false;if(driver.pad!=null)InputSystem.QueueStateEvent(driver.pad,new GamepadState());yield return Keys();Check(complete&&!failed,current.id+" fixed-tick replay "+failure);}Check(FindFirstObjectByType<SaveMenu>()!=null,"all ends at title");}
 }
 IEnumerator PadTap(GamepadState state){InputSystem.QueueStateEvent(driver.pad,state);yield return new WaitForSecondsRealtime(.05f);InputSystem.QueueStateEvent(driver.pad,new GamepadState());yield return new WaitForSecondsRealtime(.05f);}
 // Reuse the same segment schema and fixed-tick driver for explicitly approved optional routes.
 // Placement occurs only before each independent segment; gameplay input performs the traversal.
 IEnumerator LevelSegments(string file)
 {
  library=JsonUtility.FromJson<RouteLibrary>(File.ReadAllText(file));
  driver=gameObject.AddComponent<RouteTickDriver>();driver.device=keyboard;yield return NewGame();
  foreach(var segment in library.segments)
  {
   current=segment;yield return Keys();
   Check(RoomSession.Instance.RequestTransition(current.startScene,"Assets/Scenes/"+current.startScene+".unity","Entry"),"level segment start "+current.id);
   yield return Ready();PlaceForAuthorizedSetup(current.startPosition);
   Player.GetComponent<PlayerDash>().hasDash=current.dash;Motor.hasDoubleJump=current.doubleJump;Health.RestoreMax();Player.ClearTransientInput();
   yield return new WaitForFixedUpdate();
   int pogos=Player.GetComponent<PlayerAttack>().PogosPerformed;
   complete=failed=false;failure="";driver.tick=0;
   driver.observe=()=>{
    if(RoomSession.Instance.Respawning)Fail("actual death "+current.id);
    else if(DiscoveryReward.Found(current.goal)&&Motor.Grounded){complete=true;driver.active=false;}
   };
   driver.produce=()=>{
    if(driver.tick>=current.inputs.Count){Fail("input exhausted "+current.id+" pos="+Body.position);return new N5InputTick();}
    return current.inputs[driver.tick];
   };
   driver.active=true;yield return Until(()=>complete||failed,"level segment "+current.id,current.limitSeconds+5);driver.active=false;yield return Keys();
   Check(complete&&!failed,current.id+" reward + shelf landing "+failure);
   lines.Add("MEASURE "+current.id+" position="+Body.position+" pogos="+(Player.GetComponent<PlayerAttack>().PogosPerformed-pogos)+" discoveries="+DiscoveryReward.Count);
  }
 }
 IEnumerator PadNewGame(){Check(N5Args.Has("-save-path"),"isolated explicit save path");yield return Until(()=>FindFirstObjectByType<SaveMenu>()!=null,"pad title");yield return new WaitForSecondsRealtime(.2f);if(FindFirstObjectByType<SaveMenu>().CanContinue)yield return PadTap(new GamepadState().WithButton(GamepadButton.DpadDown));yield return PadTap(new GamepadState().WithButton(GamepadButton.South));if(FindFirstObjectByType<SaveMenu>()?.Confirming==true)yield return PadTap(new GamepadState().WithButton(GamepadButton.South));yield return Ready();Check(RoomSession.Instance.CurrentRoomId=="A01","gamepad New Game");}
 IEnumerator ReadyFor(int n){if(n==16){yield return Until(()=>FindFirstObjectByType<EndingMenu>()!=null,"ending start");yield return new WaitForSecondsRealtime(.2f);}else{yield return Ready();yield return Keys();yield return Until(()=>Motor.Grounded&&Mathf.Abs(Body.linearVelocity.x)<.01f,"settled segment start",5);}}
 IEnumerator Setup(int n){yield return Keys();if(n==16){SceneManager.LoadScene("Assets/Scenes/Ending.unity");yield return ReadyFor(n);yield break;}if(RoomSession.Instance.CurrentRoomId!=paths[n][0]){Check(RoomSession.Instance.RequestTransition(paths[n][0],"Assets/Scenes/"+paths[n][0]+".unity","Entry"),"segment-start scene setup "+paths[n][0]);yield return Ready();}
 var room=FindFirstObjectByType<RoomDefinition>();Check(room.TryGetSpawn("Entry",out var spawn),"segment-start Entry");PlaceForAuthorizedSetup(spawn.transform.position);Player.GetComponent<PlayerDash>().hasDash=n>=4;Motor.hasDoubleJump=n>=9;Health.RestoreMax();Player.ClearTransientInput();yield return new WaitForSecondsRealtime(.3f);}
 void SaveLibrary(string file){string temp=file+".tmp";Directory.CreateDirectory(Path.GetDirectoryName(file));File.WriteAllText(temp,JsonUtility.ToJson(library,true));if(File.Exists(file))File.Delete(file);File.Move(temp,file);}
 void SaveAttempt(int n){string prefix=N5Args.Value("-route-prefix","Validation/N8");string path=prefix+"-"+current.id+"-a"+(attempt+1)+".txt";if(File.Exists(path))throw new IOException("Preserve "+path);File.WriteAllText(path,current.result+"\n"+JsonUtility.ToJson(current,true)+"\nActual Unity fixed-tick virtual Keyboard; teleports only at segment start.\n");}
 void Begin(int n,bool replay){pathIndex=0;stage=0;stageTick=0;previous=0;complete=failed=false;failure="";bossJumped=-1;jumpUntil=nextAttack=0;driver.tick=0;driver.observe=()=>Observe(n);driver.produce=()=>{N5InputTick input;if(replay){if(driver.tick>=current.inputs.Count){failed=true;failure="recording exhausted room="+RoomSession.Instance?.CurrentRoomId+" pos="+(Player==null?Vector2.zero:Body.position);return new N5InputTick();}input=current.inputs[driver.tick];}else{input=Generate(n);input.tick=driver.tick;input.pressed=input.held&~previous;input.released=previous&~input.held;previous=input.held;current.inputs.Add(input);}return input;};driver.active=true;}
 void Observe(int n){if(n==16){if(FindFirstObjectByType<SaveMenu>()!=null){complete=true;driver.active=false;}return;}if(n==15&&FindFirstObjectByType<EndingMenu>()!=null){complete=true;driver.active=false;return;}var s=RoomSession.Instance;if(s==null){Fail("session missing");return;}if(s.Respawning){Fail("actual death");return;}if(s.Transitioning)return;string room=s.CurrentRoomId;
 if(pathIndex<paths[n].Length-1&&room==paths[n][pathIndex+1]){pathIndex++;stage=0;stageTick=driver.tick;if(pathIndex==paths[n].Length-1&&n!=15){bool owned=n==3?Player.GetComponent<PlayerDash>().hasDash:n==8?Motor.hasDoubleJump:true;if(owned){complete=true;driver.active=false;}}}
 else if(room!=paths[n][pathIndex])Fail("wrong authored exit destination "+room);
 if(pathIndex==paths[n].Length-1&&n!=15){bool owned=n==3?Player.GetComponent<PlayerDash>().hasDash:n==8?Motor.hasDoubleJump:true;if(owned){complete=true;driver.active=false;}}
 if(driver.tick*Time.fixedDeltaTime>current.limitSeconds)Fail("tick limit pos="+Body.position+" room="+room+" stage="+stage);
 }
 void Fail(string why){failed=true;failure=why;driver.active=false;}
 void NextStage(){stage++;stageTick=driver.tick;}
 N5InputTick Generate(int n){var result=new N5InputTick();if(n==16){if(driver.tick%20==0)result.held=16;return result;}if(Player.InputLocked)return result;string room=RoomSession.Instance.CurrentRoomId;float x=Body.position.x,y=Body.position.y;int t=driver.tick-stageTick;int inputVariant=n==4?N5Args.Int("-route-input-variant",attempt):attempt;int jumpPeriod=45+(inputVariant%5)*3;int hold=24+(inputVariant/5)*3;bool twice=Motor.hasDoubleJump;int pulse=driver.tick%jumpPeriod;
 if(n==15&&room=="C06"){var boss=FindFirstObjectByType<BossEncounter>();if(!boss.Fighting){result.axis=1;return result;}double phase=Time.timeAsDouble-boss.PhaseStarted;if(bossJumped!=boss.PhaseStarted&&Motor.Grounded&&((boss.State==BossEncounter.Phase.MeleeTell&&phase>boss.tuning.meleeTell-.38)||(boss.State==BossEncounter.Phase.WaveTell&&phase>boss.tuning.waveTell-.38))){jumpUntil=driver.tick+28;bossJumped=boss.PhaseStarted;}if(x<15.65f)result.axis=1;else if(x>15.85f)result.axis=-1;if(driver.tick<jumpUntil)result.held|=2;if(driver.tick>=nextAttack&&Motor.Grounded){result.axis=1;result.held|=8;nextAttack=driver.tick+20;}return result;}
 if((n==3&&room=="A05"&&!Player.GetComponent<PlayerDash>().hasDash)||(n==8&&room=="B04"&&!Motor.hasDoubleJump)){Transform pickup=n==3?FindFirstObjectByType<DashPickup>()?.transform:FindFirstObjectByType<DoubleJumpPickup>()?.transform;if(pickup==null){Fail("pickup absent");return result;}result.axis=Axis(pickup.position.x-x);if(pulse<hold)result.held|=2;return result;}
 if(n==5){if(stage==0){result.axis=Axis(8.2f-x);if(pulse<hold)result.held|=2;if(x>7.3f&&x<8.7f&&y>3.7f&&Motor.Grounded)NextStage();}
 else if(stage==1){result.axis=1;if(t<30)result.held|=2;if(x>=11.7f&&y>5.7f&&Motor.Grounded)NextStage();}
 else if(stage==2){result.axis=x<15.2f?1:0;if(x>=15.2f)NextStage();}
 else if(stage==3){result.axis=1;if(t<3)result.held|=4;if(x>17.4f)NextStage();}
 else result.axis=1;return result;}
 if(n==10){if(stage==0){result.axis=Axis(23.4f-x);if(Mathf.Abs(x-23.4f)<.3f&&Motor.Grounded)NextStage();}
 else if(stage==1){result.axis=1;if(t<14||t>=16&&t<34)result.held|=2;if(x>26.5f&&y>5.3f&&Motor.Grounded)NextStage();}
 else{var goal=FindExit("B05");result.axis=goal==null?0:Axis(goal.transform.position.x-x);}return result;}
 if(n==13&&room=="C02"){if(stage==0){result.axis=Axis(9.3f-x);if(y<3.7f&&pulse<hold)result.held|=2;if(Mathf.Abs(x-9.3f)<.3f&&Motor.Grounded)NextStage();}
 else if(stage==1){result.axis=1;if(t<14||t>=16&&t<40)result.held|=2;if(x>12&&x<16.4f&&y>7.5f&&Motor.Grounded)NextStage();}
 else if(stage==2){result.axis=x<15.3f?1:0;if(x>=15.3f)NextStage();}
 else if(stage==3){result.axis=1;if(t<40)result.held|=2;if(t==15+(attempt%5))result.held|=4;if(x>21&&Motor.Grounded)NextStage();}
 else if(stage==4){result.axis=x<24.3f?1:0;if(x>=24.3f)NextStage();}
 else if(stage==5){result.axis=1;if(t<35)result.held|=2;if(x>27.3f&&y>7.5f&&Motor.Grounded)NextStage();}
 else result.axis=1;return result;}
 string destination=paths[n][Math.Min(pathIndex+1,paths[n].Length-1)];var exit=FindExit(destination);if(exit==null){if(n==15&&destination=="Ending")return result;Fail("authored exit absent "+room+" -> "+destination);return result;}result.axis=Axis(exit.transform.position.x-x);if((n!=2||x<9||x>20)&&(pulse<hold||twice&&pulse>=hold+2&&pulse<jumpPeriod-3))result.held|=2;if(driver.tick%20==0)result.held|=8;if(n==4&&inputVariant<5&&pulse==18&&Player.GetComponent<PlayerDash>().hasDash)result.held|=4;if(n==4&&inputVariant>=5&&room=="A04"&&!Motor.Grounded){result.held|=32;if(driver.tick%16==0)result.held|=8;}return result;
 }
 RoomExit FindExit(string destination)=>Array.Find(FindObjectsByType<RoomExit>(FindObjectsSortMode.None),e=>e.destinationRoomId==destination);
 static float Axis(float dx)=>dx>.25f?1:dx<-.25f?-1:0;
}
