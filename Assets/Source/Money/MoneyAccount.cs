using UnityEngine;
using UnityEngine.Events;

public class MoneyAccount : MonoBehaviour
{
    [field: SerializeField, Min(0)] public int Money { get; private set; }
    public UnityEvent<int> OnValueChanged;

    private void Awake()
    {
        OnValueChanged.Invoke(Money);
    }

    public void AddMoney(int value)
    {
        Money += value;
        OnValueChanged.Invoke(Money);
    }

    public bool TryToTakeMoney(int value)
    {
        if (Money < value) return false;
        Money -= value;
        OnValueChanged.Invoke(Money);
        return true;
    }

    private void OnValidate()
    {
        if (OnValueChanged != null) OnValueChanged.Invoke(Money);
    }
}
