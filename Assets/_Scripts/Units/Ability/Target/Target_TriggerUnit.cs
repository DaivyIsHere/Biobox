using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_TriggerUnit : Target_Base
{
    public override List<UnitCID> GetAllTargets(Unit unit)
    {
        List<UnitCID> targetUnits = new List<UnitCID>();
        targetUnits.Add(unit.unitBattle.triggerUnit);
        return targetUnits;
    }
}
