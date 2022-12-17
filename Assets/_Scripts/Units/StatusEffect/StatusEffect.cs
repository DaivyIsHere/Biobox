using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    public StatusEffectDefinition definition;
    public int stack;

    public StatusEffect(StatusEffectDefinition definition)
    {
        this.definition = definition;
        this.stack = 1;
    }

    public StatusEffect(StatusEffectDefinition definition, int stack) : this(definition)
    {
        this.stack = stack;
    }
}
