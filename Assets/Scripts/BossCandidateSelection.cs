using System;
using UnityEngine;
// Build-scene references only; authored scenes and default assets stay unchanged.
[DefaultExecutionOrder(-1000)]
public sealed class BossCandidateSelection:MonoBehaviour
{
 public BossTuning b2,b2fast,b2slow,b3;
 public PresentationProfile av1;
 void Awake(){
  if(N5Args.Has("-boss-fixture")&&V4SceneMap.Enabled){foreach(var root in gameObject.scene.GetRootGameObjects())foreach(var room in root.GetComponentsInChildren<RoomDefinition>())if(room.roomId=="C04"&&!room.TryGetSpawn("CP-C04",out _)&&room.TryGetSpawn("Entry",out var entry)){var go=new GameObject("B11 CP-C04 fixture spawn");go.transform.SetParent(room.transform);go.transform.position=entry.transform.position;go.AddComponent<RoomSpawn>().spawnId="CP-C04";}}
  if(N5Args.Has("-av1")&&PresentationBootstrap.Instance==null){var go=new GameObject("AV1 runtime candidate");go.SetActive(false);go.AddComponent<PresentationBootstrap>().profile=av1;go.SetActive(true);}
  var boss=GetComponent<BossEncounter>();if(boss==null)return;
  string arg=Array.Find(Environment.GetCommandLineArgs(),s=>s.StartsWith("-boss="));
  string id=arg==null?"base":arg.Substring(6).ToLowerInvariant();
  BossTuning chosen=id switch{"b2"=>b2,"b2fast"=>b2fast,"b2slow"=>b2slow,"b3"=>b3,_=>boss.tuning};
  if(chosen==null){Debug.LogWarning("BOSS requested candidate unavailable: "+id+"; using base");chosen=boss.tuning;id="base";}
  boss.tuning=chosen;
  Debug.Log($"BOSS candidate={id} BossTuning={chosen.name} extendedPatterns={chosen.extendedPatterns} b3Patterns={chosen.b3Patterns} patterns={(chosen.b3Patterns?boss.EnabledPatterns:"B2-fast cycle")}");
 }
}

