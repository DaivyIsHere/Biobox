using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitTakeDamage : CCommand
{
    private UnitCID _targetCID;//the unit who took this damage
    private int _damageValue;
    private int _healthAfter;

    public CCUnitTakeDamage(UnitCID _targetCID, int damageValue, int healthAfter)
    {
        this._targetCID = _targetCID;
        this._damageValue = damageValue;
        this._healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayTakeDamage(_damageValue , _healthAfter);
        CommandExecutionComplete();
    }
}
