using System.Collections;
using UnityEngine;

public class TranslationAnimator : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [field: SerializeField] public TranslationAnimationSettings DefaultAnimationSettings { get; private set; }
    IEnumerator _startedCoroutine = null;

    public void MoveToParent()
    {
        if (_startedCoroutine != null) StopCoroutine(_startedCoroutine);
        if (_targetTransform.parent && _targetTransform.parent.TryGetComponent(out TranslationAnimationHandler handler))
            _startedCoroutine = _Animate(handler.TranslationAnimationSettings);
        else 
        {
            if (DefaultAnimationSettings) _startedCoroutine = _Animate(DefaultAnimationSettings);
            else return;
        }
        StartCoroutine(_startedCoroutine);
    }

    private IEnumerator _Animate(TranslationAnimationSettings settings)
    {
        Vector3 startPosition = _targetTransform.position;
        float timeSpent = 0f;
        while (timeSpent < settings.AnimationDuration)
        {
            float factorValue = settings.FactorValueAnimationCurve.Evaluate(timeSpent / settings.AnimationDuration);
            Vector3 currentOffset = settings.OffsetScaleAnimationCurve.Evaluate(factorValue) * settings.MaxOffset;
            Vector3 expectedPosition = _targetTransform.parent.TransformPoint(settings.ExpectedLocalPosition) + currentOffset;
            _targetTransform.position = Vector3.Lerp(startPosition, expectedPosition, factorValue);
            yield return null;
            timeSpent += Time.deltaTime;
        }
        _targetTransform.localPosition = settings.ExpectedLocalPosition;
    }
}
