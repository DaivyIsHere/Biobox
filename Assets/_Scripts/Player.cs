using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxSide playerSide;
    [SerializeField] public List<Box> allOwnBoxes = new List<Box>();
    [SerializeField] public List<Unit> allOwnUnits = new List<Unit>();

    void Awake()
    {
        TurnManager.OnBattleStart += OnBattleStart;
        TurnManager.OnTurnStart += OnTurnStart;
        TurnManager.OnWaitForPlayerTakeTurn += OnWaitForPlayerTakeTurn;
        TurnManager.OnPlayerTakeTurn += OnPlayerTakeTurn;
        TurnManager.OnTurnEnd += OnTurnEnd;
        TurnManager.OnTurnStateChanged += OnTurnStateChanged;
    }

    void OnDestroy()
    {
        TurnManager.OnBattleStart -= OnBattleStart;
        TurnManager.OnTurnStart -= OnTurnStart;
        TurnManager.OnWaitForPlayerTakeTurn -= OnWaitForPlayerTakeTurn;
        TurnManager.OnPlayerTakeTurn -= OnPlayerTakeTurn;
        TurnManager.OnTurnEnd -= OnTurnEnd;
        TurnManager.OnTurnStateChanged -= OnTurnStateChanged;
    }

    void Update()
    {

    }

    private void OnBattleStart()
    {
        AllOwnBoxes();
        AllOwnUnits();

        //UnitTrigger
        foreach (var u in allOwnUnits)
            u.unitBattle.OnBattleStart();
    }

    private void OnTurnEnd(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
            
            //UnitTrigger
            foreach (var u in allOwnUnits)
                u.unitBattle.OnSelfTurnEnd();
        }
        else
        {
            //UnitTrigger
            foreach (var u in allOwnUnits)
                u.unitBattle.OnOppoTurnEnd();
        }
    }

    private void OnTurnStart(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
            //UnitTrigger
            foreach (var u in allOwnUnits)
                u.unitBattle.OnSelfTurnStart();
        }
        else
        {
            //UnitTrigger
            foreach (var u in allOwnUnits)
                u.unitBattle.OnOppoTurnStart();
        }
    }

    private void OnWaitForPlayerTakeTurn(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
            if (IsAllUnitExhauseted())
                UnexhaustAllUnits();

            TurnAvaliableVisual();
            EnableAllUnitsAttack();
        }
    }

    private void OnPlayerTakeTurn(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
            TurnUnavaliableVisual();
            DisableAllUnitsAttack();
        }
    }

    private void OnTurnStateChanged(TurnState newState, BoxSide side)
    {

    }

    public List<Unit> AllOwnUnits()
    {
        allOwnUnits.Clear();
        foreach (var b in BattleManager.Instance.GetBoxesBySide(playerSide))
        {
            foreach (var u in b.unitList)
            {
                allOwnUnits.Add(u);
            }
        }

        return allOwnUnits;
    }

    public List<Box> AllOwnBoxes()
    {
        allOwnBoxes.Clear();
        foreach (var b in BattleManager.Instance.GetBoxesBySide(playerSide))
        {
            allOwnBoxes.Add(b);
        }

        return allOwnBoxes;
    }

    private bool IsAllUnitExhauseted()
    {
        foreach (var u in AllOwnUnits())
        {
            if (u.unitBattle.IsExhausted() == false)
                return false;
        }

        return true;
    }

    private void UnexhaustAllUnits()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitBattle.Unexhaust();
            u.unitAnimation.PlayUnexhaust();
        }
    }

    private void EnableAllUnitsAttack()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitBattle.canAttack = true;
        }
    }

    private void DisableAllUnitsAttack()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitBattle.canAttack = false;
        }
    }

    private void TurnAvaliableVisual()
    {

    }

    private void TurnUnavaliableVisual()
    {

    }
}
