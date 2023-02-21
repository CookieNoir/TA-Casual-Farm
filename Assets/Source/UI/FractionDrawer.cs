using UnityEngine;
using TMPro;

public class FractionDrawer : MonoBehaviour
{
    [SerializeField] TMP_Text _textField;
    [SerializeField] string _separatorText;
    [SerializeField] int _numeratorValue;
    [SerializeField] int _denominatorValue;

    public void SetNumeratorAndDenominator(int numerator, int denominator)
    {
        _numeratorValue = numerator;
        _denominatorValue = denominator;
        _RefreshText();
    }

    public void SetNumerator(int value)
    {
        _numeratorValue = value;
        _RefreshText();
    }

    public void SetDenominator(int value)
    {
        _denominatorValue = value;
        _RefreshText();
    }

    private void _RefreshText()
    {
        _textField.text = $"{_numeratorValue}{_separatorText}{_denominatorValue}";
    }

    private void OnValidate()
    {
        if (_textField) _RefreshText();
    }
}
