using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDatabase", menuName = "Biobox/StatusEffectDatabase", order = 1)]
public class StatusEffectDatabase : ScriptableObject
{
    public StatusEffectDefinition starved;
    public StatusEffectDefinition exhausted;
}
