using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitStartAbility : CCommand
{
    private UnitCID _targetCID;

    public CCUnitStartAbility(UnitCID _targetCID)
    {
        this._targetCID = _targetCID;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayStartAbility();
    }
}
