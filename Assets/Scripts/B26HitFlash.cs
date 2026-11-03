using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class B26HitFlash:MonoBehaviour
{
 SpriteRenderer source,image;float seconds;double began;
 public static int Emitted{get;private set;}public static double LastEnded{get;private set;}public static double LastDuration{get;private set;}
 public static void Emit(GameObject actor)
 {
  if(N5Args.Has("-v5-graybox"))return;var room=Object.FindFirstObjectByType<B25ArtRoom>();var binding=actor.GetComponent<B25ArtVisual>();if(room==null||room.settings.whiteFlash==null||binding==null||binding.images.Length==0)return;
  var source=binding.images[0];var go=new GameObject("B26 white hit flash");SceneManager.MoveGameObjectToScene(go,room.gameObject.scene);go.transform.SetPositionAndRotation(source.transform.position,source.transform.rotation);go.transform.localScale=source.transform.lossyScale;var sr=go.AddComponent<SpriteRenderer>();sr.sprite=source.sprite;sr.drawMode=source.drawMode;sr.size=source.size;sr.flipX=source.flipX;sr.sortingOrder=source.sortingOrder+30;sr.sharedMaterial=room.settings.whiteFlash;var fx=go.AddComponent<B26HitFlash>();fx.source=source;fx.image=sr;fx.seconds=room.settings.hitFlashSeconds;fx.began=Time.timeAsDouble;Emitted++;
 }
 void LateUpdate(){if(source!=null){transform.SetPositionAndRotation(source.transform.position,source.transform.rotation);transform.localScale=source.transform.lossyScale;image.flipX=source.flipX;}if(Time.timeAsDouble-began>=seconds){LastDuration=Time.timeAsDouble-began;LastEnded=Time.timeAsDouble;Debug.Log($"MEASURE B26 whiteFlash elapsed={LastDuration:F4} requested={seconds:F3}");Destroy(gameObject);}}
}
