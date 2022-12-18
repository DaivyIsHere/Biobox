using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDefinitionDatabase", menuName = "Biobox/StatDefinitionDatabase", order = 1)]
public class StatDefinitionDatabase : ScriptableObject
{
    public StatDefinition attack;
    public StatDefinition health;
    public StatDefinition maxHealth;
    public StatDefinition shield;
}
