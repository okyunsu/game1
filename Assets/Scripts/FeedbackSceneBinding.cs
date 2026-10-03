using UnityEngine;
using UnityEngine.SceneManagement;
public static class FeedbackSceneBinding
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Initialize(){SceneManager.sceneLoaded-=Loaded;SceneManager.sceneLoaded+=Loaded;}
 static void Loaded(Scene scene,LoadSceneMode mode){var tuning=Object.FindFirstObjectByType<AttackFeedback>()?.tuning;if(tuning==null)return;foreach(var root in scene.GetRootGameObjects())foreach(var component in root.GetComponentsInChildren<MonoBehaviour>(true))if(component is BossEncounter or DummyTarget)Attach(component.gameObject,tuning);}
 public static void BindExisting(FeedbackTuning tuning){foreach(var component in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))if(component is BossEncounter or DummyTarget)Attach(component.gameObject,tuning);}
 public static void Attach(GameObject go,FeedbackTuning tuning){if(go.GetComponent<HitFeedback>()!=null)return;var feedback=go.AddComponent<HitFeedback>();feedback.tuning=tuning;var overlay=new GameObject("HitOverlay");overlay.transform.SetParent(go.transform,false);feedback.overlay=overlay.AddComponent<SpriteRenderer>();feedback.overlay.sprite=go.GetComponent<SpriteRenderer>().sprite;feedback.overlay.color=Color.white;overlay.SetActive(false);}
}
