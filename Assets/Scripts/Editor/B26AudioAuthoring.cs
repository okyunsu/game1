using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class B26AudioAuthoring
{
 public static void Normalize()
 {
  var profile=AssetDatabase.LoadAssetAtPath<PresentationProfile>(B25EffectsAuthoring.ProfilePath);if(profile.soundThrottle<=0)throw new Exception("Finite sound throttle required; rule unchanged");var rows=new List<string>();double bound=0;
  foreach(var sound in profile.sounds){if(sound.clip==null)throw new Exception(sound.eventName+" missing clip");sound.clip.LoadAudioData();var samples=new float[sound.clip.samples*sound.clip.channels];if(!sound.clip.GetData(samples,0))throw new Exception("Cannot read "+sound.clip.name);float peak=0;foreach(float x in samples)peak=Mathf.Max(peak,Math.Abs(x));int voices=(int)Math.Ceiling(sound.clip.length/(Math.Max(.01,1-sound.pitchVariation)*profile.soundThrottle))+1;double contribution=peak*sound.volume*voices;bound+=contribution;rows.Add($"MEASURE event={sound.eventName} gain={sound.volume:F3} PCMpeak={peak:F6} maxVoices={voices} peakBound={contribution:F6}");}
  profile.masterVolume=.8f;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();rows.Add($"MEASURE masterGain={profile.masterVolume:F6} preLimiterWorstCaseBound={bound*profile.masterVolume:F6} filterPeakCeiling=0.85 throttle={profile.soundThrottle:F3} unchangedEventTimings=True; buffer limiter preserves ordinary gain, avoids blanket attenuation");File.WriteAllLines(N5Args.Value("-verify-output","Validation/_raw/26/audio.txt"),rows);EditorApplication.Exit(0);
 }
}
