using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    public StatusEffectDefinition effectDefinition;
    public int stack;

    public StatusEffect(StatusEffectDefinition definition, int stack)
    {
        this.effectDefinition = definition;
        this.stack = stack;
    }
}
