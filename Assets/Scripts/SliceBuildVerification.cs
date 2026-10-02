using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Opt-in standalone build checks. No scene component or ordinary-play behavior.
public sealed class SliceBuildVerification : MonoBehaviour
{
    static string output;
    readonly List<string> results=new(), errors=new();
    readonly List<InputDevice> devices=new();
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if(Application.isEditor)return;
        var args=Environment.GetCommandLineArgs();
        for(int i=0;i<args.Length-1;i++)if(args[i]=="-slice-v1-verify")
        {
            output=Path.GetFullPath(args[i+1]);
            if(File.Exists(output)){Application.Quit(2);return;}
            var go=new GameObject("Slice build verification (command line only)");
            DontDestroyOnLoad(go);go.AddComponent<SliceBuildVerification>();return;
        }
    }
    void Awake()=>Application.logMessageReceived+=Log;
    void Log(string message,string stack,LogType type){if(type is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(message+stack);}
    void Check(bool ok,string label){if(!ok)throw new Exception(label);results.Add("PASS: "+label);}
    IEnumerator Start()
    {
        foreach(var device in InputSystem.devices)if(device.enabled){devices.Add(device);InputSystem.DisableDevice(device);}
        results.Add("NOTE: exe trigger/respawn checks use direct actor placement; no heuristic traversal. Physical input disabled only for this command-line run.");
        var stack=new Stack<IEnumerator>();stack.Push(Checks());bool fail=false;
        while(stack.Count>0&&!fail)
        {
            object next=null;
            try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}
            catch(Exception e){results.Add("FAIL: "+e);fail=true;}
            if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;
        }
        Application.logMessageReceived-=Log;fail|=errors.Count>0;results.AddRange(errors);
        results.Add("MEASURE runtime Error/Exception/Assert="+errors.Count);
        results.Add("FAIL "+(fail?1:0));
        results.Insert(0,(fail?"FAIL":"PASS")+" Slice v1 exe; Unity="+Application.unityVersion+"; UTC="+DateTime.UtcNow.ToString("O"));
        foreach(var device in devices)if(device.added)InputSystem.EnableDevice(device);
        Directory.CreateDirectory(Path.GetDirectoryName(output));File.WriteAllLines(output,results);Application.Quit(fail?1:0);
    }
    void Place(Vector2 position)
    {
        var player=RoomSession.Instance.Player;var body=player.GetComponent<Rigidbody2D>();
        player.ClearTransientInput();player.transform.position=position;body.position=position;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();
    }
    IEnumerator Until(Func<bool> condition,string message)
    {
        double end=Time.realtimeSinceStartupAsDouble+5;
        while(!condition()){if(Time.realtimeSinceStartupAsDouble>end)throw new Exception("Timeout: "+message);yield return null;}
    }
    RoomDefinition Room()=>FindFirstObjectByType<RoomDefinition>();
    IEnumerator TouchCheckpoint(string id)
    {
        Checkpoint checkpoint=null;foreach(var cp in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))if(cp.checkpointId==id)checkpoint=cp;
        Check(checkpoint!=null,"checkpoint exists "+id);Place(checkpoint.transform.position);
        yield return Until(()=>RoomSession.Instance.CheckpointId==id,"physical checkpoint contact "+id);
        Check(RoomSession.Instance.CheckpointId==id,"physical contact activates "+id);
    }
    IEnumerator ExitTo(string destination)
    {
        var session=RoomSession.Instance;string previous=session.CurrentRoomId;RoomExit exit=null;
        foreach(var candidate in FindObjectsByType<RoomExit>(FindObjectsSortMode.None))if(candidate.destinationRoomId==destination)exit=candidate;
        Check(exit!=null,"authored exit "+previous+"->"+destination);
        string spawnId=exit.destinationSpawnId;Vector2 trigger=exit.GetComponent<BoxCollider2D>().bounds.center;
        double started=Time.realtimeSinceStartupAsDouble;Place(trigger);
        yield return Until(()=>session.CurrentRoomId==destination&&!session.Transitioning,"exit trigger to "+destination);
        Check(Room().TryGetSpawn(spawnId,out var spawn),"arrival spawn ID "+destination+"/"+spawnId);
        var actual=session.Player.GetComponent<Rigidbody2D>().position;var expected=(Vector2)spawn.transform.position;
        results.Add($"MEASURE {previous}->{destination}: trigger={trigger}, spawn={spawnId}, expected={expected}, actual={actual}, elapsed={Time.realtimeSinceStartupAsDouble-started:F3}s");
        Check(Vector2.Distance(actual,expected)<.1f,"arrival matches authored spawn "+destination);
        Check(FindObjectsByType<PlayerInputReader>(FindObjectsSortMode.None).Length==1,"single player after "+destination);
    }
    IEnumerator DeathReturn(string roomId,string checkpointId,Vector2 expected, bool fall)
    {
        double started=Time.realtimeSinceStartupAsDouble;
        if(fall)
        {
            var zone=FindFirstObjectByType<KillZone>();Check(zone!=null,"authored KillZone exists in "+RoomSession.Instance.CurrentRoomId);
            var bounds=zone.GetComponent<BoxCollider2D>().bounds;
            Place(new Vector2(bounds.center.x,Mathf.Max(bounds.max.y+2,3)));
        }
        else
        {
            results.Add("NOTE: A03 has no authored KillZone; invoke existing RoomSession.Die after physical CP-A03 contact. No hazard or scene change.");
            Check(RoomSession.Instance.Die(),"A03 existing death flow requested");
        }
        yield return Until(()=>RoomSession.Instance.Respawning,"death flow started");
        Check(RoomSession.Instance.Player.InputLocked,"death locks input");
        yield return Until(()=>!RoomSession.Instance.Respawning&&RoomSession.Instance.CurrentRoomId==roomId,"checkpoint return");
        var actual=RoomSession.Instance.Player.GetComponent<Rigidbody2D>().position;
        results.Add($"MEASURE death return checkpoint={checkpointId}, room={roomId}, expected={expected}, actual={actual}, elapsed={Time.realtimeSinceStartupAsDouble-started:F3}s; fall={fall}");
        Check(RoomSession.Instance.CheckpointId==checkpointId&&Vector2.Distance(actual,expected)<.12f,"return to "+checkpointId+" spawn");
        Check(!RoomSession.Instance.Player.InputLocked,"input restored after return");
    }
    IEnumerator Checks()
    {
        yield return Until(()=>RoomSession.Instance?.Player!=null&&!string.IsNullOrEmpty(RoomSession.Instance.CurrentRoomId),"initial room");
        Check(SceneManager.GetActiveScene().name=="A01"&&RoomSession.Instance.CurrentRoomId=="A01","exe starts in A01");
        Check(SceneManager.sceneCountInBuildSettings==4,"build contains exactly four scenes");
        for(int i=0;i<4;i++){string expected=$"Assets/Scenes/A0{i+1}.unity";string actual=SceneUtility.GetScenePathByBuildIndex(i);results.Add($"MEASURE build scene[{i}]={actual}");Check(actual==expected,"ordered build scene "+expected);}
        foreach(string scene in new[]{"DashTest","CombatTest","MovementTest"})Check(!Application.CanStreamedLevelBeLoaded(scene),"test scene excluded "+scene);
        Check(Room().TryGetSpawn("Entry",out var entry),"initial Entry exists");
        results.Add($"MEASURE A01 initial player={RoomSession.Instance.Player.transform.position}; Entry={entry.transform.position}");
        Check(Vector2.Distance(RoomSession.Instance.Player.transform.position,entry.transform.position)<.1f,"initial player at A01 Entry");
        yield return TouchCheckpoint("CP-A01");
        foreach(string destination in new[]{"A02","A03","A04","A03","A02","A01"})yield return ExitTo(destination);
        var cp=FindFirstObjectByType<Checkpoint>();Vector2 cp01=cp.spawn.transform.position;
        yield return ExitTo("A02");yield return DeathReturn("A01","CP-A01",cp01,true);
        yield return ExitTo("A02");yield return ExitTo("A03");yield return TouchCheckpoint("CP-A03");
        cp=FindFirstObjectByType<Checkpoint>();Vector2 cp03=cp.spawn.transform.position;
        yield return DeathReturn("A03","CP-A03",cp03,false);
        yield return new WaitForSecondsRealtime(.2f);
    }
}
