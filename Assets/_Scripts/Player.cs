using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxSide playerSide;
    [SerializeField] private List<Box> _allOwnBoxes = new List<Box>();
    [SerializeField] private List<Unit> _allOwnUnits = new List<Unit>();

    void Awake()
    {
        TurnManager.OnBattleStart += OnBattleStart;
        TurnManager.OnTurnStart += OnTurnStart;
        TurnManager.OnWaitForPlayerTakeTurn += OnWaitForPlayerTakeTurn;
        TurnManager.OnPlayerTakeTurn += OnPlayerTakeTurn;
        TurnManager.OnTurnStateChanged += OnTurnStateChanged;
    }

    void OnDestroy()
    {
        TurnManager.OnBattleStart -= OnBattleStart;
        TurnManager.OnTurnStart -= OnTurnStart;
        TurnManager.OnWaitForPlayerTakeTurn -= OnWaitForPlayerTakeTurn;
        TurnManager.OnPlayerTakeTurn -= OnPlayerTakeTurn;
        TurnManager.OnTurnStateChanged -= OnTurnStateChanged;
    }

    void Update()
    {

    }

    private void OnBattleStart()
    {
        AllOwnBoxes();
        AllOwnUnits();
    }

    private void OnTurnStart(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
            if (IsAllUnitExhauseted())
                UnexhaustAllUnits();
        }
    }

    private void OnWaitForPlayerTakeTurn(BoxSide currentSide)
    {
        if (currentSide == playerSide)
        {
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

    private void OnTurnStateChanged(TurnState newState)
    {
        
    }

    public List<Unit> AllOwnUnits()
    {
        _allOwnUnits.Clear();
        foreach (var b in BattleManager.Instance.GetBoxesBySide(playerSide))
        {
            foreach (var u in b.unitList)
            {
                _allOwnUnits.Add(u);
            }
        }

        return _allOwnUnits;
    }

    public List<Box> AllOwnBoxes()
    {
        _allOwnBoxes.Clear();
        foreach (var b in BattleManager.Instance.GetBoxesBySide(playerSide))
        {
            _allOwnBoxes.Add(b);
        }

        return _allOwnBoxes;
    }

    private bool IsAllUnitExhauseted()
    {
        foreach (var u in AllOwnUnits())
        {
            if (u.unitCombat.exhausted == false)
                return false;
        }

        return true;
    }

    private void UnexhaustAllUnits()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitCombat.exhausted = false;
            u.unitAnimation.PlayUnexhaust();
        }
    }

    private void EnableAllUnitsAttack()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitCombat.canAttack = true;
        }
    }

    private void DisableAllUnitsAttack()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitCombat.canAttack = false;
        }
    }

    private void TurnAvaliableVisual()
    {
        
    }

    private void TurnUnavaliableVisual()
    {
        
    }
}
