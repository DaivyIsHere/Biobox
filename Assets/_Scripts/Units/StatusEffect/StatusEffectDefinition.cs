using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffectDefinition", menuName = "Biobox/StatusEffectDefinition", order = 1)]
public class StatusEffectDefinition : ScriptableObject
{
    //using the reference of this scriptableObject as enums
    public bool isStackable;
}
