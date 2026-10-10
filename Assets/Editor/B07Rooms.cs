using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using A=V3RoomRoutes.Action;
using P=V3RoomRoutes.Point;
using R=V3RoomRoutes.Route;
using Object=UnityEngine.Object;

public static class B07Rooms
{
 static RoomDefinition room;static V3RoomRoutes plan;static float width,height;static Color tone;
 static P Point(float x,float y,A action=A.Walk,string id="")=>new P{position=new Vector2(x,y),action=action,id=id};
 static R Route(string name,string goal,bool dash,bool wall,bool jump,params P[] points)=>new R{name=name,goal=goal,dash=dash,wallJump=wall,doubleJump=jump,points=points};
 static GameObject Block(string name,float x,float y,float w,float h,bool marked=false)
 {
  var go=S3Authoring.Block(room,"Landing",name,new Vector2(x,y),new Vector2(w,h));go.GetComponent<SpriteRenderer>().color=tone;if(marked){go.AddComponent<ClimbableWall>();go.GetComponent<SpriteRenderer>().color=Color.cyan;}return go;
 }
 static void Platform(string name,float x,float top,float w=4)=>Block(name,x,top-.25f,w,.5f);
 static void Floor(float left,float right,float top=1)=>Block("Safe floor",(left+right)/2,top-1,right-left,2);
 static void Spawn(string id,float x,float y)=>S3Authoring.Spawn(room,id,new Vector2(x,y));
 static RoomExit Exit(string target,float x,float y,string arrival="Entry",bool main=true,string id=null)
 {
  var exit=S3Authoring.Exit(room,id??target,new Vector2(x,y),target,arrival);exit.destinationScenePath=V3SceneMap.Path(target);exit.mainPath=main;return exit;
 }
 static void Checkpoint(string id,float x,float y)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Checkpoint.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);var cp=go.GetComponent<Checkpoint>();cp.checkpointId=id;cp.spawn=S3Authoring.Spawn(room,id,new Vector2(x,y));
 }
 static GameObject Pickup<T>(float x,float y)where T:Component
 {
  var go=new GameObject(typeof(T).Name);go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.AddComponent<BoxCollider2D>().isTrigger=true;var sr=go.AddComponent<SpriteRenderer>();sr.sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;sr.color=Color.green;go.AddComponent<T>();return go;
 }
 static void Reward(string id,float x,float y)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/DiscoveryReward.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.GetComponent<DiscoveryReward>().candidateId="V3-"+room.roomId+"-"+id;
 }
 static void Enemy(string type,float x,float y)
 {
  var go=new GameObject(type+" respawn");go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/"+type+".prefab");
  if(type=="E1"){
   var live=(GameObject)PrefabUtility.InstantiatePrefab(prefab);live.transform.SetParent(go.transform);live.transform.position=go.transform.position;
   var left=new GameObject("Patrol left");left.transform.SetParent(go.transform);left.transform.position=new Vector2(x-1.2f,y);
   var right=new GameObject("Patrol right");right.transform.SetParent(go.transform);right.transform.position=new Vector2(x+1.2f,y);
   var reset=go.AddComponent<EnemyRespawn>();reset.prefab=prefab;reset.live=live.GetComponent<EnemyPatrol>();reset.leftPoint=left.transform;reset.rightPoint=right.transform;reset.live.leftPoint=left.transform;reset.live.rightPoint=right.transform;
  }else go.AddComponent<EnemyReset>().prefab=prefab;
  plan.enemyCount++;
 }
 static void Pogo(float x,float y)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PogoTarget.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.transform.localScale=new Vector3(2,.5f,1);
 }
 static void Sign(float x,float y,string text)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/SignPost.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.GetComponent<SignPost>().message=text;
 }
 static void Gate(string text,float x,float y,float w,float h)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V3/DecorativeGate.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.transform.localScale=new Vector3(w,h,1);go.GetComponent<V3DecorativeGate>().meaning=text;
 }
 static void Wall(string id,float x,float y,float h,BreakableWall.Side side,bool secret=false)
 {
  var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/BreakableWall_"+(secret?"Secret":"Shortcut")+".prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.transform.localScale=new Vector3(1,h,1);var wall=go.GetComponent<BreakableWall>();wall.wallId=id;wall.breakableFrom=side;
 }
 static void Shell(string id,float w,float h)
 {
  EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");room=Object.FindFirstObjectByType<RoomDefinition>();
  var rig=Object.FindFirstObjectByType<RoomCameraRig>();var boundary=rig.boundary;
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=boundary.transform&&!rig.transform.IsChildOf(child))Object.DestroyImmediate(child.gameObject);
  // Remove inherited non-room level roots while preserving camera output and UI bindings.
  foreach(var collider in Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).ToArray())if(collider!=boundary&&!collider.transform.IsChildOf(room.transform))Object.DestroyImmediate(collider.gameObject);
  room.roomId=id;room.name=id+" V3";room.initialSpawnId="Entry";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
  width=w;height=h;tone=id[0]=='A'?new Color(.4f,.53f,.6f):id[0]=='B'?new Color(.6f,.3f,.12f):id[0]=='C'?new Color(.65f,.8f,.88f):Color.white;
  plan=room.gameObject.AddComponent<V3RoomRoutes>();plan.dimensions=new Vector2(w,h);
  boundary.points=new[]{new Vector2(0,-5),new Vector2(w,-5),new Vector2(w,h),new Vector2(0,h)};
  rig.OutputCamera?.GetComponent<Camera>();rig.GetComponent<Camera>().backgroundColor=tone*.2f;
  Block("Room left edge",-.5f,h/2,1,h+10);Block("Room right edge",w+.5f,h/2,1,h+10);Block("Room ceiling",w/2,h+.5f,w,1);
  Spawn("Entry",4,1.81f);Spawn("FromLeft",4,1.81f);Spawn("FromRight",w-4,1.81f);
  room.GetComponent<RoomLabel>().notice="V3 "+id;
 }
 static void Pits(params Vector2[] gaps)
 {
  float left=0;foreach(var gap in gaps){Floor(left,gap.x);left=gap.y;}Floor(left,width);plan.fallPits=gaps.Length;
  var kill=S3Authoring.Block(room,"KillZone","Fall reset",new Vector2(width/2,-4),new Vector2(width,1));
 }
 static void Optional(string type,float x,bool dash,bool wall,bool jump,A mode=A.Jump)
 {
  plan.optionalType=type;Platform("Optional approach",x,2.6f,3);Platform("Optional middle",x+3.5f,4.3f,2);Platform("Optional reward shelf",x+7,6,3);Reward("optional",x+7,6.8f);
  if(wall)Block("Optional marked side",x+2.25f,4, .5f,6,true);
  var prefix=plan.routes[0].points.Where(p=>p.position.x<x-2&&p.action!=A.Boss&&p.action!=A.Wall).ToArray();
  plan.routes=plan.routes.Concat(new[]{Route("optional","Discovery:V3-"+room.roomId+"-optional",dash,wall,jump,prefix.Concat(new[]{Point(x,1.81f),Point(x,3.41f,A.Jump),Point(x+3.5f,5.11f,mode),Point(x+7,6.81f,jump?A.DoubleJump:A.Jump)}).ToArray())}).ToArray();
 }
 static void Secret(float x,float runup=7)
 {
  // Raised side alcove leaves the mandatory ground route open below it.
  Platform("Secret low approach",x-6,3.5f,3);Platform("Secret wall approach",x-2,5,3);Platform("Secret alcove floor",x+4,5,8);
  Block("Secret roof",x+4,8.5f,8,1);Block("Secret back",x+8.25f,6.5f,.5f,3);Wall("V3-secret-"+room.roomId,x,6.5f,3,BreakableWall.Side.Any,true);Reward("secret",x+5,5.8f);
  var prefix=plan.routes[0].points.Take(plan.routes[0].points.Length-1).TakeWhile(p=>p.position.x<x-8).ToArray();
  var approach=new[]{Point(x-runup,1.81f),Point(x-6,4.31f,A.DoubleJump),Point(x-2,5.81f,A.Jump),Point(x-1.3f,5.81f,A.Wall,"V3-secret-"+room.roomId),Point(x+5,5.81f)};
  plan.routes=plan.routes.Concat(new[]{Route("secret","Discovery:V3-"+room.roomId+"-secret",true,true,true,prefix.Concat(approach).ToArray())}).ToArray();
 }
 public static void Run()
 {
  string id=N5Args.Value("-b07-room","A01");Create(id);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void Create(string id)
 {
  switch(id)
  {
   case "A01":
    Shell(id,72,16);Floor(0,72);Checkpoint("CP-A01",4,1.81f);Gate("봉인문",1,4,1,6);Platform("Broad first step",14,1.7f,6);Platform("Different low step",23,2.6f,5);
    Platform("G-J optional ledge",40,4.6f,6);Reward("optional",39,5.4f);Spawn("FromOptional",40,5.41f);Exit("A06",42,5.8f,"Entry",false);Exit("A02",70,2);
    plan.beats="봉인문·출발 / 낮은 단 / 넓은 착지 / 높은 재방문 턱 / 첫 갱도";plan.optionalType="더블 점프 높은 턱";
    plan.routes=new[]{Route("main","A02",false,false,false,Point(10,1.81f),Point(14,2.51f,A.Jump),Point(16.3f,2.51f),Point(23,3.41f,A.Jump),Point(30,1.81f),Point(70,1.81f)),Route("optional","Discovery:V3-A01-optional",false,false,true,Point(36,1.81f),Point(39,5.41f,A.DoubleJump))};break;
   case "A02":
    Shell(id,30,48);Floor(0,30);for(int i=0;i<6;i++)Platform("First ascent "+i,i%2==0?8:12,3+i*2,4);Platform("Mid-level safe deck",21,13,18);
    Block("G-W left",13.75f,30,.5f,30,true);Block("G-W right",18.25f,30,.5f,30,true);Platform("Summit landing",22,45,8);Block("Visible shaft light",16,46.5f,1,1).GetComponent<BoxCollider2D>().enabled=false;
    Exit("A01",2,2);Exit("A03",28,14);Spawn("FromA03",26,13.81f);Spawn("FromB05",22,45.81f);Exit("B05",25,46,"Entry");
    plan.beats="일반 점프 소개 / 12u 발판 / A03 분기 / 4u·30u 굴뚝 / 빛·B05";
    var a2=new List<P>{Point(8,3.81f,A.Jump)};for(int i=1;i<6;i++){a2.Add(Point(i%2==0?13.5f:6.5f,1.81f+i*2));a2.Add(Point(i%2==0?11.7f:8.3f,1.81f+i*2));a2.Add(Point(i%2==0?8:12,3.81f+i*2,A.Jump));}a2.Add(Point(28,13.81f));
    var revisit=Route("revisit","B05",true,true,false,Point(17.6f,13.81f),Point(17.6f,46,A.Climb),Point(22,45.81f),Point(25,45.81f));revisit.startSpawn="FromA03";plan.routes=new[]{Route("main","A03",false,false,false,a2.ToArray()),revisit};break;
   case "A03":
    Shell(id,60,18);Floor(0,12);Floor(18,60);Floor(0,60,-3);Platform("Hub checkpoint deck",29.25f,2.5f,10.5f);Checkpoint("CP-A03",26,3.31f);Platform("Tunnel return step",15,-1,3);Platform("Tunnel return lip",20,1,4);
    Exit("A02",2,2,"FromA03");Spawn("FromA02",4,1.81f);Exit("A04",58,2);Spawn("FromA05",7,-2.19f);Spawn("FromPit",30,12);Exit("A05",6,-2,"FromShortcut",false);Wall("V3-shortcut-A05",10,-1.5f,3,BreakableWall.Side.Left);
    Platform("G-D approach low",38,4.5f,4);Platform("G-D approach middle",41,6,3);Platform("G-D approach high",42,7.5f,3);Platform("G-D destination deck",49,7.5f,8);Block("G-D room separator",44,14.25f,1,8.5f);var gd=Block("G-D lattice",44,9,1,3);gd.AddComponent<DashGate>();Exit("B01",48,9);
    Sign(29,1.8f,"여기선 칼을 뽑지 않는다. 오래된 약속이지.");Sign(36,1.8f,"위로 간 사람들은 편지를 보내지 않아.");
    plan.beats="CP·바닥촌 / 표지판 / 격자 관찰 / 동쪽 A04 / 아래 숏컷";
    var gdRoute=Route("dash","B01",true,false,false,Point(34,3.31f),Point(38,5.31f,A.Jump),Point(41,6.81f,A.Jump),Point(42,8.31f,A.Jump),Point(48,8.31f,A.GroundDash));gdRoute.startSpawn="CP-A03";
    var back=Route("return","A02",true,true,false,Point(20,1.81f),Point(15,-.19f),Point(11,1.81f,A.Jump),Point(2,1.81f));back.startSpawn="FromPit";
    plan.routes=new[]{Route("main","A04",false,false,false,Point(11,1.81f),Point(15,-.19f),Point(20,1.81f,A.Jump),Point(22,1.81f),Point(30,3.31f,A.Jump),Point(58,1.81f,A.WalkUnder)),gdRoute,back};break;
   case "A04":
    Shell(id,90,18);Floor(0,90);Exit("A03",2,2);Exit("A05",88,2);Enemy("E1",34,1.51f);Enemy("E1",38,1.51f);Platform("Pogo reward landing",42,3.7f,4);Reward("optional",42,4.5f);
    plan.beats="첫 E1 소개 / 넓은 회피 / 포고 분기 / 금 간 벽 / 출구";plan.optionalType="적 위 포고 연속";
    plan.routes=new[]{Route("main","A05",false,false,false,Point(30,1.81f),Point(36,1.81f,A.Jump),Point(42,1.81f,A.Jump),Point(52,1.81f),Point(88,1.81f)),Route("optional","Discovery:V3-A04-optional",false,false,false,Point(30,1.81f),Point(34,3.5f,A.Pogo),Point(38,3.5f,A.Pogo),Point(42,4.51f))};Secret(67);break;
   case "A05":
    Shell(id,72,16);Floor(0,72);Exit("A04",2,2);Pickup<DashPickup>(24,1.8f);Spawn("FromShortcut",68,1.81f);Exit("A03",70,2,"FromA05",false);Block("Shortcut tunnel roof",64,4.5f,16,1);Wall("V3-shortcut-A05",65,2.5f,3,BreakableWall.Side.Left);
    var practice=Block("Practice lattice",42,2.5f,1,3);practice.AddComponent<DashGate>();Block("Practice roof",42,10,1,12);
    plan.beats="사당 도착 / Dash 획득 / 격자 연습 / 방향 벽 / A03 숏컷";plan.routes=new[]{Route("main","A03",false,false,false,Point(24,1.81f),Point(39,1.81f),Point(46,1.81f,A.GroundDash),Point(63.7f,1.81f),Point(63.7f,1.81f,A.Wall,"V3-shortcut-A05"),Point(70,1.81f))};Optional("대시 타이밍",8,true,false,false,A.Dash);break;
   case "A06":
    Shell(id,60,20);Floor(0,60);Exit("A01",2,2,"FromOptional",false);Sign(10,1.8f,"도시 아래에 첫 층이 있다.");Enemy("E1",27,1.51f);Platform("Double jump terrace",20,4.6f,6);Platform("Pogo high reward",34,6,4);Reward("optional",34,6.8f);
    plan.beats="무덤 입구 / 비문 / 더블 점프 턱 / 포고 / 귀환";plan.optionalType="더블 점프 + 포고";plan.routes=new[]{Route("main","A01",true,true,true,Point(10,1.81f),Point(2,1.81f)),Route("optional","Discovery:V3-A06-optional",true,true,true,Point(16,1.81f),Point(20,5.41f,A.DoubleJump),Point(27,3.5f,A.Pogo),Point(34,6.81f,A.DoubleJump))};break;
   case "B01":
    Shell(id,84,20);Floor(0,34);Floor(38,84);Floor(34,38,-3);Checkpoint("CP-B01",20,1.81f);Spawn("FromDash",4,1.81f);Spawn("FromB04",69,8.81f);Exit("B02",82,2);var pit=Exit("A03",36,-1,"FromPit",true,"Pit");pit.gameObject.AddComponent<V3ExitRule>().downwardOnly=true;
    Gate("닫힌 상층 관문",42,8.5f,6,7);for(int i=0;i<3;i++)Block("Waiting person "+i,38+i*3,6,.7f,1.5f).GetComponent<BoxCollider2D>().enabled=false;Platform("Gate forecourt",42,5,18);
    Block("Shortcut access marked wall",55.75f,5.5f,.5f,5,true);Platform("Shortcut first stair",60,3.5f,4);Platform("Shortcut middle stair",63,5,4);Platform("Shortcut upper stair",66,6.5f,4);
    Platform("Shortcut landing",72,8,12);Wall("V3-shortcut-B04",74,9.5f,3,BreakableWall.Side.Right);Exit("B04",80,10.5f,"FromShortcut",false);Sign(32,1.8f,"낙오자의 구덩이");
    Sign(23,1.8f,"관문은 곧 열린다. 다들 그렇게 말한 지 몇 해째야.");Sign(28,1.8f,"아래로 내려가는 놈들? 낙오자지.");Sign(49,1.8f,"위로 가는 길? 아래에서 빛을 봤다던 노인이 있었지.");
    plan.beats="중층 CP / 닫힌 관문·사람 / 낙오자 구덩이 / B02 / B04 숏컷";var pitRoute=Route("return","A03",true,true,false,Point(70,1.81f),Point(36,1.81f));pitRoute.startSpawn="FromB04";
    plan.routes=new[]{Route("main","B02",true,false,false,Point(33,1.81f),Point(40,1.81f,A.DashAir),Point(82,1.81f)),pitRoute};break;
   case "B02":
    Shell(id,84,18);Pits(new Vector2(52,55));Exit("B01",2,2);Exit("B03",82,2);Enemy("E2",30,1.51f);Enemy("E2",42,1.51f);Platform("Bypass approach",22,2.5f,5);Platform("Bypass middle",23,4,3);Platform("Charger bypass",36,5.5f,20);Reward("optional",34,1.8f);
    plan.beats="돌진 예고 / 회피 공간 / 3u 낙사 / 비밀 / 출구";plan.optionalType="대시로 돌진 틈";
    plan.routes=new[]{Route("main","B03",true,false,false,Point(24,1.81f),Point(28,6.31f,A.Jump),Point(44,6.31f),Point(51,1.81f),Point(57,1.81f,A.DashAir),Point(82,1.81f)),Route("optional","Discovery:V3-B02-optional",true,false,false,Point(18,1.81f),Point(22,3.31f,A.Jump),Point(26,1.81f,A.WalkUnder),Point(34,1.81f,A.DashAir))};Secret(70,11);break;
   case "B03":
    Shell(id,30,40);Floor(0,30);Exit("B02",2,2);Spawn("FromB02",4,1.81f);Spawn("FromB04",22,37.81f);Spawn("FromB06",22,19.81f);Exit("B04",22,38);Exit("B06",27,20,"Entry",false);
    var ascent=new List<P>();P[] shootingBranch=null;
    for(int i=0;i<20;i++){float x=i%2==0?8:16,top=3+1.5f*((i+1)/2)+2*(i/2);Platform("Cover ascent "+i,x,top,5);if(i>0){float prior=3+1.5f*(i/2)+2*((i-1)/2);ascent.Add(Point(i%2==0?17:7,prior+.81f));ascent.Add(Point(i%2==0?13.5f:10.5f,prior+.81f));}ascent.Add(Point(x,top+.81f,A.Jump));if(i==11)shootingBranch=ascent.ToArray();}
    Platform("Branch landing",21,19,6);Platform("Exit landing",22,37,6);
    for(int i=0;i<3;i++){Enemy("E3",25,9+i*12);Block("Shot cover "+i,20,i==1?20:9+i*12,1,i==1?2:3);Platform("Turret base "+i,25,8.5f+i*12,4);}
    Platform("Shot challenge approach",22,23.5f,4);Platform("Shot challenge reward",27,25,6);Reward("optional",27,25.8f);
    plan.beats="낮은 등반 / E3 엄폐 / B06 분기 / 두 번째 사격 / 위 출구";plan.optionalType="사격 피하며 오르기";
    ascent.Add(Point(18.5f,36.81f));ascent.Add(Point(22,37.81f,A.Jump));
    plan.routes=new[]{Route("main","B04",true,false,false,ascent.ToArray()),Route("optional","Discovery:V3-B03-optional",true,false,false,shootingBranch.Concat(new[]{Point(18.5f,22.81f),Point(22,24.31f,A.Jump),Point(27,25.81f,A.Jump)}).ToArray())};break;
   case "B04":
    Shell(id,40,30);Floor(0,40);Exit("B03",2,2,"FromB04");Pickup<WallJumpPickup>(10,1.8f);Block("Practice left",17.75f,10,.5f,14,true);Block("Practice right",22.25f,9,.5f,16,true);Platform("Chimney summit",31,17,18);Spawn("FromShortcut",36,17.81f);Exit("B01",38,18,"FromB04",false);Block("Shortcut roof",35,20.5f,10,1);Wall("V3-shortcut-B04",34,18.5f,3,BreakableWall.Side.Left);
    Block("Optional single marked wall",5.75f,6,.5f,6,true);Platform("Optional single wall summit",8,9,4);Reward("optional",8,9.8f);
    plan.beats="벽 점프 획득 / 4u 연습 / 상단 착지 / 방향 벽 / 시장 귀환";plan.optionalType="한쪽 벽 점프 연속";
    plan.routes=new[]{Route("main","B01",true,false,false,Point(10,1.81f),Point(21.6f,1.81f),Point(21.6f,18.6f,A.Climb),Point(28,17.81f),Point(32.7f,17.81f),Point(32.7f,17.81f,A.Wall,"V3-shortcut-B04"),Point(38,17.81f)),Route("optional","Discovery:V3-B04-optional",true,true,false,Point(5.1f,1.81f),Point(5.1f,10.6f,A.Climb),Point(8,9.81f))};break;
   case "B05":
    Shell(id,24,64);Floor(0,24);Exit("A02",2,2,"FromB05");Spawn("FromC01",20,61.81f);Exit("C01",22,62);Block("Lower left",7.75f,18.5f,.5f,31,true);Block("Lower right",12.25f,17.5f,.5f,33,true);Platform("16u rest",9,17,2);Platform("32u rest",9,33,2);
    Pogo(16,34.7f);Platform("Pogo onward landing",20,36,4);Block("Upper left",17.75f,51.5f,.5f,25,true);Block("Upper right",22.25f,50,.5f,27,true);Platform("48u rest",19,49,2);Platform("Summit rest",19,61,2);Pogo(4,34.7f);Platform("Optional pogo return",3,36.5f,4);Reward("optional",3,37.3f);
    Block("Closed gate reverse window",4,25,4,6).GetComponent<BoxCollider2D>().enabled=false;Sign(5,24,"관문 너머의 사람들은 여전히 위를 보고 있다.");
    plan.beats="4u 벽 등반 / 16u 쉼 / 창·32u 쉼 / 대시·포고 연결 / 상층 도착";plan.optionalType="벽 점프 + 포고";
    var first=new[]{Point(11.6f,1.81f),Point(11.6f,19.2f,A.Climb),Point(9,17.81f,A.WalkUnder),Point(11.6f,35.2f,A.Climb),Point(9,33.81f,A.WalkUnder)};
    plan.routes=new[]{Route("main","C01",true,true,false,first.Concat(new[]{Point(14,36,A.DashAir),Point(16,36,A.Pogo),Point(20,36.81f,A.Jump),Point(21.6f,50.6f,A.Climb),Point(19,49.81f,A.WalkUnder),Point(21.6f,62.6f,A.Climb),Point(20,61.81f,A.WalkUnder),Point(22,61.81f)}).ToArray()),Route("optional","Discovery:V3-B05-optional",true,true,false,first.Concat(new[]{Point(6,36,A.DashAir),Point(4,36,A.Pogo),Point(3,37.31f,A.Jump)}).ToArray())};break;
   case "B06":
    Shell(id,60,20);Floor(0,60);Exit("B03",2,2,"FromB06",false);Enemy("E1",38,1.51f);Platform("Steam low step",18,2.4f,6);Block("Marked steam stack",27,5,.5f,8,true);
    Platform("Wall dash launch",28.75f,9,3);Platform("Wall dash landing",38,9,3);Reward("optional",38,9.8f);
    plan.beats="분기 / 낮은 증기 단 / 벽·대시 / E1 / 귀환";plan.optionalType="벽 점프 + 대시";
    plan.routes=new[]{Route("main","B03",true,false,false,Point(16,1.81f),Point(2,1.81f)),Route("optional","Discovery:V3-B06-optional",true,true,false,Point(14,1.81f),Point(18,3.21f,A.Jump),Point(26.35f,1.81f),Point(26.35f,10.6f,A.Climb),Point(28.8f,9.81f,A.WalkUnder),Point(30.2f,9.81f),Point(38,9.81f,A.DashAir))};break;
   case "C01":
    Shell(id,60,18);Floor(0,60);Checkpoint("CP-C01",12,1.81f);Exit("B05",2,2,"FromC01");Exit("C02",58,2);Sign(21,1.8f,"여기까지 와서 멈춘 이유? 위를 본 적이 있거든.");Sign(32,1.8f,"정상에 가면 뭘 할 거냐고? …글쎄.");Platform("Quiet waiting terrace",42,2,10);
    plan.beats="상층 도착 / CP / 두 표지판 / 전망 / 외벽 출구";plan.routes=new[]{Route("main","C02",true,true,false,Point(32,1.81f),Point(42,2.81f,A.Jump),Point(58,1.81f))};break;
   case "C02":
    Shell(id,96,20);Pits(new Vector2(28,33.6f),new Vector2(52,57.6f));Exit("C01",2,2);Exit("C03",94,2);Enemy("E3",72,1.51f);Block("Shot cover",68,2,1,2);
    for(int i=0;i<7;i++){var city=Block("Distant city "+i,8+i*13,8,5,3+i%3);city.GetComponent<BoxCollider2D>().enabled=false;city.GetComponent<SpriteRenderer>().sortingOrder=-10;city.GetComponent<SpriteRenderer>().color=tone*.35f;}
    plan.beats="도시 전망 / 첫 5.6u 틈 / 휴식 / 두 번째 틈 / 엄폐·비밀";plan.routes=new[]{Route("main","C03",true,true,false,Point(27,1.81f),Point(35,1.81f,A.Dash),Point(51,1.81f),Point(59,1.81f,A.Dash),Point(65,1.81f),Point(72,3.81f,A.Jump),Point(94,1.81f))};Platform("Turret bypass",72,3,12);Optional("대시 + 낙사",77,true,true,false,A.Dash);Secret(84);break;
   case "C03":
    Shell(id,72,20);Pits(new Vector2(34,37));Exit("C02",2,2);Spawn("FromC05",8,8.81f);Spawn("FromC04",68,5.41f);Block("C05 marked shaft",5.75f,5,.5f,8,true);Platform("C05 landing",9,8,6);Exit("C05",9,9,"Entry");Platform("G-J ledge",67,4.6f,10);Exit("C04",70,5.8f);
    Enemy("E1",24,1.51f);Enemy("E2",43,1.51f);Enemy("E3",52,1.51f);Platform("Combined combat bypass",48,3,16);Block("E3 cover",56,2,1,2);Platform("Pogo reward",30,4.5f,3);Reward("optional",30,6);
    plan.beats="혼합 전투 / C05 표시 벽 / 포고 분기 / 3u 틈 / G-J 재방문";plan.optionalType="적 위 포고 연속";
    var dj=Route("revisit","C04",true,true,true,Point(14,1.81f),Point(32,1.81f),Point(40,1.81f,A.Dash),Point(44,3.81f,A.Jump),Point(54,3.81f),Point(59,1.81f),Point(67,5.41f,A.DoubleJump),Point(70,5.41f));dj.startSpawn="FromC05";
    plan.routes=new[]{Route("main","C05",true,true,false,Point(6.4f,1.81f),Point(6.4f,9.6f,A.Climb),Point(9,8.81f)),dj,Route("optional","Discovery:V3-C03-optional",true,true,false,Point(20,1.81f),Point(24,3.5f,A.Pogo),Point(30,5.31f,A.Jump))};break;
   case "C05":
    Shell(id,48,30);Pits(new Vector2(29,32));Exit("C03",2,2,"FromC05");Pickup<DoubleJumpPickup>(12,1.8f);Platform("Double jump practice",22,4.6f,8);Platform("Far summit silhouette",40,20,6);Block("Distant summit",40,23,5,5).GetComponent<BoxCollider2D>().enabled=false;
    plan.beats="더블 점프 획득 / 높은 턱 / 틈 연습 / 정상 전망 / C03 귀환";plan.routes=new[]{Route("main","C03",true,true,false,Point(12,1.81f),Point(22,5.41f,A.DoubleJump),Point(16,1.81f),Point(2,1.81f))};Optional("전 능력 조합",34,true,true,true,A.DoubleJump);break;
   case "C04":
    Shell(id,54,18);Floor(0,54);Exit("C03",2,2,"FromC04");Exit("C06",52,2,"FromLeft");Checkpoint("CP-C04",48,1.81f);Platform("Last broad stair",23,2,12);Platform("Last rest",36,1.5f,8);
    plan.beats="마지막 계단 / 넓은 휴식 / CP-C04 / 보스 문";plan.routes=new[]{Route("main","C06",true,true,true,Point(23,2.81f,A.Jump),Point(36,2.31f),Point(48,1.81f),Point(52,1.81f))};break;
   case "C06":
    EditorSceneManager.OpenScene("Assets/Scenes/C06.unity");room=Object.FindFirstObjectByType<RoomDefinition>();foreach(var exit in room.GetComponentsInChildren<RoomExit>())exit.destinationScenePath=V3SceneMap.Path(exit.destinationRoomId);B07Parts.AttachBoss(room);
    plan=room.gameObject.AddComponent<V3RoomRoutes>();plan.dimensions=new Vector2(20,16);plan.beats="기존 진입 잠금 / 기존 보스 / 위 문 개방 / 정상 출구";plan.routes=new[]{Route("main","S01",true,true,true,Point(15.7f,1.81f,A.Boss),Point(6,3.81f,A.Jump),Point(10,5.81f,A.Jump),Point(6,7.81f,A.Jump),Point(10,9.81f,A.Jump))};plan.enemyCount=1;break;
   case "S01":
    Shell(id,40,42);Floor(0,40);Exit("C06",2,2,"FromLeft",false);Platform("Quiet first landing",9,3,6);Platform("Double jump ledge",16,6.6f,4);Block("Quiet marked wall",20.25f,14,.5f,12,true);Platform("Wall summit rest",25,20,6);Pogo(31,20.5f);Platform("Pogo onward",34,23,4);
    var quiet=new List<P>{Point(9,3.81f,A.Jump),Point(16,7.41f,A.DoubleJump),Point(19.6f,21.6f,A.Climb),Point(25,20.81f),Point(29,23,A.DashAir),Point(31,22,A.Pogo),Point(34,23.81f,A.DoubleJump)};
    for(int i=0;i<6;i++){float x=i%2==0?29:33,top=25+i*2;Platform("Quiet stair "+i,x,top,5);quiet.Add(Point(x,top+.81f,A.Jump));}Platform("Empty summit plaza",20,39,40);quiet.Add(Point(28,39.81f,A.DoubleJump));quiet.Add(Point(20,39.81f));
    var end=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V3/SummitEnding.prefab"));end.transform.SetParent(room.transform);end.transform.position=new Vector2(20,40);end.GetComponent<BoxCollider2D>().size=new Vector2(2,2);
    plan.beats="조용한 점프 / 전 능력 등반 / 마지막 단 / 빈 광장 / 중앙 엔딩";plan.routes=new[]{Route("main","Ending",true,true,true,quiet.ToArray())};break;
   default:throw new ArgumentException(id);
  }
  EditorSceneManager.SaveScene(SceneManager.GetActiveScene(),V3SceneMap.Path(id));
  Directory.CreateDirectory("Validation/_raw/B07");File.WriteAllText("Validation/_raw/B07/"+id+"-author.txt","MEASURE room="+id+" dimensions="+plan.dimensions+" pits="+plan.fallPits+" enemies="+plan.enemyCount+" beats="+plan.beats+" optional="+plan.optionalType+"\nSaved candidate Scene. Runtime acceptance not yet claimed.\n");
 }
}
