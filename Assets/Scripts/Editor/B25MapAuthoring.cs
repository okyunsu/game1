using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using J=B09Json;
public static class B25MapAuthoring
{
 public static void Generate()
 {
  var source=J.Read(File.ReadAllText(B25WorldAuthoring.World));
  var layout=new WorldMapLayout{rooms=J.D(source["rooms"]).Select(p=>new MapRoom{id=p.Key,rect=J.A(J.D(p.Value),"rect").Select(J.F).ToArray()}).ToArray(),
   doors=J.A(source,"doors").Select(J.D).Select(d=>new MapDoor{id=J.S(d,"id"),a=J.S(d,"a"),b=J.S(d,"b"),edge=J.S(d,"edge"),type=J.S(d,"type"),gate=J.S(d,"gate"),at=J.F(d,"at"),span=J.A(d,"span").Select(J.F).ToArray(),stub=d.ContainsKey("stub")&&(bool)d["stub"]}).ToArray()};
  if(layout.rooms.Length!=12||layout.rooms.Count(r=>r.id.StartsWith("B"))!=6)throw new Exception("AB map counts");
  const string folder="Assets/Scripts/Resources";Directory.CreateDirectory(folder);File.WriteAllText(folder+"/world_v5_AB.json",JsonUtility.ToJson(layout,true)+"\n");AssetDatabase.Refresh();
  File.WriteAllText(N5Args.Value("-verify-output","Validation/_raw/25/map-authoring.txt"),$"PASS AB map rooms={layout.rooms.Length} B=6 doors={layout.doors.Length}; coordinates preserved; Resources fallback avoids modifying existing Assets/Maps\n");EditorApplication.Exit(0);
 }
}
