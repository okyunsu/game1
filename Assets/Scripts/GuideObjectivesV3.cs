using UnityEngine;

[CreateAssetMenu(menuName="Afterglow/V3 Guide Objectives")]
public sealed class GuideObjectivesV3 : ScriptableObject
{
    public GuideObjectives.Objective[] objectives;
    public int Stage(bool dash, bool wall, bool jump, bool boss, bool ending, bool b01, bool b05, bool c01)
    {
        if(ending) return 8;
        if(boss) return 7;
        if(!dash) return 0;
        if(!b01) return 1;
        if(!wall) return 2;
        if(!b05) return 3;
        if(!c01) return 4;
        return !jump ? 5 : 6;
    }
}
