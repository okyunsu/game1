using UnityEngine;
public sealed partial class BossEncounter
{
 [Tooltip("Encounter HUD name.")] public string displayName="정상 수문장";
 [Tooltip("Require wall-jump pickup before departure trigger. Default false preserves other bosses.")] public bool startsAfterWallJumpPickup;
 [Tooltip("Wall-jump pedestal centre, world x units.")] public float acquisitionX=12;
 [Min(.1f),Tooltip("Horizontal departure distance after first pickup, world units.")] public float acquisitionDistance=3;
 [Tooltip("Optional acquisition pedestal lowered to floorY when combat locks. Scene reference; other bosses leave empty.")]public Transform sinkingPedestal;
 bool pedestalCaptured;Vector3 pedestalRest;
 void LowerPedestal(){if(sinkingPedestal==null)return;if(!pedestalCaptured){pedestalCaptured=true;pedestalRest=sinkingPedestal.position;}var c=sinkingPedestal.GetComponent<Collider2D>();sinkingPedestal.position+=Vector3.up*(floorY-c.bounds.max.y);Physics2D.SyncTransforms();}
 void RestorePedestal(){if(pedestalCaptured&&sinkingPedestal!=null){sinkingPedestal.position=pedestalRest;Physics2D.SyncTransforms();}}
 bool entryOwnershipSampled,wallOwnedAtEntry;
 float CurrentWaveTell=>PatternStarts==1&&tuning.firstWaveTell>0?tuning.firstWaveTell:tuning.waveTell;
 bool WallPickupReady(PlayerInputReader player)
 {
  bool owned=player.GetComponent<PlayerMotor>().hasWallJump;
  if(!entryOwnershipSampled){entryOwnershipSampled=true;wallOwnedAtEntry=owned;}
  return owned&&(wallOwnedAtEntry?player.transform.position.x>=acquisitionX:Mathf.Abs(player.transform.position.x-acquisitionX)>=acquisitionDistance);
 }
 public void ResetAfterArenaDeath(Vector3 spawn)
 {
  EndIntro();ClearFallingWarnings();ClearB3Hazards();if(wave!=null)Destroy(wave.gameObject);foreach(var live in candidateWaves)if(live!=null)Destroy(live.gameObject);candidateWaves.Clear();wave=null;
  HP=tuning.maxHP;Hits=WavesSpawned=PatternStarts=PhaseTransitions=0;PhaseTwo=false;deathQueued=resolvingDeath=false;damagedPatterns.Clear();pattern=attackToken=0;entryOwnershipSampled=false;
  MoveBoss(spawn);GetComponent<BoxCollider2D>().isTrigger=false;SetLock(false);RestorePedestal();SetPhase(Phase.Idle);
 }
}
