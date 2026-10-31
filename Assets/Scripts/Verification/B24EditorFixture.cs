#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
public static class B24EditorFixture
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()
 {
  if(!N5Args.Has("-b24-editor")||!N5Args.Has("-boss-fixture"))return;
  SceneManager.sceneLoaded+=Loaded;
 }
 static void Loaded(Scene scene,LoadSceneMode mode)
 {
  foreach(var root in scene.GetRootGameObjects())foreach(var room in root.GetComponentsInChildren<RoomDefinition>())
   if(room.roomId=="C04"&&!room.TryGetSpawn("CP-C04",out _)&&room.TryGetSpawn("Entry",out var entry))
   {
    var go=new GameObject("B24 Editor CP-C04 fixture");go.transform.SetParent(room.transform);go.transform.position=entry.transform.position;go.AddComponent<RoomSpawn>().spawnId="CP-C04";
    Debug.Log("FIXTURE B24 Editor CP-C04 at authored Entry; scene asset unchanged");
   }
 }
}
#endif
