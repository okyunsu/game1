using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Shared event audio and short, collider-free impact fragments. Installed only in B25 rooms/trials.
[DefaultExecutionOrder(600)]public sealed class B25EffectsHost:MonoBehaviour
{
 public PresentationProfile profile;
 public static B25EffectsHost Active{get;private set;}
 public static readonly Dictionary<string,int> Played=new();
 public static int Bursts{get;private set;}
 public bool AudioPlaying=>source!=null&&source.isPlaying;
 readonly Dictionary<string,double> last=new();AudioSource source;PlayerHealth health;bool dead;
 void Awake(){Active=this;source=gameObject.AddComponent<AudioSource>();source.playOnAwake=false;source.spatialBlend=0;gameObject.AddComponent<B26AudioHeadroom>();HitFeedback.Presented+=Hit;}
 void OnDestroy(){HitFeedback.Presented-=Hit;if(health!=null){health.Hit-=Hurt;health.Restored-=Restored;}if(Active==this)Active=null;}
 void Update(){BindTells();var candidate=RoomSession.Instance?.Player?.GetComponent<PlayerHealth>();if(candidate==health)return;if(health!=null){health.Hit-=Hurt;health.Restored-=Restored;}health=candidate;if(health!=null){health.Hit+=Hurt;health.Restored+=Restored;dead=health.HP<=0;}}
 void BindTells(){if(profile==null)return;foreach(var reset in GetComponentsInChildren<EnemyReset>()){if(reset.Live==null||reset.Live.GetComponent<B25EnemyTell>()!=null)continue;reset.Live.AddComponent<B25EnemyTell>().profile=profile;}}
 void Restored()=>dead=false;
 void Hurt(){if(Active!=this||health==null)return;if(health.HP<=0){if(dead)return;dead=true;Emit("death",health.transform.position);}else Emit("hurt",health.transform.position);Burst(health.gameObject);}
 void Hit(HitFeedback feedback,int hp){if(Active==this)EnemyHit(feedback.gameObject,hp);}
 public static void EnemyHit(GameObject actor,int hp){if(Active==null)return;B26HitFlash.Emit(actor);Emit("hit",actor.transform.position);if(hp==0){Emit(actor.GetComponent<BossEncounter>()!=null?"victory":"death",actor.transform.position);B26DeathDisplay.Spawn(actor);}Active.Burst(actor);}
 public static bool Emit(string id,Vector2 at)
 {
  var host=Active;if(host==null||host.profile==null||GameState.GetOrCreate().Paused)return false;
  if((id=="steam"||id=="lift")&&RoomSession.Instance?.Player!=null&&Vector2.Distance(at,RoomSession.Instance.Player.transform.position)>host.profile.deviceSoundRange)return false;
  if(host.last.TryGetValue(id,out double previous)&&Time.timeAsDouble-previous<host.profile.soundThrottle)return false;
  var sound=Array.Find(host.profile.sounds,s=>s.eventName==id);if(sound?.clip==null)return false;
  Played.TryGetValue(id,out int n);host.source.volume=host.profile.masterVolume;host.source.pitch=1+Mathf.Sin(n*1.618f)*sound.pitchVariation;host.source.PlayOneShot(sound.clip,sound.volume);host.last[id]=Time.timeAsDouble;Played[id]=n+1;
  Debug.Log($"MEASURE B25 AUDIO event={id} count={n+1} clip={sound.clip.name} gain={sound.volume:F3}");return true;
 }
 void Burst(GameObject actor)
 {
  var original=actor.GetComponent<SpriteRenderer>();if(original?.sprite==null)return;Bursts++;
  for(int i=0;i<4;i++){var go=new GameObject("B25 impact fragment");SceneManager.MoveGameObjectToScene(go,gameObject.scene);go.transform.position=actor.GetComponent<Collider2D>()?.bounds.center??actor.transform.position;float size=profile.impactSize;go.transform.localScale=new Vector3(size/original.sprite.bounds.size.x,size/original.sprite.bounds.size.y,1);var sprite=go.AddComponent<SpriteRenderer>();sprite.sprite=original.sprite;sprite.sortingOrder=20;sprite.color=Color.white;go.AddComponent<PresentationFragment>().Initialize(profile.fragmentSeconds,new Vector2(i%2==0?-profile.fragmentSpeed:profile.fragmentSpeed,(i<2?.6f:1.3f)*profile.fragmentSpeed));}
 }
}
