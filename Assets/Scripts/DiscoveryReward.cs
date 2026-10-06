using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public sealed class DiscoveryReward : MonoBehaviour
{
    [Header("Session discovery")]
    [Tooltip("Unique level candidate ID, A-1 through C-2. Not saved to disk.")]
    public string candidateId;
    [Header("Glow — seconds / opacity")]
    [Min(.1f), Tooltip("Seconds per glow cycle.")] public float glowPeriod = 1.2f;
    [Range(0, 1), Tooltip("Minimum glow opacity.")] public float minimumOpacity = .55f;
    static readonly HashSet<string> discovered = new();
    public static int Count => discovered.Count;
    public static bool Found(string id) => discovered.Contains(id);
    public static void ResetSession() => discovered.Clear();
    SpriteRenderer image;
    Color tint;
    void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        tint = image.color;
        GetComponent<BoxCollider2D>().isTrigger = true;
        if (Found(candidateId)) gameObject.SetActive(false);
    }
    void Update()
    {
        var color = tint;
        color.a = Mathf.Lerp(minimumOpacity, 1, .5f + .5f * Mathf.Sin(Time.unscaledTime * Mathf.PI * 2 / glowPeriod));
        image.color = color;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerInputReader>();
        if (player == null || player.InputLocked || string.IsNullOrEmpty(candidateId)) return;
        discovered.Add(candidateId);
        gameObject.SetActive(false);
    }
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
}
