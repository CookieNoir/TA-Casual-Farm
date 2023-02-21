using System.Collections;
using UnityEngine;

public class ScaleAnimator : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] Vector3 _minScale;
    [SerializeField] Vector3 _maxScale;
    [SerializeField, Min(0.01f)] float _duration;

    public void ScaleToMax()
    {
        StopAllCoroutines();
        StartCoroutine(_ChangeScale(_maxScale));
    }

    public void ScaleToMin()
    {
        StopAllCoroutines();
        StartCoroutine(_ChangeScale(_minScale));
    }

    private IEnumerator _ChangeScale(Vector3 targetScale)
    {
        Vector3 startScale = _targetTransform.localScale;
        float timeSpent = 0f;
        while (timeSpent < _duration)
        {
            _targetTransform.localScale = Vector3.Lerp(startScale, targetScale, timeSpent / _duration);
            yield return null;
            timeSpent += Time.deltaTime;
        }
        _targetTransform.localScale = targetScale;
    }

    private IEnumerator _ChangeScaleTwice(Vector3 firstTarget, Vector3 secondTarget)
    {
        yield return StartCoroutine(_ChangeScale(firstTarget));
        StartCoroutine(_ChangeScale(secondTarget));
    }

    public void ScaleToMaxAndToMin()
    {
        StopAllCoroutines();
        StartCoroutine(_ChangeScaleTwice(_maxScale, _minScale));
    }
}
