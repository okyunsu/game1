using UnityEngine;
// Audio-source filter: ordinary samples keep unity gain; only peaks need headroom.
public sealed class B26AudioHeadroom:MonoBehaviour
{
 public float InputPeak{get;private set;}public float OutputPeak{get;private set;}public int Blocks{get;private set;}public int ClippedSamples{get;private set;}
 public static float Limit(float[] data,out float before,out int clipped)
 {
  before=0;foreach(float x in data)before=Mathf.Max(before,Mathf.Abs(x));float gain=before>.85f?.85f/before:1,after=0;clipped=0;
  for(int i=0;i<data.Length;i++){data[i]*=gain;after=Mathf.Max(after,Mathf.Abs(data[i]));if(Mathf.Abs(data[i])>=1)clipped++;}return after;
 }
 void OnAudioFilterRead(float[] data,int channels){float after=Limit(data,out float before,out int clipped);InputPeak=Mathf.Max(InputPeak,before);OutputPeak=Mathf.Max(OutputPeak,after);ClippedSamples+=clipped;Blocks++;}
}
