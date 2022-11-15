using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private UnitAnimation _unitAnimation;
    [SerializeField] private UnitCombat _unitCombat;

    [Header("Info")]
    public ScriptableUnitData unitData;
    public BoxSide boxSide;
    public int boxNum;

    [Header("Display")]
    [SerializeField] private SpriteRenderer _unitSprite;
    [SerializeField] private TextMeshPro _nameDisplay;
    [SerializeField] private TextMeshPro _healthDisplay;
    [SerializeField] private TextMeshPro _attackDisplay;

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
        OnHealthChange();
        OnAttackChange();
    }

    private void OnHealthChange()
    {
        _healthDisplay.text = unitData.stats.health.ToString();
    }

    private void OnAttackChange()
    {
        _attackDisplay.text = unitData.stats.attack.ToString();
    }
}
