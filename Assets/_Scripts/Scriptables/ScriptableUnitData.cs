using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewUnitData" , menuName = "Biobox/UnitData" , order = 1)]
public class ScriptableUnitData : ScriptableObject
{
    public string unitId;

    [Header("General Info")]
    public string unitName;
    [TextArea(2,5)]
    public string description;
    public Sprite sprite;
    
    [Header("Unit Stats")]  
    public UnitStats stats;

    private void OnValidate()
    {
        //Validate Stats
        stats.maxHealth = stats.maxHealth > 0 ? stats.maxHealth : 1;
        stats.health = stats.maxHealth; // currently we want it to be the same as maxhealth on start
        
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
public struct UnitStats
{
    public int attack;
    public int maxHealth;
    public int health;
}
