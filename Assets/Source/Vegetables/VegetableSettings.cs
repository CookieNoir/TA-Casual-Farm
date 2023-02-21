using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(
    fileName = "Vegetable", 
    menuName = "Scriptable Objects/Vegetable", 
    order = 1)]
public class VegetableSettings : ScriptableObject
{
    [field: SerializeField, Min(0)] public int PricePerUnit { get; private set; } = 0;
    [field: SerializeField, Min(0f)] public float GrowthTime { get; private set; } = 1f;
    [field: SerializeField] public Sprite Icon { get; private set; }
}
