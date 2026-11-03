using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B25ArtImport
{
 public const string Root="Assets/Art/Original/v1";
 static string Output=>N5Args.Value("-verify-output","Validation/_raw/25/art-authoring.txt");
 public static Sprite Sprite(string id)=>AssetDatabase.LoadAssetAtPath<Sprite>(Root+"/"+(id.StartsWith("bg_")?"":"tiles/")+id+".png");
 public static void Import()
 {
  Directory.CreateDirectory(Root+"/tiles");foreach(string source in Directory.GetFiles("Design/art_v1","*.png").Concat(Directory.GetFiles("Design/art_v1/tiles","*.png")))
  {string path=Root+"/"+(Path.GetDirectoryName(source).Replace('\\','/').EndsWith("tiles")?"tiles/":"")+Path.GetFileName(source);File.Copy(source,path,true);}
  AssetDatabase.Refresh();var rows=new List<string>();
  foreach(string path in Directory.GetFiles(Root,"*.png",SearchOption.AllDirectories))
  {
   var importer=(TextureImporter)AssetImporter.GetAtPath(path);importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;importer.spritePixelsPerUnit=16;importer.filterMode=FilterMode.Point;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.mipmapEnabled=false;importer.wrapMode=TextureWrapMode.Repeat;
   var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;settings.spriteAlignment=Path.GetFileNameWithoutExtension(path)=="player"?7:0;importer.SetTextureSettings(settings);importer.SaveAndReimport();
   if(importer.spritePixelsPerUnit!=16||importer.filterMode!=FilterMode.Point||importer.textureCompression!=TextureImporterCompression.Uncompressed)throw new Exception("Import settings "+path);rows.Add("PASS import16PPU Point Uncompressed FullRect "+path);
  }
  if(rows.Count!=28)throw new Exception("28 PNG required for approved art v1.1");
  var art=AssetDatabase.LoadAssetAtPath<B25ArtSettings>(Root+"/ArtSettings.asset");if(art==null){art=ScriptableObject.CreateInstance<B25ArtSettings>();AssetDatabase.CreateAsset(art,Root+"/ArtSettings.asset");}art.player=Sprite("player");art.enemy=Sprite("enemy");art.e1=Sprite("E1");art.e2=Sprite("E2");art.e3=Sprite("E3");art.e4=Sprite("E4");EditorUtility.SetDirty(art);AssetDatabase.SaveAssets();File.WriteAllLines(Output,rows);EditorApplication.Exit(0);
 }
}
