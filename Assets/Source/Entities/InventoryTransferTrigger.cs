using UnityEngine;

public class InventoryTransferTrigger : MonoBehaviour
{
    [SerializeField] VegetableInventory _vegetableInventory;
    [SerializeField] EntityTypes _contributorType;
    [SerializeField] EntityTypes _customerType;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            if (entity.Type == _contributorType)
            {
                VegetableInventory.TransferVegetables(entity.Inventory, _vegetableInventory);
            }
            if (entity.Type == _customerType)
            {
                VegetableInventory.TransferVegetables(_vegetableInventory, entity.Inventory);
            }
        }
    }

    
}
