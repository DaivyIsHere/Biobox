using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class Trigger_Base
{
    
    public virtual void RegisterTrigger(Unit unit, Action<Unit> onTriggerAbility)
    {
        
    } 
}
