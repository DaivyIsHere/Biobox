using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
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
        SpawnAllBoxes();
        SpawnAllUnitsInBoxes();
    }

    private void SpawnAllBoxes()
    {
        //Left
        for (int i = 0; i < GameManager.Instance.leftBoxesUnits.Count; i++)
        {
            var boxData = GameManager.Instance.leftBoxesUnits[i];

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxBoardPosition(BoxSide.LeftSide, i), Quaternion.identity, _boardLayout.transform).GetComponent<Box>();
            newBox.boxData = boxData;
            newBox.boxLabel = new BoxLabel(BoxSide.LeftSide, i);
            newBox.gameObject.name = newBox.boxLabel.GetName();
            leftBoxes.Add(newBox);
        }

        //Right
        for (int i = 0; i < GameManager.Instance.rightBoxesUnits.Count; i++)
        {
            var boxData = GameManager.Instance.rightBoxesUnits[i];

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxBoardPosition(BoxSide.RightSide, i), Quaternion.identity, _boardLayout.transform).GetComponent<Box>();
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
                Unit newUnit = Instantiate(_unitPref, box.transform).GetComponent<Unit>();
                newUnit.transform.localPosition = _boardLayout.GetUnitBoardPosition(BoxSide.LeftSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.unitLabel = new UnitLabel(box.boxLabel, i);
                newUnit.gameObject.name = newUnit.unitData.unitName + "_" + i;
                IDManager.Instance.RegisterNewUnitCID(newUnit);
                box.unitList.Add(newUnit);
            }
        }

        //right
        foreach (var box in rightBoxes)
        {
            for (int i = 0; i < box.boxData.unitDataList.Count; i++)
            {
                Unit newUnit = Instantiate(_unitPref, box.transform).GetComponent<Unit>();
                newUnit.transform.localPosition = _boardLayout.GetUnitBoardPosition(BoxSide.RightSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.unitLabel = new UnitLabel(box.boxLabel, i);
                newUnit.gameObject.name = newUnit.unitData.unitName + "_" + i;
                IDManager.Instance.RegisterNewUnitCID(newUnit);
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

    public IEnumerator AlignAllUnitsCommand(BoxSide side)//should only be called from CCAlignUnits
    {
        float alignDuration = 0.5f;

        if (side == BoxSide.LeftSide)
        {
            foreach (var box in leftBoxes)
            {
                for (int i = 0; i < box.unitList.Count; i++)
                {
                    box.unitList[i].unitAnimation.PlayAlign(_boardLayout.GetUnitBoardPosition(BoxSide.LeftSide, i), alignDuration);
                }
            }
        }
        else if (side == BoxSide.RightSide)
        {
            foreach (var box in rightBoxes)
            {
                for (int i = 0; i < box.unitList.Count; i++)
                {
                    box.unitList[i].unitAnimation.PlayAlign(_boardLayout.GetUnitBoardPosition(BoxSide.RightSide, i), alignDuration);
                }
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

    //Use label.GetBox() instead.
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
        new CCAlignUnits(unit.unitLabel.boxSide).AddToQueue();
        //Destroy(unit.gameObject);

        //AlignAllUnits(unit.unitLabel.boxSide);//Align will set label and create a AlignCommand
    }

}
