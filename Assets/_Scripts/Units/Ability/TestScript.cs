using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestScript : MonoBehaviour
{
    public Unit unit;
    public string testS;
    public Trigger_UnitEvent trigger_UnitEvent;
    public UnitLabel unitLabel;

    public void RunTest()
    {
        unit.unitBattle.passiveAbility.trigger.CheckTrigger(unit.unitLabel, unitLabel, trigger_UnitEvent);
        //unit.unitBattle.OnBattleStart();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TestScript))]
public class TestScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Test"))
        {
            ((TestScript)target).RunTest();
        }
    }
}
#endif

