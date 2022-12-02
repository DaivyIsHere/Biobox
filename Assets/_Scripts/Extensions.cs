using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        Color newColor = renderer.color;
        newColor.a = alpha;
        renderer.color = newColor;
        
        //Now we can just call SpriteRenderer.Fade(value);
    }

    //Get the opposite side
    public static BoxSide Opposite(this BoxSide side)
    {
        if(side == BoxSide.LeftSide)
            return BoxSide.RightSide;
        else
            return BoxSide.LeftSide;
    }

    //A very easy way to get unit by label, need CombatManager to exist
    public static Unit GetUnit(this UnitLabel label)
    {
        if(!BattleManager.Instance)
        {
            Debug.LogError("Cannot find CombatManager.");
            return null;
        }

        return BattleManager.Instance.GetUnitByLabel(label);
    }

    public static Unit GetUnit(this UnitCID unitCID)
    {
        if(!IDManager.Instance)
        {
            Debug.LogError("Cannot find IDManager.");
            return null;
        }

        return IDManager.Instance.GetUnitWithID(unitCID);
    }

    public static Box GetBox(this BoxLabel label)
    {
        if(!BattleManager.Instance)
        {
            Debug.LogError("Cannot find CombatManager.");
            return null;
        }

        return BattleManager.Instance.GetBoxesBySide(label.boxSide)[label.boxNum];
    }

    public static Box GetBox(this UnitLabel label)
    {
        if(!BattleManager.Instance)
        {
            Debug.LogError("Cannot find CombatManager.");
            return null;
        }

        return BattleManager.Instance.GetBoxesBySide(label.boxSide)[label.boxNum];
    }
}
