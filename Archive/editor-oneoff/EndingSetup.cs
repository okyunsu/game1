using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public static class EndingSetup
{
 public static void Run(){EditorSceneManager.OpenScene("Assets/Scenes/C06.unity");Object.FindFirstObjectByType<BossEncounter>().gameObject.AddComponent<EndingFlow>();EditorSceneManager.SaveScene(SceneManager.GetActiveScene());EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var go=new GameObject("Ending");go.AddComponent<EndingMenu>().actions=AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Input/PlayerControls.inputactions");var camera=new GameObject("Ending Camera").AddComponent<Camera>();camera.transform.position=new Vector3(0,0,-10);camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.15f,.2f,.25f);S3Authoring.Save("Assets/Scenes/Ending.unity");AssetDatabase.SaveAssets();}
}
