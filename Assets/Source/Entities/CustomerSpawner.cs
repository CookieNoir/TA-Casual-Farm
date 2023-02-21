using UnityEngine;
using UnityEngine.Pool;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _customerPrefab;
    [SerializeField, Min(1)] int _maxCustomers;
    [SerializeField, Min(0f)] float _spawnCooldown;
    [Space(10)]
    [SerializeField] CashRegister _cashRegister;
    [SerializeField] ShelfHandler _shelfHandler;
    [SerializeField] VegetableRegistry _vegetableRegistry;
    [SerializeField] VegetablePool _vegetablePool;
    [SerializeField] Transform _entryPoint;
    ObjectPool<Customer> _customersPool;
    float _remainingTime = 0f;

    private void Awake()
    {
        _Prepare();
    }

    private void _Prepare()
    {
        _customersPool = new ObjectPool<Customer>(
            createFunc: _Create,
            actionOnGet: _Show,
            actionOnRelease: _Hide,
            maxSize: _maxCustomers);
    }

    private Customer _Create()
    {
        GameObject newInstance = Instantiate(_customerPrefab);
        Customer customer = newInstance.GetComponent<Customer>();
        _cashRegister.SubscribeOnCustomer(customer);
        return customer;
    }

    public void Release(Customer customer)
    {
        if (customer.CurrentState == Customer.CustomerStates.Leave)
        {
            _customersPool.Release(customer);
            enabled = true;
        }
    }

    private void _Show(Customer customer)
    {
        customer.gameObject.SetActive(true);
        _SetTaskForCustomer(customer);
    }

    private void _Hide(Customer customer)
    {
        customer.gameObject.SetActive(false);
        VegetableInventory inventory = customer.Box.VegetableInventory;
        while (inventory.HasItems())
        {
            Vegetable vegetable = inventory.TakeVegetable();
            _vegetablePool.ReleaseVegetable(vegetable);
        }
        customer.Box.SetActive(false);
    }

    private void _SetTaskForCustomer(Customer customer)
    {
        customer.transform.position = _entryPoint.position;
        int requiredQuantity = 1 + Random.Range(0, customer.Inventory.GetCapacity());
        VegetableSettings vegetableSettings = _vegetableRegistry.GetRandomVegetableSettings();
        Vector3 shelfPosition = _shelfHandler.GetShelfWithType(vegetableSettings)
            .GetRandomPositionNearShelf();
        customer.SetTask(shelfPosition, _entryPoint.position, vegetableSettings, requiredQuantity);
    }

    private void _TryToSpawnCustomer(float timestep)
    {
        _remainingTime -= timestep;
        if (_remainingTime <= 0f)
        {
            if (_customersPool.CountActive < _maxCustomers)
            {
                Customer customer = _customersPool.Get();
                _remainingTime = _spawnCooldown;
            }
            else
            {
                enabled = false;
            }
        }
    }

    private void Update()
    {
        _TryToSpawnCustomer(Time.deltaTime);
    }
}
