using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatDefinition", menuName = "Biobox/StatDefinition", order = 1)]
public class StatDefinition : ScriptableObject
{
    [SerializeField] private float _valueCap = -1;//-1 means there's no cap
    public float valueCap => _valueCap;
}
