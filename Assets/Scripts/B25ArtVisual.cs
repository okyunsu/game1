using UnityEngine;
// Display only. Collider, transform scale and gameplay components stay on the source.
public sealed class B25ArtVisual:MonoBehaviour
{
 public SpriteRenderer source;
 public SpriteRenderer[] images;
 public bool originalEnabled=true,followColor,followAlpha;
 void Awake(){if(source==null)return;bool gray=N5Args.Has("-v5-graybox");source.enabled=gray&&originalEnabled;foreach(var sr in images)sr.enabled=!gray;}
 void LateUpdate(){if(N5Args.Has("-v5-graybox")||(!followColor&&!followAlpha))return;foreach(var sr in images){var c=followColor?source.color:sr.color;if(followAlpha)c.a=source.color.a;sr.color=c;}}
}
