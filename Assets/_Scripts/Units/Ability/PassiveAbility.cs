using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewPassiveAbility", menuName = "Biobox/Ability/PassiveAbility", order = 1)]
public class PassiveAbility : ScriptableAbility
{
    //Editor Cache
    public bool showTrigger = false;
    public bool showCondition = false;
    public bool showAction = false;
    public bool showTarget = false;
    public bool showLimit = false;
    public int triggerTypeIndex = 0;
    public int conditionTypeIndex = 0;
    public int actionTypeIndex = 0;
    public int targetTypeIndex = 0;

    //[Header("Trigger")]
    [SerializeReference] public Trigger_Base trigger = new Trigger_Base();//new instance otherwise error on editor
    [HideInInspector]
    public List<Type> triggerTypeList = new List<Type>
    {
        typeof(Trigger_Base),
        typeof(Trigger_BattleEvent),
    };


    //[Header("Condition")]
    [SerializeReference] public List<Condition_Base> conditions = new List<Condition_Base>();
    [HideInInspector]
    public List<Type> conditionTypeList = new List<Type>
    {
        typeof(Condition_Base),
        typeof(Condition_SelfStat),
        typeof(Condition_SelfTargetStat),
        typeof(Condition_TargetStat)
    };

    //[Header("Action")]
    [SerializeReference] public List<Action_Base> actions = new List<Action_Base>();
    [HideInInspector]
    public List<Type> actionTypeList = new List<Type>
    {
        typeof(Action_Base),
        typeof(Action_StatMod),
        typeof(Action_StatusEffect)
    };

    //[Header("Target")]
    [SerializeReference] public Target_Base target = new Target_Base();
    [HideInInspector]
    public List<Type> targetTypeList = new List<Type>
    {
        typeof(Target_Base),
        typeof(Target_Relative),
    };

    //[Header("Limit")]
    [SerializeReference] public Limit_Base limit = new Limit_Base();

    public virtual void InitializeAbiltiy(Unit unit)
    {
        trigger.RegisterTrigger(unit, OnTriggerAbility);
    }

    public virtual void OnTriggerAbility(Unit unit)//sub by trigger
    {
        bool conditionMet = true;
        foreach (var c in conditions)
        {
            if(!c.ConditionMet(unit))
                conditionMet = false;
        }

        if(conditionMet)
        {
            PerformAbility();
        }
    }

    public virtual void PerformAbility()//called by OnTriggerAbility
    {
        Debug.Log("Perform ability");
    }

    public void EditorApplyTriggerType()
    {
        trigger = (Trigger_Base)Activator.CreateInstance(triggerTypeList[triggerTypeIndex]);
    }

    public void EditorApplyConditionType()
    {
        conditions.Add((Condition_Base)Activator.CreateInstance(conditionTypeList[conditionTypeIndex]));
    }

    public void EditorApplyActionType()
    {
        actions.Add((Action_Base)Activator.CreateInstance(actionTypeList[actionTypeIndex]));
    }

    public void EditorApplyTargetType()
    {
        target = (Target_Base)Activator.CreateInstance(targetTypeList[targetTypeIndex]);
    }

}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PassiveAbility))]
public class PassiveAbilityEditor : Editor
{
    public Color32 triggerColor = new Color32(136, 193, 157, 255);
    public Color32 conditionColor = new Color32(50, 147, 122, 255);
    public Color32 actionColor = new Color32(74, 126, 221, 255);
    public Color32 targetColor = new Color32(190, 104, 60, 255);
    public Color32 limitColor = new Color32(190, 60, 60, 255);
    public GUIStyle boldStyle = new GUIStyle();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PassiveAbility script = (PassiveAbility)target;
        boldStyle.fontStyle = FontStyle.Bold;

        ///Trigger
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle triggerStyle = new GUIStyle(boldStyle);
        triggerStyle.normal.textColor = triggerColor;
        script.showTrigger = EditorGUILayout.Foldout(script.showTrigger, "    [ Trigger ]", true, triggerStyle);
        if (script.showTrigger)
        {
            DisplayDropdown(script.trigger.GetType(), triggerStyle, script.triggerTypeList, ref script.triggerTypeIndex, script.EditorApplyTriggerType);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trigger"), true);
        }

        ///Condition
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle conditionStyle = new GUIStyle(boldStyle);
        conditionStyle.normal.textColor = conditionColor;
        script.showCondition = EditorGUILayout.Foldout(script.showCondition, "    [ Condition ]", true, conditionStyle);
        if (script.showCondition)
        {
            DisplayDropdown(script.conditions.GetType(), conditionStyle, script.conditionTypeList, ref script.conditionTypeIndex, script.EditorApplyConditionType);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("conditions"), true);
        }

        ///Action
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle actionStyle = new GUIStyle(boldStyle);
        actionStyle.normal.textColor = actionColor;
        script.showAction = EditorGUILayout.Foldout(script.showAction, "    [ Action ]", true, actionStyle);
        if (script.showAction)
        {
            DisplayDropdown(script.actions.GetType(), actionStyle, script.actionTypeList, ref script.actionTypeIndex, script.EditorApplyActionType);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("actions"), true);
        }

        ///Target
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle targetStyle = new GUIStyle(boldStyle);
        targetStyle.normal.textColor = targetColor;
        script.showTarget = EditorGUILayout.Foldout(script.showTarget, "    [ Target ]", true, targetStyle);
        if (script.showTarget)
        {
            DisplayDropdown(script.target.GetType(), targetStyle, script.targetTypeList, ref script.targetTypeIndex, script.EditorApplyTargetType);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"), true);
        }

        ///Limit
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle limitStyle = new GUIStyle(boldStyle);
        limitStyle.normal.textColor = limitColor;
        script.showLimit = EditorGUILayout.Foldout(script.showLimit, "    [ Limit ]", true, limitStyle);
        if (script.showLimit)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("limit"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void DisplayDropdown(Type currentType, GUIStyle infoStyle, List<Type> typeList, ref int dropDownIndex, Action applyAction)
    {
        PassiveAbility script = (PassiveAbility)target;
        //Construct a new list of every actionType's name
        List<string> typeNameList = new List<string>();
        foreach (var t in typeList)
        {
            typeNameList.Add(t.Name);
        }

        //Display dropdown
        EditorGUILayout.BeginHorizontal();
        string[] subStrings = currentType.ToString().Split('_');
        EditorGUILayout.LabelField("<" + subStrings[1] + ">", infoStyle, GUILayout.MaxWidth(100));
        dropDownIndex = EditorGUILayout.Popup(dropDownIndex, typeNameList.ToArray());
        if (GUILayout.Button("Apply"))
            applyAction();
        EditorGUILayout.EndHorizontal();
    }


}
#endif
#endregion
