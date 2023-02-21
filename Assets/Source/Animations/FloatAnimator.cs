using System;
using UnityEngine;
using UnityEngine.Events;

public class FloatAnimator : MonoBehaviour
{
    [SerializeField] Vector2 _valueRange;
    [SerializeField, Min(0.01f)] float _animationTime;
    public UnityEvent<float> OnValueChanged;
    float _timeSpent = 0f;
    bool _toMax = false;
    Func<float, float> _factorFunc;

    private void Awake()
    {
        _ChangeDirection();
    }

    private void _ChangeDirection()
    {
        _timeSpent = 0f;
        _toMax = !_toMax;
        _factorFunc = _toMax ? _ToMaxFactor : _ToMinFactor;
    }

    private float _ToMaxFactor(float value)
    {
        return value;
    }

    private float _ToMinFactor(float value)
    {
        return 1f - value;
    }

    private void _AnimateFloat(float timestep)
    {
        _timeSpent += timestep;
        if (_timeSpent >= _animationTime)
        {
            _ChangeDirection();
        }
        float newValue = Mathf.Lerp(_valueRange.x, _valueRange.y, _factorFunc(_timeSpent / _animationTime));
        OnValueChanged.Invoke(newValue);
    }

    void Update()
    {
        _AnimateFloat(Time.deltaTime);
    }
}
