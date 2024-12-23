using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_intensity_control : MonoBehaviour
{
    private Light _light;
    private float _targetIntensity;
    private Coroutine intensityTransitionCoroutine;

    // Duration of the intensity transition
    public float transitionDuration = 1f;

    void Start()
    {
        _light = GetComponent<Light>();
        _targetIntensity = _light.intensity; // Initialize with current intensity
    }

    void Update()
    {
        // Ensure the intensity is updated smoothly in case of direct changes
        if (_light.intensity != _targetIntensity)
        {
            _light.intensity = Mathf.Lerp(_light.intensity, _targetIntensity, Time.deltaTime / transitionDuration);
        }
    }

    // Method to set the intensity of the light externally
    public void Update_Component(float intensity)
    {
        _targetIntensity = intensity / 200;

        // Start the intensity transition
        if (intensityTransitionCoroutine != null)
        {
            StopCoroutine(intensityTransitionCoroutine);
        }
        intensityTransitionCoroutine = StartCoroutine(SmoothTransitionToIntensity(_targetIntensity));
    }

    // Coroutine to smoothly transition the light intensity
    private IEnumerator SmoothTransitionToIntensity(float newIntensity)
    {
        float currentIntensity = _light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            _light.intensity = Mathf.Lerp(currentIntensity, newIntensity, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _light.intensity = newIntensity;
    }
}
