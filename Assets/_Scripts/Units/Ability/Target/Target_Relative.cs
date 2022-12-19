using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Target_Relative : Target_Base
{
    public TargetRelative_Position relativePosition;

    public override List<UnitCID> GetAllTargets(Unit unit)
    {
        List<UnitCID> targetUnits = new List<UnitCID>();
        targetUnits.Add(BattleManager.Instance.GetRelativeUnit(unit.unitCID, relativePosition));
        return targetUnits;
    }
}

public enum TargetRelative_Position
{
    Self,
    Front,
    Behind,
}
