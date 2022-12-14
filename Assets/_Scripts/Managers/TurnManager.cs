using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : Singleton<TurnManager>
{
    [Header("TurnState")]
    public TurnState currentTurnState;
    public BoxSide currentSide;// which side's turn
    public int turnCount;
    public bool isResolving;

    public static event Action OnBattleStart;
    public static event Action<BoxSide> OnTurnStart;
    public static event Action<BoxSide> OnWaitForPlayerTakeTurn;
    public static event Action<BoxSide> OnPlayerTakeTurn;
    public static event Action<BoxSide> OnTurnEnd;
    public static event Action<TurnState, BoxSide> OnTurnStateChanged;

    public void StartTurnManaging()//called by battleManager
    {
        UpdateTurnState(TurnState.BattleStart,0);
    }

    public void UpdateTurnState(TurnState newState,float PendingTime)
    {
        StartCoroutine(PendingUpdateTurnState(newState, PendingTime));
    }

    private IEnumerator PendingUpdateTurnState(TurnState newState, float PendingTime)
    {
        yield return new WaitForSeconds(PendingTime);
        //print("Update to State : "+ newState.ToString());

        currentTurnState = newState;

        switch (newState)
        {
            case TurnState.BattleStart:
                OnBattleStart?.Invoke();
                HandleBattleStart();
                break;
            case TurnState.TurnStart:
                OnTurnStart?.Invoke(currentSide);
                HandleTurnStart();
                break;
            case TurnState.WaitForCurrentPlayer:
                OnWaitForPlayerTakeTurn?.Invoke(currentSide);
                HandleWaitForCurrentPlayer();
                break;
            case TurnState.TurnEnd:
                OnTurnEnd?.Invoke(currentSide);
                HandleTurnEnd();
                break;
        }

        OnTurnStateChanged?.Invoke(newState, currentSide);
    }

    public void PlayerTakeTurn()//called by UnitCombat.cs after attack
    {
        isResolving = true;
        OnPlayerTakeTurn?.Invoke(currentSide);
        StartCoroutine(ResolvingCurrentTurn(TurnState.TurnEnd));
    }

    private IEnumerator ResolvingCurrentTurn(TurnState nextState)
    {
        while(isResolving)
        {
            if(!CCommand.playingQueue)
            {
                isResolving = false;
            }
            yield return 0;
        }
        UpdateTurnState(nextState,0.5f);
    }

    private void HandleBattleStart()
    {
        turnCount = 1;
        UnitLabel label = new UnitLabel(new BoxLabel(currentSide, -1), -1);
        BattleManager.Instance.TriggerBattleEvent(new UnitLabel(), new Trigger_TurnEvent(TriggerTurnEvent_When.Battle, TriggerTurnEvent_StartEnd.Start));
        UpdateTurnState(TurnState.TurnStart, 1f);
    }

    private void HandleTurnStart()
    {
        UnitLabel label = new UnitLabel(new BoxLabel(currentSide, -1), -1);
        BattleManager.Instance.TriggerBattleEvent(label, new Trigger_TurnEvent(TriggerTurnEvent_When.AnyTurn, TriggerTurnEvent_StartEnd.Start));
        
        UpdateTurnState(TurnState.WaitForCurrentPlayer,1f);
    }

    private void HandleWaitForCurrentPlayer()
    {
        
    }

    private void HandleTurnEnd()
    {
        UnitLabel label = new UnitLabel(new BoxLabel(currentSide, -1), -1);
        BattleManager.Instance.TriggerBattleEvent(label, new Trigger_TurnEvent(TriggerTurnEvent_When.AnyTurn, TriggerTurnEvent_StartEnd.End));
        
        currentSide = currentSide.Opposite();
        turnCount += 1;
        new CCAlignUnits().AddToQueue();
        UpdateTurnState(TurnState.TurnStart,1f);
    }

    public bool IsCurrentSide(BoxSide boxSide)
    {
        return (currentSide == boxSide);
    }

    public bool CanTakeTurn(BoxSide boxSide)
    {
        return (currentTurnState == TurnState.WaitForCurrentPlayer) && !isResolving && IsCurrentSide(boxSide);
    }


}

public enum TurnState
{
    BattleStart,
    TurnStart,
    WaitForCurrentPlayer,
    TurnEnd,
}
