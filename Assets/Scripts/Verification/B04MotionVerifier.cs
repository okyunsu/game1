#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class B04MotionVerifier:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B04MotionVerifier>("-verify-b04-motion");
 N9PogoInput driver;readonly List<string> trace=new();int tick;
 protected override void Cleanup(){if(driver!=null)driver.produce=null;}
 protected override IEnumerator Tests(){yield return NewGame();RoomSession.Instance.GrantDash();RoomSession.Instance.GrantDoubleJump();yield return Keys();Player.ClearTransientInput();Body.position=new Vector2(8,1.81f);Player.transform.position=Body.position;Body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();yield return new WaitForSeconds(.3f);yield return Until(()=>Motor.Grounded,"grounded motion fixture");var shape=Player.GetComponent<BoxCollider2D>();Vector2 size=shape.size,offset=shape.offset;driver=gameObject.AddComponent<N9PogoInput>();driver.keyboard=keyboard;driver.produce=()=>{if(tick>=150)return Array.Empty<Key>();trace.Add($"{tick},{Body.position.x:F4},{Body.position.y:F4},{Body.linearVelocity.x:F4},{Body.linearVelocity.y:F4}");int n=tick++;var keys=new List<Key>();if(n<70)keys.Add(Key.RightArrow);if(n>=90&&n<125)keys.Add(Key.LeftArrow);if(n>=30&&n<44||n>=49&&n<68)keys.Add(Key.Z);if(n>=90&&n<93)keys.Add(Key.C);return keys.ToArray();};yield return Until(()=>tick>=150,"fixed input motion",12);driver.produce=null;Check(!RoomSession.Instance.Respawning&&Health.HP>0,"motion fixture survives");Check(shape.size==size&&shape.offset==offset,"collider diff0");string content=string.Join("\n",trace);string hash=BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content))).Replace("-","");lines.Add($"MEASURE motion fps={Application.targetFrameRate} AV1={PresentationBootstrap.Instance!=null} ticks={trace.Count} sha256={hash}");string csv=System.IO.Path.ChangeExtension(Evidence,"csv");if(System.IO.File.Exists(csv))throw new Exception("Preserve trace");System.IO.File.WriteAllText(csv,"tick,x,y,vx,vy\n"+content);}
}
#endif
