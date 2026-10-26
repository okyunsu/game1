#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public sealed class B21DashCornerProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B21DashCornerProbe>("-verify-b21-dash-corner");
 GameObject Box(string name,Vector2 center,Vector2 size){var go=new GameObject(name);go.layer=6;go.transform.position=center;go.AddComponent<BoxCollider2D>().size=size;return go;}
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return new WaitForSeconds(.2f);Check(B21CandidateHost.DashCornerEnabled,"dash corner explicit opt-in");
  var floor=Box("B21 dash floor",new Vector2(3000,999.5f),new Vector2(100,1));var ledge=Box("B21 dash ledge",new Vector2(3006.2f,1000.1f),new Vector2(8,.2f));
  var dash=Player.GetComponent<PlayerDash>();dash.hasDash=true;Motor.hasDoubleJump=Motor.hasWallJump=false;var fix=Player.GetComponent<B21CornerFix>();
  for(int trial=1;trial<=10;trial++)
  {
   yield return Keys();dash.Cancel();yield return new WaitForSeconds(.6f);Motor.ClearTransientState();PlaceForAuthorizedSetup(new Vector2(3000,1000.015f));yield return new WaitForSeconds(.25f);
   int before=fix.DashCorrections;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.RightArrow,Key.LeftShift));yield return Until(()=>dash.IsDashing,"dash began "+trial,1);yield return Until(()=>!dash.IsDashing,"dash ended "+trial,1);yield return Keys();
   bool passed=fix.DashCorrections>before&&Body.position.x>3002.5f&&Player.GetComponent<Collider2D>().bounds.min.y>=1000.19f;
   lines.Add("MEASURE dash-corner trial="+trial+" corrected="+(fix.DashCorrections-before)+" lift="+fix.LastDashLift.ToString("F5")+" crossed="+passed+" feet="+Body.position.ToString("F4"));Check(passed,"dash ledge actual "+trial);
  }
  Destroy(floor);Destroy(ledge);
 }
}
#endif
