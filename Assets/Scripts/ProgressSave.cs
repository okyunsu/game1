using System;
using System.IO;
using UnityEngine;
[Serializable]
public sealed class ProgressData { public int version=1; public bool dash; public bool doubleJump; public string checkpointId; }
public static class ProgressSave
{
 public static bool Enabled;
 public static ProgressData Pending;
 public static string Message { get; private set; }
 public static string FilePath { get {var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-save-path");return i>=0&&i+1<args.Length?Path.GetFullPath(args[i+1]):Path.Combine(Application.persistentDataPath,"progress.json");} }
 public static bool TryLoad(out ProgressData data)
 {
  data=null;Message="";if(!File.Exists(FilePath))return false;
  try{data=JsonUtility.FromJson<ProgressData>(File.ReadAllText(FilePath));if(data==null||data.version!=1||data.checkpointId is not ("CP-A01" or "CP-A03" or "CP-B01" or "CP-C01"))throw new InvalidDataException("Unsupported save data");return true;}
  catch(Exception e) when(e is IOException or UnauthorizedAccessException or ArgumentException){data=null;Message="저장 파일을 읽을 수 없습니다. 새 게임을 선택하세요.";return false;}
 }
 public static bool Write(ProgressData data)
 {
  try{Directory.CreateDirectory(Path.GetDirectoryName(FilePath));string temporary=FilePath+".tmp";File.WriteAllText(temporary,JsonUtility.ToJson(data,true));if(File.Exists(FilePath))File.Replace(temporary,FilePath,null);else File.Move(temporary,FilePath);Message="";return true;}
  catch(Exception e) when(e is IOException or UnauthorizedAccessException){Message="저장 실패. 다음 체크포인트/능력 획득에서 다시 저장합니다.";return false;}
 }
 public static void SaveCurrent(RoomSession session){if(Enabled)Write(new ProgressData{dash=session.Player.GetComponent<PlayerDash>().hasDash,doubleJump=session.Player.GetComponent<PlayerMotor>().hasDoubleJump,checkpointId=session.CheckpointId??"CP-A01"});}
 public static bool NewGame()
 {
  if(File.Exists(FilePath)&&!TryLoad(out _))
  {
   try{File.Move(FilePath,FilePath+".invalid-"+DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff"));}
   catch(Exception e) when(e is IOException or UnauthorizedAccessException){Message="손상 저장 보존 실패. 기존 파일을 유지합니다.";return false;}
  }
  Pending=new ProgressData{dash=false,checkpointId="CP-A01"};Enabled=true;Write(Pending);return true;
 }
 public static bool Continue(){if(!TryLoad(out var data))return false;Pending=data;Enabled=true;return true;}
 public static string PendingScene=>Pending?.checkpointId=="CP-C01"?"Assets/Scenes/C01.unity":Pending?.checkpointId=="CP-B01"?"Assets/Scenes/B01.unity":Pending?.checkpointId=="CP-A03"?"Assets/Scenes/A03.unity":"Assets/Scenes/A01.unity";
}
