using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class Trigger_Base
{
    public virtual void RegisterTrigger(Unit selfUnit, Action<Unit> onTriggerAbility)
    {
        
    }

    public virtual void UnregisterTrigger(Unit selfUnit, Action<Unit> onTriggerAbility)
    {
        
    } 

    public virtual bool CheckTrigger(UnitLabel selflabel, UnitLabel triggererLabel, Trigger_Base trigger)
    {
        return false;
    }
}
