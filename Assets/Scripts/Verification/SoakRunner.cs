using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public sealed class SoakRunner:N5Runner
{
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Init()=>N5Args.Start<SoakRunner>("-verify-soak");
 protected override string Evidence=>N5Args.Value("-verify-output","Validation/N5-TE-soak.txt");
 double start,nextSample;int transitions,deaths,pauses;long baseMemory,lastMemory;int baseObjects,lastObjects;string csv;bool sampling;
 [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]struct MemoryCounters{public uint size,faults;public UIntPtr peak,working,pagedPeak,paged,nonPagedPeak,nonPaged,pagefile,pagefilePeak;}
 [System.Runtime.InteropServices.DllImport("psapi.dll")]static extern bool GetProcessMemoryInfo(IntPtr handle,out MemoryCounters counters,uint size);
 long WorkingSet(){var p=Process.GetCurrentProcess();p.Refresh();long bytes=p.WorkingSet64;if(bytes==0&&GetProcessMemoryInfo(p.Handle,out var c,(uint)System.Runtime.InteropServices.Marshal.SizeOf<MemoryCounters>()))bytes=(long)c.working.ToUInt64();return bytes;}
 void Update(){if(!sampling)return;double elapsed=Time.realtimeSinceStartupAsDouble-start;if(elapsed<nextSample)return;long mem=WorkingSet();int objects=FindObjectsByType<GameObject>(FindObjectsSortMode.None).Length;int shots=FindObjectsByType<EnemyShot>(FindObjectsSortMode.None).Length;File.AppendAllText(csv,$"{elapsed:F3},{mem},{objects},{shots},{transitions},{deaths},{pauses},{errors.Count}\n");if(nextSample==600){baseMemory=mem;baseObjects=objects;}lastMemory=mem;lastObjects=objects;nextSample+=60;}
 protected override void Cleanup(){sampling=false;}
 protected override IEnumerator Tests(){int minutes=N5Args.Int("-verify-soak",60);Check(minutes>=1,"positive real soak duration");csv=N5Args.Value("-verify-csv","Validation/N5-TE-soak.csv");if(File.Exists(csv))throw new IOException("Preserve CSV");File.WriteAllText(csv,"elapsed_s,working_set_bytes,gameobjects,enemyshots,transitions,deaths,pause,error_exception\n");yield return ContinueGame();start=Time.realtimeSinceStartupAsDouble;nextSample=60;sampling=true;
  // Successful graph route, reversed at C04. Each exit setup is explicitly authorized.
  string[] route={"A01","A02","A03","A04","A05","A04","A03","B01","B02","B03","B04","B03","B02","B01","B05","C01","C02","C03","C04","C03","C02","C01","B05","B01","A03","A02","A01"};int index=1,cycles=0;
  while(Time.realtimeSinceStartupAsDouble-start<minutes*60){string current=RoomSession.Instance.CurrentRoomId;string destination=route[index];if(current==destination){index=(index+1)%route.Length;continue;}var exit=Array.Find(FindObjectsByType<RoomExit>(FindObjectsSortMode.None),e=>e.enabled&&e.destinationRoomId==destination);if(exit==null){index=Array.IndexOf(route,current)+1;if(index>=route.Length)index=0;continue;}
   yield return PhysicalDoor(destination);transitions++;index=(index+1)%route.Length;
   yield return Tap(Key.Escape);Check(Player.Paused,"soak Pause "+pauses);pauses++;yield return Tap(Key.Escape);Check(!Player.Paused,"soak resume");yield return Tap(Key.X);
   if(transitions%5==0&&FindFirstObjectByType<KillZone>()!=null){var kill=FindFirstObjectByType<KillZone>();Check(kill!=null,"physical KillZone setup");PlaceForAuthorizedSetup(kill.GetComponent<Collider2D>().bounds.center);yield return Until(()=>RoomSession.Instance.Respawning,"physical KillZone death");deaths++;yield return Ready();index=Array.IndexOf(route,RoomSession.Instance.CurrentRoomId)+1;if(index>=route.Length)index=0;}
   cycles++;yield return new WaitForSecondsRealtime(2);
  }
  // Wait through the exact final minute sample, even if the last route action crosses it.
  Update();sampling=false;lines.Add($"elapsed={Time.realtimeSinceStartupAsDouble-start:F3}s transitions={transitions} deaths={deaths} Pause={pauses} samples={nextSample/60-1} CSV={csv}");Check(Time.realtimeSinceStartupAsDouble-start>=minutes*60,"actual requested wall duration elapsed");if(minutes>=60){Check(baseMemory>0&&baseObjects>0,"10-minute baseline exists");Check(lastMemory<=baseMemory*1.15&&lastObjects<=baseObjects*1.15,$"memory/objects <=+15% baseline={baseMemory}/{baseObjects} final={lastMemory}/{lastObjects}");Check(transitions>=(minutes>=120?500:300)&&deaths>=(minutes>=120?100:50)&&pauses>=(minutes>=120?200:100),"required action totals");}
 }
}
