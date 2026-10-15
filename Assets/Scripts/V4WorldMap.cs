#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class V4WorldMap:MonoBehaviour
{
 public enum Display { Off, Small, Large }
 public static V4WorldMap Instance{get;private set;}
 public Display Mode{get;private set;}=Display.Small;
 ProgressData savedProgress;float nextProgressRead;
 public void RefreshProgress()=>ProgressSave.TryLoad(out savedProgress);
 public bool GateUnlocked(string gate)=>gate switch{"Dash"=>savedProgress?.dash==true,"WallJump"=>savedProgress?.wallJump==true,"DJ"=>savedProgress?.doubleJump==true,"Slam"=>savedProgress?.hasSlam==true,"Boss"=>savedProgress?.bossDefeated==true,_=>false};
 public Color GateDisplayColor(string gate)=>GateUnlocked(gate)?new Color(.35f,1,.5f):GateColor(gate);
 public string CheckpointRoom{get{string cp=savedProgress?.checkpointId;string id=cp!=null&&cp.StartsWith("CP-",StringComparison.Ordinal)?cp.Substring(3):null;return WorldMapLayout.Current?.Room(id)!=null?id:null;}}
 public bool All=>N5Args.Has("-map-all");
 public MapRoom[] VisibleRooms=>WorldMapLayout.Current?.rooms.Where(r=>All||ProgressSave.HasVisited(r.id)).ToArray()??Array.Empty<MapRoom>();
 public MapDoor[] VisibleDoors=>WorldMapLayout.Current?.doors.Where(d=>d.type!="elevator"&&(All||ProgressSave.HasVisited(d.a)||ProgressSave.HasVisited(d.b))).ToArray()??Array.Empty<MapDoor>();
 public Vector2 PlayerWorld{get{var s=RoomSession.Instance;var r=WorldMapLayout.Current?.Room(s?.CurrentRoomId);return r==null||s?.Player==null?Vector2.zero:(Vector2)s.Player.transform.position+new Vector2(r.rect[0],r.rect[1]);}}
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){if(!V4SceneMap.Enabled)return;LoadEditorLayout();if(N5Args.Has("-no-map")){Debug.Log("MAP off (-no-map)");return;}var go=new GameObject("V4 world map");DontDestroyOnLoad(go);go.AddComponent<V4WorldMap>();Debug.Log("MAP small; M cycles off/small/large; gameplay continues");}
 static void LoadEditorLayout(){
 #if UNITY_EDITOR
 WorldMapLayout.Load(UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Maps/"+WorldMapLayout.Name+".json"));
 #endif
 }
 void Awake(){Instance=this;gameObject.AddComponent<V4MiniMap>();}
 void OnDestroy(){if(Instance==this)Instance=null;}
 public void Cycle(){Display previous=Mode;Mode=(Display)(((int)Mode+1)%3);PlaytestLogger.Instance?.ObserveMap(previous.ToString(),Mode.ToString());Debug.Log("MAP state="+Mode);}
 void Update(){if(Keyboard.current?.mKey.wasPressedThisFrame==true)Cycle();if(Mode!=Display.Off&&RoomSession.Instance?.Player!=null&&Time.unscaledTime>=nextProgressRead){RefreshProgress();nextProgressRead=Time.unscaledTime+.25f;}}
 public static string GateLabel(string gate)=>gate switch{"Dash"=>"D","WallJump"=>"W","DJ"=>"J","Slam"=>"S","Boss"=>"B",_=>""};
 public static Color GateColor(string gate)=>gate switch{"Dash"=>Color.cyan,"WallJump"=>Color.yellow,"DJ"=>new Color(.8f,.5f,1),"Slam"=>new Color(1,.6f,.2f),"Boss"=>new Color(1,.35f,.35f),_=>Color.white};
 static void Line(Vector2 a,Vector2 b,Color color,float width=2){var old=GUI.matrix;var delta=b-a;GUI.color=color;GUIUtility.RotateAroundPivot(Mathf.Atan2(delta.y,delta.x)*Mathf.Rad2Deg,a);GUI.DrawTexture(new Rect(a.x,a.y-width/2,delta.magnitude,width),Texture2D.whiteTexture);GUI.matrix=old;GUI.color=Color.white;}
 static void Border(Rect r,Color c){Line(r.min,new Vector2(r.xMax,r.yMin),c);Line(new Vector2(r.xMax,r.yMin),r.max,c);Line(r.max,new Vector2(r.xMin,r.yMax),c);Line(new Vector2(r.xMin,r.yMax),r.min,c);}
 void OnGUI()
 {
  if(Mode==Display.Off||RoomSession.Instance?.Player==null||WorldMapLayout.Current==null)return;
  var rooms=VisibleRooms;if(rooms.Length==0)return;
  Rect panel=Mode==Display.Small?new Rect(Screen.width-252,12,240,150):new Rect(Screen.width*.15f,Screen.height*.15f,Screen.width*.7f,Screen.height*.7f);
  GUI.color=new Color(0,0,0,.78f);GUI.DrawTexture(panel,Texture2D.whiteTexture);GUI.color=Color.white;
  GUI.Label(new Rect(panel.x+6,panel.y+2,panel.width-12,20),"M | "+Mode+" | "+rooms.Length+" rooms");
  var area=new Rect(panel.x+8,panel.y+24,panel.width-16,panel.height-32);GUI.BeginGroup(area);
  float x0=rooms.Min(r=>r.rect[0]),y0=rooms.Min(r=>r.rect[1]),x1=rooms.Max(r=>r.rect[2]),y1=rooms.Max(r=>r.rect[3]);
  Vector2 center=new Vector2((x0+x1)/2,(y0+y1)/2);float scale=Mathf.Min((area.width-48)/Mathf.Max(1,x1-x0),(area.height-48)/Mathf.Max(1,y1-y0));
  if(Mode==Display.Small){var current=WorldMapLayout.Current.Room(RoomSession.Instance.CurrentRoomId);if(current!=null){center=new Vector2((current.rect[0]+current.rect[2])/2,(current.rect[1]+current.rect[3])/2);scale=Mathf.Min((area.width-48)/(current.rect[2]-current.rect[0]),(area.height-48)/(current.rect[3]-current.rect[1]));}}
  Vector2 P(float x,float y)=>new Vector2(area.width/2+(x-center.x)*scale,area.height/2-(y-center.y)*scale);
  var label=new GUIStyle(GUI.skin.label){fontSize=11,alignment=TextAnchor.MiddleCenter};
  foreach(var r in rooms){Vector2 top=P(r.rect[0],r.rect[3]),bottom=P(r.rect[2],r.rect[1]);var box=Rect.MinMaxRect(top.x,top.y,bottom.x,bottom.y);GUI.color=new Color(.2f,.35f,.45f,.65f);GUI.DrawTexture(box,Texture2D.whiteTexture);GUI.color=Color.white;Border(box,r.id==RoomSession.Instance.CurrentRoomId?Color.yellow:new Color(.5f,.6f,.65f));GUI.Label(box,r.id,label);if(r.id==CheckpointRoom)GUI.Label(new Rect(box.x,box.y+box.height*.55f,box.width,Mathf.Min(16,box.height*.45f)),"CP",label);}
  foreach(var d in VisibleDoors){if(d.span==null||d.span.Length!=2)continue;Vector2 a=d.edge=="x"?P(d.at,d.span[0]):P(d.span[0],d.at),b=d.edge=="x"?P(d.at,d.span[1]):P(d.span[1],d.at);Color c=GateDisplayColor(d.gate);if(d.type=="shortcut"){for(int i=0;i<5;i+=2)Line(Vector2.Lerp(a,b,i/5f),Vector2.Lerp(a,b,(i+1)/5f),c,3);}else Line(a,b,c,3);var middle=(a+b)/2;string text=GateLabel(d.gate)+(d.type=="down"?" ↓":"");GUI.color=c;GUI.Label(new Rect(middle.x+3,middle.y-14,40,20),text);GUI.color=Color.white;if(!All&&(!ProgressSave.HasVisited(d.a)||!ProgressSave.HasVisited(d.b))){string hidden=ProgressSave.HasVisited(d.a)?d.b:d.a;var r=WorldMapLayout.Current.Room(hidden);Vector2 direction=r==null?Vector2.right:(P((r.rect[0]+r.rect[2])/2,(r.rect[1]+r.rect[3])/2)-middle).normalized;Vector2 q=middle+direction*13;GUI.Label(new Rect(q.x-6,q.y-9,16,18),"?");}}
  Vector2 point=P(PlayerWorld.x,PlayerWorld.y);GUI.color=Color.green;GUI.DrawTexture(new Rect(point.x-3,point.y-3,6,6),Texture2D.whiteTexture);GUI.color=Color.white;GUI.EndGroup();
 }
}
#endif
