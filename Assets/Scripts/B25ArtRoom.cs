using System.Collections.Generic;
using UnityEngine;
public sealed class B25ArtRoom:MonoBehaviour
{
 public B25ArtSettings settings;
 public SpriteRenderer background;
 Vector3 origin,cameraOrigin;bool anchored;
 readonly HashSet<int> drawn=new();
 void Awake(){if(N5Args.Has("-v5-graybox")&&background!=null)background.enabled=false;}
 void Character(GameObject actor,bool player)
 {
  if(actor==null||!drawn.Add(actor.GetInstanceID())||actor.transform.Find("Original art character")!=null)return;
  var source=actor.GetComponent<SpriteRenderer>();var shape=actor.GetComponent<Collider2D>();if(source==null||shape==null)return;
  var sprite=player?settings.player:settings.enemy;var b=shape.bounds;var size=sprite.bounds.size;
  var go=new GameObject("Original art character");go.transform.SetParent(actor.transform);var image=go.AddComponent<SpriteRenderer>();image.sprite=sprite;image.sortingOrder=source.sortingOrder;
  var scale=new Vector3(b.size.x/size.x,b.size.y/size.y,1);go.transform.position=b.center-Vector3.Scale(sprite.bounds.center,scale);var inherited=actor.transform.lossyScale;go.transform.localScale=new Vector3(scale.x/inherited.x,scale.y/inherited.y,1);
  var binding=actor.AddComponent<B25ArtVisual>();binding.source=source;binding.images=new[]{image};binding.followAlpha=true;source.sprite=sprite;source.enabled=false;
 }
 void LateUpdate()
 {
  if(N5Args.Has("-v5-graybox"))return;
  if(RoomSession.Instance?.Player!=null)Character(RoomSession.Instance.Player.gameObject,true);
  foreach(var reset in GetComponentsInChildren<EnemyReset>())Character(reset.Live,false);
  foreach(var bug in GetComponentsInChildren<E4Hover>())Character(bug.gameObject,false);
  var camera=Camera.main;if(camera==null||background==null)return;
  if(!anchored){anchored=true;origin=background.transform.position;cameraOrigin=camera.transform.position;}
  var delta=camera.transform.position-cameraOrigin;delta.z=0;background.transform.position=origin+delta*settings.parallax;
 }
}
