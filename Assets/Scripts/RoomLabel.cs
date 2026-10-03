using UnityEngine;
[RequireComponent(typeof(RoomDefinition))]
public sealed class RoomLabel : MonoBehaviour
{
    [Tooltip("Additional authored room notice.")] public string notice;
    void OnGUI()
    {
        GUI.Label(new Rect(16,16,600,24),GetComponent<RoomDefinition>().roomId);
        var dash=RoomSession.Instance?.Player?.GetComponent<PlayerDash>();
        GUI.Label(new Rect(16,42,700,24),string.IsNullOrEmpty(notice)?"Attack X (J) / gamepad X | Dash "+(dash!=null&&dash.hasDash?"C (K / Left Shift) / B":"not acquired"):notice);
        if(!string.IsNullOrEmpty(ProgressSave.Message))GUI.Label(new Rect(16,140,800,30),ProgressSave.Message);
    }
}
