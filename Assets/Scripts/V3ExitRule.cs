using UnityEngine;

[RequireComponent(typeof(RoomExit))]
public sealed class V3ExitRule:MonoBehaviour
{
 [Tooltip("Only downward movement can enter the pit exit. No reverse exit is authored.")]public bool downwardOnly;
 [Tooltip("The boss must be defeated before entering the summit door.")]public bool bossOnly;
 public bool Allows(Collider2D player)=> (!bossOnly||ProgressSave.BossDefeated)&&(!downwardOnly||player.attachedRigidbody!=null&&player.attachedRigidbody.linearVelocity.y<0);
}
