using UnityEngine;
[DefaultExecutionOrder(900)]
public sealed class B26IdleSparkle:MonoBehaviour
{
 public B25ArtSettings settings;B25ArtVisual binding;
 public float Brightness=>Mathf.Lerp(.65f,1.5f,.5f+.5f*Mathf.Cos(Time.time*Mathf.PI*2/settings.sparkleSeconds));
 void Awake()=>binding=GetComponent<B25ArtVisual>();
 public void Refresh(){if(binding==null||N5Args.Has("-v5-graybox"))return;foreach(var image in binding.images){float alpha=image.color.a;image.color=new Color(Brightness,Brightness,Brightness,alpha);}}
 void LateUpdate()=>Refresh();
}
