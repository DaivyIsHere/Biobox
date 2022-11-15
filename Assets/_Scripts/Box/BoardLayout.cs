using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script to get info about all the layout position
public class BoardLayout : MonoBehaviour
{
    [Header("UnitsLayout")]
    private float _unitSpacing = 0.25f;
    private float _unitWidth = 2.5f;

    [Header("BoxesLayout")]
    private float leftBoxPositonX = -1.75f;
    private float rightBoxPositonX = 1.75f;
    private float _boxHeight = 4;
    private float _boxSpacing = 0.5f;

    public Vector3 GetBoxSpawnPosition(BoxSide boxSide, int boxNum)
    {
        Vector3 result = Vector3.zero;

        if(boxSide == BoxSide.LeftSide)
            result.x = leftBoxPositonX;
        else if(boxSide == BoxSide.RightSide)
            result.x = rightBoxPositonX;

        result.y = -1 * boxNum * (_boxHeight + _boxSpacing);
        
        return result;
    }

    public Vector3 GetUnitSpawnPosition(BoxSide boxSide, int order)
    {
        Vector3 result = Vector3.zero;
        if (boxSide == BoxSide.LeftSide)
            result.x = -1 * order * (_unitSpacing + _unitWidth);
        else if (boxSide == BoxSide.RightSide)
            result.x = order * (_unitSpacing + _unitWidth);
        
        return result;
    }

}
