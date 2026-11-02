using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public sealed class DiscoveryReward : MonoBehaviour
{
    [Header("Session discovery")]
    [Tooltip("Stable fragment ID. Persisted when saving is enabled.")]
    public string candidateId;
    [Header("Glow — seconds / opacity")]
    [Min(.1f), Tooltip("Seconds per glow cycle.")] public float glowPeriod = 1.2f;
    [Range(0, 1), Tooltip("Minimum glow opacity.")] public float minimumOpacity = .55f;
    static readonly HashSet<string> discovered = new();
    public static int Count => ProgressSave.Enabled?ProgressSave.FragmentCount:discovered.Count;
    public static bool Found(string id) => discovered.Contains(id)||ProgressSave.Enabled&&ProgressSave.IsFragmentCollected(id);
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
        B25EffectsHost.Emit("fragment",transform.position);
        if(ProgressSave.Enabled)ProgressSave.CollectFragment(candidateId,RoomSession.Instance);
        gameObject.SetActive(false);
    }
    void OnValidate() => GetComponent<BoxCollider2D>().isTrigger = true;
}
