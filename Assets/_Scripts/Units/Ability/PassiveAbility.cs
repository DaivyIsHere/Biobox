using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "NewPassiveAbility", menuName = "Biobox/Ability/PassiveAbility", order = 1)]
public class PassiveAbility : ScriptableAbility
{
    //[Header("Action")]
    [SerializeReference] public Action_Base action = new Action_Base();//new instance otherwise error on editor
    [HideInInspector] public int actionTypeIndex = 0;
    [HideInInspector]
    public List<Type> actionTypeList = new List<Type>
    {
        typeof(Action_Base),
        typeof(Action_StatMod),
        typeof(Action_StatusEffect)
    };


    //[Header("Condition")]
    [SerializeReference] public Condition_Base condition = new Condition_Base();
    [HideInInspector] public int conditionTypeIndex = 0;
    [HideInInspector]
    public List<Type> conditionTypeList = new List<Type>
    {
        typeof(Condition_Base),
        typeof(Condition_SelfStat)
    };

    public virtual void InitializeAbiltiy()
    {

    }

    public virtual void PerformAbility()
    {

    }

    public void EditorApplyActionType()
    {
        action = (Action_Base)Activator.CreateInstance(actionTypeList[actionTypeIndex]);
    }

    public void EditorApplyConditionType()
    {
        condition = (Condition_Base)Activator.CreateInstance(conditionTypeList[conditionTypeIndex]);
    }

}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PassiveAbility))]
public class PassiveAbilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        PassiveAbility script = (PassiveAbility)target;
        DisplayDropdown("Action", script.action.GetType(), new Color32(190, 60, 60, 255), script.actionTypeList, ref script.actionTypeIndex, script.EditorApplyActionType);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("action"), true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DisplayDropdown("Condition", script.condition.GetType(), new Color32(50, 147, 122, 255), script.conditionTypeList, ref script.conditionTypeIndex, script.EditorApplyConditionType);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("condition"), true);

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    /*
    public void DisplayAcitonTypeDropDown()
    {
        PassiveAbility script = (PassiveAbility)target;

        //Construct a new list of every actionType's name
        List<string> typeNameList = new List<string>();
        foreach (var t in script.actionTypeList)
        {
            typeNameList.Add(t.Name);
        }

        //Styles
        GUIStyle typeInfoStyle = new GUIStyle();
        typeInfoStyle.normal.textColor = new Color32(190, 60, 60, 255);
        typeInfoStyle.fontStyle = FontStyle.Bold;
        //Display dropdown
        EditorGUILayout.LabelField("[ Action ]", typeInfoStyle);
        EditorGUILayout.BeginHorizontal();
        string typeInfo = script.action.GetType().ToString();
        EditorGUILayout.LabelField("<"+typeInfo.Substring(7)+">", typeInfoStyle, GUILayout.MaxWidth(100));
        script.actionTypeIndex = EditorGUILayout.Popup(script.actionTypeIndex, typeNameList.ToArray());
        //EditorGUILayout.Space();
        if (GUILayout.Button("Apply"))
        {
            script.EditorApplyConditionType();
        }
        //EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }*/

    public void DisplayDropdown(string labelInfo, Type currentType, Color infoColor, List<Type> typeList, ref int dropDownIndex, Action applyAction)
    {
        PassiveAbility script = (PassiveAbility)target;

        //Construct a new list of every actionType's name
        List<string> typeNameList = new List<string>();
        foreach (var t in typeList)
        {
            typeNameList.Add(t.Name);
        }

        //Styles
        GUIStyle typeInfoStyle = new GUIStyle();
        typeInfoStyle.normal.textColor = infoColor;
        typeInfoStyle.fontStyle = FontStyle.Bold;
        //Display dropdown
        EditorGUILayout.LabelField("[ "+labelInfo+" ]", typeInfoStyle);
        EditorGUILayout.BeginHorizontal();
        string[] subStrings = currentType.ToString().Split('_');
        EditorGUILayout.LabelField("<"+subStrings[1]+">", typeInfoStyle, GUILayout.MaxWidth(100));
        dropDownIndex = EditorGUILayout.Popup(dropDownIndex, typeNameList.ToArray());
        if (GUILayout.Button("Apply"))
        {
            applyAction();
        }
        EditorGUILayout.EndHorizontal();
    }

    
}
#endif
#endregion
