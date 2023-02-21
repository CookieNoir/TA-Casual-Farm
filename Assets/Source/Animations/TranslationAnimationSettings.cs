using UnityEngine;
[CreateAssetMenu(fileName = "Translation Animation", menuName = "Scriptable Objects/Animation Settings/Translation")]
public class TranslationAnimationSettings : ScriptableObject
{
    [field: SerializeField] public Vector3 ExpectedLocalPosition { get; private set; }
    [field: SerializeField] public AnimationCurve FactorValueAnimationCurve { get; private set; }
    [field: SerializeField, Min(0.01f)] public float AnimationDuration { get; private set; }
    [field: SerializeField, Space(10)] public Vector3 MaxOffset { get; private set; }
    [field: SerializeField] public AnimationCurve OffsetScaleAnimationCurve { get; private set; }
}
