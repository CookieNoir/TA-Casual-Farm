using UnityEngine;
using UnityEngine.Events;

public class BuySpot : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField, Min(0)] int _cost;
    public UnityEvent<int> OnCostChanged;
    public UnityEvent OnBought;
    public UnityEvent OnNotEnoughMoney;

    private void Awake()
    {
        OnCostChanged.Invoke(_cost);
    }

    public void SetCost(int newCost)
    {
        _cost = newCost;
        OnCostChanged.Invoke(_cost);
    }

    private void _TryToBuy()
    {
        if (_player.Account.TryToTakeMoney(_cost))
        {
            OnBought.Invoke();
        }
        else
        {
            OnNotEnoughMoney.Invoke();
        }
    }

    private void OnMouseDown()
    {
        _TryToBuy();
    }

    private void OnValidate()
    {
        if (OnCostChanged != null) OnCostChanged.Invoke(_cost);
    }
}
