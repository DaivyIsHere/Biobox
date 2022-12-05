using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    public Stat attack;
    public Attribute health;
    public Stat shield;

    public UnitStats(int attackBase, int healthBase, int shieldBase)
    {
        attack = new Stat(Resources.Load<StatDefinition>("SO/StatDefinition/Attack"), attackBase);
        health = new Attribute(Resources.Load<StatDefinition>("SO/StatDefinition/Health"), healthBase);
        shield = new Stat(Resources.Load<StatDefinition>("SO/StatDefinition/Shield"), shieldBase);
    }
}
