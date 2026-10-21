using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.5f;
    [SerializeField, Range(0f, 3f)] private float maxIntensity = 1.5f;

    [Header("Flicker")]
    [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;
    [SerializeField, Range(0.01f, 2f)] private float intensitySmoothTime = 0.15f;

    private Light lightToFlicker;
    private float currentTimer;
    private float targetIntensity;
    private float intensityVelocity;

    private void Awake()
    {
        lightToFlicker = GetComponent<Light>();

        OnValidate();

        targetIntensity = lightToFlicker.intensity;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= timeBetweenIntensity)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            currentTimer = 0f;
        }

        lightToFlicker.intensity = Mathf.SmoothDamp(
            lightToFlicker.intensity,
            targetIntensity,
            ref intensityVelocity,
            intensitySmoothTime
        );
    }

    private void OnValidate()
    {
        ValidateIntensityBounds();
        ValidateFlickerTiming();
    }

    private void ValidateIntensityBounds()
    {
        if (minIntensity <= maxIntensity)
        {
            return;
        }

        Debug.LogWarning("Min Intensity is greater than max intensity. Swapping values!");

        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }

    private void ValidateFlickerTiming()
    {
        if (timeBetweenIntensity > intensitySmoothTime)
        {
            return;
        }

        Debug.LogWarning(
            $"timeBetweenIntensity ({timeBetweenIntensity}) must be greater than " +
            $"intensitySmoothTime ({intensitySmoothTime}). " +
            "Setting timeBetweenIntensity to intensitySmoothTime + 0.05."
        );

        timeBetweenIntensity = intensitySmoothTime + 0.05f;
    }
}