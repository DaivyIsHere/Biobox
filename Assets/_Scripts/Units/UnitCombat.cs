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
    [field: SerializeField] public bool exhausted { get;  set; } = false;

    void Start()
    {
        IniStats();
        _stats.attack.onValueChanged += _unit.OnAttackChange;
        _stats.health.onCurrentValueChanged += _unit.OnHealthChange;
        
        _unit.OnHealthChange(_stats.health.currentValue);
        _unit.OnAttackChange(_stats.attack.value);
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
        if(!canAttack)
            return;
        if (exhausted)
            return;

        exhausted = true;
        
        Attack();
        TurnManager.Instance.PlayerTakeTurn();
    }

    public void TakeDamage(int damageValue)
    {
        if (damageValue > 0)
        {
            _stats.health.ApplyModifier(new StatModifier(-1*damageValue, StatModType.Additive));
            if (IsDead())
            {
                // TODO command to play animation that we can know if a turn is end or not
                new CCUnitTakeDamage(_unit.unitCID, damageValue, _stats.health.currentValue, true).AddToQueue();
                new CCUnitDie(_unit.unitCID).AddToQueue();
                Die();
            }
            else
            {
                new CCUnitTakeDamage(_unit.unitCID, damageValue, _stats.health.currentValue, false).AddToQueue();
            }
        }
    }

    public void Attack()
    {
        new CCUnitAttack(_unit.unitCID).AddToQueue(); // Visual

        Unit target = BattleManager.Instance.GetFirstTargetByLabel(_unit.unitLabel);
        if (!target)
            return;
        target.unitCombat.TakeDamage(_stats.attack.value);

    }

    public void Die()
    {
        BattleManager.Instance.DespawnUnit(_unit);
    }

    public bool IsDead()
    {
        return _stats.health.currentValue <= 0;
    }
}
