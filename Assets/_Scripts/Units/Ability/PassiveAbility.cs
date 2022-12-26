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
    private List<Trigger_Base> triggerList()
    {
        return new List<Trigger_Base>()
        {
            new Trigger_TurnEvent(),
            new Trigger_UnitEvent()
        };
    }

    [TabGroup("Condition")]
    [Title("Condition", "⏑", TitleAlignments.Centered)]
    [ValueDropdown("conditionList")]
    [SerializeReference] public List<Condition_Base> conditions = new List<Condition_Base>();
    private List<Condition_Base> conditionList()
    {
        return new List<Condition_Base>()
        {
            new Condition_TargetStat(),
            new Condition_SelfTargetStat(),
        };
    }

    [TabGroup("Action")]
    [Title("Action", "⏑", TitleAlignments.Centered)]
    [ValueDropdown("actionList")]
    [SerializeReference] public List<Action_Base> actions = new List<Action_Base>();
    private List<Action_Base> actionList()
    {
        return new List<Action_Base>()
        {
            new Action_StatMod(),
            new Action_DamageAttack(),
            new Action_StatusEffect()
        };
    }

    [TabGroup("Limit")]
    [HideLabel]
    [Title("Limit", "⏑", TitleAlignments.Centered)]
    public Limit_Base limit = new Limit_Base();

    public void UninitializeAbility(Unit selfUnit)
    {
        //trigger.UnregisterTrigger(selfUnit, OnTriggerAbility);
    }

    public virtual void InitializeAbility(Unit selfUnit)
    {
        //trigger.RegisterTrigger(selfUnit, OnTriggerAbility);
    }

    public virtual void OnTriggerAbility(Unit selfUnit)//sub by trigger
    {
        //if (unit.unitBattle.IsDead())
            //return;

        bool conditionMet = true;
        foreach (var c in conditions)
        {
            if (!c.ConditionMet(selfUnit))
                conditionMet = false;
        }

        if (conditionMet)
        {
            PerformAbility(selfUnit);
        }
    }

    public virtual void PerformAbility(Unit selfUnit)//called by OnTriggerAbility
    {
        Debug.Log("Perform ability");
        new CCUnitStartAbility(selfUnit.unitCID).AddToQueue();
        foreach (var a in actions)
        {
            a.DoAction(selfUnit);
        }

        if(!selfUnit.unitBattle.IsDead())
            new CCUnitEndAbility(selfUnit.unitCID).AddToQueue();
    }

}
