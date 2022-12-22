using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitHealthChange : CCommand
{
    private UnitCID _targetCID;
    private int _changeValue;
    private int _valueAfter;

    public CCUnitHealthChange(UnitCID _targetCID, int _changeValue, int _valueAfter)
    {
        this._targetCID = _targetCID;
        this._changeValue = _changeValue;
        this._valueAfter = _valueAfter;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayHealthChange(_changeValue, _valueAfter);
    }
}
