using UnityEngine;
using UnityEngine.Rendering.Universal.Glitch;

internal sealed class GlitchController : MonoBehaviour
{
    [SerializeField] private DigitalGlitchFeature digitalGlitchFeature = default;
    [SerializeField] private AnalogGlitchFeature analogGlitchFeature = default;

    [Header("Digital")]
    [SerializeField, Range(0f, 1f)] private float intensity = default;

    [Header("Analog")]
    [SerializeField, Range(0f, 1f)] private float scanLineJitter = default;
    [SerializeField, Range(0f, 1f)] private float verticalJump = default;
    [SerializeField, Range(0f, 1f)] private float horizontalShake = default;
    [SerializeField, Range(0f, 1f)] private float colorDrift = default;

    private void Update()
    {
        digitalGlitchFeature.Intensity = intensity;

        analogGlitchFeature.ScanLineJitter = scanLineJitter;
        analogGlitchFeature.VerticalJump = verticalJump;
        analogGlitchFeature.HorizontalShake = horizontalShake;
        analogGlitchFeature.ColorDrift = colorDrift;
    }
}