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
    [SerializeField] public UnitCID triggerUnit;

    ///INVOKED FROM PLAYER.CS
    // public event Action<Unit> OnUnit_BattleStart;
    // public event Action<Unit> OnUnit_SelfTurnStart;
    // public event Action<Unit> OnUnit_SelfTurnEnd;
    // public event Action<Unit> OnUnit_OppoTurnStart;
    // public event Action<Unit> OnUnit_OppoTurnEnd;

    //Self
    ///INVOKED FROM THIS UNITBATTLE.CS
    // public event Action<Unit> OnUnit_BeforeAttack;//done
    // public event Action<Unit> OnUnit_AfterAttack;//done
    // public event Action<Unit> OnUnit_BeforeTakeHit;//done
    // public event Action<Unit> OnUnit_AfterTakeHit;//done
    // public event Action<Unit> OnUnit_AfterShieldBreak;//done
    // public event Action<Unit> OnUnit_AfterTakeDamage;//done
    // public event Action<Unit> OnUnit_AfterSummoned;//
    // public event Action<Unit> OnUnit_BeforeDeath;//done

    //Ally
    ///INVOKED FROM THIS BATTLEMANAGER.INSTANCE
    // public event Action<Unit> OnAllyUnit_BeforeAttack;//
    // public event Action<Unit> OnAllyUnit_AfterAttack;//
    // public event Action<Unit> OnAllyUnit_BeforeTakeHit;//
    // public event Action<Unit> OnAllyUnit_AfterTakeHit;//
    // public event Action<Unit> OnAllyUnit_AfterShieldBreak;//
    // public event Action<Unit> OnAllyUnit_AfterTakeDamage;//
    // public event Action<Unit> OnAllyUnit_AfterSummoned;//
    // public event Action<Unit> OnAllyUnit_BeforeDeath;//

    // //Enemy
    // ///INVOKED FROM THIS BATTLEMANAGER.INSTANCE
    // public event Action<Unit> OnEnemyUnit_BeforeAttack;//
    // public event Action<Unit> OnEnemyUnit_AfterAttack;//
    // public event Action<Unit> OnEnemyUnit_BeforeTakeHit;//
    // public event Action<Unit> OnEnemyUnit_AfterTakeHit;//
    // public event Action<Unit> OnEnemyUnit_AfterShieldBreak;//
    // public event Action<Unit> OnEnemyUnit_AfterTakeDamage;//
    // public event Action<Unit> OnEnemyUnit_AfterSummoned;//
    // public event Action<Unit> OnEnemyUnit_BeforeDeath;//


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

        // if (passiveAbility)
        //     BattleManager.Instance.OnTriggerBattleEvent += OnBattleEvent;
            //passiveAbility.InitializeAbility(_unit);
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

    public void InvokeBeforeTakeHit()
    {
        //Event hook//
        BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.BeforeTakeHit));
        //OnUnit_BeforeTakeHit?.Invoke(_unit);
        //BattleManager.Instance.InvokeEvent_UnitBeforeTakeHit(_unit);
    }

    ///Need to call InvokeBeforeTakeHit() before calling this.
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

            }
            else if (damageValue < _stats.shield.currentValue)
            {
                shieldBreakValue = Mathf.CeilToInt(damageValue * 0.5f);
                TakeStatModifier(_statDB.shield, new StatModifier(-1 * shieldBreakValue, StatModType.Additive));
            }
        }
        else
        {
            damageToTake = damageValue;
            TakeStatModifier(_statDB.health, new StatModifier(-1 * damageToTake, StatModType.Additive));
        }

        //Event Hook//
        BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.AfterTakeHit));
        //OnUnit_AfterTakeHit?.Invoke(_unit);
        //BattleManager.Instance.InvokeEvent_UnitAfterTakeHit(_unit);
    }

    public void Attack()
    {
        //Event hook//
        BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.BeforeAttack));
        //OnUnit_BeforeAttack?.Invoke(_unit);
       // BattleManager.Instance.InvokeEvent_UnitBeforeAttack(_unit);

        Unit target = BattleManager.Instance.GetFirstTargetByLabel(_unit.unitLabel);
        if (!target)
            return;
        if (_stats.attack.currentValue > 0)
        {
            target.unitBattle.InvokeBeforeTakeHit();
            new CCUnitAttack(_unit.unitCID).AddToQueue(); // Visual
            target.unitBattle.TakeDamage(_stats.attack.currentValue);
        }


        //Event hook//
        //OnUnit_AfterAttack?.Invoke(_unit);
        //BattleManager.Instance.InvokeEvent_UnitAfterAttack(_unit);
        BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.AfterAttack));
    }

    public void TakeStatModifier(StatDefinition definition, StatModifier modifier)
    {
        ///TODO : check modDown or modUp, because statModType.Multiply will mod down at 0.5f

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
                int displayValue = _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
                ///HEAL animation, or add a PlayHealanimation and call it in PlayHeal;

                //if(displayValue > 0)
                new CCUnitHeal(_unit.unitCID, displayValue, _stats.health.currentValue).AddToQueue();
            }
            else
            {
                _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
                new CCUnitTakeDamage(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.health.currentValue, IsDead()).AddToQueue();

                if (IsDead())
                {
                    //Event Hook//
                    //OnUnit_BeforeDeath?.Invoke(_unit);
                    //BattleManager.Instance.InvokeEvent_UnitBeforeDeath(_unit);
                    BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.BeforeDeath));

                    new CCUnitDie(_unit.unitCID).AddToQueue();
                    Die();
                }
                else
                {
                    new CCUnitHurt(_unit.unitCID).AddToQueue();
                }

                //Event Hook//
                //OnUnit_AfterTakeDamage?.Invoke(_unit);
                //BattleManager.Instance.InvokeEvent_UnitAfterTakeDamage(_unit);
                BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.AfterTakeDamage));
            }


        }
        else if (definition == _statDB.maxHealth)
        {
            if (modifier.value > 0)
            {
                _stats.maxHealth.ApplyModifier(modifier, 1, float.MaxValue);
                int displayValue = _stats.health.ApplyModifier(modifier, float.MinValue, _stats.maxHealth.currentValue);
                new CCUnitHeal(_unit.unitCID, displayValue, _stats.health.currentValue).AddToQueue();
            }
            else
            {
                _stats.maxHealth.ApplyModifier(modifier, 1, float.MaxValue);
                int displayValue = _stats.health.ApplyModifier(new StatModifier(0, StatModType.Additive), float.MinValue, _stats.maxHealth.currentValue);
                new CCUnitHealthChange(_unit.unitCID, displayValue, _stats.health.currentValue);
            }
        }
        else if (definition == _statDB.shield)
        {
            int oldValue = _stats.shield.currentValue;
            _stats.shield.ApplyModifier(modifier, 0, float.MaxValue);
            new CCUnitShieldDecrease(_unit.unitCID, Mathf.Abs((int)modifier.value), _stats.shield.currentValue).AddToQueue();

            ///Check ShieldBreak
            ///TODO : Display shieldbreak animation
            if (oldValue > 0 && _stats.shield.currentValue <= 0)
            {
                //Event Hook//
                //OnUnit_AfterShieldBreak?.Invoke(_unit);
                //BattleManager.Instance.InvokeEvent_UnitAfterShieldBreak(_unit);
                BattleManager.Instance.TriggerBattleEvent(_unit.unitLabel, new Trigger_UnitEvent(TriggerUnitEvent_Event.AfterShieldBreak));
            }
        }
    }

    public void Die()
    {
        if (passiveAbility)
            passiveAbility.UninitializeAbility(_unit);

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

    public void OnBattleEvent(UnitLabel triggererLabel, Trigger_Base trigger)
    {
        if(!passiveAbility)
            return;
            
        if(passiveAbility.trigger.CheckTrigger(_unit.unitLabel, triggererLabel, trigger))
            TriggerAbility();
    }

    public void TriggerAbility()
    {
        bool conditionMet = true;
        foreach (var c in passiveAbility.conditions)
        {
            if (!c.ConditionMet(_unit))
                conditionMet = false;
        }

        if (conditionMet)
        {
            Debug.Log("Perform ability");
            new CCUnitStartAbility(_unit.unitCID).AddToQueue();
            foreach (var a in passiveAbility.actions)
            {
                a.DoAction(_unit);
            }

            if (!IsDead())
                new CCUnitEndAbility(_unit.unitCID).AddToQueue();
        }
    }


    //Event
    // public void OnBattleStart() => OnUnit_BattleStart?.Invoke(_unit);
    // public void OnSelfTurnStart() => OnUnit_SelfTurnStart?.Invoke(_unit);
    // public void OnSelfTurnEnd() => OnUnit_SelfTurnEnd?.Invoke(_unit);
    // public void OnOppoTurnStart() => OnUnit_OppoTurnStart?.Invoke(_unit);
    // public void OnOppoTurnEnd() => OnUnit_OppoTurnEnd?.Invoke(_unit);

    // public void OnAlly_BeforeAttack() => OnAllyUnit_BeforeAttack?.Invoke(_unit);
    // public void OnAlly_AfterAttack() => OnAllyUnit_AfterAttack?.Invoke(_unit);
    // public void OnAlly_BeforeTakeHit() => OnAllyUnit_BeforeTakeHit?.Invoke(_unit);
    // public void OnAlly_AfterTakeHit() => OnAllyUnit_AfterTakeHit?.Invoke(_unit);
    // public void OnAlly_AfterShieldBreak() => OnAllyUnit_AfterShieldBreak?.Invoke(_unit);
    // public void OnAlly_AfterTakeDamage() => OnAllyUnit_AfterTakeDamage?.Invoke(_unit);
    // public void OnAlly_AfterSummoned() => OnAllyUnit_AfterSummoned?.Invoke(_unit);
    // public void OnAlly_BeforeDeath() => OnAllyUnit_BeforeDeath?.Invoke(_unit);

    // public void OnEnemy_BeforeAttack() => OnEnemyUnit_BeforeAttack?.Invoke(_unit);
    // public void OnEnemy_AfterAttack() => OnEnemyUnit_AfterAttack?.Invoke(_unit);
    // public void OnEnemy_BeforeTakeHit() => OnEnemyUnit_BeforeTakeHit?.Invoke(_unit);
    // public void OnEnemy_AfterTakeHit() => OnEnemyUnit_AfterTakeHit?.Invoke(_unit);
    // public void OnEnemy_AfterShieldBreak() => OnEnemyUnit_AfterShieldBreak?.Invoke(_unit);
    // public void OnEnemy_AfterTakeDamage() => OnEnemyUnit_AfterTakeDamage?.Invoke(_unit);
    // public void OnEnemy_AfterSummoned() => OnEnemyUnit_AfterSummoned?.Invoke(_unit);
    // public void OnEnemy_BeforeDeath() => OnEnemyUnit_BeforeDeath?.Invoke(_unit);

}
