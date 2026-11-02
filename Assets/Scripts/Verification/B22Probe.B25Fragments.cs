#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public sealed partial class B22Probe
{
 IEnumerator B25FirstFour()
 {
  Check(ProgressSave.FragmentCount==0&&Health.MaxHP==4,"fresh AB first-four fixture");
  var rooms=new[]{"A01","A04","A01","B02"};var points=new[]{new Vector2(34,12.8f),new Vector2(20.3f,3),new Vector2(45.2f,12.8f),new Vector2(31,9.5f)};
  for(int i=0;i<4;i++)
  {
   yield return RoomAt(rooms[i]);var shard=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderBy(r=>Vector2.Distance(r.transform.position,points[i])).First();
   if(i==3){Check(Health.TakeDamage(1,Body.position),"damage before fourth fragment fixture");yield return new WaitForSeconds(Health.tuning.knockbackLock+.1f);}
   Record("FIXTURE actual authored contact at first-pass fragment "+shard.candidateId+"; route completion is separate evidence");PlaceForAuthorizedSetup((Vector2)shard.transform.position+Vector2.down*.5f);
   yield return Until(()=>ProgressSave.IsFragmentCollected(shard.candidateId),"AB first-four actual trigger",2);Record($"MEASURE AB fragment={i+1} maxHP={Health.MaxHP} HP={Health.HP} ID={shard.candidateId}");
  }
  Check(ProgressSave.FragmentCount==4&&Health.MaxHP==5&&Health.HP==5,"AB chosen first-four maxHP5 and full heal");Check(ProgressSave.TryLoad(out var saved)&&saved.fragmentIds.Length==4&&saved.maxHPBonus==1,"AB first-four ID/bonus saved");
  yield return Title();yield return ContinueGame();Check(ProgressSave.FragmentCount==4&&Health.MaxHP==5,"AB first-four Continue maxHP5");
 }
}
#endif
