using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAlignUnits : CCommand
{
    private BoxSide _boxSide;

    public CCAlignUnits(BoxSide boxSide)
    {
        _boxSide = boxSide;
    }

    public override void StartCommandExecution()
    {
        BattleManager.Instance.StartCoroutine(BattleManager.Instance.AlignAllUnitsCommand(_boxSide));
    }
}
