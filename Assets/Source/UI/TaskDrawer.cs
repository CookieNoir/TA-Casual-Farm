using UnityEngine;

public class TaskDrawer : MonoBehaviour
{
    [SerializeField] FractionDrawer _fractionDrawer;
    [SerializeField] VegetableDrawer _vegetableDrawer;

    public void Draw(int requiredQuantity, VegetableSettings vegetableSettings)
    {
        _fractionDrawer.SetNumeratorAndDenominator(0, requiredQuantity);
        _vegetableDrawer.SetImageFromVegetable(vegetableSettings);
    }
}
