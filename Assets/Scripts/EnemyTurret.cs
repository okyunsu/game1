using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(SpriteRenderer),typeof(BoxCollider2D))]
public sealed class EnemyTurret:MonoBehaviour,IDamageable
{
    [Tooltip("Shared E3 values.")]public E3Tuning tuning;
    [Tooltip("Reusable shot prefab. Its collider controls physical cover clearance.")]public GameObject projectilePrefab;
    [Range(-1,1),Tooltip("Fixed authored firing direction (-1 left, +1 right).")]public int direction=-1;
    [Min(0),Tooltip("Horizontal muzzle offset (u), along the fixed firing direction.")]public float muzzleOffset=1.15f;
    public int HP{get;private set;}
    public bool Warning{get;private set;}
    public int ShotsFired{get;private set;}
    public double LastShotAt{get;private set;}
    double nextWarning,started;SpriteRenderer sprite;Color original;
    void Awake(){HP=tuning.maxHP;sprite=GetComponent<SpriteRenderer>();original=sprite.color;}
    public bool CanSeePlayer(){var player=RoomSession.Instance?.Player;var camera=Camera.main;if(player==null||player.InputLocked||camera==null)return false;var viewport=camera.WorldToViewportPoint(transform.position);if(viewport.z<=0||viewport.x<=0||viewport.x>=1||viewport.y<=0||viewport.y>=1)return false;Vector2 delta=player.transform.position-transform.position;if(delta.x*direction<=0||delta.magnitude>tuning.detectionRange)return false;return Physics2D.Linecast(transform.position,player.transform.position,1<<6).collider==null;}
    void Update(){if(HP<=0)return;if(Warning){sprite.color=Color.Lerp(original,Color.yellow,Mathf.PingPong((float)(Time.timeAsDouble-started)*10,1));if(!CanSeePlayer()){Warning=false;sprite.color=original;return;}if(Time.timeAsDouble-started>=tuning.warningSeconds){Fire();Warning=false;sprite.color=original;}}
        else if(Time.timeAsDouble>=nextWarning&&CanSeePlayer()){Warning=true;started=Time.timeAsDouble;nextWarning=started+tuning.shotPeriod;}}
    void Fire(){var go=Instantiate(projectilePrefab,(Vector2)transform.position+Vector2.right*direction*muzzleOffset,Quaternion.identity);SceneManager.MoveGameObjectToScene(go,gameObject.scene);var shot=go.GetComponent<EnemyShot>();shot.damage=tuning.projectileDamage;shot.speed=tuning.projectileSpeed;shot.direction=direction<0?-1:1;ShotsFired++;LastShotAt=Time.timeAsDouble;}
    public void ReceiveHit(int damage,Vector2 source){if(HP<=0||damage<=0)return;HP=Mathf.Max(0,HP-damage);if(HP==0)Destroy(gameObject);}
}
