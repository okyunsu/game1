// This component is compiled and instantiated only in Q6 candidate builds.
#if Q6_A || Q6_B || Q6_C
using UnityEngine;
using UnityEngine.SceneManagement;
public sealed class Q6CandidateLabel:MonoBehaviour
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize(){var go=new GameObject("Q6 build candidate label");DontDestroyOnLoad(go);go.AddComponent<Q6CandidateLabel>();}
 // Respawned enemies originate from unchanged prefab assets. Rebind only the
 // presentation tuning reference to the asset embedded in this candidate scene.
 string observedScene;
 void LateUpdate(){var attack=Object.FindFirstObjectByType<AttackFeedback>();if(attack==null)return;if(observedScene!=attack.gameObject.scene.name){observedScene=attack.gameObject.scene.name;var t=attack.tuning;Debug.Log($"Q6 candidate {t.name} scene={observedScene} slash={t.slashAlpha} flash={t.hitFlashSeconds} pip={t.hpPipSize} pop={t.deathPopScale}");}foreach(var hit in Object.FindObjectsByType<HitFeedback>(FindObjectsSortMode.None))if(hit.tuning!=attack.tuning)hit.tuning=attack.tuning;}
 void OnGUI(){if(SceneManager.GetActiveScene().name!="Menu")return;
 #if Q6_A
 const string label="Q6-A (current)";
 #elif Q6_B
 const string label="Q6-B (strong)";
 #else
 const string label="Q6-C (soft)";
 #endif
 GUI.Label(new Rect(12,Screen.height-24,220,22),label,new GUIStyle(GUI.skin.label){fontSize=12});}
}
#endif
