using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitHurt : CCommand
{
    private UnitCID _targetCID;//the unit who got hurt

    public CCUnitHurt(UnitCID _targetCID)
    {
        this._targetCID = _targetCID;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayHurt();
        ///We call CommandExecutionComplete() at the end of PlayHurt().
    }
}
