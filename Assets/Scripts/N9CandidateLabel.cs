#if N9_H1 || N9_H2 || N9_H3
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class N9CandidateLabel : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init(){var go=new GameObject("N9 candidate label");DontDestroyOnLoad(go);go.AddComponent<N9CandidateLabel>();}
    void OnGUI(){if(SceneManager.GetActiveScene().name!="Menu")return;
#if N9_H1
        const string label="H1 — current Hard";
#elif N9_H2
        const string label="H2 — harder";
#else
        const string label="H3 — faster combat / more forgiveness";
#endif
        GUI.Label(new Rect(12,Screen.height-26,440,24),label,new GUIStyle(GUI.skin.label){fontSize=12});}
}
#endif
