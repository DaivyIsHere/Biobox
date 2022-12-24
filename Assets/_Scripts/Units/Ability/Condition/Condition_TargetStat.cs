using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Condition_TargetStat : Condition_Base
{
    [PropertyOrder(99)]
    [TabGroup("Target")]
    [ValueDropdown("targetList")]
    [SerializeReference] public Target_Base target;
    private List<Target_Base> targetList = new List<Target_Base>
    {
        new Target_Relative(),
        new Target_AbsolutePosition()
    };

    [TabGroup("StatMod")]
    [AssetSelector]
    public StatDefinition statDefinition;
    [TabGroup("StatMod")]
    public Condition_Comparison comparison;
    [TabGroup("StatMod")]
    public int compareValue;

    public override bool ConditionMet(Unit unit)
    {
        bool result = true;
        List<UnitCID> targetUnits = target.GetAllTargets(unit);
        //we only care about the first target
        int unitStatValue = targetUnits[0].GetUnit().unitBattle.GetStatValue(statDefinition);
        switch (comparison)
        {
            case Condition_Comparison.Greater:
                result = unitStatValue > compareValue;
                break;
            case Condition_Comparison.Equal:
                result = unitStatValue == compareValue;
                break;
            case Condition_Comparison.Less:
                result = unitStatValue < compareValue;
                break;
            case Condition_Comparison.EqualOrGreater:
                result = unitStatValue >= compareValue;
                break;
            case Condition_Comparison.EqualOrLess:
                result = unitStatValue <= compareValue;
                break;
        }

        Debug.Log("<Condition> SelfStat of " + statDefinition.name + " is " + comparison.ToString() + " than " + compareValue + " , result : " + result);
        return result;
    }
}

public enum Condition_Comparison
{
    Greater,
    Equal,
    Less,
    EqualOrGreater,
    EqualOrLess
}
