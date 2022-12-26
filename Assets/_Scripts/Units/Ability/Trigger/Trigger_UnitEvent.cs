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
    /*
    public override void RegisterTrigger(Unit selfUnit, Action<Unit> onTriggerAbility)
    {
        if (target == TriggerUnitEvent_Target.Self)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    selfUnit.unitBattle.OnUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    selfUnit.unitBattle.OnUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    selfUnit.unitBattle.OnUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    selfUnit.unitBattle.OnUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    selfUnit.unitBattle.OnUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    selfUnit.unitBattle.OnUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    selfUnit.unitBattle.OnUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    selfUnit.unitBattle.OnUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
        else if (target == TriggerUnitEvent_Target.Ally)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    selfUnit.unitBattle.OnAllyUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    selfUnit.unitBattle.OnAllyUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    selfUnit.unitBattle.OnAllyUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    selfUnit.unitBattle.OnAllyUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    selfUnit.unitBattle.OnAllyUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    selfUnit.unitBattle.OnAllyUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    selfUnit.unitBattle.OnAllyUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    selfUnit.unitBattle.OnAllyUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
        else if (target == TriggerUnitEvent_Target.Enemy)
        {
            switch (unitEvent)
            {
                case (TriggerUnitEvent_Event.BeforeAttack):
                    selfUnit.unitBattle.OnEnemyUnit_BeforeAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterAttack):
                    selfUnit.unitBattle.OnEnemyUnit_AfterAttack += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeTakeHit):
                    selfUnit.unitBattle.OnEnemyUnit_BeforeTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeHit):
                    selfUnit.unitBattle.OnEnemyUnit_AfterTakeHit += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterShieldBreak):
                    selfUnit.unitBattle.OnEnemyUnit_AfterShieldBreak += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterTakeDamage):
                    selfUnit.unitBattle.OnEnemyUnit_AfterTakeDamage += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.AfterSummoned):
                    selfUnit.unitBattle.OnEnemyUnit_AfterSummoned += onTriggerAbility;
                    break;
                case (TriggerUnitEvent_Event.BeforeDeath):
                    selfUnit.unitBattle.OnEnemyUnit_BeforeDeath += onTriggerAbility;
                    break;
            }
        }
    }

    public override void UnregisterTrigger(Unit unit, Action<Unit> onTriggerAbility)
    {

    }

    */
    public override bool CheckTrigger(UnitLabel selflabel, UnitLabel triggererLabel, Trigger_Base trigger)
    {
        if (trigger.GetType() != this.GetType())
        {
            //Debug.LogWarning("Does not matched triggerType : " + this.GetType() + " > " + trigger.GetType());
            return false;
        }

        //Cache Type
        Trigger_UnitEvent _trigger = ((Trigger_UnitEvent)trigger);

        if (_trigger.unitEvent != this.unitEvent)
        {
            //Debug.LogWarning("Does not matched unitEvent : " + unitEvent.ToString() + " > " + _trigger.unitEvent.ToString());
            return false;
        }

        switch (target)
        {
            case TriggerUnitEvent_Target.Self:
                if (selflabel.GetUnit() != triggererLabel.GetUnit())
                {
                    //Debug.LogWarning("Not the same unit");
                    return false;
                }
                break;
            case TriggerUnitEvent_Target.Ally:
                if (selflabel.GetUnit() == triggererLabel.GetUnit())
                {
                    //Debug.LogWarning("Not the ally, it's self");
                    return false;
                }
                if (selflabel.boxSide != triggererLabel.boxSide)
                {
                    //Debug.LogWarning("Not the same side");
                    return false;
                }
                break;
            case TriggerUnitEvent_Target.Enemy:
                if (selflabel.boxSide == triggererLabel.boxSide)
                {
                    //Debug.LogWarning("Not the opposite side");
                    return false;
                }
                break;
            case TriggerUnitEvent_Target.AnyOther:
                if (selflabel.GetUnit() == triggererLabel.GetUnit())
                {
                    //Debug.LogWarning("ingore self in anyOther");
                    return false;
                }
                break;
        }

        Debug.Log("Trigger Matched");
        return true;

    }

    public Trigger_UnitEvent(TriggerUnitEvent_Target target, TriggerUnitEvent_Event unitEvent)
    {
        this.target = target;
        this.unitEvent = unitEvent;
    }

    public Trigger_UnitEvent(TriggerUnitEvent_Event unitEvent) : this(0, unitEvent) { }

    public Trigger_UnitEvent() : this(0, 0) { }
}

public enum TriggerUnitEvent_Target
{
    Self,
    Ally,//beside self
    Enemy,
    AnyOther//Any other
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
