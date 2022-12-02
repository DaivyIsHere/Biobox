using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : Singleton<TurnManager>
{
    [Header("TurnState")]
    public TurnState currentTurnState;
    public BoxSide currentSide;// which side's turn
    public bool isResolving;

    public static event Action<TurnState> OnTurnStateChanged;
    public static event Action<TurnState> OnTurnStart;

    void Start()
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
                break;
            case TurnState.TurnStart:
                HandleTurnStart();
                OnTurnStart?.Invoke(TurnState.TurnStart);
                break;
            case TurnState.WaitForCurrentPlayer:
                HandleWaitForCurrentPlayer();
                break;
            case TurnState.TurnEnd:
                HandleTurnEnd();
                break;
        }

        OnTurnStateChanged?.Invoke(newState);
    }

    public void OnPlayerTakeTurn()
    {
        isResolving = true;
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
