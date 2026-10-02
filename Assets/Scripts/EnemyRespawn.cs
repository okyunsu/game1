using System.Collections;
using UnityEngine;
public sealed class EnemyRespawn : MonoBehaviour
{
    public GameObject prefab;
    public Transform leftPoint, rightPoint;
    public EnemyPatrol live;
    PlayerHealth health;
    IEnumerator Start(){while(RoomSession.Instance?.Player==null)yield return null;health=RoomSession.Instance.Player.GetComponent<PlayerHealth>();if(health!=null)health.Restored+=ResetEnemy;}
    void ResetEnemy(){if(live!=null)Destroy(live.gameObject);var next=Instantiate(prefab,transform.position,Quaternion.identity,transform);live=next.GetComponent<EnemyPatrol>();live.leftPoint=leftPoint;live.rightPoint=rightPoint;}
    void OnDestroy(){if(health!=null)health.Restored-=ResetEnemy;}
}
