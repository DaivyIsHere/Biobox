using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class Trigger_UnitEvent : Trigger_Base
{
    [Space]
    [EnumToggleButtons]
    public TriggerUnitEvent_Target target;
    [Space]
    [EnumToggleButtons]
    public TriggerUnitEvent_Event unitEvent;

    public override void RegisterTrigger(Unit unit, Action<Unit> onTriggerAbility)
    {
        if (target == TriggerUnitEvent_Target.Self)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    unit.unitBattle.OnUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    unit.unitBattle.OnUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    unit.unitBattle.OnUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    unit.unitBattle.OnUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    unit.unitBattle.OnUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    unit.unitBattle.OnUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    unit.unitBattle.OnUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    unit.unitBattle.OnUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
        else if (target == TriggerUnitEvent_Target.Ally)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    unit.unitBattle.OnAllyUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    unit.unitBattle.OnAllyUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    unit.unitBattle.OnAllyUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    unit.unitBattle.OnAllyUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    unit.unitBattle.OnAllyUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    unit.unitBattle.OnAllyUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    unit.unitBattle.OnAllyUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    unit.unitBattle.OnAllyUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
        else if (target == TriggerUnitEvent_Target.Enemy)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    unit.unitBattle.OnEnemyUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    unit.unitBattle.OnEnemyUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    unit.unitBattle.OnEnemyUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    unit.unitBattle.OnEnemyUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    unit.unitBattle.OnEnemyUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    unit.unitBattle.OnEnemyUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    unit.unitBattle.OnEnemyUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    unit.unitBattle.OnEnemyUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
    }

    public override void UnregisterTrigger(Unit unit, Action<Unit> onTriggerAbility)
    {

    }
}

public enum TriggerUnitEvent_Target
{
    Self,
    Ally,
    Enemy
}

public enum TriggerUnitEvent_Event
{
    BeforeAttack,
    AfterAttack,
    BeforeTakeHit,
    AfterTakeHit,
    AfterShieldBreak,
    AfterTakeDamage,
    AfterSummoned,
    BeforeDeath,
}
