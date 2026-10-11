using UnityEngine;
[DefaultExecutionOrder(150)]
public sealed class SteamVent:MonoBehaviour
{
 public MechanicsTuning tuning;
 public bool On=>Time.time%(tuning.steamOn+tuning.steamOff)<tuning.steamOn;
 void FixedUpdate()
 {
  if(!On||GameState.GetOrCreate().Paused)return;
  var player=RoomSession.Instance?.Player;if(player==null||player.InputLocked)return;
  var body=player.GetComponent<Rigidbody2D>();var area=GetComponent<BoxCollider2D>().bounds;var shape=player.GetComponent<BoxCollider2D>().bounds;
  if(body.position.x<area.min.x||body.position.x>area.max.x||shape.min.y>=area.max.y||shape.max.y<=area.min.y||player.GetComponent<PlayerSlam>()?.Slamming==true)return;
  body.linearVelocity=new Vector2(body.linearVelocity.x,Mathf.Min(tuning.steamSpeed,Mathf.Max(0,(area.max.y-shape.min.y)/Time.fixedDeltaTime)));
 }
 void Update(){var sr=GetComponent<SpriteRenderer>();if(sr!=null){var c=sr.color;c.a=On?.5f:.08f;sr.color=c;}}
}
