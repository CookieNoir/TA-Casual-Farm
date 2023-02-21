using UnityEngine;
using TMPro;

public class IntDrawer : MonoBehaviour
{
    [SerializeField] TMP_Text _textField;

    public void SetValue(int value)
    {
        _textField.text = value.ToString();
    }
}
