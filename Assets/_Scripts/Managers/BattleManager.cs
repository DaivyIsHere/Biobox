using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Reference")]
    public List<Box> leftBoxes;
    public List<Box> rightBoxes;
    public Player leftPlayer;
    public Player rightPlayer;
    [SerializeField] private BoardLayout _boardLayout;

    [Header("Prefabs")]
    [SerializeField] private GameObject _boxPref;
    [SerializeField] private GameObject _unitPref;

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
        //new CCAlignUnits(unit.unitLabel.boxSide).AddToQueue();
        //Destroy(unit.gameObject);

        //AlignAllUnits(unit.unitLabel.boxSide);//Align will set label and create a AlignCommand
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

    public List<UnitCID> GetAboslutePositionUnit(UnitCID selfCID, TargetAbsolutePosition_RelativeBox relativeBox ,TargetAbsolutePosition_Position position)
    {
        List<UnitCID> unitCIDs = new List<UnitCID>();
        Unit selfUnit = selfCID.GetUnit();
        BoxLabel targetBox = new BoxLabel();//
        if(relativeBox == TargetAbsolutePosition_RelativeBox.SelfBox)
        {
            targetBox = selfUnit.unitLabel.boxLabel;
        }
        else if (relativeBox == TargetAbsolutePosition_RelativeBox.OppositeBox)
        {
            targetBox = selfUnit.unitLabel.boxLabel;
            targetBox.boxSide = targetBox.boxSide.Opposite();
        }
        List<Unit> unitsInBox = targetBox.GetBox().unitList;

        switch (position)
        {
            case TargetAbsolutePosition_Position.All:
                return GetAllUnitCIDByBox(targetBox);
            case TargetAbsolutePosition_Position.First:
                unitCIDs.Add(unitsInBox[0].unitCID);
                return unitCIDs;
            case TargetAbsolutePosition_Position.Last:
                unitCIDs.Add(unitsInBox[unitsInBox.Count-1].unitCID);
                return unitCIDs;
            case TargetAbsolutePosition_Position.Random:
                int random = Random.Range(0, unitsInBox.Count);
                unitCIDs.Add(unitsInBox[random].unitCID);
                return unitCIDs;
        }

        return unitCIDs;
    }
}
