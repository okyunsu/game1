using UnityEngine;
public sealed class DummyTarget:MonoBehaviour,IDamageable
{
    [Min(1),Tooltip("Test-only dummy starting HP.")] public int maxHP=10;
    public int HP { get; private set; }
    public int Hits { get; private set; }
    void Awake(){HP=maxHP;}
    public void ReceiveHit(int damage,Vector2 source){if(HP<=0)return;HP=Mathf.Max(0,HP-damage);Hits++;}
    void OnGUI(){var camera=Camera.main;if(camera==null)camera=FindFirstObjectByType<Camera>();if(camera==null)return;var p=camera.WorldToScreenPoint(transform.position);GUI.Label(new Rect(p.x-40,Screen.height-p.y-40,100,24),$"Dummy {HP}/{maxHP}");}
}
