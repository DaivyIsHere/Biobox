using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatModifier
{
    public float value;
    public StatModType type;
    public int order;
    public object source;

    public StatModifier(float value, StatModType type, int order, object source)
    {
        this.value = value;
        this.type = type;
        this.order = order;
        this.source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) {}

    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) {}

    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) {}
}

public enum StatModType
{
    Additive,
    PercentAdd,
    PercentMult,
    Override
}
