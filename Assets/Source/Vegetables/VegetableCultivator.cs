using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(VegetableInventory))]
public class VegetableCultivator : MonoBehaviour
{
    [SerializeField] VegetableSettings _targetVegetable;
    [SerializeField] VegetableInventory _vegetableInventory;
    [SerializeField] VegetablePool _vegetablePool;
    Queue<Transform> _freePositions;
    Vegetable _cultivatableVegetable = null;
    Transform _cultivatableTransform = null; 
    Dictionary<Vegetable, Transform> _grownVegetables;
    float _timeSpent = 0f;

    public void Construct(VegetablePool vegetablePool)
    {
        _vegetablePool = vegetablePool;
    }

    private void Awake()
    {
        _Prepare();
    }

    private void Start()
    {
        _TryToStartGrowing();
    }

    private void _Prepare()
    {
        int maxVegetables = _vegetableInventory.SlotPositions.Length;
        _freePositions = new(capacity: maxVegetables);
        foreach (Transform transform in _vegetableInventory.SlotPositions)
        {
            _freePositions.Enqueue(transform);
        }
        _grownVegetables = new(capacity: maxVegetables);
    }

    public void GrowForTime(float timestep)
    {
        if (_cultivatableVegetable == null) return;
        _timeSpent += timestep;
        float ripeness = _timeSpent / _targetVegetable.GrowthTime;
        _cultivatableVegetable.SetRipeness(ripeness);
        if (ripeness >= 1f)
        {
            _OnVegetableGrown();
        }
    }

    private void _OnVegetableGrown()
    {
        _grownVegetables[_cultivatableVegetable] = _cultivatableTransform;
        _vegetableInventory.AddVegetableToSlot(_cultivatableVegetable, _cultivatableTransform);
        _cultivatableVegetable = null;
        _cultivatableTransform = null;
        _TryToStartGrowing();
    }

    private void _TryToStartGrowing()
    {
        if (_cultivatableVegetable != null) return;
        if (_freePositions.TryDequeue(out Transform newPosition))
        {
            Vegetable vegetable = _vegetablePool.GetVegetable(_targetVegetable);
            vegetable.ChangeableParent.SetParent(newPosition, false);
            vegetable.ChangeableParent.SetLocalPosition(Vector3.zero);
            _cultivatableVegetable = vegetable;
            _cultivatableTransform = newPosition;
            _timeSpent = 0f;
        }
    }

    public void OnVegetableTaken(Vegetable vegetable)
    {
        if (_grownVegetables.TryGetValue(vegetable, out Transform position))
        {
            _grownVegetables.Remove(vegetable);
            _freePositions.Enqueue(position);
            _TryToStartGrowing();
        }
    }
}
