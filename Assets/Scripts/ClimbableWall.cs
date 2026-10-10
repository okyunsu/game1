using UnityEngine;

// Opt-in marker on the solid collider; no new physics layer required.
[RequireComponent(typeof(Collider2D))]
public sealed class ClimbableWall : MonoBehaviour { }
