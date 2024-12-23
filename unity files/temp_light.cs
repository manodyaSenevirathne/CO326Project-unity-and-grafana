using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_light : MonoBehaviour
{
    private Light _light;
    private Color targetColor;
    private Coroutine colorTransitionCoroutine;

    // Temperature range for color mapping (in Kelvin)
    public float minTemperature = 1000f;
    public float maxTemperature = 10000f;

    // Duration of the color transition
    public float transitionDuration = 1f;

    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Method to set the color of the light based on temperature
    public void Update_Component(float temperature)
    {
        // Map temperature to a color temperature (or RGB color) based on your application's logic
        targetColor = CalculateColorFromTemperature(temperature);

        // Start the color transition
        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        colorTransitionCoroutine = StartCoroutine(SmoothTransitionToColor(targetColor));
    }

    // Example method to calculate color based on temperature
    private Color CalculateColorFromTemperature(float temperature)
    {
        // Example logic: linear interpolation between two colors based on temperature range
        float t = Mathf.InverseLerp(minTemperature, maxTemperature, temperature);
        Color coldColor = Color.blue;
        Color hotColor = Color.red;
        return Color.Lerp(coldColor, hotColor, t);
    }

    // Coroutine to smoothly transition the light color
    private IEnumerator SmoothTransitionToColor(Color newColor)
    {
        Color currentColor = _light.color;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            _light.color = Color.Lerp(currentColor, newColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _light.color = newColor;
    }
}
