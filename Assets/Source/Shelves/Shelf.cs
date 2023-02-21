using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [field: SerializeField] public VegetableInventory Inventory { get; private set; }
    [SerializeField] Transform[] _positionsNearShelf;

    public Vector3 GetRandomPositionNearShelf()
    {
        int index = Random.Range(0, _positionsNearShelf.Length);
        return _positionsNearShelf[index].position;
    }
}
