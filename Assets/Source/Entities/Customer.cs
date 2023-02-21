using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Customer : Entity
{
    public enum CustomerStates
    {
        TakeVegetables = 0,
        PayForVegetables = 1,
        Leave = 2
    }

    [SerializeField] NavMeshAgent _meshAgent;
    [field: SerializeField] public CustomerStates CurrentState { get; private set; }
    [field: SerializeField] public Box Box { get; private set; }
    [SerializeField] Transform _boxParent;
    public UnityEvent<int, VegetableSettings> OnTaskReceived;
    public UnityEvent OnRequiredQuantityReached;
    public event Action<Customer> OnControlTransferred;
    public UnityEvent OnTaskCompleted;
    public int RequiredQuantity { get; private set; }
    public VegetableSettings TargetVegetable { get; private set; }
    Vector3 _shelfPosition;
    Vector3 _exitPosition;

    public void ReturnBoxToParent()
    {
        Box.ChangeableParent.SetParent(_boxParent);
    }

    public void SetDestination(Vector3 destination)
    {
        _meshAgent.SetDestination(destination);
    }

    public float GetRemainingDistance()
    {
        return _meshAgent.remainingDistance;
    }

    public void SetTask(Vector3 shelfPosition, Vector3 exitPosition, VegetableSettings targetVegetable, int requiredQuantity)
    {
        _shelfPosition = shelfPosition;
        _exitPosition = exitPosition;
        TargetVegetable = targetVegetable;
        Inventory.SetTargetVegetableSettings(TargetVegetable);
        RequiredQuantity = Math.Min(requiredQuantity, Inventory.GetCapacity());
        Inventory.AllowTakingVegetables();
        SetDestination(_shelfPosition);
        OnTaskReceived.Invoke(RequiredQuantity, TargetVegetable);
        CurrentState = CustomerStates.TakeVegetables;
    }

    public void OnInventoryCountChanged(int vegetablesCount)
    {
        if (CurrentState == CustomerStates.TakeVegetables)
        {
            if (vegetablesCount >= RequiredQuantity)
            {
                Inventory.ForbidTakingVegetables();
                OnControlTransferred?.Invoke(this);
                CurrentState = CustomerStates.PayForVegetables;
                OnRequiredQuantityReached.Invoke();
            }
        }
    }

    public void OnMoneyPaid()
    {
        if (CurrentState == CustomerStates.PayForVegetables)
        {
            _meshAgent.SetDestination(_exitPosition);
            CurrentState = CustomerStates.Leave;
            OnTaskCompleted.Invoke();
        }
    }
}
