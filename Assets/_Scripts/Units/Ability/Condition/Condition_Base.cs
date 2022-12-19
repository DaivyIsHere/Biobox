using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition_Base
{
    public virtual bool ConditionMet(Unit unit)
    {
        return true;
    }
}



