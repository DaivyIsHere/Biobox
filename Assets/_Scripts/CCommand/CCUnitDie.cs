using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitDie : CCommand
{
    private UnitCID _targetCID;

    public CCUnitDie(UnitCID _targetCID)
    {
        this._targetCID = _targetCID;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayDeath();
    }
}
