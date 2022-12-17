using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action_Base
{
    public virtual void DoAction()
    {
        Debug.Log("This is Base");
    }
}
