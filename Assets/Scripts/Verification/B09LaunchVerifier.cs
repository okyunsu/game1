#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class B09LaunchVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B09LaunchVerifier>("-verify-b09-launch");
 protected override IEnumerator Tests(){yield return Ready();yield return new WaitForSeconds(.2f);string expected=N5Args.Has("-v4-mechanics")?"MechanicsTest":"A01";Check(RoomSession.Instance.CurrentRoomId==expected,"launcher room "+expected);Check(Player.GetComponent<PlayerSlam>().hasSlam==(N5Args.Has("-v4-afterboss")||N5Args.Has("-v4-mechanics")),"launcher ability state");if(expected=="A01"){Check(Motor.Grounded,"foot spawn grounded");if(Debug.isDebugBuild){var map=FindFirstObjectByType<V4MiniMap>();Check(map!=null,"development map available");yield return Tap(Key.M);Check(map.Visible&&System.Array.TrueForAll(V4SceneMap.Rooms,id=>map.GridText.Contains(id)),"M actual input shows all21 rooms");Check(RoomSession.Instance.RequestTransition("A02",V4SceneMap.Path("A02"),"A01-A02"),"map visit fixture");yield return Ready();Check(map.Visible&&ProgressSave.HasVisited("A02")&&map.GridText.Contains(">A02"),"map current room and visit refresh");}}}
}
#endif
