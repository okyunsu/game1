#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using UnityEngine;

public sealed class B20LaunchProbe:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<B20LaunchProbe>("-verify-b20-launch");
 protected override IEnumerator Tests()
 {
  Check(N5Args.Has("-save-path"),"isolated save");yield return Ready();yield return null;yield return null;
  bool comparison=N5Args.Has("-body-height")||N5Args.Has("-body-size-test");var size=Player.GetComponent<BodySizeOverride>();Check((size!=null)==comparison,"comparison opt-in only");
  float expected=float.Parse(N5Args.Value("-body-height","1.6"),System.Globalization.CultureInfo.InvariantCulture);
  Check(Mathf.Abs(Player.GetComponent<Collider2D>().bounds.size.y-expected)<.001f,"requested body height");
  bool hazard=N5Args.Has("-hazard-return")&&V4SceneMap.V5,corner=N5Args.Has("-corner-fix")&&V4SceneMap.V5;
  Check((Player.GetComponent<B21HazardReturn>()!=null)==hazard,"hazard return opt-in only");
  Check((Player.GetComponent<B21CornerFix>()!=null)==corner,"corner correction opt-in only");
  if(hazard)Check(Player.GetComponent<B21HazardReturn>().settings!=null,"hazard candidate Asset included");
  if(corner)Check(Player.GetComponent<B21CornerFix>().settings!=null,"corner candidate Asset included");
  lines.Add("MEASURE candidate flags hazard="+hazard+" corner="+corner);
  if(N5Args.Has("-body-size-test"))Check(RoomSession.Instance.CurrentRoomId=="BodySizeTest","test Scene");
  lines.Add("MEASURE launch room="+RoomSession.Instance.CurrentRoomId+" height="+expected+" comparison="+comparison+" cameraSize="+FindFirstObjectByType<RoomCameraRig>().orthographicSize+" attackReach="+Player.GetComponent<PlayerAttack>().HitboxSize.x);
  yield return new WaitForSecondsRealtime(10);
 }
}
#endif
