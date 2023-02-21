using UnityEngine;

public class FramerateLimiter : MonoBehaviour
{
    [SerializeField, Min(30)] int _value = 60;

    private void Awake()
    {
        Application.targetFrameRate = _value;
    }
}
