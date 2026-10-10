using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class WallJumpTestMetrics:MonoBehaviour
{
    [System.Serializable]public sealed class Zone
    {
        public string label;[Tooltip("Course left/right edges in world units.")]public Vector2 xRange;
        [Tooltip("Floor player center y, units.")]public float floorY=1.82f;
        [Tooltip("Summit player center y, units.")]public float summitY=13.8f;
        [System.NonSerialized]public bool active;
        [System.NonSerialized]public double started;
        [System.NonSerialized]public int jumps;
    }
    public Zone[] zones;
    readonly Queue<string> recent=new();
    public double LastElapsed {get;private set;}
    public int LastJumps {get;private set;}
    public string DisplayText {get;private set;}="기록 대기";
    void Update()
    {
        var player=RoomSession.Instance?.Player;if(player==null||player.InputLocked)return;
        var motor=player.GetComponent<PlayerMotor>();var p=player.GetComponent<Rigidbody2D>().position;
        foreach(var z in zones)
        {
            bool inside=p.x>=z.xRange.x&&p.x<=z.xRange.y;
            if(z.active&&(!inside||(motor.Grounded&&p.y<z.floorY+.25f))){z.active=false;}
            if(!z.active&&inside&&p.y<z.floorY+.45f&&!motor.Grounded&&player.GetComponent<Rigidbody2D>().linearVelocity.y>0)
            {z.active=true;z.started=Time.fixedUnscaledTimeAsDouble-Time.fixedDeltaTime;z.jumps=motor.WallJumpsPerformed;}
            if(z.active&&p.y>=z.summitY)
            {
                z.active=false;LastElapsed=Time.fixedUnscaledTimeAsDouble-z.started;LastJumps=motor.WallJumpsPerformed-z.jumps;
                string record=z.label+" "+LastElapsed.ToString("0.00",System.Globalization.CultureInfo.InvariantCulture)+"s / "+LastJumps+" wall jumps";
                recent.Enqueue(record);while(recent.Count>3)recent.Dequeue();DisplayText=string.Join("\n",recent);
                Directory.CreateDirectory("Logs");File.AppendAllText("Logs/walljump-runs.txt","MEASURE "+record+"\n");
            }
        }
    }
    void OnGUI()
    {float scale=Mathf.Min(Screen.width/1280f,Screen.height/720f);var old=GUI.matrix;GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));GUI.Label(new Rect(890,570,378,140),"등반 기록 (최근 3회)\n"+DisplayText,new GUIStyle(GUI.skin.label){fontSize=16});GUI.matrix=old;}
}
