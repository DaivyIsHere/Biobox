using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitCombat : MonoBehaviour
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

    [field: Header("Player Control")]
    [SerializeField] public bool canAttack = false;//set by player

    void Start()
    {
        IniStats();

        _unit.UpdateHealthDisplay(_stats.health.currentValue);
        _unit.UpdateAttackDisplay(_stats.attack.currentValue);
        _unit.UpdateShieldDisplay(_stats.shield.currentValue);
    }

    private void IniStats()
    {
        _stats = new UnitStats(
            _unit.unitData.baseStats.attackBase,
            _unit.unitData.baseStats.healthBase,
            _unit.unitData.baseStats.healthBase,
            _unit.unitData.baseStats.shieldBase);
    }

    public int GetStatValue(StatDefinition definition)
    {
        if(definition == _statDB.attack)
            return _stats.attack.currentValue;
        else if(definition == _statDB.health)
            return _stats.health.currentValue;
        else if(definition == _statDB.maxHealth)
            return _stats.maxHealth.currentValue;
        else if(definition == _statDB.shield)
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
                _stats.shield.ApplyModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
                _stats.health.ApplyModifier(new StatModifier(-1 * damageToTake, StatModType.Additive));

                new CCUnitShieldBreak(_unit.unitCID, shieldBreakValue, _stats.shield.currentValue).AddToQueue();
                if (damageToTake > 0)
                    new CCUnitTakeDamage(_unit.unitCID, damageToTake, _stats.health.currentValue).AddToQueue();

            }
            else if (damageValue < _stats.shield.currentValue)
            {
                shieldBreakValue = Mathf.CeilToInt(damageValue * 0.5f);
                _stats.shield.ApplyModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));

                new CCUnitShieldBreak(_unit.unitCID, shieldBreakValue, _stats.shield.currentValue).AddToQueue();
            }
        }
        else
        {
            damageToTake = damageValue;
            _stats.health.ApplyModifier(new StatModifier(-1 * damageToTake, StatModType.Additive));

            new CCUnitTakeDamage(_unit.unitCID, damageToTake, _stats.health.currentValue).AddToQueue();
        }
        
        //dead or not
        if (!IsDead())
        {
            new CCUnitHurt(_unit.unitCID).AddToQueue();
        }
        else
        {
            new CCUnitDie(_unit.unitCID).AddToQueue();
            Die();
        }
    }

    public void Attack()
    {
        new CCUnitAttack(_unit.unitCID).AddToQueue(); // Visual

        Unit target = BattleManager.Instance.GetFirstTargetByLabel(_unit.unitLabel);
        if (!target)
            return;
        if (_stats.attack.currentValue > 0)
            target.unitCombat.TakeDamage(_stats.attack.currentValue);
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
}
