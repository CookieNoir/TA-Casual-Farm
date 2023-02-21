using System;
using UnityEngine;
using UnityEngine.Events;

public class VegetableInventory : MonoBehaviour
{
    [field: SerializeField] public VegetableSettings TargetVegetableSettings { get; private set; }
    [field: SerializeField] public Transform[] SlotPositions { get; private set; }
    [field: SerializeField] public bool CanTakeVegetables { get; private set; }
    [field: SerializeField, Min(0f)] public float AddingCooldown { get; private set; }
    Vegetable[] _vegetables;
    int _addedCount = 0;
    public UnityEvent OnMaxQuantityReached;
    public UnityEvent OnVegetableAdded;
    public UnityEvent<int> OnVegetablesCountChanged;
    public UnityEvent<Vegetable> OnVegetableTaken;
    public UnityEvent OnInventoryEmptied;
    float _remainingTime;

    public void AllowTakingVegetables()
    {
        CanTakeVegetables = true;
    }

    public void ForbidTakingVegetables()
    {
        CanTakeVegetables = false;
    }

    public int GetCapacity()
    {
        return _vegetables.Length;
    }

    public void SetTargetVegetableSettings(VegetableSettings vegetableSettings)
    {
        TargetVegetableSettings = vegetableSettings;
    }

    public bool HasItems()
    {
        return _addedCount > 0;
    }

    public bool HasItemsOfType(VegetableSettings vegetableSettings)
    {
        return HasItems() && (_FindVegetableOfType(vegetableSettings) >= 0);
    }

    public bool HasFreeSpace()
    {
        return _addedCount < _vegetables.Length;
    }

    public bool HasNoCooldown()
    {
        return _remainingTime <= 0f;
    }

    public bool CanAddItem(Vegetable vegetable)
    {
        return CanTakeVegetables
            && vegetable
            && HasFreeSpace()
            && HasNoCooldown()
            && (!TargetVegetableSettings || vegetable.VegetableSettings == TargetVegetableSettings);
    }

    private void Awake()
    {
        _Prepare();
    }

    private void _Prepare()
    {
        _vegetables = new Vegetable[SlotPositions.Length];
        _addedCount = 0;
    }

    public void AddVegetable(Vegetable vegetable)
    {
        if (!CanAddItem(vegetable)) return;
        int freeSlot = _FindFreeSlot();
        _AddVegetableToSlot(vegetable, freeSlot);
    }

    private int _FindFreeSlot()
    {
        int result = -1;
        if (_vegetables[_addedCount])
        {
            for (int i = 0; i < _addedCount; ++i)
            {
                if (!_vegetables[i])
                {
                    result = i;
                    break;
                }
            }
        }
        else
        {
            result = _addedCount;
        }
        return result;
    }

    private void _AddVegetableToSlot(Vegetable vegetable, int freeSlot)
    {
        vegetable.ChangeableParent.SetParent(SlotPositions[freeSlot]);
        _vegetables[freeSlot] = vegetable;
        _addedCount++;
        OnVegetableAdded.Invoke();
        OnVegetablesCountChanged.Invoke(_addedCount);
        _remainingTime = AddingCooldown;
        enabled = true;
        if (!HasFreeSpace()) OnMaxQuantityReached.Invoke();
    }

    public void AddVegetableToSlot(Vegetable vegetable, Transform slot)
    {
        if (!CanAddItem(vegetable)) return;
        int index = Array.IndexOf(SlotPositions, slot);
        if (index < 0) return;
        _AddVegetableToSlot(vegetable, index);
    }

    private int _FindAnyVegetable()
    {
        int result = -1;
        for (int i = _vegetables.Length - 1; i >= 0; --i)
        {
            if (_vegetables[i])
            {
                result = i;
                break;
            }
        }
        return result;
    }

    private int _FindVegetableOfType(VegetableSettings vegetableSettings)
    {
        int result = -1;
        for (int i = _vegetables.Length - 1; i >= 0; --i)
        {
            if (_vegetables[i] && _vegetables[i].VegetableSettings == vegetableSettings)
            {
                result = i;
                break;
            }
        }
        return result;
    }

    private Vegetable _TakeVegetableFromSlot(int slot)
    {
        Vegetable result = _vegetables[slot];
        result.ChangeableParent.SetParent(null, false);
        _vegetables[slot] = null;
        _addedCount--;
        OnVegetableTaken.Invoke(result);
        OnVegetablesCountChanged.Invoke(_addedCount);
        if (!HasItems()) OnInventoryEmptied.Invoke();
        return result;
    }

    public Vegetable TakeVegetable()
    {
        Vegetable result = null;
        if (HasItems())
        {
            int slot = _FindAnyVegetable();
            if (slot >= 0) result = _TakeVegetableFromSlot(slot);
        }
        return result;
    }

    public Vegetable TakeVegetableOfType(VegetableSettings vegetableSettings)
    {
        Vegetable result = null;
        if (vegetableSettings)
        {
            if (HasItems())
            {
                int slot;
                if (TargetVegetableSettings)
                {
                    if (TargetVegetableSettings == vegetableSettings)
                    {
                        slot = _FindAnyVegetable();
                        result = _TakeVegetableFromSlot(slot);
                    }
                }
                else
                {
                    slot = _FindVegetableOfType(vegetableSettings);
                    if (slot >= 0) result = _TakeVegetableFromSlot(slot);
                }
            }
        }
        else result = TakeVegetable();
        return result;
    }

    private void _ChangeRemainingTime(float timestep)
    {
        _remainingTime -= timestep;
        if (HasNoCooldown()) enabled = false;
    }

    private void Update()
    {
        _ChangeRemainingTime(Time.deltaTime);
    }

    public static void TransferVegetables(VegetableInventory fromInventory, VegetableInventory toInventory)
    {
        if (toInventory.CanTakeVegetables && toInventory.HasNoCooldown() && toInventory.HasFreeSpace())
        {
            if (toInventory.TargetVegetableSettings)
            {
                if (fromInventory.HasItemsOfType(toInventory.TargetVegetableSettings))
                {
                    Vegetable vegetable = fromInventory.TakeVegetableOfType(toInventory.TargetVegetableSettings);
                    toInventory.AddVegetable(vegetable);
                }
            }
            else
            {
                if (fromInventory.HasItems())
                {
                    Vegetable vegetable = fromInventory.TakeVegetable();
                    toInventory.AddVegetable(vegetable);
                }
            }
        }
    }
}
