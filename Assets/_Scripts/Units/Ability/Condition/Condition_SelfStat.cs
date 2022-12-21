using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition_SelfStat : Condition_Base
{
    public StatDefinition statDefinition;
    public ConditionStats_Comparison comparison;
    public int compareValue;

    public override bool ConditionMet(Unit unit)
    {
        bool result = true;
        int unitStatValue = unit.unitBattle.GetStatValue(statDefinition);
        switch (comparison)
        {
            case ConditionStats_Comparison.Greater:
                result = unitStatValue > compareValue;
                break;
            case ConditionStats_Comparison.Equal:
                result = unitStatValue == compareValue;
                break;
            case ConditionStats_Comparison.Less:
                result = unitStatValue < compareValue;
                break;
            case ConditionStats_Comparison.EqualOrGreater:
                result = unitStatValue >= compareValue;
                break;
            case ConditionStats_Comparison.EqualOrLess:
                result = unitStatValue <= compareValue;
                break;
        }

       Debug.Log("<Condition> SelfStat of " + statDefinition.name + " is " + comparison.ToString() + " than " + compareValue + " , result : "+ result);
        return result;
    }
}

public enum ConditionStats_Comparison
{
    Greater,
    Equal,
    Less,
    EqualOrGreater,
    EqualOrLess
}
