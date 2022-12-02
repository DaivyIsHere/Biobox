using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitTakeDamage : CCommand
{
    private UnitCID _targetCID;
    private int _damageValue;
    private int _healthAfter;
    private bool _killed;//if the unit is killed after this damage resolved

    public CCUnitTakeDamage(UnitCID _targetCID, int damageValue, int healthAfter , bool killed)
    {
        this._targetCID = _targetCID;
        this._damageValue = damageValue;
        this._healthAfter = healthAfter;
        this._killed = killed;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayTakeDamage(_damageValue , _healthAfter, _killed);
        ///We call CommandExecutionComplete() at the end of playTakeDamage.
    }
}
