using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    [field: Header("Component")]
    [field: SerializeField] public UnitAnimation unitAnimation { get; private set; }
    [field: SerializeField] public UnitCombat unitCombat { get; private set; }

    [Header("Info")]
    public ScriptableUnitData unitData;
    public UnitLabel unitLabel;
    public UnitCID unitCID;

    [Header("Display")]
    [SerializeField] private SpriteRenderer _unitSprite;
    [SerializeField] private TextMeshPro _nameDisplay;
    [SerializeField] private TextMeshPro _healthDisplay;
    [SerializeField] private TextMeshPro _attackDisplay;
    [SerializeField] private TextMeshPro _shieldDisplay;

    #region MonoCall

    void Start()
    {
        IniUnitDisplay();
    }

    #endregion

    private void IniUnitDisplay()
    {
        if (unitData == null)
            return;

        _nameDisplay.text = unitData.name;
        _unitSprite.sprite = unitData.sprite;
    }

    public void UpdateAttackDisplay(int newValue)
    {
        _attackDisplay.text = newValue.ToString();
    }

    public void UpdateHealthDisplay(int newValue)
    {
        _healthDisplay.text = newValue.ToString();
    }

    public void UpdateShieldDisplay(int newValue)
    {
        _shieldDisplay.text = newValue.ToString();
        if(newValue > 0)
            _shieldDisplay.gameObject.SetActive(true);
        else
            _shieldDisplay.gameObject.SetActive(false);
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
    public int id;

    public UnitCID(int id)
    {
        this.id = id;
    }
}
