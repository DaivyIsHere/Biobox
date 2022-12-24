using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Target_Relative : Target_Base
{
    [Space]
    [EnumToggleButtons]
    public TargetRelative_Position position;

    public override List<UnitCID> GetAllTargets(Unit unit)
    {
        List<UnitCID> targetUnits = new List<UnitCID>();
        UnitCID targetCID = BattleManager.Instance.GetRelativeUnit(unit.unitCID, position);
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
