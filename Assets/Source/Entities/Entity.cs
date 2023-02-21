using UnityEngine;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityTypes Type { get; private set; }
    [field: SerializeField] public VegetableInventory Inventory { get; private set; }
}
