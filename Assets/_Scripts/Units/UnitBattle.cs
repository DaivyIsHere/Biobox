using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Unit))]
public class UnitBattle : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitAnimation _unitAnimation;

    [Header("Stats")]
    [SerializeField] public StatDefinitionDatabase _statDB;
    [SerializeField] private UnitStats _stats;

    [Header("Status Effect")]
    [SerializeField] public StatusEffectDatabase _effectDB;
    [SerializeField] private UnitStatusEffects _statusEffects;

    [Header("Ability")]
    [SerializeField] public PassiveAbility passiveAbility;

    public event Action<Unit> OnUnit_BattleStart;
    public event Action<Unit> OnUnit_SelfTurnStart;
    public event Action<Unit> OnUnit_SelfTurnEnd;
    public event Action<Unit> OnUnit_OppoTurnStart;
    public event Action<Unit> OnUnit_OppoTurnEnd;

    public event Action<Unit> OnUnit_BeforeAttack;
    public event Action<Unit> OnUnit_AfterAttack;
    public event Action<Unit> OnUnit_BeforeTakeHit;
    public event Action<Unit> OnUnit_AfterTakeHit;
    public event Action<Unit> OnUnit_AfterShieldBreak;
    public event Action<Unit> OnUnit_AfterTakeDamage;
    public event Action<Unit> OnUnit_BeforeDeath;


    [field: Header("Player Control")]
    [SerializeField] public bool canAttack = false;//set by player

    //Called by Unit.cs
    public void IniUnitBattle()
    {
        IniStats();
        IniAbility();

        _unit.UpdateHealthDisplay(_stats.health.currentValue);
        _unit.UpdateAttackDisplay(_stats.attack.currentValue);
        _unit.UpdateShieldDisplay(_stats.shield.currentValue);
    }

    public void AbilityTest()
    {

        //OnUnit_SelfTurnEnd?.Invoke(this._unit);
    }

    private void IniStats()
    {
        _stats = new UnitStats(
            _unit.unitData.baseStats.attackBase,
            _unit.unitData.baseStats.healthBase,
            _unit.unitData.baseStats.healthBase,
            _unit.unitData.baseStats.shieldBase);
    }

    private void IniAbility()
    {
        passiveAbility = _unit.unitData.passiveAbility;

        if (passiveAbility)
        {
            passiveAbility.InitializeAbiltiy(_unit);
        }
    }

    public int GetStatValue(StatDefinition definition)
    {
        if (definition == _statDB.attack)
            return _stats.attack.currentValue;
        else if (definition == _statDB.health)
            return _stats.health.currentValue;
        else if (definition == _statDB.maxHealth)
            return _stats.maxHealth.currentValue;
        else if (definition == _statDB.shield)
            return _stats.shield.currentValue;

        return 0;
    }

    public bool IsExhausted()
    {
        return _statusEffects.HasEffect(_effectDB.exhausted);
    }

    public void Exhaust()
    {
        _statusEffects.ApplyStatusEffect(new StatusEffect(_effectDB.exhausted));
    }

    public void Unexhaust()
    {
        _statusEffects.RemoveStatusEffect(_effectDB.exhausted);
    }

    private void OnMouseDown()
    {
        if (!canAttack)
            return;
        if (IsExhausted())
            return;

        Exhaust();
        Attack();
        TurnManager.Instance.PlayerTakeTurn();
    }

    public void TakeDamage(int damageValue)
    {
        if (damageValue <= 0)
            return;

        int damageToTake = 0;
        int shieldBreakValue = 0;
        if (HasShield())
        {
            //Damage greater than shield
            if (damageValue >= _stats.shield.currentValue)
            {
                damageToTake = damageValue - _stats.shield.currentValue;
                shieldBreakValue = _stats.shield.currentValue;
                TakeStatModifier(_statDB.shield, new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
                TakeStatModifier(_statDB.health, new StatModifier(-1 * damageToTake, StatModType.Additive));

                // _stats.shield.ApplyModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
                // _stats.health.ApplyModifier(new StatModifier(-1 * damageToTake, StatModType.Additive));

                // new CCUnitShieldDecrease(_unit.unitCID, shieldBreakValue, _stats.shield.currentValue).AddToQueue();
                // if (damageToTake > 0)
                //     new CCUnitTakeDamage(_unit.unitCID, damageToTake, _stats.health.currentValue).AddToQueue();

            }
            else if (damageValue < _stats.shield.currentValue)
            {
                shieldBreakValue = Mathf.CeilToInt(damageValue * 0.5f);
                TakeStatModifier(_statDB.shield, new StatModifier(-1 * shieldBreakValue, StatModType.Additive));

                //_stats.shield.ApplyModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
                //new CCUnitShieldDecrease(_unit.unitCID, shieldBreakValue, _stats.shield.currentValue).AddToQueue();
            }
        }
        else
        {
            damageToTake = damageValue;
            TakeStatModifier(_statDB.health, new StatModifier(-1 * damageToTake, StatModType.Additive));

            //_stats.health.ApplyModifier(new StatModifier(-1 * damageToTake, StatModType.Additive));
            //new CCUnitTakeDamage(_unit.unitCID, damageToTake, _stats.health.currentValue).AddToQueue();
        }
    }

    public void Attack()
    {
        new CCUnitAttack(_unit.unitCID).AddToQueue(); // Visual

        Unit target = BattleManager.Instance.GetFirstTargetByLabel(_unit.unitLabel);
        if (!target)
            return;
        if (_stats.attack.currentValue > 0)
            target.unitBattle.TakeDamage(_stats.attack.currentValue);
    }

    public void TakeStatModifier(StatDefinition definition, StatModifier modifier)
    {
        if (modifier.value == 0)
            return;

        if (definition == _statDB.attack)
        {
            _stats.attack.ApplyModifier(modifier, 0, float.MaxValue);
            new CCUnitAttackChange(_unit.unitCID, (int)modifier.value, _stats.attack.currentValue).AddToQueue();
        }
        else if (definition == _statDB.health)
        {
            if (modifier.value > 0)
            {
                _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
                new CCUnitHeal(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.health.currentValue).AddToQueue();
            }
            else
            {
                _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
                new CCUnitTakeDamage(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.health.currentValue).AddToQueue();
            }

            if (!IsDead())
            {
                if (modifier.value > 0)
                {
                    ///HEAL animation
                }
                else
                    new CCUnitHurt(_unit.unitCID).AddToQueue();
            }
            else
            {
                new CCUnitDie(_unit.unitCID).AddToQueue();
                Die();
            }
        }
        else if (definition == _statDB.maxHealth)
        {
            _stats.maxHealth.ApplyModifier(modifier, 1, float.MaxValue);
            _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
            new CCUnitHeal(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.health.currentValue).AddToQueue();
            //new CCUnitHealthChange(_unit.unitCID, _stats.maxHealth.currentValue - _stats.health.currentValue, _stats.maxHealth.currentValue).AddToQueue();

            // if(_stats.maxHealth.currentValue <= _stats.health.currentValue)
            // {
            //     _stats.health.ApplyModifier(new StatModifier(0, StatModType.Additive), float.MinValue, _stats.maxHealth.currentValue);
            //     new CCUnitHealthChange(_unit.unitCID, _stats.maxHealth.currentValue -_stats.health.currentValue, _stats.maxHealth.currentValue).AddToQueue();
            // }
        }
        else if (definition == _statDB.shield)
        {
            _stats.shield.ApplyModifier(modifier, 0, float.MaxValue);
            new CCUnitShieldDecrease(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.shield.currentValue).AddToQueue();
            ///Check ShieldBreak
        }
    }

    public void Die()
    {
        BattleManager.Instance.DespawnUnit(_unit);
    }

    public bool HasShield()
    {
        return _stats.shield.currentValue > 0;
    }

    public bool IsDead()
    {
        return _stats.health.currentValue <= 0;
    }


    //Event
    public void OnBattleStart() => OnUnit_BattleStart?.Invoke(_unit);
    public void OnSelfTurnStart() => OnUnit_SelfTurnStart?.Invoke(_unit);
    public void OnSelfTurnEnd() => OnUnit_SelfTurnEnd?.Invoke(_unit);
    public void OnOppoTurnStart() => OnUnit_OppoTurnStart?.Invoke(_unit);
    public void OnOppoTurnEnd() => OnUnit_OppoTurnEnd?.Invoke(_unit);

}
