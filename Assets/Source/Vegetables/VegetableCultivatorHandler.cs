using System.Collections.Generic;
using UnityEngine;

public class VegetableCultivatorHandler : MonoBehaviour
{
    [SerializeField] List<VegetableCultivator> _vegetableCultivators = new();

    public void AddCultivator(VegetableCultivator vegetableCultivator)
    {
        _vegetableCultivators.Add(vegetableCultivator);
    }

    public void RemoveCultivator(VegetableCultivator vegetableCultivator)
    {
        _vegetableCultivators.Remove(vegetableCultivator);
    }

    private void _GrowForTime(float timestep)
    {
        foreach (var cultivator in _vegetableCultivators)
        {
            cultivator.GrowForTime(timestep);
        }
    }

    private void Update()
    {
        _GrowForTime(Time.deltaTime);
    }
}
