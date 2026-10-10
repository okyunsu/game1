using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public sealed class WallJumpPickup : MonoBehaviour
{
    bool acquired;
    IEnumerator Start()
    {
        while (RoomSession.Instance?.Player == null) yield return null;
        if (RoomSession.Instance.Player.GetComponent<PlayerMotor>().hasWallJump) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var session = RoomSession.Instance;
        if (acquired || session == null || other.GetComponent<PlayerInputReader>() != session.Player) return;
        acquired = true; session.GrantWallJump(); Destroy(gameObject);
    }
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
}
