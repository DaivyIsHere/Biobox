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
        UnitCID targetCID = BattleManager.Instance.GetRelativeUnit(unit.unitCID, relativePosition);
        if(targetCID.GetUnit())
            targetUnits.Add(targetCID.GetUnit().unitCID);
        return targetUnits;
    }
}

public enum TargetRelative_Position
{
    Self,
    Front,
    Behind,
}
