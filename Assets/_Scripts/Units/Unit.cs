using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    [field: Header("Component")]
    [field: SerializeField] public UnitAnimation unitAnimation { get; private set; }
    [field: SerializeField] public UnitBattle unitBattle { get; private set; }

    [Header("Info")]
    public ScriptableUnitData unitData;
    public UnitLabel unitLabel;
    public UnitCID unitCID;

    [Header("Display")]
    [SerializeField] public SpriteRenderer unitSprite;
    [SerializeField] public TextMeshPro nameDisplay;
    [SerializeField] public TextMeshPro healthDisplay;
    [SerializeField] public TextMeshPro attackDisplay;
    [SerializeField] public TextMeshPro shieldDisplay;

    #region MonoCall

    void Start()
    {
        IniUnitDisplay();
    }

    #endregion

    public void IniUnit()
    {
        unitBattle.IniUnitBattle();
    }

    private void IniUnitDisplay()
    {
        if (unitData == null)
            return;

        nameDisplay.text = unitData.name;
        unitSprite.sprite = unitData.sprite;
    }

    public void UpdateAttackDisplay(int newValue)
    {
        attackDisplay.text = newValue.ToString();
    }

    public void UpdateHealthDisplay(int newValue)
    {
        healthDisplay.text = newValue.ToString();
    }

    public void UpdateShieldDisplay(int newValue)
    {
        shieldDisplay.text = newValue.ToString();
        if(newValue > 0)
            shieldDisplay.gameObject.SetActive(true);
        else
            shieldDisplay.gameObject.SetActive(false);
    }

    public void DestroySelf()//should only called by animation.PlayDeath() by CCUnitDie
    {
        IDManager.Instance.RemoveUnitCID(unitCID);
        CCommand.CommandExecutionComplete();
        Destroy(this.gameObject);
    }
}

[System.Serializable]
public struct UnitLabel
{
    public BoxLabel boxLabel;
    public int orderInBox;// 0 = first unit in the front

    //property for quicker access
    public BoxSide boxSide { get { return boxLabel.boxSide; } }
    public int boxNum { get { return boxLabel.boxNum; } }

    public UnitLabel(BoxLabel boxlabel, int unitOrderInBox)
    {
        this.boxLabel = boxlabel;
        this.orderInBox = unitOrderInBox;
    }

    public UnitLabel(BoxSide boxSide, int boxNum, int unitOrderInBox)
    {
        this.boxLabel = new BoxLabel(boxSide, boxNum);
        this.orderInBox = unitOrderInBox;
    }
}

//Unit CombatID
[System.Serializable]
public struct UnitCID
{
    public int id; // use -1 as null unit

    public UnitCID(int id)
    {
        this.id = id;
    }
}
