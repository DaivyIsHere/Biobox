using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStatusEffects
{
    [SerializeField] public Dictionary<StatusEffectDefinition, StatusEffect> _effects;
    public Dictionary<StatusEffectDefinition, StatusEffect> effects => _effects;

    public UnitStatusEffects()
    {
        _effects = new Dictionary<StatusEffectDefinition, StatusEffect>();
    }

    public void RemoveStatusEffect(StatusEffectDefinition effectDefinition)
    {
        //if exsited, remove
    }

    public void ApplyStatusEffect(StatusEffectDefinition effectDefinition, int stackValue)
    {
        //if exsited, stack
        //else add
    }

    public void ChangeEffectStack(StatusEffectDefinition effectDefinition, int stackChangeValue)
    {
        //if exsited, change stack
    }
}
