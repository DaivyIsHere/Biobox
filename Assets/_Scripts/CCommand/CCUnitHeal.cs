using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitHeal : CCommand
{
    private UnitCID _targetCID;
    private int _healValue;
    private int _healthAfter;

    public CCUnitHeal(UnitCID _targetCID, int _healValue, int _healthAfter)
    {
        this._targetCID = _targetCID;
        this._healValue = _healValue;
        this._healthAfter = _healthAfter;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayHeal(_healValue, _healthAfter);
        //CCommand.CommandExecutionComplete();
    }
}
