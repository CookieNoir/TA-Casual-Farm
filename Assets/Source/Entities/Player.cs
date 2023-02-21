using UnityEngine;

public class Player : Entity
{
    [field: SerializeField] public MoneyAccount Account { get; private set; }
}
