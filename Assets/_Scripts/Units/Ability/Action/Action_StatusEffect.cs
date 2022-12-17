using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action_StatusEffect : Action_Base
{
    public StatusEffect statusEffect;
    
    public override void DoAction()
    {
        Debug.Log("Apply StatusEffect");
    }
}
