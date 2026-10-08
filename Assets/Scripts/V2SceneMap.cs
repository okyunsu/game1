using System.Collections.Generic;
using UnityEngine;
public sealed class V2SceneMap : MonoBehaviour
{
    public string[] rooms;
    static readonly HashSet<string> active = new();
    public static bool Enabled => active.Count>0;
    public static string Resolve(string path){foreach(var id in active)if(path=="Assets/Scenes/"+id+".unity")return "Assets/Scenes/V2/"+id+"_V2.unity";return path;}
    void Awake(){active.Clear();foreach(var id in rooms)active.Add(id);}
}
