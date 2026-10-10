using UnityEngine;

public sealed class MainPathLantern : MonoBehaviour
{
    void Start()
    {
        var exit=GetComponentInParent<RoomExit>();
        gameObject.SetActive(exit!=null&&exit.mainPath&&!GuidePresentation.Disabled);
    }
}
