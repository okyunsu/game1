#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class B20Headroom
{
 [Serializable] public sealed class Face
 {
  public string room,name,blocker;public float height,width,x0,x1,top,c,x;public int samples;
  public string Category(float shortRise,float fullRise)=>c<height+shortRise-.003f?"b":c<height+fullRise-.003f?"c":"d";
 }
 [Serializable] public sealed class Data{public Face[] faces;}
 public static List<Face> Scan(RoomDefinition room,Vector2 original)
 {
  Physics2D.SyncTransforms();var result=new List<Face>();
  var blocks=room.GetComponentsInChildren<BoxCollider2D>().Where(V5B18Clearance.Blocking).ToArray();
  var surfaces=blocks.Where(c=>c.name.StartsWith("solid",StringComparison.Ordinal)||c.name.StartsWith("platform",StringComparison.Ordinal));
  foreach(float height in new[]{1.6f,1.3f,1.1f})foreach(var face in surfaces)
  {
   var b=face.bounds;float top=b.max.y;if(top>=room.GetComponent<V4Room>().dimensions.y-.003f)continue;
   float width=original.x*height/original.y;
   var f=new Face{room=room.roomId,name=face.name,height=height,width=width,x0=b.min.x,x1=b.max.x,top=top,c=999,x=b.center.x,blocker="none"};
   var xs=new SortedSet<float>{b.min.x,b.max.x,b.min.x-.345f,b.max.x+.345f,b.center.x};
   for(float x=b.min.x-.345f;x<=b.max.x+.3451f;x+=.25f)xs.Add(x);
   foreach(float x in xs)
   {
    if(x+width/2<=b.min.x+.003f||x-width/2>=b.max.x-.003f)continue;
    // Covered internal tops and solid columns are not exposed standing positions.
    if(blocks.Any(c=>c!=face&&c.bounds.min.y<=top+.003f&&c.bounds.max.y>top+.003f&&x>c.bounds.min.x+.003f&&x<c.bounds.max.x-.003f))continue;
    f.samples++;
    foreach(var hit in blocks.Where(c=>c!=face&&c.bounds.min.y>top+.003f&&Mathf.Min(x+width/2,c.bounds.max.x)-Mathf.Max(x-width/2,c.bounds.min.x)>.003f))
    {
     float clearance=hit.bounds.min.y-top;
     if(clearance<f.c){f.c=clearance;f.x=x;f.blocker=hit.name;}
    }
   }
   if(f.samples>0)result.Add(f);
  }
  return result;
 }
}
#endif
