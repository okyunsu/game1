using UnityEngine;

// Presentation only: no collider, interaction key, combat or progression writes.
public sealed class SignPost : MonoBehaviour
{
    [TextArea,Tooltip("One line displayed when the player approaches.")] public string message;
    [Min(0),Tooltip("Player detection distance, world units.")] public float radius=2;
    [Tooltip("Label height above sign origin, world units.")] public float labelOffset=1.2f;
    [Min(10),Tooltip("Text size, screen pixels.")] public int fontSize=16;
    GUIStyle labelStyle;
    public bool TextVisible => RoomSession.Instance?.Player!=null&&!RoomSession.Instance.Player.InputLocked&&Vector2.Distance(RoomSession.Instance.Player.transform.position,transform.position)<=radius;
    void OnGUI()
    {
        if(!TextVisible||Camera.main==null)return;
        Vector3 p=Camera.main.WorldToScreenPoint(transform.position+Vector3.up*labelOffset);if(p.z<=0)return;
        labelStyle??=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter};labelStyle.fontSize=fontSize;var style=labelStyle;
        float width=Mathf.Min(720,Screen.width-24);float x=Mathf.Clamp(p.x-width*.5f,12,Screen.width-width-12);
        GUI.Label(new Rect(x,Screen.height-p.y-24,width,36),message,style);
    }
}
