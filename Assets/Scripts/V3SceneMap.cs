using System;
using System.Collections.Generic;

// V3 is an explicit candidate. No state survives or leaks into other launch modes.
public static class V3SceneMap
{
 public static readonly string[] Rooms={"A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06","S01"};
 public static bool Enabled=>N5Args.Has("-v3");
 public static string Path(string id)=>"Assets/Scenes/V3/"+id+"_V3.unity";
 public static string Resolve(string path)
 {
  if(V4SceneMap.Enabled)return V4SceneMap.Resolve(path);
  if(!Enabled)return V2SceneMap.Resolve(path);
  foreach(var id in Rooms)if(path=="Assets/Scenes/"+id+".unity")return Path(id);
  return path;
 }
}
