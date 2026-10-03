using System.Collections;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D),typeof(SpriteRenderer))]
public sealed class DoubleJumpPickup:MonoBehaviour
{
    [Min(0),Tooltip("Acquisition announcement duration (seconds).")]public float announcementSeconds=3;
    bool acquired;
    IEnumerator Start(){while(RoomSession.Instance?.Player==null)yield return null;if(RoomSession.Instance.Player.GetComponent<PlayerMotor>().hasDoubleJump)Destroy(gameObject);}
    void OnTriggerEnter2D(Collider2D other){var session=RoomSession.Instance;if(acquired||session==null||other.GetComponent<PlayerInputReader>()!=session.Player)return;acquired=true;session.GrantDoubleJump();GetComponent<SpriteRenderer>().enabled=false;GetComponent<BoxCollider2D>().enabled=false;Destroy(gameObject,announcementSeconds);}
    void OnGUI(){if(acquired)GUI.Label(new Rect(16,108,800,30),"더블 점프 획득 — 공중에서 Z 한 번 더");}
}
