using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Action_Base
{
    [PropertyOrder(99)]
    [TabGroup("Target")]
    [ValueDropdown("targetList")]
    [SerializeReference] public Target_Base target;
    private List<Target_Base> targetList = new List<Target_Base>
    {
        new Target_Relative(),
        new Target_AbsolutePosition()
    };
    
    public virtual void DoAction(Unit unit)
    {
        Debug.Log("This is Base Action");
    }
}
