using UnityEngine;
using UnityEngine.UI;

public class VegetableDrawer : MonoBehaviour
{
    [SerializeField] Image _image;

    public void SetImageFromVegetable(VegetableSettings vegetableSettings)
    {
        _image.sprite = vegetableSettings.Icon;
    }
}
