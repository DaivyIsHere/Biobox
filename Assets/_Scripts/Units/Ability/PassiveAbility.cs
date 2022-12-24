using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewPassiveAbility", menuName = "Biobox/Ability/PassiveAbility", order = 1)]
public class PassiveAbility : ScriptableAbility
{
    [TabGroup("Trigger")]
    [HideLabel]
    [Title("Trigger", "⏑", TitleAlignments.Centered)]
    [ValueDropdown("triggerList")]
    [SerializeReference] public Trigger_Base trigger = new Trigger_Base();
    private List<Trigger_Base> triggerList = new List<Trigger_Base>
    {
        new Trigger_BattleEvent()
    };

    [TabGroup("Condition")]
    [Title("Condition", "⏑", TitleAlignments.Centered)]
    [ValueDropdown("conditionList")]
    [SerializeReference] public List<Condition_Base> conditions = new List<Condition_Base>();
    private List<Condition_Base> conditionList = new List<Condition_Base>
    {
        new Condition_TargetStat(),
        new Condition_SelfTargetStat()
    };

    [TabGroup("Action")]
    [Title("Action", "⏑", TitleAlignments.Centered)]
    [ValueDropdown("actionList")]
    [SerializeReference] public List<Action_Base> actions = new List<Action_Base>();
    private List<Action_Base> actionList = new List<Action_Base>
    {
        new Action_StatMod(),
        new Action_StatusEffect()
    };

    [TabGroup("Limit")]
    [HideLabel]
    [Title("Limit", "⏑", TitleAlignments.Centered)]
    public Limit_Base limit = new Limit_Base();

    private void UninitializeAbility() 
    {

    }

    public virtual void InitializeAbiltiy(Unit unit)
    {
        trigger.RegisterTrigger(unit, OnTriggerAbility);
    }

    public virtual void OnTriggerAbility(Unit unit)//sub by trigger
    {
        if (unit.unitBattle.IsDead())
            return;

        bool conditionMet = true;
        foreach (var c in conditions)
        {
            if (!c.ConditionMet(unit))
                conditionMet = false;
        }

        if (conditionMet)
        {
            PerformAbility(unit);
        }
    }

    public virtual void PerformAbility(Unit unit)//called by OnTriggerAbility
    {
        Debug.Log("Perform ability");
        new CCUnitStartAbility(unit.unitCID).AddToQueue();
        foreach (var a in actions)
        {
            a.DoAction(unit);
        }
        new CCUnitEndAbility(unit.unitCID).AddToQueue();
    }

}
