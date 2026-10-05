using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public sealed class PogoTarget : MonoBehaviour
{
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
}
