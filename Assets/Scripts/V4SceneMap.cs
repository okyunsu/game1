public static class V4SceneMap
{
 public static readonly string[] Rooms={"A01","A02","A03","A04","A05","A06","B01","B02","B03","B04","B05","B06","C01","C02","C03","C04","C05","C06","S01","S02","Z01"};
 public static bool Enabled=>N5Args.Has("-v4");
 public static string Path(string id)=>"Assets/Scenes/V4/"+id+"_V4.unity";
 public static string Resolve(string path){if(!Enabled)return path;foreach(var id in Rooms)if(path=="Assets/Scenes/"+id+".unity")return Path(id);return path;}
}
