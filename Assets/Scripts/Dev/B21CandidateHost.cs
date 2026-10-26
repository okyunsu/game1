#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

public sealed class B21CandidateHost:MonoBehaviour
{
 public B21HazardReturnSettings hazardSettings;
 public B21CornerFixSettings cornerSettings;
 public static B21CandidateHost Instance{get;private set;}
 public static bool HazardEnabled=>Instance!=null&&N5Args.Has("-hazard-return")&&(V4SceneMap.V5||N5Args.Has("-b21-arena"));
 public static bool CornerEnabled=>Instance!=null&&N5Args.Has("-corner-fix")&&(V4SceneMap.V5||N5Args.Has("-b21-arena"));
 public static bool DashCornerEnabled=>CornerEnabled&&Instance.cornerSettings!=null&&(Instance.cornerSettings.dashCornerEnabled||N5Args.Has("-dash-corner-fix"));
 void Awake(){if(Instance!=null){Destroy(gameObject);return;}Instance=this;DontDestroyOnLoad(gameObject);Debug.Log("B21 candidate host hazardAsset="+(hazardSettings!=null)+" hazardFlag="+N5Args.Has("-hazard-return"));}
 void Update()
 {
  var player=RoomSession.Instance?.Player;if(player==null)return;
  if(HazardEnabled&&player.GetComponent<B21HazardReturn>()==null){var candidate=player.gameObject.AddComponent<B21HazardReturn>();candidate.settings=hazardSettings;}
  if(CornerEnabled&&player.GetComponent<B21CornerFix>()==null){var candidate=player.gameObject.AddComponent<B21CornerFix>();candidate.settings=cornerSettings;}
 }
}
#endif
