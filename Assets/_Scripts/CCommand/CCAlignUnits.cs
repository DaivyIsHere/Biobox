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
        CombatManager.Instance.StartCoroutine(CombatManager.Instance.AlignAllUnitsCommand(_boxSide));
    }
}
