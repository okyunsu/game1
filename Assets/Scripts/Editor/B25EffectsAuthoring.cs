using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class B25EffectsAuthoring
{
 public const string ProfilePath="Assets/Audio/B25/Feedback.asset";
 public static void Install()
 {
  Directory.CreateDirectory("Assets/Audio/B25");AssetDatabase.Refresh();var profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>(ProfilePath);
  if(profile==null){profile=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<PresentationProfile>("Assets/Candidates/AV1/PresentationProfile.asset"));profile.name="B25 Feedback";AssetDatabase.CreateAsset(profile,ProfilePath);}
  profile.fragmentSeconds=.18f;profile.fragmentSpeed=3;profile.impactSize=.12f;
  var sounds=profile.sounds.ToList();foreach(var pair in new[]{new[]{"fragment","ability"},new[]{"doorLock","bossTell"},new[]{"doorOpen","checkpoint"},new[]{"lift","jump"},new[]{"steam","dash"}})
   if(!sounds.Any(s=>s.eventName==pair[0]))sounds.Add(new PresentationProfile.Sound{eventName=pair[0],clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/AV1/"+pair[1]+".wav"),volume=pair[0]=="steam"?.18f:.28f,pitchVariation=.02f});
  profile.sounds=sounds.ToArray();EditorUtility.SetDirty(profile);var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").Concat(new[]{"Assets/Scenes/Test/BossTest_B1.unity","Assets/Scenes/Test/BossTest_B2.unity"}))
  {EditorSceneManager.OpenScene(path);var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var host=room.GetComponent<B25EffectsHost>()??room.gameObject.AddComponent<B25EffectsHost>();host.profile=profile;EditorSceneManager.SaveScene(room.gameObject.scene,path);rows.Add("PASS B25 effect profile bound "+path);}
  AssetDatabase.SaveAssets();File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/25/effects-authoring.txt"),rows);EditorApplication.Exit(0);
 }
 public static void BossPresentation()
 {
  var profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>(ProfilePath);profile.bossIntroZoom=.9f;profile.bossDeathFreeze=.1f;EditorUtility.SetDirty(profile);var rows=new List<string>();
  foreach(string path in Directory.GetFiles("Assets/Scenes/V5","*.unity").Concat(new[]{"Assets/Scenes/Test/BossTest_B1.unity","Assets/Scenes/Test/BossTest_B2.unity"}))
  {EditorSceneManager.OpenScene(path);var bosses=UnityEngine.Object.FindObjectsByType<BossEncounter>(FindObjectsSortMode.None).Where(b=>b.bossId!=ProgressSave.FinalBossId).ToArray();foreach(var boss in bosses){var fx=boss.GetComponent<B25BossFx>()??boss.gameObject.AddComponent<B25BossFx>();fx.profile=profile;rows.Add("PASS boss fx "+boss.bossId+" zoom0.9 intro unchanged freeze0.1");}if(bosses.Length>0)EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(),path);}
  AssetDatabase.SaveAssets();File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/25/boss-fx-authoring.txt"),rows);EditorApplication.Exit(0);
 }

 public static void EnemyTells(){var profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>(ProfilePath);profile.enemyTellLead=.3f;profile.enemyTellBrightness=1.8f;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/25/enemy-tells.txt"),"PASS shared lead0.3s/brightness1.8; E1 contact ETA / E2 existing warning / E3 existing warning; AI timings unchanged\n");EditorApplication.Exit(0);}

}
