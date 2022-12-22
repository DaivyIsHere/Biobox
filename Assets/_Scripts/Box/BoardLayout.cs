using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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

    [Header("Turn Messege")]
    [SerializeField] private Transform TurnMessageDisplay;
    [SerializeField] private Image TurnMessageBG;
    [SerializeField] private TextMeshProUGUI TurnMessageText;
    [SerializeField] private Color TurnEventColor;
    [SerializeField] private Color PlayerControlColor;

    void Awake()
    {
        //TurnManager.OnTurnEnd += ShowIndicator;
        TurnManager.OnTurnStateChanged += ShowTurnMessage;
    }

    void OnDestroy()
    {
        //TurnManager.OnTurnEnd -= ShowIndicator;
        TurnManager.OnTurnStateChanged -= ShowTurnMessage;
    }

    public void ShowIndicator(BoxSide side)
    {
        if (side == BoxSide.RightSide)
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

    public void ShowTurnMessage(TurnState turnState, BoxSide side)
    {
        switch (turnState)
        {
            case TurnState.BattleStart:
                PlayTurnMessage("Battle Start", TurnEventColor);
                break;
            case TurnState.TurnStart:
                PlayTurnMessage("Turn Start", TurnEventColor);
                break;
            case TurnState.WaitForCurrentPlayer:
                if (side == BoxSide.LeftSide)
                    PlayTurnMessage("[ Your Turn ]", PlayerControlColor);
                else
                    PlayTurnMessage("[ Enemy Turn ]", PlayerControlColor);
                break;
            case TurnState.TurnEnd:
                PlayTurnMessage("Turn End", TurnEventColor);
                break;
        }
    }

    public void PlayTurnMessage(string message, Color color)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(TurnMessageText.DOFade(0f, 0.1f));
        sequence.Join(TurnMessageBG.DOFade(0, 0.1f));
        TurnMessageText.text = message;
        TurnMessageText.color = color;
        TurnMessageDisplay.localScale = Vector3.one * 1.05f;
        sequence.Append(TurnMessageDisplay.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutSine));
        sequence.Join(TurnMessageBG.DOFade(0.05f, 0.5f).SetEase(Ease.InOutSine));
        sequence.Join(TurnMessageText.DOFade(1, 0.5f).SetEase(Ease.InOutSine));
    }

}
