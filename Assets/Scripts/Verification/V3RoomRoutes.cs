#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;

// Scene-authored verification itinerary, never controls normal gameplay.
public sealed class V3RoomRoutes:MonoBehaviour
{
 public enum Action { Walk,Jump,DoubleJump,Climb,Dash,DashAir,GroundDash,Pogo,Wall,Boss,WalkUnder }
 [System.Serializable]public struct Point {public Vector2 position;public Action action;public string id;}
 [System.Serializable]public sealed class Route
 {
  public string name,startSpawn="Entry",goal;
  public bool dash,wallJump,doubleJump;
  public Point[] points;
 }
 public Route[] routes;
 public string beats,optionalType;
 public int fallPits,enemyCount;
 public Vector2 dimensions;
}
#endif
