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
    public static event Action<TurnState> OnTurnStateChanged;

    public void StartTurnManaging()//called by battleManager
    {
        UpdateTurnState(TurnState.BattleStart);
    }

    private void UpdateTurnState(TurnState newState)
    {
        currentTurnState = newState;

        switch (newState)
        {
            case TurnState.BattleStart:
                HandleBattleStart();
                OnBattleStart?.Invoke();
                break;
            case TurnState.TurnStart:
                HandleTurnStart();
                OnTurnStart?.Invoke(currentSide);
                break;
            case TurnState.WaitForCurrentPlayer:
                HandleWaitForCurrentPlayer();
                OnWaitForPlayerTakeTurn?.Invoke(currentSide);
                break;
            case TurnState.TurnEnd:
                HandleTurnEnd();
                OnTurnEnd?.Invoke(currentSide);
                break;
        }

        OnTurnStateChanged?.Invoke(newState);
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
        UpdateTurnState(nextState);
    }

    private void HandleBattleStart()
    {
        turnCount = 1;
        UpdateTurnState(TurnState.TurnStart);
    }

    private void HandleTurnStart()
    {
        UpdateTurnState(TurnState.WaitForCurrentPlayer);
    }

    private void HandleWaitForCurrentPlayer()
    {
        
    }

    private void HandleTurnEnd()
    {
        currentSide = currentSide.Opposite();
        turnCount += 1;
        UpdateTurnState(TurnState.TurnStart);
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
