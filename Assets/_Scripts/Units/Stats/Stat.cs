using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Stat
{
    [Tooltip("Definition")]
    public StatDefinition statDefinition;

    [Tooltip("Base value")]
    [SerializeField] protected int _baseValue;
    public int baseValue
    {
        get
        {
            return _baseValue;
        }
        set
        {
            _isDirty = true;
            _baseValue = value;///does it know which value is it???? YES.
        }
    }

    [Tooltip("Final value")]
    protected int _value;
    public virtual int value
    {
        get
        {
            if (_isDirty)
            {
                _value = CalculateFinalValue();
                _isDirty = false;
            }
            return _value;
        }
    }

    [Tooltip("Flag")]
    protected bool _isDirty = true;//will always set the _value the first time accessing it

    [Tooltip("Modifier/Events")]
    protected List<StatModifier> _statModifiers = new List<StatModifier>();
    public event Action<int> onValueChanged;

    public Stat(StatDefinition statDefinition)
    {
        this.statDefinition = statDefinition;
    }

    public Stat(StatDefinition statDefinition , int baseValue) : this(statDefinition)
    {
        this.baseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        _isDirty = true;
        _statModifiers.Add(mod);
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (_statModifiers.Remove(mod))
        {
            _isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifierFromSource(object source)
    {
        bool didRemove = false;

        for (int i = _statModifiers.Count - 1; i >= 0; i--)
        {
            if (_statModifiers[i].source == source)
            {
                _isDirty = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.order > b.order)
            return 1;
        else if (a.order < b.order)
            return -1;
        return 0; //if(a.order == b.order)
    }

    protected virtual int CalculateFinalValue()
    {
        float newValue = baseValue;
        float sumPercentAdd = 0;
        //Sort modifiers
        _statModifiers.Sort(CompareModifierOrder);

        for (int i = 0; i < _statModifiers.Count; i++)
        {
            StatModifier mod = _statModifiers[i];
            //Calculate by type
            if (mod.type == StatModType.Additive)//FLAT
            {
                newValue += mod.value;
            }
            else if (mod.type == StatModType.PercentAdd)//PercentAdd
            {
                sumPercentAdd += mod.value;

                if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModType.PercentAdd)//keep adding until end of list or no more PercentAdd type
                {
                    newValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.type == StatModType.PercentMult)//PercentMult
            {
                newValue *= 1 + mod.value;
            }
            else if (mod.type == StatModType.Override)//Override, the newest override modifier will always be the last(?)
            {
                newValue = mod.value;
            }
        }

        if (statDefinition.valueCap >= 0)//-1 means there's no cap
        {
            newValue = Mathf.Min(newValue, statDefinition.valueCap);
        }

        if(_value != newValue)
        {
            onValueChanged?.Invoke((int)Math.Round(newValue, 4));
        }

        //12.0001f != 12f
        return (int)Math.Round(newValue, 4);
    }

}
