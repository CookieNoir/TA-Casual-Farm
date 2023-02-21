using UnityEngine;

public class Box : MonoBehaviour
{
    [field: SerializeField] public ChangeableParent ChangeableParent { get; private set; }
    [field: SerializeField] public VegetableInventory VegetableInventory { get; private set; }
    [field: SerializeField] public TranslationAnimator TranslationAnimator { get; private set; }

    public void SetActive(bool value)
    {
        ChangeableParent.GetTransform().gameObject.SetActive(value);
    }
}
