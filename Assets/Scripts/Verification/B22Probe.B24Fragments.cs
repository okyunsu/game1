#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine;
public sealed partial class B22Probe
{
 IEnumerator B24Fragments()
 {
  Check(ProgressSave.FragmentCount==0&&Health.MaxHP==4,"fresh fragment slot base HP4");Check(Health.TakeDamage(1,Body.position),"damage before four-fragment healing fixture");
  string first=null;
  foreach(string id in new[]{"A01","A02"})
  {
   yield return RoomAt(id);DisableEnemies();
   var rewards=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderBy(r=>r.transform.position.y).ThenBy(r=>r.transform.position.x).ToArray();
   foreach(var reward in rewards)
   {
    first??=reward.candidateId;Record("FIXTURE actual authored fragment contact via setup placement id="+reward.candidateId);
    PlaceForAuthorizedSetup((Vector2)reward.transform.position+Vector2.down*.5f);yield return Until(()=>ProgressSave.IsFragmentCollected(reward.candidateId),"fragment contact "+reward.candidateId,2);
    Record($"MEASURE collected={ProgressSave.FragmentCount} bonus={ProgressSave.MaxHPBonus} maxHP={Health.MaxHP} HP={Health.HP}");
   }
  }
  Check(ProgressSave.FragmentCount==4&&Health.MaxHP==5&&Health.HP==5,"fourth fragment maximum HP5 and immediate full heal");
  Check(GameHUD.Instance.FragmentNoticeVisible,"fragment notice visible");double began=Time.unscaledTimeAsDouble;yield return Until(()=>!GameHUD.Instance.FragmentNoticeVisible,"fragment notice ends",2);
  double notice=Time.unscaledTimeAsDouble-began;Record($"MEASURE fragment notice seconds={notice:F3} target=1.5");Check(Math.Abs(notice-1.5)<.08,"fragment notice 1.5s");
  Check(ProgressSave.TryLoad(out var data)&&data.fragmentIds.Length==4&&data.maxHPBonus==1,"IDs and maximum HP bonus saved synchronously");
  Check(!ProgressSave.CollectFragment(first,RoomSession.Instance)&&ProgressSave.FragmentCount==4,"duplicate fragment ID ignored");
  yield return Title();yield return ContinueGame();Check(ProgressSave.FragmentCount==4&&Health.MaxHP==5&&Health.HP==5,"Continue restores fragments and HP5");
  yield return RoomAt("A01");Check(Room.GetComponentsInChildren<DiscoveryReward>(true).All(r=>!r.gameObject.activeSelf),"collected fragments absent on reentry");
  yield return Title();yield return NewGame();Check(ProgressSave.FragmentCount==0&&Health.MaxHP==4&&Health.HP==4,"actual New Game clears fragments and maximum HP bonus");
  yield return Title();File.WriteAllText(ProgressSave.FilePath,"{\"version\":1,\"checkpointId\":\"CP-A03\",\"dash\":true}");yield return ContinueGame();
  Check(ProgressSave.FragmentCount==0&&Health.MaxHP==4&&Player.GetComponent<PlayerDash>().hasDash,"old version1 save without new fields remains compatible");
 }
}
#endif
