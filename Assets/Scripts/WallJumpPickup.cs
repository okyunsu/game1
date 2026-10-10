using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public sealed class WallJumpPickup : MonoBehaviour
{
    bool acquired;
    [Min(0),Tooltip("Acquisition announcement duration (seconds).")] public float announcementSeconds=3;
    IEnumerator Start()
    {
        while (RoomSession.Instance?.Player == null) yield return null;
        if (RoomSession.Instance.Player.GetComponent<PlayerMotor>().hasWallJump) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var session = RoomSession.Instance;
        if (acquired || session == null || other.GetComponent<PlayerInputReader>() != session.Player) return;
        acquired = true; session.GrantWallJump();GetComponent<SpriteRenderer>().enabled=false;GetComponent<BoxCollider2D>().enabled=false;Destroy(gameObject,announcementSeconds);
    }
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
    void OnGUI(){if(GameHUD.Instance==null&&acquired)GUI.Label(new Rect(16,108,800,30),"벽 점프 획득 — 표시된 벽에 붙어 Z");}
}
