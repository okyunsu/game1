using UnityEngine;
// Presentation only; the optional component is absent from the final boss.
[DefaultExecutionOrder(650)]public sealed class B25BossFx:MonoBehaviour
{
 public PresentationProfile profile;
 BossEncounter boss;RoomCameraRig cameraRig;
 void Awake(){boss=GetComponent<BossEncounter>();}
 void Update(){if(cameraRig==null)cameraRig=FindFirstObjectByType<RoomCameraRig>();if(cameraRig==null||profile==null)return;float progress=boss.Introducing?Mathf.Clamp01((float)(Time.timeAsDouble-boss.PhaseStarted)/boss.introDuration):0;cameraRig.SetPresentationZoom(boss.Introducing?Mathf.Lerp(1,profile.bossIntroZoom,Mathf.Sin(progress*Mathf.PI)):1);}
 public void PrepareDefeat(){if(profile==null||boss.bossId==ProgressSave.FinalBossId)return;HitStop.Begin(profile.bossDeathFreeze);Debug.Log($"MEASURE B25 boss finish id={boss.bossId} requested={profile.bossDeathFreeze:F3}");}
 void OnDisable(){if(cameraRig!=null)cameraRig.SetPresentationZoom(1);}
}
