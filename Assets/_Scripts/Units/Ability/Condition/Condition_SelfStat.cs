using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition_SelfStat : Condition_Base
{
    public StatDefinition statDefinition;
    public Condition_Comparison comparison;
    public int compareValue;

    public override bool ConditionMet(Unit unit)
    {
        bool result = true;
        int unitStatValue = unit.unitBattle.GetStatValue(statDefinition);
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

       Debug.Log("<Condition> SelfStat of " + statDefinition.name + " is " + comparison.ToString() + " than " + compareValue + " , result : "+ result);
        return result;
    }
}

// public enum ConditionStats_Comparison
// {
//     Greater,
//     Equal,
//     Less,
//     EqualOrGreater,
//     EqualOrLess
// }
