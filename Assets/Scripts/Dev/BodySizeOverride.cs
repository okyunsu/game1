#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using UnityEngine;
using UnityEngine.InputSystem;

// All changes belong to the live instance and a private combat-tuning copy.
// Neither the player prefab nor any tuning Asset is written.
[DefaultExecutionOrder(-150)]
public sealed class BodySizeOverride:MonoBehaviour
{
 public static readonly float[] Heights={1.6f,1.3f,1.1f};
 BoxCollider2D shape;Rigidbody2D body;PlayerAttack attack;CombatTuning original,copy;Vector3 originalScale;float originalHeight;
 [SerializeField,Tooltip("Comparison height in world units: 1.6, 1.3 or 1.1. F6 cycles these approved candidates.")]float targetHeight=1.6f;
 public float CurrentHeight=>shape.bounds.size.y;
 public float Ratio=>targetHeight/originalHeight;
 void Awake(){shape=GetComponent<BoxCollider2D>();body=GetComponent<Rigidbody2D>();attack=GetComponent<PlayerAttack>();originalScale=transform.localScale;originalHeight=shape.bounds.size.y;original=attack.tuning;copy=Instantiate(original);copy.name="Body size runtime attack geometry";attack.tuning=copy;SelectHeight(targetHeight);}
 public void SelectHeight(float height)
 {
  if(Array.FindIndex(Heights,h=>Mathf.Abs(h-height)<.001f)<0)throw new ArgumentOutOfRangeException(nameof(height));
  float feet=shape.bounds.min.y;targetHeight=height;float ratio=Ratio;transform.localScale=originalScale*ratio;Physics2D.SyncTransforms();
  Vector2 position=body.position+Vector2.up*(feet-shape.bounds.min.y);body.position=position;transform.position=position;Physics2D.SyncTransforms();
  // PlayerAttack queries world-space dimensions, so root scale alone is insufficient.
  copy.attackRange=original.attackRange*ratio;copy.attackHeight=original.attackHeight*ratio;
  copy.verticalAttackWidth=original.verticalAttackWidth*ratio;copy.verticalAttackHeight=original.verticalAttackHeight*ratio;
  copy.downAttackWidth=original.downAttackWidth*ratio;copy.downAttackHeight=original.downAttackHeight*ratio;
 }
 void Update()
 {
  var input=GetComponent<PlayerInputReader>();
  if(Keyboard.current?.f6Key.wasPressedThisFrame==true&&!input.InputLocked){int current=Array.FindIndex(Heights,h=>Mathf.Abs(h-targetHeight)<.001f);SelectHeight(Heights[(current+1)%Heights.Length]);}
 }
 void OnGUI(){GUI.Box(new Rect(Screen.width-320,12,305,58),"Body height: "+targetHeight.ToString("F1")+"u   F6: next size"+Environment.NewLine+"Attack reach: "+copy.attackRange.ToString("F3")+"u");}
 void OnDestroy(){if(attack!=null&&attack.tuning==copy)attack.tuning=original;if(copy!=null)Destroy(copy);}
}

#endif
