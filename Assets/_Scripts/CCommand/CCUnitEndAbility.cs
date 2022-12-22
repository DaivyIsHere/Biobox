using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitEndAbility : CCommand
{
    private UnitCID _targetCID;

    public CCUnitEndAbility(UnitCID _targetCID)
    {
        this._targetCID = _targetCID;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayEndAbility();
    }
}
