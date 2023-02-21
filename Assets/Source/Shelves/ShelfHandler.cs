using System.Collections.Generic;
using UnityEngine;

public class ShelfHandler : MonoBehaviour
{
    [SerializeField] List<Shelf> _shelves;

    public void AddShelf(Shelf shelf)
    {
        _shelves.Add(shelf);
    }

    public void RemoveShelf(Shelf shelf) 
    {
        _shelves.Remove(shelf);
    }

    public Shelf GetShelfWithType(VegetableSettings vegetableSettings)
    {
        return _shelves.Find(x => x.Inventory.TargetVegetableSettings == vegetableSettings);
    }
}
