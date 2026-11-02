using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B25ArtAuthoring
{
 const string Root=B25ArtImport.Root;
 static string Output=>N5Args.Value("-verify-output","Validation/_raw/25/art-authoring.txt");
 static Sprite Sprite(string id)=>B25ArtImport.Sprite(id);
 static SpriteRenderer Layer(Transform parent,string id,Sprite sprite,Vector2 size,Vector3 world,int order,Color color)
 {
  var go=new GameObject(id);go.transform.SetParent(parent);go.transform.position=world;var scale=parent.lossyScale;go.transform.localScale=new Vector3(1/scale.x,1/scale.y,1);
  var sr=go.AddComponent<SpriteRenderer>();sr.sprite=sprite;sr.drawMode=SpriteDrawMode.Tiled;sr.size=size;sr.sortingOrder=order;sr.color=color;return sr;
 }
 static string Geometry(RoomDefinition room)=>string.Join("\n",room.GetComponentsInChildren<Collider2D>(true).Select(c=>c.name+" "+c.enabled+" "+c.isTrigger+" "+c.gameObject.layer+" "+c.transform.position.ToString("F5")+" "+c.transform.lossyScale.ToString("F5")+" "+EditorJsonUtility.ToJson(c)));
 public static void Apply()
 {
  var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").OrderBy(p=>p))
  {
   EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();if(room.GetComponent<B25ArtRoom>()!=null)throw new Exception("Art already applied; regenerate source rooms first");Physics2D.SyncTransforms();string before=Geometry(room);string floor=room.roomId.Substring(0,1);
   foreach(var source in room.GetComponentsInChildren<SpriteRenderer>(true).ToArray())
   {
    var shape=source.GetComponent<BoxCollider2D>();if(shape==null||!shape.enabled||!source.enabled)continue;
    string name=source.name,id=null;bool top=false,color=false,alpha=false;
    if(name.StartsWith("solid")||name.StartsWith("cover")||name.StartsWith("breakable")||name.StartsWith("closed:")){id=floor+"_wall";top=true;}
    else if(name.StartsWith("platform"))id=floor+(source.GetComponent<CrumblePlatform>()!=null?"_crumble":"_plat");
    else if(name.StartsWith("climb"))id="climb";
    else if(name.StartsWith("hazard"))id="acid";
    else if(name.StartsWith("door:")){id="door";color=true;}
    else if(name.StartsWith("lift:"))id="B_plat";
    else if(source.GetComponent<DiscoveryReward>()!=null){id="fragment";alpha=true;}
    else if(source.GetComponent<PogoTarget>()!=null)id=source.GetComponent<DamageBlock>()!=null?"spike":"lantern";
    if(source.GetComponent<BossEncounter>()!=null)source.color=new Color(.6f,.2f,.8f);
    if(id==null)continue;
    var bounds=shape.bounds;var images=new List<SpriteRenderer>();images.Add(Layer(source.transform,"Original art "+id,Sprite(id),bounds.size,bounds.center,source.sortingOrder,Color.white));
    if(top)images.Add(Layer(source.transform,"Original art top",Sprite(floor+"_top"),new Vector2(bounds.size.x,Math.Min(1,bounds.size.y)),new Vector3(bounds.center.x,bounds.max.y-Math.Min(1,bounds.size.y)/2,0),source.sortingOrder+1,Color.white));
    var binding=source.gameObject.AddComponent<B25ArtVisual>();binding.source=source;binding.images=images.ToArray();binding.originalEnabled=source.enabled;binding.followColor=color;binding.followAlpha=alpha;source.enabled=false;
   }
   var component=room.gameObject.AddComponent<B25ArtRoom>();component.settings=AssetDatabase.LoadAssetAtPath<B25ArtSettings>(Root+"/ArtSettings.asset");var dimensions=room.GetComponent<V4Room>().dimensions;
   component.background=Layer(room.transform,"Original art background",Sprite("bg_"+floor),new Vector2(dimensions.x*1.6f,dimensions.y*1.6f),new Vector3(dimensions.x/2,dimensions.y/2,0),-100,Color.white);
   Physics2D.SyncTransforms();if(before!=Geometry(room))throw new Exception("Collider geometry changed "+room.roomId);rows.Add("PASS "+room.roomId+" art display layers; all Collider2D serialized geometry/enabled/layer/transform unchanged; parallax shared="+component.settings.parallax);
   EditorSceneManager.SaveScene(room.gameObject.scene,path);
  }
  AssetDatabase.SaveAssets();File.WriteAllLines(Output,rows);EditorApplication.Exit(0);
 }
}
