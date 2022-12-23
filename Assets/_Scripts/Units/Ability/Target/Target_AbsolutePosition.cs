using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Target_AbsolutePosition : Target_Base
{
    public TargetAbsolutePosition_Position position;
    public TargetAbsolutePosition_RelativeBox relativeBox;

    public override List<UnitCID> GetAllTargets(Unit unit)
    {
        return BattleManager.Instance.GetAboslutePositionUnit(unit.unitCID, relativeBox, position);
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

