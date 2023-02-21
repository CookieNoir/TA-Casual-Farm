using UnityEngine;
using UnityEngine.Events;

public class CustomerTrigger : MonoBehaviour
{
    public UnityEvent<Customer> OnCustomerEnteredTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            OnCustomerEnteredTrigger.Invoke(customer);
        }
    }
}
