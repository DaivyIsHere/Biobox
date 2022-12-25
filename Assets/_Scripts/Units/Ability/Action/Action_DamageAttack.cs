using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Action_DamageAttack : Action_Base
{
    [TabGroup("Damage")]
    public int damage;

    public override void DoAction(Unit selfUnit)
    {
        List<UnitCID> targetUnits = target.GetAllTargets(selfUnit);
        foreach (var u in targetUnits)
        {
            if(!u.GetUnit().unitBattle.IsDead())
            {
                u.GetUnit().unitBattle.InvokeBeforeTakeHit();
                u.GetUnit().unitBattle.TakeDamage(damage);
            }
        }

    }
}
