using UnityEngine;
public sealed class SealFloor:MonoBehaviour
{
 public string sealId;
 public bool Broken{get;private set;}
 void Start(){if(ProgressSave.IsWallBroken(sealId))Break(false);}
 public void Break(bool save=true){Broken=true;if(save)ProgressSave.MarkWallBroken(sealId,RoomSession.Instance);GetComponent<Collider2D>().enabled=false;GetComponent<SpriteRenderer>().enabled=false;}
}
