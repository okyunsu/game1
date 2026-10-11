using System.Collections;
using UnityEngine;
public sealed class EnemyReset:MonoBehaviour
{
    [Tooltip("Enemy prefab restored on death/checkpoint health reset and scene reentry.")]public GameObject prefab;
    [Tooltip("Apply the direction authored by V4 design data; off in existing scenes.")] public bool overrideTurretDirection;
    [Range(-1,1),Tooltip("Authored E3 firing direction: -1 left, +1 right.")] public int turretDirection=-1;
    public GameObject Live{get;private set;}
    PlayerHealth health;
    void Awake()=>ResetEnemy();
    IEnumerator Start(){while(RoomSession.Instance?.Player==null)yield return null;health=RoomSession.Instance.Player.GetComponent<PlayerHealth>();health.Restored+=ResetEnemy;}
    void ResetEnemy(){if(Live!=null)Destroy(Live);Live=Instantiate(prefab,transform.position,transform.rotation,transform);if(overrideTurretDirection&&Live.TryGetComponent<EnemyTurret>(out var turret))turret.direction=turretDirection;}
    void OnDestroy(){if(health!=null)health.Restored-=ResetEnemy;}
}
