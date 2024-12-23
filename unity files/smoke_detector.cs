using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke_detector : MonoBehaviour
{
    private Light _light;
    private bool _isLightOn = false;

    void Start()
    {
        _light = GetComponent<Light>();
        UpdateLightState(); // Initialize light state
    }

    // Method to toggle the light on/off
    public void Update_Component(float state)
    {
         _isLightOn = state != 0; // Convert state to boolean
        UpdateLightState(); // Update the light state
    }

    // Method to update the light state based on _isLightOn
    private void UpdateLightState()
    {
        _light.enabled = _isLightOn; // Enable/disable the light based on _isLightOn
    }
}
