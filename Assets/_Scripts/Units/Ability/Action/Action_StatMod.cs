using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action_StatMod : Action_Base
{
    public StatDefinition statDefinition;
    public StatModifier statModifier;

    public override void DoAction()
    {
        Debug.Log("Apply StatMod : "+ statDefinition.name);
    }
}
