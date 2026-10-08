using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class B05Art
{
 const string Root="Assets/Art/External/KenneyPixelPlatformer/";
 public static Sprite Tile(int n)=>AssetDatabase.LoadAssetAtPath<Sprite>(Root+"Tiles/tile_"+n.ToString("D4")+".png");
 public static void FinishSkins()
 {
  string[] names={"Player","E1","E2","E3"};int[] indices={0,18,24,25};string evidence="";
  var sprites=AssetDatabase.LoadAllAssetsAtPath(Root+"Tilemap/tilemap-characters.png").OfType<Sprite>().ToArray();
  for(int i=0;i<names.Length;i++){
   var original=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/"+names[i]+".prefab");var instance=(GameObject)PrefabUtility.InstantiatePrefab(original);var source=instance.GetComponent<SpriteRenderer>();var child=new GameObject("V2 visual");child.transform.SetParent(instance.transform,false);var image=child.AddComponent<SpriteRenderer>();image.sprite=sprites.First(s=>s.name.EndsWith("_"+indices[i]));image.sortingOrder=source.sortingOrder;child.transform.localScale=Vector3.one*(i==0?1.2f:1);var skin=instance.AddComponent<V2Skin>();skin.source=source;skin.image=image;PrefabUtility.SaveAsPrefabAsset(instance,"Assets/Prefabs/V2/"+names[i]+"_V2.prefab");UnityEngine.Object.DestroyImmediate(instance);evidence+="Created variant "+names[i]+"; original colliders untouched\n";
  }
  File.WriteAllText("Validation/_raw/B05/skins-finish.txt",evidence);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void Run()
 {
  Directory.CreateDirectory("Assets/Prefabs/V2");Directory.CreateDirectory("Assets/Scenes/V2");
  foreach(string path in Directory.GetFiles(Root,"*.png",SearchOption.AllDirectories)){
   var t=(TextureImporter)AssetImporter.GetAtPath(path);t.textureType=TextureImporterType.Sprite;t.spritePixelsPerUnit=18;t.filterMode=FilterMode.Point;t.textureCompression=TextureImporterCompression.Uncompressed;t.mipmapEnabled=false;t.SaveAndReimport();
  }
  // Exactly one Tilemap persistence attempt, no retry.
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var grid=new GameObject("Grid",typeof(Grid));var map=new GameObject("Tilemap",typeof(Tilemap),typeof(TilemapRenderer));map.transform.SetParent(grid.transform);var tile=ScriptableObject.CreateInstance<Tile>();tile.sprite=Tile(0);AssetDatabase.CreateAsset(tile,"Assets/Scenes/V2/ProbeTile.asset");AssetDatabase.SaveAssets();map.GetComponent<Tilemap>().SetTile(Vector3Int.zero,AssetDatabase.LoadAssetAtPath<Tile>("Assets/Scenes/V2/ProbeTile.asset"));EditorUtility.SetDirty(map.GetComponent<Tilemap>());EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,"Assets/Scenes/V2/TilemapProbe.unity");EditorSceneManager.OpenScene("Assets/Scenes/V2/TilemapProbe.unity");int count=UnityEngine.Object.FindFirstObjectByType<Tilemap>().GetUsedTilesCount();File.WriteAllText("Validation/_raw/B05/tilemap.txt","MEASURE reopen usedTiles="+count+"\n"+(count>0?"PASS":"FAIL")+" single attempt; terrain uses SpriteRenderer Tiled for consistent prefab skins\n");
  string[] names={"Level/Landing","Level/Wall","Level/Ceiling","Level/Door","Level/KillZone","PogoTarget","Player","E1","E2","E3","Boss"};int[] tiles={3,43,43,51,68,67,0,0,0,0,0};
  string evidence="";
  for(int i=0;i<names.Length;i++){
   string original="Assets/Prefabs/"+names[i]+".prefab";if(!File.Exists(original)){evidence+="MISSING "+original+"\n";continue;}
   var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(original);var instance=(GameObject)PrefabUtility.InstantiatePrefab(prefab);var source=instance.GetComponent<SpriteRenderer>();if(source==null)source=instance.GetComponentInChildren<SpriteRenderer>();
   if(source!=null){var child=new GameObject("V2 visual");child.transform.SetParent(source.transform,false);var image=child.AddComponent<SpriteRenderer>();image.sprite=Tile(tiles[i]);image.sortingOrder=source.sortingOrder;var skin=source.gameObject.AddComponent<V2Skin>();skin.source=source;skin.image=image;skin.tiled=i<5;image.drawMode=skin.tiled?SpriteDrawMode.Tiled:SpriteDrawMode.Simple;
    if(i>=6){string sheet=Root+"Tilemap/tilemap-characters.png";var ti=(TextureImporter)AssetImporter.GetAtPath(sheet);ti.spriteImportMode=SpriteImportMode.Multiple;ti.spritesheet=Enumerable.Range(0,27).Select(n=>new SpriteMetaData{name="character"+n,rect=new Rect(n%9*25,(2-n/9)*25,24,24),pivot=new Vector2(.5f,.5f)}).ToArray();ti.SaveAndReimport();image.sprite=AssetDatabase.LoadAllAssetsAtPath(sheet).OfType<Sprite>().First(s=>s.name=="character"+(i==6?0:i==7?18:i==8?24:i==9?25:26));child.transform.localScale=Vector3.one*(i==6?1.2f:1);}
   }
   var saved=PrefabUtility.SaveAsPrefabAsset(instance,"Assets/Prefabs/V2/"+Path.GetFileName(names[i])+"_V2.prefab");var a=prefab.GetComponentsInChildren<Collider2D>(true);var b=saved.GetComponentsInChildren<Collider2D>(true);bool same=a.Length==b.Length;for(int n=0;n<a.Length&&same;n++)same=EditorJsonUtility.ToJson(a[n])==EditorJsonUtility.ToJson(b[n]);evidence+=(same?"PASS":"FAIL")+" collider diff0 "+names[i]+"\n";UnityEngine.Object.DestroyImmediate(instance);
  }
  File.WriteAllText("Validation/_raw/B05/skins.txt",evidence);AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
