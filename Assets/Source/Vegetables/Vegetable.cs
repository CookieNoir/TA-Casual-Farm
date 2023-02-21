using UnityEngine;
using UnityEngine.Events;

public class Vegetable : MonoBehaviour
{
    [field: SerializeField] public ChangeableParent ChangeableParent { get; private set; }
    [field: SerializeField] public VegetableSettings VegetableSettings { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float Ripeness { get; private set; }
    public UnityEvent<float> OnRipenessChanged;

    public void SetRipeness(float value)
    {
        value = Mathf.Clamp01(value);
        Ripeness = value;
        OnRipenessChanged.Invoke(Ripeness);
    }

    private void OnValidate()
    {
        if (OnRipenessChanged != null) OnRipenessChanged.Invoke(Ripeness);
    }
}
