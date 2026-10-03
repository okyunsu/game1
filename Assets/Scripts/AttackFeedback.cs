using UnityEngine;
public sealed class AttackFeedback:MonoBehaviour
{
 public FeedbackTuning tuning;public SpriteRenderer slash;
 public int Activations{get;private set;}public double Started{get;private set;}public double Ended{get;private set;}
 void Start()=>FeedbackSceneBinding.BindExisting(tuning);
 public void Show(Vector2 center,Vector2 size){if(!slash.gameObject.activeSelf){Activations++;Started=Time.timeAsDouble;Debug.Log("F2 AttackSlash active");}slash.transform.position=center;var scale=transform.lossyScale;slash.transform.localScale=new Vector3(size.x/scale.x/slash.sprite.bounds.size.x,size.y/scale.y/slash.sprite.bounds.size.y,1);var color=tuning.slashColor;color.a=tuning.slashAlpha;slash.color=color;slash.gameObject.SetActive(true);}
 public void Hide(){if(slash!=null&&slash.gameObject.activeSelf){Ended=Time.timeAsDouble;slash.gameObject.SetActive(false);}}
 void OnDisable()=>Hide();
}
