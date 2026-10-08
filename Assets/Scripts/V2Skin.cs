using UnityEngine;

// Visual child only; original renderer remains available to gameplay feedback.
[ExecuteAlways]
public sealed class V2Skin : MonoBehaviour
{
    public SpriteRenderer source;
    public SpriteRenderer image;
    [Tooltip("Repeat terrain pixels at 18 PPU without scaling the collision geometry.")] public bool tiled;
    Color initial;bool captured;
    void OnEnable(){captured=false;}
    void LateUpdate()
    {
        if(source==null||image==null)return;
        if(!captured){initial=source.color;captured=true;}source.forceRenderingOff=true;
        image.enabled=source.enabled;
        image.color=source.color.r==initial.r&&source.color.g==initial.g&&source.color.b==initial.b?new Color(1,1,1,source.color.a):source.color;
        image.flipX=source.flipX;image.flipY=source.flipY;
        if(tiled){var s=source.transform.lossyScale;image.transform.localScale=new Vector3(1/Mathf.Max(.001f,Mathf.Abs(s.x)),1/Mathf.Max(.001f,Mathf.Abs(s.y)),1);image.size=new Vector2(Mathf.Abs(s.x),Mathf.Abs(s.y));}
    }
    void OnDisable(){if(source!=null)source.forceRenderingOff=false;}
}
