using UnityEngine;
// Referenced TextAssets are attached to build scene copies, never the authored scenes.
public sealed class WorldMapSource:MonoBehaviour
{
 public TextAsset[] layouts;
 void Awake(){if(V4SceneMap.Enabled)foreach(var asset in layouts)if(asset.name==WorldMapLayout.Name){WorldMapLayout.Load(asset);break;}Destroy(gameObject);}
}
