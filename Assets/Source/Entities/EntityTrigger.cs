using UnityEngine;
using UnityEngine.Events;

public class EntityTrigger : MonoBehaviour
{
    [SerializeField] EntityTypes _targetType;
    public UnityEvent OnEntityEnteredTrigger;
    public UnityEvent OnEntityExitedTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            if (entity.Type == _targetType) OnEntityEnteredTrigger.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            if (entity.Type == _targetType) OnEntityExitedTrigger.Invoke();
        }
    }
}
