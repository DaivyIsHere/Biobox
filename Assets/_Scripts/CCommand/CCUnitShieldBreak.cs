using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitShieldDecrease : CCommand
{
    private UnitCID _targetCID;
    private int _shieldBreakValue;
    private int _shieldAfter;

    public CCUnitShieldDecrease(UnitCID _targetCID, int shieldBreakValue, int shieldAfter)
    {
        this._targetCID = _targetCID;
        this._shieldBreakValue = shieldBreakValue;
        this._shieldAfter = shieldAfter;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayShieldBreak(_shieldBreakValue, _shieldAfter);
        CCommand.CommandExecutionComplete();
    }
}
