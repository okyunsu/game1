using System;
using System.IO;
using UnityEditor;
public static class Nightly
{
    public static string Tag { get { var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,"-nightTask");return i>=0?a[i+1]:"T0"; } }
    public static string Path(string task)=>$"Validation/{task}-night-{Tag}.txt";
    public static void Input(){if(File.Exists(Path("S1-02")))throw new IOException("Preserve evidence");SessionState.SetString("N1.Suffix","night-"+Tag);N1Verification.Input();}
    public static void Buffer(){if(File.Exists(Path("S1-04")))throw new IOException("Preserve evidence");SessionState.SetString("N1.Suffix","night-"+Tag);N1Verification.Forgiveness();}
}
