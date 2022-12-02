using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxSide playerSide;
    [SerializeField] private List<Unit> _allOwnUnits = new List<Unit>();

    void Awake() 
    {
        TurnManager.OnTurnStart += OnTurnStart;
    }

    void OnDestroy() 
    {
        
    }

    void Start() 
    {
        AllOwnUnits();
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.F))
            print(IsAllUnitExhauseted());

        if(Input.GetKeyDown(KeyCode.R))
            UnexhaustAllUnits();
    }

    public void OnTurnStart(TurnState turnState)
    {
        if(TurnManager.Instance.IsCurrentSide(playerSide) && IsAllUnitExhauseted())
            UnexhaustAllUnits();
    }

    public List<Unit> AllOwnUnits()
    {
        _allOwnUnits.Clear();
        foreach (var b in CombatManager.Instance.GetBoxesBySide(playerSide))
        {
            foreach (var u in b.unitList)
            {
                _allOwnUnits.Add(u);
            }
        }

        return _allOwnUnits;
    }

    public bool IsAllUnitExhauseted()
    {
        foreach (var u in AllOwnUnits())
        {
            if(u.unitCombat.exhausted == false)
                return false;
        }

        return true;
    }

    public void UnexhaustAllUnits()
    {
        foreach (var u in AllOwnUnits())
        {
            u.unitCombat.exhausted = false;
            u.unitAnimation.PlayUnexhaust();
        }
    }

}
