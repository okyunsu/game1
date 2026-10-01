#if UNITY_EDITOR
using UnityEngine;
public sealed class SliceRoute : MonoBehaviour
{
 [Tooltip("Ordered landing blocks for the authored reversible greybox route.")] public Transform[] landings;
 [TextArea, Tooltip("Short room-specific movement instruction.")] public string instruction;
 void OnGUI(){
  GUI.Label(new Rect(16,42,700,24),gameObject.name+"  |  Attack / abilities not implemented");
  GUI.Label(new Rect(16,66,900,24),instruction);
 }
}
#endif
