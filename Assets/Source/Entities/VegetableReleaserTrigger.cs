using UnityEngine;

public class VegetableReleaserTrigger : MonoBehaviour
{
    [SerializeField] VegetablePool _vegetablePool;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            while (entity.Inventory.HasItems())
            {
                Vegetable vegetable = entity.Inventory.TakeVegetable();
                _vegetablePool.ReleaseVegetable(vegetable);
            }
        }
    }
}
