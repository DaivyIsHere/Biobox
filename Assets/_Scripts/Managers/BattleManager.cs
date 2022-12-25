using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Reference")]
    [InlineEditor(InlineEditorModes.GUIOnly)]
    public List<Box> leftBoxes;
    [InlineEditor(InlineEditorModes.GUIOnly)]
    public List<Box> rightBoxes;

    public Player leftPlayer;
    public Player rightPlayer;
    [SerializeField] private BoardLayout _boardLayout;

    [Header("Prefabs")]
    [SerializeField] private GameObject _boxPref;
    [SerializeField] private GameObject _unitPref;

    //BattleEvent
    // public event Action<Unit, BoxSide> OnSide_UnitBeforeAttack;
    // public event Action<Unit, BoxSide> OnSide_UnitAfterAttack;
    // public event Action<Unit, BoxSide> OnSide_UnitBeforeTakeHit;
    // public event Action<Unit, BoxSide> OnSide_UnitAfterTakeHit;
    // public event Action<Unit, BoxSide> OnSide_UnitAfterShieldBreak;
    // public event Action<Unit, BoxSide> OnSide_UnitAfterTakeDamage;
    // public event Action<Unit, BoxSide> OnSide_UnitAfterSummoned;
    // public event Action<Unit, BoxSide> OnSide_UnitBeforeDeath;

    // public event Action<Unit> OnLeftSide_UnitBeforeAttack;
    // public event Action<Unit> OnLeftSide_UnitAfterAttack;
    // public event Action<Unit> OnLeftSide_UnitBeforeTakeHit;
    // public event Action<Unit> OnLeftSide_UnitAfterTakeHit;
    // public event Action<Unit> OnLeftSide_UnitAfterShieldBreak;
    // public event Action<Unit> OnLeftSide_UnitAfterTakeDamage;
    // public event Action<Unit> OnLeftSide_UnitAfterSummoned;
    // public event Action<Unit> OnLeftSide_UnitBeforeDeath;

    // public event Action<Unit> OnRightSide_UnitBeforeAttack;
    // public event Action<Unit> OnRightSide_UnitAfterAttack;
    // public event Action<Unit> OnRightSide_UnitBeforeTakeHit;
    // public event Action<Unit> OnRightSide_UnitAfterTakeHit;
    // public event Action<Unit> OnRightSide_UnitAfterShieldBreak;
    // public event Action<Unit> OnRightSide_UnitAfterTakeDamage;
    // public event Action<Unit> OnRightSide_UnitAfterSummoned;
    // public event Action<Unit> OnRightSide_UnitBeforeDeath;

    void Start()
    {
        InitializeBattle();
    }

    public void InitializeBattle()
    {
        SpawnAllBoxes();
        SpawnAllUnitsInBoxes();
        TurnManager.Instance.StartTurnManaging();
    }

    private void SpawnAllBoxes()
    {
        //Left
        for (int i = 0; i < GameManager.Instance.leftBoxesUnits.Count; i++)
        {
            var boxData = GameManager.Instance.leftBoxesUnits[i];

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxBoardPosition(BoxSide.LeftSide, i), Quaternion.identity, _boardLayout.boxesContainer).GetComponent<Box>();
            newBox.boxData = boxData;
            newBox.boxLabel = new BoxLabel(BoxSide.LeftSide, i);
            newBox.gameObject.name = newBox.boxLabel.GetName();
            leftBoxes.Add(newBox);
        }

        //Right
        for (int i = 0; i < GameManager.Instance.rightBoxesUnits.Count; i++)
        {
            var boxData = GameManager.Instance.rightBoxesUnits[i];

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxBoardPosition(BoxSide.RightSide, i), Quaternion.identity, _boardLayout.boxesContainer).GetComponent<Box>();
            newBox.boxData = boxData;
            newBox.boxLabel = new BoxLabel(BoxSide.RightSide, i);
            newBox.gameObject.name = newBox.boxLabel.GetName();
            rightBoxes.Add(newBox);
        }
    }

    private void SpawnAllUnitsInBoxes()
    {
        //left
        foreach (var box in leftBoxes)
        {
            for (int i = 0; i < box.boxData.unitDataList.Count; i++)
            {
                Unit newUnit = Instantiate(_unitPref, box.unitsContainer).GetComponent<Unit>();
                newUnit.transform.localPosition = _boardLayout.GetUnitBoardPosition(BoxSide.LeftSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.unitLabel = new UnitLabel(box.boxLabel, i);
                newUnit.gameObject.name = newUnit.unitData.unitName + "_" + i;
                IDManager.Instance.RegisterNewUnitCID(newUnit);
                newUnit.IniUnit();
                box.unitList.Add(newUnit);
            }
        }

        //right
        foreach (var box in rightBoxes)
        {
            for (int i = 0; i < box.boxData.unitDataList.Count; i++)
            {
                Unit newUnit = Instantiate(_unitPref, box.unitsContainer).GetComponent<Unit>();
                newUnit.transform.localPosition = _boardLayout.GetUnitBoardPosition(BoxSide.RightSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.unitLabel = new UnitLabel(box.boxLabel, i);
                newUnit.gameObject.name = newUnit.unitData.unitName + "_" + i;
                IDManager.Instance.RegisterNewUnitCID(newUnit);
                newUnit.IniUnit();
                box.unitList.Add(newUnit);
            }
        }
    }

    private void UpdateUnitLabels(BoxSide side)
    {
        if (side == BoxSide.LeftSide)
        {
            foreach (var box in leftBoxes)
            {
                for (int i = 0; i < box.unitList.Count; i++)
                {
                    box.unitList[i].unitLabel.orderInBox = i;
                }
            }
        }
        else if (side == BoxSide.RightSide)
        {
            foreach (var box in rightBoxes)
            {
                for (int i = 0; i < box.unitList.Count; i++)
                {
                    box.unitList[i].unitLabel.orderInBox = i;
                }
            }
        }
    }

    public IEnumerator AlignAllUnitsCommand()//should only be called from CCAlignUnits
    {
        float alignDuration = 0.5f;

        foreach (var box in leftBoxes)
        {
            for (int i = 0; i < box.unitList.Count; i++)
            {
                box.unitList[i].unitAnimation.PlayAlign(_boardLayout.GetUnitBoardPosition(BoxSide.LeftSide, i), alignDuration);
            }
        }
        foreach (var box in rightBoxes)
        {
            for (int i = 0; i < box.unitList.Count; i++)
            {
                box.unitList[i].unitAnimation.PlayAlign(_boardLayout.GetUnitBoardPosition(BoxSide.RightSide, i), alignDuration);
            }
        }

        yield return new WaitForSeconds(alignDuration);
        CCommand.CommandExecutionComplete();
    }

    //return the target that unit will attack
    public Unit GetFirstTargetByLabel(UnitLabel label)
    {
        // TODO We will check if there's taunt or unattackable unit here later
        UnitLabel targetLabel = label;//copy label info
        targetLabel.boxLabel.boxSide = targetLabel.boxLabel.boxSide.Opposite();
        targetLabel.orderInBox = 0;

        return targetLabel.GetUnit();
    }

    //Use label.GetUnit() instead. (From Extension.cs)
    public Unit GetUnitByLabel(UnitLabel label)
    {
        List<Box> boxes = GetBoxesBySide(label.boxSide);

        if (boxes.Count > label.boxNum)
        {
            if (boxes[label.boxNum].unitList.Count > label.orderInBox)
            {
                return boxes[label.boxNum].unitList[label.orderInBox];
            }
        }

        Debug.LogWarning("No unit found with this label");
        return null;
    }

    //Use label.GetBox() for a single box instead.
    public List<Box> GetBoxesBySide(BoxSide side)
    {
        if (side == BoxSide.LeftSide)
            return leftBoxes;
        else
            return rightBoxes;
    }

    public void DespawnUnit(Unit unit)
    {
        unit.unitLabel.GetBox().unitList.Remove(unit);
        UpdateUnitLabels(unit.unitLabel.boxSide);
    }

    public UnitCID GetRelativeUnit(UnitCID selfCID, TargetRelative_Position relativePosition)
    {
        Unit selfUnit = selfCID.GetUnit();
        BoxLabel selfBox = selfUnit.unitLabel.boxLabel;
        List<Unit> unitsInBox = selfBox.GetBox().unitList;

        switch (relativePosition)
        {
            case TargetRelative_Position.Self:
                return selfCID;
            case TargetRelative_Position.Behind:
                if (selfUnit.unitLabel.orderInBox + 1 >= unitsInBox.Count)
                    return new UnitCID(-1);//null unit
                else
                    return unitsInBox[selfUnit.unitLabel.orderInBox + 1].unitCID;
            case TargetRelative_Position.Front:
                if (selfUnit.unitLabel.orderInBox <= 0)
                    return new UnitCID(-1);//null unit
                else
                    return unitsInBox[selfUnit.unitLabel.orderInBox - 1].unitCID;
        }

        return new UnitCID(-1);
    }

    public List<UnitCID> GetAllUnitCIDByBox(BoxLabel label)
    {
        List<UnitCID> unitCIDs = new List<UnitCID>();
        foreach (var u in label.GetBox().unitList)
        {
            unitCIDs.Add(u.unitCID);
        }
        return unitCIDs;
    }

    public List<UnitCID> GetAboslutePositionUnit(UnitCID selfCID, TargetAbsolutePosition_RelativeBox relativeBox, TargetAbsolutePosition_Position position, int targetCount)
    {
        List<UnitCID> unitCIDs = new List<UnitCID>();
        Unit selfUnit = selfCID.GetUnit();

        //Get boxLabel
        BoxLabel targetBox = new BoxLabel();
        if (relativeBox == TargetAbsolutePosition_RelativeBox.SelfBox)
        {
            targetBox = selfUnit.unitLabel.boxLabel;
        }
        else if (relativeBox == TargetAbsolutePosition_RelativeBox.OppositeBox)
        {
            targetBox = selfUnit.unitLabel.boxLabel;
            targetBox.boxSide = targetBox.boxSide.Opposite();
        }

        //GetAllUnits
        List<Unit> unitsInBox = targetBox.GetBox().unitList;

        //Clamp target
        int availableCount = Mathf.Clamp(targetCount, 0, unitsInBox.Count);

        //Return targets
        switch (position)
        {
            case TargetAbsolutePosition_Position.All:
                return GetAllUnitCIDByBox(targetBox);
            case TargetAbsolutePosition_Position.First:
                for (int i = 0; i < availableCount; i++)
                {
                    unitCIDs.Add(unitsInBox[i].unitCID);
                }
                return unitCIDs;
            case TargetAbsolutePosition_Position.Last:
                for (int i = unitsInBox.Count - 1; i > (unitsInBox.Count - 1) - availableCount; i--)
                {
                    unitCIDs.Add(unitsInBox[i].unitCID);
                }
                return unitCIDs;
            case TargetAbsolutePosition_Position.Random:
                for (int i = 0; i < availableCount; i++)
                {
                    ///This is unique random, means no repeat target selected. 
                    ///Use another action to do deal dmg to random target multiple times
                    bool repeated = true;
                    int random = 0;
                    while (repeated)
                    {
                        random = UnityEngine.Random.Range(0, unitsInBox.Count);
                        repeated = false;
                        foreach (var uid in unitCIDs)
                        {
                            if (uid.id == unitsInBox[random].unitCID.id)
                                repeated = true;
                        }
                    }
                    unitCIDs.Add(unitsInBox[random].unitCID);
                }
                return unitCIDs;
        }

        return unitCIDs;
    }

    #region EventHooks

    public void InvokeEvent_UnitBeforeAttack(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeAttack();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeAttack();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeAttack();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeAttack();
        }
    }

    public void InvokeEvent_UnitAfterAttack(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterAttack();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterAttack();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterAttack();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterAttack();
        }
    }

    public void InvokeEvent_UnitBeforeTakeHit(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeTakeHit();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeTakeHit();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeTakeHit();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeTakeHit();
        }
    }

    public void InvokeEvent_UnitAfterTakeHit(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterTakeHit();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterTakeHit();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterTakeHit();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterTakeHit();
        }
    }

    public void InvokeEvent_UnitAfterShieldBreak(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterShieldBreak();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterShieldBreak();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterShieldBreak();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterShieldBreak();
        }
    }
    
    public void InvokeEvent_UnitAfterTakeDamage(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterTakeDamage();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterTakeDamage();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterTakeDamage();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterTakeDamage();
        }
    }

    public void InvokeEvent_UnitAfterSummoned(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterSummoned();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterSummoned();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_AfterSummoned();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_AfterSummoned();
        }
    }

    public void InvokeEvent_UnitBeforeDeath(Unit unit)
    {
        if (unit.unitLabel.boxSide == BoxSide.LeftSide)
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeDeath();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeDeath();
        }
        else
        {
            foreach (var u in leftPlayer.allOwnUnits)
                u.unitBattle.OnEnemy_BeforeDeath();
            foreach (var u in rightPlayer.allOwnUnits)
                u.unitBattle.OnAlly_BeforeDeath();
        }
    }


    #endregion
}
