using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Observation only: never writes progress, changes timeScale or mutates gameplay.
public sealed class PlaytestLogger:MonoBehaviour
{
 public static PlaytestLogger Instance{get;private set;}
 public string CsvPath{get;private set;}
 public bool Recording{get;private set;}
 public double ActiveSeconds=>activeSeconds;
 public double PauseSeconds=>pauseSeconds;
 public int MapOpens{get;private set;}
 public int MapChanges{get;private set;}
 double largeMapSeconds,largeMapBegan=-1;
 public double LargeMapSeconds=>largeMapSeconds+(largeMapBegan<0?0:Time.realtimeSinceStartupAsDouble-largeMapBegan);
 string arrivalExit="";
 readonly Dictionary<string,double> regional=new();readonly Dictionary<string,int> deaths=new();
 GameState state;BossEncounter boss;string room="",nextRoom="",checkpoint="";bool dash,jump,wasRespawning,wasPaused,wasFighting,menuSeen,warningSent;
 double lastSample,activeSeconds,pauseSeconds,roomBegan;int bossAttempts;DateTime menuSaveTime;
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
 static void Initialize(){if(Instance!=null)return;var go=new GameObject("Playtest Logger");DontDestroyOnLoad(go);go.AddComponent<PlaytestLogger>();}
 void Awake(){if(Instance!=null&&Instance!=this){Destroy(gameObject);return;}Instance=this;SceneManager.sceneLoaded+=Loaded;}
 DateTime SaveTime(){try{return File.Exists(ProgressSave.FilePath)?File.GetLastWriteTimeUtc(ProgressSave.FilePath):DateTime.MinValue;}catch(Exception){return DateTime.MinValue;}}
 void Loaded(Scene scene,LoadSceneMode mode)
 {
  if(scene.name=="Menu"){Finish();menuSeen=true;menuSaveTime=SaveTime();nextRoom="";return;}
  if(scene.name=="Ending"){if(Recording){if(V3SceneMap.Enabled)ObserveExit("SummitEnding","Ending");Sample();Write("ending","",null,"","Ending");Finish();}return;}
  foreach(var root in scene.GetRootGameObjects()){var definition=root.GetComponent<RoomDefinition>();if(definition!=null){nextRoom=definition.roomId;break;}}
  // sceneLoaded runs before RoomSession places the player in the next room. The old
  // position identifies which authored exit to that destination was actually used.
  if(V4SceneMap.Enabled&&Recording&&mode==LoadSceneMode.Additive&&nextRoom!=room){
   var player=RoomSession.Instance?.Player;RoomExit nearest=null;float distance=float.PositiveInfinity;
   if(player!=null)foreach(var exit in FindObjectsByType<RoomExit>(FindObjectsSortMode.None)){
    if(!exit.enabled||exit.gameObject.scene==scene||exit.destinationRoomId!=nextRoom||exit.GetComponentInParent<RoomDefinition>()?.roomId!=room)continue;
    float gap=Vector2.SqrMagnitude((Vector2)exit.GetComponent<Collider2D>().bounds.ClosestPoint(player.transform.position)-(Vector2)player.transform.position);if(gap<distance){distance=gap;nearest=exit;}
   }
   ObserveExit(RoomSession.Instance?.Respawning==true?"respawn":nearest!=null&&distance<1?nearest.exitId:"unobserved",nextRoom);
  }
  var pending=ProgressSave.Pending;
  // NewGame writes the reset slot before loading A01. Continue only reads it.
  if(!Recording&&menuSeen&&nextRoom=="A01"&&ProgressSave.Enabled&&pending!=null&&!pending.dash&&!pending.doubleJump&&!pending.bossDefeated&&!pending.endingReached&&pending.checkpointId=="CP-A01"&&SaveTime()!=menuSaveTime)Begin();
 }
 void Begin()
 {
  Unbind();regional.Clear();deaths.Clear();room=checkpoint=arrivalExit="";dash=jump=wasRespawning=wasPaused=wasFighting=false;activeSeconds=pauseSeconds=0;bossAttempts=0;lastSample=Time.realtimeSinceStartupAsDouble;warningSent=false;MapOpens=MapChanges=0;largeMapSeconds=0;largeMapBegan=-1;
  try{string folder=(V3SceneMap.Enabled||V4SceneMap.Enabled)?Path.GetFullPath(Automated?"Validation/_raw/playtest":"Playtest"):Path.Combine(Application.persistentDataPath,"Playtest");Directory.CreateDirectory(folder);string stamp=DateTime.Now.ToString("yyyyMMdd-HHmmss");string path=Path.Combine(folder,stamp+".csv");for(int i=1;File.Exists(path);i++)path=Path.Combine(folder,stamp+"-"+i+".csv");CsvPath=path;File.WriteAllText(path,"utc,event,room,x,y,cause,elapsed_active,pause_total,details\n");Recording=true;
  #if UNITY_EDITOR || DEVELOPMENT_BUILD
  if(V4WorldMap.Instance?.Mode==V4WorldMap.Display.Large)largeMapBegan=Time.realtimeSinceStartupAsDouble;
  #endif
  Write("new_game","A01",null,"","polling; fatal cause intentionally unrecorded");}
  catch(Exception e){Fail(e);}
 }
 void Sample(){double now=Time.realtimeSinceStartupAsDouble,dt=Math.Max(0,now-lastSample);lastSample=now;if(!Recording)return;if(wasPaused)pauseSeconds+=dt;else{activeSeconds+=dt;if(!string.IsNullOrEmpty(room)){string region=room=="C06"&&wasFighting?"Boss":room.Substring(0,1);regional.TryGetValue(region,out double total);regional[region]=total+dt;}}}
 void Bind(){var candidate=UnityEngine.Object.FindFirstObjectByType<GameState>();if(candidate==state)return;if(state!=null)state.Changed-=StateChanged;state=candidate;if(state!=null){state.Changed+=StateChanged;wasPaused=state.Paused;wasRespawning=state.Respawning;}}
 void StateChanged(){if(!Recording)return;Sample();bool paused=state.Paused;if(paused!=wasPaused)Write(paused?"pause_start":"pause_end",room,null,"","Pause excluded from elapsed_active");wasPaused=paused;DeathEdge();}
 void DeathEdge(){bool respawning=state!=null&&state.Respawning;if(respawning&&!wasRespawning){var player=RoomSession.Instance?.Player;string id=RoomSession.Instance?.CurrentRoomId??room;deaths.TryGetValue(id,out int count);deaths[id]=count+1;Write("death",id,player!=null?(Vector2?)player.transform.position:null,"unrecorded","");if(id=="C06")Write("boss_death",id,player!=null?(Vector2?)player.transform.position:null,"unrecorded","");}wasRespawning=respawning;}
 void Update()
 {
  if(!Recording)return;Sample();Bind();var session=RoomSession.Instance;if(session?.Player==null)return;
  if(session.CurrentRoomId!=room&&!string.IsNullOrEmpty(session.CurrentRoomId)&&session.CurrentRoomId==nextRoom){string previous=room;room=session.CurrentRoomId;roomBegan=activeSeconds;Write("room_enter",room,session.Player.transform.position,"",V4SceneMap.Enabled?"from="+previous+";via="+arrivalExit:"");}
  DeathEdge();if(state!=null&&state.Paused!=wasPaused)StateChanged();
  if(session.CheckpointId!=checkpoint){checkpoint=session.CheckpointId;if(checkpoint!=null)Write("checkpoint",room,session.Player.transform.position,"",checkpoint);}
  bool newDash=session.Player.GetComponent<PlayerDash>().hasDash,newJump=session.Player.GetComponent<PlayerMotor>().hasDoubleJump;if(newDash&&!dash)Write("ability",room,session.Player.transform.position,"","Dash");if(newJump&&!jump)Write("ability",room,session.Player.transform.position,"","DoubleJump");dash=newDash;jump=newJump;
  var candidate=UnityEngine.Object.FindFirstObjectByType<BossEncounter>();if(candidate!=boss){if(boss!=null)boss.Defeated-=BossDefeated;boss=candidate;wasFighting=false;if(boss!=null)boss.Defeated+=BossDefeated;}
  bool fighting=boss!=null&&boss.Fighting;if(fighting&&!wasFighting){bossAttempts++;Write("boss_start",room,null,"","attempt="+bossAttempts);}wasFighting=fighting;
 }
 void BossDefeated(){Sample();Write("boss_defeated","C06",null,"","");wasFighting=false;}
 public void ObserveExit(string exitId,string destination)
 {
  if(!Recording||!(V3SceneMap.Enabled||V4SceneMap.Enabled))return;Sample();var player=RoomSession.Instance?.Player;arrivalExit=exitId;
  Write("room_exit",room,player!=null?(Vector2?)player.transform.position:null,"","exit="+exitId+";to="+destination+";room_active_seconds="+Number(activeSeconds-roomBegan));
 }
 public void ObserveMap(string from,string to){if(!Recording||!V4SceneMap.Enabled)return;MapChanges++;if(from=="Off"||to=="Large")MapOpens++;double now=Time.realtimeSinceStartupAsDouble;if(largeMapBegan>=0){largeMapSeconds+=now-largeMapBegan;largeMapBegan=-1;}if(to=="Large")largeMapBegan=now;Write("map_change",room,null,"","from="+from+";to="+to+";opens="+MapOpens+";large_seconds="+Number(LargeMapSeconds));}
 // Automated runs (batchmode or any -verify-* argument) log outside the repo's Playtest/; launcher play never passes these.
 static bool Automated=>Application.isBatchMode||Array.Exists(Environment.GetCommandLineArgs(),a=>a.StartsWith("-verify-"));
 static string Cell(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
 static string Number(double value)=>value.ToString("F6",CultureInfo.InvariantCulture);
 void Write(string kind,string id,Vector2? at,string cause,string details){if(!Recording)return;try{File.AppendAllText(CsvPath,string.Join(",",Cell(DateTime.UtcNow.ToString("O")),Cell(kind),Cell(id),at.HasValue?Number(at.Value.x):"",at.HasValue?Number(at.Value.y):"",Cell(cause),Number(activeSeconds),Number(pauseSeconds),Cell(details))+"\n");}catch(Exception e){Fail(e);}}
 void Fail(Exception e){Recording=false;if(!warningSent){warningSent=true;Debug.LogWarning("Playtest logging unavailable: "+e.Message);}}
 public void Finish(){if(!Recording)return;Sample();foreach(string key in new[]{"A","B","C","Boss"}){regional.TryGetValue(key,out double seconds);Write("summary_region",key,null,"",Number(seconds));}foreach(var pair in deaths)Write("summary_deaths",pair.Key,null,"",pair.Value.ToString(CultureInfo.InvariantCulture));Write("summary_boss_attempts","C06",null,"",bossAttempts.ToString(CultureInfo.InvariantCulture));Write("summary_pause","",null,"",Number(pauseSeconds));if(V4SceneMap.Enabled){Write("summary_current_room",room,null,"","room_active_seconds="+Number(activeSeconds-roomBegan));Write("summary_map",room,null,"","opens="+MapOpens+";changes="+MapChanges+";large_seconds="+Number(LargeMapSeconds));largeMapSeconds=LargeMapSeconds;largeMapBegan=-1;}Recording=false;Unbind();}
 void Unbind(){if(state!=null)state.Changed-=StateChanged;if(boss!=null)boss.Defeated-=BossDefeated;state=null;boss=null;}
 void OnApplicationQuit()=>Finish();
 void OnDestroy(){Finish();Unbind();SceneManager.sceneLoaded-=Loaded;if(Instance==this)Instance=null;}
}
