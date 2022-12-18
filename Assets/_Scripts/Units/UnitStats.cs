using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    public Attribute attack;
    public Attribute health;
    public Attribute maxHealth;
    public Attribute shield;

    public UnitStats(int attackBase, int healthBase, int maxHealthBase, int shieldBase)
    {
        StatDefinitionDatabase database = Resources.Load<StatDefinitionDatabase>("SO/StatDefinition/_StatDefinitionDatabase");
        attack = new Attribute(database.attack, attackBase);
        health = new Attribute(database.health, healthBase);
        maxHealth = new Attribute(database.maxHealth, maxHealthBase);
        shield = new Attribute(database.shield, shieldBase);
    }
}
