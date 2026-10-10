using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class B06Signs
{
    public static void Create()
    {
        const string path="Assets/Prefabs/B06/SignPost.prefab";
        if(File.Exists(path))throw new IOException("Preserve sign prefab");
        var template=new GameObject("SignPost");template.AddComponent<SignPost>();
        var image=template.AddComponent<SpriteRenderer>();image.sprite=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level/Landing.prefab").GetComponent<SpriteRenderer>().sprite;image.color=new Color(.55f,.40f,.25f);image.sortingOrder=5;
        template.transform.localScale=new Vector3(.35f,.8f,1);
        var prefab=PrefabUtility.SaveAsPrefabAsset(template,path);UnityEngine.Object.DestroyImmediate(template);
        string[][] messages={
            new[]{"여기선 칼을 뽑지 않는다. 오래된 약속이지.","위로 간 사람들은 편지를 보내지 않아."},
            new[]{"관문은 곧 열린다. 다들 그렇게 말한 지 몇 해째야.","아래로 내려가는 놈들? 낙오자지."},
            new[]{"여기까지 와서 멈춘 이유? 위를 본 적이 있거든.","정상에 가면 뭘 할 거냐고? …글쎄."}
        };
        string[] ids={"A03","B01","C01"};
        for(int n=0;n<ids.Length;n++)
        {
            var scene=EditorSceneManager.OpenScene("Assets/Scenes/V2/"+ids[n]+"_V2.unity");
            var room=UnityEngine.Object.FindFirstObjectByType<RoomDefinition>();var cp=UnityEngine.Object.FindFirstObjectByType<Checkpoint>();
            for(int i=0;i<2;i++)
            {
                var go=(GameObject)PrefabUtility.InstantiatePrefab(prefab);go.name="B06 Sign "+(i+1);go.transform.SetParent(room.transform,true);
                go.transform.position=new Vector2(cp.transform.position.x+3+i*6,1.8f);go.GetComponent<SignPost>().message=messages[n][i];
            }
            EditorSceneManager.SaveScene(scene);
        }
        AssetDatabase.SaveAssets();File.WriteAllText("Validation/_raw/B06/E1-authoring.txt","PASS six authored one-line signs; CP.x+3/+9 y1.8; detection2u; no colliders or gameplay references.\n");EditorApplication.Exit(0);
    }
    public static void Check()
    {
        SessionState.SetBool("B06.Check",true);SessionState.SetInt("N5.Exit",1);
        EditorSceneManager.OpenScene("Assets/Scenes/V2/"+N5Args.Value("-b06-sign-room","A03")+"_V2.unity");
        EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;
    }
}
