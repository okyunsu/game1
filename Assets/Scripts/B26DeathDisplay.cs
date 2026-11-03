using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// A collider-free snapshot outlives the actor; actor death/destruction stays immediate.
public static class B26DeathDisplay
{
 public static int Spawned{get;private set;}public static double LastEnded{get;set;}
 public static void Spawn(GameObject actor)
 {
  if(N5Args.Has("-v5-graybox")||actor.GetComponent<BossEncounter>()!=null)return;
  var room=UnityEngine.Object.FindFirstObjectByType<B25ArtRoom>();var binding=actor.GetComponent<B25ArtVisual>();if(room==null||binding==null||binding.images.Length==0)return;
  var source=binding.images[0];string kind=actor.GetComponent<E4Hover>()!=null?"E4":actor.GetComponent<EnemyTurret>()!=null?"E3":actor.GetComponent<EnemyCharger>()!=null?"E2":"E1";
  for(int i=0;i<5;i++)
  {
   var go=new GameObject("B26 death display "+kind);SceneManager.MoveGameObjectToScene(go,room.gameObject.scene);go.transform.SetPositionAndRotation(source.transform.position,source.transform.rotation);go.transform.localScale=source.transform.lossyScale*(i==0?1:.18f);var sr=go.AddComponent<SpriteRenderer>();sr.sprite=source.sprite;sr.color=source.color;sr.sortingOrder=source.sortingOrder+3;
   go.AddComponent<B26FadingSprite>().Initialize(room.settings.deathSeconds,i==0?Vector2.zero:new Vector2((i%2==0?-1:1)*2,(i<3?1:2)),kind,i==0);
  }
  Spawned++;Debug.Log($"MEASURE B26 death={kind} duration={room.settings.deathSeconds:F3} colliderFree=True actorDeathTimingUnchanged=True");
 }
}
public sealed class B26FadingSprite:MonoBehaviour
{
 float duration;Vector2 velocity;double began;SpriteRenderer image;Color tint;string kind;bool report;
 public void Initialize(float seconds,Vector2 movement,string id,bool measured){duration=seconds;velocity=movement;began=Time.timeAsDouble;image=GetComponent<SpriteRenderer>();tint=image.color;kind=id;report=measured;}
 void Update(){if(GameState.GetOrCreate().Paused)return;float t=(float)((Time.timeAsDouble-began)/duration);if(t>=1){if(report){B26DeathDisplay.LastEnded=Time.timeAsDouble;Debug.Log($"MEASURE B26 deathEnd={kind} elapsed={Time.timeAsDouble-began:F3}");}Destroy(gameObject);return;}transform.position+=(Vector3)(velocity*Time.deltaTime);image.color=new Color(tint.r,tint.g,tint.b,tint.a*(1-t));}
}
