using System.Collections.Generic;
using UnityEngine;
public sealed class HitFeedback:MonoBehaviour
{
 public FeedbackTuning tuning;public SpriteRenderer overlay;
 public int Calls{get;private set;}public double FlashStarted{get;private set;}public double FlashEnded{get;private set;}public int VisibleHP{get;private set;}
 readonly List<SpriteRenderer> pips=new();int maximum;bool popped;public static int PopsCreated{get;private set;}
 void Start(){ReadHP(out int hp,out maximum);if(GetComponent<BossEncounter>()==null)for(int i=0;i<maximum;i++){var go=new GameObject("HP pip "+i);go.transform.SetParent(transform,false);var sr=go.AddComponent<SpriteRenderer>();sr.sprite=overlay.sprite;sr.sortingOrder=overlay.sortingOrder+1;pips.Add(sr);}SetHP(hp);}
 void ReadHP(out int hp,out int max){hp=max=0;if(TryGetComponent<EnemyPatrol>(out var a)){hp=a.HP;max=a.tuning.maxHP;}else if(TryGetComponent<EnemyCharger>(out var b)){hp=b.HP;max=b.tuning.maxHP;}else if(TryGetComponent<EnemyTurret>(out var c)){hp=c.HP;max=c.tuning.maxHP;}else if(TryGetComponent<DummyTarget>(out var d)){hp=d.HP;max=d.maxHP;}else if(TryGetComponent<BossEncounter>(out var e)){hp=e.HP;max=e.tuning.maxHP;}}
 void SetHP(int hp){VisibleHP=hp;for(int i=0;i<pips.Count;i++)pips[i].color=i<hp?Color.white:new Color(.2f,.2f,.2f,1);}
 public void Hit(int hp){Calls++;FlashStarted=Time.timeAsDouble;overlay.gameObject.SetActive(true);SetHP(hp);Debug.Log("F2 HitFeedback "+name+" HP="+hp);if(hp==0&&!popped&&GetComponent<BossEncounter>()==null){popped=true;var go=new GameObject("DeathPop");go.transform.position=transform.position;var sr=go.AddComponent<SpriteRenderer>();sr.sprite=overlay.sprite;sr.sortingOrder=overlay.sortingOrder;var pop=go.AddComponent<DeathPop>();pop.tuning=tuning;pop.initialSize=GetComponent<SpriteRenderer>().bounds.size;PopsCreated++;}}
 void LateUpdate(){if(overlay.gameObject.activeSelf&&Time.timeAsDouble-FlashStarted>=tuning.hitFlashSeconds){overlay.gameObject.SetActive(false);FlashEnded=Time.timeAsDouble;}var source=GetComponent<SpriteRenderer>();overlay.transform.position=source.transform.position;overlay.transform.localScale=Vector3.one;overlay.sprite=source.sprite;overlay.sortingLayerID=source.sortingLayerID;overlay.sortingOrder=source.sortingOrder+10;var bounds=GetComponent<Collider2D>().bounds;float stride=tuning.hpPipSize+tuning.hpPipGap;var scale=transform.lossyScale;for(int i=0;i<pips.Count;i++){pips[i].transform.position=new Vector3(bounds.center.x+(i-(pips.Count-1)*.5f)*stride,bounds.max.y+tuning.hpPipOffsetY,0);pips[i].transform.localScale=new Vector3(tuning.hpPipSize/scale.x/overlay.sprite.bounds.size.x,tuning.hpPipSize/scale.y/overlay.sprite.bounds.size.y,1);}}
}
public sealed class DeathPop:MonoBehaviour
{
 public FeedbackTuning tuning;public Vector2 initialSize;double started;
 void Start(){started=Time.timeAsDouble;}
 void Update(){float t=(float)((Time.timeAsDouble-started)/tuning.deathPopSeconds);if(t>=1){Destroy(gameObject);return;}var sr=GetComponent<SpriteRenderer>();float multiplier=Mathf.Lerp(1,tuning.deathPopScale,t);transform.localScale=new Vector3(initialSize.x*multiplier/sr.sprite.bounds.size.x,initialSize.y*multiplier/sr.sprite.bounds.size.y,1);sr.color=new Color(1,1,1,1-t);}
}
