using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Limit_Base
{
    public int times = -1; // -1 = no limit
    public LimitBase_Per per;
}

public enum LimitBase_Per
{
    Battle,
    Turn
}