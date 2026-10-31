#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed partial class B22Probe
{
 IEnumerator B24Vault()
 {
  yield return RoomAt("A01");int no=0,yes=0;
  var reward=Room.GetComponentsInChildren<DiscoveryReward>(true).OrderByDescending(r=>r.transform.position.x).First();
  foreach(bool owned in new[]{false,true})for(int trial=1;trial<=5;trial++)
  {
   yield return ResetAt(new Vector2(36.5f,11.615f));NoAbilities();Player.GetComponent<PlayerDash>().hasDash=owned;
   Input(Key.RightArrow);yield return Until(()=>Body.position.x>=37.15f,"vault runup",2);Input(Key.RightArrow,Key.Z);
   bool launched=false,dashed=false,success=false;double end=Time.realtimeSinceStartupAsDouble+3;
   while(Time.realtimeSinceStartupAsDouble<end)
   {
    launched|=Body.linearVelocity.y>0;var keys=new List<Key>{Key.RightArrow,Key.Z};
    if(owned&&launched&&!dashed&&Body.linearVelocity.y<=.6f){keys.Add(Key.LeftShift);dashed=true;}Input(keys.ToArray());
    if(launched&&Motor.Grounded&&Body.position.x>=43&&Math.Abs(Body.position.y-11.6f)<.1f){success=true;break;}if(Body.position.y<10.6f)break;yield return null;
   }
   yield return Keys();if(success){if(owned)yes++;else no++;}Record($"MEASURE A01 vault trial={trial} dash={owned} passed={success} pos={Body.position:F4}");
   if(success&&owned&&!DiscoveryReward.Found(reward.candidateId))yield return GoX(reward.transform.position.x);
  }
  Record($"MEASURE A01 vault summary noAbility={no}/5 dash={yes}/5 collected={DiscoveryReward.Found(reward.candidateId)}");Check(no==0&&yes==5&&DiscoveryReward.Found(reward.candidateId),"vault gate and actual fragment collected");
 }
 IEnumerator B24Enemies()
 {
  yield return RoomAt("A01");var e4=Room.GetComponentsInChildren<E4Hover>().Single();
  yield return ResetAt(new Vector2(e4.transform.position.x,e4.GetComponent<Collider2D>().bounds.max.y+.6f));NoAbilities();int pogo=Player.GetComponent<PlayerAttack>().PogosPerformed,hp=Health.HP;
  yield return Keys(Key.DownArrow,Key.X);yield return Until(()=>Player.GetComponent<PlayerAttack>().PogosPerformed>pogo,"E4 pogo",2);yield return Keys();Record($"MEASURE A01 E4 pogo={Player.GetComponent<PlayerAttack>().PogosPerformed-pogo} vy={Body.linearVelocity.y:F3} damage={hp-Health.HP}");
  bool pass=true;
  foreach(string id in new[]{"A02","A05"})
  {
   yield return RoomAt(id);bool dash=id=="A05";int avoided=0,damaged=0;
   for(int trial=1;trial<=5;trial++)
   {
    yield return ResetAt(new Vector2(dash?24:18,1.015f));NoAbilities();Player.GetComponent<PlayerDash>().hasDash=dash;
    var reset=Room.GetComponentsInChildren<EnemyReset>().Single(r=>r.prefab.GetComponent<EnemyCharger>()!=null);reset.Live.SetActive(true);var e2=reset.Live.GetComponent<EnemyCharger>();
    yield return Until(()=>e2.State==EnemyCharger.Phase.Charge,"E2 charge",3);int before=Health.HP;double began=Time.timeAsDouble,end=Time.realtimeSinceStartupAsDouble+3;bool airborne=false,usedDash=false,crossed=false;
    while(Time.realtimeSinceStartupAsDouble<end)
    {
     var keys=new List<Key>{dash?Key.LeftArrow:Key.RightArrow};if(Time.timeAsDouble-began<.43)keys.Add(Key.Z);
     if(dash&&!usedDash&&Time.timeAsDouble-began>=.18){keys.Add(Key.LeftShift);usedDash=true;}Input(keys.ToArray());airborne|=Body.linearVelocity.y>0;
     bool past=dash?Body.position.x<e2.transform.position.x-1:Body.position.x>e2.transform.position.x+1;
     if(airborne&&Motor.Grounded&&past){crossed=true;break;}if(Health.HP<=0||RoomSession.Instance.Respawning)break;yield return null;
    }
    yield return Keys();if(crossed)avoided++;if(Health.HP<before)damaged++;Record($"MEASURE {id} E2 trial={trial} jumpDash={dash} passed={crossed} dashUsed={usedDash} damage={before-Health.HP} hp={Health.HP}");
   }
   Record($"MEASURE {id} E2 summary passed={avoided}/5 damaged={damaged}/5");pass&=avoided==5&&damaged==0;
  }
  Check(pass,"both authored E2 approaches 5/5 no damage");
 }
}
#endif
