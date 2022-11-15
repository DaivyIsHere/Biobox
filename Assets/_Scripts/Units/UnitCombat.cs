using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitAnimation _unitAnimation;

    [field: Header("Status")]
    [field: SerializeField] public bool canAttack { get; private set; } = true; // read only property

    private void OnMouseDown()
    {
        if (!canAttack)
            return;


        _unitAnimation.PlayHit(this._unit);
        //canAttack = false;
    }

    
}
