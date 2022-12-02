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
    public UnitStats stats
    {
        get
        {
            return _stats;
        }
        private set
        {
            //maxhealth
            _stats.maxHealth = value.maxHealth > 0 ? value.maxHealth : 1;

            //health
            _stats.health = Mathf.Clamp(value.health, 0, _stats.maxHealth);

            //attack
            _stats.attack = value.attack >= 0 ? value.attack : 0;
        }
    }

    [field: Header("Status")]
    [field: SerializeField] public bool exhausted { get;  set; } = false;

    void Start()
    {
        stats = _unit.unitData.stats;
    }

    private void OnMouseDown()
    {
        if (exhausted)
            return;

        if(!TurnManager.Instance.CanTakeTurn(_unit.unitLabel.boxSide))
            return;

        exhausted = true;
        
        Attack();
        TurnManager.Instance.OnPlayerTakeTurn();
    }

    public void ResolveStatChanges(UnitStats statsChange)
    {
        stats += statsChange;
    }

    public void TakeDamage(int damageValue)
    {
        if (damageValue > 0)
        {
            ResolveStatChanges(new UnitStats(0, 0, -1 * damageValue));
            if (IsDead())
            {
                // TODO command to play animation that we can know if a turn is end or not
                new CCUnitTakeDamage(_unit.unitCID, damageValue, stats.health, true).AddToQueue();
                new CCUnitDie(_unit.unitCID).AddToQueue();
                Die();
            }
            else
            {
                new CCUnitTakeDamage(_unit.unitCID, damageValue, stats.health, false).AddToQueue();
            }
        }
    }

    public void Attack()
    {
        new CCUnitAttack(_unit.unitCID).AddToQueue(); // Visual

        Unit target = CombatManager.Instance.GetFirstTargetByLabel(_unit.unitLabel);
        if (!target)
            return;
        target.unitCombat.TakeDamage(stats.attack);

    }

    public void Die()
    {
        CombatManager.Instance.DespawnUnit(_unit);
    }

    public bool IsDead()
    {
        return stats.health <= 0;
    }
}
