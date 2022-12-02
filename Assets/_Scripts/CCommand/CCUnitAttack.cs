using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCUnitAttack : CCommand
{
    private UnitCID _targetCID;//CID of the attacker
    
    public CCUnitAttack(UnitCID attackerCID)
    {
        this._targetCID = attackerCID;
    }

    public override void StartCommandExecution()
    {
        _targetCID.GetUnit().unitAnimation.PlayAttack();
        CommandExecutionComplete();
    }
}
