using UnityEngine;
[RequireComponent(typeof(RoomDefinition))]
public sealed class RoomLabel : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(16,16,600,24),GetComponent<RoomDefinition>().roomId);
        GUI.Label(new Rect(16,42,600,24),"Attack / abilities not implemented");
    }
}
