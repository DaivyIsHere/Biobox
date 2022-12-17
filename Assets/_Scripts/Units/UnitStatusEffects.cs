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
        if (HasEffect(effectDefinition))
        {
            _effects.Remove(effectDefinition);
        }
    }

    public void ApplyStatusEffect(StatusEffect effectToApply)
    {
        if (HasEffect(effectToApply.definition))
        {
            if (effectToApply.definition.isStackable)
            {
                _effects[effectToApply.definition].stack += effectToApply.stack;
                if (_effects[effectToApply.definition].stack <= 0)
                {
                    RemoveStatusEffect(effectToApply.definition);
                }
            }
        }
        else
        {
            if (effectToApply.definition.isStackable)
            {
                if (effectToApply.stack > 0)
                    _effects.Add(effectToApply.definition, new StatusEffect(effectToApply.definition, effectToApply.stack));
            }
            else
            {
                _effects.Add(effectToApply.definition, new StatusEffect(effectToApply.definition, 1));
            }
        }
    }

    public int GetStack(StatusEffectDefinition effectDefinition)
    {
        if (!HasEffect(effectDefinition))
        {
            return 0;
        }
        else
        {
            return _effects[effectDefinition].stack;
        }
    }

    public bool HasEffect(StatusEffectDefinition effectDefinition)
    {
        if (_effects.ContainsKey(effectDefinition))
        {
            if (_effects[effectDefinition].stack > 0)
                return true;
        }

        return false;
    }
}
