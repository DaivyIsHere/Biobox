using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//A script to get info about all the layout position
public class BoardLayout : MonoBehaviour
{
    [Header("Reference")]
    public Transform boxesContainer;

    [Header("UnitsLayout")]
    private float _unitSpacing = 0.25f;
    private float _unitWidth = 2.5f;

    [Header("BoxesLayout")]
    private float leftBoxPositonX = -4.75f;
    private float rightBoxPositonX = 4.75f;
    private float leftFirstUnitPositionX = 3f;
    private float rightFirstUnitPositionX = -3f;
    private float _boxHeight = 4;
    private float _boxSpacing = 0.5f;

    [Header("Turn Indicator")]
    [SerializeField] private Transform _indicatorTransform;
    [SerializeField] private float _indicatorPosX_left;
    [SerializeField] private float _indicatorPosX_right;

    void Awake() 
    {
        TurnManager.OnTurnEnd += ShowIndicator;
    }

    void OnDestroy()
    {
        TurnManager.OnTurnEnd -= ShowIndicator;
    }

    public void ShowIndicator(BoxSide side)
    {
        if (side == BoxSide.LeftSide)
            _indicatorTransform.DOMoveX(_indicatorPosX_left, 0.5f).SetEase(Ease.InOutSine);
        else
            _indicatorTransform.DOMoveX(_indicatorPosX_right, 0.5f).SetEase(Ease.InOutSine);
    }

    public Vector3 GetBoxBoardPosition(BoxSide boxSide, int boxNum)
    {
        Vector3 result = Vector3.zero;

        if (boxSide == BoxSide.LeftSide)
            result.x = leftBoxPositonX;
        else if (boxSide == BoxSide.RightSide)
            result.x = rightBoxPositonX;

        result.y = -1 * boxNum * (_boxHeight + _boxSpacing);

        return result;
    }

    public Vector3 GetUnitBoardPosition(BoxSide boxSide, int order)
    {
        Vector3 result = Vector3.zero;
        if (boxSide == BoxSide.LeftSide)
            result.x = leftFirstUnitPositionX + -1 * order * (_unitSpacing + _unitWidth);
        else if (boxSide == BoxSide.RightSide)
            result.x = rightFirstUnitPositionX + order * (_unitSpacing + _unitWidth);

        return result;
    }

}
