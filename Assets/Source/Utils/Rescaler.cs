using UnityEngine;

public class Rescaler : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] Vector3 _baseScale;

    public void Rescale(float newScale)
    {
        _targetTransform.localScale = newScale * _baseScale;
    }
}
