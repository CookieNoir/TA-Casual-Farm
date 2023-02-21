using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField, Min(0f)] float _distanceForService;
    [Space]
    [SerializeField] Transform _queueFirstPosition;
    [SerializeField] Transform _queueSecondPosition;
    [Space]
    [SerializeField] Transform _boxPosition;
    Queue<Customer> _customersQueue = new();
    Vector3 _startPosition;
    Vector3 _offset;
    Customer _currentCustomer;

    private void Awake()
    {
        _Prepare();
    }

    private void _Prepare()
    {
        _startPosition = _queueFirstPosition.position;
        _offset = _queueSecondPosition.position - _startPosition;
    }

    public void SubscribeOnCustomer(Customer customer)
    {
        customer.OnControlTransferred += EnqueueCustomer;
    }

    private void _UnsubscribeFromCustomer(Customer customer)
    {
        customer.OnControlTransferred -= EnqueueCustomer;
    }

    public void EnqueueCustomer(Customer customer)
    {
        _customersQueue.Enqueue(customer);
        _MoveCustomers();
    }

    private void _MoveCustomers()
    {
        int i = 0;
        foreach (Customer customer in _customersQueue)
        {
            customer.SetDestination(_startPosition + i * _offset);
            i++;
        }
    }

    private void _TryToServeCustomer()
    {
        if (_customersQueue.TryPeek(out Customer customer))
        {
            if (customer.GetRemainingDistance() <= _distanceForService)
            {
                _currentCustomer = customer;
                StartCoroutine(_Serve());
            }
        }
    }

    private IEnumerator _Serve()
    {
        _currentCustomer.Box.SetActive(true);
        _currentCustomer.Box.ChangeableParent.SetParent(_boxPosition);
        float animationDuration = _currentCustomer.Box.TranslationAnimator
            .DefaultAnimationSettings.AnimationDuration;
        yield return new WaitForSeconds(animationDuration);
        while (_currentCustomer.Inventory.HasItems())
        {
            VegetableInventory.TransferVegetables(_currentCustomer.Inventory, _currentCustomer.Box.VegetableInventory);
            yield return new WaitForSeconds(_currentCustomer.Box.VegetableInventory.AddingCooldown);
        }
        _currentCustomer.ReturnBoxToParent();
        yield return new WaitForSeconds(animationDuration);
        int money = _currentCustomer.RequiredQuantity * _currentCustomer.TargetVegetable.PricePerUnit;
        _player.Account.AddMoney(money);
        _currentCustomer.OnMoneyPaid();
        _currentCustomer = null;
        _customersQueue.Dequeue();
        _MoveCustomers();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentCustomer) return;
        if (other.TryGetComponent(out Player player))
        {
            _TryToServeCustomer();
        }
    }
}
