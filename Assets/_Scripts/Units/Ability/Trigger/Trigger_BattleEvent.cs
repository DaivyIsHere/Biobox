using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Trigger_BattleEvent : Trigger_Base
{
    public TriggerBattleEvent_When when;
    public TriggerBattleEvent_StartEnd startEnd;

    public override void RegisterTrigger(Unit unit, Action<Unit> onTriggerAbility)
    {
        switch (when)
        {
            case TriggerBattleEvent_When.Battle:
                if (startEnd == TriggerBattleEvent_StartEnd.Start)
                    unit.unitBattle.OnUnit_BattleStart += onTriggerAbility;
                else if (startEnd == TriggerBattleEvent_StartEnd.End)
                    Debug.LogWarning("DO NOT use onBattleEnd to trigger ability");
                break;
            case TriggerBattleEvent_When.SelfTurn:
                if (startEnd == TriggerBattleEvent_StartEnd.Start)
                    unit.unitBattle.OnUnit_SelfTurnStart += onTriggerAbility;
                else if (startEnd == TriggerBattleEvent_StartEnd.End)
                    unit.unitBattle.OnUnit_SelfTurnEnd += onTriggerAbility;
                break;
            case TriggerBattleEvent_When.OpponentTurn:
                if (startEnd == TriggerBattleEvent_StartEnd.Start)
                    unit.unitBattle.OnUnit_OppoTurnStart += onTriggerAbility;
                else if (startEnd == TriggerBattleEvent_StartEnd.End)
                    unit.unitBattle.OnUnit_OppoTurnEnd += onTriggerAbility;
                break;
            case TriggerBattleEvent_When.AnyTurn:
                if (startEnd == TriggerBattleEvent_StartEnd.Start)
                {
                    unit.unitBattle.OnUnit_SelfTurnStart += onTriggerAbility;
                    unit.unitBattle.OnUnit_OppoTurnStart += onTriggerAbility;
                }
                else if (startEnd == TriggerBattleEvent_StartEnd.End)
                {
                    unit.unitBattle.OnUnit_SelfTurnEnd += onTriggerAbility;
                    unit.unitBattle.OnUnit_OppoTurnEnd += onTriggerAbility;
                }
                break;
        }
    }
}

public enum TriggerBattleEvent_When
{
    Battle,
    SelfTurn,
    OpponentTurn,
    AnyTurn
}

public enum TriggerBattleEvent_StartEnd
{
    Start,
    End
}
