using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Attribute is for something like Health that health.currentHealth has a maxValue of health.value(which is maxhealth);
[System.Serializable]
public class Attribute : Stat
{
    //[Header("Current Value")]
    [SerializeField] protected int _currentValue;
    public int currentValue => _currentValue;

    public event Action<int> onCurrentValueChanged;
    public event Action<StatModifier> appliedModifier;


    public Attribute(StatDefinition definition) : base(definition)
    {
        ///Note: this will make _isDirty = false when created. So if you're using ScriptableObject to store stats, you'll need to be careful.
        ///Because when you set the _baseValue in the inspector, the value won't calculate again since _isDirty is false.
        ///So be sure to use "new Stat()" when copying stats from the scriptableObject
        _currentValue = value;
    }

    public Attribute(StatDefinition definition, int baseValue) : base(definition, baseValue)
    {
        _currentValue = value;
    }

    //One time modifier
    public virtual void ApplyModifier(StatModifier modifier)
    {
        float newValue = _currentValue;
        switch (modifier.type)
        {
            case StatModType.Additive:
                newValue += modifier.value;
                break;
            case StatModType.PercentAdd://same as percentMult since it's a one time mult
                newValue *= modifier.value;
                break;
            case StatModType.PercentMult:
                newValue *= modifier.value;
                break;
            case StatModType.Override:
                newValue = modifier.value;
                break;
        }

        ///Edit: don't need to clamp cause we're using _value as default value
        //In our case health can go negetive, change min to 0 for normal cases.
        //newValue = Mathf.Clamp(newValue, -1* _value , _value);

        if(currentValue != newValue)
        {
            _currentValue = (int)Math.Round(newValue,4);
            onCurrentValueChanged?.Invoke((int)Math.Round(newValue,4));
            appliedModifier?.Invoke(modifier);
        }
    }

    /// <summary>
    /// One time modifier, it will return how much the value actually changed
    /// </summary>
    public virtual int ApplyModifier(StatModifier modifier, float minValue, float maxValue)
    {
        int oldValue = _currentValue;
        float newValue = _currentValue;
        switch (modifier.type)
        {
            case StatModType.Additive:
                newValue += modifier.value;
                break;
            case StatModType.PercentAdd://same as percentMult since it's a one time mult
                newValue *= modifier.value;
                break;
            case StatModType.PercentMult:
                newValue *= modifier.value;
                break;
            case StatModType.Override:
                newValue = modifier.value;
                break;
        }

        //Clamp
        newValue = Mathf.Clamp(newValue, minValue ,maxValue);


        if(currentValue != newValue)
        {
            _currentValue = (int)Math.Round(newValue,4);
            onCurrentValueChanged?.Invoke((int)Math.Round(newValue,4));
            appliedModifier?.Invoke(modifier);
        }

        return _currentValue - oldValue;
    }

}
