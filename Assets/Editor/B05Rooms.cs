using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public static class B05Rooms
{
 public static GameObject Block(Transform parent,string type,string name,Vector2 pos,Vector2 size){var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V2/"+type+"_V2.prefab"));go.name=name;go.transform.SetParent(parent);go.transform.position=pos;go.transform.localScale=new Vector3(size.x,size.y,1);return go;}
 public static void Run(){for(int n=1;n<=6;n++)Create("A"+n.ToString("D2"),n==3?72:n==4?90:n==5?72:60);AssetDatabase.SaveAssets();EditorApplication.Exit(0);}
 public static void A05Variety(){EditorSceneManager.OpenScene("Assets/Scenes/V2/A05_V2.unity");var t=GameObject.Find("V2 optional precision 1").transform;t.position=new Vector2(t.position.x,4.85f);EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorApplication.Exit(0);}
 public static void Revise(){for(int n=1;n<=6;n++){
  string id="A"+n.ToString("D2");EditorSceneManager.OpenScene("Assets/Scenes/V2/"+id+"_V2.unity");
  foreach(var c in UnityEngine.Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).ToArray())if(c!=null&&!c.isTrigger&&c.GetComponent<DashGate>()==null&&!c.name.StartsWith("V2 "))UnityEngine.Object.DestroyImmediate(c.gameObject);
  if(n==3){UnityEngine.Object.DestroyImmediate(GameObject.Find("V2 gate approach"));UnityEngine.Object.DestroyImmediate(GameObject.Find("V2 gate shelf"));UnityEngine.Object.DestroyImmediate(GameObject.Find("V2 gate second step"));var parent=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>().transform;Block(parent,"Landing","V2 gate approach",new Vector2(44,2.95f),new Vector2(4,.5f));Block(parent,"Landing","V2 gate second step",new Vector2(51.3f,4.75f),new Vector2(8.6f,.5f));Block(parent,"Landing","V2 gate shelf",new Vector2(64,4.5f),new Vector2(16,1));}
  float middle=new[]{4.5f,5.2f,4.9f,4.7f,5.0f,4.8f}[n-1];
  for(int k=0;k<3;k++){var t=GameObject.Find("V2 optional precision "+k).transform;var p=t.position;p.y=k==0?2.95f:k==1?middle-.25f:6.55f;t.position=p;}
  var reward=UnityEngine.Object.FindFirstObjectByType<DiscoveryReward>();reward.transform.position=new Vector2(reward.transform.position.x,7.6f);
  if(n==4){GameObject.Find("V2 combat bypass").transform.position=new Vector2(58,2.5f);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();foreach(var e in room.GetComponentsInChildren<RoomExit>())if(e.destinationRoomId=="A06")e.transform.position=new Vector2(2,8.8f);if(GameObject.Find("V2 A06 branch entry")==null){Block(room.transform,"Landing","V2 A06 branch entry",new Vector2(6,2.95f),new Vector2(2,.5f));Block(room.transform,"Landing","V2 A06 branch middle",new Vector2(4,4.65f),new Vector2(1.5f,.5f));Block(room.transform,"Landing","V2 A06 branch door landing",new Vector2(2,6.55f),new Vector2(2,.5f));}}
  foreach(var sr in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))if(sr.name.StartsWith("V2 support landmark"))sr.sprite=B05Art.Tile(89);
  EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
 }EditorApplication.Exit(0);}
 public static void Create(string id,float width)
 {
  string target="Assets/Scenes/V2/"+id+"_V2.unity";if(File.Exists(target))throw new IOException("Preserve "+target);
  var scene=EditorSceneManager.OpenScene("Assets/Scenes/"+id+".unity");EditorSceneManager.SaveScene(scene,target);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();room.playerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V2/Player_V2.prefab");
  // Preserve every authored spawn and exit identifier, replace geometry only in copied Scene.
  foreach(var t in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).ToArray()){
   if(t==null||t==room.transform)continue;
   var c=t.GetComponent<Collider2D>();if(c==null)continue;
   if(t.GetComponent<RoomExit>()!=null||t.GetComponent<Checkpoint>()!=null||t.GetComponent<DashPickup>()!=null||t.GetComponent<DashGate>()!=null||t.GetComponent<RoomSpawn>()!=null)continue;
   if(t.GetComponent<DiscoveryReward>()!=null||t.GetComponent<KillZone>()!=null||t.GetComponent<PogoTarget>()!=null||c.gameObject.layer==LayerMask.NameToLayer("Ground"))UnityEngine.Object.DestroyImmediate(t.gameObject);
  }
  foreach(var d in UnityEngine.Object.FindObjectsByType<DiscoveryReward>(FindObjectsSortMode.None))UnityEngine.Object.DestroyImmediate(d.gameObject);
  foreach(var reset in UnityEngine.Object.FindObjectsByType<EnemyReset>(FindObjectsSortMode.None))UnityEngine.Object.DestroyImmediate(reset.gameObject);
  foreach(var spawn in room.GetComponentsInChildren<RoomSpawn>()){var p=spawn.transform.position;p.x=p.x<5?3.5f:p.x>31?width-3.5f:p.x/36*width;spawn.transform.position=p;}
  foreach(var cp in room.GetComponentsInChildren<Checkpoint>()){cp.transform.position=new Vector2(id=="A01"?4:width*.5f,1.81f);cp.spawn.transform.position=cp.transform.position;}
  foreach(var exit in room.GetComponentsInChildren<RoomExit>()){var p=exit.transform.position;p.x=p.x<5?1.75f:p.x>31?width-1.75f:p.x/36*width;exit.transform.position=p;if(exit.destinationRoomId.StartsWith("A"))exit.destinationScenePath="Assets/Scenes/V2/"+exit.destinationRoomId+"_V2.unity";}
  foreach(var pickup in room.GetComponentsInChildren<DashPickup>())pickup.transform.position=new Vector2(width*.65f,1.8f);
  foreach(var gate in room.GetComponentsInChildren<DashGate>()){gate.transform.position=new Vector2(width*.78f,10.5f);gate.transform.localScale=new Vector3(1,11,1);}
  var ground=Block(room.transform,"Landing","V2 uninterrupted safe floor",new Vector2(width/2,0),new Vector2(width,2));
  // Tilemap persisted successfully: use its pixels for the full floor, prefab supplies collision only.
  foreach(var s in ground.GetComponentsInChildren<SpriteRenderer>())s.enabled=false;
  var grid=new GameObject("V2 tile grid",typeof(Grid));grid.transform.SetParent(room.transform);var tiles=new GameObject("Floor tiles",typeof(Tilemap),typeof(TilemapRenderer));tiles.transform.SetParent(grid.transform);tiles.transform.position=new Vector3(0,-1,0);var map=tiles.GetComponent<Tilemap>();var tile=AssetDatabase.LoadAssetAtPath<Tile>("Assets/Scenes/V2/ProbeTile.asset");for(int x=0;x<width;x++){map.SetTile(new Vector3Int(x,0,0),tile);map.SetTile(new Vector3Int(x,1,0),tile);}
  Block(room.transform,"Wall","V2 left wall",new Vector2(0,8),new Vector2(1,16));Block(room.transform,"Wall","V2 right wall",new Vector2(width,8),new Vector2(1,16));Block(room.transform,"Ceiling","V2 ceiling",new Vector2(width/2,16),new Vector2(width,1));
  // Each room has a distinct main-route profile; optional tower never lies on an exit approach.
  int number=int.Parse(id.Substring(1));float mesaX=width*.28f;
  Block(room.transform,"Landing","V2 low broad dais",new Vector2(mesaX,1.4f),new Vector2(number==2?8:5,.8f));
  if(id=="A03"){Block(room.transform,"Landing","V2 gate approach",new Vector2(48,1.5f),new Vector2(4,1));Block(room.transform,"Landing","V2 gate second step",new Vector2(52,3),new Vector2(2,.5f));Block(room.transform,"Landing","V2 gate shelf",new Vector2(64,4.5f),new Vector2(16,1));}
  if(id=="A04"){var e=new GameObject("V2 E1 reset");e.transform.SetParent(room.transform);e.transform.position=new Vector2(width*.52f,1.6f);e.AddComponent<EnemyReset>().prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/V2/E1_V2.prefab");Block(room.transform,"Landing","V2 combat bypass",new Vector2(width*.5f,2.5f),new Vector2(10,.6f));}
  if(id=="A05")Block(room.transform,"Landing","V2 Dash practice",new Vector2(width*.78f,1.6f),new Vector2(8,1.2f));
  float ox=width*.43f;for(int k=0;k<3;k++)Block(room.transform,"Landing","V2 optional precision "+k,new Vector2(ox+k*2.8f,2.3f+k*1.8f),new Vector2(k==0?2:1.3f,.5f));
  var reward=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/DiscoveryReward.prefab"));reward.transform.SetParent(room.transform);reward.transform.position=new Vector2(ox+5.6f,7);reward.GetComponent<DiscoveryReward>().candidateId="V2-"+id;
  var kill=Block(room.transform,"KillZone","V2 fall reset",new Vector2(width/2,-7),new Vector2(width,1));
  foreach(var boundary in UnityEngine.Object.FindObjectsByType<RoomCameraRig>(FindObjectsSortMode.None)){boundary.boundary.points=new[]{new Vector2(0,-1),new Vector2(width,-1),new Vector2(width,17),new Vector2(0,17)};}
  // Two art-only background layers and repeated supports give each beat a landmark.
  for(int layer=0;layer<2;layer++){var bg=new GameObject("V2 background layer "+layer);bg.transform.SetParent(room.transform);bg.transform.position=new Vector3(width/2,7,1+layer);var sr=bg.AddComponent<SpriteRenderer>();sr.sprite=B05Art.Tile(layer==0?43:45);sr.drawMode=SpriteDrawMode.Tiled;sr.size=new Vector2(width,18);sr.sortingOrder=-20+layer;sr.color=layer==0?new Color(.12f,.18f,.25f):new Color(.3f,.38f,.44f,.22f);}
  for(int k=0;k<5;k++){var prop=new GameObject("V2 support landmark "+k);prop.transform.SetParent(room.transform);prop.transform.position=new Vector3(8+k*(width-16)/4,5,0);var sr=prop.AddComponent<SpriteRenderer>();sr.sprite=B05Art.Tile(58);sr.drawMode=SpriteDrawMode.Tiled;sr.size=new Vector2(1,8);sr.sortingOrder=-5;sr.color=new Color(.55f,.65f,.7f);}
  room.GetComponent<RoomLabel>().notice="V2 "+id+" / 上 선택 발견 / 출구 →";
  EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,target);EditorSceneManager.OpenScene(target);int count=UnityEngine.Object.FindFirstObjectByType<Tilemap>().GetUsedTilesCount();File.WriteAllText("Validation/_raw/B05/"+id+"-author.txt","MEASURE width="+width+"u tileTypes="+count+"\nPASS saved new Scene; runtime traversal not yet tested\n");
 }
}


