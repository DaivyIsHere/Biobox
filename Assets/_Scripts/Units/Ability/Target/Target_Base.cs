using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Target_Base
{
    public virtual List<UnitCID> GetAllTargets(Unit unit)
    {
        return new List<UnitCID>();
    }
}
