using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class Trigger_TurnEvent : Trigger_Base
{
    [Space]
    [EnumToggleButtons]
    public TriggerTurnEvent_When when;
    [Space]
    [EnumToggleButtons]
    public TriggerTurnEvent_StartEnd startEnd;

    /*
    public override void RegisterTrigger(Unit selfUnit, Action<Unit> onTriggerAbility)
    {
        switch (when)
        {
            case TriggerTurnEvent_When.Battle:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_BattleStart += onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    Debug.LogWarning("DO NOT use onBattleEnd to trigger ability");
                break;
            case TriggerTurnEvent_When.SelfTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_SelfTurnStart += onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    selfUnit.unitBattle.OnUnit_SelfTurnEnd += onTriggerAbility;
                break;
            case TriggerTurnEvent_When.OpponentTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_OppoTurnStart += onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    selfUnit.unitBattle.OnUnit_OppoTurnEnd += onTriggerAbility;
                break;
            case TriggerTurnEvent_When.AnyTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                {
                    selfUnit.unitBattle.OnUnit_SelfTurnStart += onTriggerAbility;
                    selfUnit.unitBattle.OnUnit_OppoTurnStart += onTriggerAbility;
                }
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                {
                    selfUnit.unitBattle.OnUnit_SelfTurnEnd += onTriggerAbility;
                    selfUnit.unitBattle.OnUnit_OppoTurnEnd += onTriggerAbility;
                }
                break;
        }
    }

    public override void UnregisterTrigger(Unit selfUnit, Action<Unit> onTriggerAbility)
    {
        switch (when)
        {
            case TriggerTurnEvent_When.Battle:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_BattleStart -= onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    Debug.LogWarning("DO NOT use onBattleEnd to trigger ability");
                break;
            case TriggerTurnEvent_When.SelfTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_SelfTurnStart -= onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    selfUnit.unitBattle.OnUnit_SelfTurnEnd -= onTriggerAbility;
                break;
            case TriggerTurnEvent_When.OpponentTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                    selfUnit.unitBattle.OnUnit_OppoTurnStart -= onTriggerAbility;
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                    selfUnit.unitBattle.OnUnit_OppoTurnEnd -= onTriggerAbility;
                break;
            case TriggerTurnEvent_When.AnyTurn:
                if (startEnd == TriggerTurnEvent_StartEnd.Start)
                {
                    selfUnit.unitBattle.OnUnit_SelfTurnStart -= onTriggerAbility;
                    selfUnit.unitBattle.OnUnit_OppoTurnStart -= onTriggerAbility;
                }
                else if (startEnd == TriggerTurnEvent_StartEnd.End)
                {
                    selfUnit.unitBattle.OnUnit_SelfTurnEnd -= onTriggerAbility;
                    selfUnit.unitBattle.OnUnit_OppoTurnEnd -= onTriggerAbility;
                }
                break;
        }
    }

    */

    public override bool CheckTrigger(UnitLabel selflabel, UnitLabel triggererLabel, Trigger_Base trigger)
    {
        //Check Trigger Type
        if (trigger.GetType() != this.GetType())
        {
            //Debug.LogWarning("Does not matched triggerType : " + this.GetType() + " > " + trigger.GetType());
            return false;
        }

        //Cache Type
        Trigger_TurnEvent _trigger = ((Trigger_TurnEvent)trigger);

        //Check when
        switch (when)
        {
            case TriggerTurnEvent_When.Battle:
                if (_trigger.when != TriggerTurnEvent_When.Battle)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                break;
            case TriggerTurnEvent_When.SelfTurn:
                if (_trigger.when == TriggerTurnEvent_When.Battle)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                if (triggererLabel.boxSide != selflabel.boxSide)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                break;
            case TriggerTurnEvent_When.OpponentTurn:
                if (_trigger.when == TriggerTurnEvent_When.Battle)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                if (triggererLabel.boxSide == selflabel.boxSide)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                break;
            case TriggerTurnEvent_When.AnyTurn:
                if (_trigger.when == TriggerTurnEvent_When.Battle)
                {
                    //Debug.LogWarning("Does not matched when : " + when.ToString() + " > " + _trigger.when.ToString());
                    return false;
                }
                break;
        }

        //Check startEnd
        if (_trigger.startEnd != this.startEnd)
        {
            //Debug.LogWarning("Does not matched startEnd : " + startEnd.ToString() + " > " + _trigger.startEnd.ToString());
            return false;
        }

        //Matched
        Debug.Log("Trigger Matched");
        return true;
    }

    public Trigger_TurnEvent(TriggerTurnEvent_When when, TriggerTurnEvent_StartEnd startEnd)
    {
        this.when = when;
        this.startEnd = startEnd;
    }

    public Trigger_TurnEvent() : this(0, 0) { }

}

public enum TriggerTurnEvent_When
{
    Battle,
    SelfTurn,
    OpponentTurn,
    AnyTurn
}

public enum TriggerTurnEvent_StartEnd
{
    Start,
    End
}
