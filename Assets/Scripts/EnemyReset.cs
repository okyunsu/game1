using System.Collections;
using UnityEngine;
public sealed class EnemyReset:MonoBehaviour
{
    [Tooltip("Enemy prefab restored on death/checkpoint health reset and scene reentry.")]public GameObject prefab;
    public GameObject Live{get;private set;}
    PlayerHealth health;
    void Awake()=>ResetEnemy();
    IEnumerator Start(){while(RoomSession.Instance?.Player==null)yield return null;health=RoomSession.Instance.Player.GetComponent<PlayerHealth>();health.Restored+=ResetEnemy;}
    void ResetEnemy(){if(Live!=null)Destroy(Live);Live=Instantiate(prefab,transform.position,transform.rotation,transform);}
    void OnDestroy(){if(health!=null)health.Restored-=ResetEnemy;}
}
