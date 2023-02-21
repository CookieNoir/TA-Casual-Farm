using System.Collections.Generic;
using UnityEngine;

public class VegetableRegistry : MonoBehaviour
{
    [SerializeField] List<VegetableSettings> _vegetableSettings;

    public void AddVegetableSettings(VegetableSettings vegetableSettings)
    {
        _vegetableSettings.Add(vegetableSettings);
    }

    public void RemoveVegetableSettings(VegetableSettings vegetableSettings)
    {
        _vegetableSettings.Remove(vegetableSettings);
    }

    public VegetableSettings GetRandomVegetableSettings()
    {
        int index = Random.Range(0, _vegetableSettings.Count);
        return _vegetableSettings[index];
    }
}
