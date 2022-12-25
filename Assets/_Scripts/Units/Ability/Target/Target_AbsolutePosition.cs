using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Target_AbsolutePosition : Target_Base
{
    [Space]
    [EnumToggleButtons]
    public TargetAbsolutePosition_Position position;
    [Space]
    [EnumToggleButtons]
    public TargetAbsolutePosition_RelativeBox box;
    [Space]
    public int targetCount = 1;

    public override List<UnitCID> GetAllTargets(Unit unit)
    {
        return BattleManager.Instance.GetAboslutePositionUnit(unit.unitCID, box, position, targetCount);
    }
}

public enum TargetAbsolutePosition_RelativeBox
{
    SelfBox,
    OppositeBox
}

public enum TargetAbsolutePosition_Position
{
    All,
    First, //font most
    Last, 
    Random,
}

