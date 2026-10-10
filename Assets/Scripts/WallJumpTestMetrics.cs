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
    GUIStyle labelStyle;Vector2 previousPosition;double previousTime;int previousPlayer,previousJumps;bool hasPrevious;
    string labelText="등반 기록 (최근 3회)\n기록 대기";
    public double LastElapsed {get;private set;}
    public int LastJumps {get;private set;}
    public string DisplayText {get;private set;}="기록 대기";
    void Update()
    {
        var player=RoomSession.Instance?.Player;if(player==null||player.InputLocked){hasPrevious=false;return;}
        var motor=player.GetComponent<PlayerMotor>();var p=player.GetComponent<Rigidbody2D>().position;
        double now=Time.fixedUnscaledTimeAsDouble;bool samePlayer=hasPrevious&&previousPlayer==player.GetInstanceID();
        foreach(var z in zones)
        {
            bool inside=p.x>=z.xRange.x&&p.x<=z.xRange.y;
            if(z.active&&(!inside||(motor.Grounded&&p.y<z.floorY+.25f))){z.active=false;}
            float startY=z.floorY+.02f;
            if(!z.active&&inside&&samePlayer&&previousPosition.x>=z.xRange.x&&previousPosition.x<=z.xRange.y&&previousPosition.y<=startY&&p.y>startY&&!motor.Grounded)
            {z.active=true;float fraction=(startY-previousPosition.y)/(p.y-previousPosition.y);z.started=previousTime+(now-previousTime)*fraction;z.jumps=previousJumps;}
            if(z.active&&p.y>=z.summitY)
            {
                z.active=false;LastElapsed=Time.fixedUnscaledTimeAsDouble-z.started;LastJumps=motor.WallJumpsPerformed-z.jumps;
                string record=z.label+" "+LastElapsed.ToString("0.00",System.Globalization.CultureInfo.InvariantCulture)+"s / "+LastJumps+" wall jumps";
                recent.Enqueue(record);while(recent.Count>3)recent.Dequeue();DisplayText=string.Join("\n",recent);
                labelText="등반 기록 (최근 3회)\n"+DisplayText;
                Directory.CreateDirectory("Logs");File.AppendAllText("Logs/walljump-runs.txt","MEASURE "+record+"\n");
            }
        }
        previousPosition=p;previousTime=now;previousPlayer=player.GetInstanceID();previousJumps=motor.WallJumpsPerformed;hasPrevious=true;
    }
    void OnGUI()
    {labelStyle??=new GUIStyle(GUI.skin.label){fontSize=16};float scale=Mathf.Min(Screen.width/1280f,Screen.height/720f);var old=GUI.matrix;GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));GUI.Label(new Rect(890,570,378,140),labelText,labelStyle);GUI.matrix=old;}
}
