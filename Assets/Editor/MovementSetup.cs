using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MovementSetup
{
    public static void Create()
    {
        var tags = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        tags.FindProperty("layers").GetArrayElementAtIndex(6).stringValue = "Ground";
        tags.ApplyModifiedPropertiesWithoutUndo();
        var tuning = ScriptableObject.CreateInstance<PlayerTuning>();
        AssetDatabase.CreateAsset(tuning, "Assets/ScriptableObjects/PlayerTuning.asset");
        var texture = new Texture2D(2, 2);
        texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white }); texture.Apply();
        File.WriteAllBytes("Assets/Art/Block.png", texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset("Assets/Art/Block.png");
        var importer = (TextureImporter)AssetImporter.GetAtPath("Assets/Art/Block.png");
        importer.textureType = TextureImporterType.Sprite; importer.spritePixelsPerUnit = 2;
        importer.filterMode = FilterMode.Point; importer.SaveAndReimport();
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Block.png");
        var material = new PhysicsMaterial2D("PlayerFrictionless") { friction = 0, bounciness = 0 };
        AssetDatabase.CreateAsset(material, "Assets/Prefabs/PlayerFrictionless.physicsMaterial2D");
        var player = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Player.prefab");
        var body = player.AddComponent<Rigidbody2D>();
        body.gravityScale = 0; body.freezeRotation = true;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        var box = player.AddComponent<BoxCollider2D>(); box.size = new Vector2(.7f, 1.6f); box.sharedMaterial = material;
        var renderer = player.AddComponent<SpriteRenderer>(); renderer.sprite = sprite; renderer.drawMode = SpriteDrawMode.Sliced; renderer.size = box.size; renderer.color = new Color(.95f, .74f, .32f);
        var motor = player.AddComponent<PlayerMotor>();
        var serialized = new SerializedObject(motor);
        serialized.FindProperty("tuning").objectReferenceValue = tuning;
        serialized.FindProperty("groundLayers").intValue = 1 << 6;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        PrefabUtility.SaveAsPrefabAsset(player, "Assets/Prefabs/Player.prefab");
        PrefabUtility.UnloadPrefabContents(player);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)); camera.tag = "MainCamera";
        camera.transform.position = new Vector3(0, 1, -10);
        var cam = camera.GetComponent<Camera>(); cam.orthographic = true; cam.orthographicSize = 9;
        cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.055f, .075f, .12f);
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        instance.transform.position = new Vector3(-10, -3.65f, 0);
        Block("Floor", new Vector2(0, -5), new Vector2(30, 1), sprite);
        Block("Low Platform", new Vector2(-6, -3), new Vector2(4, .5f), sprite);
        Block("Middle Platform", new Vector2(0, -1), new Vector2(4, .5f), sprite);
        Block("High Platform", new Vector2(6, 1), new Vector2(4, .5f), sprite);
        Block("Left Wall", new Vector2(-14.5f, 1), new Vector2(1, 13), sprite);
        Block("Right Wall", new Vector2(14.5f, 1), new Vector2(1, 13), sprite);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MovementTest.unity");
        // N1 boots directly into the technical test; Menu is an empty placeholder.
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/MovementTest.unity", true) };
        AssetDatabase.SaveAssets();
    }
    static void Block(string name, Vector2 position, Vector2 size, Sprite sprite)
    {
        var go = new GameObject(name, typeof(SpriteRenderer), typeof(BoxCollider2D));
        go.layer = 6; go.transform.position = position;
        go.GetComponent<BoxCollider2D>().size = size;
        var render = go.GetComponent<SpriteRenderer>(); render.sprite = sprite; render.drawMode = SpriteDrawMode.Sliced; render.size = size; render.color = new Color(.27f, .48f, .53f);
    }
}
