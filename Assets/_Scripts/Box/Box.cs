using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Box : MonoBehaviour
{
    [Header("Reference")]
    public Transform unitsContainer;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer _boxBG;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _highlightColor;
    
    [Header("Unit")]
    public BoxData boxData;
    public List<Unit> unitList;

    [Header("Label")]
    public BoxLabel boxLabel;

    public void HighlightBox()
    {
        _boxBG.color = _highlightColor;
    }

    public void UnhighlightBox()
    {
        _boxBG.color = _defaultColor;
    }
}

public enum BoxSide
{
    LeftSide,
    RightSide
}

[System.Serializable]
public struct BoxLabel
{
    public BoxSide boxSide;
    public int boxNum;

    public BoxLabel(BoxSide side, int num)
    {
        boxSide = side;
        boxNum = num;
    }

    public string GetName()
    {
        StringBuilder sb = new StringBuilder("Box");
        if(boxSide == BoxSide.LeftSide)
            sb.Append("_left");
        else if(boxSide == BoxSide.RightSide)
            sb.Append("_right");
        sb.Append("_" + boxNum.ToString());

        return sb.ToString();
    }
}