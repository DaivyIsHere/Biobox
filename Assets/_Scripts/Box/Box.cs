using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Unit")]
    public BoxData boxData;
    public List<Unit> unitList;

    [Header("Layout")]
    public BoxSide boxSide;
    public int boxNum = 1;
    
    private float _unitWidth = 3;
    private float _unitSpacing = 0.5f;
}

public enum BoxSide
{
    LeftSide,
    RightSide
}