using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action_Base
{
    public Target_AbsolutePosition target;
    
    public virtual void DoAction(Unit unit)
    {
        Debug.Log("This is Base Action");
    }
}
