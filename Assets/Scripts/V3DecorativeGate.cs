using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class V3DecorativeGate:MonoBehaviour
{
 [TextArea,Tooltip("Designer note: this gate stays closed and has no interaction.")]public string meaning;
 void OnValidate()=>GetComponent<BoxCollider2D>().isTrigger=false;
}
