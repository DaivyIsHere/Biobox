using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Biobox/UnitData", order = 1)]
public class ScriptableUnitData : ScriptableObject
{
    public string unitId;

    [Header("General Info")]
    public string unitName;
    [TextArea(2, 5)]
    public string description;
    public Sprite sprite;

    [Header("Unit BaseStats")]
    public UnitBaseStats baseStats;

    [Header("Unit Ability")]
    public PassiveAbility passiveAbility;

    private void OnValidate()
    {
        // Get a unique identifier from the asset's unique 'Asset Path' (ex : Resources/UniData/Plunny.asset)
        // If you do change it and want to change back, just erase the uniqueID in the inspector and it will refill itself.
        if (unitId == "")
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            unitId = AssetDatabase.AssetPathToGUID(path);
#endif
        }
    }
}

[System.Serializable]
public class UnitBaseStats
{
    public int attackBase;
    public int healthBase;
    public int shieldBase;
}
