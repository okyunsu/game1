using System;
using UnityEngine;

[Serializable] public sealed class MapRoom { public string id; public float[] rect; }
[Serializable] public sealed class MapDoor { public string id,a,b,edge,type,gate; public float at; public float[] span; }
[Serializable] public sealed class WorldMapLayout
{
 public MapRoom[] rooms; public MapDoor[] doors;
 public MapRoom Room(string id)=>Array.Find(rooms,r=>r.id==id);
 public static string Name{get{const string prefix="-map-layout=";foreach(string arg in Environment.GetCommandLineArgs())if(arg.StartsWith(prefix,StringComparison.Ordinal))return arg.Substring(prefix.Length);return V4SceneMap.V5?"world_v5_A":"world_v7";}}
 public static WorldMapLayout Current;
 public static void Load(TextAsset asset){if(asset!=null)Current=JsonUtility.FromJson<WorldMapLayout>(asset.text);}
}
