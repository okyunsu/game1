using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public sealed class B05Lifecycle:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B05Lifecycle>("-verify-b05-life");
 protected override IEnumerator Tests(){yield return NewGame();string id=N5Args.Value("-b05-room","A01");if(id!="A01"){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity",id=="C06"?"FromLeft":"Entry"),"room fixture "+id);yield return Ready();}yield return new WaitForSeconds(.3f);
  var cp=FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).FirstOrDefault();bool checkpointRoom=cp!=null;if(cp!=null){PlaceForAuthorizedSetup(cp.transform.position);yield return new WaitForFixedUpdate();yield return null;Check(RoomSession.Instance.CheckpointId==cp.checkpointId,"actual checkpoint contact "+cp.checkpointId);}string expected=RoomSession.Instance.CheckpointId;
  var kill=FindObjectsByType<KillZone>(FindObjectsSortMode.None).First(k=>k.name.StartsWith("V2"));PlaceForAuthorizedSetup(kill.GetComponent<Collider2D>().bounds.center);yield return Until(()=>RoomSession.Instance.Respawning,"killzone contact");yield return Ready();Check(RoomSession.Instance.CheckpointId==expected&&Health.HP==Health.tuning.playerMaxHP,"fall death CP + full HP");Check(Health.TakeDamage(Health.HP,Body.position+Vector2.left),"lethal hit through unchanged health API");yield return Ready();Check(RoomSession.Instance.CheckpointId==expected&&Health.HP==Health.tuning.playerMaxHP,"hit death CP + full HP");
  if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,"Assets/Scenes/V2/"+id+"_V2.unity",id=="C06"?"FromLeft":"Entry"),"return fixture");yield return Ready();}
  RoomSession.Instance.GrantDash();RoomSession.Instance.GrantDoubleJump();var destinations=FindObjectsByType<RoomExit>(FindObjectsSortMode.None).Select(e=>e.destinationRoomId).Concat(FindObjectsByType<OptionalRoomEntry>(FindObjectsSortMode.None).Select(e=>e.destinationRoomId)).ToArray();foreach(string destination in destinations){var enemy=FindObjectsByType<EnemyReset>(FindObjectsSortMode.None).FirstOrDefault();int before=enemy==null?0:enemy.Live.GetInstanceID();yield return Door(destination);yield return Door(id);Check(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name==id+"_V2","bidirectional physical doors "+id+"-"+destination);if(before!=0){enemy=FindObjectsByType<EnemyReset>(FindObjectsSortMode.None).First();Check(enemy.Live!=null&&enemy.Live.GetInstanceID()!=before,"enemy reset on reentry");}}
  if(checkpointRoom){yield return Title();yield return ContinueGame();Check(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name==id+"_V2","V2 Continue CP Scene mapping");}
 }
 IEnumerator Door(string destination){var entry=FindObjectsByType<OptionalRoomEntry>(FindObjectsSortMode.None).FirstOrDefault(e=>e.destinationRoomId==destination);if(entry==null){yield return PhysicalDoor(destination);yield break;}yield return Keys();PlaceForAuthorizedSetup(entry.GetComponent<Collider2D>().bounds.center);yield return Keys(UnityEngine.InputSystem.Key.UpArrow);yield return Until(()=>RoomSession.Instance.CurrentRoomId==destination&&!RoomSession.Instance.Transitioning,"upward optional entry");yield return Ready();yield return Keys();}
}



