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
        int unitStatValue = unit.unitCombat.GetStatValue(statDefinition);
        switch (comparison)
        {
            case ConditionStats_Comparison.Greater:
                return unitStatValue > compareValue;
            case ConditionStats_Comparison.Equal:
                return unitStatValue == compareValue;
            case ConditionStats_Comparison.Less:
                return unitStatValue < compareValue;
            case ConditionStats_Comparison.EqualOrGreater:
                return unitStatValue >= compareValue;
            case ConditionStats_Comparison.EqualOrLess:
                return unitStatValue <= compareValue;
        }

        return true;
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
