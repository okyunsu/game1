using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class B07Authoring
{
 public static void Foundation()
 {
  Directory.CreateDirectory("Assets/Scenes/V3");AssetDatabase.Refresh();
  string path=V3SceneMap.Path("A01");if(File.Exists(path))throw new IOException("Preserve existing V3 foundation");
  var scene=EditorSceneManager.OpenScene("Assets/Scenes/A01.unity");EditorSceneManager.SaveScene(scene,path);
  AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
 public static void RemainingFoundation()
 {
  foreach(string id in V3SceneMap.Rooms)
  {
   string path=V3SceneMap.Path(id);if(File.Exists(path))continue;
   var scene=EditorSceneManager.OpenScene("Assets/Scenes/"+(id=="S01"?"A01":id)+".unity");
   var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();room.roomId=id;
   foreach(var exit in room.GetComponentsInChildren<RoomExit>())exit.destinationScenePath=V3SceneMap.Path(exit.destinationRoomId);
   room.GetComponent<RoomLabel>().notice="V3 기반 / 방 제작 전";
   EditorSceneManager.SaveScene(scene,path);
  }
  AssetDatabase.SaveAssets();EditorApplication.Exit(0);
 }
}
