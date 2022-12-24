using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Action_StatMod : Action_Base
{
    [TabGroup("StatMod")]
    [AssetSelector]
    public StatDefinition statDefinition;
    [TabGroup("StatMod")]
    public StatModifier statModifier;

    public override void DoAction(Unit unit)
    {
        List<UnitCID> targetUnits = target.GetAllTargets(unit);
        foreach (var u in targetUnits)
        {
            u.GetUnit().unitBattle.TakeStatModifier(statDefinition, statModifier);
            Debug.Log("Apply StatMod : " + statDefinition.name + "Value : " + statModifier.value);
        }

    }
}
