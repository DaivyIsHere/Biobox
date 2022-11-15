using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [Header("Reference")]
    public List<Box> leftBoxes;
    public List<Box> rightBoxes;
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

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxSpawnPosition(BoxSide.LeftSide, i), Quaternion.identity, _boardLayout.transform).GetComponent<Box>();
            newBox.boxData = boxData;
            newBox.boxNum = i;
            newBox.boxSide = BoxSide.LeftSide;
            leftBoxes.Add(newBox);
        }

        //Right
        for (int i = 0; i < GameManager.Instance.rightBoxesUnits.Count; i++)
        {
            var boxData = GameManager.Instance.rightBoxesUnits[i];

            Box newBox = Instantiate(_boxPref, _boardLayout.GetBoxSpawnPosition(BoxSide.RightSide, i), Quaternion.identity, _boardLayout.transform).GetComponent<Box>();
            newBox.boxData = boxData;
            newBox.boxNum = i;
            newBox.boxSide = BoxSide.RightSide;
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
                newUnit.transform.localPosition = _boardLayout.GetUnitSpawnPosition(BoxSide.LeftSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.boxNum = box.boxNum;
                newUnit.boxSide = box.boxSide;
                box.unitList.Add(newUnit);
            }
        }

        //right
        foreach (var box in rightBoxes)
        {
            for (int i = 0; i < box.boxData.unitDataList.Count; i++)
            {
                Unit newUnit = Instantiate(_unitPref, box.transform).GetComponent<Unit>();
                newUnit.transform.localPosition = _boardLayout.GetUnitSpawnPosition(BoxSide.RightSide, i);
                newUnit.unitData = box.boxData.unitDataList[i];
                newUnit.boxNum = box.boxNum;
                newUnit.boxSide = box.boxSide;
                box.unitList.Add(newUnit);
            }
        }
    }


    //return the target that unit will attack
    public Unit GetFirstTargetByUnit(Unit unit)
    {
        //if (boxNum == 1 && side == BoxSide.LeftSide)

        return null;
    }
}
