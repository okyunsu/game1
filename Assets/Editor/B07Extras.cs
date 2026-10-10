using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using A=V3RoomRoutes.Action;
using P=V3RoomRoutes.Point;

public static class B07Extras
{
 static RoomDefinition room;static V3RoomRoutes plan;
 static P Point(float x,float y,A action=A.Walk)=>new P{position=new Vector2(x,y),action=action};
 static void Shelf(float x,float top,float width)
 {var go=S3Authoring.Block(room,"Landing","Second optional landing",new Vector2(x,top-.25f),new Vector2(width,.5f));go.GetComponent<SpriteRenderer>().color=room.roomId[0]=='A'?new Color(.4f,.53f,.6f):room.roomId[0]=='B'?new Color(.6f,.3f,.12f):new Color(.65f,.8f,.88f);}
 public static void SecretRetry()
 {
  var scene=EditorSceneManager.OpenScene(V3SceneMap.Path("A04"));
  var routes=Object.FindFirstObjectByType<V3RoomRoutes>();
  var secret=routes.routes.First(r=>r.name=="secret");
  int index=System.Array.FindIndex(secret.points,p=>Mathf.Approximately(p.position.x,60)||Mathf.Approximately(p.position.x,56));
  var point=secret.points[index];point.position.x=56;point.action=A.WalkUnder;secret.points[index]=point;
  EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
  File.WriteAllText("Validation/_raw/B07/R-A04-author.txt","MEASURE A04 secret runup 7u -> 11u; approach x60 ->56. B02/C02 accepted candidate pattern reused; geometry unchanged.\n");
  EditorApplication.Exit(0);
 }
 public static void Optional()
 {
  foreach(string id in new[]{"A04","B02","C02","C03"})
  {
   var scene=EditorSceneManager.OpenScene(V3SceneMap.Path(id));room=Object.FindFirstObjectByType<RoomDefinition>();plan=room.GetComponent<V3RoomRoutes>();
   if(plan.routes.Any(r=>r.name=="optional2")){if(id=="C03"){plan.routes.First(r=>r.name=="optional2").points=new[]{Point(5.1f,1.81f),Point(5.1f,10.6f,A.Climb),Point(13,1.81f),Point(20,1.81f),Point(24,3.5f,A.Pogo),Point(28,3.5f,A.Pogo)};EditorSceneManager.SaveScene(scene);}continue;}
   Vector2 rewardAt;P[] points;
   if(id=="C03"){rewardAt=new Vector2(28,4.2f);points=new[]{Point(20,1.81f),Point(24,3.5f,A.Pogo),Point(28,3.5f,A.Pogo)};}
   else{Shelf(8,4.6f,2);Shelf(14,id=="C02"?4.6f:6.4f,2);rewardAt=new Vector2(14,id=="C02"?5.4f:7.2f);points=new[]{Point(5.5f,1.81f,A.WalkUnder),Point(8,5.41f,A.DoubleJump),Point(8.5f,5.41f,A.WalkUnder),Point(14,rewardAt.y+.01f,id=="C02"?A.Dash:A.DoubleJump)};}
   var go=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/DiscoveryReward.prefab"));go.transform.SetParent(room.transform);go.transform.position=rewardAt;go.GetComponent<DiscoveryReward>().candidateId="V3-"+id+"-optional2";
   plan.routes=plan.routes.Concat(new[]{new V3RoomRoutes.Route{name="optional2",goal="Discovery:V3-"+id+"-optional2",dash=true,wallJump=true,doubleJump=true,points=points}}).ToArray();EditorSceneManager.SaveScene(scene);
  }
  AssetDatabase.SaveAssets();File.WriteAllText("Validation/_raw/B07/BL2-author.txt","MEASURE second optional: A04/B02 DJ precision, C02 DJ then Dash, C03 two existing-enemy pogos. Existing mandatory geometry preserved.\n");EditorApplication.Exit(0);
 }
 public static void Decor()
 {
  int count=0;
  var sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;
  foreach(string id in V3SceneMap.Rooms.Where(id=>id!="S01"))
  {
   var scene=EditorSceneManager.OpenScene(V3SceneMap.Path(id));room=Object.FindFirstObjectByType<RoomDefinition>();plan=room.GetComponent<V3RoomRoutes>();
   if(room.transform.Find("Regional decor")!=null)continue;
   var root=new GameObject("Regional decor");root.transform.SetParent(room.transform);
   for(int i=0;i<3;i++)
   {
    var go=new GameObject(id[0]=='A'?"Pipe and support":id[0]=='B'?"Steam gear block":"Window and railing");go.transform.SetParent(root.transform);go.transform.position=new Vector3(plan.dimensions.x*(.25f+i*.25f),plan.dimensions.y*.6f,1);go.transform.localScale=id[0]=='A'?new Vector3(.5f,7,1):id[0]=='B'?new Vector3(2.5f,2.5f,1):new Vector3(2,6,1);
    var image=go.AddComponent<SpriteRenderer>();image.sprite=sprite;image.sortingOrder=-20;image.color=id[0]=='A'?new Color(.25f,.4f,.45f,.6f):id[0]=='B'?new Color(.45f,.2f,.08f,.6f):new Color(.45f,.7f,.85f,.6f);count++;
   }
   EditorSceneManager.SaveScene(scene);
  }
  File.WriteAllText("Validation/_raw/B07/BL4-author.txt","MEASURE decor blocks="+count+" collider components=0; S01 untouched\n");AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
