using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;
using J=B09Json;
public static class B09Authoring
{
 static RoomDefinition room;static MechanicsTuning tuning;static readonly List<string> rows=new();static readonly List<GuideGraph.Edge> edges=new();
 static Sprite sprite=>AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
 static void Init(){Directory.CreateDirectory("Assets/Scenes/V4");Directory.CreateDirectory("Assets/Prefabs/V4");Directory.CreateDirectory("Validation/_raw/B09");AssetDatabase.Refresh();const string path="Assets/ScriptableObjects/MechanicsTuning.asset";tuning=AssetDatabase.LoadAssetAtPath<MechanicsTuning>(path);if(tuning==null){tuning=ScriptableObject.CreateInstance<MechanicsTuning>();AssetDatabase.CreateAsset(tuning,path);}PlayerVariant(); }
 static void PlayerVariant(){const string path="Assets/Prefabs/V4/Player.prefab";if(File.Exists(path))return;var root=PrefabUtility.LoadPrefabContents("Assets/Prefabs/Player.prefab");root.GetComponent<BoxCollider2D>().offset=Vector2.up*.8f;var renderer=root.GetComponent<SpriteRenderer>();var source=renderer.sprite;var foot=Sprite.Create(source.texture,source.rect,new Vector2(.5f,0),source.pixelsPerUnit,0,SpriteMeshType.FullRect);foot.name="V4 foot origin";AssetDatabase.CreateAsset(foot,"Assets/Prefabs/V4/PlayerFootSprite.asset");renderer.sprite=foot;PrefabUtility.SaveAsPrefabAsset(root,path);PrefabUtility.UnloadPrefabContents(root);}
 static GameObject Rect(string name,float x,float y,float w,float h,Color color,bool trigger=false){var go=new GameObject(name);go.transform.SetParent(room.transform);go.transform.position=new Vector2(x+w/2,y+h/2);go.transform.localScale=new Vector3(w,h,1);go.layer=trigger?0:6;go.AddComponent<SpriteRenderer>().sprite=sprite;go.GetComponent<SpriteRenderer>().color=color;go.AddComponent<BoxCollider2D>().isTrigger=trigger;return go;}
 static GameObject Rect(string name,List<object> rect,Color color,bool trigger=false)=>Rect(name,J.F(rect[0]),J.F(rect[1]),J.F(rect[2]),J.F(rect[3]),color,trigger);
 static void Shell(string id,float w,float h)
 {
  EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");room=Object.FindFirstObjectByType<RoomDefinition>();var rig=Object.FindFirstObjectByType<RoomCameraRig>();var boundary=rig.boundary;
  foreach(var child in room.transform.Cast<Transform>().ToArray())if(child!=boundary.transform&&!rig.transform.IsChildOf(child))Object.DestroyImmediate(child.gameObject);
  foreach(var c in Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).ToArray())if(c!=boundary&&!c.transform.IsChildOf(room.transform))Object.DestroyImmediate(c.gameObject);
  room.roomId=id;room.name=id+" V4";room.initialSpawnId="Entry";room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V4/Player.prefab");room.GetComponent<RoomLabel>().notice="V4 "+id;
  boundary.points=new[]{Vector2.zero,new Vector2(w,0),new Vector2(w,h),new Vector2(0,h)};var v4=room.gameObject.AddComponent<V4Room>();v4.tuning=tuning;v4.dimensions=new Vector2(w,h);
 }
 static RoomSpawn Spawn(string id,float x,float y)=>S3Authoring.Spawn(room,id,new Vector2(x,y));
 static void Mark(Dictionary<string,object> mark,int index)
 {
  float x=J.F(mark,"x"),y=J.F(mark,"y");string text=J.S(mark,"t");string pickup=text.Contains("대시 사당")?"Dash":text.Contains("벽 점프 사당")?"Wall":text.Contains("더블 점프 사당")?"Double":"";
  if(pickup!=""){var go=Rect(pickup+" pickup",x-.4f,y-.4f,.8f,.8f,Color.green,true);if(pickup=="Dash")go.AddComponent<DashPickup>();else if(pickup=="Wall")go.AddComponent<WallJumpPickup>();else go.AddComponent<DoubleJumpPickup>();return;}
  if(text.Contains("CP")){string id="CP-"+room.roomId;var cp=Rect(id,x-.4f,y-.8f,.8f,1.6f,Color.yellow,true).AddComponent<Checkpoint>();cp.checkpointId=id;Physics2D.SyncTransforms();var surface=Physics2D.Raycast(new Vector2(x,y),Vector2.down,100,1<<6);cp.spawn=Spawn(id,x,surface.collider==null?y:surface.point.y+.01f);return;}
  if(text.Contains("조각")){var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/DiscoveryReward.prefab"));go.transform.SetParent(room.transform);go.transform.position=new Vector2(x,y);go.GetComponent<DiscoveryReward>().candidateId="V4-"+room.roomId+"-"+index;return;}
  if(text.Contains("엔딩 트리거")){Rect("Summit ending",x-1,y-1,2,2,Color.white,true).AddComponent<V3SummitEnding>();return;}
  var sign=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/SignPost.prefab"));sign.transform.SetParent(room.transform);sign.transform.position=new Vector2(x,y);sign.GetComponent<SignPost>().message=text;
  rows.Add("MARK "+room.roomId+" "+index+" sign-only: "+text);
 }
 [MenuItem("Afterglow/Build V4 From Design")]
 public static void Generate()
 {
  Init();rows.Clear();edges.Clear();File.Copy("Design/rooms_v2.json","Assets/Resources/V4Design.json",true);var data=J.D(J.Read(File.ReadAllText("Design/rooms_v2.json"))["rooms"]);var world=J.D(J.Read(File.ReadAllText("Design/world_v7.json"))["rooms"]);
  foreach(var pair in data)
  {
   var d=J.D(pair.Value);var size=J.A(d,"size");Shell(pair.Key,J.F(size[0]),J.F(size[1]));room.GetComponent<V4Room>().designJson=JsonFragment(pair.Value);
   var worldRect=J.A(J.D(world[pair.Key]),"rect");rows.Add("WORLD "+pair.Key+" rect="+string.Join(",",worldRect));
   var blocks=new Dictionary<string,GameObject[]>();
   foreach(string kind in new[]{"solid","platform","climb","seal","cover","breakable","hazard","pad"})
   {
    var list=J.A(d,kind);var made=new List<GameObject>();for(int i=0;i<list.Count;i++)
    {
     var go=Rect(kind+i,(List<object>)list[i],kind=="climb"?Color.cyan:kind=="seal"?new Color(.65f,.4f,.9f):kind=="hazard"?Color.red:kind=="pad"?Color.magenta:new Color(.4f,.5f,.6f),kind=="hazard");made.Add(go);
     if(kind=="climb")go.AddComponent<ClimbableWall>();if(kind=="seal")go.AddComponent<SealFloor>().sealId="V4-"+pair.Key+"-seal"+i;
     if(kind=="cover"||kind=="breakable"){var wall=go.AddComponent<BreakableWall>();wall.wallId="V4-"+pair.Key+"-"+kind+i;wall.hitsToBreak=1;}
     if(kind=="hazard")go.AddComponent<DamageBlock>();if(kind=="pad")go.AddComponent<SlamPad>().tuning=tuning;
    }blocks[kind]=made.ToArray();rows.Add("COUNT "+pair.Key+" "+kind+" json="+list.Count+" scene="+made.Count);
   }
   foreach(var index in J.A(d,"crumble"))blocks["platform"][(int)J.F(index)].AddComponent<CrumblePlatform>().tuning=tuning;
   foreach(var node in J.A(d,"pogo")){var p=J.D(node);bool spike=J.S(p,"kind")=="spike";var go=Rect("pogo:"+J.S(p,"id"),J.F(p,"x")-.5f,J.F(p,"y"),1,.5f,spike?Color.red:Color.yellow,true);go.AddComponent<PogoTarget>();if(spike)go.AddComponent<DamageBlock>();}
   foreach(var node in J.A(d,"steam")){var p=J.D(node);Rect("steam:"+J.S(p,"id"),J.F(p,"x0"),J.F(p,"y0"),J.F(p,"x1")-J.F(p,"x0"),J.F(p,"top")-J.F(p,"y0"),Color.white,true).AddComponent<SteamVent>().tuning=tuning;}
   foreach(var node in J.A(d,"lifts")){var p=J.D(node);float x0=J.F(p,"x0"),x1=J.F(p,"x1"),lo=J.F(p,"lo"),hi=J.F(p,"hi");var lift=Rect("lift:"+J.S(p,"id"),x0,hi-.3f,x1-x0,.3f,Color.yellow).AddComponent<LeverLift>();lift.low=lo;lift.high=hi;lift.tuning=tuning;lift.liftId="V4-"+pair.Key+"-"+J.S(p,"id");var lever=J.A(p,"lever");foreach(bool upper in new[]{true,false}){var go=Rect("Lift lever "+upper,J.F(lever[0])-.3f,(upper?J.F(lever[1]):lo)+.4f,.6f,1,Color.yellow,true);var call=go.AddComponent<LiftLever>();call.lift=lift;call.upper=upper;}}
   foreach(var node in J.A(d,"enemies")){var p=J.D(node);string type=J.S(p,"type","E4");float x=J.F(p,"x"),y=J.F(p,"y");if(type.StartsWith("BOSS"))continue;if(type.StartsWith("E4")||p.ContainsKey("id")){Rect("enemy:"+J.S(p,"id"),x-.5f,y,1,1,Color.green,true).AddComponent<E4Hover>().tuning=tuning;}else{string prefabType=type.StartsWith("E3")?"E3":type.StartsWith("E2")?"E2":"E1";var go=S3Authoring.Enemy(prefabType,new Vector2(x,y+.51f));go.transform.SetParent(room.transform);if(prefabType=="E3"){var reset=go.GetComponent<EnemyReset>();reset.overrideTurretDirection=true;reset.turretDirection=type.Contains("→")?1:-1;}}}
   var spawnList=J.A(d,"spawns");foreach(var node in spawnList){var p=J.D(node);Spawn(J.S(p,"id"),J.F(p,"x"),J.F(p,"y"));}var first=J.D(spawnList.First());var start=spawnList.Select(J.D).FirstOrDefault(s=>J.S(s,"id")=="start")??first;Spawn("Entry",J.F(start,"x"),J.F(start,"y"));if(pair.Key=="A01")Spawn("CP-A01",J.F(start,"x"),J.F(start,"y"));
   foreach(var node in J.A(d,"doors"))
   {
    var p=J.D(node);var r=J.A(p,"rect");string target=J.S(p,"to").Split('(')[0],id=J.S(p,"id");var go=Rect("door:"+id,r,Color.white,true);var exit=go.AddComponent<RoomExit>();exit.exitId=id;exit.destinationRoomId=target;exit.destinationScenePath=V4SceneMap.Path(target);exit.destinationSpawnId=id;exit.mainPath=target!="S02"&&target!="Z01";var rule=go.AddComponent<V4ExitRule>();rule.gate=J.S(p,"gate");if(pair.Key=="Z01"&&id=="Z01-S02")rule.elevatorSeal=blocks["seal"].Last().GetComponent<SealFloor>();if(pair.Key=="S02")exit.enabled=false;edges.Add(new GuideGraph.Edge{from=pair.Key,to=target,exitId=id,position=go.transform.position});
    bool hasArrival=J.A(J.D(data[target]),"spawns").Select(J.D).Any(s=>J.S(s,"id")==id);if(!hasArrival){exit.enabled=false;edges.RemoveAt(edges.Count-1);rows.Add("ONEWAY inactive reverse marker "+pair.Key+" "+id);}
    if(exit.mainPath&&hasArrival&&pair.Key!="S02"){var lamp=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/B06/GuideLantern.prefab"));lamp.transform.SetParent(go.transform,false);lamp.transform.localPosition=Vector3.up*.8f;lamp.transform.localScale=new Vector3(.28f/J.F(r[2]),.36f/J.F(r[3]),1);}
   }
   int markIndex=0;foreach(var mark in J.A(d,"marks"))Mark(J.D(mark),markIndex++);
   foreach(string kind in new[]{"doors","spawns","marks","enemies","pogo","lifts","steam","crumble"})rows.Add("COUNT "+pair.Key+" "+kind+" json="+J.A(d,kind).Count+" scene="+J.A(d,kind).Count);
   if(pair.Key=="C06")Boss(blocks["solid"]);
   SaveDeterministic(V4SceneMap.Path(pair.Key));
  }
  const string graphPath="Assets/Resources/GuideGraph_V4.asset";var graph=AssetDatabase.LoadAssetAtPath<GuideGraph>(graphPath);if(graph==null){graph=ScriptableObject.CreateInstance<GuideGraph>();AssetDatabase.CreateAsset(graph,graphPath);}graph.edges=edges.ToArray();EditorUtility.SetDirty(graph);AssetDatabase.SaveAssets();File.WriteAllLines("Validation/_raw/B09/T5-elements-"+DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff")+".txt",rows);
 }
 static string JsonFragment(object obj){return "Design/rooms_v2.json";}
 static void SaveDeterministic(string path)
 {
  var scene=room.gameObject.scene;
  foreach(var root in scene.GetRootGameObjects())if(PrefabUtility.IsPartOfPrefabInstance(root))PrefabUtility.UnpackPrefabInstance(root,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
  foreach(var transform in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)))if(PrefabUtility.IsAnyPrefabInstanceRoot(transform.gameObject))PrefabUtility.UnpackPrefabInstance(transform.gameObject,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
  EditorSceneManager.SaveScene(scene,path);
  var ids=new Dictionary<long,long>();long next=10000;
  foreach(var transform in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true))){foreach(var obj in new Object[]{transform.gameObject}.Concat(transform.GetComponents<Component>().Cast<Object>())){long old=(long)GlobalObjectId.GetGlobalObjectIdSlow(obj).targetObjectId;if(old>4&&!ids.ContainsKey(old))ids[old]=next++;}}
  string text=File.ReadAllText(path);text=System.Text.RegularExpressions.Regex.Replace(text,@"(?<=fileID: )-?\d+(?!\d)(?!,\s*guid:)",m=>ids.TryGetValue(long.Parse(m.Value),out var id)?id.ToString():m.Value);
  text=System.Text.RegularExpressions.Regex.Replace(text,@"(?<=&)-?\d+",m=>ids.TryGetValue(long.Parse(m.Value),out var id)?id.ToString():m.Value);
  var parts=System.Text.RegularExpressions.Regex.Split(text,@"(?m)(?=^--- !u!)");File.WriteAllText(path,parts[0]+string.Concat(parts.Skip(1).OrderBy(p=>long.Parse(System.Text.RegularExpressions.Regex.Match(p,@"&(-?\d+)").Groups[1].Value))),new System.Text.UTF8Encoding(false));
 }
 static void Boss(GameObject[] solids)
 {
  var source=EditorSceneManager.OpenScene("Assets/Scenes/C06.unity",OpenSceneMode.Additive);var original=source.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<BossEncounter>()).Single();var go=Object.Instantiate(original.gameObject);SceneManager.MoveGameObjectToScene(go,room.gameObject.scene);go.transform.SetParent(room.transform);var boss=go.GetComponent<BossEncounter>();var spec=J.A(J.D(J.D(J.Read(File.ReadAllText("Design/rooms_v2.json"))["rooms"])["C06"]),"enemies").Select(J.D).First(e=>J.S(e,"type").StartsWith("BOSS"));var delta=new Vector3(J.F(spec,"x"),J.F(spec,"y")+boss.GetComponent<Collider2D>().bounds.extents.y,0)-go.transform.position;go.transform.position+=delta;var flow=go.GetComponent<EndingFlow>();if(flow!=null)Object.DestroyImmediate(flow);boss.warning=Object.Instantiate(original.warning,room.transform);boss.warning.transform.position+=delta;boss.wavePrefab=original.wavePrefab;boss.leftExit=room.GetComponentsInChildren<RoomExit>().First(e=>e.destinationRoomId=="C04");EditorSceneManager.CloseScene(source,true);var reward=room.gameObject.AddComponent<V4BossReward>();reward.boss=boss;reward.upperExit=room.GetComponentsInChildren<RoomExit>().First(e=>e.destinationRoomId=="S01");reward.upperExit.enabled=false;reward.roof=solids.Where(s=>s.GetComponent<Collider2D>().bounds.min.y>=15).ToArray();
 }
 [MenuItem("Afterglow/Open Mechanics Test")]
 public static void OpenTest(){if(!File.Exists("Assets/Scenes/Test/MechanicsTest.unity"))TestCourse();EditorSceneManager.OpenScene("Assets/Scenes/Test/MechanicsTest.unity");}
 public static void TestCourse()
 {
  Init();Shell("MechanicsTest",100,20);room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");room.GetComponent<V4Room>().testCourse=true;Spawn("Entry",4,1.81f);Rect("floor",0,0,100,1,Color.gray);Rect("Slam pad",12,1,2,.3f,Color.magenta).AddComponent<SlamPad>().tuning=tuning;Rect("Seal",21,1,3,.3f,Color.magenta).AddComponent<SealFloor>().sealId="test-seal";
  Rect("Lantern",30,2,1,.5f,Color.yellow,true).AddComponent<PogoTarget>();var spike=Rect("Spike pogo",36,1,1,.5f,Color.red,true);spike.AddComponent<PogoTarget>();spike.AddComponent<DamageBlock>();Rect("E4",43,2,1,1,Color.green,true).AddComponent<E4Hover>().tuning=tuning;
  Rect("Steam",51,1,3,9,Color.white,true).AddComponent<SteamVent>().tuning=tuning;var lift=Rect("Lift",63,8,2.6f,.3f,Color.yellow).AddComponent<LeverLift>();lift.low=1;lift.high=8.3f;lift.tuning=tuning;lift.liftId="test-lift";foreach(bool upper in new[]{true,false}){var lever=Rect("Lever "+upper,61,upper?9:1.5f,.6f,1,Color.yellow,true).AddComponent<LiftLever>();lever.lift=lift;lever.upper=upper;}Rect("Crumble",75,2,5,.5f,Color.yellow).AddComponent<CrumblePlatform>().tuning=tuning;
  Rect("Climb left",86,1,1,14,Color.cyan).AddComponent<ClimbableWall>();Rect("Climb right",91,1,1,14,Color.cyan).AddComponent<ClimbableWall>();
  room.gameObject.AddComponent<MechanicsTestPresets>().baseline=tuning;
  Mark(new Dictionary<string,object>{{"x",4.0},{"y",1.8},{"t","아래+C: 내려찍기 / 아래+X: 포고 / F8: 개발 능력 부여 / 장치 순서: pad,seal,lantern,spike,E4,steam,lift,crumble"}},0);
  SaveDeterministic("Assets/Scenes/Test/MechanicsTest.unity");AssetDatabase.SaveAssets();
 }
 public static void Run(){Generate();TestCourse();EditorApplication.Exit(0);}
}



