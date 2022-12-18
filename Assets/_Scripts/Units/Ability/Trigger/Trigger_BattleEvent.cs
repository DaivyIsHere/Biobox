using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger_BattleEvent : Trigger_Base
{
    public TriggerBattleEvent_When when;
    public TriggerBattleEvent_StartEnd startEnd;
}

public enum TriggerBattleEvent_When
{
    Battle,
    SelfTurn,
    OpponentTurn,
    BothTurn
}

public enum TriggerBattleEvent_StartEnd
{
    Start,
    End
}
