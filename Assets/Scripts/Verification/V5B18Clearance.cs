#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Opt-in Scene measurement. No authored objects, gameplay values, or saves are modified.
public sealed class V5B18Clearance : N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Init()=>N5Args.Start<V5B18Clearance>("-verify-b18-clearance");
 public static bool Blocking(Collider2D c)=>c.enabled&&!c.isTrigger&&c.gameObject.layer==6;
 public static bool Positive(Bounds a,Bounds b)=>Mathf.Min(a.max.x,b.max.x)-Mathf.Max(a.min.x,b.min.x)>.003f&&Mathf.Min(a.max.y,b.max.y)-Mathf.Max(a.min.y,b.min.y)>.003f;
 public static List<string> Scan(RoomDefinition room,Vector2 size)
 {
  Physics2D.SyncTransforms();var rows=new List<string>();int clearance=0,enemyFailures=0,patrolFailures=0,samples=0;
  var blockers=room.GetComponentsInChildren<Collider2D>().Where(Blocking).ToArray();
  var faces=blockers.OfType<BoxCollider2D>().Where(c=>c.name.StartsWith("solid",StringComparison.Ordinal)||c.name.StartsWith("platform",StringComparison.Ordinal)).ToArray();
  float roof=room.GetComponent<V4Room>().dimensions.y;
  foreach(var face in faces)
  {
   Bounds b=face.bounds;float top=b.max.y;if(top>=roof-.001f)continue;
   var xs=new SortedSet<float>{b.min.x,b.max.x,b.min.x-size.x/2+.005f,b.max.x+size.x/2-.005f};
   for(float x=b.min.x-size.x/2+.005f;x<=b.max.x+size.x/2-.005f;x+=.25f)xs.Add(x);
   foreach(float x in xs)
   {
    // Joined blocks cover internal lower faces. Only exposed, supported positions count.
    if(blockers.Any(c=>c!=face&&c.bounds.min.y<=top+.003f&&c.bounds.max.y>top+.003f&&x>c.bounds.min.x-.001f&&x<c.bounds.max.x+.001f))continue;
    samples++;float height=size.y+.6f;var center=new Vector2(x,top+.012f+height/2);var querySize=new Vector2(size.x-.006f,height-.006f);var body=new Bounds(center,querySize);
    foreach(var hit in Physics2D.OverlapBoxAll(center,querySize,0,1<<6).Where(c=>c!=face&&Blocking(c)&&Positive(body,c.bounds)))
    {
     var eff=hit.GetComponent<PlatformEffector2D>();bool oneWay=eff!=null&&hit.usedByEffector&&eff.useOneWay;
     rows.Add((oneWay?"MEASURE ":"FAIL ")+room.roomId+" clearance surface="+face.name+" x="+x.ToString("F3")+" feet="+top.ToString("F3")+" blocker="+hit.name+" oneWay="+oneWay+" minY="+hit.bounds.min.y.ToString("F3"));if(!oneWay)clearance++;
    }
   }
  }
  foreach(var reset in room.GetComponentsInChildren<EnemyReset>())
  {
   var shape=reset.prefab.GetComponent<BoxCollider2D>();if(shape==null){rows.Add("FAIL "+room.roomId+" enemy-shape missing prefab="+reset.prefab.name);enemyFailures++;continue;}
   Vector2 shapeSize=Vector2.Scale(shape.size,reset.prefab.transform.localScale);Vector2 center=(Vector2)reset.transform.position+shape.offset;
   Action<string,Vector2,bool> inspect=(label,at,patrol)=>{
    var box=new Bounds(at,shapeSize-new Vector2(.006f,.006f));
    var hits=Physics2D.OverlapBoxAll(at,shapeSize-new Vector2(.006f,.006f),0,1<<6).Where(c=>Blocking(c)&&Positive(box,c.bounds)).ToArray();
    float feet=at.y-shapeSize.y/2;
    bool support=Physics2D.OverlapBoxAll(new Vector2(at.x,feet-.025f),new Vector2(shapeSize.x-.02f,.08f),0,1<<6).Any(c=>Blocking(c)&&Mathf.Abs(c.bounds.max.y-feet)<.075f);
    if(hits.Length>0||!support){rows.Add("FAIL "+room.roomId+" "+label+" prefab="+reset.prefab.name+" x="+at.x.ToString("F3")+" feet="+feet.ToString("F3")+" blockers="+string.Join("|",hits.Select(c=>c.name))+" support="+support);if(patrol)patrolFailures++;else enemyFailures++;}
    else rows.Add("MEASURE "+room.roomId+" "+label+" x="+at.x.ToString("F3")+" feet="+feet.ToString("F3")+" blockers=none support=True");
   };
   inspect("enemy-spawn",center,false);var binding=reset.GetComponent<V5PatrolBinding>();if(binding==null)continue;
   foreach(var point in new[]{binding.leftPoint,binding.rightPoint})inspect("patrol-end",(Vector2)point.position+shape.offset,true);
  }
  rows.Add("MEASURE "+room.roomId+" summary samples="+samples+" clearance="+clearance+" enemy="+enemyFailures+" patrol="+patrolFailures);return rows;
 }
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();
  foreach(string id in V4SceneMap.Rooms){if(RoomSession.Instance.CurrentRoomId!=id){Check(RoomSession.Instance.RequestTransition(id,V4SceneMap.Path(id),"Entry"),"authored room fixture "+id);yield return Ready();}lines.AddRange(Scan(FindFirstObjectByType<RoomDefinition>(),Player.GetComponent<BoxCollider2D>().size));} Check(!lines.Any(l=>l.StartsWith("FAIL",StringComparison.Ordinal)),"clearance / enemy / patrol zero failures");
 }
}
#endif
