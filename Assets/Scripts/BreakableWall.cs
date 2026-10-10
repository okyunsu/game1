using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(SpriteRenderer))]
public sealed class BreakableWall : MonoBehaviour
{
    public enum Side { Left, Right, Any }
    [Min(1),Tooltip("Distinct attack hits required to open this wall.")] public int hitsToBreak=3;
    [Tooltip("Only attacks originating on this side can damage the wall.")] public Side breakableFrom=Side.Any;
    [Tooltip("Unique persistent wall ID. Duplicate IDs share open state.")] public string wallId;
    public int Hits { get; private set; }
    public bool Broken => !string.IsNullOrWhiteSpace(wallId)&&ProgressSave.IsWallBroken(wallId);
    void Start(){if(Broken)gameObject.SetActive(false);}
    void LateUpdate()
    {
        var mark=transform.Find("Open side indicator");var player=RoomSession.Instance?.Player;
        if(mark!=null)mark.GetComponent<SpriteRenderer>().enabled=player!=null&&(breakableFrom==Side.Any||breakableFrom==Side.Left&&player.transform.position.x<transform.position.x||breakableFrom==Side.Right&&player.transform.position.x>transform.position.x);
    }
    public bool ReceiveAttack(Vector2 source)
    {
        if(Broken||string.IsNullOrWhiteSpace(wallId))return false;
        if(breakableFrom==Side.Left&&source.x>=transform.position.x||breakableFrom==Side.Right&&source.x<=transform.position.x)return false;
        Hits++;GetComponent<HitFeedback>()?.Hit(Mathf.Max(0,hitsToBreak-Hits));
        if(Hits>=hitsToBreak)
        {
            ProgressSave.MarkWallBroken(wallId,RoomSession.Instance);
            GetComponent<BoxCollider2D>().enabled=false;GetComponent<SpriteRenderer>().enabled=false;
            // Existing hit feedback completes its flash/death pop before hiding.
            Invoke(nameof(Hide),GetComponent<HitFeedback>()?.tuning.hitFlashSeconds??0);
        }
        return true;
    }
    void Hide()=>gameObject.SetActive(false);
}
