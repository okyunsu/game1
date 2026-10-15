public static class V4SceneMap
{
 public static string[] Rooms{
 get{
 #if UNITY_EDITOR
 if(WorldMapLayout.Current==null)WorldMapLayout.Load(UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TextAsset>("Assets/Maps/"+WorldMapLayout.Name+".json"));
 #endif
 if(WorldMapLayout.Current==null)throw new System.InvalidOperationException("Map layout TextAsset not loaded");
 return System.Array.ConvertAll(WorldMapLayout.Current.rooms,r=>r.id);
 }}
 public static bool Enabled=>N5Args.Has("-v4");
 public static string Path(string id)=>"Assets/Scenes/V4/"+id+"_V4.unity";
 public static string Resolve(string path){if(!Enabled)return path;foreach(var id in Rooms)if(path=="Assets/Scenes/"+id+".unity")return Path(id);return path;}
}
