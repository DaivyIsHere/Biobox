using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitCombat : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitAnimation _unitAnimation;

    [Header("CurrentStats")]
    [SerializeField] private UnitStats _stats;

    [field: Header("Status")]
    [SerializeField] public bool canAttack = false;//set by player
    [field: SerializeField] public bool exhausted { get; set; } = false;

    [Header("Status Effect")]
    [SerializeField] private StatusEffectDatabase _effectDatabase;
    [SerializeField] private UnitStatusEffects _statusEffects;

    void Start()
    {
        IniStats();

        _unit.UpdateHealthDisplay(_stats.health.currentValue);
        _unit.UpdateAttackDisplay(_stats.attack.value);
        _unit.UpdateShieldDisplay(_stats.shield.value);

        _statusEffects._effects.Add(_effectDatabase.exhausted,new StatusEffect(_effectDatabase.exhausted, 5));
        print(_statusEffects._effects[_effectDatabase.exhausted].stack);

    }

    private void IniStats()
    {
        _stats = new UnitStats(
            _unit.unitData.baseStats.attackBase,
            _unit.unitData.baseStats.healthBase,
            _unit.unitData.baseStats.shieldBase);
    }

    private void OnMouseDown()
    {
        if (!canAttack)
            return;
        if (exhausted)
            return;

        exhausted = true;

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
            if (damageValue >= _stats.shield.value)
            {
                damageToTake = damageValue - _stats.shield.value;
                shieldBreakValue = _stats.shield.value;
                _stats.shield.AddModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
                _stats.health.ApplyModifier(new StatModifier(-1 * damageToTake, StatModType.Additive));

                new CCUnitShieldBreak(_unit.unitCID, shieldBreakValue, _stats.shield.value).AddToQueue();
                if (damageToTake > 0)
                    new CCUnitTakeDamage(_unit.unitCID, damageToTake, _stats.health.currentValue).AddToQueue();

            }
            else if (damageValue < _stats.shield.value)
            {
                shieldBreakValue = Mathf.CeilToInt(damageValue * 0.5f);
                _stats.shield.AddModifier(new StatModifier(-1 * shieldBreakValue, StatModType.Additive));

                new CCUnitShieldBreak(_unit.unitCID, shieldBreakValue, _stats.shield.value).AddToQueue();
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
        if (_stats.attack.value > 0)
            target.unitCombat.TakeDamage(_stats.attack.value);
    }

    public void Die()
    {
        BattleManager.Instance.DespawnUnit(_unit);
    }

    public bool HasShield()
    {
        return _stats.shield.value > 0;
    }

    public bool IsDead()
    {
        return _stats.health.currentValue <= 0;
    }
}
