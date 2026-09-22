using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSetup
{
    public static void Create()
    {
        var asset = ScriptableObject.CreateInstance<InputActionAsset>();
        var gameplay = asset.AddActionMap("Gameplay");
        var move = gameplay.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
        move.AddCompositeBinding("2DVector").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
        move.AddCompositeBinding("2DVector").With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
        move.AddBinding("<Gamepad>/leftStick"); move.AddBinding("<Gamepad>/dpad");
        var jump = gameplay.AddAction("Jump", InputActionType.Button);
        jump.AddBinding("<Keyboard>/space"); jump.AddBinding("<Gamepad>/buttonSouth");
        var pause = gameplay.AddAction("Pause", InputActionType.Button);
        pause.AddBinding("<Keyboard>/escape"); pause.AddBinding("<Gamepad>/start");
        var ui = asset.AddActionMap("UI");
        var navigate = ui.AddAction("Navigate", InputActionType.Value, expectedControlLayout: "Vector2");
        navigate.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow").With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
        navigate.AddBinding("<Gamepad>/dpad");
        var submit = ui.AddAction("Submit", InputActionType.Button);
        submit.AddBinding("<Keyboard>/enter"); submit.AddBinding("<Gamepad>/buttonSouth");
        var cancel = ui.AddAction("Cancel", InputActionType.Button);
        cancel.AddBinding("<Keyboard>/escape"); cancel.AddBinding("<Gamepad>/buttonEast");
        System.IO.Directory.CreateDirectory("Assets/Input");
        System.IO.File.WriteAllText("Assets/Input/PlayerControls.inputactions", asset.ToJson());
        Object.DestroyImmediate(asset);
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene("Assets/Scenes/Boot.unity");
        var player = new GameObject("Player", typeof(PlayerInputReader));
        var serialized = new SerializedObject(player.GetComponent<PlayerInputReader>());
        serialized.FindProperty("actions").objectReferenceValue = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Input/PlayerControls.inputactions");
        serialized.ApplyModifiedPropertiesWithoutUndo();
        PrefabUtility.SaveAsPrefabAssetAndConnect(player, "Assets/Prefabs/Player.prefab", InteractionMode.AutomatedAction);
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        var settings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        settings.FindProperty("activeInputHandler").intValue = 1;
        settings.ApplyModifiedPropertiesWithoutUndo();
        AssetDatabase.SaveAssets();
    }
}
