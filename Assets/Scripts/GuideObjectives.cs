using UnityEngine;

[CreateAssetMenu(menuName="Afterglow/Guide Objectives")]
public sealed class GuideObjectives : ScriptableObject
{
    public enum Condition { NoDash, NoDoubleJump, BossAlive, Complete }
    [System.Serializable] public sealed class Objective
    {
        public Condition condition;
        [TextArea,Tooltip("One line goal displayed at the top of the HUD.")] public string text;
        [Tooltip("Stable destination room ID, independent of candidate Scene path.")] public string roomId;
    }
    public Objective[] objectives;
    public int Stage(bool dash,bool jump,bool boss)
    {
        for(int i=0;i<objectives.Length;i++)
            if(objectives[i].condition==Condition.NoDash&&!dash||objectives[i].condition==Condition.NoDoubleJump&&!jump||objectives[i].condition==Condition.BossAlive&&!boss||objectives[i].condition==Condition.Complete)return i;
        return -1;
    }
}
