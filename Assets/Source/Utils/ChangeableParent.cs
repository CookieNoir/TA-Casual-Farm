using UnityEngine;
using UnityEngine.Events;

public class ChangeableParent: MonoBehaviour
{
    [SerializeField] Transform _transform;
    public UnityEvent OnParentChanged;

    public Transform GetTransform()
    {
        return _transform;
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }

    public void SetParent(Transform targetTransform, bool sendCall = true)
    {
        _transform.parent = targetTransform;
        if (sendCall) OnParentChanged.Invoke();
    }

    public void SetLocalPosition(Vector3 localPosition)
    {
        _transform.localPosition = localPosition;
    }
}
