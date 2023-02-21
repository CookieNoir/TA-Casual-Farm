using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VegetablePool : MonoBehaviour
{
    [Serializable]
    public class PoolData
    {
        public VegetableSettings vegetable;
        public GameObject prefab;
        [Min(0)] public int startCapacity;
    }

    [SerializeField] PoolData[] _vegetablePrefabs;
    [SerializeField] Transform _vegetablesParent;
    Dictionary<VegetableSettings, ObjectPool<Vegetable>> _pools = new();

    private void Awake()
    {
        _CreatePools();
    }

    private void _CreatePools()
    {
        foreach (PoolData pool in _vegetablePrefabs)
        {
            _pools[pool.vegetable] = new ObjectPool<Vegetable>(
                createFunc: () => _Create(pool),
                actionOnGet: _Show,
                actionOnRelease: _Hide,
                defaultCapacity: pool.startCapacity
                );
        }
    }

    public Vegetable GetVegetable(VegetableSettings vegetableSettings)
    {
        return _pools[vegetableSettings].Get();
    }

    public void ReleaseVegetable(Vegetable vegetable)
    {
        _pools[vegetable.VegetableSettings].Release(vegetable);
    }

    private Vegetable _Create(PoolData pool)
    {
        GameObject newInstance = Instantiate(pool.prefab, _vegetablesParent);
        Vegetable vegetable = newInstance.GetComponent<Vegetable>();
        return vegetable;
    }

    private void _Show(Vegetable vegetable)
    {
        vegetable.SetRipeness(0f);
        vegetable.gameObject.SetActive(true);
    }

    private void _Hide(Vegetable vegetable)
    {
        vegetable.gameObject.SetActive(false);
        vegetable.transform.parent = _vegetablesParent;
    }
}
